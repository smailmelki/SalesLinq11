using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;

namespace SalesLinq.Forms
{
    public partial class frm_MasterList : frm_Master
    {
        public frm_MasterList()
        {
            InitializeComponent();
        }

        private void Frm_MasterList_Load(object sender, EventArgs e)
        {
            btn_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_Print.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
            RefreshData();            
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            // لمعرفة ماذا يوجد في هذا الموقع اذا كان سطر أو غير ذلك
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)//للتأكد من أن المستخدم ضغط على خلية في الصف
            {
                OpenForm(Convert.ToInt32(view.GetFocusedRowCellValue("ID")));
            }
            RefreshData();
        }

        public virtual void OpenForm(int id)
        {

        }

        public override void Save()
        {

        }
        public override void Delete()
        {

        }
    }
}
