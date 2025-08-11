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

namespace MyDVLD
{
    public partial class ctrlScheduledTest : UserControl
    {
        int _TestAppointmentID = -1;
        int _TestID = -1;

        clsLocalDrivingLicense LocalLicense;
        clsTestTypes.enTestType testtype;
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentID;
            }
        }

        public int TestID
        {
            get
            {
                return _TestID;
            }
        }


        public clsTestTypes.enTestType TestType
        {
            get
            {
                return testtype;
            }

            set
            {
                testtype = value;

                switch (testtype)
                {

                    case clsTestTypes.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestTypes.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestTypes.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;

                        }
                }
            }
        }
            
        public void LoadData(int AppointmentID)
        {
            _TestAppointmentID = AppointmentID;

            clsTestAppointments testAppointment = clsTestAppointments.FindTestAppointmentInfoByID(AppointmentID);

            if (testAppointment == null)
            {
                MessageBox.Show("Error: No  Appointment ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }

            LocalLicense =clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(testAppointment.LocalDrivingLicenseApplicationID);

            if (LocalLicense == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + LocalLicense.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseAppID.Text=LocalLicense.LocalDrivingLicenseID.ToString();

            lblDrivingClass.Text= clsLicenseClass.FindLicenseClassInfoByID(LocalLicense.LocalLicenseClassID).ClassName;
            
            lblFullName.Text=LocalLicense.PersonInfo.FullName;
            lblTrial.Text = clsLocalDrivingLicense.TotalTrialsPerTest(LocalLicense.LocalDrivingLicenseID, (int)testtype).ToString();

          

            lblDate.Text=testAppointment.AppointmentDate.ToString();

            clsTestTypes testTypes = clsTestTypes.FindTestTypeInfo(testtype);
            lblFees.Text=testTypes.TestTypeFees.ToString();

            _TestID=testAppointment.TestID;

            if (_TestID != -1)
                lblTestID.Text = _TestID.ToString();
            else
                lblTestID.Text = "Not taken yet!";
        }
    }
}
