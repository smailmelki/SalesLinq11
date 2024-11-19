using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace SalesLinq.Forms
{
    public partial class frm_Login : XtraForm
    {
        public frm_Login()
        {           
            InitializeComponent();
        }

        bool ApplicationExit = true;   // متغير يمنع اغلاق البرنامج في حالة اغلاق فورم الدخول       
        public void Btn_LogIn_Click(object sender, EventArgs e)
        {
            using (var db=new DAL.DBDataContext())
            {
                if (db.Users.Count() == 0)
                {
                    MessageBox.Show("لا يوجد مستخدمين");
                    return;
                }
                var userName = txt_UserName.Text;
                var passWord = txt_UserPassword.Text;

                var user = db.Users.SingleOrDefault(x => x.UserName == userName);
                if (user==null)
                    goto LogInFailed;
                else
                {
                    if(user.IsActive==false)
                    {
                        XtraMessageBox.Show(
                      text: "تم تعطيل هذا الحساب",
                      caption: "",
                      icon: MessageBoxIcon.Error,
                      buttons: MessageBoxButtons.OK
                      );
                        return;
                    }
                    var pasWordHash = user.Password;
                    var hasher = new Liphsoft.Crypto.Argon2.PasswordHasher();
                    if(hasher.Verify(pasWordHash, passWord))
                    {
                        // Successfully login

                        this.Hide();
                        SplashScreenManager.ShowForm(parentForm:frm_Main.Instance,typeof(StartSplash));//فتح فورم التحميل

                        // put loading over here // تحميل كل البيانات الضرورية مثل قائمة الأصناف قبل تشغيل البرنامج

                        Class.Session.SetUser(user);
                        Type t = typeof(Class.Session);
                        var propertys = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);//ليتم التحميل قبل تشغيل البرنامج Session يجلب كل ما في 
                        foreach (var item in propertys)
                        {
                            var obj = item.GetValue(null);
                        }

                        ////////////////////
                        frm_Main.Instance.Lbl_UserName.Caption = user.Name;   // كتابة اسم المستخدم اعلى الصفحة الرئيسية
                        frm_Main.Instance.AddAccordionElement();              // تحميل أيقونات الصفحة الرئيسية
                        frm_Main.Instance.Show();                             // عرض الصفحة الرئيسية

                        ApplicationExit = false;
                        this.Close();
                        SplashScreenManager.CloseForm();
                        return;

                        ////////////////////
                    }
                    else
                        goto LogInFailed;
                }
            }
            LogInFailed:
            XtraMessageBox.Show(
               text: "اسم المستخدم أو كلمة السر غير صحيحة",
               caption: "",
               icon: MessageBoxIcon.Error,
               buttons: MessageBoxButtons.OK
               );
            return;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Frm_Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ApplicationExit)
            {
                Application.Exit();
            }
        }
    }
}