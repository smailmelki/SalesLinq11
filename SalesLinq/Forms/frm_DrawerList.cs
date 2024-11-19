using System;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_DrawerList : frm_MasterList
    {
        public frm_DrawerList()
        {
            InitializeComponent();
            RefreshData();
        }

        private void Frm_DrawerList_Load(object sender, EventArgs e)
        {
            gridView1.Columns["ID"].Visible = false;// اخفاء عمود الرقم
            gridView1.Columns["Name"].Caption = "الاسم";// تغيير عنوان رأس العمود
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridControl1.DataSource = db.Drawers.Select(x => new { x.ID, x.Name });
            }
            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_Drawer(id), true);
        }
        public override void New()
        {
            frm_Main.OpenForm(new frm_Drawer(), true);
        }
    }
}