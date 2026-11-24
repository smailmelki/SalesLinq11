namespace SalesLinq.Forms
{
    partial class frm_NewLogin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.TextBox txt_Password;
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
            txt_Name = new System.Windows.Forms.TextBox();
            txt_UserName = new System.Windows.Forms.TextBox();
            txt_Password = new System.Windows.Forms.TextBox();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.Padding = new System.Windows.Forms.Padding(16);
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.RowCount = 3;
            for (int i = 0; i < 3; i++)
                layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            layout.Controls.Add(lbl_Name, 0, 0);
            layout.Controls.Add(txt_Name, 1, 0);
            layout.Controls.Add(lbl_UserName, 0, 1);
            layout.Controls.Add(txt_UserName, 1, 1);
            layout.Controls.Add(lbl_Password, 0, 2);
            layout.Controls.Add(txt_Password, 1, 2);

            ConfigureLabel(lbl_Name, "اسم المدير");
            ConfigureLabel(lbl_UserName, "اسم الدخول");
            ConfigureLabel(lbl_Password, "كلمة المرور");
            ConfigureTextBox(txt_Name);
            ConfigureTextBox(txt_UserName);
            ConfigureTextBox(txt_Password);
            txt_Password.UseSystemPasswordChar = true;

            errorProvider.ContainerControl = this;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(480, 230);
            Controls.Add(layout);
            Name = "frm_NewLogin";
            Text = "إنشاء حساب المدير الأول";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;
            FormClosed += new System.Windows.Forms.FormClosedEventHandler(Frm_NewLogin_FormClosed);

            layout.ResumeLayout(false);
            layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
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
    }
}