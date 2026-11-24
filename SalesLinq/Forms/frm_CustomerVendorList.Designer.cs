namespace SalesLinq.Forms
{
    partial class frm_CustomerVendorList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_CustomerVendorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 580);
            this.MinimumSize = new System.Drawing.Size(750, 450);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frm_CustomerVendorList";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "قائمة العملاء والموردين";
            this.Load += new System.EventHandler(this.Frm_CustomerVendorList_Load);
            this.ResumeLayout(false);
        }
    }
}