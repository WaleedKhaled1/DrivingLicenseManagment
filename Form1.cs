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
    public partial class Form1 : Form
    {
        frmLoginScreen _frmLogin;
        public Form1(frmLoginScreen frm)
        {
            InitializeComponent();
            _frmLogin =frm; ;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople frm=new frmManagePeople();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers frm=new frmManageUsers();
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassGlobal1.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }

        private void currentUserToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            frmUserInfo frm = new frmUserInfo(ClassGlobal1.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(ClassGlobal1.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();
            frm.ShowDialog();
        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicense frm = new frmAddEditLocalDrivingLicense();
            
            frm.ShowDialog();
            
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplication frm= new frmManageLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers  frm= new frmListDrivers();
            frm.ShowDialog();
        }

        private void inToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewIntrnationalDrivingLicense frm = new frmAddNewIntrnationalDrivingLicense();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageIntrnationalDrivingLicense frm = new frmManageIntrnationalDrivingLicense();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicenseApplication frm = new frmRenewLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void replacmentForLostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacmentForLostOrDamaged frm=new frmReplacmentForLostOrDamaged();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm= new frmDetainLicense();
            frm.ShowDialog();
        }

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm=new frmReleaseDetainedLicense();
            frm.ShowDialog();

        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm=new frmManageDetainedLicenses();
            frm.ShowDialog();
        }

        private void relaiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm= new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplication frm=new frmManageLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }
    }
}
