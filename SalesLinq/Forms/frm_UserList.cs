using System;
using System.Data;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_UserList : frm_MasterList
    {     
        public frm_UserList()
        {
            InitializeComponent();
        }

        private void Frm_UserList_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;// عدم السماح بالتعديل على الجدول
            gridView1.Columns["ID"].Visible = false;// اخفاء عمود الرقم
            gridView1.Columns["Name"].Caption = "الاسم";// تغيير عنوان رأس العمود
            gridView1.Columns["IsActive"].Caption = "نشط";// تغيير عنوان رأس العمود
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridControl1.DataSource = db.Users.Select(x => new { x.ID, x.Name, x.IsActive }).ToList();
            }
            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_User(id), true);
        }
        public override void New()
        {
            frm_Main.OpenForm(new frm_User(), true);
        }

    }
}