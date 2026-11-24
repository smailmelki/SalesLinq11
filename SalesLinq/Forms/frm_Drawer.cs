using System;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_Drawer : NativeMasterForm
    {
        private DAL.Drawer drawer;

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

        private void LoadDrawer(int id)
        {
            using (var db = new DAL.DBDataContext())
                drawer = db.Drawers.Single(x => x.ID == id);

            GetData();
        }

        public override void New()
        {
            drawer = new DAL.Drawer();
            base.New();
        }

        public override void GetData()
        {
            txt_Name.Text = drawer == null ? string.Empty : drawer.Name;
            PartID = drawer == null ? 0 : drawer.ID;
            PartName = drawer == null ? string.Empty : drawer.Name;
            btn_Delete.Enabled = drawer != null && drawer.ID != 0;
            base.GetData();
        }

        public override void SetData()
        {
            drawer.Name = txt_Name.Text.Trim();
            base.SetData();
        }

        public override bool IsDataValide()
        {
            if (string.IsNullOrWhiteSpace(txt_Name.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم الخزنة.", "بيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Name.Focus();
                return false;
            }

            return base.IsDataValide();
        }

        public override void Save()
        {
            if (!IsDataValide())
                return;

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
                            if (drawer.ID == 0)
                            {
                                SetData();
                                var account = new DAL.Account { Name = drawer.Name };
                                db.Accounts.InsertOnSubmit(account);
                                db.SubmitChanges();

                                drawer.AccountID = account.ID;
                                db.Drawers.InsertOnSubmit(drawer);
                            }
                            else
                            {
                                db.Drawers.Attach(drawer);
                                SetData();
                                var account = db.Accounts.Single(x => x.ID == drawer.AccountID);
                                account.Name = drawer.Name;
                            }

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

                PartID = drawer.ID;
                PartName = drawer.Name;
                base.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حفظ الخزنة.\n" + ex.Message, "خطأ في الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Delete()
        {
            if (drawer == null || drawer.ID == 0 || !AskForDeletion())
                return;

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
                            bool hasTransactions = db.Journals.Any(x => x.AccountID == drawer.AccountID);
                            if (hasTransactions)
                            {
                                transaction.Rollback();
                                MessageBox.Show(
                                    "لا يمكن حذف الخزنة لأنها مرتبطة بحركات أو قيود محاسبية.",
                                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            db.Drawers.Attach(drawer);
                            db.Drawers.DeleteOnSubmit(drawer);
                            db.Accounts.DeleteAllOnSubmit(
                                db.Accounts.Where(x => x.ID == drawer.AccountID));

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
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حذف الخزنة.\n" + ex.Message, "خطأ في الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}