using System;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    /// <summary>
    /// أساس شاشات الإدخال المبنية بعناصر Windows Forms القياسية.
    /// </summary>
    public class NativeMasterForm : Form
    {
        protected readonly ToolStrip toolStrip;
        protected readonly ToolStripButton btn_Save;
        protected readonly ToolStripButton btn_New;
        protected readonly ToolStripButton btn_Delete;
        protected readonly ToolStripButton btn_Print;
        protected readonly ToolStripButton btn_Refresh;

        public bool IsNew;
        public string PartName;
        public int PartID;

        public static string ErrorText => "هذا الحقل مطلوب";

        public NativeMasterForm()
        {
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            KeyPreview = true;

            toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                RightToLeft = RightToLeft.Yes
            };

            btn_Save = new ToolStripButton("حفظ");
            btn_New = new ToolStripButton("جديد");
            btn_Delete = new ToolStripButton("حذف");
            btn_Print = new ToolStripButton("طباعة");
            btn_Refresh = new ToolStripButton("تحديث");

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                btn_Save, btn_New, btn_Delete, new ToolStripSeparator(),
                btn_Print, btn_Refresh
            });

            Controls.Add(toolStrip);

            btn_Save.Click += (s, e) =>
            {
                if (CheckActionAuthorization(Name, IsNew ? Master.Actions.Add : Master.Actions.Edit) &&
                    IsDataValide())
                {
                    Save();
                    btn_Delete.Enabled = true;
                }
            };
            btn_New.Click += (s, e) => New();
            btn_Delete.Click += (s, e) =>
            {
                if (CheckActionAuthorization(Name, Master.Actions.Delete))
                    Delete();
            };
            btn_Print.Click += (s, e) =>
            {
                if (CheckActionAuthorization(Name, Master.Actions.Print))
                    Print();
            };
            btn_Refresh.Click += (s, e) => RefreshData();
            KeyDown += NativeMasterForm_KeyDown;
        }

        private void NativeMasterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) btn_Save.PerformClick();
            else if (e.KeyCode == Keys.F2) btn_New.PerformClick();
            else if (e.KeyCode == Keys.F3) btn_Print.PerformClick();
            else if (e.KeyCode == Keys.F4) btn_Delete.PerformClick();
            else return;

            e.Handled = true;
        }

        public static bool CheckActionAuthorization(string formName, Master.Actions action, DAL.User user = null)
        {
            user = user ?? Session.User;
            if (user == null)
                return false;

            if (user.UserType == (byte)Master.UserType.Admin)
                return true;

            var screen = Session.ScreenAccesses?.SingleOrDefault(x => x.ScreenName == formName);
            if (screen == null)
                return false;

            bool allowed;
            switch (action)
            {
                case Master.Actions.Add: allowed = screen.CanAdd; break;
                case Master.Actions.Edit: allowed = screen.CanEdit; break;
                case Master.Actions.Delete: allowed = screen.CanDelete; break;
                case Master.Actions.Print: allowed = screen.CanPrint; break;
                default: allowed = false; break;
            }

            if (!allowed)
                MessageBox.Show("غير مصرح لك بتنفيذ هذه العملية.", "الصلاحيات",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return allowed;
        }

        public enum ActionType { Add, Edit, Delete, Print }

        public void InsertUserAction(ActionType actionType)
        {
            InsertUserAction(actionType, PartID, PartName, Name);
        }

        public static void InsertUserAction(ActionType actionType, int partId, string partName, string screenName)
        {
            if (Session.User == null)
                return;

            int screenId = 0;
            var screen = Screens.GetScreens.SingleOrDefault(x => x.ScreenName == screenName);
            if (screen != null)
                screenId = screen.ScreenID;

            using (var db = new DAL.DBDataContext())
            {
                db.UserLogs.InsertOnSubmit(new DAL.UserLog
                {
                    UserID = Session.User.ID,
                    PartID = partId,
                    PartName = partName,
                    ActionType = (byte)actionType,
                    ActionDate = DateTime.Now,
                    ScreenID = screenId
                });
                db.SubmitChanges();
            }
        }

        public virtual void GetHistory() { }

        public virtual void Save()
        {
            MessageBox.Show("تم الحفظ بنجاح.", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshData();
            InsertUserAction(IsNew ? ActionType.Add : ActionType.Edit);
            IsNew = false;
            GetHistory();
        }

        public virtual void New()
        {
            IsNew = true;
            GetData();
            IsNew = true;
            btn_Delete.Enabled = false;
        }

        public virtual void Delete()
        {
            MessageBox.Show("تم الحذف بنجاح.", "حذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InsertUserAction(ActionType.Delete);
            New();
        }

        public virtual void Print()
        {
            InsertUserAction(ActionType.Print);
        }

        public virtual void RefreshData() { }
        public virtual void GetData() { GetHistory(); }
        public virtual void SetData() { }
        public virtual bool IsDataValide() { return true; }

        public static bool AskForDeletion()
        {
            return MessageBox.Show("هل تريد حذف هذا السجل؟", "تأكيد الحذف",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }
}