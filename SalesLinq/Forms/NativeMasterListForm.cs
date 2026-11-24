using System;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    /// <summary>
    /// أساس شاشات القوائم والجداول باستخدام عناصر Windows Forms الأصلية.
    /// </summary>
    public class NativeMasterListForm : NativeMasterForm
    {
        protected readonly DataGridView gridView1;

        // عملية الإضافة تُفوض إلى شاشة الإدخال التي تتحقق من صلاحيتها باسمها.
        protected override bool AuthorizeNewAction => false;

        public NativeMasterListForm()
        {
            gridView1 = new DataGridView
            {
                Name = "gridView1",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                RightToLeft = RightToLeft.Yes,
                BackgroundColor = System.Drawing.SystemColors.Window,
                BorderStyle = BorderStyle.None
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

            int id;
            if (!int.TryParse(Convert.ToString(value), out id))
                return;

            OpenForm(id);
            RefreshData();
        }

        public virtual void OpenForm(int id) { }
        public override void Save() { }
        public override void Delete() { }
    }
}