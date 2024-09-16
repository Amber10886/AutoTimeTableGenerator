using System;
using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class FormDays : Form
    {
        public FormDays()
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
                    query = "select DayID[ID], Name[Day], IsActive[Status] from DayTable";
                }
                else
                {
                    query = "select DayID[ID], Name[Day], IsActive[Status] from DayTable where Name like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Daylist = DataBase_Layer.Retrive(query);
                dgvDays.DataSource = Daylist;
                if (dgvDays.Rows.Count > 0)
                {
                    dgvDays.Columns[0].Width = 80;
                    dgvDays.Columns[1].Width = 150;
                    dgvDays.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }
        private void FormDays_Load(object sender, EventArgs e)
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
            if (txtDayName.Text.Length == 0)
            {
                ep.SetError(txtDayName, "Please Enter Correct Day Name!");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from DayTable where Name = '" + txtDayName.Text.ToUpper().Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exist!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into DayTable(Name , IsActive) values ('{0}','{1}')",
                                    txtDayName.Text.ToUpper().Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Day details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtDayName.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvDays.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvDays.Enabled = true;
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
            if (dgvDays != null)
            {
                if (dgvDays.Rows.Count > 0)
                {
                    if (dgvDays.SelectedRows.Count == 1)
                    {
                        txtDayName.Text = Convert.ToString(dgvDays.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvDays.CurrentRow.Cells[2].Value);
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
            if (txtDayName.Text.Length == 0)
            {
                ep.SetError(txtDayName, "Please Enter Day Name!");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from DayTable where Name = '" + txtDayName.Text.ToUpper().Trim() + "' and DayID != '" + Convert.ToString(dgvDays.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exist!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }

            }

            string updatequery = string.Format("update DayTable set Name = '{0}',IsActive = '{1}' where DayID = '{2}'",
                                    txtDayName.Text.ToUpper().Trim(), chkStatus.Checked, Convert.ToString(dgvDays.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Day details. then try again!");
            }
        }
    }
}
    
