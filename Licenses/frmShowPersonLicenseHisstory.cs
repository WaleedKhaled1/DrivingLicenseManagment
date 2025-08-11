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
    public partial class frmShowPersonLicenseHisstory : Form
    {
        int _PersonID = -1;

        public frmShowPersonLicenseHisstory(int personID)
        {
            InitializeComponent();
            _PersonID=personID;
        }

        private void frmShowPersonLicenseHisstory_Load(object sender, EventArgs e)
        {
            if (_PersonID != -1)
            {
                ctrlPersonInfoWithFilter1.LoadData(_PersonID);
                ctrlPersonInfoWithFilter1.FilterEnabled = false;

                ctrlDrivingLicenses1.LoadData(_PersonID);
            }

            else
            {
                ctrlPersonInfoWithFilter1.FilterEnabled = true;
                ctrlPersonInfoWithFilter1.FilterFocus();
                ctrlPersonInfoWithFilter1.OnPersonSelected += LoadLicensesByFindPerson;
            }

        }

        private void LoadLicensesByFindPerson(int personID)
        {
            _PersonID = personID;

            if (_PersonID != -1)
            {
                ctrlDrivingLicenses1.LoadData(_PersonID);
            }
               
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
