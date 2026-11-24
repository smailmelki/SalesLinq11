using System;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    /// <summary>
    /// شاشة موحدة لإدارة بيانات العملاء والموردين.
    /// </summary>
    public partial class frm_CustomerVendor : NativeMasterForm
    {
        private bool CloseAfterSave;
        private bool IsCustomer;
        private DAL.CustomersAndVendore CusVnd;

        public static int AddNew(bool isCustomer)
        {
            using (var form = new frm_CustomerVendor(isCustomer))
            {
                form.CloseAfterSave = true;
                form.ShowDialog();
                return form.CusVnd == null ? 0 : form.CusVnd.ID;
            }
        }

        public frm_CustomerVendor(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            Name = IsCustomer ? "frm_Customer" : "frm_Vendor";
            Text = IsCustomer ? "بيانات عميل" : "بيانات مورد";
            New();
        }

        public frm_CustomerVendor(int id)
        {
            InitializeComponent();
            LoadObject(id);
            Name = IsCustomer ? "frm_Customer" : "frm_Vendor";
            Text = IsCustomer ? "بيانات عميل" : "بيانات مورد";
        }

        private void LoadObject(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                CusVnd = db.CustomersAndVendores.SingleOrDefault(x => x.ID == id);
                if (CusVnd == null)
                    throw new InvalidOperationException("العميل أو المورد المطلوب غير موجود.");

                // الاحتفاظ بالسجل كنسخة منفصلة عن سياق قاعدة البيانات.
                IsCustomer = CusVnd.IsCustomer;
            }

            GetData();
        }

        public override void New()
        {
            CusVnd = new DAL.CustomersAndVendore
            {
                MaxCredit = 10000,
                IsCustomer = IsCustomer
            };

            base.New();
            Text = IsCustomer ? "إضافة عميل جديد" : "إضافة مورد جديد";
        }

        public override void GetData()
        {
            if (CusVnd == null)
                return;

            txt_Name.Text = CusVnd.Name ?? string.Empty;
            txt_Mobile.Text = CusVnd.Mobile ?? string.Empty;
            txt_Phone.Text = CusVnd.Phone ?? string.Empty;
            txt_Address.Text = CusVnd.Address ?? string.Empty;
            txt_AccountID.Text = CusVnd.AccountID == 0
                ? string.Empty
                : CusVnd.AccountID.ToString();
            num_MaxCredit.Value = ToDecimal(CusVnd.MaxCredit);

            PartID = CusVnd.ID;
            PartName = CusVnd.Name ?? string.Empty;
            btn_Delete.Enabled = CusVnd.ID > 0;
            base.GetData();
        }

        public override void SetData()
        {
            CusVnd.Name = txt_Name.Text.Trim();
            CusVnd.Mobile = NullIfEmpty(txt_Mobile.Text);
            CusVnd.Phone = NullIfEmpty(txt_Phone.Text);
            CusVnd.Address = NullIfEmpty(txt_Address.Text);
            CusVnd.IsCustomer = IsCustomer;
            CusVnd.MaxCredit = Convert.ToDouble(num_MaxCredit.Value);
            base.SetData();
        }

        private static decimal ToDecimal(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                return 0;

            decimal result;
            if (!decimal.TryParse(value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out result))
                return 0;

            return Math.Min(result, 999999999999999999M);
        }

        private static string NullIfEmpty(string value)
        {
            value = (value ?? string.Empty).Trim();
            return value.Length == 0 ? null : value;
        }

        public override bool IsDataValide()
        {
            if (string.IsNullOrWhiteSpace(txt_Name.Text))
            {
                MessageBox.Show("يرجى إدخال اسم " + (IsCustomer ? "العميل." : "المورد."),
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Name.Focus();
                return false;
            }

            if (num_MaxCredit.Value < 0)
            {
                MessageBox.Show("حد الائتمان لا يمكن أن يكون سالباً.",
                    "بيانات غير صحيحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                num_MaxCredit.Focus();
                return false;
            }

            using (var db = new DAL.DBDataContext())
            {
                bool nameExists = db.CustomersAndVendores.Any(x =>
                    x.ID != CusVnd.ID &&
                    x.IsCustomer == IsCustomer &&
                    x.Name.Trim() == txt_Name.Text.Trim());

                if (nameExists)
                {
                    MessageBox.Show("هذا الاسم مسجل مسبقاً ضمن قائمة " +
                        (IsCustomer ? "العملاء." : "الموردين."),
                        "اسم مكرر", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Name.Focus();
                    return false;
                }
            }

            return base.IsDataValide();
        }

        public override void Save()
        {
            int originalId = CusVnd.ID;

            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            DAL.CustomersAndVendore savedParty;
                            DAL.Account account;

                            if (originalId == 0)
                            {
                                account = new DAL.Account();
                                db.Accounts.InsertOnSubmit(account);
                                db.SubmitChanges();

                                savedParty = CusVnd;
                                savedParty.AccountID = account.ID;
                                SetData();
                                db.CustomersAndVendores.InsertOnSubmit(savedParty);
                            }
                            else
                            {
                                savedParty = db.CustomersAndVendores
                                    .Single(x => x.ID == originalId);

                                account = db.Accounts.Single(x => x.ID == savedParty.AccountID);
                                CusVnd = savedParty;
                                SetData();
                            }

                            account.Name = savedParty.Name;
                            db.SubmitChanges();
                            transaction.Commit();
                            CusVnd = savedParty;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (originalId == 0 && CusVnd != null)
                    CusVnd.ID = 0;

                MessageBox.Show("تعذر حفظ بيانات " + (IsCustomer ? "العميل" : "المورد") +
                    " وحسابه المحاسبي.\n" + ex.Message,
                    "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PartID = CusVnd.ID;
            PartName = CusVnd.Name;
            txt_AccountID.Text = CusVnd.AccountID.ToString();
            base.Save();

            if (CloseAfterSave)
                Close();
            else
                Text = (IsCustomer ? "بيانات عميل: " : "بيانات مورد: ") + CusVnd.Name;
        }

        public override void Delete()
        {
            if (CusVnd == null || CusVnd.ID == 0 || !AskForDeletion())
                return;

            int deletedId = CusVnd.ID;
            string deletedName = CusVnd.Name;
            int accountId = CusVnd.AccountID;

            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            bool hasJournalEntries = db.Journals.Any(x => x.AccountID == accountId);
                            if (hasJournalEntries)
                            {
                                transaction.Rollback();
                                MessageBox.Show(
                                    "لا يمكن حذف هذا " + (IsCustomer ? "العميل" : "المورد") +
                                    " لأنه مرتبط بفواتير أو قيود محاسبية. يمكنك الاحتفاظ به دون استخدام.",
                                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var trackedParty = db.CustomersAndVendores
                                .Single(x => x.ID == deletedId);
                            db.CustomersAndVendores.DeleteOnSubmit(trackedParty);
                            db.SubmitChanges();

                            var trackedAccount = db.Accounts.Single(x => x.ID == accountId);
                            db.Accounts.DeleteOnSubmit(trackedAccount);
                            db.SubmitChanges();

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                PartID = deletedId;
                PartName = deletedName;
                base.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حذف السجل: " + ex.Message,
                    "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}