using System;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_AccessProfileList : frm_MasterList
    {
        public frm_AccessProfileList()
        {
            InitializeComponent();
            RefreshData();
        }

        private void Frm_AccessProfileList_Load(object sender, EventArgs e)
        {
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["Name"].Caption = "الاسم";// تغيير عنوان رأس العمود
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridControl1.DataSource = db.UserAccessProfileNames.ToList();
            }
            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_AccessProfile(id), true);
        }
        public override void New()
        {
            frm_Main.OpenForm(new frm_AccessProfile(), true);
        }
    }



   



}