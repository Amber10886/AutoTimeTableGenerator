using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class ProgramForm : Form
    {
        public ProgramForm()
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
                    query = "select ProgramID[ID], ProgramTitle[Program], IsActive[Status] from ProgramTable";
                }
                else
                {
                    query = "select ProgramID[ID], ProgramTitle[Program], IsActive[Status] from ProgramTable where ProgramTitle like '%" + searchvalue.Trim() + "%'";
                }
                DataTable programlist = DataBase_Layer.Retrive(query);
                dgvProgram.DataSource = programlist;
                if (dgvProgram.Rows.Count > 0)
                {
                    dgvProgram.Columns[0].Width = 80;
                    dgvProgram.Columns[1].Width = 150;
                    dgvProgram.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }

        private void ProgramForm_Load(object sender, EventArgs e)
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
            if (txtProgramName.Text.Length == 0)
            {
                ep.SetError(txtProgramName, "Please Enter Correct Program Name!");
                txtProgramName.Focus();
                txtProgramName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from ProgramTable where ProgramTitle = '" + txtProgramName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramName, "Already Exist!");
                    txtProgramName.Focus();
                    txtProgramName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into ProgramTable(ProgramTitle , IsActive) values ('{0}','{1}')",
                                    txtProgramName.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Program details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtProgramName.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvProgram.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvProgram.Enabled = true;
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
            if (dgvProgram != null)
            {
                if (dgvProgram.Rows.Count > 0)
                {
                    if (dgvProgram.SelectedRows.Count == 1)
                    {
                        txtProgramName.Text = Convert.ToString(dgvProgram.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgram.CurrentRow.Cells[2].Value);
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
            if (txtProgramName.Text.Length == 0)
            {
                ep.SetError(txtProgramName, "Please Enter Program Title!");
                txtProgramName.Focus();
                txtProgramName.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from ProgramTable where ProgramTitle = '" + txtProgramName.Text.Trim() + "' and ProgramID != '" + Convert.ToString(dgvProgram.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramName, "Already Exist!");
                    txtProgramName.Focus();
                    txtProgramName.SelectAll();
                    return;
                }

            }

            string updatequery = string.Format("update ProgramTable set ProgramTitle = '{0}',IsActive = '{1}' where ProgramID = '{2}'",
                                    txtProgramName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvProgram.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Program details. then try again!");
            }
        }
    }
}
