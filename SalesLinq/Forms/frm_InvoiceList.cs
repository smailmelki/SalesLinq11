using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using SalesLinq.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesLinq.Forms
{
    public partial class frm_InvoiceList : frm_MasterList
    {
        Master.InvoiceType Type;//نوع الفاتورة
        RepositoryItemLookUpEdit repoPartType;
        RepositoryItemLookUpEdit repoPart;
        RepositoryItemLookUpEdit repoBranch;
        RepositoryItemLookUpEdit repoDrawer;
        public frm_InvoiceList(Master.InvoiceType _Type)
        {
            InitializeComponent();
            dateEdit1.DateTime = dateEdit2.DateTime = DateTime.Now;
            lkp_PartType.EditValueChanged += Lkp_PartType_EditValueChanged;
            gridView1.AddButtonToGroupHeader(Properties.Resources.editfilter, OpenFilter);//اضافة زر للغريد فيو واضافة الحدث له
            //flyoutPanel1.Options.CloseOnOuterClick = true;// اغلاق النافذة عند الضغط خارجها
            Type = _Type;
            gridControl1.ViewRegistered += GridControl1_ViewRegistered;//حدث فتح الفاتورة في قائمة الفواتير من علامة + لتغيير أسماء الأعمدة
            gridView1.OptionsDetail.ShowDetailTabs = false;//لاخفاء تاب العنوان

            gridView1.PopupMenuShowing += GridView1_PopupMenuShowing;//حدث الظغط بزر الفأرة الأيمن لعرض القائمة
            gridView1.OptionsSelection.MultiSelect = true;// السماح بالتحديد المتعدد للأسطر
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.RowCellStyle += GridView1_RowCellStyle;

        }

        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            var remains = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "Remaing"));//المتبقي
            if (e.Column.FieldName== "PayStatus")
            {
                if (remains == 0)//اذا كان المتبقي يساوي الصفر
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;// DevExpress استعمال لون الثيم الخاص بال 
                else if (remains > 0)//اذا كان المتبقي أكبر الصفر
                {
                    var net = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "Net"));//الصافي
                    if (remains < net)
                        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;// DevExpress استعمال لون الثيم الخاص بال 
                    else //اذا كان المتبقي أصغر الصفر
                        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;// DevExpress استعمال لون الثيم الخاص بال 
                }

            }
        }

        /// <summary>
        /// دالة لعرض القائمة في حالة ان المسنخدم ضغط على خلية من السطر  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.HitInfo.InRow || e.HitInfo.InRowCell)//التأكد من ان المسنخدم ضغط على خلية من السطر
            {
                var dxButtonPrint = new DevExpress.Utils.Menu.DXMenuItem() { Caption = "Print" };//زر خاص بالقامة المنبثقة
                dxButtonPrint.ImageOptions.SvgImage = Properties.Resources.print;               //اضافة صورة لزر الطباعة في القائمة
                dxButtonPrint.Click += DxButtonPrint_Click;                                     //اضافة حدث للضغط على الزر
                e.Menu.Items.Add(dxButtonPrint);                                                //اضافة الزر للقائمة

                var dxButtonDelete = new DevExpress.Utils.Menu.DXMenuItem() { Caption = "حذف"}; //زر خاص بالقامة المنبثقة
                dxButtonDelete.ImageOptions.SvgImage = Properties.Resources.actions_trash;      //اضافة صورة لزر الطباعة في القائمة
                dxButtonDelete.Click += DxButtonDelete_Click;                                   //اضافة حدث للضغط على الزر
                e.Menu.Items.Add(dxButtonDelete);                                               //اضافة الزر للقائمة
            }
        }

        public override void Delete()
        {
            DxButtonDelete_Click(null, null);
        }

        private void DxButtonDelete_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedInvoicesIDs();
            if (ids != null)
                frm_Invoice.Delete(ids, this.Type, this.Name);
        }

        private void DxButtonPrint_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedInvoicesIDs();
            if (ids != null)
                frm_Invoice.Print(ids, this.Type, this.Name);
        }

        private List<int> GetSelectedInvoicesIDs()
        {

            var handels = gridView1.GetSelectedRows();
            List<int> ids = new List<int>();
            foreach (var handel in handels)
            {
                ids.Add(Convert.ToInt32(gridView1.GetRowCellValue(handel, "ID")));
            }
            if (ids.Count == 0)
            {
                XtraMessageBox.Show(text: "الرجاء اختيار فاتورة واحدةعلى الأقل");
                return null;
            }
            return ids;
        }
        /// <summary>
        /// دالة تسمية رؤوس تفاصيل الفاتورة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            var view = e.View as GridView;
            if (view != null && view.LevelName == "Prodects")
            {
                view.OptionsView.ShowViewCaption = true;
                view.ViewCaption = "الأصناف";//تسمية تاب العنوان
                view.Columns["Product"].Caption = "الصنف";
                view.Columns["Unit"].Caption = "الوحدة";
                view.Columns["Price"].Caption = "السعر";
                view.Columns["ItemQty"].Caption = "الكمية";
                view.Columns["DiscountValue"].Caption = "الخصم";
                view.Columns["TotalCostValue"].Caption = "الاجمالي";
            }
        }


        /// <summary>
        ///  فتح نافذة الفيلتر
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        void OpenFilter(object s,EventArgs e)
        {
            flyoutPanel1.ShowPopup();
        }
        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_Invoice(Type,id));
        }

        public override void New()
        {
            frm_Main.OpenForm(new frm_Invoice(Type));
        }
        /// <summary>
        /// تعبئة الفورم ببيانات الفواتير حسب النوع
        /// </summary>
        public override void RefreshData()
        {
            DateTime? fromDate = null;
            DateTime? ToDate = null;

            if (dateEdit1.DateTime.Year > 1950)
                fromDate = dateEdit1.DateTime;
            if (dateEdit2.DateTime.Year > 1950)
                ToDate = dateEdit2.DateTime;

            

            using (var db = new DAL.DBDataContext())
            {
                var query = from inv in db.InvoiceHeaders.Where(x=>x.InvoiceType==(byte)Type)    
                            select new
                            {
                                inv.ID,
                                inv.Code,
                                inv.Date,
                                inv.PartType,
                                inv.PartID,
                                inv.Branch,
                                ItemsCount = db.InvoiceDetails.Where(x => x.InvoiceID == inv.ID).Count(),
                                inv.PostedToStore,
                                inv.Total,
                                inv.DiscountRation,
                                inv.DiscountValue,
                                inv.Tax,
                                inv.TaxValue,
                                inv.Expences,
                                inv.Net,
                                inv.Paid,
                                inv.Drawer,
                                inv.Remaing,
                                PayStatus =(inv.Remaing==0)? "مسددة": (inv.Remaing == inv.Net) ? "غير مسددة" : "مسددة جزئيا",
                                Prodects = (from itm in db.InvoiceDetails.Where(x => x.InvoiceID == inv.ID)
                                            from pro in db.Products.Where(x => x.ID == itm.ItemID).DefaultIfEmpty()
                                            from unt in db.UnitNames.Where(x => x.ID == itm.ItemUnitID).DefaultIfEmpty()
                                            select new
                                            {
                                                Product = pro.Name,
                                                Unit = unt.Name,
                                                itm.Price,
                                                itm.ItemQty,
                                                itm.DiscountValue,
                                                itm.TotalCostValue
                                            }).ToList(),
                            };

                if (fromDate != null)
                    query = query.Where(x => x.Date.Date >= fromDate.Value.Date);                           // فلترة النتائج بحسب التاريخ
                if (ToDate != null)
                    query = query.Where(x => x.Date.Date <= ToDate.Value.Date);                             // فلترة النتائج بحسب التاريخ
                if (glkp_PartID.IsEditValueValide(false))
                    query = query.Where(x => x.PartID == Convert.ToInt32(glkp_PartID.EditValue));           // فلترة النتائج بحسب طرف التعامل
                if (lkp_PartType.IsEditValueValide(false))
                    query = query.Where(x => x.PartType == Convert.ToInt32(lkp_PartType.EditValue));        // فلترة النتائج بحسب نوع طرف التعامل
                if (lkp_Drawer.IsEditValueValide(false))
                    query = query.Where(x => x.Drawer == Convert.ToInt32(lkp_Drawer.EditValue));            // فلترة النتائج بحسب الخزنة
                if (lkp_Branch.IsEditValueValide(false))
                    query = query.Where(x => x.Branch == Convert.ToInt32(lkp_Branch.EditValue));            // فلترة النتائج بحسب المخزن


                gridControl1.DataSource = query;
                var Parts = new List<DAL.CustomersAndVendore>();                                            // جلب بيانات طرف التعامل
                Parts.AddRange(Session.Customers);
                Parts.AddRange(Session.vendores);

                var ins = query.FirstOrDefault();
                repoBranch.InitializeData(Session.Store, gridView1.Columns[nameof(ins.Branch)], gridControl1);             //لعرض اسم المخزن بدل رقمه
                repoDrawer.InitializeData(Session.Drawer, gridView1.Columns[nameof(ins.Drawer)], gridControl1);            //لعرض اسم الخزنة بدل رقمها
                repoPartType.InitializeData(Master.InvoicesPartTypesList, gridView1.Columns[nameof(ins.PartType)], gridControl1);  //لعرض اسم نوع طرف التعامل بدل رقمه
                repoPart.InitializeData(Parts, gridView1.Columns[nameof(ins.PartID)], gridControl1);                       //لعرض اسم طرف التعامل بدل رقمه

            }
            base.RefreshData();
        }
        private void Frm_InvoiceList_Load(object sender, EventArgs e)
        {
            switch (Type)//تغييرات في الفاتورة حسب نوعها
            {
                case Master.InvoiceType.Purchase:
                    this.Text = "فواتير المشتريات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.ViewPurchaseInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    break;
                case Master.InvoiceType.Sales:
                    this.Text = "فواتير المبيعات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.ViewSalesInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    break;
                case Master.InvoiceType.PurchaseReturn:
                    this.Text = "فواتير مردود المشتريات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.ViewPurchaseReturnInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    break;
                case Master.InvoiceType.SalesReturn:
                    this.Text = "فواتير مردود المبيعات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.ViewSalesReturnInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    break;
                default:
                    throw new NotImplementedException();
            }
            lkp_PartType.InitializeData(Class.Master.InvoicesPartTypesList);// Extention Method /* تصبح الدالة وكأنها اضافة تظهر بعد النقطة بشكل عادي 28 */
            lkp_PartType.Properties.PopulateColumns();
            //lkp_PartType.Properties.Columns["ID"].Visible = false;//في معلومات طرف التعامل ID اخفاء عمود 
            lkp_Branch.InitializeData(Session.Store);
            lkp_Drawer.InitializeData(Session.Drawer);

            dateEdit1.AddClearValueButton();
            dateEdit2.AddClearValueButton();
            lkp_Branch.AddClearValueButton();
            lkp_Drawer.AddClearValueButton();
            lkp_PartType.AddClearValueButton();
            glkp_PartID.AddClearValueButton();

            TranslateCaption();
            
            btn_Refresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btn_Print.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        void TranslateCaption()
        {
            gridView1.ColumnPanelRowHeight = 75;
            gridView1.Columns["ID"].Caption = "كود";
            gridView1.Columns["Code"].Caption = "الرقم";
            gridView1.Columns["Date"].Caption = "التاريخ";
            gridView1.Columns["PartType"].Caption = "طرف التعامل";
            gridView1.Columns["PartID"].Caption = "اسم طرف التعامل";
            gridView1.Columns["Branch"].Caption = "الفرع";
            gridView1.Columns["ItemsCount"].Caption = "عدد الأصناف";
            gridView1.Columns["PostedToStore"].Caption = "مرحل من المخزن";
            gridView1.Columns["Total"].Caption = "الاجمالي";
            gridView1.Columns["DiscountValue"].Caption = "ق الخصم";
            gridView1.Columns["DiscountRation"].Caption = "ن الخصم";
            gridView1.Columns["Tax"].Caption = "ن الضريبة";
            gridView1.Columns["TaxValue"].Caption = "ق الضريبة";
            gridView1.Columns["Expences"].Caption = "مصروفات أخرى";
            gridView1.Columns["Net"].Caption = "الصافي";
            gridView1.Columns["Paid"].Caption = "المدفوع";
            gridView1.Columns["Drawer"].Caption = "الخزنة";
            gridView1.Columns["Remaing"].Caption = "المتبقي";
            gridView1.Columns["PayStatus"].Caption = "حالة السداد";

            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd-MM-yyyy hh:mm:tt"; // تغيير فورمات العرض في العمود الى تاريخ
            gridView1.Columns["Date"].DisplayFormat.FormatType= FormatType.Custom ;       // تغيير فورمات العرض في العمود الى تاريخ
        }

        private void Btn_Apply_Click(object sender, EventArgs e)
        {
            RefreshData();
            //flyoutPanel1.HidePopup();// غلق النافذة
        }

        /// <summary>
        /// دالة تجلب العملاء والموردين في شاشة الفيلتر عند اختيار نوع طرف التعامل
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lkp_PartType_EditValueChanged(object sender, EventArgs e)
        {
            if (lkp_PartType.IsEditValueOfTypeInt())
            {
                int partType = Convert.ToInt32(lkp_PartType.EditValue);
                if (partType == (int)Master.PartType.Customer)
                {
                    glkp_PartID.InitializeData(Session.Customers);
                    lyc_Part.Text = "العميل";
                }
                else if (partType == (int)Master.PartType.Vendor)
                {
                    glkp_PartID.InitializeData(Session.vendores);
                    lyc_Part.Text = "المورد";
                }

            }
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            dateEdit1.EditValue =
            dateEdit2.EditValue =
            lkp_Branch.EditValue =
            lkp_Drawer.EditValue =
            lkp_PartType.EditValue =
            glkp_PartID.EditValue = null;
        }
        /// <summary>
        /// اخفاء نافذة الفيلتر
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_FlyClose_Click(object sender, EventArgs e)
        {
            flyoutPanel1.HidePopup();
        }
        /// <summary>
        /// عرض الفيلتر في التقرير
        /// </summary>
        /// <returns></returns>
        public string GetFilters()
        {
            var str = "";

            if (dateEdit1.EditValue != null)
                str += " | " +  lyc_FromDate.Text + " : " + dateEdit1.Text;
            if (dateEdit2.EditValue != null)
                str += " | " + lyc_ToDate.Text + " : " + dateEdit2.Text;
            if (lkp_Branch.EditValue != null)
                str += " | " + lyc_Branch.Text + " : " + lkp_Branch.Text;
            if (lkp_Drawer.EditValue != null)
                str += " | " + lyc_Drawer.Text + " : " + lkp_Drawer.Text;
            if (lkp_PartType.EditValue != null)
                str += " | " + lyc_PartType.Text + " : " + lkp_PartType.Text;
            if (glkp_PartID.EditValue != null)
                str += " | " + lyc_Part.Text + " : " + glkp_PartID.Text;                       
            return str;
        }

        /// <summary>
        /// طباعة الفورم في التقرير
        /// </summary>
        public override void Print()
        {
            Reporting.rpt_GridPrint.Print(gridControl1,"كشف "+ this.Text, GetFilters());
            //PartName = this.Name; // لتسجيل حركات المستخدم
            //base.Print();
        }


    }
}
