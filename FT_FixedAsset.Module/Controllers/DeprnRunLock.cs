using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace FT_FixedAsset.Module.Controllers
{
    /// <summary>
    /// Depreciation run log (FTS11ODRL) and the "month being depreciated" lock.
    /// - Run Depreciation registers a running row (period + progress) and closes it at the end.
    /// - While a run is active, postings dated inside its period are blocked (write off, disposal, cancel, import...).
    /// - Only one run at a time.
    /// - A run with no progress for StaleMinutes is treated as interrupted (e.g. IIS restart), so it never blocks forever.
    /// </summary>
    public static class DeprnRunLock
    {
        public const int StaleMinutes = 10;

        public sealed class RunInfo
        {
            public int Id;
            public DateTime PrdFrom, PrdTo;
            public string StartBy;
            public DateTime StartDate;
            public int Total, Done;
        }

        private const string CreateTableSql = @"
IF OBJECT_ID('dbo.FTS11ODRL') IS NULL
BEGIN
    CREATE TABLE dbo.FTS11ODRL (
        DocEntry  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FTS11ODRL PRIMARY KEY,
        DateFrm   DATETIME NOT NULL,
        DateTo    DATETIME NOT NULL,
        PrdFrom   DATETIME NOT NULL,
        PrdTo     DATETIME NOT NULL,
        Status    NVARCHAR(1) NOT NULL,          -- R Running, F Finished, E Error, X Interrupted
        Total     INT NOT NULL DEFAULT 0,
        Done      INT NOT NULL DEFAULT 0,
        Generated INT NOT NULL DEFAULT 0,
        Skipped   INT NOT NULL DEFAULT 0,
        ErrCnt    INT NOT NULL DEFAULT 0,
        StartBy   NVARCHAR(50) NULL,
        StartDate DATETIME NOT NULL,
        LastBeat  DATETIME NOT NULL,
        EndDate   DATETIME NULL,
        Note      NVARCHAR(254) NULL
    );
    CREATE INDEX iStatus_FTS11ODRL ON dbo.FTS11ODRL (Status);
END
IF COL_LENGTH('dbo.FTS11ODRL', 'StopReq') IS NULL
    ALTER TABLE dbo.FTS11ODRL ADD StopReq BIT NOT NULL CONSTRAINT DF_FTS11ODRL_StopReq DEFAULT 0;   -- Stop button
IF OBJECT_ID('dbo.FTS11DRL1') IS NULL
    CREATE TABLE dbo.FTS11DRL1 (                     -- errors of a run (one line per asset)
        RunEntry  INT NOT NULL,
        LineNum   INT NOT NULL,
        FAKey     INT NULL,
        FACode    NVARCHAR(200) NULL,
        Message   NVARCHAR(1000) NULL,
        CONSTRAINT PK_FTS11DRL1 PRIMARY KEY (RunEntry, LineNum)
    );";

        // marks runs without progress for StaleMinutes as interrupted
        private static readonly string ExpireSql =
            "UPDATE dbo.FTS11ODRL SET Status = 'X', EndDate = GETDATE(), Note = 'Interrupted (no progress for " + StaleMinutes + " minutes)' " +
            "WHERE Status = 'R' AND LastBeat < DATEADD(MINUTE, -" + StaleMinutes + ", GETDATE())";

        private static readonly string ActiveSql =
            "SELECT TOP 1 DocEntry, PrdFrom, PrdTo, StartBy, StartDate, Total, Done FROM dbo.FTS11ODRL " +
            "WHERE Status = 'R' AND LastBeat >= DATEADD(MINUTE, -" + StaleMinutes + ", GETDATE()) ORDER BY DocEntry DESC";

        /// <summary>A run by id (any status), or null.</summary>
        public static RunInfo GetRun(string connectionString, int runId)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT DocEntry, PrdFrom, PrdTo, StartBy, StartDate, Total, Done FROM dbo.FTS11ODRL WHERE DocEntry = @id", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new RunInfo
                    {
                        Id = r.GetInt32(0), PrdFrom = r.GetDateTime(1), PrdTo = r.GetDateTime(2),
                        StartBy = r.IsDBNull(3) ? "" : r.GetString(3), StartDate = r.GetDateTime(4), Total = r.GetInt32(5), Done = r.GetInt32(6)
                    };
                }
            }
        }

        /// <summary>The depreciation run in progress, or null.</summary>
        public static RunInfo GetActive(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (!TableExists(conn)) return null;   // never set up = no run can be active
                return ReadActive(conn, null);
            }
        }

        /// <summary>
        /// Message to show when a document dated <paramref name="docDate"/> cannot be posted now, or null when it can.
        /// </summary>
        public static string CheckPosting(string connectionString, DateTime docDate, string what)
        {
            RunInfo run = GetActive(connectionString);
            if (run == null) return null;
            DateTime d = docDate.Date;
            if (d < run.PrdFrom.Date || d > run.PrdTo.Date) return null;
            return (string.IsNullOrEmpty(what) ? "" : what + ": ") + "cannot post into " + Period(run) + " while depreciation is running " +
                   Progress(run) + ". Please try again after it finishes.";
        }

        /// <summary>
        /// Check for a document about to be saved or cancelled (transactions T02xx/T03xx, depreciation journal, F/A master).
        /// Returns the message to show, or null when it may be posted.
        /// - Transactions / journal: blocked when the document date is inside the running period.
        /// - New asset: blocked when its acquisition or effective date is inside the running period.
        /// - Cancel asset: blocked when the asset is part of the run (effective on or before the period end).
        /// </summary>
        public static string CheckDocument(string connectionString, object doc, bool cancelling)
        {
            if (doc == null) return null;
            Type t = doc.GetType();
            bool isAsset = t.Name == "FTS11OFAM";
            DateTime? docDate = isAsset ? null : (DateValue(doc, "U_DocDate") ?? DateValue(doc, "DocDate"));
            if (!isAsset && docDate == null) return null;                       // not a posting document
            if (isAsset && !cancelling && !BoolValue(doc, "IsNew")) return null; // editing an existing asset master

            RunInfo run = GetActive(connectionString);
            if (run == null) return null;

            string name = Text(doc, "U_FACode");
            string what = isAsset ? ("F/A " + name).Trim() : ("Document " + (Text(doc, "DocNum") is string n && n != "0" && n != "" ? n : "")).Trim();
            string tail = " while depreciation for " + Period(run) + " is running " + Progress(run) + ". Please try again after it finishes.";

            if (isAsset)
            {
                DateTime? eff = DateValue(doc, "U_EffDate"), acq = DateValue(doc, "U_AcqDate");
                if (cancelling)
                {
                    if (eff == null || eff.Value.Date <= run.PrdTo.Date) return what + ": cannot be cancelled" + tail;
                    return null;
                }
                if (InRun(run, acq) || InRun(run, eff)) return what + ": acquisition / effective date is inside the depreciation period - cannot be added" + tail;
                return null;
            }

            if (InRun(run, docDate)) return what + ": cannot " + (cancelling ? "cancel" : "post") + " a document dated " +
                                            docDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + tail;
            return null;
        }

        private static bool InRun(RunInfo run, DateTime? d)
        {
            return d != null && d.Value.Date >= run.PrdFrom.Date && d.Value.Date <= run.PrdTo.Date;
        }

        private static object Prop(object o, string name)
        {
            var p = o.GetType().GetProperty(name);
            return p == null ? null : p.GetValue(o, null);
        }

        private static DateTime? DateValue(object o, string name)
        {
            object v = Prop(o, name);
            if (v is DateTime d && d.Year > 1900) return d;
            return null;
        }

        private static bool BoolValue(object o, string name)
        {
            return Prop(o, name) is bool b && b;
        }

        private static string Text(object o, string name)
        {
            object v = Prop(o, name);
            return v == null ? "" : v.ToString();
        }

        /// <summary>Message when anything that may post is blocked because a run is active (e.g. data import), or null.</summary>
        public static string CheckAnyRun(string connectionString)
        {
            RunInfo run = GetActive(connectionString);
            if (run == null) return null;
            return "Depreciation for " + Period(run) + " is running " + Progress(run) + ". Please try again after it finishes.";
        }

        /// <summary>
        /// Registers a new run. Returns its id, or 0 with <paramref name="blockMessage"/> when another run is active.
        /// </summary>
        public static int Start(string connectionString, DateTime dateFrm, DateTime dateTo, string user, int total, out string blockMessage)
        {
            blockMessage = null;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Exec(conn, null, CreateTableSql);

                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    // serialize "check + insert" so two users clicking Run at the same moment cannot both start
                    Exec(conn, tr, "DECLARE @r INT; EXEC @r = sp_getapplock @Resource = 'FTS11_DEPRN_RUN', @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 15000; " +
                                   "IF @r < 0 RAISERROR('Could not start depreciation run (lock timeout), please try again.', 16, 1);");
                    Exec(conn, tr, ExpireSql);

                    RunInfo active = ReadActive(conn, tr);
                    if (active != null)
                    {
                        tr.Rollback();
                        blockMessage = "Depreciation for " + Period(active) + " is already running " + Progress(active) + ".";
                        return 0;
                    }

                    DateTime prdFrom, prdTo;
                    PeriodRange(conn, tr, dateFrm, dateTo, out prdFrom, out prdTo);

                    using (var cmd = new SqlCommand(
                        "INSERT INTO dbo.FTS11ODRL (DateFrm, DateTo, PrdFrom, PrdTo, Status, Total, Done, StartBy, StartDate, LastBeat) " +
                        "VALUES (@f, @t, @pf, @pt, 'R', @total, 0, @user, GETDATE(), GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tr))
                    {
                        cmd.Parameters.Add("@f", SqlDbType.DateTime).Value = dateFrm;
                        cmd.Parameters.Add("@t", SqlDbType.DateTime).Value = dateTo;
                        cmd.Parameters.Add("@pf", SqlDbType.DateTime).Value = prdFrom;
                        cmd.Parameters.Add("@pt", SqlDbType.DateTime).Value = prdTo;
                        cmd.Parameters.Add("@total", SqlDbType.Int).Value = total;
                        cmd.Parameters.Add("@user", SqlDbType.NVarChar, 50).Value = (object)user ?? DBNull.Value;
                        int id = (int)cmd.ExecuteScalar();
                        tr.Commit();
                        return id;
                    }
                }
            }
        }

        /// <summary>Progress heartbeat (keeps the lock alive).</summary>
        public static void Beat(string connectionString, int runId, int done)
        {
            if (runId <= 0) return;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("UPDATE dbo.FTS11ODRL SET Done = @d, LastBeat = GETDATE() WHERE DocEntry = @id AND Status = 'R'", conn))
                {
                    cmd.Parameters.Add("@d", SqlDbType.Int).Value = done;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* progress only - never stop the run because of it */ }
        }

        /// <summary>Closes the run (status F finished / E error). Always call in finally.</summary>
        public static void Finish(string connectionString, int runId, bool ok, int done, string note)
        {
            Finish(connectionString, runId, ok, done, -1, -1, -1, note);
        }

        /// <summary>Closes the run with its result counts (-1 = not known).</summary>
        public static void Finish(string connectionString, int runId, bool ok, int done, int generated, int skipped, int errors, string note)
        {
            if (runId <= 0) return;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.FTS11ODRL SET Status = @s, Done = @d, EndDate = GETDATE(), LastBeat = GETDATE(), Note = @n, " +
                    "Generated = CASE WHEN @g < 0 THEN Generated ELSE @g END, Skipped = CASE WHEN @k < 0 THEN Skipped ELSE @k END, " +
                    "ErrCnt = CASE WHEN @e < 0 THEN ErrCnt ELSE @e END WHERE DocEntry = @id", conn))
                {
                    cmd.Parameters.Add("@s", SqlDbType.NVarChar, 1).Value = ok ? "F" : "E";
                    cmd.Parameters.Add("@d", SqlDbType.Int).Value = done;
                    cmd.Parameters.Add("@g", SqlDbType.Int).Value = generated;
                    cmd.Parameters.Add("@k", SqlDbType.Int).Value = skipped;
                    cmd.Parameters.Add("@e", SqlDbType.Int).Value = errors;
                    cmd.Parameters.Add("@n", SqlDbType.NVarChar, 254).Value = note == null ? (object)DBNull.Value : (note.Length > 254 ? note.Substring(0, 254) : note);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = runId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* a stale row expires by itself after StaleMinutes */ }
        }

        /// <summary>Creates / upgrades the run log tables (FTS11ODRL, FTS11DRL1).</summary>
        public static void EnsureTables(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Exec(conn, null, CreateTableSql);
            }
        }

        #region helpers
        private static bool TableExists(SqlConnection conn)
        {
            using (var cmd = new SqlCommand("SELECT CASE WHEN OBJECT_ID('dbo.FTS11ODRL') IS NULL THEN 0 ELSE 1 END", conn))
                return (int)cmd.ExecuteScalar() == 1;
        }

        private static RunInfo ReadActive(SqlConnection conn, SqlTransaction tr)
        {
            using (var cmd = new SqlCommand(ActiveSql, conn, tr))
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (!r.Read()) return null;
                return new RunInfo
                {
                    Id = r.GetInt32(0),
                    PrdFrom = r.GetDateTime(1),
                    PrdTo = r.GetDateTime(2),
                    StartBy = r.IsDBNull(3) ? "" : r.GetString(3),
                    StartDate = r.GetDateTime(4),
                    Total = r.GetInt32(5),
                    Done = r.GetInt32(6)
                };
            }
        }

        // first day of the posting period containing DateFrm .. last day of the period containing DateTo
        // (SAP periods via FTS11_vw_OFPR, same as sp_FTS11_GenerateDeprn); calendar months if not found
        private static void PeriodRange(SqlConnection conn, SqlTransaction tr, DateTime dateFrm, DateTime dateTo, out DateTime prdFrom, out DateTime prdTo)
        {
            prdFrom = new DateTime(dateFrm.Year, dateFrm.Month, 1);
            prdTo = new DateTime(dateTo.Year, dateTo.Month, 1).AddMonths(1).AddDays(-1);
            try
            {
                using (var cmd = new SqlCommand(
                    "SELECT (SELECT TOP 1 F_RefDate FROM FTS11_vw_OFPR WHERE @f BETWEEN F_RefDate AND T_RefDate), " +
                    "       (SELECT TOP 1 T_RefDate FROM FTS11_vw_OFPR WHERE @t BETWEEN F_RefDate AND T_RefDate)", conn, tr))
                {
                    cmd.Parameters.Add("@f", SqlDbType.DateTime).Value = dateFrm.Date;
                    cmd.Parameters.Add("@t", SqlDbType.DateTime).Value = dateTo.Date;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            if (!r.IsDBNull(0)) prdFrom = r.GetDateTime(0);
                            if (!r.IsDBNull(1)) prdTo = r.GetDateTime(1);
                        }
                    }
                }
            }
            catch { /* keep calendar months */ }
        }

        private static void Exec(SqlConnection conn, SqlTransaction tr, string sql)
        {
            using (var cmd = new SqlCommand(sql, conn, tr)) { cmd.CommandTimeout = 60; cmd.ExecuteNonQuery(); }
        }

        private static string Period(RunInfo run)
        {
            return run.PrdFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + run.PrdTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private static string Progress(RunInfo run)
        {
            return "(started by " + run.StartBy + " at " + run.StartDate.ToString("HH:mm", CultureInfo.InvariantCulture) +
                   (run.Total > 0 ? ", " + run.Done.ToString("N0", CultureInfo.InvariantCulture) + " / " + run.Total.ToString("N0", CultureInfo.InvariantCulture) + " assets" : "") + ")";
        }
        #endregion
    }
}
