using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.ProgramSemesterForms
{
    public partial class FormProgramSemesterSubject : Form
    {
        public FormProgramSemesterSubject()
        {
            InitializeComponent();
        }

        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if(string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select [ProgramSemesterSubjectID] [ID], [ProgramID],[Program], ProgramSemesterID, Title [Semester], LecturerSubjectID, SSTitle [Subject], Capacity, IsSubjectActive [Status] from" +
                        " v_AllSemesterSubjects where [ProgramSemesterIsActive] = 1 and [ProgramIsActive] = 1 and [SemesterIsActive] = 1 and [SubjectIsActive] = 1" +
                        "order by ProgramSemesterID";
                }
                else
                {
                    query = "select [ProgramSemesterSubjectID] [ID], [ProgramID],[Program], ProgramSemesterID, Title [Semester], LecturerSubjectID, SSTitle [Subject], Capacity, IsSubjectActive [Status] from" +
                        "v_AllSemesterSubjects where [ProgramSemesterIsActive] = 1 and [ProgramIsActive] = 1 and [SemesterIsActive] = 1 and [SubjectIsActive] = 1" +
                        "AND (Program + ' ' + Title + ' ' +SSTitle) like '%"+searchvalue+ "%' order by ProgramSemesterID";
                }
                DataTable semesterlist = DataBase_Layer.Retrive(query);
                dgvTeacherSubjects.DataSource = semesterlist;
                if(dgvTeacherSubjects.Rows.Count >0)
                {
                    dgvTeacherSubjects.Columns[0].Visible = false;//ProgramSemesterSubjectID
                    dgvTeacherSubjects.Columns[1].Visible = false;//ProgramID
                    dgvTeacherSubjects.Columns[2].Width = 120;//Program
                    dgvTeacherSubjects.Columns[3].Visible = false;//ProgramSemesterID
                    dgvTeacherSubjects.Columns[4].Width = 160;//Semester
                    dgvTeacherSubjects.Columns[5].Visible = false;//LecturerSubjectID
                    dgvTeacherSubjects.Columns[6].Width = 300;//Subject
                    dgvTeacherSubjects.Columns[7].Width = 80;//Capacity
                    dgvTeacherSubjects.Columns[8].Width = 80;//Status
                    dgvTeacherSubjects.ClearSelection();
                }

            }
            catch 
            {

                MessageBox.Show("Some unexpected Issue Occue Please Try Again!");
            }
        }
        private void FormProgramSemesterSubject_Load(object sender, EventArgs e)
        {
            ComboHelper.AllProgramSemesters(cmbSemester);
            ComboHelper.AllTeacherSubjects(cmbSubjects);
            FillGrid(string.Empty);
        }

        private void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTitle.Text = cmbSubjects.SelectedIndex == 0 ? string.Empty : cmbSubjects.Text;
        }
        private void FormClear()
        {
            txtTitle.Clear();
            cmbSubjects.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            FormClear();
        }

       
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtTitle.Text.Trim().Length==0)
            {
                ep.SetError(txtTitle, "Please Enter Semester Subject Title!");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if(cmbSemester.SelectedIndex==0)
            {
                ep.SetError(cmbSemester, "Please Select Semester!");
                cmbSemester.Focus();
                return;
            }
            if (cmbSubjects.SelectedIndex==0)
            {
                ep.SetError(cmbSubjects, "Please Select Subject!");
                cmbSubjects.Focus();
                return;
            }
            string checkquery = "select * from ProgramSemesterSubjectTable where" +
                " ProgramSemesterID = '" + cmbSemester.SelectedValue +"' and" +
                "LecturerSubjectID +'" + cmbSubjects.SelectedValue+"'";
            DataTable dt = DataBase_Layer.Retrive(checkquery);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ep.SetError(cmbSubjects, "Already Exist!");
                    cmbSubjects.Focus();
                    return;
                }
            }
            string insertquery = string.Format("insert into ProgramSemesterSubjectTable" +
                "(SSTitle, ProgramSemesterID, LecturerSubjectID) values('{0}', '{1}', '{2}')" ,
                txtTitle.Text.Trim(), cmbSemester.SelectedValue, cmbSubjects.SelectedValue);
            bool result = DataBase_Layer.Insert(insertquery);
            if(result == true)
            {
                MessageBox.Show("Subjects Assign Successfully!");
                FillGrid(string.Empty);
                FormClear();

            }
            else
            {
                MessageBox.Show("Please Provide Correct Details, and Try Again!");
            }
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvTeacherSubjects != null)
            {
                if (dgvTeacherSubjects.Rows.Count > 0)
                {
                    if (dgvTeacherSubjects.SelectedRows.Count == 1)
                    {
                        if(MessageBox.Show("Are You Sure You Want to Change Status?", "Confirmation",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool existstatus = Convert.ToBoolean(dgvTeacherSubjects.CurrentRow.Cells[8].Value);
                            int semestersubjectid = Convert.ToInt32(dgvTeacherSubjects.CurrentRow.Cells[0].Value);
                            bool status = false;
                            if(existstatus == true)
                            {
                                status = false;
                            }
                            else
                            {
                                status = true;
                            }
                            string updatequery = string.Format("Update ProgramSemesterSubjectTable set" +
                                " IsSubjectActive = '{0}' where ProgramSemesterSubjectID = '{1}'", status, semestersubjectid);
                            bool result = DataBase_Layer.Update(updatequery);
                            if(result == true) 
                            {
                                MessageBox.Show("Change Successfully!");
                                FillGrid(string.Empty);
                            }
                            else
                            {
                                MessageBox.Show("Please Try Again!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select One Record!");
                    }
                }
                else
                {
                    MessageBox.Show("List is Empty!");
                }

            }
            else
            {
                MessageBox.Show("List is Empty!");
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }
    }
}
