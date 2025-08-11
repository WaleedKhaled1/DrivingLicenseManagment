using BusinessLayer;
using MyDVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MyDVLD
{
    public partial class frmListTestAppointments : Form
    {

        int _LocalDrivingLicenseID = -1;
        clsTestTypes.enTestType _TestType;
        bool IsRetake;

        private static DataTable _dtAllAppointments;
        private DataTable _dtAppointments;
        public frmListTestAppointments(int LocalDrivingLicenseID,clsTestTypes.enTestType testtype)
        {
            InitializeComponent();
            _LocalDrivingLicenseID=LocalDrivingLicenseID;
            _TestType=testtype;
           
        }

        private void SetDefaultDataForThisTest( )
        {
            if (_TestType== clsTestTypes.enTestType.VisionTest)
            {
                pbImage.Image = Resources.Vision_512;
                lblTestType.Text = "Vision Test Appintments";
            }

            else if (_TestType == clsTestTypes.enTestType.WrittenTest)
            {
                pbImage.Image = Resources.Written_Test_512;
                lblTestType.Text = "Written Test Appintments";
            }

            else if (_TestType == clsTestTypes.enTestType.StreetTest)
            {
                pbImage.Image = Resources.Street_Test_32;
                lblTestType.Text = "Street Test Appintments";
            }
        }

        private void frmVisionTestAppontments_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadLocalLicenseAndApplicationInfoForTest(_LocalDrivingLicenseID);
            SetDefaultDataForThisTest();

            _dtAllAppointments = clsTestAppointments.GetAllTestAppointments(_LocalDrivingLicenseID,(int)_TestType);
            if (_dtAllAppointments.Rows.Count != 0)
            {
                _dtAppointments = _dtAllAppointments.DefaultView.ToTable(false, "TestAppointmentID", "AppointmentDate", "PaidFees", "IsLocked");
                dataGridView1.DataSource = _dtAppointments;
                lblRecord.Text = dataGridView1.Rows.Count.ToString();
            }

            if (dataGridView1.Rows.Count > 0)
            {

                dataGridView1.Columns[0].HeaderText = "Appointment ID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Appointment Date";
                dataGridView1.Columns[1].Width = 200;

                dataGridView1.Columns[2].HeaderText = "Paid Fees";
                dataGridView1.Columns[2].Width = 90;

                dataGridView1.Columns[3].HeaderText = "Is Locked";
                dataGridView1.Columns[3].Width = 90;
                dataGridView1.Columns[3].ReadOnly = true;
            }

            }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            clsLocalDrivingLicense localDrivingLicenseApplication = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(_LocalDrivingLicenseID);

            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            clsTests LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if (LastTest == null)
            {
                frmScheduleAppointment frm1 = new frmScheduleAppointment(_LocalDrivingLicenseID, _TestType);
                frm1.ShowDialog();
                frmVisionTestAppontments_Load(null, null);
                return;
            }

            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleAppointment frm2 = new frmScheduleAppointment
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);

            frm2.ShowDialog();

            frmVisionTestAppontments_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTestAppointments testAppointments = clsTestAppointments.FindTestAppointmentInfoByID((int)dataGridView1.CurrentRow.Cells[0].Value);

            frmScheduleAppointment frm = new frmScheduleAppointment(_LocalDrivingLicenseID, _TestType,(int) dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmVisionTestAppontments_Load(null, null);
        }

        private void takeTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest frm=new frmTakeTest(_TestType, (int)dataGridView1.CurrentRow.Cells[0].Value);
             frm.ShowDialog();
            frmVisionTestAppontments_Load(null, null);

        }

    }
}
