using DevExpress.XtraEditors;
using SalesLinq.Class;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_Stores : frm_Master
    {
        DAL.Store store;

        public frm_Stores()
        {
            InitializeComponent();
            New();
        }

        public frm_Stores(int id)
        {
            InitializeComponent();
            var db = new DAL.DBDataContext();
            store = db.Stores.Where(s => s.ID == id).First();
            GetData();
        }

        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(txt_Name.Text))
            {
                txt_Name.ErrorText = "الرجاء إدخال اسم المخزن";
                return;
            }

            using (var db = new DAL.DBDataContext())
            {
                db.Connection.Open();
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Transaction = transaction;
                    try
                    {
                        var salesAccount = new DAL.Account();
                        var salesReturnAccount = new DAL.Account();
                        var inventoryAccount = new DAL.Account();
                        var costOfGoodsAccount = new DAL.Account();

                        if (store.ID == 0)
                        {
                            store.DiscountAllowedAccountID = Session.Defualts.DiscountAllowedAccountID;
                            store.DiscountReceivedAccountID = Session.Defualts.DiscountReceivedAccountID;

                            db.Accounts.InsertOnSubmit(salesAccount);
                            db.Accounts.InsertOnSubmit(salesReturnAccount);
                            db.Accounts.InsertOnSubmit(inventoryAccount);
                            db.Accounts.InsertOnSubmit(costOfGoodsAccount);
                        }
                        else
                        {
                            db.Stores.Attach(store);
                            salesAccount = db.Accounts.Single(x => x.ID == store.SalesAccountID);
                            salesReturnAccount = db.Accounts.Single(x => x.ID == store.SalesReturnAccountID);
                            inventoryAccount = db.Accounts.Single(x => x.ID == store.InventoryAccountID);
                            costOfGoodsAccount = db.Accounts.Single(x => x.ID == store.CostOfSoldGoodsAccountID);
                        }

                        SetData();

                        salesAccount.Name = store.Name + "- مبيعات";
                        salesReturnAccount.Name = store.Name + "- مردود مبيعات";
                        inventoryAccount.Name = store.Name + "- المخزون";
                        costOfGoodsAccount.Name = store.Name + "- تكلفة البضاعة المباعة";

                        // حفظ أولي للحصول على أرقام الحسابات الجديدة عند إنشاء مخزن.
                        db.SubmitChanges();

                        store.SalesAccountID = salesAccount.ID;
                        store.SalesReturnAccountID = salesReturnAccount.ID;
                        store.InventoryAccountID = inventoryAccount.ID;
                        store.CostOfSoldGoodsAccountID = costOfGoodsAccount.ID;

                        if (store.ID == 0)
                            db.Stores.InsertOnSubmit(store);

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

            PartID = store.ID;
            PartName = store.Name;
            base.Save();
        }

        public override void GetData()
        {
            txt_Name.Text = store.Name;
            PartID = store.ID;     // لتسجيل حركات المستخدم
            PartName = store.Name; // لتسجيل حركات المستخدم
        }

        public override void SetData()
        {
            store.Name = txt_Name.Text;
        }

        public override void New()
        {
            store = new DAL.Store();
            //GetData();
            base.New();

        }

        /// <summary>
        /// حذف المخزن
        /// </summary>
        public override void Delete()
        {
            if (!AskForDeletion())
                return;

            using (var db = new DAL.DBDataContext())
            {
                db.Connection.Open();
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Transaction = transaction;
                    try
                    {
                        // لا نحذف مخزناً له حركات مخزون أو قيود محاسبية مرتبطة.
                        bool hasStoreLogs = db.StoreLogs.Any(x => x.StoreID == store.ID);
                        bool hasAccountLogs = db.Journals.Any(x =>
                            x.AccountID == store.CostOfSoldGoodsAccountID ||
                            x.AccountID == store.SalesAccountID ||
                            x.AccountID == store.SalesReturnAccountID ||
                            x.AccountID == store.InventoryAccountID);

                        if (hasStoreLogs || hasAccountLogs)
                        {
                            transaction.Rollback();
                            XtraMessageBox.Show(
                                "عفواً، لا يمكن حذف المخزن لأنه مستخدم في النظام.",
                                "تعذر الحذف",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        db.Stores.Attach(store);
                        db.Stores.DeleteOnSubmit(store);
                        db.Accounts.DeleteAllOnSubmit(db.Accounts.Where(x =>
                            x.ID == store.CostOfSoldGoodsAccountID ||
                            x.ID == store.SalesAccountID ||
                            x.ID == store.SalesReturnAccountID ||
                            x.ID == store.InventoryAccountID));

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

            base.Delete();
        }

        public override bool IsDataValide()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ("الرجاء ادخال اسم المخزن");
                return false;
            }
            else
            {
                return base.IsDataValide();
            }
        }
    }
    
}
