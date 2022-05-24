using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XafDTW
{
    public partial class Result : Form
    {
        public List<DisplayResult> displayResult { get; set; }
        public Result(List<DisplayResult> Result)
        {
            InitializeComponent();
            cmbResult.Items.Add("All");
            cmbResult.Items.Add("Success");
            cmbResult.Items.Add("Failed");
            cmbResult.SelectedIndex = 0;

            displayResult = Result;
        }

        private void Result_Load(object sender, EventArgs e)
        {
            dgvResult.Rows.Clear();
            dgvResult.DataSource = displayResult;
        }

        private void cmbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbResult.SelectedIndex == 0)
                dgvResult.DataSource = displayResult;
            if (cmbResult.SelectedIndex == 1)
                dgvResult.DataSource = displayResult.Where(x => x.SuccessFail == "Success").ToList();
            if (cmbResult.SelectedIndex == 2)
                dgvResult.DataSource = displayResult.Where(x => x.SuccessFail == "Failed").ToList();

        }
    }
}
