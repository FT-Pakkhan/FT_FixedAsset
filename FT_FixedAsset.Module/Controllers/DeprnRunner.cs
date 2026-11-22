using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace FT_FixedAsset.Module.Controllers
{
    /// <summary>
    /// Runs depreciation in the background so the screen is not blocked:
    /// the button returns at once and the progress window (DeprnProgress.aspx) shows
    /// done / total / errors while it runs and the summary when it ends.
    /// Each asset still goes through sp_FTS11_GenerateDeprn exactly as before.
    /// Progress, result and error lines are kept in FTS11ODRL / FTS11DRL1.
    /// </summary>
    public static class DeprnRunner
    {
        /// <summary>Opens the progress window for a run (set by the web module).</summary>
        public static Action<int> ShowProgress;

        /// <summary>Session key holding the company connection for the progress window.</summary>
        public const string SessionConnKey = "FT_DeprnConn";

        public sealed class Asset
        {
            public int FAKey;
            public string FACode;
        }

        /// <summary>
        /// Registers the run (one at a time; blocks postings into the period) and starts it in the background.
        /// Returns the run id, or 0 with <paramref name="blockMessage"/>.
        /// </summary>
        public static int Start(string connectionString, DateTime dateFrm, DateTime dateTo, string userCode, List<Asset> assets, out string blockMessage)
        {
            DeprnRunLock.EnsureTables(connectionString);
            int runId = DeprnRunLock.Start(connectionString, dateFrm, dateTo, userCode, assets.Count, out blockMessage);
            if (runId == 0) return 0;

            if (HostingEnvironment.IsHosted)
                // ASP.NET knows about it: on an app restart the token is signalled and the run is closed properly
                HostingEnvironment.QueueBackgroundWorkItem(ct => Run(connectionString, runId, dateFrm, dateTo, userCode, assets, ct));
            else
                Task.Run(() => Run(connectionString, runId, dateFrm, dateTo, userCode, assets, CancellationToken.None));
            return runId;
        }

        private static void Run(string cs, int runId, DateTime dateFrm, DateTime dateTo, string userCode, List<Asset> assets, CancellationToken ct)
        {
            int done = 0, errors = 0;
            bool ok = false;
            string note = null;
            var errorKeys = new HashSet<int>();
            var runKeys = new HashSet<int>();
            DateTime serverStart = DateTime.Now;
            try
            {
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var now = new SqlCommand("SELECT GETDATE()", conn)) serverStart = (DateTime)now.ExecuteScalar();

                    var beat = System.Diagnostics.Stopwatch.StartNew();
                    foreach (Asset a in assets)
                    {
                        if (ct.IsCancellationRequested) { note = "Stopped after " + done + " of " + assets.Count + " assets (web server restarting)"; break; }

                        runKeys.Add(a.FAKey);
                        string err = GenerateDeprnForAsset(conn, dateFrm, dateTo, a.FAKey, userCode);
                        if (err != null)
                        {
                            errors++;
                            errorKeys.Add(a.FAKey);
                            AddError(conn, runId, errors, a, err);
                        }
                        done++;

                        if (beat.ElapsedMilliseconds >= 1000)   // progress for the window + keeps the lock alive
                        {
                            beat.Restart();
                            if (Beat(conn, runId, done, errors)) { note = "Stopped by user after " + done + " of " + assets.Count + " assets"; break; }
                        }
                    }
                    ok = note == null;
                }
            }
            catch (Exception ex)
            {
                note = "Stopped after " + done + " of " + assets.Count + " assets: " + ex.Message;
            }
            finally
            {
                int generated = -1, skipped = -1;
                try
                {
                    var run = DeprnRunLock.GetRun(cs, runId);
                    if (run != null)
                    {
                        using (var conn = new SqlConnection(cs))
                        {
                            conn.Open();
                            var genKeys = GeneratedAssets(conn, serverStart, run.PrdFrom, run.PrdTo, userCode);
                            genKeys.IntersectWith(runKeys);
                            generated = genKeys.Count;
                            genKeys.UnionWith(errorKeys);
                            skipped = done - genKeys.Count;
                        }
                    }
                }
                catch { /* counts are informative only */ }
                DeprnRunLock.Finish(cs, runId, ok, done, generated, skipped, errors, note);
            }
        }

        // sp_FTS11_GenerateDeprn for one asset (same call as GenController.GenerateDeprn). Returns the error text, or null when OK.
        private static string GenerateDeprnForAsset(SqlConnection conn, DateTime dateFrm, DateTime dateTo, int faKey, string userCode)
        {
            try
            {
                using (var cmd = new SqlCommand("EXEC sp_FTS11_GenerateDeprn '" + dateFrm.ToString("yyyyMMdd") + "', '" + dateTo.ToString("yyyyMMdd") +
                                                "', '" + faKey + "', '" + userCode.Replace("'", "''") + "', ''", conn))
                {
                    cmd.CommandTimeout = 0;
                    string code = null, msg = null;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (r.Read())
                                if (r.FieldCount >= 2) { code = Convert.ToString(r[0]); msg = Convert.ToString(r[1]); }
                        } while (r.NextResult());
                    }
                    if (code != null && code != "0") return string.IsNullOrWhiteSpace(msg) ? "Error " + code : msg;
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // assets that got a new depreciation entry in this run (period, created by this user since the run started)
        private static HashSet<int> GeneratedAssets(SqlConnection conn, DateTime since, DateTime prdFrom, DateTime prdTo, string userCode)
        {
            var keys = new HashSet<int>();
            using (var cmd = new SqlCommand(
                "SELECT DISTINCT FAKey FROM FTS11JDT1 WHERE CreateDate >= @since AND CreateBy = @user AND AcctCat = 'ACCU' " +
                "AND TransType IN ('T9901', 'T9902') AND TransStatus <> 'C' AND DocDate BETWEEN @pf AND @pt", conn))
            {
                cmd.CommandTimeout = 300;
                cmd.Parameters.Add("@since", SqlDbType.DateTime).Value = since;
                // sp_FTS11_GenerateDeprn stores the user in an NVARCHAR(10) parameter
                cmd.Parameters.Add("@user", SqlDbType.NVarChar, 10).Value = userCode.Length > 10 ? userCode.Substring(0, 10) : userCode;
                cmd.Parameters.Add("@pf", SqlDbType.DateTime).Value = prdFrom;
                cmd.Parameters.Add("@pt", SqlDbType.DateTime).Value = prdTo;
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read()) keys.Add(r.GetInt32(0));
            }
            return keys;
        }

        private static void AddError(SqlConnection conn, int runId, int lineNum, Asset a, string message)
        {
            try
            {
                using (var cmd = new SqlCommand("INSERT INTO dbo.FTS11DRL1 (RunEntry, LineNum, FAKey, FACode, Message) VALUES (@r, @l, @k, @c, @m)", conn))
                {
                    cmd.Parameters.Add("@r", SqlDbType.Int).Value = runId;
                    cmd.Parameters.Add("@l", SqlDbType.Int).Value = lineNum;
                    cmd.Parameters.Add("@k", SqlDbType.Int).Value = a.FAKey;
                    cmd.Parameters.Add("@c", SqlDbType.NVarChar, 200).Value = (object)a.FACode ?? DBNull.Value;
                    cmd.Parameters.Add("@m", SqlDbType.NVarChar, 1000).Value = message.Length > 1000 ? message.Substring(0, 1000) : message;
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* the count is still kept in FTS11ODRL */ }
        }

        // progress heartbeat; returns true when the user pressed Stop
        private static bool Beat(SqlConnection conn, int runId, int done, int errors)
        {
            try
            {
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.FTS11ODRL SET Done = @d, ErrCnt = @e, LastBeat = GETDATE() WHERE DocEntry = @id AND Status = 'R'; " +
                    "SELECT StopReq FROM dbo.FTS11ODRL WHERE DocEntry = @id", conn))
                {
                    cmd.Parameters.Add("@d", SqlDbType.Int).Value = done;
                    cmd.Parameters.Add("@e", SqlDbType.Int).Value = errors;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                    object stop = cmd.ExecuteScalar();
                    return stop is bool b && b;
                }
            }
            catch { return false; }
        }

        #region progress window data
        public sealed class Status
        {
            public int Run, Total, Done, Generated, Skipped, Errors, ElapsedSec;
            public string State, Period, StartBy, Started, Note;
            public bool StopRequested;
            public List<string[]> ErrorLines = new List<string[]>();
        }

        /// <summary>The latest run (running or not) - the progress window opens on it.</summary>
        public static int ActiveRunId(string cs)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "IF OBJECT_ID('dbo.FTS11ODRL') IS NULL SELECT 0 ELSE " +
                    "SELECT ISNULL((SELECT TOP 1 DocEntry FROM dbo.FTS11ODRL WHERE Status = 'R' AND LastBeat >= DATEADD(MINUTE, -" + DeprnRunLock.StaleMinutes + ", GETDATE()) ORDER BY DocEntry DESC), 0)", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static Status GetStatus(string cs, int runId, int maxErrorLines)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var s = new Status { Run = runId };
                using (var cmd = new SqlCommand(
                    "SELECT Status, Total, Done, Generated, Skipped, ErrCnt, PrdFrom, PrdTo, StartBy, StartDate, " +
                    "DATEDIFF(SECOND, StartDate, CASE WHEN Status = 'R' THEN GETDATE() ELSE ISNULL(EndDate, LastBeat) END), Note, " +
                    "CASE WHEN COL_LENGTH('dbo.FTS11ODRL','StopReq') IS NULL THEN 0 ELSE 1 END " +
                    "FROM dbo.FTS11ODRL WHERE DocEntry = @id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return null;
                        string st = r.GetString(0);
                        // a run without progress for StaleMinutes is shown as interrupted
                        s.State = st == "R" ? "running" : st == "F" ? "finished" : st == "X" ? "interrupted" : "stopped";
                        s.Total = r.GetInt32(1); s.Done = r.GetInt32(2); s.Generated = r.GetInt32(3); s.Skipped = r.GetInt32(4); s.Errors = r.GetInt32(5);
                        s.Period = r.GetDateTime(6).ToString("dd/MM/yyyy") + " - " + r.GetDateTime(7).ToString("dd/MM/yyyy");
                        s.StartBy = r.IsDBNull(8) ? "" : r.GetString(8);
                        s.Started = r.GetDateTime(9).ToString("dd/MM/yyyy HH:mm");
                        s.ElapsedSec = r.IsDBNull(10) ? 0 : r.GetInt32(10);
                        s.Note = r.IsDBNull(11) ? "" : r.GetString(11);
                    }
                }
                if (s.State == "running")
                {
                    using (var cmd = new SqlCommand("SELECT CASE WHEN COL_LENGTH('dbo.FTS11ODRL','StopReq') IS NULL THEN 0 ELSE (SELECT CAST(StopReq AS INT) FROM dbo.FTS11ODRL WHERE DocEntry = @id) END", conn))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                        s.StopRequested = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                    }
                }
                if (s.State != "running" && maxErrorLines > 0)
                {
                    using (var cmd = new SqlCommand("IF OBJECT_ID('dbo.FTS11DRL1') IS NOT NULL SELECT TOP (@n) FACode, Message FROM dbo.FTS11DRL1 WHERE RunEntry = @id ORDER BY LineNum", conn))
                    {
                        cmd.Parameters.Add("@n", SqlDbType.Int).Value = maxErrorLines;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                        using (SqlDataReader r = cmd.ExecuteReader())
                            while (r.Read()) s.ErrorLines.Add(new[] { r.IsDBNull(0) ? "" : r.GetString(0), r.IsDBNull(1) ? "" : r.GetString(1) });
                    }
                }
                return s;
            }
        }

        /// <summary>All error lines of a run (CSV download).</summary>
        public static List<string[]> AllErrors(string cs, int runId)
        {
            var list = new List<string[]>();
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand("IF OBJECT_ID('dbo.FTS11DRL1') IS NOT NULL SELECT FACode, Message FROM dbo.FTS11DRL1 WHERE RunEntry = @id ORDER BY LineNum", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(new[] { r.IsDBNull(0) ? "" : r.GetString(0), r.IsDBNull(1) ? "" : r.GetString(1) });
            }
            return list;
        }

        /// <summary>Stop button: the run stops after the current asset (assets already done stay done).</summary>
        public static void RequestStop(string cs, int runId)
        {
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand("UPDATE dbo.FTS11ODRL SET StopReq = 1 WHERE DocEntry = @id AND Status = 'R'", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
