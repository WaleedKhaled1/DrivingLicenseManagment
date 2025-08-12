using BusinessLayer;
using MyDVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MyDVLD
{

    public partial class frmAddEditPerson : Form
    {

        int _PersonID;

        public delegate void PersonAddedHandler(int personID);
        public event PersonAddedHandler OnPersonAdded;
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        public enum enMode { AddMode = 0, UpdateMode = 1 };
        enMode Mode = enMode.AddMode;

        clsPeople obj;

        public void LoadPerson()
        {
            

            if (_PersonID == -1)
                Mode = enMode.AddMode;
            else
                Mode = enMode.UpdateMode;

            LoadDefaultData();
            LoadPersonData();

        }

        private void LoadDefaultData()  
        {
            DateTime MaxDate = DateTime.Today.AddYears(-18);

            dateTimePicker1.Value = MaxDate;
            dateTimePicker1.MaxDate = MaxDate;
            dateTimePicker1.MinDate = DateTime.Today.AddYears(-100);

            DataTable dt = clsPeople.GetAllCountries();

            cbCountry.DataSource = dt;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
            cbCountry.SelectedValue = 90;

        }

        private void LoadPersonData()
        {


            if (Mode == enMode.AddMode)
            {
                obj = new clsPeople();
                lblForm.Text = "Add New Person";
                return; 
            }


            obj = clsPeople.FindByID(_PersonID);


            if (obj == null)
            {
                MessageBox.Show("Person Is not Found", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblForm.Text = "Update Person";
            lblID.Text = _PersonID.ToString();
            txtFirst.Text = obj.FirstName;
            txtSecond.Text = obj.SecondName;
            if (obj.ThirdName != null)
                txtThird.Text = obj.ThirdName;
            txtLast.Text = obj.LastName;
            txtNN.Text = obj.NationalNo;
            if (obj.Gendor == 0)
                rbMale.Select();
            else
                rbFemale.Select();

            txtEmail.Text = obj.Email;
            txtAddress.Text = obj.Address;
            dateTimePicker1.Value = obj.DateOfBirth;
            textPhone.Text = obj.Phone;
            cbCountry.SelectedValue = obj.CountryID;

            if (obj.ImagePath != "")
            {
                pbImage.ImageLocation = obj.ImagePath;
            } 

            else
            {
                pbImage.ImageLocation = null;
            }



        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbImage.Load(selectedFilePath);
                llRemove.Visible = true;
                // ...
            }
        }

        private void llRemove_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbImage.Image = null;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider.SetError(Temp, null);
            }

        }

        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {
            LoadPerson(); //IMPORTANT:YOU SHOULD SENT THE PERSONID!!!!!!!!
        }

        private bool _HandlePersonImage()
        {


            if (obj.ImagePath != pbImage.ImageLocation)
            {
                if (obj.ImagePath != "")
                {

                    try
                    {
                        File.Delete(obj.ImagePath);
                    }
                    catch (IOException)
                    {
                       
                    }
                }
            }

                if (pbImage.ImageLocation != null)
                {
                    string SourceImageFile = pbImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!_HandlePersonImage())
                return;

            obj.NationalNo = txtNN.Text;
            obj.FirstName = txtFirst.Text;
            obj.SecondName = txtSecond.Text;

            if (obj.Email != null)
            {
                obj.Email = txtEmail.Text;
            }
            else
            {
                obj.Email = "";
            }

            if (obj.Email != null)
            {
                obj.ThirdName = txtThird.Text;
            }
            else
            {
                obj.ThirdName = "";
            }

            obj.LastName = txtLast.Text;
            obj.DateOfBirth = dateTimePicker1.Value;
            obj.CountryID = (int)cbCountry.SelectedValue;

            if (rbFemale.Checked)
                obj.Gendor = 1;
            else
                obj.Gendor = 0;

            obj.Address = txtAddress.Text;
            obj.Phone = textPhone.Text;

            if (pbImage.ImageLocation != null)
            {
                obj.ImagePath = pbImage.ImageLocation;
            }
            else
            {
                obj.ImagePath = "";
            }

            if (obj.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
              

                lblForm.Text = "Upadate Person";
                lblID.Text = obj.PersonID.ToString();
                OnPersonAdded?.Invoke(obj.PersonID);
            }
            else
            {
                MessageBox.Show("Save Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        
        }

        private void txtNN_Validating_1(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNN.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtNN, "This field is required!");
                return;
            }
            else
            {
                errorProvider.SetError(txtNN, null);
            }

            if (txtNN.Text.Trim() != obj.NationalNo && clsPeople.IsExist(txtNN.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtNN, "National Number is used for another person!");

            }
            else
            {
                errorProvider.SetError(txtNN, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains(".com"))
                {
                    e.Cancel=true;
                    errorProvider.SetError(txtEmail, "Invalid Email address format!");
                    return;
                }
            }

            errorProvider.SetError(txtEmail, "");
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
                pbImage.Image = Resources.Male_512;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
                pbImage.Image = Resources.Female_512;
        }

    }

    }

