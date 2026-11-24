using System;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_StoresList : NativeMasterListForm
    {
        public frm_StoresList()
        {
            InitializeComponent();
        }

        private void Frm_StoresList_Load(object sender, EventArgs e)
        {
            if (gridView1.Columns.Contains("ID"))
                gridView1.Columns["ID"].Visible = false;

            if (gridView1.Columns.Contains("Name"))
                gridView1.Columns["Name"].HeaderText = "الاسم";
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridView1.DataSource = db.Stores
                    .OrderBy(x => x.Name)
                    .Select(x => new { x.ID, x.Name })
                    .ToList();
            }

            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_Stores(id), true);
        }

        public override void New()
        {
            frm_Main.OpenForm(new frm_Stores(), true);
            RefreshData();
        }
    }
}