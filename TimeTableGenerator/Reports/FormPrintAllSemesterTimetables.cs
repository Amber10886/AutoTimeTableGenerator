using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTableGenerator.Reports
{
    public partial class FormPrintAllSemesterTimetables : Form
    {
        public FormPrintAllSemesterTimetables()
        {
            InitializeComponent();

            rpt_PrintSemesterWiseTimeTable rpt = new rpt_PrintSemesterWiseTimeTable();
            rpt.Refresh();
            crv.ReportSource = rpt;
        }

        private void FormPrintAllSemesterTimetables_Load(object sender, EventArgs e)
        {

        }
    }
}
