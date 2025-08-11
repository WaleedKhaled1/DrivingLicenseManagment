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
    public partial class FrmUpdateApplicationType : Form
    {
        private int _AppTypeID=-1;
        clsApplicationTypes obj;
        public FrmUpdateApplicationType(int ID)
        {
            InitializeComponent();
            _AppTypeID = ID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            obj=clsApplicationTypes.FindApplicationTypeInfo(_AppTypeID);

            if (obj != null)
            {
                lblID.Text = obj.AppTypeID.ToString();
                txtTitle.Text = obj.AppTitle.ToString();

                txtFees.Text = obj.AppTypeFees.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            obj.AppTitle=txtTitle.Text;
            obj.AppTypeFees=((float)Convert.ToDouble(txtFees.Text));

            if (obj.UpdateApplicationType())
            {
                MessageBox.Show("Updated Was Successfully","Updated",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Update Was failed", "failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
