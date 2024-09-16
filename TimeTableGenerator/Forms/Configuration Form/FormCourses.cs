using System;
using System.Data;
using System.Windows.Forms;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class FormCourses : Form
    {
        public FormCourses()
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
                    query = "select CourseID[ID],Title[Subjects],CrHrs,RoomTypeID ,TypeName[Type],IsActive from V_AllSubjects";
                }
                else
                {
                    query = "select CourseID[ID], Title[Subjects], CrHrs, RoomTypeID,TypeName[Type], IsActive from V_AllSubjects where (Title + ' ' + TypeName) like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Lablist = DataBase_Layer.Retrive(query);
                dgvSubjects.DataSource = Lablist;
                if (dgvSubjects.Rows.Count > 0)
                {
                    dgvSubjects.Columns[0].Width = 60;  //CourseID
                    dgvSubjects.Columns[1].Width = 250; //Title
                    dgvSubjects.Columns[2].Width = 60; //CrHrs
                    dgvSubjects.Columns[3].Visible = false; //RoomTypeID
                    dgvSubjects.Columns[4].Width = 80; //TypeName
                    dgvSubjects.Columns[5].Width = 80; //IsActive
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }
        private void txtCrHrs_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void FormCourses_Load(object sender, EventArgs e)
        {
            cmbCrHrs.SelectedIndex = 0;
            ComboHelper.RoomTypes(cmbSelectType);
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSubjectTitle.Text.Length == 0)
            {
                ep.SetError(txtSubjectTitle, "Please Enter Subject Title!");
                txtSubjectTitle.Focus();
                txtSubjectTitle.SelectAll();
                return;
            }
            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type!");
                cmbSelectType.Focus();
                return;
            }


            DataTable checktitle = DataBase_Layer.Retrive("select * from CoursesTable where Title = '" + txtSubjectTitle.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSubjectTitle, "Already Exist!");
                    txtSubjectTitle.Focus();
                    txtSubjectTitle.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into CoursesTable(Title, CrHrs, RoomTypeID, IsActive) values ('{0}','{1}','{2}','{3}')",
                                    txtSubjectTitle.Text.ToUpper().Trim(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                SaveClearForm();
            }
            else
            {
                MessageBox.Show("Please provide correct details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtSubjectTitle.Clear();
            cmbSelectType.SelectedIndex = 0;
            cmbCrHrs.SelectedIndex = 0;
            chkStatus.Checked = false;

        }
        public void SaveClearForm()
        {
            txtSubjectTitle.Clear();
            chkStatus.Checked = true;
            FillGrid(string.Empty);

        }
        public void EnableComponents()
        {
            dgvSubjects.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvSubjects.Enabled = true;
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
            if (dgvSubjects != null)
            {
                if (dgvSubjects.Rows.Count > 0)
                {
                    if (dgvSubjects.SelectedRows.Count == 1)
                    {
                        txtSubjectTitle.Text = Convert.ToString(dgvSubjects.CurrentRow.Cells[1].Value);
                        cmbSelectType.SelectedValue = Convert.ToString(dgvSubjects.CurrentRow.Cells[3].Value); //RoomTypeID
                        cmbCrHrs.Text = Convert.ToString(dgvSubjects.CurrentRow.Cells[2].Value); //CrHrs
                        chkStatus.Checked = Convert.ToBoolean(dgvSubjects.CurrentRow.Cells[5].Value); //IsActive
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
            if (txtSubjectTitle.Text.Length == 0)
            {
                ep.SetError(txtSubjectTitle, "Please Enter Subject Title!");
                txtSubjectTitle.Focus();
                txtSubjectTitle.SelectAll();
                return;
            }
            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type!");
                cmbSelectType.Focus();
                return;
            }


            DataTable checktitle = DataBase_Layer.Retrive("select * from CoursesTable where Title = '" + txtSubjectTitle.Text.Trim() + "' and CourseID != '"+Convert.ToString(dgvSubjects.CurrentRow.Cells[0].Value)+"'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSubjectTitle, "Already Exist!");
                    txtSubjectTitle.Focus();
                    txtSubjectTitle.SelectAll();
                    return;
                }
            }
            string updatequery = string.Format("update CoursesTable set Title ='{0}', CrHrs ='{1}', RoomTypeID ='{2}', IsActive ='{3}' where CourseID = '{4}'",
                                    txtSubjectTitle.Text.ToUpper().Trim(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked, Convert.ToString(dgvSubjects.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct details. then try again!");
            }
        }
    }
}