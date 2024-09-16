using System;
using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class FormLabs : Form
    {
        public FormLabs()
        {
            InitializeComponent();
        }
        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select LabID[ID], LabTitle[Lab], Capacity , IsActive[Status] from LabTable";
                }
                else
                {
                    query = "select LabID[ID], LabTitle[Lab],Capacity, IsActive[Status] from LabTable where LabTitle like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Lablist = DataBase_Layer.Retrive(query);
                dgvLabs.DataSource = Lablist;
                if (dgvLabs.Rows.Count > 0)
                {
                    dgvLabs.Columns[0].Width = 80;
                    dgvLabs.Columns[1].Width = 150;
                    dgvLabs.Columns[2].Width = 100;
                    dgvLabs.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }
        private void FormLabs_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtLabTitle.Text.Length == 0)
            {
                ep.SetError(txtLabTitle, "Please Enter Correct Lab Title!");
                txtLabTitle.Focus();
                txtLabTitle.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length==0)
            {
                ep.SetError(txtCapacity, "Please Enter Lab Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from LabTable where LabTitle = '" + txtLabTitle.Text.ToUpper().Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabTitle, "Already Exist!");
                    txtLabTitle.Focus();
                    txtLabTitle.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into LabTable(LabTitle , Capacity, IsActive) values ('{0}','{1}','{2}')",
                                    txtLabTitle.Text.ToUpper().Trim(), txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Lab details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtLabTitle.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvLabs.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvLabs.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvLabs != null)
            {
                if (dgvLabs.Rows.Count > 0)
                {
                    if (dgvLabs.SelectedRows.Count == 1)
                    {
                        txtLabTitle.Text = Convert.ToString(dgvLabs.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvLabs.CurrentRow.Cells[2].Value);
                        EnableComponents();
                    }
                    else
                    {
                        MessageBox.Show("Please Select One Record!");
                    }

                }
                else
                {
                    MessageBox.Show("List Is Empty!");
                }
            }
            else
            {
                MessageBox.Show("List Is Empty!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtLabTitle.Text.Length == 0)
            {
                ep.SetError(txtLabTitle, "Please Enter Lab Title!");
                txtLabTitle.Focus();
                txtLabTitle.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from LabTable where LabTitle = '" + txtLabTitle.Text.ToUpper().Trim() + "' and LabID != '" + Convert.ToString(dgvLabs.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabTitle, "Already Exist!");
                    txtLabTitle.Focus();
                    txtLabTitle.SelectAll();
                    return;
                }
            }
            string updatequery = string.Format("update LabTable set LabTitle = '{0}',Capacity = '{1}',IsActive = '{2}' where LabID = '{3}'",
                                   txtLabTitle.Text.ToUpper().Trim(), chkStatus.Checked, Convert.ToString(dgvLabs.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Lab details. then try again!");
            }
        }

        private void txtCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)&& !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
