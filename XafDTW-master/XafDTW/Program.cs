using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using XafDTW.Helpers;

namespace XafDTW
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static System.Threading.Mutex mutex = null;
        [STAThread]
        static void Main()
        {
            //string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            const string appName = "XafDTW";
            bool createdNew;

            mutex = new System.Threading.Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Application.Exit();
                return;
            }

            MyFunctions.RegisterEntities();
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();

            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login loginForm = new Login(security);

            loginForm.defuserid = Logs.getConfigurationAppSettingValue("DefaultUserID");
            loginForm.defpassword = Logs.getConfigurationAppSettingValue("DefaultPassword");

            Application.Run(loginForm);
        }
    }
}
