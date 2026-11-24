namespace SalesLinq.Forms
{
    partial class frm_DrawerList
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
            SuspendLayout();

            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(528, 536);
            Margin = new System.Windows.Forms.Padding(6);
            Name = "frm_DrawerList";
            Text = "قائمة الخزن";
            Load += new System.EventHandler(Frm_DrawerList_Load);

            ResumeLayout(false);
        }
    }
}