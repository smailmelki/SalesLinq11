using SalesLinq.Forms;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SalesLinq.Class
{
    /// <summary>
    /// كلاص به خصائص الزر
    /// </summary>
    public class ScreensAccessProfile
    {
        public static int MaxID = 1;
        /// <summary>
        /// دالة تعطي للشاشة خصائصها
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public ScreensAccessProfile(string name, ScreensAccessProfile parent=null)
        {
            ScreenName = name;
            ScreenID = MaxID++;
            if (parent != null)
                ParentScreenID = parent.ScreenID;
            else
                ParentScreenID = 0;
            Actions = new List<Master.Actions>()
            {
                Master.Actions.Show,
                Master.Actions.Open,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print,
            };
        }

        public int ScreenID { get; set; }            // رقم الزر
        public int ParentScreenID { get; set; }      // القائمة التابعة لها
        public string ScreenName { get; set; }       // اسم الشاشة
        public string ScreenCaption { get; set; }    // الاسم الظاهر على الزر
        public bool CanShow { get; set; }            // يستطيع رؤيتها
        public bool CanOpen { get; set; }            // يستطيع فتحها
        public bool CanAdd { get; set; }             // يستطيع الاضافة منها
        public bool CanEdit { get; set; }            // يستطيع التعديل منها
        public bool CanDelete { get; set; }          // يستطيع الحذف منها
        public bool CanPrint { get; set; }           // يستطيع الطباعة منها

        public List<Master.Actions> Actions { get; set; }
    }
    public static class Screens
    {
        /// <summary>
        /// زر مجموعة البيانات 
        /// </summary>
        public static ScreensAccessProfile mainSettings = new ScreensAccessProfile("elm_MainSettings")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "البيانات"
        };
        /// <summary>
        ///  زر بيانات الشركة تابع لمجموعة البيانات
        /// </summary>
        public static ScreensAccessProfile CompanyInfo = new ScreensAccessProfile(nameof(frm_CompantInfo), mainSettings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show , Master.Actions .Edit, Master.Actions .Open},//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "بيانات الشركة"
        };
        /// <summary>
        ///  زر مجموعة العملاء 
        /// </summary>
        public static ScreensAccessProfile Customers = new ScreensAccessProfile("elm_Customers")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "العملاء"
        };
        /// <summary>
        ///  زر اضافة عميل تابع لمجموعة العملاء
        /// </summary>
        public static ScreensAccessProfile AddCustomer = new ScreensAccessProfile("frm_Customer", Customers)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة عميل"
        };
        /// <summary>
        /// زر عرض العملاء تابع لمجموعة العملاء
        /// /// </summary>
        public static ScreensAccessProfile ViewCustomer = new ScreensAccessProfile("frm_CustomerList", Customers)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض العملاء"
        };
        /// <summary>
        /// زر مجموعة الموردون 
        /// </summary>
        public static ScreensAccessProfile Vendors = new ScreensAccessProfile("elm_Vendors")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "الموردون"
        };
        /// <summary>
        /// زر اضافة مورد تابع لمجموعة الموردون
        /// </summary>
        public static ScreensAccessProfile AddVendor = new ScreensAccessProfile("frm_Vendor", Vendors)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة مورد"
        };
        /// <summary>
        /// زر عرض الموردين تابع لمجموعة الموردون
        /// </summary>
        public static ScreensAccessProfile ViewVendors = new ScreensAccessProfile("frm_VendorList", Vendors)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض الموردين" 
        };
        /// <summary>
        /// زر مجموعة الأفرع أو المخازن 
        /// </summary>
        public static ScreensAccessProfile Stores = new ScreensAccessProfile("elm_Stores")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "الأفرع"
        };
        /// <summary>
        /// زر اضافة مخزن تابع لمجموعة المخازن
        /// </summary>
        public static ScreensAccessProfile AddStore = new ScreensAccessProfile(nameof(frm_Stores), Stores)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة مخزن" 
        };
        /// <summary>
        /// زر عرض المخازن تابع لمجموعة المخازن
        /// </summary>
        public static ScreensAccessProfile ViewStores = new ScreensAccessProfile(nameof(frm_StoresList), Stores)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض المخازن" 
        };
        /// <summary>
        /// زر مجموعة الخزن 
        /// </summary>
        public static ScreensAccessProfile Drawers = new ScreensAccessProfile("elm_Drawers")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "الخزن"
        };
        /// <summary>
        /// زر اضافة خزنة تابع لمجموعة الخزن
        /// </summary>
        public static ScreensAccessProfile AddDrawer = new ScreensAccessProfile(nameof(frm_Drawer), Drawers)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة خزنة" 
        };
        /// <summary>
        /// زر عرض الخزن تابع لمجموعة الخزن
        /// </summary>
        public static ScreensAccessProfile ViewDrawers = new ScreensAccessProfile(nameof(frm_DrawerList), Drawers)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض الخزن" 
        };
        /// <summary>
        /// زر مجموعة الأصناف 
        /// </summary>
        public static ScreensAccessProfile Products = new ScreensAccessProfile("elm_Products")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "الأصناف"
        };
        /// <summary>
        /// زر عرض الأصناف تابع لمجموعة الأصناف
        /// </summary>
        public static ScreensAccessProfile ProductCategory = new ScreensAccessProfile(nameof(frm_ProductCategory), Products)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "مجموعات الأصناف" 
        };
        /// <summary>
        /// زر اضافة صنف تابع لمجموعة الأصناف
        /// </summary>
        public static ScreensAccessProfile AddProduct = new ScreensAccessProfile(nameof(frm_Product), Products)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة صنف" 
        };
        /// <summary>
        /// زر عرض الأصناف تابع لمجموعة الأصناف
        /// </summary>
        public static ScreensAccessProfile ViewProducts = new ScreensAccessProfile(nameof(frm_ProductList), Products)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض الأصناف" 
        };
        /// <summary>
        /// زر مجموعة المشتريات
        /// </summary>
        public static ScreensAccessProfile Purchases = new ScreensAccessProfile("elm_Purchases")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "المشتريات"
        };
        /// <summary>
        ///  زر اضافة فاتورة مشتريات تابع لمجموعة المشتريات
        /// </summary>
        public static ScreensAccessProfile AddPurchaseInvoice = new ScreensAccessProfile("frm_PurchaseInvoice", Purchases)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete, Master.Actions.Print },
            ScreenCaption = "اضافة فاتورة مشتريات" 
        };
        /// <summary>
        ///  زر عرض فواتير المشتريات تابع لمجموعة المشتريات
        /// </summary>
        public static ScreensAccessProfile ViewPurchaseInvoice = new ScreensAccessProfile("frm_PurchaseInvoiceList", Purchases)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Print },
            ScreenCaption = "عرض فواتير المشتريات" 
        };
        /// <summary>
        ///  زر اضافة فاتورة مردود مشتريات تابع لمجموعة المشتريات
        /// </summary>
        public static ScreensAccessProfile AddPurchaseReturnInvoice = new ScreensAccessProfile("frm_PurchaseReturnInvoice", Purchases)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete, Master.Actions.Print },
            ScreenCaption = "اضافة فاتورة مردود مشتريات" 
        };
        /// <summary>
        ///  زر عرض فواتير مردود المشتريات تابع لمجموعة المشتريات
        /// </summary>
        public static ScreensAccessProfile ViewPurchaseReturnInvoice = new ScreensAccessProfile("frm_PurchaseReturnInvoiceList", Purchases)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Print },
            ScreenCaption = "عرض فواتير مردود المشتريات" 
        };
        /// <summary>
        /// زر مجموعة الاعدادات 
        /// </summary>
        public static ScreensAccessProfile Settings = new ScreensAccessProfile("elm_Settings")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "الاعدادات"
        };
        /// <summary>
        /// زر اضافة نموذج اعدادات تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile AddUserSettingsProfile = new ScreensAccessProfile(nameof(frm_UserSettingsProfile), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة نموذج اعدادات" 
        };
        /// <summary>
        /// زر عرض نماذج الاعدادات تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile ViewUserSettingsProfile = new ScreensAccessProfile(nameof(frm_UserSettingsProfileList), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض نماذج الاعدادات" 
        };
        /// <summary>
        /// زر اضافة نموذج صلاحيات الوصول تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile AddAccessProfile = new ScreensAccessProfile(nameof(frm_AccessProfile), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة نموذج صلاحيات الوصول" 
        };
        /// <summary>
        /// زر عرض نماذج صلاحيات الوصول تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile ViewAccessProfile = new ScreensAccessProfile(nameof(frm_AccessProfileList), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض نماذج صلاحيات الوصول" 
        };
        /// <summary>
        /// زر اضافة مستخدم تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile AddUser = new ScreensAccessProfile(nameof(frm_User), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "اضافة مستخدم"
        };
        /// <summary>
        /// زر عرض المستخدمين تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile ViewUser = new ScreensAccessProfile(nameof(frm_UserList), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add },//الاكشن الموجودة في هذه الشاشة
            ScreenCaption = "عرض المستخدمين" 
        };
        /// <summary>
        /// زر عرض المستخدمين تابع لمجموعة الاعدادات
        /// </summary>
        public static ScreensAccessProfile ViewSetting = new ScreensAccessProfile(nameof(ConnectionSettingsForm), Settings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open },
            ScreenCaption = "اعدادات الاتصال" 
        };
        /// <summary>
        /// زر مجموعة المبيعات
        /// </summary>
        public static ScreensAccessProfile Sales = new ScreensAccessProfile("elm_Sales")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "المبيعات"
        };
        /// <summary>
        ///  زر اضافة فاتورة مبيعات تابع لمجموعة المبيعات
        /// </summary>
        public static ScreensAccessProfile AddSalesInvoice = new ScreensAccessProfile("frm_SalesInvoice", Sales)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete, Master.Actions.Print },
            ScreenCaption = "اضافة فاتورة مبيعات"
        };
        /// <summary>
        ///  زر عرض فواتير المبيعات تابع لمجموعة المبيعات
        /// </summary>
        public static ScreensAccessProfile ViewSalesInvoice = new ScreensAccessProfile("frm_SalesInvoiceList", Sales)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Print },
            ScreenCaption = "عرض فواتير المبيعات" 
        };
        /// <summary>
        ///  زر اضافة فاتورة مردود مبيعات تابع لمجموعة المبيعات
        /// </summary>
        public static ScreensAccessProfile AddSalesReturnInvoice = new ScreensAccessProfile("frm_SalesReturnInvoice", Sales)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Edit, Master.Actions.Delete, Master.Actions.Print },
            ScreenCaption = "اضافة فاتورة مردود مبيعات" 
        };
        /// <summary>
        ///  زر عرض فواتير مردود المبيعات تابع لمجموعة المبيعات
        /// </summary>
        public static ScreensAccessProfile ViewSalesReturnInvoice = new ScreensAccessProfile("frm_SalesReturnInvoiceList", Sales)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Print },
            ScreenCaption = "عرض فواتير مردود المبيعات" 
        };
        /// <summary>
        /// زر مجموعة المالية
        /// </summary>
        public static ScreensAccessProfile Finance = new ScreensAccessProfile("elm_Finance")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "المالية"
        };
        /// <summary>
        /// زر اضافة سند قبض نقدي تابع لمجموعة المالية
        /// </summary>
        public static ScreensAccessProfile AddCashIn = new ScreensAccessProfile(nameof(frm_CashNoteIn), Finance)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Delete },
            ScreenCaption = "اضافة سند قبض نقدي" 
        };
        /// <summary>
        /// زر اضافة سند دفع نقدي تابع لمجموعة المالية
        /// </summary>
        public static ScreensAccessProfile AddCashOut = new ScreensAccessProfile(nameof(frm_CashNoteOut), Finance)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Open, Master.Actions.Add, Master.Actions.Delete },
            ScreenCaption = "اضافة سند دفع نقدي" 
        };

        /// <summary>
        /// دالة ترجع قائمة بكل الشاشات
        /// </summary>
        public static List<ScreensAccessProfile> GetScreens 
        {
            get
            {
                Type t = typeof(Screens);
                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);// فقط static و public متغير يحتوي البروبرتي

                var list = new List<ScreensAccessProfile>();
                foreach (var item in fields)
                {
                    var obj = item.GetValue(null);
                    if (obj != null && obj.GetType() == typeof(ScreensAccessProfile))
                        list.Add((ScreensAccessProfile)item.GetValue(null));
                }
                return list;
            }
        }
    }
}
