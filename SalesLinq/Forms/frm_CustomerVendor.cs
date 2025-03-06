using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SalesLinq.Forms
{
    public partial class frm_CustomerVendor : frm_Master
    {
        bool CloseAfterSave;
        public static int AddNew(bool isCustomer)
        {
            frm_CustomerVendor frm = new frm_CustomerVendor(isCustomer);
            frm.CloseAfterSave = true;
            frm.ShowDialog();
            return frm.CusVnd.ID;
        }

        bool IsCustomer;//للتفريق بين المورد والعميل
        DAL.CustomersAndVendore CusVnd;
        /// <summary>
        /// في حالة الجديد
        /// </summary>
        /// <param name="isCustomer">ترسل صحيح للعميل وخطأ للمورد</param>
        public frm_CustomerVendor(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            this.Text = (IsCustomer) ? "عميل" : "مورد";
            this.Name = IsCustomer ? "frm_Customer" : "frm_Vendor";
            New();
        }
        /// <summary>
        /// في حالة التعديل
        /// </summary>
        /// <param name="id">ترسل رقم العميل أو المورد للتعديل</param>
        public frm_CustomerVendor(int id)
        {
            InitializeComponent();
            LoadObject(id);
            this.Text = (IsCustomer) ? "عميل" : "مورد";
            this.Name = IsCustomer ? "frm_Customer" : "frm_Vendor";
        }
        /// <summary>
        /// احضار بيانات المورد أو العميل للتعديل
        /// </summary>
        /// <param name="id">ترسل رقم العميل أو المورد</param>
        void LoadObject(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                CusVnd = db.CustomersAndVendores.Single(x => x.ID == id);
                IsCustomer = CusVnd.IsCustomer;
                GetData();
            }
        }

        public override void New()
        {
            CusVnd = new DAL.CustomersAndVendore();
            CusVnd.MaxCredit = 10000;
            base.New();
        }
        /// <summary>
        /// ارجاع البيانات للتعديل
        /// </summary>
        public override void GetData()
        {
            txt_Name.Text = CusVnd.Name;
            txt_Mobile.Text = CusVnd.Mobile;
            txt_Phone.Text = CusVnd.Phone;
            txt_Address.Text = CusVnd.Address;
            txt_AccountID.Text = CusVnd.AccountID.ToString();
            spinEdit1.EditValue = CusVnd.MaxCredit;

            PartID = CusVnd.ID; // لتسجيل حركات المستخدم
            PartName = CusVnd.Name; // لتسجيل حركات المستخدم
            base.GetData();
        }
        /// <summary>
        /// ملأ البيانات للتعديل والاضافة
        /// </summary>
        public override void SetData()
        {
            CusVnd.Name = txt_Name.Text;
            CusVnd.Mobile = txt_Mobile.Text;
            CusVnd.Phone = txt_Phone.Text;
            CusVnd.Address = txt_Address.Text;
            CusVnd.IsCustomer = IsCustomer;
            CusVnd.MaxCredit = Convert.ToDouble(spinEdit1.EditValue);
            base.SetData();
        }

        /// <summary>
        /// التحقق من كتابة الاسم وكذلك من تكراره في قاعدة البيانات 
        /// </summary>
        /// <returns> اذا كان الاسم فارغا أو موجودا من قبل يرجع خطأ وإلا يرجع صح</returns>
        public override bool IsDataValide()
         {
            // التحقق من أن الاسم مكتوب
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            else
            {
                // التحقق من أن الاسم غير مسجل في قاعدة البيانات مسبقا
                var db = new DAL.DBDataContext();
                var oldObj = db.CustomersAndVendores.Where(x => x.Name.Trim() == txt_Name.Text.Trim() && x.IsCustomer == IsCustomer && x.ID != CusVnd.ID);
                if (oldObj.Count() > 0)
                {
                    txt_Name.ErrorText = "هذا الاسم مسجل مسبقا";
                    return false;
                }
                else
                {
                    return base.IsDataValide();
                }

            }

         }
        /// <summary>
        /// حفظ المورد أو العميل
        /// </summary>
        public override void Save()
        {
            //التحق من البيانات قبل الحفظ
            //if (!IsDataValid())
            //    return;
            var db = new DAL.DBDataContext();
            DAL.Account account;
            if (CusVnd.ID == 0)
            {
                db.CustomersAndVendores.InsertOnSubmit(CusVnd);
                account = new DAL.Account();
                db.Accounts.InsertOnSubmit(account);
            }
            else
            {
                db.CustomersAndVendores.Attach(CusVnd);
                account = db.Accounts.Single(s => s.ID == CusVnd.AccountID);
            }

            SetData();
            account.Name = CusVnd.Name;
            db.SubmitChanges();
            CusVnd.AccountID = account.ID;
            db.SubmitChanges();
           
            PartID = CusVnd.ID; // لتسجيل حركات المستخدم
            PartName = CusVnd.Name; // لتسجيل حركات المستخدم DESKTOP-R4T3N57\Smail
            base.Save();
            if (CloseAfterSave)
                this.Close();
        }
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    //التأكد من أن الخزنة غير مستخدمة قبل حذفها
                    var accountLog = db.Journals.Where(x => x.AccountID == CusVnd.AccountID).Count();

                    if (accountLog > 0)
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف هذا العميل / المورد حيث أنه مرتبط بعدة فواتير"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        return;
                    }

                    db.CustomersAndVendores.Attach(CusVnd);
                    db.CustomersAndVendores.DeleteOnSubmit(CusVnd);
                    db.SubmitChanges();
                    db.Accounts.DeleteAllOnSubmit(db.Accounts.Where(x => x.ID == CusVnd.AccountID));
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }
    }
}