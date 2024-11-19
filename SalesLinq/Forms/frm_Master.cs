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

        public static string ErrorText
        {
            get
            {
                return "هذا الحقل مطلوب";
            }
        }
        public frm_Master()
        {
            InitializeComponent();
        }

        public virtual void Btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, IsNew ? Master.Actions.Add : Master.Actions.Edit))
                if (IsDataValide())
                {
                    Save();
                    btn_Delete.Enabled = true;
                }
        }

        private void Btn_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();
        }

        private void Btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, Master.Actions.Delete))
                Delete();
        }

        private void Btn_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, Master.Actions.Print))
                Print();
        }

        private void Btn_Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshData();
        }
        /// <summary>
        /// التحقق من الصلاحيات أولا
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="actions"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckActionAuthorization(string formName, Master.Actions actions, DAL.User user = null)
        {
            if (user == null)
                user = Session.User;

            if (user.UserType == (byte)Master.UserType.Admin)
                return true;
            else
            {
                var screen = Session.ScreenAccesses.SingleOrDefault(x => x.ScreenName == formName);
                bool flag = true;
                if (screen != null)
                {
                    switch (actions)
                    {

                        case Master.Actions.Add:
                            flag = screen.CanAdd;
                            break;
                        case Master.Actions.Edit:
                            flag = screen.CanEdit;
                            break;
                        case Master.Actions.Delete:
                            flag = screen.CanDelete;
                            break;
                        case Master.Actions.Print:
                            flag = screen.CanPrint;
                            break;
                        default:
                            break;
                    }
                }
                if (flag == false)
                {
                    XtraMessageBox.Show(
                                        text: " غير مصرح لك",
                                        caption: "",
                                        icon: MessageBoxIcon.Error,
                                        buttons: MessageBoxButtons.OK
                                        );
                }
                return flag;
            }
        }

        public enum ActionType
        {
            Add,
            Edit,
            Delete,
            Print
        }
        /// <summary>
        /// دالة تسجيل حركات المستخدم
        /// </summary>
        /// <param name="actionTyp"></param>
        public void InsertUserAction(ActionType actionTyp)
        {
            InsertUserAction(actionTyp,this.PartID,this.PartName,this.Name);
        }
        /// <summary>
        /// دالة تسجيل حركات المستخدم
        /// </summary>
        /// <param name="actionTyp"></param>
        /// <param name="PartID"></param>
        /// <param name="PartName"></param>
        /// <param name="ScreenName"></param>
        public static void InsertUserAction(ActionType actionTyp,int PartID,string PartName,string ScreenName)
        {
            int screenID = 0;
            var screen = Class.Screens.GetScreens.SingleOrDefault(x => x.ScreenName == ScreenName);
            if (screen != null) screenID = screen.ScreenID;
            using (var db = new DAL.DBDataContext())
            {
                db.UserLogs.InsertOnSubmit(new DAL.UserLog()
                {
                    UserID = Session.User.ID,
                    PartID = PartID,
                    PartName = PartName,
                    ActionType = (byte)actionTyp,
                    ActionDate = DateTime.Now,
                    ScreenID = screenID
                });
                db.SubmitChanges();
            }
        }

        public virtual void GetHistory()
        {

        }

        public virtual void Save()
        {
            XtraMessageBox.Show("تم الحفظ بنجاح");
            RefreshData();
            InsertUserAction((IsNew) ? ActionType.Add : ActionType.Edit);
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
            XtraMessageBox.Show(

                text: "تم الحذف بنجاح",
                caption: "حذف",
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Information );

            InsertUserAction(ActionType.Delete);
            New();
        }

        public virtual void Print()
        {
            InsertUserAction(ActionType.Print);
        }

        public virtual void RefreshData()
        {
            //GetHistory();
        }

        public virtual void GetData()
        {
            GetHistory();
        }

        public virtual void SetData()
        {

        }

        public virtual bool IsDataValide()
        {
            return true;
        }

        private void BarDockControlTop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btn_Save.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_New.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                btn_Print.PerformClick();
            }
            if (e.KeyCode == Keys.F4)
            {
                btn_Delete.PerformClick();
            }
        }

        public static bool AskForDeletion()
        {
            return (XtraMessageBox.Show(

                text: "هل تريد الحذف",
                caption: "تأكيد الحذف",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Asterisk) == DialogResult.Yes
                );
        }



    }
}