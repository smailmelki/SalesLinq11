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
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ("الرجاء ادخال اسم الفرع");
                return;
            }
            // انشاء حسابات المخزن
            DAL.Account SalesAccount = new DAL.Account();
            DAL.Account SalesReturnAccount = new DAL.Account();
            DAL.Account InventoryAccount = new DAL.Account();
            DAL.Account CostOfGoodsAccount = new DAL.Account();


            var db = new DAL.DBDataContext();

            if (store.ID == 0)// ID=0 في حالة الضغط على جديد يكون
            {
                // اضافة الحسابات للمخزن في حالة كانت اضافة
                db.Stores.InsertOnSubmit(store);
                db.Accounts.InsertOnSubmit(SalesAccount);
                db.Accounts.InsertOnSubmit(SalesReturnAccount);
                db.Accounts.InsertOnSubmit(InventoryAccount);
                db.Accounts.InsertOnSubmit(CostOfGoodsAccount);

                store.DiscountAllowedAccountID = Session.Defualts.DiscountAllowedAccountID;
                store.DiscountReceivedAccountID = Session.Defualts.DiscountReceivedAccountID;
            }
            else
            {
                // جلب الحسابات من المخزن في حالة التعديل
                db.Stores.Attach(store);
                SalesAccount = db.Accounts.Single(x => x.ID == store.SalesAccountID);
                SalesReturnAccount = db.Accounts.Single(x => x.ID == store.SalesReturnAccountID);
                InventoryAccount = db.Accounts.Single(x => x.ID == store.InventoryAccountID);
                CostOfGoodsAccount = db.Accounts.Single(x => x.ID == store.CostOfSoldGoodsAccountID);
            }

            SetData();
            // تعديل أسماء الحسابات
            SalesAccount.Name = store.Name + "- مبيعات";
            SalesReturnAccount.Name = store.Name + "- مردود مبيعات";
            InventoryAccount.Name = store.Name + "- المخزون";
            CostOfGoodsAccount.Name = store.Name + "- تكلفة البضاعة المباعة";
            db.SubmitChanges();// اعتماد التغييرات

            store.SalesAccountID = SalesAccount.ID;
            store.SalesReturnAccountID = SalesReturnAccount.ID;
            store.InventoryAccountID = InventoryAccount.ID;
            store.CostOfSoldGoodsAccountID = CostOfGoodsAccount.ID;

            db.SubmitChanges();// اعتماد التغييرات
            PartID = store.ID;     // لتسجيل حركات المستخدم
            PartName = store.Name; // لتسجيل حركات المستخدم
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
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {

                    //التأكد من أن المخزن غير مستخدم قبل حذفه
                    var log = db.StoreLogs.Where(x => x.StoreID == store.ID).Count();
                    var accountLog = db.Journals.Where(x => x.AccountID == store.CostOfSoldGoodsAccountID ||
                                                        x.AccountID == store.SalesAccountID ||
                                                        x.AccountID == store.SalesReturnAccountID ||
                                                        x.AccountID == store.InventoryAccountID
                                                       ).Count();

                    if (log + accountLog > 0)
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف المخزن حيث أنه تم استخدامه في النظام"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        return;
                    }

                    db.Stores.Attach(store);
                    db.Stores.DeleteOnSubmit(store);
                    db.Accounts.DeleteAllOnSubmit(db.Accounts.Where(x =>
                                                                    x.ID == store.CostOfSoldGoodsAccountID ||
                                                                    x.ID == store.SalesAccountID ||
                                                                    x.ID == store.SalesReturnAccountID ||
                                                                    x.ID == store.InventoryAccountID
                                                                   ));
                    db.SubmitChanges();
                }
                base.Delete();
            }
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
