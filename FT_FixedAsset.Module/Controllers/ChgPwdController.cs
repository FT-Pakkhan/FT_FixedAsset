using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB.Helpers;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Controllers
{
    /// <summary>
    /// Applies Password Administration (FTS11OPPA) to "Change My Password" and the
    /// forced change at logon: complexity rules, no reuse of the last N passwords,
    /// and records the change (FTS11OUPH history + SystemUsers.LastPwdChgOn).
    /// Ported from the Hiap Teck Portal.
    /// </summary>
    public class ChgPwdController : ObjectViewController<DetailView, ChangePasswordOnLogonParameters>
    {
        private DialogController dialogController;
        private string newPassword;
        private bool passedValidation;

        protected override void OnActivated()
        {
            base.OnActivated();
            dialogController = Frame.GetController<DialogController>();
            if (dialogController != null)
            {
                dialogController.AcceptAction.Executing += AcceptAction_Executing;
                dialogController.AcceptAction.Executed += AcceptAction_Executed;
            }
        }

        protected override void OnDeactivated()
        {
            if (dialogController != null)
            {
                dialogController.AcceptAction.Executing -= AcceptAction_Executing;
                dialogController.AcceptAction.Executed -= AcceptAction_Executed;
                dialogController = null;
            }
            base.OnDeactivated();
        }

        private string ConnectionString()
        {
            var helper = new ConnectionStringParser(Application.ConnectionString);
            helper.RemovePartByName("xpodatastorepool");
            return helper.GetConnectionString();
        }

        private void AcceptAction_Executing(object sender, CancelEventArgs e)
        {
            passedValidation = false;
            newPassword = null;

            var parameters = View.CurrentObject as ChangePasswordOnLogonParameters;   // ChangePasswordParameters derives from it
            if (parameters == null || SecuritySystem.CurrentUserId == null) return;

            Guid userOid = (Guid)SecuritySystem.CurrentUserId;
            string cs = ConnectionString();
            string storedHash = GetStoredPassword(cs, userOid);

            var cpp = parameters as ChangePasswordParameters;
            if (cpp != null && !PasswordHash.Verify(storedHash, cpp.OldPassword ?? ""))
                throw new UserFriendlyException("The current password is incorrect.");

            PasswordPolicy policy = FTS11OPPA.Load(cs);
            if (policy != null)
            {
                string error = policy.CheckComplexity(parameters.NewPassword);
                if (error != null)
                    throw new UserFriendlyException(error);

                if (policy.NumPrevPwd > 0)
                {
                    bool reused = PasswordHash.Verify(storedHash, parameters.NewPassword ?? "");
                    if (!reused)
                    {
                        foreach (string prev in GetPreviousPasswords(cs, userOid, policy.NumPrevPwd))
                            if (PasswordHash.Verify(prev, parameters.NewPassword ?? "")) { reused = true; break; }
                    }
                    if (reused)
                        throw new UserFriendlyException("The last " + policy.NumPrevPwd + " password(s) cannot be reused.");
                }
            }

            newPassword = parameters.NewPassword;
            passedValidation = true;
        }

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            if (!passedValidation || SecuritySystem.CurrentUserId == null) return;
            passedValidation = false;

            string hashed = PasswordHash.Hash(newPassword);
            newPassword = null;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                cn.Open();
                using (SqlTransaction tran = cn.BeginTransaction())
                using (var cmd = new SqlCommand(
                    "DECLARE @Now datetime = GETDATE(); " +
                    "INSERT INTO dbo.FTS11OUPH (UserOid, PrevPwd, ChangedOn) VALUES (@Oid, @PrevPwd, @Now); " +
                    "UPDATE dbo.SystemUsers SET LastPwdChgOn = @Now WHERE Oid = @Oid; " +
                    "UPDATE dbo.PermissionPolicyUser SET ChangePasswordOnFirstLogon = 0 WHERE Oid = @Oid;", cn, tran))
                {
                    cmd.Parameters.Add("@Oid", SqlDbType.UniqueIdentifier).Value = (Guid)SecuritySystem.CurrentUserId;
                    cmd.Parameters.Add("@PrevPwd", SqlDbType.NVarChar, 200).Value = hashed;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
            }
        }

        private static List<string> GetPreviousPasswords(string cs, Guid userOid, int count)
        {
            var result = new List<string>();
            using (var cn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(
                "SELECT TOP (@Count) PrevPwd FROM dbo.FTS11OUPH WHERE UserOid = @Oid ORDER BY ChangedOn DESC, DocEntry DESC", cn))
            {
                cmd.Parameters.Add("@Count", SqlDbType.Int).Value = count;
                cmd.Parameters.Add("@Oid", SqlDbType.UniqueIdentifier).Value = userOid;
                cn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        if (!r.IsDBNull(0)) result.Add(r.GetString(0));
            }
            return result;
        }

        private static string GetStoredPassword(string cs, Guid userOid)
        {
            using (var cn = new SqlConnection(cs))
            using (var cmd = new SqlCommand("SELECT StoredPassword FROM dbo.PermissionPolicyUser WHERE Oid = @Oid", cn))
            {
                cmd.Parameters.Add("@Oid", SqlDbType.UniqueIdentifier).Value = userOid;
                cn.Open();
                object v = cmd.ExecuteScalar();
                return v == null || v == DBNull.Value ? null : (string)v;
            }
        }

        internal static class PasswordHash
        {
            public static string Hash(string plain)
            {
                return PasswordCryptographer.HashPasswordDelegate != null
                    ? PasswordCryptographer.HashPasswordDelegate(plain)
                    : new PasswordCryptographer().GenerateSaltedPassword(plain);
            }

            public static bool Verify(string storedHash, string plain)
            {
                if (string.IsNullOrEmpty(storedHash)) return string.IsNullOrEmpty(plain);   // blank password
                try
                {
                    return PasswordCryptographer.VerifyHashedPasswordDelegate != null
                        ? PasswordCryptographer.VerifyHashedPasswordDelegate(storedHash, plain)
                        : new PasswordCryptographer().AreEqual(storedHash, plain);
                }
                catch (Exception)
                {
                    return false;   // not a hash
                }
            }
        }
    }
}
