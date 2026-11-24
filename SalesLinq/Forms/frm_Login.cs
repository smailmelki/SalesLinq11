using System;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.DAL;

namespace SalesLinq.Forms
{
    // شاشة تسجيل الدخول
    public partial class frm_Login : Form
    {
        private bool applicationExit = true;

        public frm_Login()
        {
            InitializeComponent();
        }

        public void Btn_LogIn_Click(object sender, EventArgs e)
        {
            string userName = txt_UserName.Text.Trim();
            string passWord = txt_UserPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(passWord))
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور.",
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new DBDataContext())
                {
                    if (!db.Users.Any())
                    {
                        MessageBox.Show("لا يوجد مستخدمون مسجلون.",
                            "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var user = db.Users.SingleOrDefault(x => x.UserName == userName);
                    if (user == null || !VerifyPassword(user, passWord))
                    {
                        MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة.",
                            "فشل تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_UserPassword.Clear();
                        txt_UserPassword.Focus();
                        return;
                    }

                    if (!user.IsActive)
                    {
                        MessageBox.Show("هذا الحساب معطّل. يرجى مراجعة مدير النظام.",
                            "الحساب معطّل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    HandleSuccessfulLogin(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر إتمام تسجيل الدخول.\n" + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool VerifyPassword(User user, string passWord)
        {
            var hasher = new Liphsoft.Crypto.Argon2.PasswordHasher();
            return hasher.Verify(user.Password, passWord);
        }

        private void HandleSuccessfulLogin(User user)
        {
            Cursor previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Class.Session.SetUser(user);

                // تحميل خصائص الجلسة التي تعتمد على قاعدة البيانات.
                Type sessionType = typeof(Class.Session);
                var properties = sessionType.GetProperties(
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Static);

                foreach (var property in properties)
                    property.GetValue(null);

                frm_Main.Instance.Lbl_UserName.Text = user.Name;
                frm_Main.Instance.AddAccordionElement();
                frm_Main.Instance.Show();

                applicationExit = false;
                Close();
            }
            finally
            {
                Cursor.Current = previousCursor;
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Frm_Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (applicationExit)
                Application.Exit();
        }
    }
}