using System;
using System.Windows.Forms;
using TimeTableGenerator.Configuration_Form;
using TimeTableGenerator.Forms;
using TimeTableGenerator.Forms.Configuration_Form;
using TimeTableGenerator.Forms.LectureSubjectForms;
using TimeTableGenerator.Forms.ProgramSemesterForms;
using TimeTableGenerator.Forms.TimeSlotForms;
using TimeTableGenerator.Reports;




namespace TimeTableGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm: new FormPrintAllSemesterTimetables());

        }
    }

    
}
