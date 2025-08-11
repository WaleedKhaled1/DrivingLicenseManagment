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
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
       int _LocalDrivingLicenseID=-1;
        clsLocalDrivingLicense obj;

      

        public frmIssueDrivingLicenseFirstTime(int localDrivingLicenseID)
        {
            InitializeComponent();
            _LocalDrivingLicenseID = localDrivingLicenseID;
        }

        private void frmIssueDrivingLicenseFirstTime_Load(object sender, EventArgs e)
        {
            obj = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(_LocalDrivingLicenseID);

            if (obj == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsTests.PassedAllTests(_LocalDrivingLicenseID))
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = obj.GetActiveLicenseID();

            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }



            ctrlDrivingLicenseApplicationInfo1.LoadLocalLicenseAndApplicationInfoForTest(_LocalDrivingLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            
            clsLicenses licenses = new clsLicenses();
          

            licenses.ApplicationID=obj.ApplicationID;
            licenses.LicenseClass=obj.LocalLicenseClassID;
            licenses.IssueDate=DateTime.Now;
            licenses.ExpirationDate =licenses.IssueDate.AddYears(clsLicenseClass.FindLicenseClassInfoByID(obj.LocalLicenseClassID).DefaultValidityLength);
            licenses.Notes=textBox1.Text;
            licenses.PaidFees= clsLicenseClass.FindLicenseClassInfoByID(obj.LocalLicenseClassID).ClassFees;

           licenses.IsActive=true;

            licenses.IssueReason =clsLicenses.enIssue.FirstTime;

            licenses.CreatedByUserID=obj.CreatedByUserID;

            licenses.PersonID=obj.ApplicantPersonID;
            licenses.DCreatedDate=DateTime.Now;

            if(licenses.Save())
            {
                MessageBox.Show("License Issued Successfully with license ID = " + licenses.LicenseID,"Succeded",MessageBoxButtons.OK,MessageBoxIcon.Information);
                obj.SetComplete();
            }
            else
                MessageBox.Show("License Issued Was Failed","Succeded", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
