using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using SalesLinq.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static SalesLinq.Class.MasterFinance;

namespace SalesLinq.Forms
{
    public partial class frm_Invoice : frm_Master
    {
        DAL.InvoiceHeader Invoice;
        Master.InvoiceType Type;
        DAL.DBDataContext generalDB;

        RepositoryItemGridLookUpEdit repoItems;
        RepositoryItemLookUpEdit repoItemsAll;
        RepositoryItemLookUpEdit repoUOM;
        RepositoryItemLookUpEdit repoStores;

        public frm_Invoice(Master.InvoiceType _Type)
        {
            InitializeComponent();
            InitialiseEarlyEvents();
            Type = _Type;
            SetFormType();
            RefreshData();
            New();
        }

        public frm_Invoice(Master.InvoiceType _Type, int id)
        {
            InitializeComponent();
            InitialiseEarlyEvents();
            Type = _Type;
            SetFormType();
            RefreshData();
            using (var db = new DAL.DBDataContext())
            {
                Invoice = db.InvoiceHeaders.Single(x => x.ID == id);
                GetData();
                IsNew = false;
            };
        }
        /// <summary>
        /// اضافة احداث في الفاتورة
        /// </summary>
        void InitialiseEarlyEvents()
        {
            gridView1.ValidateRow += GridView1_ValidateRow;//حدث للتأكد من القيمة المدخة
            gridView1.InvalidRowException += GridView1_InvalidRowException;// حدث رسالة الخطأ
            lkp_PartType.EditValueChanged += Lkp_PartType_EditValueChanged;// حدث تغيير نوع طرف التعامل
            glkp_PartID.EditValueChanged += Glkp_PartID_EditValueChanged;//حدث تغيير طرف التعامل

        }

        DAL.InvoiceDetail detailsInstace = new DAL.InvoiceDetail(); // من أجل استخراج اسماء الحقول منه
        /// <summary>
        /// تغييرات في الفاتورة حسب نوعها
        /// </summary>
        void SetFormType()
        {
            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    this.Text = "فاتورة مشتريات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.AddPurchaseInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    chk_PostToStore.Enabled = false;
                    chk_PostToStore.Checked = true;
                    break;
                case Master.InvoiceType.Sales:
                    this.Text = "فاتورة مبيعات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.AddSalesInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    break;
                case Master.InvoiceType.PurchaseReturn:
                    this.Text = "فاتورة مردود مشتريات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.AddPurchaseReturnInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    chk_PostToStore.Enabled = false;
                    chk_PostToStore.Checked = true;
                    lyc_SourceID.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;//اظهار زر المصدر في شاشة مردود المشتريات
                    lyc_SelectSourceItems.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;//اظهار زر اظافة عناصر الفاتورة في شاشة مردود المبيعات
                    break;
                case Master.InvoiceType.SalesReturn:
                    this.Text = "فاتورة مردود مبيعات";// تسمية الفورم حسب نوع الفاتورة
                    this.Name = Class.Screens.AddSalesReturnInvoice.ScreenName;// تغيير اسم فورم الفاتورة
                    chk_PostToStore.Enabled = false;
                    chk_PostToStore.Checked = true;
                    lyc_SourceID.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;//اظهار زر المصدر في شاشة مردود المبيعات
                    lyc_SelectSourceItems.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;//اظهار زر اظافة عناصر الفاتورة في شاشة مردود المبيعات
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        void SetUpGridViewByInvoiceType()
        {
            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    gridView1.Columns[nameof(detailsInstace.StoreID)].OptionsColumn.AllowFocus = false;// منع تغيير المخزن 
                    break;
                case Master.InvoiceType.Sales:
                    break;
                case Master.InvoiceType.PurchaseReturn:
                case Master.InvoiceType.SalesReturn:
                    gridView1.Columns[nameof(detailsInstace.StoreID)].OptionsColumn.AllowFocus = false;// منع تغيير المخزن       
                    gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;               //لمنع اضافة اسطر
                    AddReturnColumns();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// دالة لاضافة عمودين لشاشة فاتورة المرتجع
        /// </summary>
        void AddReturnColumns()
        {
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "clmSourceQty",
                FieldName = "SourceQty",
                Caption = "الكمية الاساسية",

                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                VisibleIndex = gridView1.Columns[nameof(detailsInstace.ItemQty)].VisibleIndex - 1
            });
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "clmOtherQty",
                FieldName = "OtherQty",
                Caption = "كميات أخرى",

                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                VisibleIndex = gridView1.Columns[nameof(detailsInstace.ItemQty)].VisibleIndex - 1
            });

            gridView1.Columns["OtherQty"].OptionsColumn.AllowEdit = gridView1.Columns["SourceQty"].OptionsColumn.AllowEdit = false;
        }

        private void Frm_Invoice_Load(object sender, EventArgs e)
        {
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;// للسماح باضافة سطر جديد من قبل المستخدم
            SetUpGridViewByInvoiceType();

            btn_Print.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            lkp_PartType.InitializeData(Class.Master.InvoicesPartTypesList);// Extention Method /* تصبح الدالة وكأنها اضافة تظهر بعد النقطة بشكل عادي 28 */
            lkp_PartType.Properties.PopulateColumns();
            lkp_PartType.Properties.Columns["ID"].Visible = false;//في معلومات طرف التعامل ID اخفاء عمود 
            glkp_PartID.ButtonClick += Lkp_PartType_ButtonClick;//انشاء حدث الضغط على الزر + plus الأخضر
            glkp_PartID.Properties.ValidateOnEnterKey = true;//Enter يقوم بالادخال عند الضغط على 
            glkp_PartID.Properties.AllowNullInput = DefaultBoolean.False;
            glkp_PartID.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;// لعرض الأصناف بقائمة كاملة
            glkp_PartID.Properties.ImmediatePopup = true;//السماح بالبحث في الأصناف الاكمال التلقائي
            glkp_PartID.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));// اضافة الزر + plus الأخضر
            var PartIDView = glkp_PartID.Properties.View;// الجدول الذي يعرض الأصناف
            PartIDView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;//اختيار الصف كاملا
            PartIDView.OptionsSelection.UseIndicatorForSelection = true;// اختيار السطر من الانديكايتور في الجانب
            PartIDView.OptionsView.ShowAutoFilterRow = true;//اظهار خانة البحث أوتوماتيكيا
            PartIDView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            PartIDView.PopulateColumns(glkp_PartID.Properties.DataSource); // 
            //PartIDView.Columns["IsActive"].Visible = false; // لاخفاء IsActive في قائمة الأصناف
            //PartIDView.Columns["Type"].Visible = false; // لاخفاء Type في قائمة الأصناف



            gridView1.Columns[nameof(detailsInstace.ID)].Visible = false; //ID اخفاء عمود 
            gridView1.Columns[nameof(detailsInstace.InvoiceID)].Visible = false; //InvoiceID اخفاء عمود 

            repoUOM.InitializeData(Session.UnitNames, gridView1.Columns[nameof(detailsInstace.ItemUnitID)], gridControl1);
            repoStores = new RepositoryItemLookUpEdit();
            repoStores.InitializeData(Session.Store, gridView1.Columns[nameof(detailsInstace.StoreID)], gridControl1);

            repoItems = new RepositoryItemGridLookUpEdit();
            repoItems.InitializeData(Session.ProductView.Where(x => x.IsActive == true), gridView1.Columns[nameof(detailsInstace.ItemID)], gridControl1);// عرض الأصناف النشطة فقط
            repoItems.ValidateOnEnterKey = true;//Enter يقوم بالادخال عند الضغط على 
            repoItems.AllowNullInput = DefaultBoolean.False;
            repoItems.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;// لعرض الأصناف بقائمة كاملة
            repoItems.ImmediatePopup = true;//السماح بالبحث في الأصناف الاكمال التلقائي
            repoItems.Buttons.Add(new EditorButton(ButtonPredefines.Plus));// اضافة الزر + plus الأخضر
            repoItems.ButtonClick += RepoItems_ButtonClick;//انشاء حدث الضغط على الزر + plus الأخضر
            var repoView = repoItems.View;// الجدول الذي يعرض الأصناف
            repoView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;//اختيار الصف كاملا
            repoView.OptionsSelection.UseIndicatorForSelection = true;// اختيار السطر من الانديكايتور في الجانب
            repoView.OptionsView.ShowAutoFilterRow = true;//اظهار خانة البحث أوتوماتيكيا
            repoView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            repoView.PopulateColumns(repoItems.DataSource); // 
            repoView.Columns["IsActive"].Visible = false; // لاخفاء IsActive في قائمة الأصناف
            repoView.Columns["Type"].Visible = false; // لاخفاء Type في قائمة الأصناف

            //تسمية رؤوس الأعمدة
            repoView.Columns["ID"].Visible = false;
            repoView.Columns["Code"].Visible = false;
            repoView.Columns["Name"].Caption = "الاسم";
            repoView.Columns["Descreption"].Caption = "الوصف";
            repoView.Columns["CategoryName"].Caption = "الفئة";

            repoItemsAll.InitializeData(Session.ProductView, gridView1.Columns[nameof(detailsInstace.ItemID)], gridControl1);// ملأ القائمة بكل الأصناف لكن يتم عرض الاصناف النشطة فقط -برمجتها في كود أخر-...ا

            RepositoryItemSpinEdit spinEdit = new RepositoryItemSpinEdit();
            gridView1.Columns[nameof(detailsInstace.TotalPrice)].ColumnEdit = spinEdit;//TotalPrice اضافة الريبوزيتوري ايتم الى الغريد فيو في خانة 
            gridView1.Columns[nameof(detailsInstace.Price)].ColumnEdit = spinEdit;//Price اضافة الريبوزيتوري ايتم الى الغريد فيو في خانة 
            gridView1.Columns[nameof(detailsInstace.ItemQty)].ColumnEdit = spinEdit;//ItemQty اضافة الريبوزيتوري ايتم الى الغريد فيو في خانة 
            gridView1.Columns[nameof(detailsInstace.DiscountValue)].ColumnEdit = spinEdit;//DiscountValue اضافة الريبوزيتوري ايتم الى الغريد فيو في خانة 

            RepositoryItemSpinEdit spinRatioEdit = new RepositoryItemSpinEdit();
            gridView1.Columns[nameof(detailsInstace.Discount)].ColumnEdit = spinRatioEdit;//Discount اضافة الريبوزيتوري ايتم الى الغريد فيو في خانة 

            spinRatioEdit.Increment = 0.01m;// القيمة التي يأخذها عند تدوير عدلة الفأرة
            spinRatioEdit.EditMask = "p";//نوع الماسك الخاص بالنسبة المئوية
            spinRatioEdit.Mask.UseMaskAsDisplayFormat = true;// استخدام الماسك في العرض
            spinRatioEdit.MaxValue = 1;//القيمة العليا للخلية 

            gridControl1.RepositoryItems.Add(spinRatioEdit); // اضافة الريبوزيتوري ايتم الى الغريد كونترول
            gridControl1.RepositoryItems.Add(spinEdit); // اضافة الريبوزيتوري ايتم الى الغريد كونترول

            gridView1.Columns[nameof(detailsInstace.TotalPrice)].OptionsColumn.AllowFocus = false;// منع التركيز على عمود المجموع 
            // اضافة عمود الباركود
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "clmCode",
                FieldName = "Code",
                Caption = "الكود",
                UnboundType = DevExpress.Data.UnboundColumnType.String
            });
            // اضافة عمود المسلسل
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "clmIndex",
                FieldName = "Index",
                Caption = "مسلسل",
                UnboundType = DevExpress.Data.UnboundColumnType.Integer,
                MaxWidth = 30
            });
            // اضافة عمود الرصيد
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "clmBalance",
                FieldName = "Balance",
                Caption = "الرصيد",
                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                MaxWidth = 30
            });
            //تسمية رؤوس الأعمدة
            gridView1.Columns[nameof(detailsInstace.ItemID)].Caption = "الصنف";
            gridView1.Columns[nameof(detailsInstace.CostValue)].Caption = "سعر التكلفة";
            gridView1.Columns[nameof(detailsInstace.Discount)].Caption = "ن.خصم";
            gridView1.Columns[nameof(detailsInstace.DiscountValue)].Caption = "ق.خصم";
            gridView1.Columns[nameof(detailsInstace.ItemQty)].Caption = "الكمية";
            gridView1.Columns[nameof(detailsInstace.ItemUnitID)].Caption = "الوحدة";
            gridView1.Columns[nameof(detailsInstace.Price)].Caption = "السعر";
            gridView1.Columns[nameof(detailsInstace.StoreID)].Caption = "المخزن";
            gridView1.Columns[nameof(detailsInstace.TotalCostValue)].Caption = "اجمالي التكلفة";
            gridView1.Columns[nameof(detailsInstace.TotalPrice)].Caption = "اجمالي السعر";
            //منع التحديد في عمود المسلسل
            gridView1.Columns["Index"].OptionsColumn.AllowFocus = false;
            //منع التحديد في عمود التكلفة والتكلفة الكلية
            gridView1.Columns[nameof(detailsInstace.CostValue)].OptionsColumn.AllowFocus = false;
            gridView1.Columns[nameof(detailsInstace.TotalCostValue)].OptionsColumn.AllowFocus = false;
            // ترتيب الأعمدة
            gridView1.Columns["Index"].VisibleIndex = 0;
            gridView1.Columns["Index"].Width = 30;
            gridView1.Columns["Code"].VisibleIndex = 1;
            gridView1.Columns["Code"].Width = 50;
            gridView1.Columns[nameof(detailsInstace.ItemID)].VisibleIndex = 2;
            gridView1.Columns[nameof(detailsInstace.ItemID)].MinWidth = 125;
            gridView1.Columns[nameof(detailsInstace.ItemUnitID)].VisibleIndex = 3;
            gridView1.Columns[nameof(detailsInstace.ItemUnitID)].Width = 60;
            gridView1.Columns[nameof(detailsInstace.ItemQty)].VisibleIndex = 4;
            gridView1.Columns[nameof(detailsInstace.ItemQty)].Width = 50;
            gridView1.Columns[nameof(detailsInstace.Price)].VisibleIndex = 5;
            gridView1.Columns[nameof(detailsInstace.Price)].Width = 60;
            gridView1.Columns[nameof(detailsInstace.Discount)].VisibleIndex = 6;
            gridView1.Columns[nameof(detailsInstace.Discount)].Width = 80;
            gridView1.Columns[nameof(detailsInstace.DiscountValue)].VisibleIndex = 7;
            gridView1.Columns[nameof(detailsInstace.DiscountValue)].MinWidth = 30;
            gridView1.Columns[nameof(detailsInstace.TotalPrice)].VisibleIndex = 8;
            gridView1.Columns[nameof(detailsInstace.TotalPrice)].MinWidth = 30;
            gridView1.Columns[nameof(detailsInstace.CostValue)].VisibleIndex = 9;
            gridView1.Columns[nameof(detailsInstace.CostValue)].MinWidth = 30;
            gridView1.Columns[nameof(detailsInstace.TotalCostValue)].VisibleIndex = 10;
            gridView1.Columns[nameof(detailsInstace.TotalCostValue)].MinWidth = 30;
            gridView1.Columns[nameof(detailsInstace.StoreID)].VisibleIndex = 11;
            gridView1.Columns[nameof(detailsInstace.StoreID)].MinWidth = 100;
            gridView1.Columns["Balance"].VisibleIndex = 12;
            gridView1.Columns["Balance"].MinWidth = 45;
            gridView1.Columns["Balance"].OptionsColumn.AllowFocus = false;
            gridView1.Columns[nameof(detailsInstace.sourceRowID)].Visible = false;

            //لتغيير لون الاسطر بالتناوب
            gridView1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196);// لون خاص بالاسطر الفردية
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.Appearance.OddRow.BackColor = Color.WhiteSmoke;//لون خاص بالاسطر الزوجية
            gridView1.OptionsView.EnableAppearanceOddRow = true;

            //اضافة ريبوزيتوري ايتم من أجل أن يحتوي على زر الحذف 
            RepositoryItemButtonEdit buttonEdit = new RepositoryItemButtonEdit();
            gridControl1.RepositoryItems.Add(buttonEdit);
            buttonEdit.Buttons.Clear();
            buttonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
            buttonEdit.ButtonClick += ButtonEdit_ButtonClick;
            // انشاء زر الحذف 
            GridColumn clmnDelete = new GridColumn()
            {
                Name = "clmnDelete",
                Caption = "حذف",
                FieldName = "Delete",
                ColumnEdit = buttonEdit,
                VisibleIndex = 20,
                Width = 30,
                MaxWidth = 30
            };
            //لاظهار زر الحذف بدون نص
            buttonEdit.TextEditStyle = TextEditStyles.HideTextEditor;
            // اضافة زر الحذف في الجدول في اخر السطر
            gridView1.Columns.Add(clmnDelete);

            ReadUserSettings();
            if (!Debugger.IsAttached)
            {
                gridView1.RestorLayouts(this.Name);// استرجاع شكل شاشة جدول الفاتورة اثناء فتحها
                layoutControl1.RestoreLayout(this.Name); //استرجاع شكل شاشة الفاتورة اثناء فتحها
            }


            btn_CustomizeLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;               // اظهار زر التعديل على شاشة الفاتورة
            btn_CustomizeLayout.ItemClick += (ss, ee) => { layoutControl1.ShowCustomizationForm(); };    // فتح نافذة التعديل على شاشة الفاتورة
            layoutControl1.CustomizationMode = DevExpress.XtraLayout.CustomizationModes.Quick;           // تعديل مود نافذة التعديل
            layoutControl1.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView; // طريقة العرض في نافذة التعديل
            layoutControl1.OptionsCustomizationForm.ShowSaveButton =
            layoutControl1.OptionsCustomizationForm.ShowLoadButton = false;                              // اخفاء زر الحفظ والتحميل في نافذة التعديل
            //layoutControl1.OptionsCustomizationForm.ShowPropertyGrid = true;                             // اظهار الخصائص في نافذة التعديل
            foreach (BaseLayoutItem item in layoutControl1.Items)
            {
                item.AllowHide = false;               // لمنع اخفاء العناصر من قبل المستخدم عند التعديل على شكل شاشة الفاتورة
            }
            glkp_Initial();
            var storeINS = Session.Store.FirstOrDefault();
            lkp_Branch.Properties.PopulateColumns();
            lkp_Branch.Properties.Columns[nameof(storeINS.ID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.CostOfSoldGoodsAccountID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.DiscountAllowedAccountID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.DiscountReceivedAccountID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.InventoryAccountID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.SalesAccountID)].Visible =
            lkp_Branch.Properties.Columns[nameof(storeINS.SalesReturnAccountID)].Visible = false;
            lkp_Branch.Properties.Columns[nameof(storeINS.Name)].Caption = "الاسم";
            // ترجمة الاعمدة الخاصة بالمخزن في خانة اختيار العناصر في الفاتورة
            repoStores.PopulateColumns();
            repoStores.Columns[nameof(storeINS.ID)].Visible =
            repoStores.Columns[nameof(storeINS.CostOfSoldGoodsAccountID)].Visible =
            repoStores.Columns[nameof(storeINS.DiscountAllowedAccountID)].Visible =
            repoStores.Columns[nameof(storeINS.DiscountReceivedAccountID)].Visible =
            repoStores.Columns[nameof(storeINS.InventoryAccountID)].Visible =
            repoStores.Columns[nameof(storeINS.SalesAccountID)].Visible =
            repoStores.Columns[nameof(storeINS.SalesReturnAccountID)].Visible = false;
            repoStores.Columns[nameof(storeINS.Name)].Caption = "الاسم";



            #region Events
            //spn_DiscountValue.Enter += Spn_DiscountValue_Enter;
            spn_DiscountValue.Enter += new System.EventHandler(this.Spn_DiscountValue_Enter);
            spn_DiscountValue.Leave += Spn_DiscountValue_Leave;
            spn_DiscountValue.EditValueChanged += Spn_DiscountValue_EditValueChanged;// دالة حساب الخصم ونسبته اثناء الكتابة
            spn_DiscountRation.EditValueChanged += Spn_DiscountValue_EditValueChanged;// دالة حساب الخصم ونسبته اثناء الكتابة

            spn_TaxValue.Enter += Spn_TaxValue_Enter;
            spn_TaxValue.Leave += Spn_TaxValue_Leave;
            spn_TaxValue.EditValueChanged += Spn_TaxValue_EditValueChanged;// دالة حساب الضريبة ونسبتها اثناء الكتابة
            spn_Tax.EditValueChanged += Spn_TaxValue_EditValueChanged;// دالة حساب الضريبة ونسبتها اثناء الكتابة

            spn_TaxValue.EditValueChanged += Spn_EditValueChanged;// دالة حساب الصافي
            spn_DiscountValue.EditValueChanged += Spn_EditValueChanged;// دالة حساب الصافي
            spn_Expences.EditValueChanged += Spn_EditValueChanged;// دالة حساب الصافي
            spn_Total.EditValueChanged += Spn_EditValueChanged;// دالة حساب الصافي

            spn_Paid.EditValueChanged += Spn_Paid_EditValueChanged;// دالة حساب المتبقي
            spn_Net.EditValueChanged += Spn_Paid_EditValueChanged;// دالة حساب المتبقي
            spn_Net.EditValueChanging += Spn_Net_EditValueChanging;// دالة ادخال المدفوع اليا اذا لم يتم تغييره يدويا

            spn_Net.DoubleClick += Spn_Net_MouseDoubleClick;// حدث الضغط على الصافي مرتين

            lkp_Branch.EditValueChanging += Lkp_Branch_EditValueChanging;//Lkp_Branch حدث تغيير الفرع 
            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;// دالة لعرض الوحدات الخاصة بالصنف فقط
            gridView1.CellValueChanged += GridView1_CellValueChanged;// حدث التغير في قيمة الخلية
            gridView1.CellValueChanging += GridView1_CellValueChanging;//حدث التغيير في قيمة الخلية
            gridView1.RowCountChanged += GridView1_RowCountChanged;// حدث حساب الاجمالي عند اضافة سطر أو حذفه 
            gridView1.RowUpdated += GridView1_RowUpdated;// حدث حساب الاجمالي عند تغيير القيمة في أي الخلية  
            gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
            gridControl1.ProcessGridKey += GridControl1_ProcessGridKey;// حدث الضغط على زر في الغريد كونترول
            glkp_Source.EditValueChanged += Glkp_Source_EditValueChanged; // حدث تغير القيمة في زر المصدر

            gridView_ReturnInvoices.RowCountChanged += GridView_ReturnInvoices_RowCountChanged; // حدث تغير اسطر جدول مردود الفاتورة
            GridView_ReturnInvoices_RowCountChanged(gridView_ReturnInvoices, new EventArgs());
            gridView_ReturnInvoices.DoubleClick += GridView_ReturnInvoices_DoubleClick;

            this.Activated += Frm_Invoice_Activated;// حدث تنشيط الفورم
            this.KeyPreview = true;//السماح باحداث لوحة المفاتيح
            this.KeyDown += Frm_Invoice_KeyDown;//حدث الضغط على زر في لوحة المفاتيح
            this.FormClosing += Frm_Invoice_FormClosing;// حدث اغلاق الفورم
            #endregion

        }
        /// <summary>
        /// دالة فتح فاتورة المردودات من خلال الفاتورة الأصلية
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ReturnInvoices_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            // لمعرفة ماذا يوجد في هذا الموقع اذا كان سطر أو غير ذلك
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)//للتأكد من أن المستخدم ضغط على خلية في الصف
            {
                frm_Main.OpenForm(new frm_Invoice((Type== Master.InvoiceType.Purchase) ? Master.InvoiceType.PurchaseReturn: Master.InvoiceType.SalesReturn, (int) gridView_ReturnInvoices.GetFocusedRowCellValue("ID")));
            }
        }

        void glkp_Initial()
        {
            var customerINS = Session.Customers.FirstOrDefault();
            glkp_PartID.Properties.View.Columns[nameof(customerINS.Name)].Caption = "الاسم";
            glkp_PartID.Properties.View.Columns[nameof(customerINS.Address)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.Mobile)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.Phone)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.AccountID)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.MaxCredit)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.ID)].Visible =
            glkp_PartID.Properties.View.Columns[nameof(customerINS.IsCustomer)].Visible = false;
        }
        /// <summary>
        /// دالة لاخفاء جدول مردود الفاتورة اذا كانت فارغة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ReturnInvoices_RowCountChanged(object sender, EventArgs e)
        {
            lyc_ReturnInvoices.Visibility = lyc_GridView_ReturnInvoices.Visibility = (gridView_ReturnInvoices.RowCount > 0) ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        /// <summary>
        /// دالة لجلب عناصر رأس الفاتورة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Glkp_Source_EditValueChanged(object sender, EventArgs e)
        {
            // تفريغ شاشة الفاتورة عند تغيير طرف التعامل
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.SelectAll();
            gridView1.DeleteSelectedRows();
            gridView1.OptionsSelection.MultiSelect = false;

            if (glkp_Source.EditValue is int sourceID && sourceID > 0)//sourceID اذا كانت تحتوي على قيمة رقمية يضعها في المتغير 
            {
                using (DAL.DBDataContext db = new DAL.DBDataContext())
                {
                    ReturnSourceDetails = db.InvoiceDetails.Where(x => x.InvoiceID == sourceID).ToList();//جلب بيانات الفاتورة الأصلية 
                    DAL.InvoiceHeader SourceInvoice = db.InvoiceHeaders.Single(x => x.ID == sourceID);
                    spn_DiscountRation.EditValue = SourceInvoice.DiscountRation;                         //يجلب قيمة الخصم
                    spn_Tax.EditValue = SourceInvoice.Tax;                                               //يجلب قيمة الضريبة
                    btn_SelectItems.PerformClick();
                }
            }

        }

        /// <summary>
        /// دالة تجلب بيانات التغييرات التي قام بها المستخدم 
        /// </summary>
        public override void GetHistory()
        {
            if (IsNew) return;
            var mainScreenID = Class.Screens.GetScreens.SingleOrDefault(x => x.ScreenName == this.Name).ScreenID;
            var listScreen = Class.Screens.GetScreens.SingleOrDefault(x => x.ScreenName == this.Name + "List");
            int listScreenID = mainScreenID;
            if (listScreen != null)
                listScreenID = listScreen.ScreenID;

            using (var db = new DAL.DBDataContext())
            {
                var data = db.UserLogs.Where(x =>
                x.PartID == Invoice.ID &&
                (x.ScreenID == mainScreenID || x.ScreenID == listScreenID));

                gridControl2.DataSource = (from d in data.OrderByDescending(x => x.ActionDate)
                                           join u in db.Users on d.UserID equals u.ID
                                           select new
                                           {
                                               u.Name,
                                               d.ActionDate,
                                               Action = (d.ActionType == (byte)ActionType.Add) ? "اضافة" :
                                               (d.ActionType == (byte)ActionType.Delete) ? "حذف" :
                                               (d.ActionType == (byte)ActionType.Edit) ? "تعديل" :
                                               (d.ActionType == (byte)ActionType.Print) ? "طباعة" : string.Empty,

                                           }).ToList();
            }
            gridView_ActionHistory.Columns["Action"].Caption = "الحدث";
            gridView_ActionHistory.Columns["Name"].Caption = "المستخدم";
            gridView_ActionHistory.Columns["ActionDate"].Caption = "التاريخ";
            gridView_ActionHistory.Columns["ActionDate"].DisplayFormat.FormatType = FormatType.Custom;
            gridView_ActionHistory.Columns["ActionDate"].DisplayFormat.FormatString = "yyyy-MM-dd hh:mm:ss tt";
            gridView_ActionHistory.RowStyle += GridView_ActionHistory_RowStyle;
        }

        private void GridView_ActionHistory_RowStyle(object sender, RowStyleEventArgs e)
        {
            var view = sender as GridView;
            var action = view.GetRowCellValue(e.RowHandle, "Action");
            if (action == null) return;
            if (action.ToString() == "اضافة")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            else if (action.ToString() == "حذف")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            else if (action.ToString() == "تعديل")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            else if (action.ToString() == "طباعة")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            e.HighPriority = true;
        }


        /// <summary>
        /// حفظ شكل شاشة الفاتورة أثناء الاغلاق
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_Invoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridView1.SaveLayouts(this.Name);// حفظ شكل شاشة جدول الفاتورة اثناء اغلاقها
            layoutControl1.SaveLayout(this.Name); //حفظ شكل شاشة الفاتورة اثناء اغلاقها 

        }

        AccountBalance accountBalance;
        /// <summary>
        /// دالة تجلب معلومات عن طرف التعامل
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Glkp_PartID_EditValueChanged(object sender, EventArgs e)
        {

            var id = Convert.ToInt32(glkp_PartID.EditValue);

            if (id != 0)
            {
                DAL.CustomersAndVendore account;

                if (Convert.ToByte(lkp_PartType.EditValue) == (byte)Master.PartType.Vendor)
                {
                    account = Session.vendores.SingleOrDefault(x => x.ID == id);
                }
                else
                {
                    account = Session.Customers.SingleOrDefault(x => x.ID == id);
                }
                if (account == null)
                    goto IFEmpty;
                txt_PartAddress.Text = account.Address;
                txt_PartMaxCredit.EditValue = account.MaxCredit;
                txt_PartPhone.Text = account.Phone;
                accountBalance = GetAccountBalance(account.AccountID);
                txt_PartBalance.Text = accountBalance.Balance;
                if (Type == Master.InvoiceType.SalesReturn || Type == Master.InvoiceType.PurchaseReturn)
                {
                    // جلب فواتير طرف التعامل
                    using (DAL.DBDataContext db = new DAL.DBDataContext())
                    {
                        var SourceInvoices = db.InvoiceHeaders
                            .Where(x => x.InvoiceType == ((Type == Master.InvoiceType.SalesReturn) ? (byte)Master.InvoiceType.Sales
                            : (byte)Master.InvoiceType.Purchase) &&
                            x.PartID == id && x.PartType == Convert.ToByte(lkp_PartType.EditValue)).Select(x => new { x.ID, x.Code }).ToList();
                        glkp_Source.InitializeData(SourceInvoices, "Code", "ID");
                        glkp_Source.EditValue = null;
                    }
                }

                return;
            }
            else
            {
                goto IFEmpty;
            }
        IFEmpty:
            txt_PartMaxCredit.EditValue = 0;
            txt_PartPhone.Text =
            txt_PartBalance.Text =
            txt_PartAddress.Text = string.Empty;
        }

        /// <summary>
        /// حتى يتم تغيير الوحدة عند تغيير صناف مضاف مسبقا
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ItemID")
            {
                var row = gridView1.GetRow(e.RowHandle) as DAL.InvoiceDetail;
                if (row != null)
                {
                    if (row.ItemID != 0 && e.Value.Equals(row.ItemID) == false)
                        row.ItemUnitID = 0;
                }
            }

        }

        private void RepoItems_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Plus)
                frm_Main.OpenFormByName(nameof(frm_Product));
        }

        #region GridViewStuff
        /// <summary>
        ///Shift + F حدث الضغط على 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_Invoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                MoveFocuseToGrid(e.Modifiers == Keys.Shift);
            }
            else if (e.KeyCode == Keys.F6)
            {
                //TODO Go to Produce By index
            }
            else if (e.KeyCode == Keys.F7)
                glkp_PartID.Focus();
            else if (e.KeyCode == Keys.F8)
                txt_Code.Focus();
            else if (e.KeyCode == Keys.F9)
                spn_DiscountValue.Focus();
            else if (e.KeyCode == Keys.F10)
                spn_TaxValue.Focus();
            else if (e.KeyCode == Keys.F11)
                spn_Expences.Focus();
            else if (e.KeyCode == Keys.F12)
                spn_Paid.Focus();
        }

        /// <summary>
        /// حدث الضغط على زر الحذف
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
            if (view.FocusedRowHandle >= 0)
            {
                view.DeleteSelectedRows();
            }
        }

        /// <summary>
        /// حدث تنشيط الفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_Invoice_Activated(object sender, EventArgs e)
        {
            MoveFocuseToGrid();
        }

        /// <summary>
        /// منع رسالة الخطأ من الظهور
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            if (e.Row == null || (e.Row as DAL.InvoiceDetail).ItemID == 0)
            {
                gridView1.DeleteRow(e.RowHandle);
                e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.Ignore;
                return;
            }
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        /// <summary>
        /// التأكد من القيمة المدخة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            var row = e.Row as DAL.InvoiceDetail;
            if (row == null || row.ItemID == 0)
            {
                e.Valid = false;
                return;
            }

            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    break;
                case Master.InvoiceType.Sales:
                    if (row.CostValue > 0)
                    {
                        if (row.CostValue > row.Price)
                        {
                            switch (Session.UserSettings.Sales.WhenSellingItemWithPriceLowerThanCostPrice)
                            {
                                case Master.WorningLevels.DoNotEnteript:
                                    break;
                                case Master.WorningLevels.Prevent:
                                    e.Valid = false;
                                    gridView1.SetColumnError(gridView1.Columns[nameof(row.Price)], "سعر البيع أقل من سعر التكلفة");

                                    break;
                                case Master.WorningLevels.ShowWarning:
                                    if (XtraMessageBox.Show(text: "سعر البيع أقل من سعر التكلفة ... هل تريد المتابعة",
                                        caption: string.Empty,
                                        icon: MessageBoxIcon.Question,
                                        buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        e.Valid = false;
                                        gridView1.SetColumnError(gridView1.Columns[nameof(row.Price)], "سعر البيع أقل من سعر التكلفة");
                                    }
                                    break;

                            }
                        }
                    }
                    if (Convert.ToDecimal(row.Discount) > Session.UserSettings.Sales.MaxDiscountPerItem)
                    {
                        e.Valid = false;
                        gridView1.SetColumnError(gridView1.Columns[nameof(row.Discount)], "نسبة الخصم غير مسموحة");
                    }
                    break;
                case Master.InvoiceType.SalesReturn:
                case Master.InvoiceType.PurchaseReturn:
                    // validate that qty is not more than qty in the souce
                    double? OtherQty = gridView1.GetRowCellValue(e.RowHandle, "OtherQty") as double?;
                    double? SourceQty = gridView1.GetRowCellValue(e.RowHandle, "SourceQty") as double?;

                    if (row.ItemQty > Convert.ToDouble((SourceQty ?? 0) - (OtherQty ?? 0)))
                    {
                        e.Valid = false;
                        gridView1.SetColumnError(gridView1.Columns[nameof(row.ItemQty)], "لا يمكن للكمية ان تكون أكبر من الكمية المتاحة من المصدر");
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// هندلة الضغط على زر من لوحة المفاتيح في الغريد كونترول
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            GridControl control = sender as GridControl;
            if (control == null) return;
            GridView view = control.FocusedView as GridView;
            if (view == null) return;
            if (view.FocusedColumn == null) return;
            if (e.KeyCode == Keys.Return)//enter في حالة الضغط على الزر 
            {
                string focusedColumn = view.FocusedColumn.FieldName;
                if (view.FocusedColumn.FieldName == "Code" || view.FocusedColumn.FieldName == "ItemID")
                {
                    GridControl1_ProcessGridKey(sender, new KeyEventArgs(Keys.Tab));// ادخال الصنف في الفاتورة والرجوع لمكان الكود
                }
                if (view.FocusedRowHandle < 0)
                {
                    view.AddNewRow();
                    view.FocusedColumn = view.Columns[focusedColumn];
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                view.FocusedColumn = view.VisibleColumns[view.FocusedColumn.VisibleIndex - 1];
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control)// لحذف الصنف من لوحة المفاتيح
            {

                if (view.FocusedRowHandle >= 0)
                {
                    view.DeleteSelectedRows();
                }

                //if (XtraMessageBox.Show(
                //    text: "هل تريد ازالة الصنف المحدد من الفاتورة",
                //    caption: "تأكيد الحذف",
                //    icon: MessageBoxIcon.Question,
                //    buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                //    var datasourceRowHandel = view.GetDataSourceRowIndex(view.FocusedRowHandle);
                //    var rows = view.DataSource as Collection<DAL.InvoiceDetail>;
                //    var row = rows[datasourceRowHandel];
                //    rows.RemoveAt(datasourceRowHandel);
                //    e.Handled = true;
                //}

            }
        }

        string enteredCode;
        /// <summary>
        /// دالة لاضافة قيمة أو قرائتها
        /// من خلال الأعمدة غير المربوطة بقاعدة البيانات
        /// مثل عمود الكودبار
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Code")
            {
                if (e.IsSetData)
                {
                    enteredCode = e.Value.ToString();
                }
                else if (e.IsGetData)
                {
                    e.Value = enteredCode;
                }
            }
            else if (e.Column.FieldName == "Index")
                e.Value = gridView1.GetVisibleRowHandle(e.ListSourceRowIndex) + 1;//تعبئة عمود المسلسل
            else if (e.Column.FieldName == "Balance")
            {
                var row = e.Row as DAL.InvoiceDetail;
                var rowindex = e.ListSourceRowIndex;//رقم السطر
                if (row == null || row.ItemID == 0 || row.StoreID == 0 || row.ItemUnitID == 0)
                {
                    e.Value = null;
                    return;
                }
                var proBalance = Session.ProductsBalances.FirstOrDefault(x => x.ProductID == row.ItemID && x.StoreID == row.StoreID);
                if (proBalance == null)
                {
                    e.Value = 0;
                    return;
                }
                var balance = proBalance.Balance;
                var product = Session.ProductView.Single(x => x.ID == row.ItemID);
                var factor = product.Units.Single(x => x.UnitID == row.ItemUnitID).Factor;

                ///انقاص كمية العنصر داخل نفس الفاتورة اذا كان من نفس المخزن                
                var rows = (gridView1.DataSource as Collection<DAL.InvoiceDetail>).Take(rowindex).Where(x => x.ItemID == row.ItemID && x.StoreID == row.StoreID);
                double otherBalance = 0;

                ///الانقاص يكون سطرا يسطر
                foreach (var item in rows)
                {
                    var otherFactor = product.Units.Single(x => x.UnitID == item.ItemUnitID).Factor;
                    otherBalance += item.ItemQty * otherFactor;
                }

                e.Value = (balance - otherBalance) / factor;
            }
            else if (e.Column.FieldName == "SourceQty")
            {
                var returnRow = e.Row as DAL.InvoiceDetail;
                if (returnRow == null) return;
                if (e.IsGetData)
                {
                    var sourceRow = ReturnSourceDetails.SingleOrDefault(x => x.ID == returnRow.sourceRowID);
                    if (sourceRow != null)
                        e.Value = sourceRow.ItemQty;
                    else
                        e.Value = 0;
                }

            }
            else if (e.Column.FieldName == "OtherQty")
            {
                var returnRow = e.Row as DAL.InvoiceDetail;
                if (returnRow == null) return;
                if (e.IsGetData)
                {
                    var db = new DAL.DBDataContext();
                    var OtherReturnRows = db.InvoiceDetails.Where(x => x.sourceRowID == returnRow.sourceRowID && x.ID != returnRow.ID).Sum(x => (Nullable<double>)x.ItemQty) ?? 0;// حساب الكميات المرتجعة سابقا
                    e.Value = OtherReturnRows;
                }

            }
        }
        List<DAL.InvoiceDetail> ReturnSourceDetails = new List<DAL.InvoiceDetail>();// متغير ليحتوي قائمةعناصر الفاتورة الأصلية

        /// <summary>
        /// دالة حساب الاجمالي  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_RowUpdated(object sender, RowObjectEventArgs e)
        {
            var items = gridView1.DataSource as Collection<DAL.InvoiceDetail>;
            if (items == null)
                spn_Total.EditValue = 0;
            else
                spn_Total.EditValue = items.Sum(x => x.TotalPrice);
        }

        int CurrentRowsCount = 0;
        /// <summary>
        ///  لجمع الأصناف المتشابهة والزيادة في الكمية فقظ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_RowCountChanged(object sender, EventArgs e)
        {
            if (CurrentRowsCount < gridView1.RowCount)
            {
                var rows = gridView1.DataSource as Collection<DAL.InvoiceDetail>;
                var lastRow = rows.Last();
                var row = rows.FirstOrDefault(x => x.ItemID == lastRow.ItemID && x.ItemUnitID == lastRow.ItemUnitID
                && x.StoreID == lastRow.StoreID && x.Price == lastRow.Price
                && x != lastRow);
                if (row != null)
                {
                    row.ItemQty += lastRow.ItemQty;
                    GridView1_CellValueChanged(sender,
                        new CellValueChangedEventArgs(gridView1.FindRowHandelByRowObject(row),
                        gridView1.Columns[nameof(row.ItemQty)], row.ItemQty));// للزيادة في الكمية عند استعمال الباركود
                    rows.Remove(lastRow);
                }
            }

            CurrentRowsCount = gridView1.RowCount;

            GridView1_RowUpdated(sender, null);
        }
        /// <summary>
        /// دالة لجلب الصنف بالباركو
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            var row = gridView1.GetRow(e.RowHandle) as DAL.InvoiceDetail;
            if (row == null)
                return;
            Session.ProductViewClass itemv = null;
            Session.ProductViewClass.productUOMView unitv = null;
            if (e.Column.FieldName == "Code")
            {
                string ItemCode = e.Value.ToString();
                //عند القراءة من باركود الميزان
                //اذا كانت القراءة بالكود مفعلة وطول الباركود مثل الطول الافتراصي و يبدأ برمز لباركود بصيغة الميزان أو السعر
                if (Session.GlobalSettings.ReadFromScaleBarcode &&
                    ItemCode.Length == Session.GlobalSettings.BarcodeLength &&
                    ItemCode.StartsWith(Session.GlobalSettings.ScaleBarcodePrefix))
                {
                    var itemCodeString = e.Value.ToString()
                        .Substring(Session.GlobalSettings.ScaleBarcodePrefix.Length,
                        Session.GlobalSettings.ProductCodeLength);//استخراج كود الضنف فقط من الباركود
                    ItemCode = Convert.ToInt32(itemCodeString).ToString();//تحويله لرقم ثم ارجاعه لنص ليحذف منه الاصفار في يمين الرقم
                    //استخراج القيمة من الباركود سواء كانت وزن أو سعر
                    string ReadValue = e.Value.ToString().Substring(
                        Session.GlobalSettings.ScaleBarcodePrefix.Length +
                        Session.GlobalSettings.ProductCodeLength);
                    if (Session.GlobalSettings.IgnoreCheckDigit)
                        ReadValue = ReadValue.Remove(ReadValue.Length - 1, 1);

                    double value = Convert.ToDouble(ReadValue); //تحويل القيمة الى رقمي
                    value = value / (Math.Pow(10, Session.GlobalSettings.DivideValueBy));// تحويل من الغرام الى الكيلوغرام
                    if (Session.GlobalSettings.ReadMode == Master.ReadValueMode.Weight)//اذا كان الكودبار خاص بقرائة الوزن
                        row.ItemQty = value;
                    else if (Session.GlobalSettings.ReadMode == Master.ReadValueMode.Price)//اذا كان الكودبار خاص بقرائة السعر
                    {
                        itemv = Session.ProductView.FirstOrDefault(x => x.Units.Select(u => u.Barcode).Contains(ItemCode));// جلب الأصناف التي تحتوي على الكود المكتوب في الخلية
                        if (itemv != null)
                        {
                            unitv = itemv.Units.First(x => x.Barcode == ItemCode);//itemv ابحث عن الوحدة من نتائج البحث في 
                            switch (Type)
                            {
                                case Master.InvoiceType.PurchaseReturn:
                                case Master.InvoiceType.Purchase:
                                    row.ItemQty = value / unitv.BuyPrice;
                                    break;
                                case Master.InvoiceType.Sales:
                                case Master.InvoiceType.SalesReturn:
                                    row.ItemQty = value / unitv.SellPrice;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }
                if (itemv == null)//اذا لم يتم قرائته بالكودبار ?
                    itemv = Session.ProductView.FirstOrDefault(x => x.Units.Select(u => u.Barcode).Contains(ItemCode));// جلب الأصناف التي تحتوي على الكود المكتوب في الخلية
                if (itemv != null)
                {
                    row.ItemID = itemv.ID;
                    if (unitv == null)
                        unitv = itemv.Units.First(x => x.Barcode == ItemCode);//itemv ابحث عن الوحدة من نتائج البحث في 
                    row.ItemUnitID = unitv.UnitID;

                    GridView1_CellValueChanged(sender,
                        new CellValueChangedEventArgs(
                            e.RowHandle, gridView1.Columns[nameof(detailsInstace.ItemID)], row.ItemID));

                    GridView1_CellValueChanged(sender,
                    new CellValueChangedEventArgs(
                        e.RowHandle, gridView1.Columns[nameof(detailsInstace.ItemUnitID)], row.ItemUnitID));
                    enteredCode = string.Empty;// مسح الكود بعد ادخال الصنف في حالة كان الصنف موجودا
                    return;
                }
                enteredCode = string.Empty;// مسح الكود في حالة أنه غير مرتبط بأي صنف 
            }
            /////////////////////////
            if (row.ItemID == 0)
                return;
            itemv = Session.ProductView.Single(x => x.ID == row.ItemID);

            if (row.ItemUnitID == 0)
            {
                row.ItemUnitID = itemv.Units.First().UnitID;
                GridView1_CellValueChanged(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(detailsInstace.ItemUnitID)], row.ItemUnitID));
            }
            unitv = itemv.Units.Single(x => x.UnitID == row.ItemUnitID);
            /////////////////////////
            switch (e.Column.FieldName)
            {

                case nameof(detailsInstace.ItemID): //عند تغيير الايتم ايدي يتغير الستور ايدي
                    if (row.ItemID == 0)
                    {
                        gridView1.DeleteRow(e.RowHandle);
                        return;
                    }
                    if (row.StoreID == 0 && lkp_Branch.IsEditValueValideAndNotZero())
                        row.StoreID = Convert.ToInt32(lkp_Branch.EditValue);// اختيار المخزن من الفرع مباشرة عند ادخال الصنف في الفاتورة
                    break;
                // لما اليونت ايدي تتغير يدخل السعر والكمية
                case nameof(detailsInstace.ItemUnitID):// في حالة تغيير الوحدة

                    switch (Type)
                    {
                        case Master.InvoiceType.Purchase:
                            row.Price = unitv.BuyPrice;//سعر الشراء
                            break;
                        case Master.InvoiceType.Sales:
                            row.Price = unitv.SellPrice;//سعر البيع
                            break;
                        case Master.InvoiceType.PurchaseReturn:
                        case Master.InvoiceType.SalesReturn:

                            var ReturnSourceRow = ReturnSourceDetails.Where(x => x.ID == row.sourceRowID).SingleOrDefault();
                            if (ReturnSourceRow != null)
                            {
                                row.Price = ReturnSourceRow.Price;
                            }


                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    if (row.ItemQty == 0)// تعديل الكمية اذا كانت صفر افتراضيا
                        row.ItemQty = 1;

                    GridView1_CellValueChanged(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(detailsInstace.Price)], row.Price));
                    //TODO حساب الرصيد الحالي

                    break;
                case nameof(detailsInstace.Price):
                case nameof(detailsInstace.Discount):
                case nameof(detailsInstace.ItemQty):
                    //TODO يجب جلب سعر التكلفة من المخزن عند تغيير كمية الصنف أو الصنف نفسه
                    row.DiscountValue = row.Discount * (row.ItemQty * row.Price);
                    goto case nameof(detailsInstace.DiscountValue);// ينتقل لحالة تغيير قيمة الخصم
                case nameof(detailsInstace.DiscountValue):
                    if (gridView1.FocusedColumn.FieldName == nameof(detailsInstace.DiscountValue))
                        row.Discount = row.DiscountValue / (row.ItemQty * row.Price);
                    row.TotalPrice = (row.ItemQty * row.Price) - row.DiscountValue;
                    goto case nameof(detailsInstace.StoreID);// ينتقل لحالة تغيير المخزن


                case nameof(detailsInstace.StoreID):// في حالة تغيير المخزن
                    switch (Type)
                    {
                        case Master.InvoiceType.Purchase:
                            row.CostValue = row.TotalPrice / row.ItemQty;       // حساب سعر التكلفة في فاتورة الشراء
                            row.TotalCostValue = row.TotalPrice;                // حساب سعر التكلفة الاجمالية في فاتورة الشراء
                            break;
                        case Master.InvoiceType.Sales:
                            var store = (row.StoreID == 0) ? Convert.ToInt32(lkp_Branch.EditValue) : row.StoreID;
                            var costPerMainUnit = MasterInventory.GetCostOfNextProduct(row.ItemID, store, row.ItemQty);

                            row.CostValue = costPerMainUnit * unitv.Factor;     // حساب سعر التكلفة في فاتورة البيع
                            row.TotalCostValue = row.CostValue * row.ItemQty;   // حساب سعر التكلفة الاجمالية في فاتورة البيع
                            break;
                        case Master.InvoiceType.PurchaseReturn:
                        case Master.InvoiceType.SalesReturn:

                            var ReturnSourceRow = ReturnSourceDetails.Where(x => x.ID == row.sourceRowID).SingleOrDefault();
                            if (ReturnSourceRow != null)
                            {
                                row.CostValue = ReturnSourceRow.CostValue;
                                row.TotalCostValue = row.CostValue * row.ItemQty;   // حساب سعر التكلفة الاجمالية في فاتورة البيع

                            }


                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// دالة تستطيع من خلالها تغيير محتوى الريبوزيتوري ايتم المحدد
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "ItemUnitID")//عند التحديد على عمود الوحدات
            {
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();
                repo.NullText = string.Empty;
                e.RepositoryItem = repo;
                var row = gridView1.GetRow(e.RowHandle) as DAL.InvoiceDetail;
                if (row == null)
                    return;
                var item = Session.ProductView.SingleOrDefault(x => x.ID == row.ItemID);
                if (item == null)
                    return;
                repo.InitializeData(item.Units, null, null, "UnitName", "UnitID");
                repo.Columns.Clear();
                repo.Columns.Add(new LookUpColumnInfo("UnitName"));
                repo.ShowHeader = false;
            }
            else if (e.Column.FieldName == nameof(detailsInstace.ItemID))
            {
                e.RepositoryItem = repoItems;
            }
        }

        #endregion


        private void Lkp_Branch_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            var items = gridView1.DataSource as Collection<DAL.InvoiceDetail>;
            if (e.OldValue is int && e.NewValue is int)
                foreach (var row in items)
                {
                    if (row.StoreID == Convert.ToInt32(e.OldValue))
                        row.StoreID = Convert.ToInt32(e.NewValue);
                }
        }

        /// <summary>
        /// دالة تقوم بجلب قائمة طرف التعامل ( موردين أو عملاء
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
                    glkp_PartID.EditValue = Session.Defualts.Customer;
                }

                else if (partType == (int)Master.PartType.Vendor)
                {
                    glkp_PartID.InitializeData(Session.vendores);
                    glkp_PartID.EditValue = Session.Defualts.Vendor;
                }
                glkp_Initial();
            }
        }


        #region SpenEditCalculations

        /// <summary>
        /// تغيير المدفوع ليساوي الصافي
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_Net_MouseDoubleClick(object sender, EventArgs e)
        {
            spn_Paid.EditValue = spn_Net.EditValue;
        }

        /// <summary>
        /// دالة ادخال المدفوع اليا اذا لم يتم تغييره يدويا
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_Net_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Type == Master.InvoiceType.Sales && Session.UserSettings.Sales.DefaultPayMethodInSales == Master.PayMethods.Cash)
            {
                spn_Paid.EditValue = e.NewValue;
            }
            else if (Type == Master.InvoiceType.Sales && Session.UserSettings.Sales.DefaultPayMethodInSales == Master.PayMethods.Credit)
            {
                spn_Paid.EditValue = 0;
            }
        }



        /// <summary>
        /// دالة حساب المتبقي
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_Paid_EditValueChanged(object sender, EventArgs e)
        {
            var net = Convert.ToDouble(spn_Net.EditValue);
            var paid = Convert.ToDouble(spn_Paid.EditValue);
            spn_Remaing.EditValue = net - paid;
        }
        /// <summary>
        /// دالة حساب الصافي
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            Spn_TaxValue_EditValueChanged(sender, e);
            Spn_DiscountValue_EditValueChanged(sender, e);
            var tax = Convert.ToDouble(spn_TaxValue.EditValue);
            var discount = Convert.ToDouble(spn_DiscountValue.EditValue);
            var expences = Convert.ToDouble(spn_Expences.EditValue);

            spn_Net.EditValue = (total + tax - discount + expences);
        }

        /// <summary>
        /// دالة حساب الضريبة ونسبتها اثناء الكتابة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_TaxValue_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            var Val = Convert.ToDouble(spn_TaxValue.EditValue);
            var ratio = Convert.ToDouble(spn_Tax.EditValue);
            if (IsTaxValueFocused)
                spn_Tax.EditValue = (Val / total);
            else
                spn_TaxValue.EditValue = (total * ratio);
        }

        Boolean IsTaxValueFocused;
        private void Spn_TaxValue_Leave(object sender, EventArgs e)
        {
            IsTaxValueFocused = false;
        }

        private void Spn_TaxValue_Enter(object sender, EventArgs e)
        {
            IsTaxValueFocused = true;
        }
        /// <summary>
        /// دالة حساب الخصم ونسبته اثناء الكتابة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spn_DiscountValue_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            var discountVal = Convert.ToDouble(spn_DiscountValue.EditValue);
            var discountRation = Convert.ToDouble(spn_DiscountRation.EditValue);
            if (IsDiscountValueFocused)
            {
                spn_DiscountRation.EditValue = (discountVal / total);
            }
            else
            {
                spn_DiscountValue.EditValue = (total * discountRation);
            }
        }

        Boolean IsDiscountValueFocused;
        private void Spn_DiscountValue_Enter(object sender, EventArgs e)
        {
            IsDiscountValueFocused = true;
        }

        private void Spn_DiscountValue_Leave(object sender, EventArgs e)
        {
            IsDiscountValueFocused = false;
        }

        /// <summary>
        /// حدث الضغط على زر اضافة عميل أو مورد مباشرة من هذا الفورم 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lkp_PartType_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Plus)
            {
                var isCustomer = Convert.ToInt32(lkp_PartType.EditValue) == (int)Master.PartType.Customer;
                var newID = frm_CustomerVendor.AddNew(isCustomer);
                if (newID != 0)
                    glkp_PartID.EditValue = newID;
            }
        }
        #endregion


        void MoveFocuseToGrid(bool FocuseToItem = false)
        {
            this.gridView1.Focus();
            gridView1.FocusedRowHandle = GridControl.InvalidRowHandle;
            gridView1.FocusedColumn = FocuseToItem ? gridView1.Columns["ItemID"] : gridView1.Columns["Code"];
            gridView1.AddNewRow();
            gridView1.UpdateCurrentRow();
        }
        /// <summary>
        /// التأكد من عدم تكرار الكود
        /// </summary>
        /// <returns></returns>
        bool IsCodeExists()
        {
            using (var db = new DAL.DBDataContext())
            {
                var data = db.InvoiceHeaders.Where(x => x.ID != Invoice.ID && x.InvoiceType == (byte)Type && x.Code == txt_Code.Text).Count();
                if (data > 0)
                    txt_Code.ErrorText = "هذا الكود متكرر";
                return data > 0;

            }
        }

        public override void RefreshData()
        {
            lkp_Branch.InitializeData(Session.Store);
            lkp_Drawer.InitializeData(Session.Drawer);
            base.RefreshData();
        }

        public override bool IsDataValide()
        {
            int NumberOfErrors = 0;
            if (gridView1.RowCount == 0)//التحقق من أن الفاتورة تحتوي على صنف واحد على الأقل
            {
                NumberOfErrors++;
                XtraMessageBox.Show(
                   text: "الرجاء ادخال صنف واحد على الأقل",
                   caption: string.Empty,
                   buttons: MessageBoxButtons.OK,
                   icon: MessageBoxIcon.Error);
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.FocusedRowHandle = i;
                GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));
                if (gridView1.HasColumnErrors) // اللتحقق من أن الفاتورة لا تحتوي على اخطاء
                {
                    NumberOfErrors++;
                    XtraMessageBox.Show(
                       text: "الرجاء التحقق من الأخطاء في الجدول",
                       caption: string.Empty,
                       buttons: MessageBoxButtons.OK,
                       icon: MessageBoxIcon.Error);
                    GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));
                    return false;
                }
            }

            //if (gridView1.HasColumnErrors)
            //{
            //    NumberOfErrors++;
            //    XtraMessageBox.Show(
            //        text: "الرجاء التحقق من الأخطاء في الجدول",
            //       caption: "",
            //       buttons: MessageBoxButtons.OK,
            //       icon: MessageBoxIcon.Error);
            //}
            NumberOfErrors += IsCodeExists() ? 1 : 0;// التحقق من أن الكود غير مكرر 
            NumberOfErrors += txt_Code.IsTextValide() ? 0 : 1;// التحقق من أن النص غير فارغ 
            NumberOfErrors += lkp_PartType.IsEditValueValide() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += lkp_Drawer.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += lkp_Branch.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += glkp_PartID.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص ليس صفر
            NumberOfErrors += (dt_Date.IsDateValide()) ? 0 : 1;// التحقق من أن النص بصيغة تاريخ
            NumberOfErrors += (spn_DiscountValue.IsValueNotLessThanZero()) ? 0 : 1;// التحقق من أن قيمة الخصم ليست اقل من الصفر
            NumberOfErrors += (spn_Expences.IsValueNotLessThanZero()) ? 0 : 1;// التحقق من أن المصروفات ليست اقل من الصفر
            NumberOfErrors += (spn_Paid.IsValueNotLessThanZero()) ? 0 : 1;// التحقق من أن قيمة المدفوع ليست اقل من الصفر
            NumberOfErrors += (spn_TaxValue.IsValueNotLessThanZero()) ? 0 : 1;// التحقق من أن قيمة الضريبة ليست اقل من الصفر 

            if (chk_PostToStore.Checked)
            {
                NumberOfErrors += dt_PostDate.IsDateValide() ? 0 : 1;// التحقق من أن النص بصيغة تاريخ
                //layoutControlGroup2.Expanded = dt_PostDate.IsDateValide() ? false : true; ;// التحقق من أن النص بصيغة تاريخ
            }

            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    break;
                case Master.InvoiceType.Sales:
                    // منع التغيير في الخصم للمستخدم غير المسموح له به
                    if (Invoice.DiscountRation != Convert.ToDouble(spn_DiscountRation.EditValue) &&
                            Session.UserSettings.Sales.MaxDiscountInInvoice < Convert.ToDecimal(spn_DiscountRation.EditValue))
                    {
                        NumberOfErrors++;
                        spn_DiscountRation.ErrorText = "هذا الخصم غير مسموح به";
                    }
                    if (Invoice.ID == 0)
                    {
                        //التحقق في اعدادات المستخدم من عميل تخطى حد الائتمان
                        var id = Convert.ToInt32(glkp_PartID.EditValue);
                        if (id != 0)
                        {
                            DAL.CustomersAndVendore account;
                            if (lkp_PartType.EditValue.Equals((byte)Master.PartType.Vendor))
                            {
                                account = Session.vendores.Single(x => x.ID == id);
                            }
                            else
                            {
                                account = Session.Customers.Single(x => x.ID == id);
                            }
                            if (account.MaxCredit <= accountBalance.BalanceAmount && accountBalance.BalanceType == AccountBalance.BalanceTypes.Debit)
                            {
                                switch (Session.UserSettings.Sales.WhenSellingToCustomerExeededMaxCredit)
                                {
                                    case Master.WorningLevels.DoNotEnteript:
                                        break;
                                    case Master.WorningLevels.ShowWarning:
                                        if (XtraMessageBox.Show(text: "لقد تخطى هذا العميل حد الائتمان ... هل تريد المتابعة",
                                            caption: string.Empty,
                                            icon: MessageBoxIcon.Question,
                                            buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            NumberOfErrors++;
                                        }
                                        break;
                                    case Master.WorningLevels.Prevent:
                                        NumberOfErrors++;
                                        XtraMessageBox.Show("لايمكن البيع لهذا العميل حيث تخطى حد الائتمان");
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case Master.InvoiceType.PurchaseReturn:
                case Master.InvoiceType.SalesReturn:
                    break;
                default:
                    throw new NotImplementedException();
            }

            return (NumberOfErrors == 0);
        }

        /// <summary>
        /// دالة لتوليد كود للفاتورة
        /// </summary>
        /// <returns></returns>
        private string GetNewInvoiceCode()
        {
            string maxCode;
            using (var db = new DAL.DBDataContext())
            {
                maxCode = db.InvoiceHeaders.Where(x => x.InvoiceType == (int)Type).OrderByDescending(x => x.Date).Select(x => x.Code ?? "0").FirstOrDefault();
            }

            return Master.GetNextNumberInString(maxCode);
        }

        public override void New()
        {
            Invoice = new DAL.InvoiceHeader()
            {
                Drawer = Session.Defualts.Drawer,
                Date = DateTime.Now,
                PostedToStore = true,
                PostDate = DateTime.Now,
                Code = GetNewInvoiceCode(),
            };
            switch (Type)
            {
                // قيم افتراضية للفاتورة الجديدة في حالة المشتريات أو مردود المشتريات

                case Master.InvoiceType.Purchase:
                    Invoice.PartType = (int)Master.PartType.Vendor; // نوع طرف التعامل مورد
                    Invoice.PartID = Session.Defualts.Vendor;//رقم المورد
                    Invoice.Branch = Session.Defualts.RamStore;// رقم المخزن الافتراضي للمشتريات
                    break;
                // قيم افتراضية للفاتورة الجديدة في حالة المبيعات أو مردود المبيعات

                case Master.InvoiceType.Sales:
                    Invoice.PartType = (int)Master.PartType.Customer; // نوع طرف التعامل عميل
                    Invoice.PartID = Session.Defualts.Customer;// رقم العميل
                    Invoice.Branch = Session.Defualts.Store;// رقم المخزن الافتراضي للمبيعات
                    break;
                case Master.InvoiceType.PurchaseReturn:
                    Invoice.PartType = (int)Master.PartType.Vendor; // نوع طرف التعامل مورد
                    Invoice.PartID = Session.Defualts.Vendor;//رقم المورد
                    Invoice.Branch = Session.Defualts.RamStore;// رقم المخزن الافتراضي لمردود المشتريات
                    break;
                case Master.InvoiceType.SalesReturn:
                    Invoice.PartType = (int)Master.PartType.Customer; // نوع طرف التعامل عميل
                    Invoice.PartID = Session.Defualts.Customer;// رقم العميل
                    Invoice.Branch = Session.Defualts.Store;// رقم المخزن الافتراضي لمردود المبيعات
                    break;
                default:
                    throw new NotImplementedException();
            }
            base.New();
            MoveFocuseToGrid();
        }


        public override void GetData()
        {
            Glkp_Source_EditValueChanged(null, null);
            lkp_Branch.EditValue = Invoice.Branch;
            lkp_Drawer.EditValue = Invoice.Drawer;
            lkp_PartType.EditValue = Invoice.PartType;
            glkp_Source.EditValue = Invoice.SourceID;
            glkp_PartID.EditValue = Invoice.PartID;
            txt_Code.Text = Invoice.Code;
            dt_Date.DateTime = Invoice.Date;
            dt_DelivaryDate.EditValue = Invoice.DelivaryDate;
            dt_PostDate.EditValue = Invoice.PostDate;
            memoEme_ShoppingAddress.Text = Invoice.ShippingAddress;
            me_Notes.Text = Invoice.Notes;
            chk_PostToStore.Checked = Invoice.PostedToStore;
            spn_DiscountRation.EditValue = Invoice.DiscountRation;
            spn_DiscountValue.EditValue = Invoice.DiscountValue;
            spn_Expences.EditValue = Invoice.Expences;
            spn_Net.EditValue = Invoice.Net;
            spn_Paid.EditValue = Invoice.Paid;
            spn_Remaing.EditValue = Invoice.Remaing;
            spn_Tax.EditValue = Invoice.Tax;
            spn_TaxValue.EditValue = Invoice.TaxValue;
            spn_Total.EditValue = Invoice.Total;
            generalDB = new DAL.DBDataContext();
            gridControl1.DataSource = generalDB.InvoiceDetails.Where(x => x.InvoiceID == Invoice.ID);
            gridControl_ReturnInvoices.DataSource = generalDB.InvoiceHeaders.Where(x => x.SourceID == Invoice.ID && Invoice.ID != 0).Select(
               x => new { x.ID, x.Date }).ToList();//جلب بيانات فواتير المردودت الخاصة بالفاتورة الحالية

            PartID = Invoice.ID;                                // تسجيل حركات المستخدم
            PartName = Invoice.Code + " - " + glkp_PartID.Text; // تسجيل حركات المستخدم
            base.GetData();
        }
        public override void SetData()
        {
            Invoice.Branch = Convert.ToInt32(lkp_Branch.EditValue);
            Invoice.Drawer = Convert.ToInt32(lkp_Drawer.EditValue);
            Invoice.PartType = Convert.ToByte(lkp_PartType.EditValue);
            Invoice.PartID = Convert.ToInt32(glkp_PartID.EditValue);
            Invoice.Code = txt_Code.Text;
            Invoice.Date = dt_Date.DateTime;
            Invoice.DelivaryDate = dt_DelivaryDate.EditValue as DateTime?;
            Invoice.PostDate = dt_PostDate.EditValue as DateTime?;
            Invoice.ShippingAddress = memoEme_ShoppingAddress.Text;
            Invoice.Notes = me_Notes.Text;
            Invoice.PostedToStore = chk_PostToStore.Checked;
            Invoice.DiscountRation = Convert.ToDouble(spn_DiscountRation.EditValue);
            Invoice.DiscountValue = Convert.ToDouble(spn_DiscountValue.EditValue);
            Invoice.Expences = Convert.ToDouble(spn_Expences.EditValue);
            Invoice.Net = Convert.ToDouble(spn_Net.EditValue);
            Invoice.Paid = Convert.ToDouble(spn_Paid.EditValue);
            Invoice.Remaing = Convert.ToDouble(spn_Remaing.EditValue);
            Invoice.Tax = Convert.ToDouble(spn_Tax.EditValue);
            Invoice.TaxValue = Convert.ToDouble(spn_TaxValue.EditValue);
            Invoice.Total = Convert.ToDouble(spn_Total.EditValue);
            Invoice.InvoiceType = (byte)Type;
            Invoice.SourceID = Convert.ToInt32(glkp_Source.EditValue);
        }

        public override void Save()
        {
            gridView1.UpdateCurrentRow();
            var db = new DAL.DBDataContext();
            if (Invoice.ID == 0)
            {
                db.InvoiceHeaders.InsertOnSubmit(Invoice);
            }
            else
            {
                db.InvoiceHeaders.Attach(Invoice);
            }

            SetData();

            var items = gridView1.DataSource as Collection<DAL.InvoiceDetail>;  // <DAL.InvoiceDetail> متغير لقاعدة بيانات الغريد فيو على شكل
            /// تقسيم التكلفة على الأصناف
            switch (Type)
            {
                case Master.InvoiceType.Purchase:// في حالة فاتورة مشتريات
                    if (Invoice.Expences > 0) // في حالة وجود مصاريف أخرى
                    {
                        var total = items.Sum(x => x.TotalPrice);                           //السعر الكلي
                        var totalQty = items.Sum(x => x.ItemQty);                           // العدد الكلي
                        if (total <= 0 || totalQty <= 0 || items.Any(x => x.ItemQty <= 0))
                            throw new InvalidOperationException("لا يمكن توزيع المصاريف على فاتورة تحتوي على قيمة أو كمية صفرية.");

                        var ByPriceUnit = Invoice.Expences / total;                         // نسبة المصاريف إلى قيمة المشتريات
                        var ByQtyUnit = Invoice.Expences / totalQty;                        // المصاريف لكل وحدة

                        /////////////// تخصيص الفورم المنبثقة للسؤال على طريقة تقسيم التكلفة ///////////////
                        XtraDialogArgs args = new XtraDialogArgs();
                        UserControls.CostDistributionOption distributionOption = new UserControls.CostDistributionOption();
                        args.Caption = string.Empty;
                        args.Content = distributionOption;
                        ((XtraBaseArgs)args).Buttons = new DialogResult[]
                            {
                                DialogResult.OK
                            };
                        args.Showing += Args_Showing;
                        XtraDialog.Show(args);

                        foreach (var row in items)
                        {
                            if (distributionOption.SelectedOption == Master.CostDistributionOptions.ByPrice)
                                row.CostValue = (row.TotalPrice / row.ItemQty) + (ByPriceUnit * row.TotalPrice / row.ItemQty);
                            else
                                row.CostValue = (row.TotalPrice / row.ItemQty) + ByQtyUnit;

                            row.TotalCostValue = row.ItemQty * row.CostValue;  // حساب سعر التكلفة الاجمالية
                        }
                    }
                    else // في حالة عدم وجود مصاريف أخرى
                    {
                        foreach (var row in items)
                        {
                            row.CostValue = row.TotalPrice / row.ItemQty;  // حساب سعر التكلفة
                            row.TotalCostValue = row.TotalPrice;           // حساب سعر التكلفة الاجمالية
                        }
                    }
                    break;
                case Master.InvoiceType.Sales:// في حالة فاتورة مبيعات
                    break;
                case Master.InvoiceType.PurchaseReturn:// في حالة فاتورة مردود مشتريات
                case Master.InvoiceType.SalesReturn:// في حالة فاتورة مردود مبيعات
                    //TODO 
                    break;
                default:
                    throw new NotImplementedException();
            }


            #region Journals القيود
            #region شرح مختصر للقيود
            // المخزن - حساب المخزن
            // حساب المورد


            //////////
            // حساب المخزون -----------مدين 100
            // حساب المورد -----------دائن 100

            // حساب الخزنة ----------- دائن 70
            // حساب المورد -----------مدين 70


            // كشف حساب للمورد > المدين - الدائن < 100-70 = 30 دائن   

            // حساب الخزنة ----------- دائن 30
            // حساب المورد -----------مدين 30

            // كشف حساب للمورد > المدين - الدائن < 100-70-30 = 0 


            // حساب الخزنة ----------- مدين 500
            // حساب المورد -----------دائن 500

            // كشف حساب للمورد > المدين - الدائن < 100-70-30+500 = 500 دائن

            ////////////////////////// القيود  ///////////////////////////////

            // فاتورة بقيمة بضاعة 1000
            // ضريبة 120 
            // مصروف نقل 100
            // خصم 50

            // الصافي  1170



            //             حساب المخزون                - مدين - 1100
            //               حساب الضريبة المضافة       - مدين - 120
            // حساب المورد                - دائن -              1220




            //                حساب المورد                    - مدين - 50
            // حساب خصم نقدي مكتسب            - دائن -                50

            //              حساب المورد                  - مدين - 1000
            // حساب الخزنة                   - دائن -             1000
            ///////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////
            // فاتورة بقيمة بضاعة 1200     تكلفتها 1100 
            // ضريبة 120
            // مصروف نقل 100
            // خصم  50

            //  الصافي 1370 --------------------------------------------------

            //  حساب العميل مدين

            // حساب العميل               - مدين -           1420
            //           حساب المبيعات              - دائن - 1300
            //            حساب الضريبة المضافة              - دائن - 120


            /// في حالة البيع من مخزن آخر غير الفرع الذي يتم البيع منه يجب تحويل قيمة البضاعة بين حسابات المخزون لهذه المخازن
            /// بالتالي يجب أن نتحقق اولا من وجود مخازن مختلفة عن مخزن البيع
            /// بعده نجمع تكلفة البضاعة المباعة لهذه المخازن كلا على حدى


            //           حساب المخزون              - دائن - 1100
            //           حساب تكلفة البضاعة المباعة              - مدين - 1100


            //             حساب العميل               - دائن - 50
            //  حساب خصم نقدي مسموح به          - مدين -            50


            //           حساب العميل               - دائن - 1370
            //  حساب الخزنة               - مدين -          1370
            #endregion
            ////////////////////////// القيود  ///////////////////////////////
            db.Journals.DeleteAllOnSubmit(db.Journals.Where(x => x.SourceType == (byte)Type && x.SourceID == Invoice.ID));//حذف القيود في حالة التعديل على فاتورة قديمة
            db.SubmitChanges();
            var store = db.Stores.Single(x => x.ID == Invoice.Branch);      //المخزن
            var drawer = db.Drawers.Single(x => x.ID == Invoice.Drawer);    //الخزنة
            string msg;                             // عبارة القيد
            int StoreAccount;                       // حساب المخزن
            int TaxAccount;                         // حساب الضريبة
            int partAcountID;                       // مورد أو عميل
            int DiscountAccount;                    // حساب الخصم النقدي
            bool IsPartCredit;                      // دائن أو مدين
            bool InsertCostOfSoldGoodsJournal;      // يوجد تكلفة للبضاعة المباعة
            bool IsIn;                              // بضاعة داخلة أو خارجة
            partAcountID = db.CustomersAndVendores.Single(x => x.ID == Invoice.PartID).AccountID;// مورد أو عميل

            switch (Type)
            {
                case Master.InvoiceType.Purchase://في حالة فاتورة مشتريات
                    StoreAccount = store.InventoryAccountID;                       // حساب المخزون
                    TaxAccount = Session.Defualts.PurchaseTax;                     // حساب ضريبة الشراء
                    DiscountAccount = Session.Defualts.DiscountReceivedAccountID;  // حساب الخصم النقدي المكتسب
                    IsPartCredit = true;
                    IsIn = true;
                    InsertCostOfSoldGoodsJournal = false;                               // لا يوجد تكلفة للبضاعة المباعة في الشراء
                    msg = $"  فاتورة شراء رقم {Invoice.ID} للعميل {glkp_PartID.Text}";// رقم الفاتورة واسم المتعامل
                    break;
                case Master.InvoiceType.Sales: //في حالة فاتورة مبيعات
                    StoreAccount = store.SalesAccountID;                           // حساب المبيعات
                    TaxAccount = Session.Defualts.SalesTax;                        //حساب ضريبة البيع
                    DiscountAccount = Session.Defualts.DiscountAllowedAccountID;   // حساب الخصم النقدي المسموح به
                    IsPartCredit = false;
                    IsIn = false;
                    InsertCostOfSoldGoodsJournal = true;                          
                    msg = $"  فاتورة مبيعات رقم {Invoice.ID} للعميل {glkp_PartID.Text}";// رقم الفاتورة واسم المتعامل
                    break;
                case Master.InvoiceType.PurchaseReturn://في حالة فاتورة مردود مشتريات
                    StoreAccount = store.InventoryAccountID;                       // حساب المخزون
                    TaxAccount = Session.Defualts.PurchaseTax;                     // حساب ضريبة الشراء
                    DiscountAccount = Session.Defualts.DiscountReceivedAccountID;  // حساب الخصم النقدي المكتسب
                    IsPartCredit = false;
                    IsIn = false;
                    InsertCostOfSoldGoodsJournal = false;                               // لا يوجد تكلفة للبضاعة المباعة في الشراء
                    msg = $"  فاتورة مردود شراء رقم {Invoice.ID} للعميل {glkp_PartID.Text}";// رقم الفاتورة واسم المتعامل
                    break;

                case Master.InvoiceType.SalesReturn://في حالة فاتورة مردود مبيعات
                    StoreAccount = store.SalesReturnAccountID;
                    TaxAccount = Session.Defualts.SalesTax;
                    DiscountAccount = Session.Defualts.DiscountAllowedAccountID;
                    IsPartCredit = true;
                    IsIn = true;
                    InsertCostOfSoldGoodsJournal = true;
                    msg = $"  فاتورة مردود مبيعات رقم {Invoice.ID} للعميل {glkp_PartID.Text}";// رقم الفاتورة واسم المتعامل
                    break;

                default:
                    throw new NotImplementedException();
            }

            db.Journals.InsertOnSubmit(new DAL.Journal()  //Part   مورد أو عميل  Credit = Invoice.Total + Invoice.TaxValue + Invoice.Expences
            {
                AccountID = partAcountID,
                Code = 54545,
                Debit = (!IsPartCredit) ? Invoice.Total + Invoice.TaxValue + Invoice.Expences : 0,
                Credit = (IsPartCredit) ? Invoice.Total + Invoice.TaxValue + Invoice.Expences : 0,
                InsertDate = Invoice.Date,
                Notes = msg,
                SourceID = Invoice.ID,
                SourceType = (byte)Type
            });

            db.Journals.InsertOnSubmit(new DAL.Journal()  // Store Inventory  Debit = Invoice.Total,
            {
                AccountID = StoreAccount,
                Code = 54545,
                Debit = (IsPartCredit) ? Invoice.Total + Invoice.Expences : 0,
                Credit = (!IsPartCredit) ? Invoice.Total + Invoice.Expences : 0,
                InsertDate = Invoice.Date,
                Notes = msg,
                SourceID = Invoice.ID,
                SourceType = (byte)Type
            });

            if (Invoice.Tax > 0)
                db.Journals.InsertOnSubmit(new DAL.Journal()  // Store Tax   Debit = Invoice.TaxValue,
                {
                    AccountID = TaxAccount,
                    Code = 54545,
                    Debit = (IsPartCredit) ? Invoice.TaxValue : 0,
                    Credit = (!IsPartCredit) ? Invoice.TaxValue : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- ضريبة مضافة",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });

            if (Invoice.DiscountValue > 0)
            {
                db.Journals.InsertOnSubmit(new DAL.Journal()  // Store DiscountValue   Credit = Invoice.DiscountValue,
                {
                    AccountID = DiscountAccount,
                    Code = 54545,
                    Debit = (!IsPartCredit) ? Invoice.DiscountValue : 0,
                    Credit = (IsPartCredit) ? Invoice.DiscountValue : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- خصم ",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
                db.Journals.InsertOnSubmit(new DAL.Journal()  // Store DiscountValue   Debit = Invoice.DiscountValue,
                {
                    AccountID = partAcountID,
                    Code = 54545,
                    Debit = (IsPartCredit) ? Invoice.DiscountValue : 0,
                    Credit = (!IsPartCredit) ? Invoice.DiscountValue : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- خصم ",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
            }

            if (InsertCostOfSoldGoodsJournal)
            {

                //           حساب المخزون              - دائن - 1100
                //           حساب تكلفة البضاعة المباعة              - مدين - 1100

                if (items.Where(x => x.StoreID != Invoice.Branch).Count() > 0)
                {
                    var otherStores = items.Where(x => x.StoreID != Invoice.Branch).Select(x => x.StoreID).Distinct();

                    //           حساب المخزون المخزن الآخر              - دائن - 1100
                    //           حساب المخزون الخاص بالفرع              - مدين - 1100

                    foreach (var otherStore in otherStores)
                    {
                        var Cost = items.Where(x => x.StoreID == otherStore).Sum(x => x.TotalCostValue);
                        db.Journals.InsertOnSubmit(new DAL.Journal()  // CostOfSoldGoods
                        {
                            AccountID = store.InventoryAccountID,
                            Code = 54545,
                            Debit = (!IsPartCredit) ? Cost : 0,
                            Credit = (IsPartCredit) ? Cost : 0,
                            InsertDate = Invoice.Date,
                            Notes = msg + "- نقل الباعة للبيع",
                            SourceID = Invoice.ID,
                            SourceType = (byte)Type
                        });
                        db.Journals.InsertOnSubmit(new DAL.Journal()  // CostOfSoldGoods
                        {
                            AccountID = db.Stores.Single(x => x.ID == otherStore).InventoryAccountID,
                            Code = 54545,
                            Debit = (IsPartCredit) ? Cost : 0,
                            Credit = (!IsPartCredit) ? Cost : 0,
                            InsertDate = Invoice.Date,
                            Notes = msg + "- نقل الباعة للبيع",
                            SourceID = Invoice.ID,
                            SourceType = (byte)Type
                        });
                    }
                }


                var TotalCost = items.Sum(x => x.TotalCostValue);
                db.Journals.InsertOnSubmit(new DAL.Journal()  // CostOfSoldGoods
                {
                    AccountID = store.InventoryAccountID,
                    Code = 54545,
                    Debit = (IsPartCredit) ? TotalCost : 0,
                    Credit = (!IsPartCredit) ? TotalCost : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- تكلفة البضاعة المباعة",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
                db.Journals.InsertOnSubmit(new DAL.Journal()  // CostOfSoldGoods
                {
                    AccountID = store.CostOfSoldGoodsAccountID,
                    Code = 54545,
                    Debit = (!IsPartCredit) ? TotalCost : 0,
                    Credit = (IsPartCredit) ? TotalCost : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- تكلفة البضاعة المباعة",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
            }
            if (Invoice.Paid > 0)
            {
                db.Journals.InsertOnSubmit(new DAL.Journal()  // Store DiscountValue   Credit = Invoice.Paid,
                {
                    AccountID = drawer.AccountID,
                    Code = 54545,
                    Debit = (!IsPartCredit) ? Invoice.Paid : 0,
                    Credit = (IsPartCredit) ? Invoice.Paid : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- سداد",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
                db.Journals.InsertOnSubmit(new DAL.Journal()  // Store DiscountValue    Debit = Invoice.Paid,
                {
                    AccountID = partAcountID,
                    Code = 54545,
                    Debit = (IsPartCredit) ? Invoice.Paid : 0,
                    Credit = (!IsPartCredit) ? Invoice.Paid : 0,
                    InsertDate = Invoice.Date,
                    Notes = msg + "- سداد",
                    SourceID = Invoice.ID,
                    SourceType = (byte)Type
                });
            }

            db.SubmitChanges();//حفظ التغييرات

            ////////////////////////// القيود  ///////////////////////////////
            #endregion

            foreach (var row in items)
                row.InvoiceID = Invoice.ID;
            DeleteInvoiceStoreLogDetails(Invoice.ID, (byte)Type);
            generalDB.SubmitChanges();
            db.SubmitChanges();
            //StoreLogs تعبئة جدول 
            if (Invoice.PostedToStore)
                foreach (var row in items)
                {
                    var unitView = Session.ProductView.Single(x => x.ID == row.ItemID).Units.Single(x => x.UnitID == row.ItemUnitID);
                    db.StoreLogs.InsertOnSubmit(new DAL.StoreLog()
                    {
                        SourceType = (byte)Type,
                        SourceID = row.ID,
                        ProductID = row.ItemID,
                        StoreID = row.StoreID,
                        Qty = row.ItemQty * unitView.Factor,
                        IsInTransaction = IsIn,
                        CostValue = row.CostValue / unitView.Factor,
                        InsertTime = Invoice.PostDate.Value,
                        Notes = msg
                    });
                }
            db.SubmitChanges();
            PartID = Invoice.ID;                                    // تسجيل حركة المستخدم
            PartName = Invoice.Code + " - " + glkp_PartID.Text;     // تسجيل حركة المستخدم
            base.Save();
            // تحديث البيانات في شاشة قائمة الفواتير
            var forms = Application.OpenForms.Cast<Form>().Where(x =>
                x.Name == this.Name + "List" ||
                (x is frm_InvoiceList && ((frm_InvoiceList)x).InvoiceType == Type));// تحديث القائمة القديمة أو القائمة المنقولة حسب نوع الفاتورة
            foreach (var frm in forms)
            {
                if (frm == null) continue;
                if (frm is frm_MasterList)
                    ((frm_MasterList)frm).RefreshData();
                else if (frm is NativeMasterListForm)
                    ((NativeMasterListForm)frm).RefreshData();
            }
        }
        /// <summary>
        ///قبل الحفظ StoreLogs دالة لحذف البيانات في جدول
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <param name="type"></param>
        public static void DeleteInvoiceStoreLogDetails(int invoiceID, byte type)
        {
            using (var db = new DAL.DBDataContext())
            {
                db.StoreLogs.DeleteAllOnSubmit(
                                db.StoreLogs.Where(x => x.SourceType == type &&
                                db.InvoiceDetails.Where(i => i.InvoiceID == invoiceID).Select(d => d.ID).Contains(x.SourceID)));
                db.SubmitChanges();
            }
        }

        private void Args_Showing(object sender, XtraMessageShowingArgs e)
        {

            e.Form.ControlBox = false;
            e.Form.Height = 150;
            e.Buttons[DialogResult.OK].Text = "متابعة وحفظ";
        }
        public override void Print()
        {
            Print(Invoice.ID, this.Type, this.Name);
            //base.Print();
        }
        public static void Print(DAL.InvoiceHeader invoice, Master.InvoiceType Type, string CallerScreenName)
        {
            Print(invoice.ID, Type, CallerScreenName);
        }
        public static void Print(int id, Master.InvoiceType Type, string CallerScreenName)
        {
            Print(new List<int> { id }, Type, CallerScreenName);
        }
        public static void Print(List<int> ids, Master.InvoiceType Type, string CallerScreenName)
        {
            /////////////////////////// من أجل صلاحيات المستخدم /////////////////////////////
            var name = string.Empty;
            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    name = Class.Screens.AddPurchaseInvoice.ScreenName;
                    break;
                case Master.InvoiceType.Sales:
                    name = Class.Screens.AddSalesInvoice.ScreenName;
                    break;
                case Master.InvoiceType.PurchaseReturn:
                    name = Class.Screens.AddPurchaseReturnInvoice.ScreenName;
                    break;
                case Master.InvoiceType.SalesReturn:
                    name = Class.Screens.AddSalesReturnInvoice.ScreenName;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (CheckActionAuthorization(name, Master.Actions.Print) == false)
                return;
            ///////////////////////////////////////////////////////

            using (var db = new DAL.DBDataContext())
            {
                var invoice = (from inv in db.InvoiceHeaders
                               join str in db.Stores on inv.Branch equals str.ID
                               from prt in db.CustomersAndVendores.Where(x => x.ID == inv.PartID).DefaultIfEmpty()
                               from drw in db.Drawers.Where(x => x.ID == inv.Drawer).DefaultIfEmpty()
                               where ids.Contains(inv.ID)
                               select new
                               {
                                   inv.ID,
                                   Store = str.Name,
                                   inv.Code,
                                   Drawer = drw.Name,
                                   PartName = prt.Name,
                                   Phone = prt.Phone,
                                   inv.Date,
                                   inv.DiscountValue,
                                   inv.Expences,
                                   InvoiceType =
                                     (inv.InvoiceType == (byte)Master.InvoiceType.Purchase) ? "فاتورة مشتريات" :
                                     (inv.InvoiceType == (byte)Master.InvoiceType.Sales) ? "فاتورة مبيعات" :
                                     (inv.InvoiceType == (byte)Master.InvoiceType.SalesReturn) ? "فاتورة مردود مبيعات" :
                                     (inv.InvoiceType == (byte)Master.InvoiceType.PurchaseReturn) ? "فاتورة مردود مشتريات" : "UnDefined",
                                   inv.Net,
                                   inv.Notes,
                                   inv.Paid,
                                   PartType =
                                     (inv.PartType == (byte)Master.PartType.Customer) ? "عميل" :
                                     (inv.PartType == (byte)Master.PartType.Vendor) ? "مورد" : "UnDefined",
                                   inv.Remaing,
                                   inv.TaxValue,
                                   inv.Total,
                                   ProductCount = db.InvoiceDetails.Where(x => x.InvoiceID == inv.ID).Count(),
                                   // List في التقرير ويكون على شكل rpt.DetailReport.DataMember = "Products" هذا الاسم يوضع في "Products"
                                   Products = (
                                   from d in db.InvoiceDetails.Where(x => x.InvoiceID == inv.ID)
                                   from p in db.Products.Where(x => x.ID == d.ItemID)
                                   from u in db.UnitNames.Where(x => x.ID == d.ItemUnitID).DefaultIfEmpty()
                                   select new
                                   {
                                       ProductName = p.Name,
                                       UnitName = u.Name,
                                       d.ItemQty,
                                       d.Price,
                                       d.TotalPrice
                                   }).ToList()
                               }).ToList();
                invoice.ForEach(x =>
                {
                    frm_Master.InsertUserAction(ActionType.Print, x.ID, x.Code + " - " + x.PartName, CallerScreenName);
                });
                Reporting.rpt_Invoice.Print(invoice);// ارسال البيانات لدالة الطباعة في التقرير
            }
        }
        /// <summary>
        /// حذف مجموعة فواتير محددة
        /// </summary>
        /// <param name="ids">قائمة بارقام الفواتير</param>
        /// <param name="Type"></param>
        /// <param name="CallerScreenName"></param>
        public static void Delete(List<int> ids, Master.InvoiceType Type, string CallerScreenName)
        {
            /////////////////////////// من أجل صلاحيات المستخدم /////////////////////////////
            var name = string.Empty;
            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    name = Class.Screens.AddPurchaseInvoice.ScreenName;
                    break;
                case Master.InvoiceType.Sales:
                    name = Class.Screens.AddSalesInvoice.ScreenName;
                    break;
                case Master.InvoiceType.PurchaseReturn:
                    name = Class.Screens.AddPurchaseReturnInvoice.ScreenName;
                    break;
                case Master.InvoiceType.SalesReturn:
                    name = Class.Screens.AddSalesReturnInvoice.ScreenName;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (CheckActionAuthorization(name, Master.Actions.Delete) == false)
                return;

            if (AskForDeletion())
            {
                string TotalMsg = string.Empty;
                foreach (var id in ids)
                {
                    string msg;
                    if (Delete(id, out msg))
                    {
                        frm_Master.InsertUserAction(ActionType.Delete, id, string.Empty, CallerScreenName);
                    }
                    TotalMsg += msg + Environment.NewLine;
                }
                UI.ViewActionReport.ViewMSG(TotalMsg);
            }
        }
        /// <summary>
        /// حذف الفاتورة التي لا يوجد ماهو مرتبط بها
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Delete(int id, out string msg)
        {
            using (var DB = new DAL.DBDataContext())
            {
                var Invoice = DB.InvoiceHeaders.Where(x => x.ID == id).SingleOrDefault();
                if (DB.InvoiceHeaders.Where(x => x.SourceID == id).Count() > 0)
                {
                    msg = $"لا يمكن حذف الفاتورةرقم {id} حيث يوجد سندات أخرى مرتبطة بها";
                    return false;
                }
                DB.StoreLogs.DeleteAllOnSubmit(DB.StoreLogs.Where(x => x.SourceType == Invoice.InvoiceType && DB.InvoiceDetails.Where(y => y.InvoiceID == id).Select(y => y.ID).Contains(x.SourceID)));
                DB.SubmitChanges();
                DB.Journals.DeleteAllOnSubmit(DB.Journals.Where(y => y.SourceID == id));
                DB.SubmitChanges();
                DB.InvoiceDetails.DeleteAllOnSubmit(DB.InvoiceDetails.Where(y => y.InvoiceID == id));
                DB.SubmitChanges();
                DB.InvoiceHeaders.DeleteOnSubmit(Invoice);
                DB.SubmitChanges();
                msg = $"تم حذف الفاتورة رقم {id} بنجاح";
                return true;
            }
        }

        /// <summary>
        /// حذف فاتورة واحدة
        /// </summary>
        public override void Delete()
        {
            if (AskForDeletion())
            {
                string msg;
                if (Delete(Invoice.ID, out msg))
                {
                    base.Delete();
                    btn_New.PerformClick();
                }
                else
                {
                    XtraMessageBox.Show(
                        text: "لا يمكن حذف الفاتورة حيث يوجد سندات أخرى مرتبطة بها",
                        caption: "فشل الحذف",
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error);
                    return;
                }


            }
        }
        /// <summary>
        /// تطبيق اعدادات المستخدم
        /// </summary>
        void ReadUserSettings()
        {
            var ins = new DAL.InvoiceDetail();
            switch (Type)
            {
                case Master.InvoiceType.Purchase:
                    break;
                case Master.InvoiceType.Sales:
                    spn_Paid.Enabled = Session.UserSettings.Sales.CanChangePaidInSales;// امكانية تغيير المدفوع
                    chk_PostToStore.Enabled = Session.UserSettings.Sales.CanNotPostToStoreInSales;// امكانية تغيير الترحيل الى المخزن
                    gridView1.Columns["Price"].OptionsColumn.AllowEdit = Session.UserSettings.Sales.CanChangeItemPriceInSales;// امكانية تغيير سعر الصنف
                    gridView1.Columns["ItemQty"].OptionsColumn.AllowEdit = Session.UserSettings.Sales.CanChangeQtyInSales;// امكانية تغيير الكمية في الصنف يدويا عند البيع
                    gridView1.Columns[nameof(ins.CostValue)].Visible =
                    gridView1.Columns[nameof(ins.TotalCostValue)].Visible =
                    gridView1.Columns[nameof(ins.CostValue)].OptionsColumn.ShowInCustomizationForm =
                    gridView1.Columns[nameof(ins.TotalCostValue)].OptionsColumn.ShowInCustomizationForm = !Session.UserSettings.Sales.HideCostInSales;//ShowInCustomizationForm اخفاء التكلفة في فاتورة البيع وفي أداة 

                    lkp_PartType.Enabled = Session.UserSettings.Sales.CanSellToVendor;// امكانية البيع للمورد
                    dt_Date.Enabled = Session.UserSettings.Sales.CanChangeSalesInvoiceDate;// امكانية تغيير تاريخ البيع
                    break;
                case Master.InvoiceType.PurchaseReturn:
                case Master.InvoiceType.SalesReturn:
                    break;
                default:
                    throw new NotImplementedException();
            }

        }

        private void Chk_PostToStore_CheckedChanged(object sender, EventArgs e)
        {
            lyc_PostDate.Visibility = chk_PostToStore.Checked ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }
        /// <summary>
        /// دالة عرض شاشة المرتجع
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SelectItems_Click(object sender, EventArgs e)
        {
            XtraForm form = new XtraForm()
            {
                Size = new Size(500, 750),
                StartPosition = FormStartPosition.CenterScreen,
                Name = "SelectSourceItems",
                Text = "اختيار الأصناف للارجاع",
                RightToLeft = System.Windows.Forms.RightToLeft.Yes,
                RightToLeftLayout = true
            };
            form.IconOptions.ShowIcon = false;
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            GridControl gridControl = new GridControl();
            gridControl.Dock = DockStyle.Fill;
            GridView view = new GridView();
            gridControl.MainView = view;
            view.OptionsBehavior.Editable = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsCustomization.AllowColumnMoving = false;
            view.OptionsCustomization.AllowQuickHideColumns = false;
            view.OptionsSelection.MultiSelect = true;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            view.OptionsBehavior.AutoPopulateColumns = false;
            view.OptionsCustomization.AllowSort = false;
            view.OptionsView.ShowIndicator = false;
            //view حدث اختيار عناصر من النافذة 
            var source = gridView1.DataSource as Collection<DAL.InvoiceDetail>;
            view.SelectionChanged += (viewSender, ChangeEvent) =>
            {
                var selectedRow = this.ReturnSourceDetails[ChangeEvent.ControllerRow];

                if (source == null) return;
                if (ChangeEvent.Action == CollectionChangeAction.Add)
                {
                    if (source.Where(x => x.sourceRowID == selectedRow.ID).Count() == 0)
                    {
                        source.Add(new DAL.InvoiceDetail()
                        {
                            CostValue = selectedRow.CostValue,
                            ItemID = selectedRow.ItemID,
                            ItemUnitID = selectedRow.ItemUnitID,
                            ItemQty = selectedRow.ItemQty,
                            DiscountValue = selectedRow.DiscountValue,
                            Price = selectedRow.Price,
                            sourceRowID = selectedRow.ID,
                            Discount = selectedRow.Discount,
                            StoreID = (lkp_Branch.EditValue is int storeID) ? storeID : selectedRow.StoreID,
                            TotalCostValue = selectedRow.TotalCostValue,
                            TotalPrice = selectedRow.TotalPrice,

                        });
                    }

                }
                else if (ChangeEvent.Action == CollectionChangeAction.Remove)
                {
                    if (source.Where(x => x.sourceRowID == selectedRow.ID).Count() > 0)
                    {
                        source.Remove(source.Single(x => x.sourceRowID == selectedRow.ID));
                    }
                }
            };


            gridControl.RepositoryItems.AddRange(new RepositoryItem[] { repoUOM, repoItemsAll });

            // لجلب ID واخفائه
            view.Columns.AddField("ID");
            //لجلب اسماء عناصر الفاتورة واظهارها
            var ProductColumn = view.Columns.AddField(nameof(detailsInstace.ItemID));
            ProductColumn.Visible = true;
            ProductColumn.Caption = "الصنف";
            //لجلب وحدات عناصر الفاتورة واظهارها
            var UnitColumn = view.Columns.AddField(nameof(detailsInstace.ItemUnitID));
            UnitColumn.Visible = true;
            UnitColumn.Caption = "الوحدة";

            //لجلب كميات عناصر الفاتورة واظهارها
            var clm_ItemQty = view.Columns.AddField(nameof(detailsInstace.ItemQty));
            clm_ItemQty.Caption = "الكمية";
            clm_ItemQty.Visible = true;

            var repoMyItems = new RepositoryItemLookUpEdit();
            var repoMyUOM = new RepositoryItemLookUpEdit();
            repoMyItems.InitializeData(Session.ProductView, ProductColumn, gridControl);
            repoMyUOM.InitializeData(Session.UnitNames, UnitColumn, gridControl);
            gridControl.DataSource = this.ReturnSourceDetails;
            form.Controls.Add(gridControl);

            // وضع العناصر المختارة مسبقا مختارة عند فتح النافذة مرة أخرى
            gridControl.ForceInitialize();
            if (source != null)
                for (int i = 0; i < this.ReturnSourceDetails.Count(); i++)
                {
                    if (source.Where(x => x.sourceRowID == ReturnSourceDetails[i].ID).Count() > 0)
                        view.SelectRow(i);
                }

            form.ShowDialog();
        }
    }
}