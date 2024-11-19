using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLinq.Class
{
    public class UserSettingsTemplate
    {
        int ProfileID { get; set; }
        public UserSettingsTemplate (int profileid)
        {
            ProfileID = profileid;
            General = new GeneralSettings(ProfileID);
            Invoices = new InvoicesSettings(ProfileID);
            Sales = new SalesSettings(ProfileID);
            Purchase = new PurchaseSettings(ProfileID);
        }

        public GeneralSettings General { get; set; }       // اعدادات عامة
        public InvoicesSettings Invoices { get; set; }     // اعدادات الفاتورة
        public SalesSettings Sales { get; set; }           // اعدادات البيع
        public PurchaseSettings Purchase { get; set; }     // اعدادات الشراء
        /// <summary>
        /// دالة ترجع اسماء كل العناصر بالعربية
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string GetPropCaption(string propName)
        {
            UserSettingsTemplate ins;
            switch (propName)
            {
                case nameof(ins.General):return "اعدادات عامة";
                case nameof(ins.Invoices):return "اعدادات الفواتير";
                case nameof(ins.Sales):return "اعدادات البيع";
                case nameof(ins.Purchase):return "اعدادات الشراء";
                case nameof(ins.General.DefaultRowStore):return "المخزن الافتراضي للخامات";
                case nameof(ins.General.DefaultStore):return "المخزن الافتراضي";
                case nameof(ins.General.DefaultDrawer):return "الخزينة الافتراضية";
                case nameof(ins.General.DefaultCustomer):return "العميل الافتراضي";
                case nameof(ins.General.DefaultVendor):return "المورد الافتراضي";
                case nameof(ins.General.CanChangeStore):return "السماح بتغيير المخزن";
                case nameof(ins.General.CanChangeDrawer):return "السماح بتغيير الخزنة";
                case nameof(ins.General.CanChangeCustomer):return "السماح بتغيير العميل";
                case nameof(ins.General.CanChangeVendor):return "السماح بتغيير المورد";
                case nameof(ins.General.CanViewDocumentHistory):return "بامكانه عرض سجل التغييرات";
                case nameof(ins.Invoices.CanChangeTax): return "السماح بتغيير الضريبة";
                case nameof(ins.Invoices.CanDeleteItemsInInvoices): return "السماح بحذف الأصناف من الفاتورة";
                case nameof(ins.Sales.CanChangePaidInSales): return "السماح بتغيير المبلغ المدفوع";
                case nameof(ins.Sales.CanNotPostToStoreInSales): return "انشاء فواتير بدون صرف";
                case nameof(ins.Sales.DefaultPayMethodInSales): return "نوع السداد الافتراضي";
                case nameof(ins.Sales.WhenSellingToCustomerExeededMaxCredit): return "عند البيع لعميل تجاوز حد الائتمان";
                case nameof(ins.Sales.CanChangeItemPriceInSales): return "السماح بتغيير سعر الصنف";
                case nameof(ins.Invoices.WhenSellingItemReachedReOrderLevel): return "عند صرف صنف وصل رصيده الى حد الطلب";
                case nameof(ins.Invoices.WhenSellingItemWithQtyMoreThanAvailableQty): return "عند صرف كمية من صنف اكثر من المتاح";
                case nameof(ins.Sales.WhenSellingItemWithPriceLowerThanCostPrice): return "عند البيع بسعر أقل من سعر التكلفة";
                case nameof(ins.Sales.MaxDiscountInInvoice): return "أقصى خصم مسموح للفاتورة";
                case nameof(ins.Sales.MaxDiscountPerItem): return "أقصى خصم مسموح للصنف";
                case nameof(ins.Sales.CanSellToVendor): return "السماح بالبيع للموردين";
                case nameof(ins.Sales.CanChangeSalesInvoiceDate): return "السماح بتغيير التاريخ";
                case nameof(ins.Sales.CanChangeQtyInSales): return "السماح بتغيير الكمية";
                case nameof(ins.Sales.HideCostInSales): return "اخفاء التكلفة من الفاتورة";
                case nameof(ins.Purchase.CanChangeItemPriceInPurchase): return "السماح بتغيير سعر الشراء";
                case nameof(ins.Purchase.CanBuyFromCustomer): return "السماح بالشراء من العملاء";
                case nameof(ins.Purchase.CanChangePurchaseInvoiceDate): return "السماح بتغيير التاريخ";

                default: return "$" + propName + "$";
            }
        }
        /// <summary>
        /// دالة ترجع العناصر التي توضع في الشاشة الخاصة بكل خاصية
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static BaseEdit GetPropertyControl(string propName, object propertyValue)
        {
            UserSettingsTemplate ins;
            BaseEdit edit = null;//أصل كل الكونترول
            switch (propName)
            {
                case nameof(ins.General.CanChangeCustomer):
                case nameof(ins.General.CanChangeStore):
                case nameof(ins.General.CanChangeDrawer):
                case nameof(ins.General.CanChangeVendor):
                case nameof(ins.General.CanViewDocumentHistory):
                case nameof(ins.Invoices.CanChangeTax):
                case nameof(ins.Invoices.CanDeleteItemsInInvoices):
                case nameof(ins.Sales.CanChangePaidInSales):
                case nameof(ins.Sales.CanNotPostToStoreInSales):
                case nameof(ins.Sales.CanChangeItemPriceInSales):
                case nameof(ins.Sales.CanSellToVendor):
                case nameof(ins.Sales.CanChangeSalesInvoiceDate):
                case nameof(ins.Sales.CanChangeQtyInSales):
                case nameof(ins.Sales.HideCostInSales):
                case nameof(ins.Purchase.CanChangeItemPriceInPurchase):
                case nameof(ins.Purchase.CanBuyFromCustomer):
                case nameof(ins.Purchase.CanChangePurchaseInvoiceDate):
                    edit = new ToggleSwitch();
                    ((ToggleSwitch)edit).Properties.OnText = "نعم";
                    ((ToggleSwitch)edit).Properties.OffText = "لا";
                    break;
                case nameof(ins.General.DefaultStore):
                case nameof(ins.General.DefaultRowStore):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Session.Store);
                    break;
                case nameof(ins.General.DefaultDrawer):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Session.Drawer);
                    break;
                case nameof(ins.General.DefaultCustomer):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Session.Customers);
                    break;
                case nameof(ins.General.DefaultVendor):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Session.vendores);
                    break;
                case nameof(ins.Sales.DefaultPayMethodInSales):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Master.PayMethodsList);
                    break;
                case nameof(ins.Sales.WhenSellingToCustomerExeededMaxCredit):
                case nameof(ins.Invoices.WhenSellingItemReachedReOrderLevel):
                case nameof(ins.Invoices.WhenSellingItemWithQtyMoreThanAvailableQty):
                case nameof(ins.Sales.WhenSellingItemWithPriceLowerThanCostPrice):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).InitializeData(Master.WorningLevelsList);
                    break;
                case nameof(ins.Sales.MaxDiscountInInvoice):
                case nameof(ins.Sales.MaxDiscountPerItem):
                    edit = new SpinEdit();
                    ((SpinEdit)edit).Properties.Increment = 0.01m;                                           // القيمة التي يأخذها عند تدوير عدلة الفأرة
                    ((SpinEdit)edit).Properties.EditMask = "p";                                              //نوع الماسك الخاص بالنسبة المئوية
                    ((SpinEdit)edit).Properties.Mask.UseMaskAsDisplayFormat = true;                          // استخدام الماسك في العرض
                    ((SpinEdit)edit).Properties.MaxValue = 1;                                                //القيمة العليا للخلية 
                    break;

                default:
                    break;
            }
            if (edit != null)
            {
                edit.Name = propName;
                edit.Properties.NullText = "";
                edit.EditValue = propertyValue;
            }
            return edit;
        }
    }
    /// <summary>
    /// اعدادات عامة
    /// </summary>
    public class GeneralSettings
    {
        int ProfileID { get; set; }
        public GeneralSettings(int profileid)
        {
            ProfileID = profileid;
        }
        public int DefaultRowStore { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }            //المخزن الافتراضي عند الشراء
        public int DefaultStore { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }               //المخزن الافتراضي عند البيع
        public int DefaultDrawer { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }              //الخزنة الافتراضية
        public int DefaultCustomer { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }            //العميل الافتراضي
        public int DefaultVendor { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }              //المورد الافتراضي
        public bool CanChangeStore { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(),ProfileID)); } }            //يستطيع تغيير المخزن
        public bool CanChangeDrawer { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }          //يستطيع نغيير الخزنة الافتراضية
        public bool CanChangeCustomer { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }        //يستطيع تغيير العميل الافتراضي
        public bool CanChangeVendor { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }          //يستطيع تغيير المورد الافتراضي
        public bool CanViewDocumentHistory { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }   //يستطيع استعراض سجل التغييرات
    }

    /// <summary>
    /// اعدادات الفاتورة
    /// </summary>
    public class InvoicesSettings
    {
        int ProfileID { get; set; }
        public InvoicesSettings(int profileid)
        {
            ProfileID = profileid;
        }
        public bool CanChangeTax { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                               //يستطيع تغيير الضريبة
        public bool CanDeleteItemsInInvoices { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                   //يستطيع حذف عناصر من الفاتورة
        public Master.WorningLevels WhenSellingItemReachedReOrderLevel { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }         //عند صرف صنف وصل رصيده الى حد الطلب
        public Master.WorningLevels WhenSellingItemWithQtyMoreThanAvailableQty { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } } //عند صرف كمية من صنف اكثر من المتاح

    }

    /// <summary>
    /// اعدادات البيع
    /// </summary>
    public class SalesSettings
    {
        int ProfileID { get; set; }
        public SalesSettings(int profileid)
        {
            ProfileID = profileid;
        }
        public bool CanChangePaidInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                         //يستطيع تغيير المدفوع في فاتورة البيع
        public bool CanNotPostToStoreInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                     //يستطيع عمل فاتورة بدون ترحيل
        public Master.PayMethods DefaultPayMethodInSales { get { return Master.FromByteArray<Master.PayMethods>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                            //طريقة الدفع الافتراضية عند البيع
        public Master.WorningLevels WhenSellingToCustomerExeededMaxCredit { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }        //البيع لعميل زاد عن حد الائتمان
        public bool CanChangeItemPriceInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                    //يستطيع تغيير السعر في فاتورة البيع
        public Master.WorningLevels WhenSellingItemWithPriceLowerThanCostPrice { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }   //عند البيع بسعر أقل من سعر التكلف
        public decimal MaxDiscountInInvoice { get { return Master.FromByteArray<decimal>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                   //أقصى حد للخصم في الفاتورة ككل
        public decimal MaxDiscountPerItem { get { return Master.FromByteArray<decimal>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                     //أقصى حد للخصم للعنصر الواحد
        public bool CanSellToVendor { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                              //يستطيع البيع للموردين
        public bool CanChangeSalesInvoiceDate { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                    //يستطيع تغيير تاريخ فاتورة البيع
        public bool CanChangeQtyInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                          //يستطيع تغيير الكمية في البيع
        public bool HideCostInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                              //اخفاء سعر التكلفة
    }

    /// <summary>
    /// اعدادات الشراء
    /// </summary>
    public class PurchaseSettings
    {
        int ProfileID { get; set; }
        public PurchaseSettings(int profileid)
        {
            ProfileID = profileid;
        }
        public bool CanChangeItemPriceInPurchase { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }    //يستطيع تغيير السعر في فاتورة الشراء
        public bool CanBuyFromCustomer { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }              //يستطيع الشراء من العملاء
        public bool CanChangePurchaseInvoiceDate { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }    //يستطيع تغيير تاريخ فاتورة الشراء

    }
}
