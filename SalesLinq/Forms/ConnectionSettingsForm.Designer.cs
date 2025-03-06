namespace SalesLinq.Forms
{
    partial class ConnectionSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.serverTextBox = new DevExpress.XtraEditors.TextEdit();
            this.databaseTextBox = new DevExpress.XtraEditors.TextEdit();
            this.windowsAuthRadio = new System.Windows.Forms.RadioButton();
            this.SqlServerAuthRadio = new System.Windows.Forms.RadioButton();
            this.btnCheked = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.usernameTextBox = new DevExpress.XtraEditors.TextEdit();
            this.passwordTextBox = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.serverTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(185, 226);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 39);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "حفظ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(311, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "الغاء";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(264, 37);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverTextBox.Properties.Appearance.Options.UseFont = true;
            this.serverTextBox.Size = new System.Drawing.Size(173, 30);
            this.serverTextBox.TabIndex = 2;
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.Location = new System.Drawing.Point(264, 87);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseTextBox.Properties.Appearance.Options.UseFont = true;
            this.databaseTextBox.Size = new System.Drawing.Size(173, 30);
            this.databaseTextBox.TabIndex = 3;
            // 
            // windowsAuthRadio
            // 
            this.windowsAuthRadio.AutoSize = true;
            this.windowsAuthRadio.Checked = true;
            this.windowsAuthRadio.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowsAuthRadio.Location = new System.Drawing.Point(412, 144);
            this.windowsAuthRadio.Name = "windowsAuthRadio";
            this.windowsAuthRadio.Size = new System.Drawing.Size(151, 27);
            this.windowsAuthRadio.TabIndex = 4;
            this.windowsAuthRadio.TabStop = true;
            this.windowsAuthRadio.Text = "Windows Auth";
            this.windowsAuthRadio.UseVisualStyleBackColor = true;
            this.windowsAuthRadio.CheckedChanged += new System.EventHandler(this.windowsAuthRadio_CheckedChanged);
            // 
            // SqlServerAuthRadio
            // 
            this.SqlServerAuthRadio.AutoSize = true;
            this.SqlServerAuthRadio.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SqlServerAuthRadio.Location = new System.Drawing.Point(405, 182);
            this.SqlServerAuthRadio.Name = "SqlServerAuthRadio";
            this.SqlServerAuthRadio.Size = new System.Drawing.Size(158, 27);
            this.SqlServerAuthRadio.TabIndex = 5;
            this.SqlServerAuthRadio.Text = "Sql Server Auth";
            this.SqlServerAuthRadio.UseVisualStyleBackColor = true;
            this.SqlServerAuthRadio.CheckedChanged += new System.EventHandler(this.windowsAuthRadio_CheckedChanged);
            // 
            // btnCheked
            // 
            this.btnCheked.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheked.Appearance.Options.UseFont = true;
            this.btnCheked.Location = new System.Drawing.Point(17, 35);
            this.btnCheked.Name = "btnCheked";
            this.btnCheked.Size = new System.Drawing.Size(198, 34);
            this.btnCheked.TabIndex = 6;
            this.btnCheked.Text = "تحقق من الاتصال بالسيرفر";
            this.btnCheked.Click += new System.EventHandler(this.btnCheked_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(502, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(61, 23);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "السيرفر :";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(464, 90);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(99, 23);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "قاعدة البيانات :";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Enabled = false;
            this.usernameTextBox.Location = new System.Drawing.Point(17, 144);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(198, 26);
            this.usernameTextBox.TabIndex = 9;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Enabled = false;
            this.passwordTextBox.Location = new System.Drawing.Point(17, 182);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(198, 26);
            this.passwordTextBox.TabIndex = 10;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Enabled = false;
            this.labelControl3.Location = new System.Drawing.Point(248, 146);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(95, 23);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "اسم المستخدم :";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Enabled = false;
            this.labelControl4.Location = new System.Drawing.Point(254, 184);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(89, 23);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "الرقم السري :";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 262);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(102, 19);
            this.labelVersion.TabIndex = 11;
            this.labelVersion.Text = "Version 1.0.0.0";
            // 
            // ConnectionSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 290);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCheked);
            this.Controls.Add(this.SqlServerAuthRadio);
            this.Controls.Add(this.windowsAuthRadio);
            this.Controls.Add(this.databaseTextBox);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(590, 322);
            this.MinimumSize = new System.Drawing.Size(590, 322);
            this.Name = "ConnectionSettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اعدادات الاتصال بالسيرفر";
            ((System.ComponentModel.ISupportInitialize)(this.serverTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextBox.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit serverTextBox;
        private DevExpress.XtraEditors.TextEdit databaseTextBox;
        private System.Windows.Forms.RadioButton windowsAuthRadio;
        private System.Windows.Forms.RadioButton SqlServerAuthRadio;
        private DevExpress.XtraEditors.SimpleButton btnCheked;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit usernameTextBox;
        private DevExpress.XtraEditors.TextEdit passwordTextBox;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Label labelVersion;
    }
}