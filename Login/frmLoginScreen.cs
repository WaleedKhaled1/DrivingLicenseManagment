using BusinessLayer;
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
    public partial class frmLoginScreen : Form
    {
 
        string FilePath = "C:\\License\\Users.txt";        
        public frmLoginScreen()
        { 
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool IsActive = true;
          
           
            if (clsUsers.IsUserExistAndActive(txtUser.Text, txtPass.Text, ref IsActive))
            {
                if (IsActive)
                {
                    if (checkBox1.Checked)
                    {
                        FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(fs);
                        {
                            writer.WriteLine($"{txtUser.Text},{txtPass.Text}");
                            writer.Close();
                        }

                   
                    }
                    clsUsers obj = clsUsers.FindUserByUserNameAndPassword(txtUser.Text, txtPass.Text);
                  
                    ClassGlobal1.CurrentUser = obj;

                    Form1 frm = new Form1(this);
                    frm.ShowDialog();

                }
                else
                {
                    MessageBox.Show("User is deactivated, please contact admin!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid Username/Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            if (File.Exists(FilePath))
            {
                StreamReader reader = new StreamReader(FilePath);
                string line = reader.ReadLine();
                string[] Parts = { };
                if (line != null)
                {
                    Parts = line.Split(',');
                    txtUser.Text = Parts[0];
                    txtPass.Text = Parts[1];
                }

                reader.Close();

              
            }
            
        }
    }
}
