using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_AccessProfile : frm_Master
    {
        DAL.UserAccessProfileName profile;
        public frm_AccessProfile()
        {
            InitializeComponent();
            New();
            GetData();
        }
        public frm_AccessProfile(int id)
        {
            InitializeComponent();
            using (var db=new DAL.DBDataContext())
            {
                profile = db.UserAccessProfileNames.SingleOrDefault(x => x.ID == id);
            }
            if (profile != null)
            {
                textEdit1.Text = profile.Name;
                PartID = profile.ID;     // لتسجيل حركات المستخدم
                PartName = profile.Name; // لتسجيل حركات المستخدم
                GetData();
            }
        }

        public override void New()
        {
            profile = new DAL.UserAccessProfileName();
            textEdit1.Text = profile.Name;
            btn_Delete.Enabled = false;
            treeList1.CustomNodeCellEdit += TreeList1_CustomNodeCellEdit;
        }
        /// <summary>
        /// اخفاء خانة الاختيار في الخصائص الغير موجودة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeList1_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
        {
            if (e.Node.Id >= 0)
            {
                var row = treeList1.GetRow(e.Node.Id) as Class.ScreensAccessProfile;
                if(row != null)
                {
                    if (e.Column.FieldName == nameof(ins.CanAdd) && row.Actions.Contains(Class.Master.Actions.Add) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                    else if (e.Column.FieldName == nameof(ins.CanDelete) && row.Actions.Contains(Class.Master.Actions.Delete) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                    else if (e.Column.FieldName == nameof(ins.CanEdit) && row.Actions.Contains(Class.Master.Actions.Edit) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                    else if (e.Column.FieldName == nameof(ins.CanOpen) && row.Actions.Contains(Class.Master.Actions.Open) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                    else if (e.Column.FieldName == nameof(ins.CanPrint) && row.Actions.Contains(Class.Master.Actions.Print) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                    else if (e.Column.FieldName == nameof(ins.CanShow) && row.Actions.Contains(Class.Master.Actions.Show) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();//اخفاء خانة الاختيار في الخصائص الغير موجودة
                    }
                }
            }
        }
        // انستانس من اجل أسماء الشاشات
        Class.ScreensAccessProfile ins;
        /// <summary>
        /// تعبئة treeList1 بالبيانات
        /// </summary>
        public override void GetData()
        {
            List<Class.ScreensAccessProfile> data;
            using (DAL.DBDataContext db = new DAL.DBDataContext())
            {
                data = (from s in Class.Screens.GetScreens
                        from d in db.UserAccessProfileDetails
                        .Where(x => x.ProfileID == profile.ID && x.ScreenID == s.ScreenID).DefaultIfEmpty()
                        select new Class.ScreensAccessProfile(s.ScreenName)
                        {
                            CanAdd = (d == null) ? true : d.CanAdd,
                            CanDelete = (d == null) ? true : d.CanDelete,
                            CanEdit = (d == null) ? true : d.CanEdit,
                            CanOpen = (d == null) ? true : d.CanOpen,
                            CanPrint = (d == null) ? true : d.CanPrint,
                            CanShow = (d == null) ? true : d.CanShow,
                            Actions = s.Actions,
                            ScreenName = s.ScreenName,
                            ScreenCaption = s.ScreenCaption,
                            ScreenID = s.ScreenID,
                            ParentScreenID = s.ParentScreenID
                        }).ToList();
            }
            //treeList1 جلب كل الشاشات ووضعها في 
            treeList1.DataSource = data;
            //العمود الأب
            treeList1.KeyFieldName = nameof(ins.ScreenID);
            //العمود الابن
            treeList1.ParentFieldName = nameof(ins.ParentScreenID);
            treeList1.Columns[nameof(ins.ScreenName)].Visible = false;
            treeList1.Columns[nameof(ins.ScreenName)].OptionsColumn.AllowEdit = false;
            treeList1.Columns[nameof(ins.ScreenCaption)].OptionsColumn.AllowEdit = false;
            treeList1.BestFitColumns();
        }
        /// <summary>
        /// التحقق من المدخلات في الاعدادات
        /// </summary>
        /// <returns></returns>
        public override bool IsDataValide()
        {
            int flag = 0;
            if (textEdit1.Text.Trim() == string.Empty)
            {
                flag++;
                textEdit1.ErrorText = ErrorText;
            }
            else
            {
                using (var db =new DAL.DBDataContext())
                {
                    if (db.UserAccessProfileNames.Where(x => x.Name == textEdit1.Text.Trim() && x.ID != profile.ID).Any())
                    {
                        flag++;
                        textEdit1.ErrorText = "هذا الاسم مستخدم";
                    }
                }
            }
            return (flag == 0);
        }
        public override void Save()
        {
            var db = new DAL.DBDataContext();
            if (profile.ID == 0)
            {
                db.UserAccessProfileNames.InsertOnSubmit(profile);
            }
            else
            {
                db.UserAccessProfileNames.Attach(profile);
            }
            profile.Name = textEdit1.Text;
            db.SubmitChanges();
            db.UserAccessProfileDetails.DeleteAllOnSubmit(
            db.UserAccessProfileDetails.Where(x => x.ProfileID == profile.ID));
            db.SubmitChanges();

            var data = treeList1.DataSource as List<Class.ScreensAccessProfile>;
            var dbData = data.Select(s => new DAL.UserAccessProfileDetail
            {
                CanAdd = s.CanAdd,
                CanDelete = s.CanDelete,
                CanEdit = s.CanEdit,
                CanOpen = s.CanOpen,
                CanPrint = s.CanPrint,
                CanShow = s.CanShow,                
                ProfileID = profile.ID,   
                ScreenID = s.ScreenID   
            }).ToList();
            db.UserAccessProfileDetails.InsertAllOnSubmit(dbData);
            db.SubmitChanges();
            PartID = profile.ID;     // لتسجيل حركات المستخدم
            PartName = profile.Name; // لتسجيل حركات المستخدم
            base.Save();
        }
        RepositoryItemCheckEdit repoCheck;
        private void Frm_AccessProfile_Load(object sender, EventArgs e)
        {
            textEdit1.Text = profile.Name;
            treeList1.CustomNodeCellEdit += TreeList1_CustomNodeCellEdit;
            //العمود الأب
            treeList1.KeyFieldName = nameof(ins.ScreenID);
            //العمود الابن
            treeList1.ParentFieldName = nameof(ins.ParentScreenID);
            treeList1.Columns[nameof(ins.ScreenName)].Visible = false;
            treeList1.Columns[nameof(ins.ScreenName)].OptionsColumn.AllowEdit = false;
            treeList1.Columns[nameof(ins.ScreenCaption)].OptionsColumn.AllowEdit = false;
            //تغيير اسماء الأعمدة
            treeList1.Columns[nameof(ins.CanAdd)].Caption = "اضافة";
            treeList1.Columns[nameof(ins.CanDelete)].Caption = "حذف";
            treeList1.Columns[nameof(ins.CanEdit)].Caption = "تعديل";
            treeList1.Columns[nameof(ins.CanOpen)].Caption = "فتح";
            treeList1.Columns[nameof(ins.CanPrint)].Caption = "طباعة";
            treeList1.Columns[nameof(ins.CanShow)].Caption = "عرض";

            treeList1.BestFitColumns();
            //تغيير شكل اداة الاختيار
            repoCheck = new RepositoryItemCheckEdit();
            repoCheck.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
            repoCheck.CheckBoxOptions.SvgColorChecked = DXSkinColors.ForeColors.Information;
            repoCheck.CheckBoxOptions.SvgColorUnchecked = DXSkinColors.ForeColors.DisabledText;
            treeList1.Columns[nameof(ins.CanAdd)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanDelete)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanEdit)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanOpen)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanPrint)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanShow)].ColumnEdit = repoCheck;
            //تغيير عرض الأعمدة
            treeList1.Columns[nameof(ins.CanAdd)].Width =
            treeList1.Columns[nameof(ins.CanDelete)].Width =
            treeList1.Columns[nameof(ins.CanEdit)].Width =
            treeList1.Columns[nameof(ins.CanOpen)].Width =
            treeList1.Columns[nameof(ins.CanPrint)].Width =
            treeList1.Columns[nameof(ins.CanShow)].Width = 25;
        }
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    //التأكد من أنه غير مستخدم قبل حذفه
                    if (db.Users.Where(x => x.ScreenProfileID == profile.ID).Any())
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف صلاحية الوصول حيث أنها مرتبطة بأحد المستخدمين"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        return;
                    }
                    db.UserAccessProfileNames.Attach(profile);
                    db.UserAccessProfileNames.DeleteOnSubmit(profile);
                    db.UserAccessProfileDetails.DeleteAllOnSubmit(db.UserAccessProfileDetails.Where(x => x.ProfileID == profile.ID));
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }
    }
}