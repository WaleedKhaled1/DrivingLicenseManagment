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
    public partial class frmManageIntrnationalDrivingLicense : Form
    {
        private static DataTable _dtAllInternationalLicensesApplications;
        public frmManageIntrnationalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmManageIntrnationalDrivingLicense_Load(object sender, EventArgs e)
        {

            _dtAllInternationalLicensesApplications = clsInternationalDrivingLicensens.GetAllInternationalLicensesApplications();
            dgvInternationalLicensesHistory.DataSource = _dtAllInternationalLicensesApplications;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();


            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {

                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 90;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 90;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 90;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[3].Width = 90;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 150;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[5].Width = 150;

                dgvInternationalLicensesHistory.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[6].Width = 90;
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses license = clsLicenses.FindLicenseInfoByLicenseID((int)dgvInternationalLicensesHistory.CurrentRow.Cells[3].Value);
            clsLocalDrivingLicense localDrivingLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(license.ApplicationID);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(localDrivingLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddNewIntrnationalDrivingLicense frm=new frmAddNewIntrnationalDrivingLicense();
            frm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses license = clsLicenses.FindLicenseInfoByLicenseID((int)dgvInternationalLicensesHistory.CurrentRow.Cells[3].Value);
            clsLocalDrivingLicense localDrivingLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(license.ApplicationID);

            frmShowPersonInfo frm = new frmShowPersonInfo(localDrivingLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalDrivingLicenseInfo frm = new frmShowInternationalDrivingLicenseInfo((int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
