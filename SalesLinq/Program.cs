using SalesLinq.Class;
using SalesLinq.Properties;
using System;
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
            string connectionString = Settings.Default.SalesDBConnectionString;
            string connectionError;
            if (!string.IsNullOrWhiteSpace(connectionString) &&
                DataBaseClass.TestDatabaseConnection(connectionString, out connectionError))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                using (var db = new DAL.DBDataContext())
                {
                    if (db.Users.Any())//في حالة كانت قاعدة البيانات لا تحتوي على أي مستخدم 
                    {
                        var frm = new Forms.frm_Login();
                        frm.Show();
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
