using System;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_UserSettingsProfileList : frm_MasterList
    {
        public frm_UserSettingsProfileList()
        {
            InitializeComponent();
            RefreshData();
        }

        private void Frm_UserSettingsProfileList_Load(object sender, EventArgs e)
        {
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["Name"].Caption = "الاسم";// تغيير عنوان رأس العمود
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridControl1.DataSource = db.UserSettingsProfiles.ToList();
            }
            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_UserSettingsProfile(id), true);
        }
        public override void New()
        {
            frm_Main.OpenForm(new frm_UserSettingsProfile(), true);
        }
    }
}