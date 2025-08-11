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
    public partial class frmAddEditUsers : Form
    {
        public enum enModeUsers {AddMode=0,UpdateMode=1}

        private enModeUsers Mode;

        private int _UserID = -1;
        clsUsers _User;
        public frmAddEditUsers()
        {
            InitializeComponent();
            Mode = enModeUsers.AddMode;
        }

        public frmAddEditUsers(int UserID)
        {
            InitializeComponent();

            Mode = enModeUsers.UpdateMode;
            _UserID = UserID;
        }

        private void _ResetDefualtValues()
        {
         

            if (Mode == enModeUsers.AddMode)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUsers();

               tpLogin.Enabled = false;
                

            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                btnSave.Enabled = true;


            }

            txtName.Text = "";
            txtPass.Text = "";
            txtConfirm.Text = "";
            checkBox1.Checked = true;
        }

        private void _LoadData()
        {

            _User = clsUsers.FindUser(_UserID);
            ctrlPersonInfoWithFilter1.FilterEnabled=false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the person was not found
            lblID.Text = _User.UserID.ToString();
            txtName.Text = _User.UserName;
            txtPass.Text = _User.Password;
            txtConfirm.Text = _User.Password;
            checkBox1.Checked = _User.IsActive;
            ctrlPersonInfoWithFilter1.LoadData(_User.PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            string userName = txtName.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtName, "UserName cannot be blank!");
                return;
            }

            if (Mode == enModeUsers.AddMode)
            {
                if (clsUsers.isUserExist(userName))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtName, "Username is already used!");
                    return;
                }
            }
            else // Update mode
            {
                if (_User.UserName != userName && clsUsers.isUserExist(userName))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtName, "Username is used by another user");
                    return;
                }
            }

            e.Cancel = false; 
            errorProvider1.SetError(txtName, null);
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPass, "Password cannot be blank!");
                return;
            }
            e.Cancel = false;
            errorProvider1.SetError(txtPass, null);
        }

        private void txtConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirm.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirm, "Confirm Password cannot be blank!");
                return;
            }

            if (txtPass.Text != txtConfirm.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirm, "Confirm Password doesn't match Password!");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(txtConfirm, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsActive=false;

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Invalid Information!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _User.PersonID = ctrlPersonInfoWithFilter1.PersonID;
            _User.UserName = txtName.Text.Trim();
            _User.Password = txtPass.Text.Trim();
            _User.IsActive = checkBox1.Checked;

            if (_User.Save())
            {
                lblID.Text = _User.UserID.ToString();
                Mode = enModeUsers.UpdateMode;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void frmAdd_New_User_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (Mode == enModeUsers.UpdateMode)
                _LoadData();    

            txtConfirm.PasswordChar = '*';
            txtPass.PasswordChar = '*';
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonInfoWithFilter1.PersonID != -1)
            {
                if (Mode == enModeUsers.AddMode)
                {
                    if (clsUsers.IsPersonUser(ctrlPersonInfoWithFilter1.PersonID))
                    {
                        MessageBox.Show("Selected Person already is a user!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tpLogin.Enabled = false;
                        ctrlPersonInfoWithFilter1.FilterFocus();
                    }

                    else
                    {
                        tpLogin.Enabled = true;
                        tabControlPerson.SelectedIndex = 1;
                    }
                }


                else
                {
                    tabControlPerson.SelectedIndex = 1;
                }
            }

            else
            {

                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonInfoWithFilter1.FilterFocus();
            }

        }
    }
}
