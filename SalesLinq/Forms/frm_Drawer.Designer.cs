namespace SalesLinq.Forms
{
    partial class frm_Drawer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_Name;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            lbl_Name = new System.Windows.Forms.Label();
            txt_Name = new System.Windows.Forms.TextBox();

            layoutPanel.SuspendLayout();
            SuspendLayout();

            layoutPanel.ColumnCount = 2;
            layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Absolute, 100F));
            layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 100F));
            layoutPanel.Controls.Add(lbl_Name, 0, 0);
            layoutPanel.Controls.Add(txt_Name, 1, 0);
            layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutPanel.Padding = new System.Windows.Forms.Padding(12);
            layoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            layoutPanel.RowCount = 1;
            layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Absolute, 38F));
            layoutPanel.Name = "layoutPanel";

            lbl_Name.AutoSize = true;
            lbl_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_Name.Text = "الاسم";
            lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            txt_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            txt_Name.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            txt_Name.Name = "txt_Name";
            txt_Name.TabIndex = 0;

            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(572, 172);
            Controls.Add(layoutPanel);
            Name = "frm_Drawer";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "إضافة خزنة";

            layoutPanel.ResumeLayout(false);
            layoutPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}