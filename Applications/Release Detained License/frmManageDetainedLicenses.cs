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
    public partial class frmManageDetainedLicenses : Form
    {
        private static DataTable _dtAllDetainedLicenses;
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _dtAllDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtAllDetainedLicenses;
            lblRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 80;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 80;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 160;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 110;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 110;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 90;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 200;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 80;

            }


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Release Application ID" || cbFilterBy.Text == "Detain ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None" && cbFilterBy.Text != "Is Released");

            if (cbFilterBy.Text == "Is Released")
            {
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
            }
            else
                cbIsReleased.Visible = false;

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsReleased.Text == "All")
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
            else if (cbIsReleased.Text == "Yes")
                _dtAllDetainedLicenses.DefaultView.RowFilter = $"[IsReleased]=true";
            else if (cbIsReleased.Text == "No")
                _dtAllDetainedLicenses.DefaultView.RowFilter = $"[IsReleased]=false";
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "Is Released":
                    {
                        FilterColumn = "IsReleased";
                        break;
                    };

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
                _dtAllDetainedLicenses.DefaultView.RowFilter = $"[{FilterColumn}]={txtFilterValue.Text}";
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = $"[{FilterColumn}] LIKE '{txtFilterValue.Text}%'"; 

            lblRecords.Text = _dtAllDetainedLicenses.Rows.Count.ToString();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm=new frmReleaseDetainedLicense();
            frm.ShowDialog();
            frmManageDetainedLicenses_Load(null, null);
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm=new frmDetainLicense();
            frm.ShowDialog();
            frmManageDetainedLicenses_Load(null, null);

        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            frmShowPersonInfo frm =new frmShowPersonInfo(Locallicense.PersonID);
            frm.ShowDialog();
         }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm= new frmReleaseDetainedLicense((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);//Sent Pramenter To Show Data Without enter license ID
            frm.ShowDialog();
            frmManageDetainedLicenses_Load(null, null);
        }
    }
}
