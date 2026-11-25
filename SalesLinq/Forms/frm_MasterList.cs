using System;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SalesLinq.Forms
{
    /// <summary>قائمة انتقالية متوافقة مع القوائم القديمة أثناء ترحيلها إلى WinForms.</summary>
    public partial class frm_MasterList : frm_Master
    {
        public frm_MasterList() { InitializeComponent(); }

        private void Frm_MasterList_Load(object sender, EventArgs e)
        {
            btn_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_Print.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
            RefreshData();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            var mouse = e as DXMouseEventArgs;
            var view = sender as GridView;
            if (mouse == null || view == null) return;
            GridHitInfo hit = view.CalcHitInfo(mouse.Location);
            if (!hit.InRow && !hit.InRowCell) return;

            object value = view.GetFocusedRowCellValue("ID");
            int id;
            if (value == null || value == DBNull.Value ||
                !int.TryParse(value.ToString(), out id) || id <= 0) return;
            OpenForm(id);
        }

        public virtual void OpenForm(int id) { }
        public override void Save() { }
        public override void Delete() { }
    }
}