using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using SalesLinq.DAL;

namespace SalesLinq.Forms
{
    // فورم تسجيل الدخول
    public partial class frm_Login : XtraForm
    {
        public frm_Login()
        {
            InitializeComponent();  // تهيئة مكونات الفورم
        }

        bool ApplicationExit = true;   // متغير للتحكم في إغلاق التطبيق عند إغلاق فورم تسجيل الدخول

        // حدث عند الضغط على زر الدخول
        public void Btn_LogIn_Click(object sender, EventArgs e)
        {
            using (var db = new DBDataContext())  // إنشاء اتصال بقاعدة البيانات
            {
                // التحقق إذا كانت قاعدة البيانات لا تحتوي على أي مستخدمين
                if (db.Users.Count() == 0) 
                {
                    // عرض رسالة إذا لم يكن هناك مستخدمين
                    XtraMessageBox.Show(
                        text: "لا يوجد مستخدمين",
                        caption: "",
                        icon: MessageBoxIcon.Warning,
                        buttons: MessageBoxButtons.OK);  
                    return;
                }

                var userName = txt_UserName.Text;  // الحصول على اسم المستخدم من مربع النص
                var passWord = txt_UserPassword.Text;  // الحصول على كلمة المرور من مربع النص
                var user = db.Users.SingleOrDefault(x => x.UserName == userName);  // التحقق إذا كان يوجد مستخدم بهذا الاسم

                // إذا لم يتم العثور على المستخدم أو كانت كلمة المرور غير صحيحة
                if (user == null || !VerifyPassword(user, passWord))
                {
                    ShowLoginFailedMessage();  // عرض رسالة فشل الدخول
                    return;
                }

                // التحقق إذا كان الحساب مفعلًا
                if (!user.IsActive)
                {
                    XtraMessageBox.Show(
                        text: "تم تعطيل هذا الحساب",  // عرض رسالة إذا تم تعطيل الحساب
                        caption: "",
                        icon: MessageBoxIcon.Error,
                        buttons: MessageBoxButtons.OK);
                    return;
                }

                // في حال كان تسجيل الدخول ناجحًا
                HandleSuccessfulLogin(user);  // التعامل مع الدخول الناجح
            }
        }

        // دالة للتحقق من كلمة المرور باستخدام خوارزمية Argon2
        private bool VerifyPassword(User user, string passWord)
        {
            var hasher = new Liphsoft.Crypto.Argon2.PasswordHasher();  // إنشاء كائن للتحقق من كلمة المرور
            return hasher.Verify(user.Password, passWord);  // التحقق إذا كانت كلمة المرور المدخلة صحيحة
        }

        // دالة لعرض رسالة فشل الدخول
        private void ShowLoginFailedMessage()
        {
            XtraMessageBox.Show(
                text: "اسم المستخدم أو كلمة السر غير صحيحة",  // عرض رسالة إذا كانت البيانات المدخلة خاطئة
                caption: "",
                icon: MessageBoxIcon.Error,
                buttons: MessageBoxButtons.OK);
        }

        // دالة للتعامل مع الدخول الناجح وعرض الصفحة الرئيسية
        private void HandleSuccessfulLogin(User user)
        {
            this.Hide();  // إخفاء نافذة تسجيل الدخول
            SplashScreenManager.ShowForm(parentForm: frm_Main.Instance, typeof(StartSplash));  // عرض شاشة تحميل مؤقتة

            // تحميل البيانات الضرورية (مثل الأصناف، الإعدادات، الخ) قبل عرض الصفحة الرئيسية
            Class.Session.SetUser(user);  // تعيين المستخدم في الجلسة الحالية

            // الحصول على خصائص الجلسة وتحديث البيانات اللازمة
            Type t = typeof(Class.Session);
            var properties = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var property in properties)
            {
                var obj = property.GetValue(null);  // الحصول على قيمة كل خاصية في الجلسة
            }

            // تحديث واجهة المستخدم
            frm_Main.Instance.Lbl_UserName.Caption = user.Name;  // عرض اسم المستخدم في الصفحة الرئيسية
            frm_Main.Instance.AddAccordionElement();  // إضافة عناصر الواجهة
            frm_Main.Instance.Show();  // عرض الصفحة الرئيسية

            ApplicationExit = false;  // تغيير حالة التطبيق بحيث لا يتم غلقه عند إغلاق الفورم
            this.Close();  // إغلاق نافذة تسجيل الدخول
            SplashScreenManager.CloseForm();  // إغلاق شاشة التحميل
        }

        // حدث عند الضغط على زر إلغاء
        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();  // إغلاق التطبيق
        }

        // حدث عند إغلاق فورم تسجيل الدخول
        private void Frm_Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            // إذا كان يجب إغلاق التطبيق، قم بإغلاقه
            if (ApplicationExit)
                Application.Exit();
        }
    }
}
