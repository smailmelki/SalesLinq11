using System;
using System.Data.SqlClient;

namespace SalesLinq.Class
{
    public static class DataBaseClass
    {

        /// <summary>
        /// دالة تتحقق من صحة جملة الاتصال بقاعدة البيانات
        /// </summary>
        public static bool TestDatabaseConnection(string database, string serverName, string UserName = null, string Password = null)
        {
            string connectionString;
            // الحصول على جملة الاتصال من ملف الإعدادات
            if (UserName != null && Password != null)
            {
                connectionString = $"Data Source={serverName};Initial Catalog={database};User ID={UserName};Password={Password};";
            }
            else
            {
                connectionString = $"Data Source={serverName};Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True";
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true; // تم الاتصال بنجاح
                }
            }
            catch(Exception ex) 
            {               
                return false; // فشل الاتصال
            }
        }
        /// <summary>
        /// دالة تعرض نافذة إعدادات الاتصال
        /// </summary>
        public static void ShowConnectionSettingsForm()
        {
            // يمكنك هنا إنشاء نافذة جديدة لإعدادات الاتصال
            var settingsForm = new Forms.ConnectionSettingsForm();

            // عرض النافذة كحوار مشروط - البرنامج ينتظر حتى يتم إغلاق هذه النافذة
            settingsForm.ShowDialog();
        }
    }
}
