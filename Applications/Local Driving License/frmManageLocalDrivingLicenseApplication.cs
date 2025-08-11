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
    public partial class frmManageLocalDrivingLicenseApplication : Form
    {
        private static DataTable _dtAllApplications;

        public frmManageLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmManageLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
           
            _dtAllApplications = clsLocalDrivingLicense.GetAllLocalDrivingLicenseApplications();
            dataGridView1.DataSource = _dtAllApplications;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
           

            if (dataGridView1.Rows.Count > 0)
            {

                dataGridView1.Columns[0].HeaderText = "L.D.L.AppID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Driving Class";
                dataGridView1.Columns[1].Width = 180;

                dataGridView1.Columns[2].HeaderText = "NationalNo.";
                dataGridView1.Columns[2].Width = 90;

                dataGridView1.Columns[3].HeaderText = "FullName";
                dataGridView1.Columns[3].Width = 220;

                dataGridView1.Columns[4].HeaderText = "Application Date";
                dataGridView1.Columns[4].Width = 180;

                dataGridView1.Columns[5].HeaderText = "Passed Tests";
                dataGridView1.Columns[5].Width = 180;

                dataGridView1.Columns[6].HeaderText = "Status";
                dataGridView1.Columns[6].Width = 90;

            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "NationalNo.":
                    FilterColumn = "NationalNo";
                    break;

                case "FullName":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;
            }

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllApplications.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")

                _dtAllApplications.DefaultView.RowFilter = $"[{FilterColumn}]={txtFilter.Text.Trim()}";
            else
                _dtAllApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text != "None");

            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicense obj = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID((int)dataGridView1.CurrentRow.Cells[0].Value);
          
            if(MessageBox.Show("Are you sure do want to cancel this application?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
               if (obj.Cancel())
                    MessageBox.Show("Application Cancelled Successfully","Cancelled",MessageBoxButtons.OK,MessageBoxIcon.Information);
               else
                    MessageBox.Show("Cancel Was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            frmManageLocalDrivingLicenseApplication_Load(null,null);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicense frm = new frmAddEditLocalDrivingLicense();
            frm.ShowDialog();
            frmManageLocalDrivingLicenseApplication_Load(null, null);
        }

        private void DisableOrEnable()
        {
            string statusText = dataGridView1.CurrentRow.Cells[6].Value.ToString();

            if (Enum.TryParse(statusText, out clsApplications.enStatus status))
            {
                if (status == clsApplications.enStatus.Cancelled)
                {
                }
                else if (status == clsApplications.enStatus.Completed)
                {
                    showApplicationDetailsToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = false;
                    deleteApplicationToolStripMenuItem.Enabled = false;
                    seToolStripMenuItem.Enabled = false;
                    cancelApplicationToolStripMenuItem.Enabled = false;
                    sechduleStreetTestToolStripMenuItem.Enabled = false;
                    issueToolStripMenuItem.Enabled = false;
                    showToolStripMenuItem.Enabled = true;
                    showPersonToolStripMenuItem.Enabled = true;
                }

                else
                {
                    int testPasses = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value);

                    //Shared Enabled
                    showApplicationDetailsToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = true;
                    deleteApplicationToolStripMenuItem.Enabled = true;
                    cancelApplicationToolStripMenuItem.Enabled = true;
                    showPersonToolStripMenuItem.Enabled = true;

                    switch (testPasses)
                    {
                        case 0:
                            seToolStripMenuItem.Enabled = true;
                            sechduleVisionTestToolStripMenuItem.Enabled = true;
                            sechduleWrittenToolStripMenuItem.Enabled = false;
                            sechduleStreetTestToolStripMenuItem.Enabled = false;
                            issueToolStripMenuItem.Enabled = false;
                            showToolStripMenuItem.Enabled = false;
                            break;

                        case 1:
                            seToolStripMenuItem.Enabled = true;
                            sechduleVisionTestToolStripMenuItem.Enabled = false;
                            sechduleWrittenToolStripMenuItem.Enabled = true;
                            sechduleStreetTestToolStripMenuItem.Enabled = false;
                            issueToolStripMenuItem.Enabled = false;
                            showToolStripMenuItem.Enabled = false;
                            break;

                        case 2:
                            seToolStripMenuItem.Enabled = true;
                            sechduleVisionTestToolStripMenuItem.Enabled = false;
                            sechduleWrittenToolStripMenuItem.Enabled = false;
                            sechduleStreetTestToolStripMenuItem.Enabled = true;
                            issueToolStripMenuItem.Enabled = false;
                            showToolStripMenuItem.Enabled = false;
                            break;

                        case 3:
                            seToolStripMenuItem.Enabled = false;
                            sechduleVisionTestToolStripMenuItem.Enabled = false;
                            sechduleWrittenToolStripMenuItem.Enabled = false;
                            sechduleStreetTestToolStripMenuItem.Enabled = false;
                            issueToolStripMenuItem.Enabled = true;
                            showToolStripMenuItem.Enabled = false;
                            break;
                    }
                }
            }
                

            }

        private void _ScheduleTest(clsTestTypes.enTestType TestType)
        {

            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, TestType);
            frm.ShowDialog();

            frmManageLocalDrivingLicenseApplication_Load(null, null);

        }
        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.VisionTest);
        }

        private void sechduleWrittenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.WrittenTest);

        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.StreetTest);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicense LocalDrivingLicenseApplication =
                    clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID
                                                    (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dataGridView1.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showToolStripMenuItem.Enabled = LicenseExists;
            editToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enStatus.New);
            seToolStripMenuItem.Enabled = !LicenseExists;

            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enStatus.New);

            deleteApplicationToolStripMenuItem.Enabled =
                (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enStatus.New);

            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.StreetTest);

            seToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplications.enStatus.New);

            if (seToolStripMenuItem.Enabled)
            {
                sechduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                sechduleWrittenToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                sechduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;

            }
        }

        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmIssueDrivingLicenseFirstTime frm =new frmIssueDrivingLicenseFirstTime((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmManageLocalDrivingLicenseApplication_Load(null, null);


        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm =new frmShowLicenseInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicense frm =new frmAddEditLocalDrivingLicense((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmManageLocalDrivingLicenseApplication_Load(null, null);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete This Application","Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.No)
            return;

            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

                if (clsLocalDrivingLicense._DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID))
                {
                    MessageBox.Show("Deleted was Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   frmManageLocalDrivingLicenseApplication_Load(null,null);
                }
                else
                {
                    MessageBox.Show("Delete Application Was Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            


        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID((int)dataGridView1.CurrentRow.Cells[0].Value);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLocalDrivingLicenseAppInfo frm = new frmShowLocalDrivingLicenseAppInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
