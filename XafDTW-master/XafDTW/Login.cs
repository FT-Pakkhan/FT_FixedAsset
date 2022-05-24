using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;

namespace XafDTW
{
    public partial class Login : Form
    {
        public string defuserid { get; set; }
        public string defpassword { get; set; }
        private IObjectSpace securedObjectSpace;
        public SecurityStrategyComplex Security { get; set; }
        string company { get; set; }

        public Login(SecurityStrategyComplex security)
        {
            InitializeComponent();
            Security = security;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = "";
            string userName = txtUserID.Text;
            string password = txtPassword.Text;
            company = ((KeyValuePair<string, string>)cmbCompany.SelectedItem).Key;

            if (!Security.IsAuthenticated)
            {
                DataTable dt = null;
                string sqlQuery = "SELECT * FROM ODBC WHERE DBName = '" + company + "' ";
                dt = SQLHandler.GetDataTable(sqlQuery);
                foreach (DataRow row in dt.Rows)
                {
                    connectionString = row["ConnString"].ToString();
                }

                IObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(Security, connectionString, null);

                securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
                Security.Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
                try
                {
                    Security.Logon(securedObjectSpace);
                    txtUserID.Enabled = false;
                    txtPassword.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Hide();
                MainForm mainform = new MainForm(Security, objectSpaceProvider, connectionString);
                mainform.Closed += (s, args) => this.Close();
                mainform.Show();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string sqlQuery = "SELECT * FROM ODBC";
            Dictionary<string, string> cmbSource = new Dictionary<string, string>();

            dt = SQLHandler.GetDataTable(sqlQuery);
            foreach (DataRow row in dt.Rows)
            {
                cmbSource.Add(row["DBName"].ToString(), row["Description"].ToString());
            }
            cmbCompany.DataSource = new BindingSource(cmbSource, null);
            cmbCompany.DisplayMember = "Value";
            cmbCompany.ValueMember = "Key";
            cmbCompany.SelectedIndex = 0;

            txtUserID.Text = defuserid;
            txtPassword.Text = defpassword;

        }
    }
}
