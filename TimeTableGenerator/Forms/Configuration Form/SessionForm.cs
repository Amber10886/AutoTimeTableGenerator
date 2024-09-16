using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class SessionForm : Form
    {
        public SessionForm()
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
                    query = "select SessionID[ID] , SessionTitle , IsActive[Status] from SessionTable";
                }
                else
                {
                    query = "select SessionID[ID] , SessionTitle , IsActive[Status] from SessionTable where SessionTitle like '%" + searchvalue.Trim() + "%'";
                }
                DataTable sessionlist = DataBase_Layer.Retrive(query);
                dgvSession.DataSource = sessionlist;
                if (dgvSession.Rows.Count > 0)
                {
                    dgvSession.Columns[0].Width = 80;
                    dgvSession.Columns[1].Width = 150;
                    dgvSession.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void SessionForm_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSessionTitle.Text.Length < 9)
            {
                ep.SetError(txtSessionTitle, "Please Enter Correct Session Title!");
                txtSessionTitle.Focus();
                txtSessionTitle.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from SessionTable where SessionTitle = '" + txtSessionTitle.Text.Trim() + "' and SessionID != '" + Convert.ToString(dgvSession.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSessionTitle, "Already Exist!");
                    txtSessionTitle.Focus();
                    txtSessionTitle.SelectAll();
                    return;
                }
            }
            string updatequery = string.Format("update SessionTable set SessionTitle = '{0}',IsActive = '{1}' where SessionID = '{2}'",
                                    txtSessionTitle.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvSession.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Session details. then try again!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSessionTitle.Text.Length < 9)
            {
                ep.SetError(txtSessionTitle, "Please Enter Correct Session Title!");
                txtSessionTitle.Focus();
                txtSessionTitle.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from SessionTable where SessionTitle = '" + txtSessionTitle.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSessionTitle, "Already Exist!");
                    txtSessionTitle.Focus();
                    txtSessionTitle.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into SessionTable(SessionTitle , IsActive) values ('{0}','{1}')",
                                    txtSessionTitle.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Session details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtSessionTitle.Clear();
            chkStatus.Checked = false;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        public void EnableComponents()
        {
            dgvSession.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvSession.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);

        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            if (dgvSession != null)
            {
                if (dgvSession.Rows.Count > 0)
                {
                    if (dgvSession.SelectedRows.Count == 1)
                    {
                        txtSessionTitle.Text = Convert.ToString(dgvSession.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvSession.CurrentRow.Cells[2].Value);
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
    }
}



