using System;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_StoresList : frm_MasterList
    {
        public frm_StoresList()
        {
            InitializeComponent();
        }

        private void Frm_StoresList_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;// عدم السماح بالتعديل على الجدول
            gridView1.Columns["ID"].Visible = false;// اخفاء عمود الرقم
            gridView1.Columns["Name"].Caption = "الاسم";// تغيير عنوان رأس العمود
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridControl1.DataSource = db.Stores.Select(x => new { x.ID, x.Name });
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
        }
    }
}
