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
    public partial class frmAddNewIntrnationalDrivingLicense : Form
    {
        int _LocalLicenseID = -1;
        int _InternationalLicenseID = -1;
        clsInternationalDrivingLicensens obj;
        public frmAddNewIntrnationalDrivingLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddNewIntrnationalDrivingLicense_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter11.OnLicenseSelected += LoadDataForAppInfo;
        }

        private void LoadDataForAppInfo(int LicenseID)
        {
        

            _LocalLicenseID = LicenseID;
            btnIssue.Enabled = true;
            LicenseHistory.Enabled = true;

            lblApplicationDate.Text = DateTime.Now.ToString();
            lblIssueDate.Text = DateTime.Now.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString();
            lblCreatedByUser.Text = ClassGlobal1.CurrentUser.UserName;
            lblLocalLicenseID.Text = LicenseID.ToString();

                lblFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.NewInternationalLicense).AppTypeFees.ToString();


            _InternationalLicenseID = clsInternationalDrivingLicensens.GetActiveInternationalLicenseIDByDriverID(ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.DriverID);

            if (_InternationalLicenseID!=-1)
            {
                MessageBox.Show("Your already have international driving license", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
                lblApplicationID.Text = clsInternationalDrivingLicensens.FindInternationalLicenseInfoByInternationalLicenseID(_InternationalLicenseID).ApplicationID.ToString();

                btnIssue.Enabled = false;
                llLicenseInfo.Enabled = true;
                return;
            }

            if (ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }


        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            obj.DriverID=clsLicenses.FindLicenseInfoByLicenseID(_LocalLicenseID).DriverID;
            obj.IssuedUsingLocalLicenseID = _LocalLicenseID;
            obj.IssueDate=DateTime.Now;
            obj.ExpirationDate=obj.IssueDate.AddYears(1);
            obj.IsActive= true;
            obj.CreatedByUserID=ClassGlobal1.CurrentUser.UserID;
            obj.ApplicantPersonID = clsLicenses.FindLicenseInfoByLicenseID(_LocalLicenseID).PersonID;
            obj.ApplicationDate = Convert.ToDateTime( lblApplicationDate.Text );
            obj.ApplicationTypeID=(int)clsApplications.enApplicationType.NewInternationalLicense;
            obj.ApplicationStatus=clsApplications.enStatus.Completed;
            obj.ApplicationLastStatusDate= DateTime.Now;
            obj.PaidFees=Convert.ToSingle( lblFees.Text);

            if(obj.Save())
            {
                MessageBox.Show("International Issued Successfully with ID= "+obj.InternationalLicenseID, "Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblApplicationID.Text=obj.ApplicationID.ToString();
                lblInternationalLicenseID.Text=obj.InternationalLicenseID.ToString();
                _InternationalLicenseID = obj.InternationalLicenseID;

                btnIssue.Enabled = false;
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();
                llLicenseInfo.Enabled=true;
            }

            else
                MessageBox.Show("Save was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void LicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalDrivingLicenseInfo frm =new frmShowInternationalDrivingLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void LicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_LocalLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
