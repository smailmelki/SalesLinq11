using DevExpress.XtraEditors;
using SalesLinq.Class;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class ConnectionSettingsForm : XtraForm
    {
        public ConnectionSettingsForm()
        {
            InitializeComponent();
        }

        private void ComboBoxAuth_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAuth.SelectedIndex == (int)Authentication.WindowsAuthentication)
            {
                textEditUsername.Enabled =
                textEditPassword.Enabled = false;
            }
            else
            {
                textEditUsername.Enabled =
                textEditPassword.Enabled = true;
            }
        }

        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            ///////////////////////////////////
            comboBoxAuth.Properties.Items.Add(Authentication.WindowsAuthentication.ToString());
            comboBoxAuth.Properties.Items.Add(Authentication.SqlServerAuthentication.ToString());
            comboBoxAuth.SelectedIndexChanged += ComboBoxAuth_OnSelectedIndexChanged;
            ///////////////////////////////////
            // تعبئة الحقول بالقيم المحفوظة
            textEditServer.Text = Properties.Settings.Default.ServerName;
            textEditDataBase.Text = Properties.Settings.Default.DataBase;
            if (Properties.Settings.Default.SalesDBConnectionString.Contains("Integrated Security=True"))
            {
                comboBoxAuth.SelectedIndex = (int)Authentication.WindowsAuthentication;
            }
            else
            {
                comboBoxAuth.SelectedIndex = (int)Authentication.SqlServerAuthentication;
                textEditUsername.Text = Properties.Settings.Default.UserName;
                textEditPassword.Text = Properties.Settings.Default.Password;
            }
            this.labelControlVersion.Text = "الاصدار : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private async void btnConectionChek_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textEditServer.Text.Trim()))
            {
                if (await Task.Run(() => DataBaseClass.TestDatabaseConnection(textEditServer.Text)))
                {
                    XtraMessageBox.Show("تم الاتصال بالسيرفر", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    XtraMessageBox.Show("فشل الاتصال بالسيرفر", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textEditServer.Text.Trim()) || string.IsNullOrWhiteSpace(textEditDataBase.Text.Trim()))
            {
                XtraMessageBox.Show("الرجاء ادخال اسم السيرفر وقاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textEditServer.Focus();
                return;
            }

            btn_Save.Enabled = false; // تعطيل الزر أثناء العملية لمنع التكرار
            try
            {
                string connectionString = $"Data Source={textEditServer.Text};Initial Catalog={textEditDataBase.Text};";
                if (comboBoxAuth.SelectedIndex == 1)
                {
                    if (string.IsNullOrWhiteSpace(textEditUsername.Text.Trim()) || string.IsNullOrWhiteSpace(textEditPassword.Text.Trim()))
                    {
                        XtraMessageBox.Show("الرجاء ادخال اسم المستخدم وكلمة المرور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textEditUsername.Focus();
                        return;
                    }
                    connectionString += $"User ID={textEditUsername.Text};Password={textEditPassword.Text};";
                }
                else
                {
                    connectionString += "Integrated Security=True;";
                }

                if (await Task.Run(() => DataBaseClass.TestDatabaseConnection(textEditServer.Text)))
                {
                    Properties.Settings.Default.DataBase = textEditDataBase.Text;
                    Properties.Settings.Default.ServerName = textEditServer.Text;
                    Properties.Settings.Default.UserName = textEditUsername.Text;
                    Properties.Settings.Default.Password = textEditPassword.Text;
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
                btn_Save.Enabled = true; // إعادة تمكين الزر بعد انتهاء العملية
            }
        }
    }

    enum Authentication
    {
        WindowsAuthentication,
        SqlServerAuthentication
    }
}
