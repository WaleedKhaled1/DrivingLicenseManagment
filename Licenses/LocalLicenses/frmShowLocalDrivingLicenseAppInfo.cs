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
    public partial class frmShowLocalDrivingLicenseAppInfo : Form
    {
        int LocalDrivingLicenseApplicationID=-1;
        public frmShowLocalDrivingLicenseAppInfo(int localDrivingLicenseApplicationid)
        {
            InitializeComponent();
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationid;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowLocalDrivingLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadLocalLicenseAndApplicationInfoForTest(LocalDrivingLicenseApplicationID);
        }
    }
}
