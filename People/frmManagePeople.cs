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
    public partial class frmManagePeople : Form
    {
        private static DataTable _dtAllPeople = DVLDBusiness.GetAllPeople();

   

        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "GendorCaption", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");


        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeople()
        {
            _dtAllPeople = DVLDBusiness.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "Gendor", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");
            dataGridView1.DataSource = _dtPeople;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
  
            dataGridView1.DataSource = _dtPeople;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            if (dataGridView1.Rows.Count > 0)
            {

                dataGridView1.Columns[0].HeaderText = "Person ID";
               dataGridView1.Columns[0].Width = 90;
               
               dataGridView1.Columns[1].HeaderText = "National No.";
               dataGridView1.Columns[1].Width = 90;
          
               dataGridView1.Columns[2].HeaderText = "First Name";
               dataGridView1.Columns[2].Width = 90;
            
               dataGridView1.Columns[3].HeaderText = "Second Name";
               dataGridView1.Columns[3].Width = 90;
             
               dataGridView1.Columns[4].HeaderText = "Third Name";
               dataGridView1.Columns[4].Width = 90;
        
               dataGridView1.Columns[5].HeaderText = "Last Name";
               dataGridView1.Columns[5].Width = 90;
      
               dataGridView1.Columns[6].HeaderText = "Gendor";
               dataGridView1.Columns[6].Width = 80;
            
               dataGridView1.Columns[7].HeaderText = "Date Of Birth";
               dataGridView1.Columns[7].Width = 110;
             
               dataGridView1.Columns[8].HeaderText = "Nationality";
               dataGridView1.Columns[8].Width = 90;
            
               dataGridView1.Columns[9].HeaderText = "Phone";
               dataGridView1.Columns[9].Width = 90;
          
               dataGridView1.Columns[10].HeaderText = "Email";
                dataGridView1.Columns[10].Width = 130;
            }



            
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.ShowDialog();
            _RefreshPeople();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedid = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmAddEditPerson frm = new frmAddEditPerson(selectedid);
            frm.ShowDialog();
            _RefreshPeople();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedid = (int)dataGridView1.CurrentRow.Cells[0].Value;
           frmShowPersonInfo personinfo = new frmShowPersonInfo(selectedid);
            personinfo.ShowDialog();
            _RefreshPeople();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dataGridView1.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (DVLDBusiness.DeletePersonByID((int)dataGridView1.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Deleted was Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Person was not deleted because it has data linked to it", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _RefreshPeople();
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text != "None");

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
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "NationalNo.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "Gendor";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

      

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
   
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
