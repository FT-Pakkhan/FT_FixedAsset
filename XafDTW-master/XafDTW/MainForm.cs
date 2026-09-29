using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XafDTW.Helpers;

namespace XafDTW
{
    public partial class MainForm : Form
    {
        public SecurityStrategyComplex Security { get; set; }
        public IObjectSpaceProvider ObjectSpaceProvider { get; set; }
        public string ConnString { get; set; }
        string ExcelFullPath { get; set; }
        string TypeCode { get; set; }
        public MainForm(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider, string connString)
        {
            InitializeComponent();
            Security = security;
            ObjectSpaceProvider = objectSpaceProvider;
            ConnString = connString;
            openFileDialog.FileName = "*" + Logs.getConfigurationAppSettingValue("ExcelExtension");
        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            DialogResult drResult = openFileDialog.ShowDialog();
            if (drResult == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
                ExcelFullPath = txtFilePath.Text;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (ExcelFullPath == null)
            {
                MessageBox.Show("Excel File is empty.");
                return;
            }
            Code code = new Code(Security, ObjectSpaceProvider, ConnString);
            TypeCode = ((KeyValuePair<string,string>)cmbSelect.SelectedItem).Key;
            code.selectFunction(ExcelFullPath, TypeCode);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            List<Type> listtype = ObjectSpaceProvider.EntityStore.RegisteredEntities.ToList();

            Dictionary<string, string> cmbSource = new Dictionary<string, string>();
            foreach (Type t in listtype)
            {
                string[] mytype = t.ToString().Split('.');

                switch (mytype[mytype.Length - 1].ToLower())
                {

                    case "fts11ofac":
                        cmbSource.Add(mytype[mytype.Length - 1], "Fixed Asset Class");
                        break;
                    case "fts11ofag":
                        cmbSource.Add(mytype[mytype.Length - 1], "Fixed Asset Group");
                        break;
                    case "fts11oloc":
                        cmbSource.Add(mytype[mytype.Length - 1], "Location");
                        break;
                    case "fts11oind":
                        cmbSource.Add(mytype[mytype.Length - 1], "Indicator");
                        break;
                    case "fts11ofam":
                        cmbSource.Add(mytype[mytype.Length - 1], "Fixed Asset Master");
                        break;
                        //cmbSelect.Items.Add(mytype[mytype.Length - 1]);
                }

            }
            cmbSelect.DataSource = new BindingSource(cmbSource, null);
            cmbSelect.DisplayMember = "Value";
            cmbSelect.ValueMember = "Key";
            cmbSelect.SelectedIndex = 0;
        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = "";
            this.Hide();
            this.Dispose();

            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
            Login loginForm = new Login(security);
            loginForm.defuserid = Logs.getConfigurationAppSettingValue("DefaultUserID");
            loginForm.defpassword = Logs.getConfigurationAppSettingValue("DefaultPassword");
            loginForm.Show();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            this.Close();
            Application.Exit();
        }
    }
}
