using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Security
{
    /// <summary>
    /// Standard logon + Password Administration (FTS11OPPA) rules:
    /// account lock after N failed logons, auto-unlock after the lock duration,
    /// and "change password" forced when the password is older than the expiry days.
    /// Ported from the Hiap Teck Portal.
    /// </summary>
    public class ExpiringPasswordAuthentication : AuthenticationStandard
    {
        public override object Authenticate(IObjectSpace objectSpace)
        {
            var logonParameters = LogonParameters as AuthenticationStandardLogonParameters;
            SystemUsers lockUser = logonParameters == null || string.IsNullOrEmpty(logonParameters.UserName) ? null :
                objectSpace.FindObject<SystemUsers>(new BinaryOperator("UserName", logonParameters.UserName));
            PasswordPolicy policy = LoadPolicy(objectSpace);

            if (lockUser != null && lockUser.IsLocked)
            {
                DateTime unlockOn = lockUser.LockedDate.AddMinutes(LockMinutes(policy));
                if (DateTime.Now < unlockOn)
                    throw new InvalidOperationException(LockedMessage(unlockOn));

                // lock duration passed - unlock automatically
                lockUser.IsLocked = false;
                objectSpace.CommitChanges();
            }

            object user;
            try
            {
                user = base.Authenticate(objectSpace);   // throws on a wrong user name / password
            }
            catch (AuthenticationException)
            {
                if (lockUser != null && RecordFailedLogon(objectSpace, lockUser, policy))
                    throw new InvalidOperationException(LockedMessage(lockUser.LockedDate.AddMinutes(LockMinutes(policy))));
                throw;
            }

            if (lockUser != null && lockUser.FailedLogonCount > 0)
            {
                lockUser.FailedLogonCount = 0;
                objectSpace.CommitChanges();
            }

            var appUser = user as SystemUsers;
            if (appUser == null || appUser.ChangePasswordOnFirstLogon)
                return user;   // already has to change it

            try
            {
                if (IsPasswordExpired(appUser, policy))
                {
                    appUser.ChangePasswordOnFirstLogon = true;
                    objectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                // never block a valid logon because the expiry check failed
                Tracing.Tracer.LogError(ex);
            }

            return user;
        }

        private static PasswordPolicy LoadPolicy(IObjectSpace objectSpace)
        {
            try
            {
                return PasswordPolicy.From(objectSpace.GetObjects<FTS11OPPA>().OrderBy(o => o.DocEntry).FirstOrDefault());
            }
            catch (Exception ex)
            {
                // table not created yet in this company database - no rules
                Tracing.Tracer.LogError(ex);
                return null;
            }
        }

        // returns true when this failed attempt locks the account
        private static bool RecordFailedLogon(IObjectSpace objectSpace, SystemUsers user, PasswordPolicy policy)
        {
            int maxAttempts = policy != null ? policy.NumAuthLoc : 0;
            if (maxAttempts <= 0) return false;

            try
            {
                user.FailedLogonCount++;
                if (user.FailedLogonCount >= maxAttempts)
                    user.IsLocked = true;
                objectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(ex);
                return false;
            }
            return user.IsLocked;
        }

        private static int LockMinutes(PasswordPolicy policy)
        {
            return policy != null ? policy.LockMinutes : 30;
        }

        private static string LockedMessage(DateTime unlockOn)
        {
            return "Your account is locked due to too many failed logon attempts. " +
                   "Please try again after " + unlockOn.ToString("dd/MM/yyyy hh:mm tt") + " or contact your administrator.";
        }

        private static bool IsPasswordExpired(SystemUsers user, PasswordPolicy policy)
        {
            int expDays = policy != null ? policy.PwdExp : 0;
            if (expDays <= 0) return false;

            // never changed through the system (empty) = expired, same as the portal
            DateTime lastChanged = user.LastPwdChgOn == DateTime.MinValue ? new DateTime(1900, 1, 1) : user.LastPwdChgOn;
            return (DateTime.Now - lastChanged).TotalDays > expDays;
        }
    }
}
