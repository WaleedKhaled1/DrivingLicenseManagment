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
using System.IO;
using System.Windows.Forms;

namespace MyDVLD
{
    public partial class ctrlpersoninfo : UserControl
    {
        private int _PersonID = -1;
        DVLDBusiness obj;
        public int PersonID { get { return _PersonID; } }

        public DVLDBusiness SelectedPersonInfo
        {
            get { return obj; }
        }
        public ctrlpersoninfo()
        {
            InitializeComponent();
        }

        public void LoadPersonInfoByID(int personid)
        {
            _PersonID = personid;

            obj = DVLDBusiness.FindByID(personid);

            if (obj == null)
            {
                MessageBox.Show("The Person was not found!","Wrong",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            lblID.Text = personid.ToString();
            lblName.Text = (obj.FirstName + " " + obj.SecondName + " " + obj.ThirdName + " " + obj.LastName);
            lblNN.Text = obj.NationalNo;
            if (obj.Gendor == 0)
            {
                lblGendor.Text = "Male";
                pictureBoxFemale.Visible = false;
            }
            else
            {
                lblGendor.Text = "Female";
                pictureBoxMale.Visible = false;
            }

            lblEmail.Text = obj.Email;
            lblAddress.Text = obj.Address;

            lblDate.Text = obj.DateOfBirth.ToString();
            lblPhone.Text = obj.Phone;
            lblCountry.Text = DVLDBusiness.GetCountryNameByID(obj.CountryID);

           _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            if (obj.Gendor == 0)
                pbImage.Image = Resources.Male_512;
            else
                pbImage.Image = Resources.Female_512;

            string ImagePath = obj.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public void LoadPersonInfoByNationalNo(string nationalnumber)
        {

            obj = DVLDBusiness.FindByNationalNo(nationalnumber);

            if (obj == null)
            {
                MessageBox.Show("The Person was not found!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
                return;
            }
            _PersonID = obj.PersonID;

            lblID.Text =obj.PersonID.ToString();
            lblName.Text = (obj.FirstName + " " + obj.SecondName + " " + obj.ThirdName + " " + obj.LastName);
            lblNN.Text = obj.NationalNo;
            if (obj.Gendor == 0)
            {
                lblGendor.Text = "Male";
                pictureBoxFemale.Visible = false;
            }
            else
            {
                lblGendor.Text = "Female";
                pictureBoxMale.Visible = false;
            }

            lblEmail.Text = obj.Email;
            lblAddress.Text = obj.Address;

            lblDate.Text = obj.DateOfBirth.ToString();
            lblPhone.Text = obj.Phone;
            lblCountry.Text = DVLDBusiness.GetCountryNameByID(obj.CountryID);
            _LoadPersonImage();
        }

        private void llEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson frm=new frmAddEditPerson(_PersonID);
            frm.ShowDialog();
           LoadPersonInfoByID(_PersonID);
          
        }


    }
}
