using System;
using System.Linq;
using SalesLinq.Class;
using DevExpress.XtraGrid.Views.Grid;

namespace SalesLinq.Forms
{
    public partial class frm_ProductList : frm_Master
    {
        public frm_ProductList()
        {
            InitializeComponent();
        }
        private void Frm_ProductList_Load(object sender, EventArgs e)
        {
            RefreshData();
            //Session.ProductView.ListChanged += ProductView_ListChanged;
            gridView1.OptionsBehavior.Editable = false;//منع التعديل
            btn_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اخفاء زر الحفظ
            btn_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اخفاء زر الحذف
            gridView1.DoubleClick += GridView1_DoubleClick;// اضافة حدث الصغط على الجدول
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;// اضافة حدث من اجل عرض نص بدل نص اخر في الجدول
            gridControl1.ViewRegistered += GridControl1_ViewRegistered;//+ حدث ينشط عند اضافة جدول داخل الجدول الرئيسي أي عند الضغط على علامة 
            gridView1.OptionsDetail.ShowDetailTabs = false;// لاخفاء التاب داخل الجدول الداخلي جدول الوحدات

            // لتسمية الأعمدة في الجدول الأساسي
            var ins = new Session.ProductViewClass(); //var ins = Session.ProductView.FirstOrDefault()// نفس الشيئ
            gridView1.Columns[nameof(ins.CategoryName)].Caption = "الفئة";
            gridView1.Columns[nameof(ins.Code)].Caption = "الكود";
            gridView1.Columns[nameof(ins.Descreption)].Caption = "الوصف";
            gridView1.Columns[nameof(ins.IsActive)].Caption = "نشط";
            gridView1.Columns[nameof(ins.Name)].Caption = "الاسم";
            gridView1.Columns[nameof(ins.Type)].Caption = "النوع";
            gridView1.Columns[nameof(ins.ID)].Visible = false;
        }


        public override void New()
        {
            var frm = new frm_Product();
            frm_Main.OpenForm(frm, true);
        }
        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            if (e.View.LevelName == "Units")
            {
                GridView view = e.View as GridView;
                view.OptionsView.ShowViewCaption = true; // لعرض عنوان على الجدول الداخلي
                view.ViewCaption = "وحدات القياس";// اعطاء اسم للعنوان
                //لتسمية الأعمدة الخاصة بجدول الوحدات الداخلي أو الفرعي
                view.Columns["UnitID"].Visible = false;
                view.Columns["UnitName"].Caption = "اسم الوحدة";
                view.Columns["Factor"].Caption = "المعامل";
                view.Columns["SellPrice"].Caption = "سعر البيع";
                view.Columns["BuyPrice"].Caption = "سعر الشراء";
                view.Columns["Barcode"].Caption = "الباركود";
            }
        }

        /// <summary>
        /// دالة لفتح الفورم الخاصة بتعديل صنف نفس الفورم الخاصة باضافة صنف جديد 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            if (int.TryParse(gridView1.GetFocusedRowCellValue("ID").ToString(), out id) && id > 0) //لفتح فورم التعديل عند الضغط على السطر فقط
            {
                var frm = new frm_Product(id);
                frm_Main.OpenForm(frm, true);
            }
        }

        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {
                // لعرض نص بدل رقم في جدول الأصناف في عمود النوع وعند الضغط عليه يظهر الرقم عادي
                e.DisplayText = Master.ProductTypesList.Single(x => x.ID == Convert.ToInt32(e.Value)).Name;
            }
        }

        public override void RefreshData()
        {
            gridControl1.DataSource = Session.ProductView;
            base.RefreshData();
        }
    }
}