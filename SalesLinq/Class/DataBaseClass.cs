using System;
using System.Data.SqlClient;

namespace SalesLinq.Class
{
    public static class DataBaseClass
    {

        /// <summary>
        /// دالة تتحقق من صحة جملة الاتصال بقاعدة البيانات
        /// </summary>
        public static bool TestDatabaseConnection(string serverName , string dataBase = "master")
        {
            // الحصول على جملة الاتصال من ملف الإعدادات
            string connectionString = $"Data Source={serverName};Initial Catalog={dataBase};Integrated Security=True;TrustServerCertificate=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true; // تم الاتصال بنجاح
                }
            }
            catch(Exception) 
            {               
                return false; // فشل الاتصال
            }
        }
    }
}
