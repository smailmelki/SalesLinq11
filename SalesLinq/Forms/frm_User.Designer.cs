namespace SalesLinq.Forms
{
    partial class frm_User
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.Label lbl_UserType;
        private System.Windows.Forms.Label lbl_SettingProfile;
        private System.Windows.Forms.Label lbl_ScreenProfile;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.ComboBox cmb_UserType;
        private System.Windows.Forms.ComboBox cmb_SettingProfile;
        private System.Windows.Forms.ComboBox cmb_ScreenProfile;
        private System.Windows.Forms.CheckBox chk_IsActive;
        private System.Windows.Forms.ErrorProvider errorProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            layout = new System.Windows.Forms.TableLayoutPanel();
            lbl_Name = new System.Windows.Forms.Label();
            lbl_UserName = new System.Windows.Forms.Label();
            lbl_Password = new System.Windows.Forms.Label();
            lbl_UserType = new System.Windows.Forms.Label();
            lbl_SettingProfile = new System.Windows.Forms.Label();
            lbl_ScreenProfile = new System.Windows.Forms.Label();
            txt_Name = new System.Windows.Forms.TextBox();
            txt_UserName = new System.Windows.Forms.TextBox();
            txt_Password = new System.Windows.Forms.TextBox();
            cmb_UserType = new System.Windows.Forms.ComboBox();
            cmb_SettingProfile = new System.Windows.Forms.ComboBox();
            cmb_ScreenProfile = new System.Windows.Forms.ComboBox();
            chk_IsActive = new System.Windows.Forms.CheckBox();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(errorProvider)).BeginInit();
            SuspendLayout();
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72F));
            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.Padding = new System.Windows.Forms.Padding(16);
            layout.RowCount = 7;
            for (int i = 0; i < 7; i++)
                layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            layout.Controls.Add(lbl_Name, 0, 0);
            layout.Controls.Add(txt_Name, 1, 0);
            layout.Controls.Add(lbl_UserName, 0, 1);
            layout.Controls.Add(txt_UserName, 1, 1);
            layout.Controls.Add(lbl_Password, 0, 2);
            layout.Controls.Add(txt_Password, 1, 2);
            layout.Controls.Add(lbl_UserType, 0, 3);
            layout.Controls.Add(cmb_UserType, 1, 3);
            layout.Controls.Add(lbl_SettingProfile, 0, 4);
            layout.Controls.Add(cmb_SettingProfile, 1, 4);
            layout.Controls.Add(lbl_ScreenProfile, 0, 5);
            layout.Controls.Add(cmb_ScreenProfile, 1, 5);
            layout.Controls.Add(chk_IsActive, 1, 6);
            ConfigureLabel(lbl_Name, "الاسم");
            ConfigureLabel(lbl_UserName, "اسم الدخول");
            ConfigureLabel(lbl_Password, "كلمة المرور");
            ConfigureLabel(lbl_UserType, "نوع الدخول");
            ConfigureLabel(lbl_SettingProfile, "نموذج الإعدادات");
            ConfigureLabel(lbl_ScreenProfile, "نموذج الوصول");
            ConfigureTextBox(txt_Name);
            ConfigureTextBox(txt_UserName);
            ConfigureTextBox(txt_Password);
            txt_Password.UseSystemPasswordChar = true;
            ConfigureCombo(cmb_UserType);
            ConfigureCombo(cmb_SettingProfile);
            ConfigureCombo(cmb_ScreenProfile);
            chk_IsActive.AutoSize = true;
            chk_IsActive.Checked = true;
            chk_IsActive.Text = "مستخدم نشط";
            chk_IsActive.Anchor = System.Windows.Forms.AnchorStyles.Right;
            errorProvider.ContainerControl = this;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(520, 340);
            Controls.Add(layout);
            Name = "frm_User";
            Text = "بيانات المستخدم";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;
            layout.ResumeLayout(false);
            layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(errorProvider)).EndInit();
            ResumeLayout(false);
            toolStrip.BringToFront();
        }

        private static void ConfigureLabel(System.Windows.Forms.Label label, string text)
        {
            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.Text = text;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label.AutoSize = true;
        }

        private static void ConfigureTextBox(System.Windows.Forms.TextBox textBox)
        {
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
        }

        private static void ConfigureCombo(System.Windows.Forms.ComboBox combo)
        {
            combo.Dock = System.Windows.Forms.DockStyle.Fill;
            combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
        }
    }
}