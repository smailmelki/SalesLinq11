namespace SalesLinq.Forms
{
    partial class ConnectionSettingsForm
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
            this.textEditServer = new System.Windows.Forms.TextBox();
            this.textEditDataBase = new System.Windows.Forms.TextBox();
            this.comboBoxAuth = new System.Windows.Forms.ComboBox();
            this.textEditUsername = new System.Windows.Forms.TextBox();
            this.textEditPassword = new System.Windows.Forms.TextBox();
            this.btnConectionChek = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.labelControlVersion = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelServer = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.labelAuthentication = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelServer, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textEditServer, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelDatabase, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textEditDataBase, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelAuthentication, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxAuth, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelUsername, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textEditUsername, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.labelPassword, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textEditPassword, 1, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(520, 234);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labels
            // 
            this.labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelServer.Text = "اسم الخادم:";
            this.labelServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Text = "قاعدة البيانات:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAuthentication.Text = "نوع المصادقة:";
            this.labelAuthentication.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUsername.Text = "اسم المستخدم:";
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.Text = "كلمة المرور:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputs
            // 
            this.textEditServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditServer.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textEditDataBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditDataBase.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.comboBoxAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAuth.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textEditUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditUsername.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textEditPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditPassword.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textEditPassword.UseSystemPasswordChar = true;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.btnConectionChek);
            this.flowLayoutPanel.Controls.Add(this.btn_Save);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.flowLayoutPanel.Size = new System.Drawing.Size(520, 48);
            this.flowLayoutPanel.TabIndex = 1;
            this.btnConectionChek.Text = "اختبار الاتصال";
            this.btnConectionChek.AutoSize = true;
            this.btnConectionChek.MinimumSize = new System.Drawing.Size(130, 32);
            this.btnConectionChek.Click += new System.EventHandler(this.btnConectionChek_Click);
            this.btn_Save.Text = "حفظ الإعدادات";
            this.btn_Save.AutoSize = true;
            this.btn_Save.MinimumSize = new System.Drawing.Size(130, 32);
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // labelControlVersion
            // 
            this.labelControlVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelControlVersion.Padding = new System.Windows.Forms.Padding(12, 0, 12, 8);
            this.labelControlVersion.Size = new System.Drawing.Size(520, 28);
            this.labelControlVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConnectionSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 320);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.labelControlVersion);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionSettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعدادات الاتصال بقاعدة البيانات";
            this.Load += new System.EventHandler(this.ConnectionSettingsForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox textEditServer;
        private System.Windows.Forms.TextBox textEditDataBase;
        private System.Windows.Forms.ComboBox comboBoxAuth;
        private System.Windows.Forms.TextBox textEditUsername;
        private System.Windows.Forms.TextBox textEditPassword;
        private System.Windows.Forms.Button btnConectionChek;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label labelControlVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label labelAuthentication;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
    }
}