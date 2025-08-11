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
    public partial class ctrlDrivingInternationalLicenseInfo : UserControl
    {
        public ctrlDrivingInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadDataByLocalLicenseID(int LocalLicenseID)
        {
            clsLicenses license=clsLicenses.FindLicenseInfoByLicenseID(LocalLicenseID);
            clsInternationalDrivingLicensens obj=clsInternationalDrivingLicensens.FindInternationalLicenseInfoByDriverID(license.DriverID);

            lblFullName.Text=obj.PersonInfo.FullName;
            lblInternationalLicenseID.Text=obj.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text=obj.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text=obj.PersonInfo.NationalNo.ToString();
            if (obj.PersonInfo.Gendor == 0)
            {
                lblGendor.Text = "Male";
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                lblGendor.Text = "Female";
                pbPersonImage.Image = Resources.Female_512;
            }

            lblIssueDate.Text=obj.IssueDate.ToString(); 
            lblApplicationID.Text=obj.ApplicationID.ToString();
            if (obj.ExpirationDate <= DateTime.Now)
            {
                clsInternationalDrivingLicensens.UpdateStatusInternationalLicenseToInActive(obj.InternationalLicenseID);
                lblIsActive.Text = "No";
            }
            else
                lblIsActive.Text = "Yes";

            lblDateOfBirth.Text=obj.PersonInfo.DateOfBirth.ToString();
            lblDriverID.Text=obj.DriverID.ToString();
            lblExpirationDate.Text=obj.ExpirationDate.ToString();

            if(obj.PersonInfo.ImagePath!=null)
            pbPersonImage.ImageLocation= obj.PersonInfo.ImagePath;

        }


        public void LoadDataByIntrnationalLicenseID(int InternationalLicenseID)
        {
            clsInternationalDrivingLicensens obj = clsInternationalDrivingLicensens.FindInternationalLicenseInfoByInternationalLicenseID(InternationalLicenseID);

            if(obj != null)
            {
                LoadDataByLocalLicenseID(obj.IssuedUsingLocalLicenseID);
            }

        }
    }
}
