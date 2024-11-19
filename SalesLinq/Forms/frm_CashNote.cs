using SalesLinq.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SalesLinq.Class.MasterFinance;

namespace SalesLinq.Forms
{
    public partial class frm_CashNote : frm_Master
    {
        DAL.CashNote Note=null;
        bool IsCashIN;
        public frm_CashNote(bool _IsCashIN)
        {
            InitializeComponent();
            IsCashIN = _IsCashIN;
            lkp_PartType.EditValueChanged += Lkp_PartType_EditValueChanged;// حدث تغيير نوع طرف التعامل
            glkp_PartID.EditValueChanged += Glkp_PartID_EditValueChanged;//حدث تغيير طرف التعامل
            New();
        }
        public override void RefreshData()
        {
            lkp_PartType.InitializeData(Class.Master.PartTypesList);
            lkp_Branch.InitializeData(Session.Store);
            lkp_Drawer.InitializeData(Session.Drawer);
            base.RefreshData();
        }

        private void Frm_CashNote_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

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
                else if (partType == (int)Master.PartType.Account)
                {
                    glkp_PartID.InitializeData(Session.Accounts);
                    glkp_PartID.EditValue = null;
                }

            }
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
                DAL.CustomersAndVendore account=null;

                if (Convert.ToByte(lkp_PartType.EditValue) == (byte)Master.PartType.Vendor)//int or byte
                {
                    account = Session.vendores.SingleOrDefault(x => x.ID == id);
                }
                else if (Convert.ToByte(lkp_PartType.EditValue) == (byte)Master.PartType.Customer)
                {
                    account = Session.Customers.SingleOrDefault(x => x.ID == id);
                }
                else if (Convert.ToByte(lkp_PartType.EditValue) == (byte)Master.PartType.Account)
                {
                    account = new DAL.CustomersAndVendore() { AccountID = id };
                }
                if (account == null)
                    goto IFEmpty;
                txt_PartAddress.Text = account.Address;
                txt_PartMaxCredit.EditValue = account.MaxCredit;
                txt_PartPhone.Text = account.Phone;
                accountBalance = MasterFinance.GetAccountBalance(account.AccountID);
                txt_PartBalance.Text = accountBalance.Balance;
               
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
            txt_PartAddress.Text = "";
        }

        public override void New()
        {
            Note = new DAL.CashNote() { Date = DateTime.Now,
                Drawer = Session.Defualts.Drawer,
                StoreID = Session.Defualts.Store,
                Code = GetNewCashCode()
            };
            Note.IsCashIn = IsCashIN;
            base.New();
        }

        /// <summary>
        /// دالة لتوليد كود للفاتورة
        /// </summary>
        /// <returns></returns>
        private string GetNewCashCode()
        {
            string maxCode;
            using (var db = new DAL.DBDataContext())
            {
                maxCode = db.CashNotes.Where(x => x.IsCashIn == IsCashIN).OrderByDescending(x => x.Date).Select(x => x.Code ?? "0").FirstOrDefault();
            }

            return Master.GetNextNumberInString(maxCode);
        }

        public override void GetData()
        {
            spn_Amount.EditValue = Note.Amount;
            txt_Code.Text = Note.Code;
            dt_Date.DateTime = Note.Date;
            spn_Discount.EditValue = Note.DiscountValue;
            lkp_Drawer.EditValue = Note.Drawer;
            txt_ID.Text = Note.ID.ToString();
            me_Notes.SelectedText = Note.Notes;
            lkp_PartType.EditValue = Note.PartID;
            glkp_PartID.EditValue = Note.PartType;
            lkp_Branch.EditValue = Note.StoreID;
            base.GetData();
        }
        public override void SetData()
        {
            Note.Amount = Convert.ToDouble(spn_Amount.EditValue);
            Note.Code = txt_Code.Text;
            Note.Date = dt_Date.DateTime;
            Note.DiscountValue = Convert.ToDouble(spn_Discount.EditValue);
            Note.Drawer = Convert.ToInt32(lkp_Drawer.EditValue);
            Note.Notes = me_Notes.SelectedText;
            Note.PartID = Convert.ToInt32(lkp_PartType.EditValue);
            Note.PartType = Convert.ToByte(glkp_PartID.EditValue);
            Note.StoreID = Convert.ToInt32(lkp_Branch.EditValue);          
            base.SetData();
        }

        bool IsCodeExists()
        {
            using (var db = new DAL.DBDataContext())
            {
                var data = db.CashNotes.Where(x => x.ID != Note.ID && x.IsCashIn == IsCashIN && x.Code == txt_Code.Text).Count();
                if (data > 0)
                    txt_Code.ErrorText = "هذا الكود متكرر";
                return data > 0;

            }
        }

        public override bool IsDataValide()
        {
            int NumberOfErrors = 0;

            NumberOfErrors += IsCodeExists() ? 1 : 0;// التحقق من أن الكود غير مكرر 
            NumberOfErrors += txt_Code.IsTextValide() ? 0 : 1;// التحقق من أن النص غير فارغ 
            NumberOfErrors += lkp_PartType.IsEditValueValide() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += lkp_Drawer.IsEditValueValide() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += lkp_Branch.IsEditValueValide() ? 0 : 1;// التحقق من أن النص رقمي
            NumberOfErrors += glkp_PartID.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص ليس صفر
            NumberOfErrors += lkp_Branch.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص ليس صفر
            NumberOfErrors += (dt_Date.IsDateValide()) ? 0 : 1;// التحقق من أن النص بصيغة تاريخ
            NumberOfErrors += (spn_Amount.IsValueIsBiggerThanZero()) ? 0 : 1;// التحقق من أن قيمة المدفوع اكبر من الصفر

            return (NumberOfErrors == 0);
        }

        public override void Save()
        {
            //base.Save();
        }
    }
}
