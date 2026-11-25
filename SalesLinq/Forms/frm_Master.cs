using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_Master : XtraForm
    {
        public bool IsNew;
        public string PartName;
        public int PartID;
        public static string ErrorText { get { return "هذا الحقل مطلوب"; } }

        public frm_Master() { InitializeComponent(); KeyPreview = true; }

        public virtual void Btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckActionAuthorization(Name, IsNew ? Master.Actions.Add : Master.Actions.Edit)) return;
            if (!IsDataValide()) return;
            Save();
            btn_Delete.Enabled = !IsNew;
        }
        private void Btn_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) { New(); }
        private void Btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { if (CheckActionAuthorization(Name, Master.Actions.Delete)) Delete(); }
        private void Btn_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { if (CheckActionAuthorization(Name, Master.Actions.Print)) Print(); }
        private void Btn_Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) { RefreshData(); }

        public static bool CheckActionAuthorization(string formName, Master.Actions action, DAL.User user = null)
        {
            user = user ?? Session.User;
            if (user == null)
            {
                XtraMessageBox.Show("يرجى تسجيل الدخول أولاً.", "صلاحيات الوصول", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (user.UserType == (byte)Master.UserType.Admin) return true;
            var screen = Session.ScreenAccesses == null ? null :
                Session.ScreenAccesses.SingleOrDefault(x => x.ScreenName == formName);
            bool allowed = screen != null; // منع السماح الافتراضي عند غياب تعريف الشاشة
            if (screen != null)
            {
                switch (action)
                {
                    case Master.Actions.Add: allowed = screen.CanAdd; break;
                    case Master.Actions.Edit: allowed = screen.CanEdit; break;
                    case Master.Actions.Delete: allowed = screen.CanDelete; break;
                    case Master.Actions.Print: allowed = screen.CanPrint; break;
                }
            }
            if (!allowed) XtraMessageBox.Show("غير مصرح لك بتنفيذ هذه العملية.", "صلاحيات الوصول", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return allowed;
        }

        public enum ActionType { Add, Edit, Delete, Print }
        public void InsertUserAction(ActionType type) { InsertUserAction(type, PartID, PartName, Name); }
        public static void InsertUserAction(ActionType type, int partID, string partName, string screenName)
        {
            if (Session.User == null) return;
            int screenID = 0;
            var screen = Class.Screens.GetScreens.SingleOrDefault(x => x.ScreenName == screenName);
            if (screen != null) screenID = screen.ScreenID;
            using (var db = new DAL.DBDataContext())
            {
                db.UserLogs.InsertOnSubmit(new DAL.UserLog { UserID = Session.User.ID, PartID = partID,
                    PartName = partName, ActionType = (byte)type, ActionDate = DateTime.Now, ScreenID = screenID });
                db.SubmitChanges();
            }
        }
        public virtual void GetHistory() { }
        public virtual void Save()
        {
            XtraMessageBox.Show("تم الحفظ بنجاح");
            RefreshData();
            InsertUserAction(IsNew ? ActionType.Add : ActionType.Edit);
            IsNew = false;
            GetHistory();
        }
        public virtual void New() { IsNew = true; GetData(); IsNew = true; btn_Delete.Enabled = false; }
        public virtual void Delete()
        {
            XtraMessageBox.Show("تم الحذف بنجاح", "حذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InsertUserAction(ActionType.Delete); New();
        }
        public virtual void Print() { InsertUserAction(ActionType.Print); }
        public virtual void RefreshData() { }
        public virtual void GetData() { GetHistory(); }
        public virtual void SetData() { }
        public virtual bool IsDataValide() { return true; }
        private void BarDockControlTop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) btn_Save.PerformClick();
            else if (e.KeyCode == Keys.F2) btn_New.PerformClick();
            else if (e.KeyCode == Keys.F3) btn_Print.PerformClick();
            else if (e.KeyCode == Keys.F4) btn_Delete.PerformClick();
        }
        public static bool AskForDeletion()
        {
            return XtraMessageBox.Show("هل تريد الحذف؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}