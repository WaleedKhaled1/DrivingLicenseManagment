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
    public partial class ctrlDrivingLicensesHistory : UserControl
    {
        private static DataTable _dtAllLocalLicenses;
        private static DataTable _dtAllInternationalLicenses;
        int _PersonID = -1;
        public ctrlDrivingLicensesHistory()
        {
            InitializeComponent();
        }

        public void LoadData(int PersonID)
        {
            _PersonID = PersonID;
            tcDriverLicenses.SelectedIndex = 0;
            LoadLocalLicenses();    
            LoadInernationalLicenses();
        }

        private void LoadLocalLicenses()
        {
            _dtAllLocalLicenses = clsLicenses.GetAllLocalLicensesForThisPerson(_PersonID);

            if (_dtAllLocalLicenses == null)
            {
                return;
            }

            dgvLocalLicensesHistory.DataSource = _dtAllLocalLicenses;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();


            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {

                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 90;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 90;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class.Name";
                dgvLocalLicensesHistory.Columns[2].Width = 150;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 150;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 150;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 90;
            }
        }

        private void LoadInernationalLicenses()
        {

            _dtAllInternationalLicenses = clsInternationalDrivingLicensens.GetAllInternationalLicensesForThisPerson(_PersonID);

            if (_dtAllInternationalLicenses == null)
            {
                return;
            }

            dgvInternationalLicensesHistory.DataSource = _dtAllInternationalLicenses;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();


            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {

                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 90;
                
                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 90;
                
                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 90;
                
                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 150;
                
                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 150;
                
                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 90;
            }
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID((int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value);
            clsLocalDrivingLicense LocalLicenseApp=clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmShowInternationalDrivingLicenseInfo frm = new frmShowInternationalDrivingLicenseInfo((int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
