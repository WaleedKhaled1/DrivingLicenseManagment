using BusinessLayer;
using MyDVLD.Properties;
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
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        int _LicenseID = -1;
        clsLicenses licenses;
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicenses SelectedLicenseInfo
        { get { return licenses; } }

        public void LoadData(int LocalDrivingLicenseAppID)
        {
            clsLocalDrivingLicense  obj = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(LocalDrivingLicenseAppID);
             licenses = clsLicenses.FindLicenseInfoByAppID(obj.ApplicationID);
            _LicenseID=licenses.LicenseID;


                lblClass.Text = clsLicenseClass.FindLicenseClassInfoByID(obj.LocalLicenseClassID).ClassName;
            lblFullName.Text = obj.PersonInfo.FullName;
            lblLicenseID.Text = licenses.LicenseID.ToString();
            lblNationalNo.Text = obj.PersonInfo.NationalNo;
            if (obj.PersonInfo.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            lblIssueDate.Text = licenses.IssueDate.ToString();
            lblExpirationDate.Text = licenses.ExpirationDate.ToString();
            lblIssueReason.Text = licenses.IssueReasonText;

            if (licenses.Notes!= "")
                lblNotes.Text = licenses.Notes;
            else
                lblNotes.Text = "No Notes";
            

            if (licenses.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

            lblDateOfBirth.Text = obj.PersonInfo.DateOfBirth.ToString();
            lblDriverID.Text = licenses.DriverID.ToString();

             LoadIsDetainedStatus(_LicenseID);
          
            if (!string.IsNullOrWhiteSpace(obj.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = obj.PersonInfo.ImagePath;
            }
            else
            {
                if (obj.PersonInfo.Gendor == 0)
                    pbPersonImage.Image = Resources.Male_512;
                else
                    pbPersonImage.Image = Resources.Female_512;

            }


        }

        public bool LoadIsDetainedStatus(int LicenseID)
        {
            if(clsDetainedLicense.IsDetained(LicenseID))
            {
                lblIsDetained.Text ="Yes";
                return true;
            }

            else
            {
                lblIsDetained.Text = "No";
                return false;
            }
        }


    }
}
