using DevExpress.LookAndFeel;
using SalesLinq.Class;
using SalesLinq.Properties;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (DataBaseClass.TestDatabaseConnection(Settings.Default.ServerName,Settings.Default.DataBase))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                UserLookAndFeel.Default.SkinName = Settings.Default.SkinName.ToString();                                            // تحميل سمة البرنامج
                UserLookAndFeel.Default.SetSkinStyle(Settings.Default.SkinName.ToString(), Settings.Default.PaletteName.ToString());// تحميل سمة البرنامج

                using (var db = new DAL.DBDataContext())
                {
                    if (db.Users.Any())//في حالة كانت قاعدة البيانات لا تحتوي على أي مستخدم 
                    {
                        var frm = new Forms.frm_Login();
                        frm.Show();
                        if (Debugger.IsAttached)
                        {
                            frm.txt_UserName.Text = "admin";
                            frm.txt_UserPassword.Text = "admin";
                            frm.Btn_LogIn_Click(null, null);
                        }
                    }
                    else
                    {
                        var frm = new Forms.frm_NewLogin();// فتح فورم تسجيل أول مستخدم
                        frm.Show();
                    }
                }
                Application.Run();
                //Application.Run(Forms.frm_Main.Instance);
            }
            else
            {
                var settingsForm = new Forms.ConnectionSettingsForm();
                settingsForm.ShowDialog();
            }
        }
    }
}
