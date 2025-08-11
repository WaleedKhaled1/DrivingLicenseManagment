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
    public partial class frmChangePassword : Form
    {
        int _UserID = -1;
        clsUsers obj;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

       

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            obj=clsUsers.FindUser(_UserID);

            txtConfirm.PasswordChar = '*';
            txtCurrent.PasswordChar = '*';
            txtNew.PasswordChar = '*';

            ctrlUserInfo1.LoadUserInfo(_UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrent_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrent.Text) || txtCurrent.Text != obj.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrent, "Current Password is wrong!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrent, "");
            }
        }

        private void txtNew_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNew.Text)|| txtNew.Text==obj.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNew, "New Password cannot be blank");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNew, "");
            }
        }

        private void txtConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNew.Text)||txtNew.Text!=txtConfirm.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirm, "Confirm doesnt match with password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirm, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Invalid Information!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            obj.Password = txtNew.Text;

            if (obj.Save())
            {
                MessageBox.Show("Password Changed Successfully.",
                   "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("An Erro Occured, Password did not change.",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
