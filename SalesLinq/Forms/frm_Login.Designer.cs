namespace SalesLinq.Forms
{
    partial class frm_Login
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.txt_UserPassword = new System.Windows.Forms.TextBox();
            this.btn_LogIn = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.lbl_UserPassword = new System.Windows.Forms.Label();
            this.lbl_Intro = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lbl_Intro, 0, 0);
            this.tableLayoutPanel.SetColumnSpan(this.lbl_Intro, 2);
            this.tableLayoutPanel.Controls.Add(this.lbl_UserName, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.txt_UserName, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lbl_UserPassword, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.txt_UserPassword, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(28, 36, 28, 18);
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lbl_Intro
            // 
            this.lbl_Intro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Intro.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Intro.Text = "مرحباً بك\r\nأدخل بيانات الدخول للمتابعة";
            this.lbl_Intro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labels
            // 
            this.lbl_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_UserName.Text = "اسم المستخدم";
            this.lbl_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_UserPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_UserPassword.Text = "كلمة المرور";
            this.lbl_UserPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // text boxes
            // 
            this.txt_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_UserName.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.txt_UserPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_UserPassword.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.txt_UserPassword.UseSystemPasswordChar = true;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.btn_LogIn);
            this.buttonPanel.Controls.Add(this.btn_Cancel);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(28, 6, 28, 18);
            this.buttonPanel.Size = new System.Drawing.Size(475, 62);
            this.buttonPanel.TabIndex = 1;
            this.btn_LogIn.Text = "دخول";
            this.btn_LogIn.MinimumSize = new System.Drawing.Size(120, 34);
            this.btn_LogIn.Click += new System.EventHandler(this.Btn_LogIn_Click);
            this.btn_Cancel.Text = "إلغاء";
            this.btn_Cancel.MinimumSize = new System.Drawing.Size(120, 34);
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // frm_Login
            // 
            this.AcceptButton = this.btn_LogIn;
            this.CancelButton = this.btn_Cancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 360);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Login";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تسجيل الدخول";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_Login_FormClosed);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.TextBox txt_UserPassword;
        private System.Windows.Forms.Button btn_LogIn;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_UserPassword;
        private System.Windows.Forms.Label lbl_Intro;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
    }
}