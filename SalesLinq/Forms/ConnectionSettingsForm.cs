using DevExpress.XtraEditors;
using SalesLinq.Class;
using System;
using System.Data.Sql;
using System.Data;
using System.Threading.Tasks;
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
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(serverTextBox.Text.Trim()) || string.IsNullOrWhiteSpace(databaseTextBox.Text.Trim()))
            {
                XtraMessageBox.Show("الرجاء ادخال اسم السيرفر وقاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                serverTextBox.Focus();
                return;
            }

            btnSave.Enabled = false; // تعطيل الزر أثناء العملية لمنع التكرار
            try
            {
                string connectionString = $"Data Source={serverTextBox.Text};Initial Catalog={databaseTextBox.Text};";
                connectionString += windowsAuthRadio.Checked ? "Integrated Security=True;" : $"User ID={usernameTextBox.Text};Password={passwordTextBox.Text};";

                if (await Task.Run(() => DataBaseClass.TestDatabaseConnection(serverTextBox.Text)))
                {
                    Properties.Settings.Default.DataBase = databaseTextBox.Text;
                    Properties.Settings.Default.ServerName = serverTextBox.Text;
                    Properties.Settings.Default.UserName = usernameTextBox.Text;
                    Properties.Settings.Default.Password = passwordTextBox.Text;
                    Properties.Settings.Default.SalesDBConnectionString = connectionString;
                    Properties.Settings.Default.Save();

                    await Task.Run(() =>
                    {
                        using (var db = new DAL.DBDataContext())
                        {
                            if (!db.DatabaseExists())
                            {
                                try
                                {
                                    db.CreateDatabase();
                                    Invoke(new Action(() =>
                                    {
                                        XtraMessageBox.Show("تم انشاء قاعدة البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }));
                                }
                                catch (Exception)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        XtraMessageBox.Show("حدث خطأ أثناء انشاء قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }));
                                }
                            }
                        }
                    });

                    Application.Restart();
                }
                else
                {
                    XtraMessageBox.Show("فشل الاتصال", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                btnSave.Enabled = true; // إعادة تمكين الزر بعد انتهاء العملية
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnCheked_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(serverTextBox.Text.Trim()))
            {
                if (await Task.Run(() => DataBaseClass.TestDatabaseConnection(serverTextBox.Text)))
                {
                    XtraMessageBox.Show("تم الاتصال بالسيرفر", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    XtraMessageBox.Show("فشل الاتصال بالسيرفر", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
