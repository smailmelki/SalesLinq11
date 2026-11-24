namespace SalesLinq.Forms
{
    partial class frm_CustomerVendor
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.lbl_Phone = new System.Windows.Forms.Label();
            this.txt_Phone = new System.Windows.Forms.TextBox();
            this.lbl_Mobile = new System.Windows.Forms.Label();
            this.txt_Mobile = new System.Windows.Forms.TextBox();
            this.lbl_Address = new System.Windows.Forms.Label();
            this.txt_Address = new System.Windows.Forms.TextBox();
            this.lbl_AccountID = new System.Windows.Forms.Label();
            this.txt_AccountID = new System.Windows.Forms.TextBox();
            this.lbl_MaxCredit = new System.Windows.Forms.Label();
            this.num_MaxCredit = new System.Windows.Forms.NumericUpDown();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_MaxCredit)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Padding = new System.Windows.Forms.Padding(18);
            this.mainLayout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainLayout.RowCount = 6;
            for (int i = 0; i < 6; i++)
                this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainLayout.Controls.Add(this.lbl_Name, 0, 0);
            this.mainLayout.Controls.Add(this.txt_Name, 1, 0);
            this.mainLayout.Controls.Add(this.lbl_Phone, 0, 1);
            this.mainLayout.Controls.Add(this.txt_Phone, 1, 1);
            this.mainLayout.Controls.Add(this.lbl_Mobile, 0, 2);
            this.mainLayout.Controls.Add(this.txt_Mobile, 1, 2);
            this.mainLayout.Controls.Add(this.lbl_Address, 0, 3);
            this.mainLayout.Controls.Add(this.txt_Address, 1, 3);
            this.mainLayout.Controls.Add(this.lbl_AccountID, 0, 4);
            this.mainLayout.Controls.Add(this.txt_AccountID, 1, 4);
            this.mainLayout.Controls.Add(this.lbl_MaxCredit, 0, 5);
            this.mainLayout.Controls.Add(this.num_MaxCredit, 1, 5);
            // 
            // Labels
            // 
            this.lbl_Name.Text = "الاسم:";
            this.lbl_Phone.Text = "الهاتف:";
            this.lbl_Mobile.Text = "الهاتف المحمول:";
            this.lbl_Address.Text = "العنوان:";
            this.lbl_AccountID.Text = "رقم الحساب:";
            this.lbl_MaxCredit.Text = "حد الائتمان:";
            foreach (var label in new[] { this.lbl_Name, this.lbl_Phone, this.lbl_Mobile, this.lbl_Address, this.lbl_AccountID, this.lbl_MaxCredit })
            {
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                label.Margin = new System.Windows.Forms.Padding(4);
            }
            // 
            // Text fields
            // 
            foreach (var field in new[] { this.txt_Name, this.txt_Phone, this.txt_Mobile, this.txt_Address, this.txt_AccountID })
            {
                field.Dock = System.Windows.Forms.DockStyle.Fill;
                field.Margin = new System.Windows.Forms.Padding(4, 9, 4, 9);
            }
            this.txt_AccountID.ReadOnly = true;
            this.txt_AccountID.TabStop = false;
            this.num_MaxCredit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.num_MaxCredit.Margin = new System.Windows.Forms.Padding(4, 9, 4, 9);
            this.num_MaxCredit.DecimalPlaces = 2;
            this.num_MaxCredit.Minimum = 0;
            this.num_MaxCredit.Maximum = 999999999999999999M;
            this.num_MaxCredit.ThousandsSeparator = true;
            this.num_MaxCredit.Increment = 100;
            // 
            // frm_CustomerVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 390);
            this.MinimumSize = new System.Drawing.Size(550, 360);
            this.Controls.Add(this.mainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frm_CustomerVendor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "بيانات عميل أو مورد";
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_MaxCredit)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Label lbl_Phone;
        private System.Windows.Forms.TextBox txt_Phone;
        private System.Windows.Forms.Label lbl_Mobile;
        private System.Windows.Forms.TextBox txt_Mobile;
        private System.Windows.Forms.Label lbl_Address;
        private System.Windows.Forms.TextBox txt_Address;
        private System.Windows.Forms.Label lbl_AccountID;
        private System.Windows.Forms.TextBox txt_AccountID;
        private System.Windows.Forms.Label lbl_MaxCredit;
        private System.Windows.Forms.NumericUpDown num_MaxCredit;
    }
}