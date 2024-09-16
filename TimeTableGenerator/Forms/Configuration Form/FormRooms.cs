using System;
using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.Configuration_Form
{
    public partial class FormRooms : Form
    {
        public FormRooms()
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
                    query = "select RoomID[ID], RoomNo[Room],Capacity, IsActive[Status] from RoomTable";
                }
                else
                {
                    query = "select RoomID[ID], RoomNo[Room],Capacity, IsActive[Status] from RoomTable where RoomNo like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Roomlist = DataBase_Layer.Retrive(query);
                dgvRooms.DataSource = Roomlist;
                if (dgvRooms.Rows.Count > 0)
                {
                    dgvRooms.Columns[0].Width = 80;
                    dgvRooms.Columns[1].Width = 150;
                    dgvRooms.Columns[2].Width = 100;
                    dgvRooms.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch
            {

                MessageBox.Show("Some Unexpected issue is occur please try again!");
            }
        }

        private void FormRooms_Load(object sender, EventArgs e)
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
            if (txtRoomNo.Text.Length == 0)
            {
                ep.SetError(txtRoomNo, "Please Enter Correct Room No!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Room Capacity");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exist!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("insert into RoomTable(RoomNo ,Capacity, IsActive) values ('{0}','{1}','{2}')",
                                    txtRoomNo.Text.Trim(),txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DataBase_Layer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Room details. then try again!");
            }
        }
        public void ClearForm()
        {
            txtRoomNo.Clear();
            chkStatus.Checked = false;

        }
        public void EnableComponents()
        {
            dgvRooms.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }

        public void DisableComponents()
        {
            dgvRooms.Enabled = true;
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

            if (dgvRooms != null)
            {
                if (dgvRooms.Rows.Count > 0)
                {
                    if (dgvRooms.SelectedRows.Count == 1)
                    {
                        txtRoomNo.Text = Convert.ToString(dgvRooms.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvRooms.CurrentRow.Cells[2].Value);
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
            if (txtRoomNo.Text.Length == 0)
            {
                ep.SetError(txtRoomNo, "Please Enter Room No!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }

            DataTable checktitle = DataBase_Layer.Retrive("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim() + "' and RoomID != '" + Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exist!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }
            string updatequery = string.Format("update RoomTable set RoomNo = '{0}',Capacity = '{1}',IsActive = '{2}' where RoomID = '{3}'",
                                   txtRoomNo.Text.Trim(),txtCapacity.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value));
            bool result = DataBase_Layer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated SuccessFully!");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please provide correct Room details. then try again!");
            }
        }

        private void txtCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
