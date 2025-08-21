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
using Microsoft.Win32;
using System.Diagnostics;

namespace MyDVLD
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        { 
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool IsActive = true;

            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\LOGIN";
            string valueName = "VALUE NAME";

            string SourceNameInEventViewer = "Login";

            if (clsUsers.IsUserExistAndActive(txtUser.Text, txtPass.Text, ref IsActive))
            {
                if (IsActive)
                {
                    if (checkBox1.Checked)
                    {
                        try
                        {
                            Registry.SetValue(KeyPath,valueName,txtUser.Text+","+txtPass.Text, RegistryValueKind.String);
                        }

                        catch (Exception ex)
                        {
                            if (!EventLog.SourceExists(SourceNameInEventViewer))
                            {
                                EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                            }

                            EventLog.WriteEntry(SourceNameInEventViewer,ex.Message, EventLogEntryType.Error);

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

                string line = Registry.GetValue(KeyPath,valueName,null)as string;
                string[] Parts = { };
                if (line != null)
                {
                    Parts = line.Split(',');
                    txtUser.Text = Parts[0];
                    txtPass.Text = Parts[1];
                }

        }
    }
}
