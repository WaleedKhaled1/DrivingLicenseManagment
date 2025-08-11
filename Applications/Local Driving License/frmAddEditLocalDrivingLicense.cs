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
using static MyDVLD.ClassGlobal1;

namespace MyDVLD
{
    public partial class frmAddEditLocalDrivingLicense : Form
    {
        public enum enMode { AddMode = 0, UpdateMode = 1 }
        enMode _Mode = enMode.AddMode;
        int _LocalDrivingLicenseID = -1;
        clsLocalDrivingLicense obj;
        clsUsers User;
        public frmAddEditLocalDrivingLicense()
        {
            InitializeComponent();
            _Mode = enMode.AddMode;
        }

        public frmAddEditLocalDrivingLicense(int LocalDrivingLicenseID)
        {
            InitializeComponent();
            _Mode = enMode.UpdateMode;
            _LocalDrivingLicenseID=LocalDrivingLicenseID;
        }

        private void _ResetDefualtValues()
        {
            DataTable dt = clsLicenseClass.GetAllClasses();
            cbClasses.DataSource = dt;
            cbClasses.DisplayMember = "ClassName";
            cbClasses.ValueMember = "LicenseClassID";

            if (_Mode == enMode.AddMode)
            {

                lblMode.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                obj = new clsLocalDrivingLicense();
                ctrlPersonInfoWithFilter1.FilterFocus();
                tabApplication.Enabled = false;

                cbClasses.SelectedIndex = 2;
                lblFees.Text = clsApplicationTypes.FindApplicationTypeInfo((int)clsApplications.enApplicationType.NewDrivingLicense).AppTypeFees.ToString();
                lblDate.Text = DateTime.Now.ToShortDateString();
                lblUser.Text = ClassGlobal1.CurrentUser.UserName;
            }

            else
            {
                lblMode.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tabApplication.Enabled = true;
                btnSave.Enabled = true;


            }
        }

        public void LoadData()
        { 

            ctrlPersonInfoWithFilter1.FilterEnabled = false;

            obj = clsLocalDrivingLicense.FindLocalDrivingLicenseInfoByID(_LocalDrivingLicenseID);
            ctrlPersonInfoWithFilter1.LoadData(obj.ApplicantPersonID);

            lblID.Text = _LocalDrivingLicenseID.ToString();
            lblDate.Text = obj.ApplicationDate.ToString();
            cbClasses.SelectedValue=obj.LocalLicenseClassID;
            lblFees.Text = obj.PaidFees.ToString();
            lblUser.Text=obj.CreatedByUserID.ToString();

        }

        private void frmNewLocalDrivingApplication_Load(object sender, EventArgs e)
        {
           _ResetDefualtValues();

                if (_Mode == enMode.UpdateMode)
                {
                    LoadData();
                }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int SelectedClassIndex = clsLicenseClass.FindLicenseClassInfoByName(cbClasses.Text).LicenseClassID;

            int AppIDForNew = clsApplications.GetActiveApplicationIDForLicenseClass(ctrlPersonInfoWithFilter1.PersonID, (int)clsApplications.enApplicationType.NewDrivingLicense,SelectedClassIndex);

                if (AppIDForNew!=-1)
                {
                    MessageBox.Show("Choose another LicenseClass,The selected person Already\nHave an active application for the selected class with id=" + AppIDForNew,
                    "Wrong",MessageBoxButtons.OK,MessageBoxIcon.Error);
                cbClasses.Focus();
                        return;
                }


                if (clsLicenses.GetActiveLicenseIDByPersonID(ctrlPersonInfoWithFilter1.PersonID, SelectedClassIndex)!=-1)
                {
                MessageBox.Show("Person already have a license with the same applied driving class ,choose different driving class","Wrong",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                        return;
                }

            obj.LocalLicenseClassID = SelectedClassIndex;
            obj.ApplicantPersonID=ctrlPersonInfoWithFilter1.PersonID;
            obj.ApplicationDate=Convert.ToDateTime( lblDate.Text);
            obj.ApplicationTypeID =(int) clsApplications.enApplicationType.NewDrivingLicense;
            obj.ApplicationStatus=clsApplications.enStatus.New;
            obj.ApplicationLastStatusDate=DateTime.Now;
            obj.PaidFees=((float)Convert.ToDouble( lblFees.Text));
            

            User=clsUsers.FindUserByUserNameAndPassword(CurrentUser.UserName, CurrentUser.Password);
           obj.CreatedByUserID =User.UserID;
           

            if(obj.Save())
            {
                MessageBox.Show("Data Saved Successfully","Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Save was Failed", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.UpdateMode)
            {
                btnSave.Enabled = true;
                tabApplication.Enabled = true;
                tpcPersonInfo.SelectedIndex = 1;
                return;
            }   

            if (ctrlPersonInfoWithFilter1.PersonID != -1)
            {
                tabApplication.Enabled = true;
                tpcPersonInfo.SelectedIndex = 1;
            }

            else
            {

                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonInfoWithFilter1.FilterFocus();
            }
        }

        private void ctrlPersonInfoWithFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}
