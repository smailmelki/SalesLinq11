using SalesLinq.Class;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class ConnectionSettingsForm : Form
    {
        private enum Authentication
        {
            WindowsAuthentication,
            SqlServerAuthentication
        }

        public ConnectionSettingsForm()
        {
            InitializeComponent();
        }

        private bool UsesWindowsAuthentication =>
            comboBoxAuth.SelectedIndex == (int)Authentication.WindowsAuthentication;

        private string BuildSelectedConnectionString(string databaseName)
        {
            return DataBaseClass.BuildConnectionString(
                textEditServer.Text,
                databaseName,
                UsesWindowsAuthentication,
                textEditUsername.Text,
                textEditPassword.Text);
        }

        private void ComboBoxAuth_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bool sqlAuthentication = !UsesWindowsAuthentication;
            textEditUsername.Enabled = sqlAuthentication;
            textEditPassword.Enabled = sqlAuthentication;
        }

        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            comboBoxAuth.Items.Clear();
            comboBoxAuth.Items.Add(Authentication.WindowsAuthentication.ToString());
            comboBoxAuth.Items.Add(Authentication.SqlServerAuthentication.ToString());

            textEditServer.Text = Properties.Settings.Default.ServerName;
            textEditDataBase.Text = Properties.Settings.Default.DataBase;

            string savedConnection = Properties.Settings.Default.SalesDBConnectionString ?? string.Empty;
            bool windowsAuthentication =
                savedConnection.IndexOf("Integrated Security=True", StringComparison.OrdinalIgnoreCase) >= 0 ||
                savedConnection.IndexOf("Integrated Security=SSPI", StringComparison.OrdinalIgnoreCase) >= 0;

            comboBoxAuth.SelectedIndex = windowsAuthentication
                ? (int)Authentication.WindowsAuthentication
                : (int)Authentication.SqlServerAuthentication;

            textEditUsername.Text = Properties.Settings.Default.UserName;
            textEditPassword.Text = Properties.Settings.Default.Password;
            comboBoxAuth.SelectedIndexChanged += ComboBoxAuth_OnSelectedIndexChanged;

            labelControlVersion.Text = "الإصدار: " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        private async void btnConectionChek_Click(object sender, EventArgs e)
        {
            if (!ValidateConnectionInputs())
                return;

            btnConectionChek.Enabled = false;
            try
            {
                string connectionString = BuildSelectedConnectionString(textEditDataBase.Text);
                string error = null;
                bool connected = await Task.Run(() =>
                    DataBaseClass.TestDatabaseConnection(connectionString, out error));

                MessageBox.Show(
                    connected ? "تم الاتصال بقاعدة البيانات بنجاح." : "تعذر الاتصال بقاعدة البيانات.\n" + error,
                    connected ? "نجاح" : "فشل الاتصال",
                    MessageBoxButtons.OK,
                    connected ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "بيانات الاتصال غير صحيحة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnConectionChek.Enabled = true;
            }
        }

        private bool ValidateConnectionInputs()
        {
            if (string.IsNullOrWhiteSpace(textEditServer.Text) ||
                string.IsNullOrWhiteSpace(textEditDataBase.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الخادم وقاعدة البيانات.",
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textEditServer.Focus();
                return false;
            }

            if (!UsesWindowsAuthentication &&
                (string.IsNullOrWhiteSpace(textEditUsername.Text) ||
                 string.IsNullOrEmpty(textEditPassword.Text)))
            {
                MessageBox.Show("يرجى إدخال اسم مستخدم SQL Server وكلمة المرور.",
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textEditUsername.Focus();
                return false;
            }

            return true;
        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (!ValidateConnectionInputs())
                return;

            btn_Save.Enabled = false;
            btnConectionChek.Enabled = false;

            try
            {
                // نختبر الخادم باستخدام قاعدة master حتى يمكن إعداد قاعدة جديدة.
                string serverConnection = BuildSelectedConnectionString("master");
                string error = null;
                bool connected = await Task.Run(() =>
                    DataBaseClass.TestDatabaseConnection(serverConnection, out error));

                if (!connected)
                {
                    MessageBox.Show("فشل الاتصال بالخادم.\n" + error,
                        "فشل الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string databaseConnection = BuildSelectedConnectionString(textEditDataBase.Text);

                Properties.Settings.Default.ServerName = textEditServer.Text.Trim();
                Properties.Settings.Default.DataBase = textEditDataBase.Text.Trim();
                Properties.Settings.Default.UserName = UsesWindowsAuthentication ? string.Empty : textEditUsername.Text.Trim();
                Properties.Settings.Default.Password = UsesWindowsAuthentication ? string.Empty : textEditPassword.Text;
                Properties.Settings.Default.SalesDBConnectionString = databaseConnection;
                Properties.Settings.Default.Save();

                await Task.Run(() =>
                {
                    using (var db = new DAL.DBDataContext())
                    {
                        if (!db.DatabaseExists())
                            db.CreateDatabase();
                    }
                });

                MessageBox.Show("تم حفظ إعدادات الاتصال بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حفظ إعدادات الاتصال أو تهيئة قاعدة البيانات.\n" + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_Save.Enabled = true;
                btnConectionChek.Enabled = true;
            }
        }
    }
}