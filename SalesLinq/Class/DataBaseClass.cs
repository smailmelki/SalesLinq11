using System;
using System.Data.SqlClient;

namespace SalesLinq.Class
{
    public static class DataBaseClass
    {
        /// <summary>
        /// ينشئ جملة اتصال آمنة مع دعم مصادقة Windows ومصادقة SQL Server.
        /// </summary>
        public static string BuildConnectionString(
            string serverName,
            string databaseName,
            bool useWindowsAuthentication,
            string userName = null,
            string password = null)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                throw new ArgumentException("يجب إدخال اسم الخادم.", nameof(serverName));

            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("يجب إدخال اسم قاعدة البيانات.", nameof(databaseName));

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = serverName.Trim(),
                InitialCatalog = databaseName.Trim(),
                IntegratedSecurity = useWindowsAuthentication,
                TrustServerCertificate = true,
                ConnectTimeout = 5,
                ApplicationName = "SalesLinq"
            };

            if (!useWindowsAuthentication)
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentException("يجب إدخال اسم مستخدم SQL Server.", nameof(userName));

                if (string.IsNullOrEmpty(password))
                    throw new ArgumentException("يجب إدخال كلمة مرور SQL Server.", nameof(password));

                builder.UserID = userName.Trim();
                builder.Password = password;
                builder.PersistSecurityInfo = false;
            }

            return builder.ConnectionString;
        }

        /// <summary>
        /// يختبر جملة الاتصال نفسها التي سيستخدمها التطبيق.
        /// </summary>
        public static bool TestDatabaseConnection(string connectionString, out string errorMessage)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }

                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// توافق خلفي مع الاستدعاءات القديمة التي تستخدم مصادقة Windows.
        /// </summary>
        public static bool TestDatabaseConnection(string serverName, string dataBase = "master")
        {
            try
            {
                var connectionString = BuildConnectionString(serverName, dataBase, true);
                return TestDatabaseConnection(connectionString, out _);
            }
            catch
            {
                return false;
            }
        }
    }
}