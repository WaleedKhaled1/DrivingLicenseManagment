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
    public partial class frmShowPersonInfo : Form
    {
        int _PersonID = -1;
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void PersonInfo_Load(object sender, EventArgs e)
        {
            ctrlpersoninfo2.LoadPersonInfoByID(_PersonID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlpersoninfo2_Load(object sender, EventArgs e)
        {

        }
    }
}
