using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Liphsoft.Crypto.Argon2;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_NewLogin : frm_Master
    {
        DAL.User user;
        bool ApplicationExit = true;   // متغير يمنع اغلاق البرنامج في حالة اغلاق فورم الدخول       
        public frm_NewLogin()
        {
            InitializeComponent();
            New();
        }
        private void Frm_NewLogin_Load(object sender, EventArgs e)
        {
            lkp_UserType.InitializeData(Master.UserTypeList);
            lkp_UserType.EditValue = 1;
            toggleSwitch1.IsOn = true;
        }

        public override void New()
        {
            btn_New.Visibility = BarItemVisibility.Never;
            btn_Delete.Visibility = BarItemVisibility.Never;
            user = new DAL.User();
            user.IsActive = true;
        }
        public override void SetData()
        {
            //تشفير الرقم السري
            if (user.Password != txt_Passwoord.Text)
            {
                var hasher = new PasswordHasher();
                string myhash = hasher.Hash(txt_Passwoord.Text);
                txt_Passwoord.Text = myhash;
            }
            user.Name = txt_Name.Text;
            user.Password = txt_Passwoord.Text;
            user.UserName = txt_UserName.Text.Trim();
            user.UserType = Convert.ToByte(lkp_UserType.EditValue);
            user.IsActive = toggleSwitch1.IsOn;
        }

        public override void Btn_Save_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (IsDataValide())
            {
                Save();
            }
        }

        /// <summary>
        /// حفظ بيانات أول مستخدم للبرنامج
        /// </summary>
        public override void Save()
        {
            using (var db = new DAL.DBDataContext())
            {
                db.Users.InsertOnSubmit(user);
                SetData();
                db.SubmitChanges();
            }
            XtraMessageBox.Show("تم الحفظ بنجاح");
            ApplicationExit = false;
            this.Close();
            using (var db = new DAL.DBDataContext())
            {
                if (db.Users.Count() == 0)
                {
                    var frm = new frm_NewLogin();
                    frm.Show();
                }
                else
                {
                    var frm = new frm_Login();
                    frm.Show();

                }
            }
        }
        /// <summary>
        /// التحقق من البيانات المدخلة قبل الحفظ
        /// </summary>
        /// <returns></returns>
        public override bool IsDataValide()
        {
            int NumberOfErrors = 0;

            NumberOfErrors += txt_Name.IsTextValide() ? 0 : 1;// التحقق من أن النص غير فارغ 
            NumberOfErrors += txt_Passwoord.IsTextValide() ? 0 : 1;// التحقق من أن النص غير فارغ 
            NumberOfErrors += txt_UserName.IsTextValide() ? 0 : 1;// التحقق من أن النص غير فارغ 
            NumberOfErrors += lkp_UserType.IsEditValueValideAndNotZero() ? 0 : 1;// التحقق من أن النص رقمي و ليس صفر           
            if ((int)lkp_UserType.EditValue != 1) // التحقق من أن المستخدم بصلاحية مدير
            {
                NumberOfErrors += 1;
                lkp_UserType.ErrorText = "يجب أن يكون المستخدم الأول بصلاحية مدير";
            }
            if (!toggleSwitch1.IsOn) // التحقق من أن المستخدم نشط
            {
                NumberOfErrors += 1;
                toggleSwitch1.ErrorText = "يجب أن يكون المستخدم نشط";
            }
            return (NumberOfErrors == 0);
        }

        private void Frm_NewLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ApplicationExit)
            {
                Application.Exit();
            }
        }
    }
}