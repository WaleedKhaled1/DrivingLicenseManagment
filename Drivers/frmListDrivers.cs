using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD
{
    public partial class frmListDrivers : Form
    {
        DataTable _dtAllDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
              _dtAllDrivers= clsDrivers.GetAllDriversInfo();

            dataGridView1.DataSource = _dtAllDrivers;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
            cbFilter.SelectedIndex = 0;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "Driver ID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Person ID";
                dataGridView1.Columns[1].Width = 90;

                dataGridView1.Columns[2].HeaderText = "National No.";
                dataGridView1.Columns[2].Width = 90;

                dataGridView1.Columns[3].HeaderText = "Full Name";
                dataGridView1.Columns[3].Width = 200;

                dataGridView1.Columns[4].HeaderText = "Date";
                dataGridView1.Columns[4].Width = 150;

                dataGridView1.Columns[5].HeaderText = "Active Licenses";
                dataGridView1.Columns[5].Width = 90;
            }


        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilter.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
               _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID" || FilterColumn == "DriverID")
                _dtAllDrivers.DefaultView.RowFilter = $"[{FilterColumn}]={txtFilter.Text}";

            else
                _dtAllDrivers.DefaultView.RowFilter = $"[{FilterColumn}] like '{txtFilter.Text}%'";

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "Driver ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dataGridView1.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory((int)dataGridView1.CurrentRow.Cells[1].Value);
                frm.ShowDialog();
        }

        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewIntrnationalDrivingLicense frm = new frmAddNewIntrnationalDrivingLicense();
            frm.ShowDialog();
        }
    }
}
