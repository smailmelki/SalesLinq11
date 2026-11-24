using System;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_UserList : NativeMasterListForm
    {
        public frm_UserList()
        {
            InitializeComponent();
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridView1.DataSource = db.Users
                    .OrderBy(x => x.Name)
                    .Select(x => new { x.ID, x.Name, x.UserName, x.IsActive })
                    .ToList();
            }

            if (gridView1.Columns.Contains("ID"))
                gridView1.Columns["ID"].Visible = false;
            if (gridView1.Columns.Contains("Name"))
                gridView1.Columns["Name"].HeaderText = "الاسم";
            if (gridView1.Columns.Contains("UserName"))
                gridView1.Columns["UserName"].HeaderText = "اسم الدخول";
            if (gridView1.Columns.Contains("IsActive"))
                gridView1.Columns["IsActive"].HeaderText = "نشط";
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