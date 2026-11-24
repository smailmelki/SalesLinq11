using System;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Class
{
    public class UserSettingsTemplate
    {
        int ProfileID { get; set; }
        public UserSettingsTemplate (int profileid)
        {
            ProfileID = profileid;
            Default = new DefaultlSettings(ProfileID);
            Invoices = new InvoicesSettings(ProfileID);
            Sales = new SalesSettings(ProfileID);
            Purchase = new PurchaseSettings(ProfileID);
            Genera = new GeneraSettings(ProfileID);
        }

        public DefaultlSettings Default { get; set; }       // اعدادات عامة
        public InvoicesSettings Invoices { get; set; }     // اعدادات الفاتورة
        public SalesSettings Sales { get; set; }           // اعدادات البيع
        public PurchaseSettings Purchase { get; set; }     // اعدادات الشراء
        public GeneraSettings Genera { get; set; }     // اعدادات الشراء
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
                case nameof(ins.Default):return "اعدادات افتراضية";
                case nameof(ins.Invoices):return "اعدادات الفواتير";
                case nameof(ins.Sales):return "اعدادات البيع";
                case nameof(ins.Purchase):return "اعدادات الشراء";
                case nameof(ins.Genera):return "اعدادات عامة";
                case nameof(ins.Default.DefaultRowStore):return "المخزن الافتراضي للخامات";
                case nameof(ins.Default.DefaultStore):return "المخزن الافتراضي";
                case nameof(ins.Default.DefaultDrawer):return "الخزينة الافتراضية";
                case nameof(ins.Default.DefaultCustomer):return "العميل الافتراضي";
                case nameof(ins.Default.DefaultVendor):return "المورد الافتراضي";
                case nameof(ins.Default.CanChangeStore):return "السماح بتغيير المخزن";
                case nameof(ins.Default.CanChangeDrawer):return "السماح بتغيير الخزنة";
                case nameof(ins.Default.CanChangeCustomer):return "السماح بتغيير العميل";
                case nameof(ins.Default.CanChangeVendor):return "السماح بتغيير المورد";
                case nameof(ins.Default.CanViewDocumentHistory):return "بامكانه عرض سجل التغييرات";
                case nameof(ins.Invoices.CanChangeTax): return "السماح بتغيير الضريبة";
                case nameof(ins.Invoices.CanDeleteItemsInInvoices): return "السماح بحذف الأصناف من الفاتورة";
                case nameof(ins.Invoices.WhenSellingItemReachedReOrderLevel): return "عند صرف صنف وصل رصيده الى حد الطلب";
                case nameof(ins.Invoices.WhenSellingItemWithQtyMoreThanAvailableQty): return "عند صرف كمية من صنف اكثر من المتاح";
                case nameof(ins.Sales.CanChangePaidInSales): return "السماح بتغيير المبلغ المدفوع";
                case nameof(ins.Sales.CanNotPostToStoreInSales): return "انشاء فواتير بدون صرف";
                case nameof(ins.Sales.DefaultPayMethodInSales): return "نوع السداد الافتراضي";
                case nameof(ins.Sales.WhenSellingToCustomerExeededMaxCredit): return "عند البيع لعميل تجاوز حد الائتمان";
                case nameof(ins.Sales.CanChangeItemPriceInSales): return "السماح بتغيير سعر الصنف";
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
                case nameof(ins.Genera.PrintMode):return "طريقة الطباعة";
                case nameof(ins.Genera.ReadValueMode):return "نوع ميزان الباركود";
                case nameof(ins.Genera.AcceptReadFromScaleBarcode):return "السماح بالقراءة من ميزان الباركود";

                default: return "$" + propName + "$";
            }
        }
        /// <summary>
        /// دالة ترجع العناصر التي توضع في الشاشة الخاصة بكل خاصية
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static Control GetPropertyControl(string propName, object propertyValue)
        {
            UserSettingsTemplate ins;
            Control control = null;

            switch (propName)
            {
                case nameof(ins.Default.CanChangeCustomer):
                case nameof(ins.Default.CanChangeStore):
                case nameof(ins.Default.CanChangeDrawer):
                case nameof(ins.Default.CanChangeVendor):
                case nameof(ins.Default.CanViewDocumentHistory):
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
                case nameof(ins.Genera.AcceptReadFromScaleBarcode):
                    control = new CheckBox
                    {
                        Checked = Convert.ToBoolean(propertyValue),
                        Text = "مفعّل",
                        AutoSize = true
                    };
                    break;

                case nameof(ins.Default.DefaultStore):
                case nameof(ins.Default.DefaultRowStore):
                    control = CreateLookup(Session.Store.Select(x => new Master.ValueAndID { ID = x.ID, Name = x.Name }), propertyValue);
                    break;
                case nameof(ins.Default.DefaultDrawer):
                    control = CreateLookup(Session.Drawer.Select(x => new Master.ValueAndID { ID = x.ID, Name = x.Name }), propertyValue);
                    break;
                case nameof(ins.Default.DefaultCustomer):
                    control = CreateLookup(Session.Customers.Select(x => new Master.ValueAndID { ID = x.ID, Name = x.Name }), propertyValue);
                    break;
                case nameof(ins.Default.DefaultVendor):
                    control = CreateLookup(Session.vendores.Select(x => new Master.ValueAndID { ID = x.ID, Name = x.Name }), propertyValue);
                    break;
                case nameof(ins.Sales.DefaultPayMethodInSales):
                    control = CreateLookup(Master.PayMethodsList, propertyValue);
                    break;
                case nameof(ins.Sales.WhenSellingToCustomerExeededMaxCredit):
                case nameof(ins.Invoices.WhenSellingItemReachedReOrderLevel):
                case nameof(ins.Invoices.WhenSellingItemWithQtyMoreThanAvailableQty):
                case nameof(ins.Sales.WhenSellingItemWithPriceLowerThanCostPrice):
                    control = CreateLookup(Master.WorningLevelsList, propertyValue);
                    break;
                case nameof(ins.Sales.MaxDiscountInInvoice):
                case nameof(ins.Sales.MaxDiscountPerItem):
                    control = new NumericUpDown
                    {
                        Minimum = 0,
                        Maximum = 1,
                        Increment = 0.01m,
                        DecimalPlaces = 2,
                        Value = Math.Max(0, Math.Min(1, Convert.ToDecimal(propertyValue)))
                    };
                    break;
                case nameof(ins.Genera.PrintMode):
                    control = CreateLookup(Master.PrintModeTypesList, propertyValue);
                    break;
                case nameof(ins.Genera.ReadValueMode):
                    control = CreateLookup(Master.ReadValueModeList, propertyValue);
                    break;
            }

            if (control != null)
            {
                control.Name = propName;
                control.Dock = DockStyle.Fill;
                control.Margin = new Padding(4, 6, 4, 6);
            }

            return control;
        }

        private static ComboBox CreateLookup(System.Collections.Generic.IEnumerable<Master.ValueAndID> source, object value)
        {
            var combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Name",
                ValueMember = "ID",
                DataSource = source.ToList()
            };
            if (value != null)
                combo.SelectedValue = Convert.ToInt32(value);
            return combo;
        }
    }
    /// <summary>
    /// اعدادات عامة
    /// </summary>
    public class DefaultlSettings
    {
        int ProfileID { get; set; }
        public DefaultlSettings(int profileid)
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
        public Master.WorningLevels WhenSellingItemWithQtyMoreThanAvailableQty { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } } //عند صرف كمية من صنف اكثر من المتاح
        public Master.WorningLevels WhenSellingItemReachedReOrderLevel { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }         //عند صرف صنف وصل رصيده الى حد الطلب
        public bool CanDeleteItemsInInvoices { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                   //يستطيع حذف عناصر من الفاتورة
        public bool CanChangeTax { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                               //يستطيع تغيير الضريبة

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
        public Master.PayMethods DefaultPayMethodInSales { get { return Master.FromByteArray<Master.PayMethods>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                            //طريقة الدفع الافتراضية عند البيع
        public Master.WorningLevels WhenSellingToCustomerExeededMaxCredit { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }        //البيع لعميل زاد عن حد الائتمان
        public Master.WorningLevels WhenSellingItemWithPriceLowerThanCostPrice { get { return Master.FromByteArray<Master.WorningLevels>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }   //عند البيع بسعر أقل من سعر التكلف
        public decimal MaxDiscountInInvoice { get { return Master.FromByteArray<decimal>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                   //أقصى حد للخصم في الفاتورة ككل
        public decimal MaxDiscountPerItem { get { return Master.FromByteArray<decimal>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                     //أقصى حد للخصم للعنصر الواحد
        public bool CanChangePaidInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                         //يستطيع تغيير المدفوع في فاتورة البيع
        public bool CanNotPostToStoreInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                     //يستطيع عمل فاتورة بدون ترحيل
        public bool CanChangeItemPriceInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                    //يستطيع تغيير السعر في فاتورة البيع
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

    public class GeneraSettings
    {
        int ProfileID { get; set; }
        public GeneraSettings(int profileid)
        {
            ProfileID = profileid;
        }
        public Master.PrintMode PrintMode { get { return Master.FromByteArray<Master.PrintMode>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } } //طريقة الطباعة
        public Master.ReadValueMode ReadValueMode { get { return Master.FromByteArray<Master.ReadValueMode>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } } //طريقة الطباعة
        public bool AcceptReadFromScaleBarcode { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetCallerName(), ProfileID)); } }                                                              //اخفاء سعر التكلفة


    }
}
