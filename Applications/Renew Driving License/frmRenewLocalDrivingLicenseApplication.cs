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
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        int _OldLocalLicenseID = -1;
        int _NewLocalLicenseID = -1;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter11.txtLicenseIDFocus();
            ctrlDrivingLicenseInfoWithFilter11.OnLicenseSelected += LoadApplicationNewLicenseInfo;
        }

        private void LoadApplicationNewLicenseInfo(int _LocalLicenseID)
        {
            LicenseHistory.Enabled = true;
            this._OldLocalLicenseID= _LocalLicenseID;

            lblApplicationDate.Text = DateTime.Now.ToString();
            lblIssueDate.Text = DateTime.Now.ToString();
            lblApplicationFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.RenewDrivingLicense).AppTypeFees.ToString();

            clsLicenses LocalLicense = ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo;

            if (LocalLicense== null )
            {
              MessageBox.Show("This Licese Was Not Found!" ,"Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsLicenseClass licenseclass = clsLicenseClass.FindLicenseClassInfoByID(LocalLicense.LicenseClass);
            lblLicenseFees.Text=licenseclass.ClassFees.ToString();

            lblOldLicenseID.Text=_LocalLicenseID.ToString();
            lblExpirationDate.Text =DateTime.Now.AddYears(licenseclass.DefaultValidityLength).ToString();
            lblCreatedByUser.Text=ClassGlobal1.CurrentUser.UserName;
            lblTotalFees.Text= (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();

            CheckForLocalLicense();

        }
        private void CheckForLocalLicense()
        {
           
            if (ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo != null)
            {
                if(!ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.IsActive)
                {
                    MessageBox.Show("Selected license is InActive","Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnRenew.Enabled = false;
                }

                else if(ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.ExpirationDate>=DateTime.Now)
                {
                    MessageBox.Show("Selected license is not yet Expired,it will expire on:\n"+ ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.ExpirationDate, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnRenew.Enabled = false;
                }

                else
                {
                    btnRenew.Enabled = true
                        ;
                }
            }
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            clsLicenses OldLicense = clsLicenses.FindLicenseInfoByLicenseID(_OldLocalLicenseID);

            clsLocalDrivingLicense obj = new clsLocalDrivingLicense();
            obj.LocalLicenseClassID=OldLicense.LicenseClass;

            obj.ApplicantPersonID = clsLicenses.FindLicenseInfoByLicenseID(_OldLocalLicenseID).PersonID;
            obj.ApplicationDate = Convert.ToDateTime(lblApplicationDate.Text);
            obj.ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense;
            obj.ApplicationStatus = clsApplications.enStatus.Completed;
            obj.ApplicationLastStatusDate = DateTime.Now;
            obj.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            obj.CreatedByUserID = ClassGlobal1.CurrentUser.UserID;

            if (!obj.Save())
            {
                MessageBox.Show("Failed Save");
               
                return;
            }

            lblApplicationID.Text = obj.ApplicationID.ToString();

          

            clsLicenses license = new clsLicenses();

            license.ApplicationID = obj.ApplicationID;
            license.DriverID = OldLicense.DriverID;
            license.LicenseClass = OldLicense.LicenseClass;
            license.IssueDate = DateTime.Now;

            license.ExpirationDate = license.IssueDate.AddYears(clsLicenseClass.FindLicenseClassInfoByID(OldLicense.LicenseClass).DefaultValidityLength);
            license.Notes = txtNotes.Text;
            license.PaidFees = Convert.ToSingle(lblLicenseFees.Text);
            license.IsActive = true;
            license.IssueReason = clsLicenses.enIssue.Renew;
            license.CreatedByUserID = ClassGlobal1.CurrentUser.UserID;

            license.PersonID = OldLicense.PersonID;

            if (license.Save())
            {
                _NewLocalLicenseID=license.LicenseID;
                MessageBox.Show("Licese Renewed Successfully with ID=" + _NewLocalLicenseID, "Licese Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRenew.Enabled = false;
                clsLicenses.UpdateStatusLocalLicenseToActiveOrInActive(_OldLocalLicenseID, 0);
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();

                lblRenewedLicenseID.Text = license.LicenseID.ToString();
                LicenseInfo.Enabled = true;
            }

            else
            {
                MessageBox.Show("Licese Renewe Failed","Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense=clsLicenses.FindLicenseInfoByLicenseID(_OldLocalLicenseID);
            clsLocalDrivingLicense LocalLicenseApp=clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm=new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void LicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_NewLocalLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }
    }
}
