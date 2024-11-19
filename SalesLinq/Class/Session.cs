using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SalesLinq.Class
{
    public static class Session
    {
        

        public static class Defualts
        {
            public static int Drawer {get => UserSettings.Default.DefaultDrawer; }       //  الخزنة ... الدرج الافتراضي
            public static int Customer { get => UserSettings.Default.DefaultCustomer; }  // العميل الافتراضي
            public static int Vendor { get => UserSettings.Default.DefaultVendor; }      //  المورد الافتراضي
            public static int Store { get => UserSettings.Default.DefaultStore; }        //  المخزن الافتراضي في حالة البيع
            public static int RamStore { get => UserSettings.Default.DefaultRowStore; }  //  المخزن الافتراضي في حالة الشراء
            public static int DiscountAllowedAccountID { get => 8; }//خصم نقدي مسموح به 44
            public static int DiscountReceivedAccountID { get => 9; }//44 خصم نقدي مكتسب
            public static int SalesTax { get => 17; }//ضريبة مضافة للبيع 
            public static int PurchaseTax { get => 18; }//ضريبة مضافة للشراء
            public static int PurchaseExpences { get => 19; }//حساب مصروفات شراء
        }
        private static DAL.CompanyInfo companyInfo;
        public static DAL.CompanyInfo CompanyInfo
        { get
            {
                if(companyInfo == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        companyInfo = db.CompanyInfos.FirstOrDefault();
                    }
                }
                return companyInfo;
            }
        }
        public static void ResetCompanyInfo()
        {
            companyInfo = null;
        }
        /// <summary>
        /// اعدادات المستخدم
        /// </summary>
        public static class CurrentSettings
        {
            public static Master.PrintMode InvoicePrintMode { get => UserSettings.Genera.PrintMode; }
        }
        public static UserSettingsTemplate _userSettings;
        public static UserSettingsTemplate UserSettings
        {
            get
            {
                if (_userSettings == null)
                    _userSettings = new UserSettingsTemplate(User.SettingsProfileID);
                return _userSettings;
            }
        }
        /// <summary>
        /// اعدادات عامة
        /// </summary>
        public static class GlobalSettings
        {
            public static bool ReadFromScaleBarcode { get => UserSettings.Genera.AcceptReadFromScaleBarcode; }// قبول القراءة من باركود الميزان
            public static byte ProductCodeLength { get => 5; }//عدد الأرقام التي  تعبر على رمز الصنف في الباركود
            public static byte BarcodeLength { get => 13; }//عدد الأرقام التي  يحتويها الباركود
            public static byte ValueCodeLength { get => 5; }//عدد الأرقام التي  تعبر على وزن أو سعر الصنف في الباركود 
            public static Master.ReadValueMode ReadMode { get => UserSettings.Genera.ReadValueMode; }// باركود خاص بالوزن أو بالسعر
            public static Boolean IgnoreCheckDigit { get => true; }//يحتوي على رقم التحقق في آخر الباركود
            public static string ScaleBarcodePrefix { get => (Session.GlobalSettings.ReadMode == Master.ReadValueMode.Price) ? "21" : "20"; }//رمز يبتدئ به باركود الميزان
            public static int DivideValueBy { get => (Session.GlobalSettings.ReadMode == Master.ReadValueMode.Price) ? 2:3; }//للتحويل من الغرام الى الكيلوغرام أو من 1 الى 100 حسب السعر أو الوزن
        }

        /// <summary>
        /// دالة ترجع قائمة الوحدات
        /// </summary>
        private static BindingList<DAL.UnitName> unitNames;
        public static BindingList<DAL.UnitName> UnitNames
        {
            get
            {
                if (unitNames == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        unitNames = new BindingList<DAL.UnitName>(db.UnitNames.ToList());
                    }
                }
                return unitNames;
            }
        }

        /// <summary>
        /// دالة ترجع قائمة الأصناف
        /// </summary>
        private static BindingList<DAL.Product> _Products;
        public static BindingList<DAL.Product> Products
        {
            get
            {
                if (_Products == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _Products = new BindingList<DAL.Product>(db.Products.ToList());
                    }
                    DataBaseWatcher.Products = new TableDependency.SqlClient.SqlTableDependency<DAL.Product>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.Products.OnChanged += DataBaseWatcher.Products_Changed; //DataBaseWatcher.Products_Changed حدث التغيير في حالة البيانات يشغل الدالة 
                    DataBaseWatcher.Products.Start();
                }
                return _Products;
            }
        }

        /// <summary>
        /// دالة ترجع قائمة وحدات الأصناف
        /// </summary>
        private static BindingList<DAL.ProductUnit> _ProductUnits;
        public static BindingList<DAL.ProductUnit> ProductUnits
        {
            get
            {
                if (_ProductUnits == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _ProductUnits = new BindingList<DAL.ProductUnit>(db.ProductUnits.ToList());
                    }
                    DataBaseWatcher.ProductUnitsWatcher = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.ProductUnits>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.ProductUnitsWatcher.OnChanged += DataBaseWatcher.ProductUnits_Changed; //DataBaseWatcher.Products_Changed حدث التغيير في حالة البيانات يشغل الدالة 
                    DataBaseWatcher.ProductUnitsWatcher.Start();
                }
                return _ProductUnits;
            }
        }

        private static BindingList<ProductBalance> productsBalances;
        public static BindingList<ProductBalance> ProductsBalances
        {
            get
            {
                if (productsBalances == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        var data = from sl in db.StoreLogs
                                   group sl by new { sl.ProductID, sl.StoreID } into g
                                   select new ProductBalance
                                   {
                                       Balance =
                                       (g.Where(x => x.IsInTransaction == true).Sum(x => (double?)x.Qty) ?? 0) -
                                       (g.Where(x => x.IsInTransaction == false).Sum(x => (double?)x.Qty) ?? 0),
                                       ProductID = g.Key.ProductID,
                                       StoreID = g.Key.StoreID
                                   };
                        productsBalances = new BindingList<ProductBalance>(data.ToList());
                    }
                    DataBaseWatcher.StoreLog = new TableDependency.SqlClient.SqlTableDependency<DAL.StoreLog>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.StoreLog.OnChanged += DataBaseWatcher.StoreLog_Changed; //DataBaseWatcher.StoreLogs_Changed حدث التغيير في حالة البيانات يشغل الدالة 
                    DataBaseWatcher.StoreLog.Start();
                }
                return productsBalances;
            }
        }

        /// <summary>
        /// كلاس لعرض الأصناف بشكل كامل واحترافي
        /// </summary>
        private static BindingList<ProductViewClass> productViewClasses;
        public static BindingList<ProductViewClass> ProductView
        {
            get
            {
                if (productViewClasses == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        var data = from pr in Session.Products
                                   join cg in db.ProductCategories on pr.CategoryID equals cg.ID
                                   select new ProductViewClass
                                   {
                                       ID = pr.ID,
                                       Code = pr.Code,
                                       Name = pr.Name,
                                       CategoryName = cg.Name,
                                       Descreption = pr.Descreption,
                                       IsActive = pr.IsActive,
                                       Type = pr.Type,
                                       // لعرض جدول داخل جدول نضيف جدول بشكل عادي وهو يضاف تلقائيا داخل الجدول
                                       Units = (from u in db.ProductUnits
                                                where u.ProductID == pr.ID
                                                join un in db.UnitNames on u.UnitID equals un.ID
                                                select new ProductViewClass.productUOMView
                                                {
                                                    UnitID = u.UnitID,
                                                    UnitName = un.Name,
                                                    Factor = u.Factor,
                                                    SellPrice = u.SellPrice,
                                                    BuyPrice = u.BuyPrice,
                                                    Barcode = u.Barcode
                                                }).ToList()
                                       // نفس الشئ مع الطريقة السابقة
                                       //UOM = db.ProductUnits.
                                       //     Where(x => x.ProductID == pr.ID).Select(u => new
                                       //    {
                                       //     UnitName = db.UnitNames.Single(un => un.ID == u.UnitID).Name,
                                       //     u.Factor,
                                       //     u.SellPrice,
                                       //     u.BuyPrice,
                                       //     u.Barcode
                                       //    }).ToList()
                                   };
                        productViewClasses = new BindingList<ProductViewClass>(data.ToList());
                    }
                }
                    return productViewClasses;
            }
        }

          
        public class ProductViewClass
        {
            public static ProductViewClass GetProduct(int id)
            {
                ProductViewClass obj;
                using (var db = new DAL.DBDataContext())
                {
                    var data = from pr in Session.Products
                               where pr.ID==id
                               join cg in db.ProductCategories on pr.CategoryID equals cg.ID
                               select new ProductViewClass
                               {
                                   ID = pr.ID,
                                   Code = pr.Code,
                                   Name = pr.Name,
                                   CategoryName = cg.Name,
                                   Descreption = pr.Descreption,
                                   IsActive = pr.IsActive,
                                   Type = pr.Type,
                                   // لعرض جدول داخل جدول نضيف جدول بشكل عادي وهو يضاف تلقائيا داخل الجدول
                                   Units = (from u in db.ProductUnits
                                            where u.ProductID == pr.ID
                                            join un in db.UnitNames on u.UnitID equals un.ID
                                            select new ProductViewClass.productUOMView
                                            {
                                                UnitID=u.UnitID,
                                                UnitName = un.Name,
                                                Factor = u.Factor,
                                                SellPrice = u.SellPrice,
                                                BuyPrice = u.BuyPrice,
                                                Barcode = u.Barcode
                                            }).ToList()
                                   // نفس الشئ مع الطريقة السابقة
                                   //Units = db.ProductUnits.Where(x => x.ProductID == pr.ID).Select(u => new
                                   //{
                                   //    UnitName = db.UnitNames.Single(un => un.ID == u.UnitID).Name,
                                   //    u.Factor,
                                   //    u.SellPrice,
                                   //    u.BuyPrice,
                                   //    u.Barcode
                                   //}).ToList()
                               };
                    obj = data.First();
                };
                   return obj;
            }
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public string Descreption { get; set; }
            public Boolean IsActive { get; set; }
            public byte Type { get; set; }
            public List<productUOMView> Units { get; set; }
            /// <summary>
            /// كلاس خاص بالوحدة
            /// </summary>
            public class productUOMView
            {
                public int UnitID { get; set; }
                public string UnitName { get; set; }
                public double Factor { get; set; }
                public double SellPrice { get; set; }
                public double BuyPrice { get; set; }
                public string Barcode { get; set; }
            }

        }

        public class ProductBalance
        {
            public int ProductID { get; set; }
            public int StoreID { get; set; }
            public double Balance { get; set; }
        }

        /// <summary>
        /// دالة ترجع قائمة الموردين
        /// </summary>
        private static BindingList<DAL.CustomersAndVendore> _vendores;
        public static BindingList<DAL.CustomersAndVendore> vendores
        {
            get
            {
                if (_vendores == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _vendores = new BindingList<DAL.CustomersAndVendore>(db.CustomersAndVendores.Where(x => x.IsCustomer == false).ToList());
                    }
                    DataBaseWatcher.Vendors = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.CustomersAndVendores>(Properties.Settings.Default.SalesDBConnectionString, filter: new DataBaseWatcher.VendorsOnly(), executeUserPermissionCheck: false);
                    DataBaseWatcher.Vendors.OnChanged += DataBaseWatcher.Vendors_Changed;
                    DataBaseWatcher.Vendors.Start();
                }
                return _vendores;
            }
        }

        /// <summary>
        /// دالة ترجع قائمة الحسابات
        /// </summary>
        private static BindingList<DAL.Account> _Accounts;
        public static BindingList<DAL.Account> Accounts
        {
            get
            {
                if (_Accounts == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _Accounts = new BindingList<DAL.Account>(db.Accounts.ToList());
                    }
                    DataBaseWatcher.AccountsList = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.Accounts>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.AccountsList.OnChanged += DataBaseWatcher.Accounts_Changed;
                    DataBaseWatcher.AccountsList.Start();
                }
                return _Accounts;
            }
        }


        /// <summary>
        /// دالة ترجع قائمة الخزنات
        /// </summary>
        private static BindingList<DAL.Drawer> _drawer;
        public static BindingList<DAL.Drawer> Drawer
        {
            get
            {
                if (_drawer == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _drawer = new BindingList<DAL.Drawer>(db.Drawers.ToList());
                    }
                    DataBaseWatcher.DrawersWatcher = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.Drawers>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.DrawersWatcher.OnChanged += DataBaseWatcher.Drawer_Changed;
                    DataBaseWatcher.DrawersWatcher.Start();
                }
                return _drawer;
            }
        }
        /// <summary>
        /// دالة ترجع قائمة المخازن
        /// </summary>
        private static BindingList<DAL.Store> _store;
        public static BindingList<DAL.Store> Store
        {
            get
            {
                if (_store == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _store = new BindingList<DAL.Store>(db.Stores.ToList());
                    }
                    DataBaseWatcher.StoresWatcher = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.Stores>(Properties.Settings.Default.SalesDBConnectionString, executeUserPermissionCheck: false);
                    DataBaseWatcher.StoresWatcher.OnChanged += DataBaseWatcher.Store_Changed;
                    DataBaseWatcher.StoresWatcher.Start();
                }
                return _store;
            }
        }

        /// <summary>
        /// ProfileProperties دالة ترجع قائمة 
        /// </summary>
        //private static BindingList<DAL.UserSettingsProfileProperty> _profileProperties;
        //public static BindingList<DAL.UserSettingsProfileProperty> ProfileProperties
        //{
        //    get
        //    {
        //        if (_profileProperties == null)
        //        {
        //            using (var db = new DAL.DBDataContext())
        //            {
        //                _profileProperties = new BindingList<DAL.UserSettingsProfileProperty>(db.UserSettingsProfileProperties.ToList());
        //            }
        //        }
        //        return _profileProperties;
        //    }
        //}
        private static DAL.User _user;
        public static DAL.User User { get => _user;}
        public static void SetUser(DAL.User user)
        {
            _user = user;
            using (DAL.DBDataContext db = new DAL.DBDataContext())
            {
                _screenAccesses = (from s in Class.Screens.GetScreens
                                   from d in db.UserAccessProfileDetails
                                   .Where(x => x.ProfileID == user.ScreenProfileID && x.ScreenID == s.ScreenID).DefaultIfEmpty()
                                   select new Class.ScreensAccessProfile(s.ScreenName)
                                   {
                                       CanAdd = (d == null) ? true : d.CanAdd,
                                       CanDelete = (d == null) ? true : d.CanDelete,
                                       CanEdit = (d == null) ? true : d.CanEdit,
                                       CanOpen = (d == null) ? true : d.CanOpen,
                                       CanPrint = (d == null) ? true : d.CanPrint,
                                       CanShow = (d == null) ? true : d.CanShow,
                                       Actions = s.Actions,
                                       ScreenName = s.ScreenName,
                                       ScreenCaption = s.ScreenCaption,
                                       ScreenID = s.ScreenID,
                                       ParentScreenID = s.ParentScreenID
                                   }).ToList();
            }
        }

        /// <summary>
        /// دالة ترجع قائمة العملاء
        /// </summary>
        private static BindingList<DAL.CustomersAndVendore> _customers;
        public static BindingList<DAL.CustomersAndVendore> Customers
        {
            get
            {
                if (_customers == null)
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        _customers = new BindingList<DAL.CustomersAndVendore>(db.CustomersAndVendores.Where(x => x.IsCustomer == true).ToList());
                    }

                    DataBaseWatcher.Customers = new TableDependency.SqlClient.SqlTableDependency<DataBaseWatcher.CustomersAndVendores>(Properties.Settings.Default.SalesDBConnectionString, filter: new DataBaseWatcher.CustomersOnly(), executeUserPermissionCheck: false);
                    DataBaseWatcher.Customers.OnChanged += DataBaseWatcher.Customer_Changed;
                    DataBaseWatcher.Customers.Start();
                }
                return _customers;
            }
        }
        private static List<ScreensAccessProfile> _screenAccesses;
        public static List<ScreensAccessProfile> ScreenAccesses
        {
            get
            {
                //if (User.UserType == (byte)Master.UserType.Admin)
                //    return Screens.GetScreens;
                //else
                    return _screenAccesses;
            }
        }
    }
}
