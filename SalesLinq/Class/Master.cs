using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using SalesLinq.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesLinq.Class
{
    public static class Master
    {
        /// <summary>
        /// انشاء قاعدة بيانات وهمية
        /// </summary>
        public class ValueAndID
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public static List<ValueAndID> ProductTypesList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(byte)ProductType.Inventory,Name="مخزني"},
                new ValueAndID() { ID=(byte)ProductType.Service,Name="خدمي"}
            };

        public enum ProductType
        {
            Inventory,
            Service
        }

        public static List<ValueAndID> InvoiceTypeList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)InvoiceType.Purchase,Name="مشتريات"},
                new ValueAndID() { ID=(int)InvoiceType.Sales,Name="مبيعات"},
                new ValueAndID() { ID=(int)InvoiceType.PurchaseReturn,Name="مردود مشتريات"},
                new ValueAndID() { ID=(int)InvoiceType.SalesReturn,Name="مردود مبيعات"}
            };

        public enum InvoiceType
        {
            Purchase = SourceType.Purchase,
            Sales = SourceType.Sales,
            PurchaseReturn = SourceType.PurchaseReturn,
            SalesReturn = SourceType.SalesReturn

        }

        public enum SourceType
        {
            Purchase,
            Sales,
            PurchaseReturn,
            SalesReturn,
        }

        public enum CostDistributionOptions
        {
            ByPrice,
            ByQty,
        }

        public enum PrintMode
        {
            Direct,
            ShowPreview,
            ShowDialog
        }



        public static List<ValueAndID> InvoicesPartTypesList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)PartType.Vendor,Name="مورد"},
                new ValueAndID() { ID=(int)PartType.Customer,Name="عميل"}
            };
        public static List<ValueAndID> PartTypesList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)PartType.Vendor,Name="مورد"},
                new ValueAndID() { ID=(int)PartType.Customer,Name="عميل"},
                new ValueAndID() { ID=(int)PartType.Account,Name="حساب"},

            };
        public enum PartType
        {
            Vendor,
            Customer,
            Account
        }

        public static List<ValueAndID> PayMethodsList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)PayMethods.Cash,Name="نقدي"},
                new ValueAndID() { ID=(int)PayMethods.Credit,Name="آجل"}
            };
        public enum PayMethods
        {
            Cash=1,
            Credit
        }

        public static List<ValueAndID> UserTypeList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)UserType.Admin , Name ="مدير نظام"},
                new ValueAndID() { ID=(int)UserType.User , Name ="دخول مخصص"}
            };
        public enum UserType
        {
            Admin = 1,
            User
        }

        public static List<ValueAndID> WorningLevelsList = new List<ValueAndID>()
            {
                new ValueAndID() { ID=(int)WorningLevels.DoNotEnteript,Name="عدم التداخل"},
                new ValueAndID() { ID=(int)WorningLevels.ShowWarning,Name="تحذير"},
                new ValueAndID() { ID=(int)WorningLevels.Prevent,Name="منع"}
            };
        public enum WorningLevels
        {
            DoNotEnteript = 1,
            ShowWarning,
            Prevent
        }

        public enum Actions
        {
            Show = 1,
            Open,
            Add,
            Edit,
            Delete,
            Print
        }

        public static int FindRowHandelByRowObject(this GridView view , object row)
        {
            if (row != null)
            {
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    if (row.Equals(view.GetRow(i)))
                        return i;
                }
            }
            return DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }


        /// <summary>
        /// التحقق من أن النص غير فارغ 
        /// </summary>
        /// <param name="txt">TextEdit تستقبل </param>
        /// <returns>يظهر الخطأ ويرجع صحيح اذا كانت غير فارغة ويرجع خطأ اذا كانت فارغة</returns>
        public static bool IsTextValide(this TextEdit txt)
        {
            if (txt.Text.Trim() == string.Empty)
            {
                txt.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        /// <summary>
        /// التحقق من أنه رقمي
        /// </summary>
        /// <param name="lkp">LookUpEditBase تستقبل </param>
        /// <returns>يرجع صحيح اذا كانت القيمة رقمية وخطأ اذا كان غير ذلك ويظهر الخطأ</returns>
        public static bool IsEditValueValide(this LookUpEditBase lkp, bool SetErrur = true)
        {
            if (lkp.IsEditValueOfTypeInt() == false)
            {
                if (SetErrur)
                    lkp.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        /// <summary>
        /// التحقق من أن القيمة ليست أصغر من الصفر
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="SetError"></param>
        /// <returns>ويرجع صحيح اذا كانت القيمة ليست أصغر من الصفر ويرجع خطأ اذا كانت غير ذلك ويظهر الخطأ</returns>
        public static bool IsValueNotLessThanZero(this SpinEdit edit, bool SetError = true)
        {
            if (SetError && edit.Value < 0)
                edit.ErrorText = "لا يمكن للقيمة أن تكون اصغر من الصفر";
            return edit.Value >= 0;
        }
        /// <summary>
        /// التحقق من أن القيمة اكبر من الصفر
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="SetError"></param>
        /// <returns>ويرجع صحيح اذا كانت القيمة أكبر من الصفر ويرجع خطأ اذا كانت غير ذلك ويظهر الخطأ</returns>
        public static bool IsValueIsBiggerThanZero(this SpinEdit edit, bool SetError = true)
        {
            if (SetError && edit.Value <= 0)
                edit.ErrorText = "يجب أن تكون القيمة أكبر من الصفر";
            return edit.Value > 0;
        }
        /// <summary>
        /// التحقق من انه ليس صفر
        /// </summary>
        /// <param name="lkp">LookUpEditBase يستقبل </param>
        /// <returns>يرجع صحيح اذا كانت رقمية و ليست صفر ويرجع خطأ اذا كانت غير ذلك ويظهر الخطأ</returns>
        public static bool IsEditValueValideAndNotZero(this LookUpEditBase lkp , bool SetErrur = true)
        {
            if (lkp.IsEditValueOfTypeInt() == false || Convert.ToInt32(lkp.EditValue) == 0)
            {
                if (SetErrur)
                    lkp.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        /// <summary>
        /// التحقق من أنه تاريخ
        /// </summary>
        /// <param name="dt">DateEdit يستقبل </param>
        /// <returns>يظهر الخطأ ويرجع صحيح اذا كانت صيغة تاريخ ويرجع خطأ اذا كانت غير ذلك</returns>
        public static bool IsDateValide(this DateEdit dt)
        {
            if (dt.DateTime.Year < 1950) 
            {
                dt.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        /// <summary>
        /// التحقق من أنه رقمي
        /// </summary>
        /// <param name="edit">LookUpEditBase تستقبل </param>
        /// <returns>يرجع صحيح اذا كان رقمي وخطأ اذا كان غير ذلك</returns>
        public static bool IsEditValueOfTypeInt(this LookUpEditBase edit)
        {
            var val = edit.EditValue;
            bool test =(val is int || val is byte);
            return test;
        }
        // انشاء ريبوزيتوري ايتم مع اعطائه بعض القيم الافتراضية مسبقا
        public static void InitializeData(this RepositoryItemLookUpEditBase repo, object datasource,GridColumn column,GridControl grid)
        {
            InitializeData(repo, datasource, column, grid, "Name", "ID");
        }
        /// <summary>
        /// انشاء ريبوزيتوري ايتم افتراضي ونسخه عند الحاجة
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="datasource"></param>
        /// <param name="column"></param>
        /// <param name="grid"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        public static void InitializeData(this RepositoryItemLookUpEditBase repo, object datasource, GridColumn column, GridControl grid, string displayMember, string valueMember)
        {
            if (repo == null)
                repo = new RepositoryItemLookUpEdit();

            repo.DataSource = datasource;
            repo.DisplayMember = displayMember;
            repo.ValueMember = valueMember;
            repo.NullText = "";
            repo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            if (column != null)
                column.ColumnEdit = repo;
            if (grid != null)
                grid.RepositoryItems.Add(repo);
        }

        public static void InitializeData(this LookUpEdit lkp, object datasource)
        {
            lkp.InitializeData(datasource, "Name", "ID");// Extention Method /* تصبح الدالة وكأنها اضافة تظهر بعد النقطة بشكل عادي 28 */
        }

        public static void InitializeData(this LookUpEdit lkp, object datasource, string displayMember, string valueMember)
        {
            lkp.Properties.DataSource = datasource;
            lkp.Properties.DisplayMember = displayMember;
            lkp.Properties.ValueMember = valueMember;
            lkp.Properties.Columns.Clear();
            lkp.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo()
            {
                FieldName = displayMember,
            });
            lkp.Properties.ShowHeader = false;
            //lkp.Properties.PopulateColumns(datasource);
            //lkp.Properties.Columns[valueMember].Visible = false;
            lkp.Properties.NullText = "";
        }

        public static void InitializeData(this GridLookUpEdit lkp, object datasource)
        {
            lkp.InitializeData(datasource, "Name", "ID");// Extention Method /* تصبح الدالة وكأنها اضافة تظهر بعد النقطة بشكل عادي 28 */
        }

        public static void InitializeData(this GridLookUpEdit lkp, object datasource, string displayMember, string valueMember)
        {
            lkp.Properties.DataSource = datasource;
            lkp.Properties.DisplayMember = displayMember;
            lkp.Properties.ValueMember = valueMember;
            lkp.Properties.NullText = "";

            lkp.Properties.ValidateOnEnterKey = true;
            lkp.Properties.AllowNullInput = DefaultBoolean.False;
            lkp.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;// لعرض الأصناف بقائمة كاملة
            lkp.Properties.ImmediatePopup = true;//السماح بالبحث في الأصناف الاكمال التلقائي
            var PartIDView = lkp.Properties.View;// الجدول الذي يعرض الأصناف
            PartIDView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;//اختيار الصف كاملا
            PartIDView.OptionsSelection.UseIndicatorForSelection = true;// اختيار السطر من الانديكايتور في الجانب
            PartIDView.OptionsView.ShowAutoFilterRow = true;//اظهار خانة البحث أوتوماتيكيا
            PartIDView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            PartIDView.PopulateColumns(lkp.Properties.DataSource); // 

        }
        /// <summary>
        /// دالة لتوليد كود جديد
        /// </summary>
        /// <param name="Number">تستقبل الكود السابق</param>
        /// <returns>ترجع كود جديد من نفس التنسيق للكود السابق</returns>
        public static string GetNextNumberInString(string Number)
        {
            if (Number == string.Empty || Number == null)
                return "1";
            string str1 = "";
            foreach (char c in Number)
                str1 = char.IsDigit(c) ? str1 + c.ToString() : "";
            if (str1 == string.Empty)
                return Number + "1";
            string str2 = str1.Insert(0, "1");
            str2 = (Convert.ToInt64(str2) + 1).ToString();
            string str3 = str2[0] == '1' ? str2.Remove(0, 1) : str2.Remove(0, 1).Insert(0, "1");

            int indx = Number.LastIndexOf(str1);
            Number = Number.Remove(indx);
            Number = Number.Insert(indx, str3);
            return Number;
        }
        /// <summary>
        /// دالة تحول أي يبانات بنظام ثنائي الى النظام العادي
        /// </summary>
        /// <param name="data"></param>
        /// <returns>تستقبل رقم بنظام ثنائي وترجعه بنظام عشري</returns>
        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                return (T)formatter.Deserialize(stream);
            };
        }
        /// <summary>
        /// دالة تحول أي يبانات بالنظام العادي الى النظام الثنائي 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>تستقبل رقم بنظام عشري وترجعه بنظام ثنائي</returns>
        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream,obj);
                return stream.ToArray();
            };
        }

        /// <summary>
        /// دالة تستقبل اسم الخاصية ورقم الأيدي وترجع قيمة الخاصية 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="profileID"></param>
        /// <returns>ترجع قيمة الخاصية</returns>
        public static byte[] GetPropertyValue(string propertyName,int profileID)
        {
            using (var db = new DAL.DBDataContext())
            {
                var prop = db.UserSettingsProfileProperties.SingleOrDefault(x => x.ProfileID == profileID &&
                x.PropertyName == propertyName);
                if (prop == null)
                    return null;
                return prop.PropertyValue.ToArray();
            }
        }
        /// <summary>
        /// دالة ترجع اسم الدالة التي التي تحتويها(الدالة الأب )ا
        /// </summary>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public static string GetCallerName([CallerMemberName]string callerName = "")
        {
            return callerName;
        }

    }
}
