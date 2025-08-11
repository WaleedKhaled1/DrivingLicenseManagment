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
    public partial class frmReplacmentForLostOrDamaged : Form
    {
        int _OldLocalLicenseID = -1;
        int _NewLocalLicenseID = -1;
        public frmReplacmentForLostOrDamaged()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReplacmentForLostOrDamaged_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter11.txtLicenseIDFocus();
            ctrlDrivingLicenseInfoWithFilter11.OnLicenseSelected += LoadReplacmentApplicationLicenseInfo;
        }

        private void LoadReplacmentApplicationLicenseInfo(int _LocalLicenseID)
        {
            llShowLicenseHistory.Enabled = true;
            this._OldLocalLicenseID = _LocalLicenseID;

            lblApplicationDate.Text = DateTime.Now.ToString();
            lblApplicationFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense).AppTypeFees.ToString();

            lblOldLicenseID.Text = _LocalLicenseID.ToString();
            lblCreatedByUser.Text = ClassGlobal1.CurrentUser.UserName;

            IsLicenseInActive();
        }

        private void IsLicenseInActive()
        {
            if (ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo != null)
            {
                if (!ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.IsActive)
                {
                    MessageBox.Show("Selected License Is Not Active, choose an active license.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnIssueReplacement.Enabled = false;
                  
                }

                else if(ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo.ExpirationDate<=DateTime.Now)
                {
                    MessageBox.Show("Selected License Is Expired you Should Renew it", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnIssueReplacement.Enabled = false;
                }

                else
                {
                    btnIssueReplacement.Enabled = true;
                }
            }
            else
                MessageBox.Show("Selected License Was Not Found.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue a Replacment for the license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLicenses OldLicense = ctrlDrivingLicenseInfoWithFilter11.SelectedLicenseInfo;

            clsLocalDrivingLicense obj = new clsLocalDrivingLicense();
            obj.LocalLicenseClassID = OldLicense.LicenseClass;

            obj.ApplicantPersonID = clsLicenses.FindLicenseInfoByLicenseID(_OldLocalLicenseID).PersonID;
            obj.ApplicationDate = Convert.ToDateTime(lblApplicationDate.Text);

            if(rbDamagedLicense.Checked)
            obj.ApplicationTypeID = (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                obj.ApplicationTypeID = (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense;

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
            license.Notes = "";


            license.PaidFees = Convert.ToSingle(OldLicense.PaidFees);
            license.IsActive = true;

            if(rbDamagedLicense.Checked)
            license.IssueReason = clsLicenses.enIssue.ReplacmentForDamaged;
            else
                license.IssueReason = clsLicenses.enIssue.ReplacmentForLost;

            license.CreatedByUserID = ClassGlobal1.CurrentUser.UserID;

            license.PersonID = OldLicense.PersonID;

            if (license.Save())
            {
                _NewLocalLicenseID = license.LicenseID;
                MessageBox.Show("Licese Was Replaced Successfully with ID=" + _NewLocalLicenseID, "Licese Replaced", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssueReplacement.Enabled = false;
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();

                clsLicenses.UpdateStatusLocalLicenseToActiveOrInActive(_OldLocalLicenseID,0);

                lblRreplacedLicenseID.Text = license.LicenseID.ToString();
                llShowLicenseInfo.Enabled = true;
            }

            else
            {
                MessageBox.Show("Licese Replacemnet was Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_OldLocalLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm =new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense).AppTypeFees.ToString();
            lblTitle.Text = "Replacment For Damaged License";
            this.Text = lblTitle.Text;
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.ReplaceLostDrivingLicense).AppTypeFees.ToString();
            lblTitle.Text = "Replacment For Lost License";
            this.Text=lblTitle.Text;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_NewLocalLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }
    }
}
