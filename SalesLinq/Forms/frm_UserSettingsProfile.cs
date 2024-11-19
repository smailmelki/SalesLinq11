using DevExpress.Utils.Animation;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using SalesLinq.Class;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_UserSettingsProfile : frm_Master
    {
        DAL.UserSettingsProfile profile;
        List<BaseEdit> editors;// قائمة للتحقق من البيانات المدخلة
        public frm_UserSettingsProfile()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
            New();
            GetData();
        }
        public frm_UserSettingsProfile(int id)
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
            using (var db = new DAL.DBDataContext())
            {
                profile = db.UserSettingsProfiles.Single(x => x.ID == id);
                textEdit1.Text = profile.Name;
                GetData();
            }
        }
        /// <summary>
        /// دالة لفتح التاب عند الضغط على العنصر
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccordionControl1_ElementClick(object sender, DevExpress.XtraBars.Navigation.ElementClickEventArgs e)
        {
            var index = accordionControl1.Elements.IndexOf(e.Element);
            xtraTabControl1.SelectedTabPageIndex = index;
        }

        public override void New()
        {
            profile = new DAL.UserSettingsProfile();
            textEdit1.Text = profile.Name;
        }
        /// <summary>
        /// تصميم شاشة نماذج الاعدادات بالكود
        /// </summary>
        public override void GetData()
        {

            editors = new List<BaseEdit>();
            Class.UserSettingsTemplate settings = new Class.UserSettingsTemplate(profile.ID);
            accordionControl1.Elements.Clear();             //حذف كل العناصر
            xtraTabControl1.TabPages.Clear();               //حذف كل التابات
            accordionControl1.AllowItemSelection = true;    //الابقاء على تحديد العنصر المختار

            var catalog = settings.GetType().GetProperties();//settingsTemplate متغير يحتوي بيانات جميع البروبرتيز العامة التي في 
            //اضافة التابات والعناصر 
            foreach (var item in catalog)
            {
                accordionControl1.Elements.Add(new DevExpress.XtraBars.Navigation.AccordionControlElement()
                {
                    Name = nameof(item),
                    Text = Class.UserSettingsTemplate.GetPropCaption(item.Name),
                    Style = DevExpress.XtraBars.Navigation.ElementStyle.Item,
                });

                var page = new DevExpress.XtraTab.XtraTabPage()
                { Name = item.Name, Text = Class.UserSettingsTemplate.GetPropCaption(item.Name) };
                xtraTabControl1.TabPages.Add(page);
                LayoutControl lc = new LayoutControl();
                EmptySpaceItem empty1 = new EmptySpaceItem();
                EmptySpaceItem empty2 = new EmptySpaceItem();
                empty2.SizeConstraintsType = SizeConstraintsType.Custom;
                empty2.MaxSize = new Size(500, 0);
                empty2.MinSize = new Size(250, 0);
                lc.AddItem(empty1);
                lc.AddItem(empty2, empty1, DevExpress.XtraLayout.Utils.InsertType.Right);

                var props = item.GetValue(settings).GetType().GetProperties();// متغير لجلب البيانات
                foreach (var prop in props)
                {
                    BaseEdit edit = Class.UserSettingsTemplate.GetPropertyControl(prop.Name, prop.GetValue(item.GetValue(settings)));
                    if (edit != null)
                    {
                        var layoutItem = lc.AddItem(string.Empty, edit, empty1, DevExpress.XtraLayout.Utils.InsertType.Top);
                        layoutItem.TextVisible = true;
                        layoutItem.Text = Class.UserSettingsTemplate.GetPropCaption(prop.Name);
                        layoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
                        layoutItem.MaxSize = new Size(700, 30);
                        layoutItem.MinSize = new Size(250, 30);
                        editors.Add(edit);
                    }

                }
                lc.Dock = DockStyle.Fill;
                page.Controls.Add(lc);
            }
            PartID = profile.ID;     // لتسجيل حركات المستخدم
            PartName = profile.Name; // لتسجيل حركات المستخدم
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
                using (var db = new DAL.DBDataContext())
                {
                    if( db.UserSettingsProfiles.Where(x => x.Name == textEdit1.Text.Trim() && x.ID != profile.ID).Count()>0)
                    {
                        flag++;
                        textEdit1.ErrorText = "هذا الاسم مستخدم";
                    }
                }
            }
            editors.ForEach(e =>
            {
                if (e.GetType() == typeof(LookUpEdit) && ((LookUpEdit)e).EditValue.ToString().Trim() == "0")
                {
                    flag += ((LookUpEdit)e).IsEditValueValideAndNotZero() ? 0 : 1;
                }
                //if (e.GetType() == typeof(LookUpEdit) && ((LookUpEdit)e).Properties.DataSource.GetType() != typeof(List<Master.ValueAndID>))
                //{
                //    Debug.Print(((LookUpEdit)e).EditValue.ToString().Trim());
                //    flag += ((LookUpEdit)e).IsEditValueValideAndNotZero() ? 0 : 1;
                //}
            });
            return (flag == 0);
        }

        public override void Save()
        {
            var db = new DAL.DBDataContext();
            if (profile.ID == 0)
            {
                db.UserSettingsProfiles.InsertOnSubmit(profile);
            }
            else
            {
                db.UserSettingsProfiles.Attach(profile);
            }
            profile.Name = textEdit1.Text;
            db.SubmitChanges();
            db.UserSettingsProfileProperties.DeleteAllOnSubmit(
            db.UserSettingsProfileProperties.Where(x => x.ProfileID == profile.ID));
            db.SubmitChanges();
            editors.ForEach(e =>
            {
                db.UserSettingsProfileProperties.InsertOnSubmit(new DAL.UserSettingsProfileProperty()
                {
                    ProfileID = profile.ID,
                    PropertyName = e.Name,
                    PropertyValue = Master.ToByteArray<object>(e.EditValue)
                });
            });
            db.SubmitChanges();
            PartID = profile.ID;     // لتسجيل حركات المستخدم
            PartName = profile.Name; // لتسجيل حركات المستخدم
            base.Save();
        }
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    //التأكد من أنه غير مستخدم قبل حذفه
                    var log = db.Users.Where(x => x.SettingsProfileID == profile.ID).Count();
                    if (log > 0)
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف الاعداد حيث أنه مرتبط بأحد المستخدمين"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        return;
                    }
                    db.UserSettingsProfiles.Attach(profile);
                    db.UserSettingsProfiles.DeleteOnSubmit(profile);
                    db.UserSettingsProfileProperties.DeleteAllOnSubmit(db.UserSettingsProfileProperties.Where(x => x.ProfileID == profile.ID));
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }

        private void Frm_UserSettingsProfile_Load(object sender, EventArgs e)
        {
            accordionControl1.AnimationType = DevExpress.XtraBars.Navigation.AnimationType.Simple;
            accordionControl1.Appearance.Item.Hovered.Font =
                new Font(accordionControl1.Appearance.Item.Hovered.Font, FontStyle.Bold);
            accordionControl1.Appearance.Item.Hovered.Options.UseFont = true;//تغيير الخط عند وضع مؤشر الفأرة عليه
            accordionControl1.Appearance.Item.Pressed.Font =
                new Font(accordionControl1.Appearance.Item.Pressed.Font, FontStyle.Bold);
            accordionControl1.Appearance.Item.Pressed.Options.UseFont = true;//تغيير الخط عند الصغط عليه
            accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;//ScrollBar اخفاء 
            accordionControl1.OptionsMinimizing.AllowMinimizeMode = DevExpress.Utils.DefaultBoolean.False;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;//xtraTabControl1 اخفاء راس الصفحة الخاص ب 
            // خاص باعدادات الانيميشن عند تغيير التاب
            xtraTabControl1.Transition.AllowTransition = DevExpress.Utils.DefaultBoolean.True;
            xtraTabControl1.Transition.EasingMode = DevExpress.Data.Utils.EasingMode.EaseInOut;
            SlideFadeTransition trans = new SlideFadeTransition();
            trans.Parameters.EffectOptions = PushEffectOptions.FromBottom;
            xtraTabControl1.Transition.TransitionType = trans;
            xtraTabControl1.SelectedPageChanging += XtraTabControl1_SelectedPageChanging;
        }
        /// <summary>
        /// دالة لجعل التابات تتغير من الاعلى الى الأسفل والعكس
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            SlideFadeTransition trans = new SlideFadeTransition();
            var currenPage = xtraTabControl1.TabPages.IndexOf(e.Page);
            var PrevPage = xtraTabControl1.TabPages.IndexOf(e.PrevPage);
            if (currenPage > PrevPage)
                trans.Parameters.EffectOptions = PushEffectOptions.FromBottom;
            else
                trans.Parameters.EffectOptions = PushEffectOptions.FromTop;
            xtraTabControl1.Transition.TransitionType = trans;
        }
    }
}