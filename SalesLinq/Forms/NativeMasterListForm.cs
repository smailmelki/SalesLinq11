using System;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    /// <summary>
    /// أساس شاشات القوائم والجداول دون الاعتماد على DevExpress.
    /// </summary>
    public class NativeMasterListForm : NativeMasterForm
    {
        protected readonly DataGridView gridView1;

        public NativeMasterListForm()
        {
            gridView1 = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                RightToLeft = RightToLeft.Yes
            };

            Controls.Add(gridView1);
            gridView1.BringToFront();
            toolStrip.BringToFront();

            Load += NativeMasterListForm_Load;
        }

        private void NativeMasterListForm_Load(object sender, EventArgs e)
        {
            btn_Save.Visible = false;
            btn_Print.Visible = false;
            btn_Delete.Visible = false;

            gridView1.CellDoubleClick += GridView1_CellDoubleClick;
            RefreshData();
        }

        private void GridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !gridView1.Columns.Contains("ID"))
                return;

            var value = gridView1.Rows[e.RowIndex].Cells["ID"].Value;
            if (value == null || value == DBNull.Value)
                return;

            OpenForm(Convert.ToInt32(value));
            RefreshData();
        }

        public virtual void OpenForm(int id) { }
        public override void Save() { }
        public override void Delete() { }
    }
}