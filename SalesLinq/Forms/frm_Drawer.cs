using DevExpress.XtraEditors;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_Drawer : frm_Master
    {
        DAL.Drawer drawer;
        public frm_Drawer()
        {
            InitializeComponent();
            New();
        }
        public frm_Drawer(int id)
        {
            InitializeComponent();
            LoadDrawer(id);
        }
        void LoadDrawer(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                drawer = db.Drawers.Single(x => x.ID == id);
                GetData();
            }
        }
        /// <summary>
        /// انشاء خزينة جديدة
        /// </summary>
        public override void New()
        {
            drawer = new DAL.Drawer();
            base.New(); 
        }

        public override void GetData()
        {
            txt_Name.Text = drawer.Name;
            PartID = drawer.ID;     // لتسجيل حركات المستخدم
            PartName = drawer.Name; // لتسجيل حركات المستخدم
            base.GetData();
        }

        public override void SetData()
        {
            drawer.Name = txt_Name.Text;
            base.SetData();
        }

        public override void Save()
        {

            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ("الرجاء ادخال اسم الخزنة");
                return;
            }

            var db = new DAL.DBDataContext();
            DAL.Account account;
            if (drawer.ID == 0)// ID=0 في حالة الضغط على جديد يكون افتراضيا
            {
                account = new DAL.Account();// انشاء حساب للخزينة 
                db.Drawers.InsertOnSubmit(drawer);
                db.Accounts.InsertOnSubmit(account);
            }
            else
            {
                db.Drawers.Attach(drawer); 
                account = db.Accounts.Single(s => s.ID == drawer.AccountID);
            }
            SetData();
            account.Name = drawer.Name;
            db.SubmitChanges();// حفظ حساب الخزينة
            drawer.AccountID = account.ID;
            db.SubmitChanges();// حفظ الخزينة

            PartID = drawer.ID;     // لتسجيل حركات المستخدم
            PartName = drawer.Name; // لتسجيل حركات المستخدم
            base.Save();
        }
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    //التأكد من أن الخزنة غير مستخدمة قبل حذفها
                    var accountLog = db.Journals.Where(x => x.AccountID == drawer.AccountID).Count();

                    if (accountLog > 0)
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف الخزنة حيث أنه تم استخدامها في النظام"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        return;
                    }

                    db.Drawers.Attach(drawer);
                    db.Drawers.DeleteOnSubmit(drawer);
                    db.SubmitChanges();
                    db.Accounts.DeleteAllOnSubmit(db.Accounts.Where(x => x.ID == drawer.AccountID));
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }
        public override bool IsDataValide()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ("الرجاء ادخال اسم الخزنة");
                return false;
            }
            else
            {
                return base.IsDataValide();
            }
        }
    }
}