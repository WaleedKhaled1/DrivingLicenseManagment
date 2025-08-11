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
    public partial class frmScheduleAppointment : Form
    {
        int _LocalDrivingLicenseID = -1;
        clsTestTypes.enTestType _testType;
        int _AppointmentID = -1;
        bool _IsRetake;
        public frmScheduleAppointment(int licenseID,clsTestTypes.enTestType testtype, int AppointmentID=-1)
        {
            InitializeComponent();
                _LocalDrivingLicenseID=licenseID;
                _testType=testtype;
            _AppointmentID=AppointmentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleAppointment_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _testType;
            ctrlScheduleTest1.LoadData(_LocalDrivingLicenseID,_AppointmentID);
        }


    }
}
