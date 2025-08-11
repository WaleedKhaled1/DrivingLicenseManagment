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
using static BusinessLayer.clsTestTypes;

namespace MyDVLD
{
    public partial class frmUpdateTestType : Form
    {
        public clsTestTypes. enTestType _TestTypeID = enTestType.VisionTest;

        clsTestTypes obj;
        public frmUpdateTestType(enTestType ID)
        {
            InitializeComponent();
            _TestTypeID = ID;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            obj = clsTestTypes.FindTestTypeInfo(_TestTypeID);

            if (obj != null)
            {
                lblID.Text = obj.TestTypeID.ToString();
                txtTitle.Text = obj.TestTypeTitle.ToString();
                txtDescription.Text = obj.TestTypeDescription.ToString();
                txtFees.Text = obj.TestTypeFees.ToString();
            }
            else
                MessageBox.Show("Not Found!");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if(!this.ValidateChildren())
            {
                if (!this.ValidateChildren())
                {
                    MessageBox.Show("Invalid Information!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            obj.TestTypeTitle = txtTitle.Text;
            obj.TestTypeFees = ((float)Convert.ToDouble(txtFees.Text));
            obj.TestTypeDescription=txtDescription.Text;

            if (obj.UpdateTestType())
            {
                MessageBox.Show("Updated Was Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtDescription, null);
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
