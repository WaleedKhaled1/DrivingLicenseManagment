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
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        clsLocalDrivingLicense obj;
        int _LocalLicenseAppID=-1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalLicenseAppID; }
        }
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        public void LoadLocalLicenseAndApplicationInfoForTest(int LocalDrivingLicenseID)
        {
            _LocalLicenseAppID = LocalDrivingLicenseID;
            obj=clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(LocalDrivingLicenseID);

            if (obj == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + LocalDrivingLicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseID=clsLicenses.GetActiveLicenseIDByPersonID(obj.ApplicantPersonID,obj.LocalLicenseClassID);

            llShowLicenseInfo.Enabled = LicenseID != -1;

            lblLocalID.Text = LocalDrivingLicenseID.ToString();

            lblLicense.Text= clsLicenseClass.FindLicenseClassInfoByID(obj.LocalLicenseClassID).ClassName;


            lblTest.Text = obj.GetNumberOfPassedTests().ToString()+"/3";
            lblID.Text=obj.ApplicationID.ToString();
            lblStatus.Text = ((clsApplications.enStatus)obj.ApplicationStatus).ToString();
            lblFees.Text=obj.PaidFees.ToString();
            lblType.Text=((clsApplications.enApplicationType)obj.ApplicationTypeID).ToString();
            lblApplicant.Text=obj.PersonInfo.FullName;
            lblDate.Text=obj.ApplicationDate.ToString();
            lblStatusDate.Text=obj.ApplicationLastStatusDate.ToString();
            lblUser.Text=ClassGlobal1.CurrentUser.UserName;
        }

        private void llPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           frmShowPersonInfo personInfo=new frmShowPersonInfo(obj.ApplicantPersonID);
           personInfo.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm=new frmShowLicenseInfo(_LocalLicenseAppID);
            frm.ShowDialog();
        }
    }
}
