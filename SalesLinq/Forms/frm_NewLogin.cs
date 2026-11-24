using System;
using System.Linq;
using System.Windows.Forms;
using Liphsoft.Crypto.Argon2;

namespace SalesLinq.Forms
{
    public partial class frm_NewLogin : NativeMasterForm
    {
        private bool applicationExit = true;

        protected override bool AllowUnauthenticatedSave => true;
        protected override bool AuthorizeNewAction => false;

        public frm_NewLogin()
        {
            InitializeComponent();
            IsNew = true;
            btn_Save.Text = "إنشاء حساب المدير";
            btn_New.Visible = false;
            btn_Delete.Visible = false;
            btn_Print.Visible = false;
            btn_Refresh.Visible = false;
        }

        public override bool IsDataValide()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txt_Name.Text))
            {
                errorProvider.SetError(txt_Name, ErrorText);
                valid = false;
            }
            else errorProvider.SetError(txt_Name, string.Empty);

            if (string.IsNullOrWhiteSpace(txt_UserName.Text))
            {
                errorProvider.SetError(txt_UserName, ErrorText);
                valid = false;
            }
            else errorProvider.SetError(txt_UserName, string.Empty);

            if (string.IsNullOrWhiteSpace(txt_Password.Text))
            {
                errorProvider.SetError(txt_Password, "كلمة المرور مطلوبة.");
                valid = false;
            }
            else errorProvider.SetError(txt_Password, string.Empty);

            return valid;
        }

        public override void Save()
        {
            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction(
                        System.Data.IsolationLevel.Serializable))
                    {
                        db.Transaction = transaction;
                        try
                        {
                            if (db.Users.Any())
                            {
                                transaction.Rollback();
                                MessageBox.Show("يوجد مستخدم مسجل بالفعل. سيتم فتح شاشة تسجيل الدخول.",
                                    "إعداد أول تشغيل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                OpenLogin();
                                return;
                            }

                            var user = new DAL.User
                            {
                                Name = txt_Name.Text.Trim(),
                                UserName = txt_UserName.Text.Trim(),
                                Password = new PasswordHasher().Hash(txt_Password.Text),
                                UserType = (byte)SalesLinq.Class.Master.UserType.Admin,
                                IsActive = true
                            };

                            db.Users.InsertOnSubmit(user);
                            db.SubmitChanges();
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("تم إنشاء حساب المدير بنجاح.", "اكتمل الإعداد",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر إنشاء حساب المدير.\n" + ex.Message,
                    "خطأ في الإعداد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenLogin()
        {
            applicationExit = false;
            var login = new frm_Login();
            login.Show();
            Close();
        }

        private void Frm_NewLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (applicationExit)
                Application.Exit();
        }
    }
}