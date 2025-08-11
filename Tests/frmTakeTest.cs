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
    public partial class frmTakeTest : Form
    {
        clsTestTypes.enTestType _testType;
        int _AppointmentID = -1;
        clsTests Test;
        public frmTakeTest( clsTestTypes.enTestType testtype,int AppointmentID)
        {
            InitializeComponent();
            _testType = testtype;
            _AppointmentID=AppointmentID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestType = _testType;
            ctrlScheduledTest1.LoadData(_AppointmentID);

            if (clsTestAppointments.FindTestAppointmentInfoByID(_AppointmentID).IsLocked == true)
            {
                btnSave.Enabled = false;
                lblLock.Visible = true;

                int _TestID = ctrlScheduledTest1.TestID;

                Test = clsTests.Find(_TestID);

                textBox1.Text = Test.Notes;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
            }

            else
                Test = new clsTests();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("You must enter a reult!","Wrong",MessageBoxButtons.OK,MessageBoxIcon.Error);
                  return;
            }


            Test.TestAppointmentID=_AppointmentID;

            Test.TestResult = rbPass.Checked;

            Test.Notes=textBox1.Text;
            Test.CreatedByUserID=ClassGlobal1.CurrentUser.UserID;

            if (MessageBox.Show("Are you sure you want to save? After that you cannot change " +
                   "the Pass/Fail results after you save ?.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (Test.AddNewTest())
                {

                    MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    clsTestAppointments.UpdateAppointmentStatusToLocked(_AppointmentID);
                   
                }

                else
                    MessageBox.Show("Save Was failed.", "failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbPass_Validating(object sender, CancelEventArgs e)
        {
            if(rbPass.Checked==false && rbFail.Checked==false)
            {
                e.Cancel = true;
                errorProvider1.SetError(rbFail, "Please enter a Result");
            }
        }
    }
}
