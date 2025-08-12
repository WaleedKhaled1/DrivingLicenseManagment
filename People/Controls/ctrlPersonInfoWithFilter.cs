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
    public partial class ctrlPersonInfoWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;

        string ShowBy = "";

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                button2.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                groupBoxFilter.Enabled = _FilterEnabled;
            }
        }
    
        public clsPeople SelectedPersonInfo
        {
            get { return ctrlpersoninfo1.SelectedPersonInfo; }
        }


        public ctrlPersonInfoWithFilter()
        {
            InitializeComponent();
        }

        private int _PersonID = -1;

        public int PersonID
        {
            get { return ctrlpersoninfo1.PersonID; }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            switch (cbFilter.Text)
            {
                case "Person ID":
                    {
                        ctrlpersoninfo1.LoadPersonInfoByID(Convert.ToInt32(txtFilter.Text));
                        
                        OnPersonSelected?.Invoke(ctrlpersoninfo1.PersonID);
                        break;
                    }

                case "National No.":
                    {
                            ctrlpersoninfo1.LoadPersonInfoByNationalNo(txtFilter.Text);
                        OnPersonSelected?.Invoke(ctrlpersoninfo1.PersonID);
                        break;
                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.OnPersonAdded += LoadData;
            frm.ShowDialog();

        }

        public void DisapleFilter()
        {
            groupBoxFilter.Enabled = false;
        }

        public void FilterFocus()
        {
            txtFilter.Focus();
        }

        public void LoadData(int PersonID)
        {
            txtFilter.Text=PersonID.ToString();
            ctrlpersoninfo1.LoadPersonInfoByID((PersonID));
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1.PerformClick();
            }

            if (cbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ctrlPersonInfoWithFilter_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 1;
            txtFilter.Focus();
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilter, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilter, null);
            }
        }
    }
}
