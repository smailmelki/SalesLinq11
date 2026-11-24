namespace SalesLinq.Forms
{
    partial class frm_Product
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
            this.detailsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Code = new System.Windows.Forms.Label();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.lbl_Type = new System.Windows.Forms.Label();
            this.lkp_Type = new System.Windows.Forms.ComboBox();
            this.lbl_Category = new System.Windows.Forms.Label();
            this.lkp_Category = new System.Windows.Forms.ComboBox();
            this.lbl_CostCalculationMethod = new System.Windows.Forms.Label();
            this.lkp_CostCalculationMethod = new System.Windows.Forms.ComboBox();
            this.checkEdit1 = new System.Windows.Forms.CheckBox();
            this.memoEdit1 = new System.Windows.Forms.TextBox();
            this.picturePanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureEdit1 = new System.Windows.Forms.PictureBox();
            this.pictureButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_LoadImage = new System.Windows.Forms.Button();
            this.btn_ClearImage = new System.Windows.Forms.Button();
            this.unitsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.unitsHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_Units = new System.Windows.Forms.Label();
            this.txt_NewUnit = new System.Windows.Forms.TextBox();
            this.btn_AddUnit = new System.Windows.Forms.Button();
            this.gridView1 = new System.Windows.Forms.DataGridView();
            this.mainLayout.SuspendLayout();
            this.detailsLayout.SuspendLayout();
            this.picturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1)).BeginInit();
            this.pictureButtons.SuspendLayout();
            this.unitsPanel.SuspendLayout();
            this.unitsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Padding = new System.Windows.Forms.Padding(14);
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.mainLayout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainLayout.Controls.Add(this.detailsLayout, 0, 0);
            this.mainLayout.Controls.Add(this.picturePanel, 1, 0);
            this.mainLayout.Controls.Add(this.unitsPanel, 0, 1);
            this.mainLayout.SetColumnSpan(this.unitsPanel, 2);
            // 
            // detailsLayout
            // 
            this.detailsLayout.ColumnCount = 2;
            this.detailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.detailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsLayout.Padding = new System.Windows.Forms.Padding(8);
            this.detailsLayout.RowCount = 7;
            for (int i = 0; i < 6; i++)
                this.detailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.detailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailsLayout.Controls.Add(this.lbl_Code, 0, 0);
            this.detailsLayout.Controls.Add(this.txt_Code, 1, 0);
            this.detailsLayout.Controls.Add(this.lbl_Name, 0, 1);
            this.detailsLayout.Controls.Add(this.txt_Name, 1, 1);
            this.detailsLayout.Controls.Add(this.lbl_Type, 0, 2);
            this.detailsLayout.Controls.Add(this.lkp_Type, 1, 2);
            this.detailsLayout.Controls.Add(this.lbl_Category, 0, 3);
            this.detailsLayout.Controls.Add(this.lkp_Category, 1, 3);
            this.detailsLayout.Controls.Add(this.lbl_CostCalculationMethod, 0, 4);
            this.detailsLayout.Controls.Add(this.lkp_CostCalculationMethod, 1, 4);
            this.detailsLayout.Controls.Add(this.checkEdit1, 1, 5);
            this.detailsLayout.Controls.Add(this.memoEdit1, 1, 6);
            this.detailsLayout.Controls.Add(new System.Windows.Forms.Label { Text = "الوصف:", Dock = System.Windows.Forms.DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 6);
            // 
            // labels and fields
            // 
            this.lbl_Code.Text = "الكود:";
            this.lbl_Name.Text = "الاسم:";
            this.lbl_Type.Text = "النوع:";
            this.lbl_Category.Text = "الفئة:";
            this.lbl_CostCalculationMethod.Text = "طريقة حساب التكلفة:";
            foreach (var label in new[] { this.lbl_Code, this.lbl_Name, this.lbl_Type, this.lbl_Category, this.lbl_CostCalculationMethod })
            {
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            }
            foreach (var control in new System.Windows.Forms.Control[] { this.txt_Code, this.txt_Name, this.lkp_Type, this.lkp_Category, this.lkp_CostCalculationMethod })
            {
                control.Dock = System.Windows.Forms.DockStyle.Fill;
                control.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            }
            this.lkp_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lkp_Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.lkp_Category.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.lkp_Category.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.lkp_CostCalculationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.checkEdit1.Text = "نشط";
            this.checkEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.Multiline = true;
            this.memoEdit1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            // 
            // picturePanel
            // 
            this.picturePanel.ColumnCount = 1;
            this.picturePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.picturePanel.RowCount = 2;
            this.picturePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.picturePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.picturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picturePanel.Padding = new System.Windows.Forms.Padding(6);
            this.picturePanel.Controls.Add(this.pictureEdit1, 0, 0);
            this.picturePanel.Controls.Add(this.pictureButtons, 0, 1);
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureEdit1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pictureButtons.Controls.Add(this.btn_LoadImage);
            this.pictureButtons.Controls.Add(this.btn_ClearImage);
            this.btn_LoadImage.Text = "اختيار صورة";
            this.btn_ClearImage.Text = "إزالة الصورة";
            this.btn_LoadImage.AutoSize = true;
            this.btn_ClearImage.AutoSize = true;
            // 
            // unitsPanel
            // 
            this.unitsPanel.ColumnCount = 1;
            this.unitsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.unitsPanel.RowCount = 2;
            this.unitsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.unitsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.unitsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unitsPanel.Controls.Add(this.unitsHeader, 0, 0);
            this.unitsPanel.Controls.Add(this.gridView1, 0, 1);
            this.unitsHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unitsHeader.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.unitsHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.unitsHeader.Controls.Add(this.lbl_Units);
            this.unitsHeader.Controls.Add(this.txt_NewUnit);
            this.unitsHeader.Controls.Add(this.btn_AddUnit);
            this.lbl_Units.Text = "وحدات القياس";
            this.lbl_Units.AutoSize = true;
            this.lbl_Units.Padding = new System.Windows.Forms.Padding(8, 6, 12, 0);
            this.txt_NewUnit.Width = 170;
            this.txt_NewUnit.Margin = new System.Windows.Forms.Padding(4);
            this.btn_AddUnit.Text = "إضافة وحدة";
            this.btn_AddUnit.AutoSize = true;
            this.btn_AddUnit.Click += new System.EventHandler(this.BtnAddUnit_Click);
            this.btn_LoadImage.Click += new System.EventHandler(this.BtnLoadImage_Click);
            this.btn_ClearImage.Click += new System.EventHandler(this.BtnClearImage_Click);
            // 
            // gridView1
            // 
            this.gridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridView1.ColumnHeadersHeight = 38;
            this.gridView1.RowTemplate.Height = 32;
            this.gridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // frm_Product
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.MinimumSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.mainLayout);
            this.toolStrip.BringToFront();
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frm_Product";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "بيانات الصنف";
            this.mainLayout.ResumeLayout(false);
            this.detailsLayout.ResumeLayout(false);
            this.detailsLayout.PerformLayout();
            this.picturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1)).EndInit();
            this.pictureButtons.ResumeLayout(false);
            this.pictureButtons.PerformLayout();
            this.unitsPanel.ResumeLayout(false);
            this.unitsHeader.ResumeLayout(false);
            this.unitsHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TableLayoutPanel detailsLayout;
        private System.Windows.Forms.Label lbl_Code;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Label lbl_Type;
        private System.Windows.Forms.ComboBox lkp_Type;
        private System.Windows.Forms.Label lbl_Category;
        private System.Windows.Forms.ComboBox lkp_Category;
        private System.Windows.Forms.Label lbl_CostCalculationMethod;
        private System.Windows.Forms.ComboBox lkp_CostCalculationMethod;
        private System.Windows.Forms.CheckBox checkEdit1;
        private System.Windows.Forms.TextBox memoEdit1;
        private System.Windows.Forms.TableLayoutPanel picturePanel;
        private System.Windows.Forms.PictureBox pictureEdit1;
        private System.Windows.Forms.FlowLayoutPanel pictureButtons;
        private System.Windows.Forms.Button btn_LoadImage;
        private System.Windows.Forms.Button btn_ClearImage;
        private System.Windows.Forms.TableLayoutPanel unitsPanel;
        private System.Windows.Forms.FlowLayoutPanel unitsHeader;
        private System.Windows.Forms.Label lbl_Units;
        private System.Windows.Forms.TextBox txt_NewUnit;
        private System.Windows.Forms.Button btn_AddUnit;
        private System.Windows.Forms.DataGridView gridView1;
    }
}