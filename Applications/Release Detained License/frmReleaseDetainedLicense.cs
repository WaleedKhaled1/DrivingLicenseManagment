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
using static BusinessLayer.clsLicenses;

namespace MyDVLD
{
    public partial class frmReleaseDetainedLicense : Form
    {
        int _LicenseID = -1;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
           _LicenseID = LicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter11.txtLicenseIDFocus();

            ctrlDrivingLicenseInfoWithFilter11.OnLicenseSelected += LoadDataForAppInfo;//زي اشتراك

            if (_LicenseID!=-1)
            {
                ctrlDrivingLicenseInfoWithFilter11.LoadData(_LicenseID);
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();
            }

          
        }

        private void LoadDataForAppInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            clsDetainedLicense detain=clsDetainedLicense.FindLicenseDetainedInfoByLicenseID(LicenseID);

            llShowLicenseHistory.Enabled = true;

            if (!clsDetainedLicense.IsDetained(LicenseID))
            {
                MessageBox.Show("Selected License is not Detained, choose another one.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRelease.Enabled = true;
            lblDetainID.Text=detain.DetainID.ToString();
            lblDetainDate.Text=detain.DetainDate.ToString();
            lblLicenseID.Text=detain.LicenseID.ToString();
            lblCreatedByUser.Text = (clsUsers.FindUser(detain.CreatedByUserID).UserName).ToString();
            lblFineFees.Text=detain.FineFees.ToString();

            lblApplicationFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense).AppTypeFees.ToString();
            lblTotalFees.Text=(Convert.ToSingle(lblFineFees.Text)+ Convert.ToSingle(lblApplicationFees.Text)).ToString();


        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_LicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_LicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsApplications App=new clsApplications();

            App.ApplicantPersonID = clsLicenses.FindLicenseInfoByLicenseID(_LicenseID).PersonID;
            App.ApplicationDate = DateTime.Now;
            App.ApplicationTypeID=(int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense;
            App.ApplicationStatus=clsApplications.enStatus.Completed;
            App.ApplicationLastStatusDate=DateTime.Now;
            App.PaidFees=Convert.ToSingle( lblApplicationFees.Text);
            App.CreatedByUserID=ClassGlobal1.CurrentUser.UserID;

            if(!App.Save())
            {
                MessageBox.Show("Save Application was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            lblApplicationID.Text=App.ApplicationID.ToString();

            clsDetainedLicense detain=new clsDetainedLicense();
            detain.LicenseID=_LicenseID;
            detain.ReleaseDate=DateTime.Now;
            detain.ReleasedByUserID=ClassGlobal1.CurrentUser.UserID;
            detain.ReleaseApplicationID=App.ApplicationID;

            if(detain.UpdateDetainedLicenseToReleased())
            {
                MessageBox.Show("Released Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                clsLicenses.UpdateStatusLocalLicenseToActiveOrInActive(_LicenseID, 1);
               
                btnRelease.Enabled = false;
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();
                llShowLicenseInfo.Enabled = true;
            }
            else
                MessageBox.Show("Release was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
