using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.SourceCode
{
    public class ComboHelper
    {
        //Semester
        public static void Semesters(ComboBox cmb)
        {
            DataTable dtSemesters = new DataTable();
            dtSemesters.Columns.Add("SemesterID");
            dtSemesters.Columns.Add("SemesterTitle");
            dtSemesters.Rows.Add("0", "---Select---");
            try
            {  
                DataTable dt = DataBase_Layer.Retrive("select SemesterID, SemesterTitle from SemesterTable where IsActive = 1");
                if(dt != null)
                {
                    if(dt.Rows.Count > 0)
                    {
                        foreach(DataRow semester in dt.Rows)
                        {
                            dtSemesters.Rows.Add(semester["SemesterID"], semester["SemesterTitle"]);
                        }
                    }
                }
                cmb.DataSource = dtSemesters;
                cmb.ValueMember = "SemesterID";
                cmb.DisplayMember = "SemesterTitle";
            }
            catch
            {
                cmb.DataSource = dtSemesters;
            }
        }

        //Programs
        public static void Programs(ComboBox cmb)
        {
            DataTable dtPrograms = new DataTable();
            dtPrograms.Columns.Add("ProgramID");
            dtPrograms.Columns.Add("ProgramTitle");
            dtPrograms.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select ProgramID, ProgramTitle from ProgramTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow program in dt.Rows)
                        {
                            dtPrograms.Rows.Add(program["ProgramID"], program["ProgramTitle"]);
                        }
                    }
                }
                cmb.DataSource = dtPrograms;
                cmb.ValueMember = "ProgramID";
                cmb.DisplayMember = "ProgramTitle";
            }
            catch
            {
                cmb.DataSource = dtPrograms;
            }
        }

        //RoomType
        public static void RoomTypes(ComboBox cmb)
        {
            DataTable dtTypes = new DataTable();
            dtTypes.Columns.Add("RoomTypeID");
            dtTypes.Columns.Add("TypeName");
            dtTypes.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select RoomTypeID, TypeName from RoomTypeTable");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtTypes.Rows.Add(type["RoomTypeID"], type["TypeName"]);
                        }
                    }
                }
                cmb.DataSource = dtTypes;
                cmb.ValueMember = "RoomTypeID";
                cmb.DisplayMember = "TypeName";
            }
            catch
            {
                cmb.DataSource = dtTypes;
            }
        }

        //DayTable
        public static void AllDays(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("DayID");
            dtList.Columns.Add("Name");
            dtList.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select DayID, Name from DayTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtList.Rows.Add(type["DayID"], type["Name"]);
                        }
                    }
                }
                cmb.DataSource = dtList;
                cmb.ValueMember = "DayID";
                cmb.DisplayMember = "Name";
            }
            catch
            {
                cmb.DataSource = dtList;
            }
        }


        public static void AllTeachers(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("LecturerID");
            dtList.Columns.Add("FullName");
            dtList.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select LecturerID, FullName from LecturerTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtList.Rows.Add(type["LecturerID"], type["FullName"]);
                        }
                    }
                }
                cmb.DataSource = dtList;
                cmb.ValueMember = "LecturerID";
                cmb.DisplayMember = "FullName";
            }
            catch
            {
                cmb.DataSource = dtList;
            }
        }

        public static void AllSubjects(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("CourseID");
            dtList.Columns.Add("Title");
            dtList.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select CourseID, Title from CoursesTable where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtList.Rows.Add(type["CourseID"], type["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtList;
                cmb.ValueMember = "CourseID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtList;
            }
        }

        public static void AllProgramSemesters(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("ProgramSemesterID");
            dtList.Columns.Add("Title");
            dtList.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select ProgramSemesterID, Title from v_ProgramSemesterActiveList where ProgramSemesterIsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtList.Rows.Add(type["ProgramSemesterID"], type["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtList;
                cmb.ValueMember = "ProgramSemesterID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtList;
            }
        }

        public static void AllTeacherSubjects(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("LecturerSubjectID");
            dtList.Columns.Add("SubjectTitle");
            dtList.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DataBase_Layer.Retrive("select LecturerSubjectID, SubjectTitle from v_AllSubjectTeachers where IsActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtList.Rows.Add(type["LecturerSubjectID"], type["SubjectTitle"]);
                        }
                    }
                }
                cmb.DataSource = dtList;
                cmb.ValueMember = "LecturerSubjectID";
                cmb.DisplayMember = "SubjectTitle";
            }
            catch
            {
                cmb.DataSource = dtList;
            }
        }

        public static void TimeSlotsNumbers(ComboBox cmb)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("Number");
            dtList.Rows.Add("0", "---Select---");
            dtList.Rows.Add("1", "1");
            dtList.Rows.Add("2", "2");
            dtList.Rows.Add("3", "3");
            dtList.Rows.Add("4", "4");
            dtList.Rows.Add("5", "5");
            dtList.Rows.Add("6", "6");
            dtList.Rows.Add("7", "7");
            dtList.Rows.Add("8", "8");
            dtList.Rows.Add("9", "9");
            dtList.Rows.Add("10", "10");
            dtList.Rows.Add("11", "11");
            dtList.Rows.Add("12", "12");
            dtList.Rows.Add("13", "13");
            dtList.Rows.Add("14", "14");
            dtList.Rows.Add("15", "15");
            dtList.Rows.Add("16", "16");
            dtList.Rows.Add("17", "17");
            dtList.Rows.Add("18", "18");
            dtList.Rows.Add("19", "19");
            dtList.Rows.Add("20", "20");
            dtList.Rows.Add("21", "21");
            dtList.Rows.Add("22", "22");
            dtList.Rows.Add("23", "23");
            dtList.Rows.Add("24", "24");
            dtList.Rows.Add("25", "25");
            dtList.Rows.Add("26", "26");
            dtList.Rows.Add("27", "27");
            dtList.Rows.Add("28", "28");
            dtList.Rows.Add("29", "29");
            dtList.Rows.Add("30", "30");
            dtList.Rows.Add("31", "31");
            dtList.Rows.Add("32", "32");
            dtList.Rows.Add("33", "33");
            dtList.Rows.Add("34", "34");
            dtList.Rows.Add("35", "35");
            dtList.Rows.Add("36", "36");
            dtList.Rows.Add("37", "37");
            dtList.Rows.Add("38", "38");
            dtList.Rows.Add("39", "39");
            dtList.Rows.Add("40", "40");
           
            cmb.DataSource = dtList;
            cmb.ValueMember = "ID";
            cmb.DisplayMember = "Number";
           
        }
    }
}
