using DevExpress.XtraEditors;
using SalesLinq.Class;
using System;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class ConnectionSettingsForm: XtraForm
    {
        public ConnectionSettingsForm()
        {
            InitializeComponent();
            // تعبئة الحقول بالقيم المحفوظة
            serverTextBox.Text = Properties.Settings.Default.ServerName;
            databaseTextBox.Text = Properties.Settings.Default.DataBase;
            if (Properties.Settings.Default.SalesDBConnectionString.Contains("Integrated Security=True"))
            {
                windowsAuthRadio.Checked = true;
            }
            else
            {
                windowsAuthRadio.Checked = false;
                usernameTextBox.Text = Properties.Settings.Default.UserName;
                passwordTextBox.Text = Properties.Settings.Default.Password;
            }
        }

        // في نافذة ConnectionSettingsForm
        private void btnSave_Click(object sender, EventArgs e)
        {
            // بناء جملة الاتصال من القيم المدخلة
            string connectionString = $"Data Source={serverTextBox.Text};Initial Catalog={databaseTextBox.Text};";

            if (windowsAuthRadio.Checked)
                connectionString += "Integrated Security=True;";
            else
                connectionString += $"User ID={usernameTextBox.Text};Password={passwordTextBox.Text};";

            // حفظ جملة الاتصال في ملف الإعدادات
            Properties.Settings.Default.DataBase = databaseTextBox.Text;
            Properties.Settings.Default.ServerName = serverTextBox.Text;
            Properties.Settings.Default.UserName = usernameTextBox.Text;
            Properties.Settings.Default.Password = passwordTextBox.Text;
            Properties.Settings.Default.SalesDBConnectionString = connectionString;
            if (checkConnection())
            {
                Properties.Settings.Default.Save();
                Application.Restart();
            }
            else
            {
                XtraMessageBox.Show("فشل الاتصال", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCheked_Click(object sender, EventArgs e)
        {
            if (checkConnection())
            {
                XtraMessageBox.Show("تم الاتصال بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                XtraMessageBox.Show("فشل الاتصال", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        bool checkConnection()
        {
            if (windowsAuthRadio.Checked)
                return DataBaseClass.TestDatabaseConnection(databaseTextBox.Text, serverTextBox.Text);
            else
                return DataBaseClass.TestDatabaseConnection(databaseTextBox.Text, serverTextBox.Text, usernameTextBox.Text, passwordTextBox.Text);
        }

        private void windowsAuthRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (windowsAuthRadio.Checked)
            {
                usernameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
            }
            else
            {
                usernameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
            }
        }

        private void SqlServerAuthRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (SqlServerAuthRadio.Checked)
            {
                usernameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
            }
            else
            {
                usernameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
            }
        }
    }
}
