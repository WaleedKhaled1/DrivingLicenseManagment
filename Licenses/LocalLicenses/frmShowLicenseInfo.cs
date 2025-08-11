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
    public partial class frmShowLicenseInfo : Form
    {
        int _LocalDrivingLicenseAppID = -1;
        public frmShowLicenseInfo(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            _LocalDrivingLicenseAppID = localDrivingLicenseAppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadData(_LocalDrivingLicenseAppID);
        }
    }
}
