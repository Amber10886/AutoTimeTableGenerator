using System;
using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class FormLecturers : Form
    {
        public FormLecturers()
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
                    query = "select LecturerID[ID], FullName[Lecturer], ContactNo[Contact No], IsActive[Status] from LecturerTable";
                }
                else
                {
                    query = "select LecturerID[ID], FullName[Lecturer], ContactNo[Contact No], IsActive[Status] from LecturerTable where (FullName + ' ' +ContactNo) like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Lablist = DataBase_Layer.Retrive(query);
                dgvLecturer.DataSource = Lablist;
                if (dgvLecturer.Rows.Count > 0)
                {
                    dgvLecturer.Columns[0].Width = 80;
                    dgvLecturer.Columns[1].Width = 150;
                    dgvLecturer.Columns[2].Width = 100;
                    dgvLecturer.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }

        private void FormLecturers_Load(object sender, EventArgs e)
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
            if (txtLecturer.Text.Length == 0 || txtLecturer.Text.Length > 31)
            {
                ep.SetError(txtLecturer, "Please Enter Full Name!");
                txtLecturer.Focus();
                txtLecturer.SelectAll();
                return;
            }
            if (txtContactNo.Text.Length < 12)
            {
                ep.SetError(txtContactNo, "Please Enter Correct Contact No!");
                txtContactNo.Focus();
                txtContactNo.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from LecturerTable where FullName = '" + txtLecturer.Text.ToUpper().Trim() + "' and ContactNo = '"+txtContactNo.Text.Trim()+"'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLecturer, "Already Exist!");
                    txtLecturer.Focus();
                    txtLecturer.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into LecturerTable(FullName, ContactNo, IsActive) values ('{0}','{1}','{2}')",
                                    txtLecturer.Text.ToUpper().Trim(), txtContactNo.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtLecturer.Clear();
            txtContactNo.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvLecturer.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvLecturer.Enabled = true;
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
            if (dgvLecturer != null)
            {
                if (dgvLecturer.Rows.Count > 0)
                {
                    if (dgvLecturer.SelectedRows.Count == 1)
                    {
                        txtLecturer.Text = Convert.ToString(dgvLecturer.CurrentRow.Cells[1].Value);
                        txtContactNo.Text = Convert.ToString(dgvLecturer.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvLecturer.CurrentRow.Cells[3].Value);
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
            if (txtLecturer.Text.Length == 0 || txtLecturer.Text.Length > 31)
            {
                ep.SetError(txtLecturer, "Please Enter Full Name!");
                txtLecturer.Focus();
                txtLecturer.SelectAll();
                return;
            }
            if (txtContactNo.Text.Length < 12)
            {
                ep.SetError(txtContactNo, "Please Enter Correct Contact No!");
                txtContactNo.Focus();
                txtContactNo.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from LecturerTable where FullName = '" + txtLecturer.Text.ToUpper().Trim() + "'and ContactNo ='"+txtContactNo.Text.Trim()+"' and LecturerID != '" + Convert.ToString(dgvLecturer.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLecturer, "Already Exist!");
                    txtLecturer.Focus();
                    txtLecturer.SelectAll();
                    return;
                }
            }
            string updatequery = string.Format("update LecturerTable set FullName = '{0}', ContactNo = '{1}',IsActive = '{2}' where LecturerID = '{3}'",
                                   txtLecturer.Text.ToUpper().Trim(), txtContactNo.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvLecturer.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct  details. then try again!");
            }
        }
    }
}
