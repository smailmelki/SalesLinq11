namespace SalesLinq.Forms
{
    partial class frm_UserSettingsProfileList
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
            this.SuspendLayout();
            // 
            // frm_UserSettingsProfileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 554);
            this.Name = "frm_UserSettingsProfileList";
            this.Text = "نماذج اعدادات المستخدمين";
            this.Load += new System.EventHandler(this.Frm_UserSettingsProfileList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}