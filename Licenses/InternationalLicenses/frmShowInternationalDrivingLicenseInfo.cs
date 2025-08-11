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
    public partial class frmShowInternationalDrivingLicenseInfo : Form
    {
        int _InternationalDrivingLicenseID = -1;
        public frmShowInternationalDrivingLicenseInfo(int internationalID)
        {
            InitializeComponent();
            _InternationalDrivingLicenseID=internationalID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmShowInternationalDrivingLicenseInfo_Load(object sender, EventArgs e)
        {
                ctrlDrivingInternationalLicenseInfo1.LoadDataByIntrnationalLicenseID(_InternationalDrivingLicenseID);
        }
    }
}
