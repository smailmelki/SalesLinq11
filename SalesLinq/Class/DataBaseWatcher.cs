using SalesLinq.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Abstracts;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;


namespace SalesLinq.Class
{
    public static class DataBaseWatcher
    {
        public static SqlTableDependency<DAL.Product> Products;
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول الأصناف مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Products_Changed(object sender, RecordChangedEventArgs<DAL.Product> e)
        {            
           
            frm_Main.Instance.Invoke(new Action(() => // طريقة لفتح المسار الرئيسي المشغل للفورم الرئيسي واعطائه التفويض لتشغيل الدالة
            {
                if (e.ChangeType == ChangeType.Insert) // في حالة الاضافة
                {
                    Session.Products.Add(e.Entity);
                    Session.ProductView.Add(Session.ProductViewClass.GetProduct(e.Entity.ID));
                }
                else if (e.ChangeType == ChangeType.Update) // في حالة التعديل
                {
                    var index = Session.Products.IndexOf(Session.Products.Single(x => x.ID == e.Entity.ID));
                    Session.Products.Remove(Session.Products.Single(x => x.ID == e.Entity.ID));
                    Session.Products.Insert(index,e.Entity);

                    var viewindex = Session.ProductView.IndexOf(Session.ProductView.Single(x => x.ID == e.Entity.ID));
                    Session.ProductView.Remove(Session.ProductView.Single(x => x.ID == e.Entity.ID));
                    Session.ProductView.Insert(viewindex,Session.ProductViewClass.GetProduct(e.Entity.ID));

                }
                else if (e.ChangeType == ChangeType.Delete)  // في حالة الحذف
                {
                    Session.Products.Remove(Session.Products.Single(x => x.ID == e.Entity.ID));
                    Session.ProductView.Remove(Session.ProductView.Single(x => x.ID == e.Entity.ID));

                }
            }));
        }

        public static SqlTableDependency<DAL.StoreLog> StoreLog;
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول حركة المخازن مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void StoreLog_Changed(object sender, RecordChangedEventArgs<DAL.StoreLog> e)
        {

            frm_Main.Instance.Invoke(new Action(() => // طريقة لفتح المسار الرئيسي المشغل للفورم الرئيسي واعطائه التفويض لتشغيل الدالة
            {
                var balance = MasterInventory.GetProductBalanceInStore(e.Entity.ProductID, e.Entity.StoreID);

                    Session.ProductsBalances.Remove(Session.ProductsBalances.FirstOrDefault(x => x.ProductID == e.Entity.ProductID &&
                    x.StoreID==e.Entity.StoreID));
                    Session.ProductsBalances.Add(new Session.ProductBalance()
                    {
                        Balance= balance,
                        ProductID = e.Entity.ProductID,
                        StoreID=e.Entity.StoreID
                    });

 
            }));
        }

        public class Stores : DAL.Store { }// انشاء كلاس يرث من الكلاس الأساسي من أجل أن يكون اسمه يطابق الاسم في قاعدة البيانات واستخدامه بدل من الكلاس الأساسي
        public static SqlTableDependency<Stores> StoresWatcher;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول المخازن مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Store_Changed(object sender, RecordChangedEventArgs<Stores> e)
        {
            switch (e.ChangeType)
            {
                case ChangeType.None:
                    break;
                case ChangeType.Delete:
                    Session.Store.Remove(Session.Store.SingleOrDefault(x => x.ID == e.Entity.ID));
                    break;
                case ChangeType.Insert:
                    Session.Store.Add(e.Entity);
                    break;
                case ChangeType.Update:
                    var index = Session.Store.IndexOf(Session.Store.SingleOrDefault(x => x.ID == e.Entity.ID));
                    Session.Store[index] = e.Entity;
                    break;
            }
        }

        public class Drawers : DAL.Drawer { }// انشاء كلاس يرث من الكلاس الأساسي من أجل أن يكون اسمه يطابق الاسم في قاعدة البيانات واستخدامه بدل من الكلاس الأساسي
        public static SqlTableDependency<Drawers> DrawersWatcher;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول الخزنات مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Drawer_Changed(object sender, RecordChangedEventArgs<Drawers> e)
        {
            switch (e.ChangeType)
            {
                case ChangeType.None:
                    break;
                case ChangeType.Delete:
                    Session.Drawer.Remove(Session.Drawer.SingleOrDefault(x => x.ID == e.Entity.ID));
                    break;
                case ChangeType.Insert:
                    Session.Drawer.Add(e.Entity);
                    break;
                case ChangeType.Update:
                    var index = Session.Drawer.IndexOf(Session.Drawer.SingleOrDefault(x => x.ID == e.Entity.ID));
                    Session.Drawer[index] = e.Entity;
                    break;
            }
        }

        public class ProductUnits : DAL.ProductUnit { }// انشاء كلاس يرث من الكلاس الأساسي من أجل أن يكون اسمه يطابق الاسم في قاعدة البيانات واستخدامه بدل من الكلاس الأساسي
        public static SqlTableDependency<ProductUnits> ProductUnitsWatcher;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول الوحدات مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProductUnits_Changed(object sender, RecordChangedEventArgs<ProductUnits> e)
        {
            frm_Main.Instance.Invoke(new Action(() => // طريقة لفتح المسار الرئيسي المشغل للفورم الرئيسي واعطائه التفويض لتشغيل الدالة
            {
                var viewIndex = Session.ProductView.IndexOf(Session.ProductView.Single(x => x.ID == e.Entity.ProductID));
                Session.ProductView[viewIndex] = Session.ProductViewClass.GetProduct(e.Entity.ProductID);
            }));
        }


        public class Accounts : DAL.Account { }// انشاء كلاس يرث من الكلاس الأساسي من أجل أن يكون اسمه يطابق الاسم في قاعدة البيانات واستخدامه بدل من الكلاس الأساسي 
        public static SqlTableDependency<Accounts> AccountsList;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول الحسابات مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Accounts_Changed(object sender, RecordChangedEventArgs<Accounts> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.None:
                        break;
                    case ChangeType.Delete:
                        Session.Accounts.Remove(Session.Accounts.Single(x => x.ID == e.Entity.ID));
                        break;
                    case ChangeType.Insert:
                        Session.Accounts.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        int index = Session.Accounts.IndexOf(Session.Accounts.Single(x => x.ID == e.Entity.ID));
                        Session.Accounts.Remove(Session.Accounts.Single(x => x.ID == e.Entity.ID));
                        Session.Accounts.Insert(index, e.Entity);
                        break;
                    default:
                        break;
                }
            }));
        }


        public class CustomersAndVendores : DAL.CustomersAndVendore { }// انشاء كلاس يرث من الكلاس الأساسي من أجل أن يكون اسمه يطابق الاسم في قاعدة البيانات واستخدامه بدل من الكلاس الأساسي 
        public static SqlTableDependency<CustomersAndVendores> Vendors;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول الموردين مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Vendors_Changed(object sender, RecordChangedEventArgs<CustomersAndVendores> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.None:
                        break;
                    case ChangeType.Delete:
                        Session.vendores.Remove(Session.vendores.Single(x => x.ID == e.Entity.ID));
                        break;
                    case ChangeType.Insert:
                        Session.vendores.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        int index = Session.vendores.IndexOf(Session.vendores.Single(x => x.ID == e.Entity.ID));
                        Session.vendores.Remove(Session.vendores.Single(x => x.ID == e.Entity.ID));
                        Session.vendores.Insert(index,e.Entity);
                        break;
                    default:
                        break;
                }
            }));
        }
        //لعمل الحدث عند تغيير عميل فقط وليس مورد TableDependency فيلتر خاص بـ 
        public class VendorsOnly : ITableDependencyFilter
        {
            public string Translate()
            {
                return "[IsCustomer]= 0";
            }
        }

        ////////////////
        public static SqlTableDependency<CustomersAndVendores> Customers;// لمراقبة قاعدة البيانات
        /// <summary>
        /// دالة لاظهار التغيير الحاصل في جدول العملاء مباشرة بدون عمل تحديث للفورم
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Customer_Changed(object sender, RecordChangedEventArgs<CustomersAndVendores> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.None:
                        break;
                    case ChangeType.Delete:
                        Session.Customers.Remove(Session.Customers.Single(x => x.ID == e.Entity.ID));
                        break;
                    case ChangeType.Insert:
                        Session.Customers.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        int index = Session.Customers.IndexOf(Session.Customers.Single(x => x.ID == e.Entity.ID));
                        Session.Customers.Remove(Session.Customers.Single(x => x.ID == e.Entity.ID));
                        Session.Customers.Insert(index,e.Entity);
                        break;
                    default:
                        break;
                }
            }));
        }
        //لعمل الحدث عند تغيير مورد فقط وليس عميل TableDependency فيلتر خاص بـ 
        public class CustomersOnly : ITableDependencyFilter
        {
            public string Translate()
            {
                return "[IsCustomer]= 1";
            }
        }

    }
}
