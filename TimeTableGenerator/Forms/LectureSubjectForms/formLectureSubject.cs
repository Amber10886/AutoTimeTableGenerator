using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.LectureSubjectForms
{
    public partial class formLectureSubject : Form
    {
        public formLectureSubject()
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
                    query = "select LecturerSubjectID[ID],SubjectTitle[Subject Title],LecturerID, FullName[Lecturer], CourseID, " +
                            "Title[Course],IsActive[Status] from v_AllSubjectTeachers";
                }
                else
                {
                    query = "select LecturerSubjectID[ID], SubjectTitle, LecturerID, FullName[Lecturer], CourseID, " +
                            "Title[Course],IsActive[Status] from v_AllSubjectTeachers" +
                            "where (SubjectTitle +''+ FullName +''+Title) like '%"+searchvalue.Trim()+"%'";
                }
                DataTable semesterlist = DataBase_Layer.Retrive(query);
                dgvTeacherSubjects.DataSource = semesterlist;
                if (dgvTeacherSubjects.Rows.Count > 0)
                {
                    dgvTeacherSubjects.Columns[0].Visible = false; //LecturerSubjectID
                    dgvTeacherSubjects.Columns[1].Width = 250;      //SubjectTitle
                    dgvTeacherSubjects.Columns[2].Visible = false; //LecturerID
                    dgvTeacherSubjects.Columns[3].Width = 150;     //FullName
                    dgvTeacherSubjects.Columns[4].Visible = false;     //CourseID
                    dgvTeacherSubjects.Columns[5].Width = 300;     //Title
                    dgvTeacherSubjects.Columns[6].Width = 100;     //Status

                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }

        public void ClearForm()
        {
            cmbTeachers.SelectedIndex = 0;
            cmbSubjects.SelectedIndex = 0;
            chkStatus.Checked = true;

        }
        public void EnableComponents()
        {
            dgvTeacherSubjects.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvTeacherSubjects.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);

        }
        private void formLectureSubject_Load(object sender, EventArgs e)
        {
            ComboHelper.AllSubjects(cmbSubjects);
            ComboHelper.AllTeachers(cmbTeachers);
            FillGrid(string.Empty);
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

       

        private void btnSave_Click(object sender, EventArgs e)
        {
            try

            {
                ep.Clear();
                if(cmbTeachers.SelectedIndex == 0)
                {
                    ep.SetError(cmbTeachers, "Please Select Teacher!");
                    cmbTeachers.Focus();
                    return ;
                }

                if (cmbSubjects.SelectedIndex == 0)
                {
                    ep.SetError(cmbSubjects, "Please Select Suject!");
                    cmbSubjects.Focus();
                    return ;
                }

                DataTable dt = DataBase_Layer.Retrive("select * from LecturerSubjectTable where LecturerID='"+cmbTeachers.SelectedValue+ "'and CourseID='"+cmbSubjects.SelectedValue+"'");
                if(dt != null)
                {
                    if(dt.Rows.Count > 0)
                    {
                        ep.SetError(cmbSubjects, "Already Registered!");
                        cmbSubjects.Focus();
                        return;
                    }
                }

                string insertquery = string.Format("insert into LecturerSubjectTable(SubjectTitle , LecturerID,CourseID,IsActive) values ('{0}','{1}','{2}','{3}')",  
                          cmbSubjects.Text + "(" + cmbTeachers.Text + ")", cmbTeachers.SelectedValue, cmbSubjects.SelectedValue, chkStatus.Checked) ;
                bool result = DataBase_Layer.Insert(insertquery);
                if(result==true ) 
                {
                    MessageBox.Show("Subject Assign SuccessFully!");
                    DisableComponents();
                    return;
                }
                else
                {
                    MessageBox.Show("Some Unexpected Issue Is Occur Please Try Again!");
                }
            }
	         catch 

            {
                MessageBox.Show("Please Check SQL Server Agent Connectivity!");
               
            }
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvTeacherSubjects !=null)
                {
                    if(dgvTeacherSubjects.Rows.Count>0)
                    {
                        if(dgvTeacherSubjects.SelectedRows.Count==1)
                        {
                            if (MessageBox.Show("Are You Sure You Want To Update Selected Record?", "Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {


                                string id = Convert.ToString(dgvTeacherSubjects.CurrentRow.Cells[0].Value);
                                bool status = (Convert.ToBoolean(dgvTeacherSubjects.CurrentRow.Cells[6].Value) == true ? false : true);
                                string updatequery = "update  LecturerSubjectTable set IsActive='" + status + "'where LecturerSubjectID='" + id + "'";
                                bool result = DataBase_Layer.Update(updatequery);
                                if (result == true)
                                {
                                    MessageBox.Show("Status Changed SuccessFully!");
                                    DisableComponents();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Some Unexpected Issue Is Occur Please Try Again!");
                                }
                            }
                        }
                    }
                }
            }
            catch 
            {

            }
        }

      

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            DisableComponents();
        }
    }
}
