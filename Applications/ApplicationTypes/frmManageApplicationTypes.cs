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
    public partial class frmManageApplicationTypes : Form
    {
        private static DataTable _dtApp;
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtApp = clsApplicationTypes.GetAllApplicationTypes();
            dataGridView1.DataSource = _dtApp;

            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            if (dataGridView1.Rows.Count > 0)
            {

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Title";
                dataGridView1.Columns[1].Width = 220;

                dataGridView1.Columns[2].HeaderText = "Fees";
                dataGridView1.Columns[2].Width = 90;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUpdateApplicationType frm =new FrmUpdateApplicationType((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmApplicationTypes_Load(null, null);
        }
    }
}
