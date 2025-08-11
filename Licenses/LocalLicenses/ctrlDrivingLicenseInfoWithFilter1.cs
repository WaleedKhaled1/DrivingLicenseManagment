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
    public partial class ctrlDrivingLicenseInfoWithFilter1 : UserControl
    {
        public event Action<int> OnLicenseSelected;

        public ctrlDrivingLicenseInfoWithFilter1()
        {
            InitializeComponent();
        }


        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo2.LicenseID; }
        }

        public clsLicenses SelectedLicenseInfo
        { get { return ctrlDriverLicenseInfo2.SelectedLicenseInfo; } }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            LoadData(Convert.ToInt32(txtLicenseID.Text));
        }

        public void LoadData(int LicenseID)
        {

            txtLicenseID.Text = LicenseID.ToString();

            clsLicenses Locallicense = clsLicenses.FindLicenseInfoByLicenseID(Convert.ToInt32(txtLicenseID.Text));
            if(Locallicense==null)
            {
                MessageBox.Show("License Was not Found!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsLocalDrivingLicense LocalLicenseApp = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByAppID(Locallicense.ApplicationID);


            ctrlDriverLicenseInfo2.LoadData(LocalLicenseApp.LocalDrivingLicenseID);
            OnLicenseSelected?.Invoke(Convert.ToInt32(txtLicenseID.Text));

        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }
        public void SetTheFilterDisabled()
        {
            gbFilters.Enabled = false;
        }


    }
}
