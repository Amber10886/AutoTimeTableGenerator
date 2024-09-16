using System;
using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.Configuration_Form
{
    public partial class FormSemester : Form
    {
        public FormSemester()
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
                    query = "select SemesterID[ID], SemesterTitle[Semester], IsActive[Status] from SemesterTable";
                }
                else
                {
                    query = "select SemesterID[ID] ,SemesterTitle[Semester]  , IsActive[Status] from SemesterTable where SemesterTitle like '%" + searchvalue.Trim() + "%'";
                }
                DataTable semesterlistt = DataBase_Layer.Retrive(query);
                dgvSemester.DataSource = semesterlistt;
                if (dgvSemester.Rows.Count > 0)
                {
                    dgvSemester.Columns[0].Width = 80;
                    dgvSemester.Columns[1].Width = 150;
                    dgvSemester.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }

        private void FormSemester_Load(object sender, EventArgs e)
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
            if (txtSemesterName.Text.Length == 0)
            {
                ep.SetError(txtSemesterName, "Please Enter Correct Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from SemesterTable where SemesterTitle = '" + txtSemesterName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exist!");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into SemesterTable(SemesterTitle , IsActive) values ('{0}','{1}')",
                                    txtSemesterName.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Semester details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtSemesterName.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvSemester.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvSemester.Enabled = true;
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
            if (dgvSemester != null)
            {
                if (dgvSemester.Rows.Count > 0)
                {
                    if (dgvSemester.SelectedRows.Count == 1)
                    {
                        txtSemesterName.Text = Convert.ToString(dgvSemester.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvSemester.CurrentRow.Cells[2].Value);
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
            if (txtSemesterName.Text.Length == 0)
            {
                ep.SetError(txtSemesterName, "Please Enter Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from SemesterTable where SemesterTitle = '" + txtSemesterName.Text.Trim() + "' and SemesterID != '" + Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exist!");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }

            }

            string updatequery = string.Format("update SemesterTable set SemesterTitle = '{0}',IsActive = '{1}' where SemesterID = '{2}'",
                                    txtSemesterName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Semester details. then try again!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}   
    