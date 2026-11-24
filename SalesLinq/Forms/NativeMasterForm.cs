using System;
using System.Linq;
using System.Drawing;
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

        protected virtual bool AuthorizeNewAction => true;
        protected virtual bool AllowUnauthenticatedSave => false;

        public NativeMasterForm()
        {
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            BackColor = Color.FromArgb(244, 247, 251);
            ForeColor = Color.FromArgb(38, 52, 72);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(760, 520);
            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                RightToLeft = RightToLeft.Yes,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(38, 52, 72),
                Font = new Font("Segoe UI", 9F),
                Padding = new Padding(4, 2, 4, 2),
                RenderMode = ToolStripRenderMode.System
            };

            btn_Save = CreateToolbarButton("حفظ");
            btn_New = CreateToolbarButton("جديد");
            btn_Delete = CreateToolbarButton("حذف");
            btn_Delete.Enabled = false;
            btn_Print = CreateToolbarButton("طباعة");
            btn_Refresh = CreateToolbarButton("تحديث");

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                btn_Save, btn_New, btn_Delete, new ToolStripSeparator(),
                btn_Print, btn_Refresh
            });

            Controls.Add(toolStrip);

            btn_Save.Click += (s, e) =>
            {
                if ((AllowUnauthenticatedSave || CheckActionAuthorization(Name, IsNew ? Master.Actions.Add : Master.Actions.Edit)) &&
                    IsDataValide())
                {
                    Save();
                    if (!IsNew)
                        btn_Delete.Enabled = true;
                }
            };

            btn_New.Click += (s, e) =>
            {
                if (!AuthorizeNewAction || CheckActionAuthorization(Name, Master.Actions.Add))
                    New();
            };

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

        private static ToolStripButton CreateToolbarButton(string text)
        {
            return new ToolStripButton(text)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(38, 52, 72),
                Margin = new Padding(3, 2, 3, 2)
            };
        }

        private void NativeMasterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) btn_Save.PerformClick();
            else if (e.KeyCode == Keys.F2) btn_New.PerformClick();
            else if (e.KeyCode == Keys.F3) btn_Print.PerformClick();
            else if (e.KeyCode == Keys.F4) btn_Delete.PerformClick();
            else return;

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        public static bool CheckActionAuthorization(string formName, Master.Actions action, DAL.User user = null)
        {
            user = user ?? Session.User;
            if (user == null)
            {
                MessageBox.Show("يجب تسجيل الدخول أولاً.", "الصلاحيات",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (user.UserType == (byte)Master.UserType.Admin)
                return true;

            var screen = Session.ScreenAccesses?.SingleOrDefault(x => x.ScreenName == formName);
            if (screen == null)
            {
                MessageBox.Show("لا توجد صلاحية معرفة لهذه الشاشة.", "الصلاحيات",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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
            btn_Delete.Enabled = true;
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