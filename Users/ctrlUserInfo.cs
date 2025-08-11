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
    public partial class ctrlUserInfo : UserControl
    {
        int _UserID = -1;
        clsUsers obj;

        public int UserID { get { return _UserID; } }
        public ctrlUserInfo()
        {
            InitializeComponent();

        }

        public void LoadUserInfo(int userid)
        {
            obj = clsUsers.FindUser(userid);
            if (obj == null)
            {
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
                _UserID = obj.UserID;
                lblID.Text = obj.UserID.ToString();
                lblName.Text = obj.UserName;

            lblActive.Text = Convert.ToBoolean(obj.IsActive) ? "Yes" : "No";

            ctrlpersoninfo1.LoadPersonInfoByID(obj.PersonID);
           
        }

  
    }
}
