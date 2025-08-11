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
    public partial class frmDetainLicense : Form
    {
        int _LocalDrivingLicenseID = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDrivingLicenseInfoWithFilter11_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter11.txtLicenseIDFocus();
            ctrlDrivingLicenseInfoWithFilter11.OnLicenseSelected += LoadDataForAppInfo;
        }

        private void LoadDataForAppInfo(int LicenseID)
        {
            if (!clsDetainedLicense.IsDetained(LicenseID))
            {
                btnDetain.Enabled = true;
            }
            else
            {
                MessageBox.Show("Selected License already detained, choose another one.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            llShowLicenseHistory.Enabled = true;
            _LocalDrivingLicenseID = LicenseID;
            lblDetainDate.Text = DateTime.Now.ToString();
            lblLicenseID.Text=LicenseID.ToString();
            lblCreatedByUser.Text = ClassGlobal1.CurrentUser.UserName;

            txtFineFees.Focus();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!clsLicenses.FindLicenseInfoByLicenseID(_LocalDrivingLicenseID).IsActive)
            {
                MessageBox.Show("Your local driving license is InActive", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("You should Enter a FineFees", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            clsDetainedLicense detain=new clsDetainedLicense();

            detain.LicenseID = _LocalDrivingLicenseID;
            detain.DetainDate=Convert.ToDateTime( lblDetainDate.Text);
            detain.FineFees=Convert.ToSingle( txtFineFees.Text);
            detain.CreatedByUserID=ClassGlobal1.CurrentUser.UserID;
            detain.IsReleased=false;

            if(detain.Save())
            {
                MessageBox.Show("License Detained Successfully?", "Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblDetainID.Text=detain.DetainID.ToString();

                clsLicenses.UpdateStatusLocalLicenseToActiveOrInActive(_LocalDrivingLicenseID, 0);

                btnDetain.Enabled = false;
                ctrlDrivingLicenseInfoWithFilter11.SetTheFilterDisabled();
                llShowLicenseInfo.Enabled = true;
            }

            else
                MessageBox.Show("Detain was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_LocalDrivingLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowPersonLicenseHisstory frm = new frmShowPersonLicenseHisstory(LocalLicenseApp.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(_LocalDrivingLicenseID);
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseApp.LocalDrivingLicenseID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);
            }
        }
    }
}
