using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module
{
    public class ChangeDatabaseActiveDirectoryAuthentication : AuthenticationActiveDirectory<SecuritySystemUser, CustomLogonParametersForActiveDirectoryAuthentication>
    {
        public ChangeDatabaseActiveDirectoryAuthentication()
        {
            CreateUserAutomatically = true;
        }
        public override bool IsLogoffEnabled
        {
            get
            {
                return true;
            }
        }
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return true;
            }
        }
    }
}