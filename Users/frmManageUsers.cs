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
    public partial class frmManageUsers : Form
    {

        private static DataTable _dtAllUsers = clsUsers.GetAllUsers();



        private DataTable _dtUsers = _dtAllUsers.DefaultView.ToTable(false, "UserID", "PersonID", "FullName", "UserName", "IsActive");
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void _RefreshUsers()
        {
            _dtAllUsers = clsUsers.GetAllUsers();

            _dtUsers = _dtAllUsers.DefaultView.ToTable(false, "UserID", "PersonID", "FullName","UserName","IsActive");

            dataGridView1.DataSource = _dtUsers;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _dtUsers;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
            cbFilter.SelectedIndex = 0;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "User ID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Person ID";
                dataGridView1.Columns[1].Width = 90;

                dataGridView1.Columns[2].HeaderText = "Full Name";
                dataGridView1.Columns[2].Width = 200;

                dataGridView1.Columns[3].HeaderText = "User Name";
                dataGridView1.Columns[3].Width = 90;

                dataGridView1.Columns[4].HeaderText = "Is Active";
                dataGridView1.Columns[4].Width = 90;
                dataGridView1.Columns[4].ReadOnly = true;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text != "None" && cbFilter.Text!="Active");

            if(cbFilter.Text=="Active")
                {
                cbFilterActive.Visible = true;
                cbFilterActive.Focus();
                }
            else
            cbFilterActive.Visible=false;

            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "User Name":
                    FilterColumn = "UserName";
                    break;


                case "Active":
                    FilterColumn = "IsActive";
                    break;

                default:
                    FilterColumn = "None";
                    break;


            }

         
            
                if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
                {
                    _dtUsers.DefaultView.RowFilter = "";
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                    return;
                }
            

            if (FilterColumn == "PersonID" || FilterColumn == "UserID")
                _dtUsers.DefaultView.RowFilter = $"[{FilterColumn}]={txtFilter.Text}";

            else
                _dtUsers.DefaultView.RowFilter = $"[{FilterColumn}] like '{txtFilter.Text}%'";

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void cbFilterActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterActive.Text == "All")
                _dtUsers.DefaultView.RowFilter = "";
            else if (cbFilterActive.Text == "Yes")
                _dtUsers.DefaultView.RowFilter = $"[IsActive]=true";
            else if (cbFilterActive.Text == "No")
                _dtUsers.DefaultView.RowFilter = $"[IsActive]=false";
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditUsers frm =new frmAddEditUsers();
            frm.ShowDialog();
            _RefreshUsers();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int selectedUserID = (int)dataGridView1.CurrentRow.Cells[0].Value;
           frmAddEditUsers frm =new frmAddEditUsers(selectedUserID);

            frm.ShowDialog();
            _RefreshUsers();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedid = (int)dataGridView1.CurrentRow.Cells[0].Value;

            if (MessageBox.Show("Are you sure you want to delete User [" +selectedid + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                if (clsUsers.DeleteUser(selectedid))
                {
                    MessageBox.Show("Deleted was Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshUsers();
                }
                else
                {
                    MessageBox.Show("User was not deleted because it has data linked to it", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedUserID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmChangePassword frm =new frmChangePassword(selectedUserID);
            frm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void phoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            int selectedUserid = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmUserInfo frm = new frmUserInfo(selectedUserid);
            frm.ShowDialog();
        }
    }
}
