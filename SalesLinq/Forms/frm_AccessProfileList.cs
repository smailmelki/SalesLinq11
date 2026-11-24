using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_AccessProfileList : NativeMasterListForm
    {
        public frm_AccessProfileList()
        {
            InitializeComponent();
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                gridView1.DataSource = db.UserAccessProfileNames
                    .OrderBy(x => x.Name)
                    .Select(x => new { x.ID, x.Name })
                    .ToList();
            }

            if (gridView1.Columns.Contains("ID"))
                gridView1.Columns["ID"].Visible = false;
            if (gridView1.Columns.Contains("Name"))
                gridView1.Columns["Name"].HeaderText = "اسم النموذج";
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