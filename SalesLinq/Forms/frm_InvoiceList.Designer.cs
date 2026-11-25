namespace SalesLinq.Forms
{
    partial class frm_InvoiceList
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
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions1 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions2 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            this.flyoutPanel1 = new DevExpress.Utils.FlyoutPanel();
            this.flyoutPanelControl1 = new DevExpress.Utils.FlyoutPanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.lkp_Branch = new DevExpress.XtraEditors.LookUpEdit();
            this.lkp_Drawer = new DevExpress.XtraEditors.LookUpEdit();
            this.glkp_PartID = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lkp_PartType = new DevExpress.XtraEditors.LookUpEdit();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.btn_Apply = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Clear = new DevExpress.XtraEditors.SimpleButton();
            this.btn_FlyClose = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lyc_FromDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_ToDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_PartType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_Part = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_Branch = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_Drawer = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).BeginInit();
            this.flyoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanelControl1)).BeginInit();
            this.flyoutPanelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Branch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Drawer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkp_PartID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_PartType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_FromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_ToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_PartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Part)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Branch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Drawer)).BeginInit();
            this.SuspendLayout();
            // 
            // flyoutPanel1
            // 
            this.flyoutPanel1.Controls.Add(this.flyoutPanelControl1);
            this.flyoutPanel1.Location = new System.Drawing.Point(155, 65);
            this.flyoutPanel1.Name = "flyoutPanel1";
            this.flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            this.flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            this.flyoutPanel1.OptionsButtonPanel.AllowGlyphSkinning = true;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelHeight = 50;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelLocation = DevExpress.Utils.FlyoutPanelButtonPanelLocation.Bottom;
            buttonImageOptions1.Location = DevExpress.XtraEditors.ButtonPanel.ImageLocation.AboveText;
            buttonImageOptions1.SvgImage = global::SalesLinq.Properties.Resources.find;
            buttonImageOptions1.SvgImageSize = new System.Drawing.Size(20, 20);
            buttonImageOptions2.Location = DevExpress.XtraEditors.ButtonPanel.ImageLocation.AboveText;
            buttonImageOptions2.SvgImage = global::SalesLinq.Properties.Resources.clearheaderandfooter;
            buttonImageOptions2.SvgImageSize = new System.Drawing.Size(20, 20);
            this.flyoutPanel1.OptionsButtonPanel.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.Utils.PeekFormButton("تطبيق", true, buttonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, "Apply", -1, false),
            new DevExpress.Utils.PeekFormButton("مسح الفلاتر", true, buttonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, "Clear", -1, false)});
            this.flyoutPanel1.OwnerControl = this.gridControl1;
            this.flyoutPanel1.Size = new System.Drawing.Size(342, 320);
            this.flyoutPanel1.TabIndex = 5;
            // 
            // flyoutPanelControl1
            // 
            this.flyoutPanelControl1.Controls.Add(this.layoutControl1);
            this.flyoutPanelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flyoutPanelControl1.FlyoutPanel = this.flyoutPanel1;
            this.flyoutPanelControl1.Location = new System.Drawing.Point(0, 0);
            this.flyoutPanelControl1.Name = "flyoutPanelControl1";
            this.flyoutPanelControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flyoutPanelControl1.Size = new System.Drawing.Size(342, 320);
            this.flyoutPanelControl1.TabIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.lkp_Branch);
            this.layoutControl1.Controls.Add(this.lkp_Drawer);
            this.layoutControl1.Controls.Add(this.glkp_PartID);
            this.layoutControl1.Controls.Add(this.lkp_PartType);
            this.layoutControl1.Controls.Add(this.dateEdit1);
            this.layoutControl1.Controls.Add(this.dateEdit2);
            this.layoutControl1.Controls.Add(this.btn_Apply);
            this.layoutControl1.Controls.Add(this.btn_Clear);
            this.layoutControl1.Controls.Add(this.btn_FlyClose);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(850, 72, 650, 400);
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(338, 316);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // lkp_Branch
            // 
            this.lkp_Branch.Location = new System.Drawing.Point(24, 195);
            this.lkp_Branch.Name = "lkp_Branch";
            this.lkp_Branch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Branch.Size = new System.Drawing.Size(210, 26);
            this.lkp_Branch.StyleController = this.layoutControl1;
            this.lkp_Branch.TabIndex = 11;
            // 
            // lkp_Drawer
            // 
            this.lkp_Drawer.Location = new System.Drawing.Point(24, 165);
            this.lkp_Drawer.Name = "lkp_Drawer";
            this.lkp_Drawer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Drawer.Size = new System.Drawing.Size(210, 26);
            this.lkp_Drawer.StyleController = this.layoutControl1;
            this.lkp_Drawer.TabIndex = 10;
            // 
            // glkp_PartID
            // 
            this.glkp_PartID.Location = new System.Drawing.Point(24, 135);
            this.glkp_PartID.Name = "glkp_PartID";
            this.glkp_PartID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkp_PartID.Properties.NullText = "";
            this.glkp_PartID.Properties.PopupView = this.gridLookUpEdit1View;
            this.glkp_PartID.Size = new System.Drawing.Size(210, 26);
            this.glkp_PartID.StyleController = this.layoutControl1;
            this.glkp_PartID.TabIndex = 9;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // lkp_PartType
            // 
            this.lkp_PartType.Location = new System.Drawing.Point(24, 105);
            this.lkp_PartType.Name = "lkp_PartType";
            this.lkp_PartType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_PartType.Properties.NullText = "";
            this.lkp_PartType.Size = new System.Drawing.Size(210, 26);
            this.lkp_PartType.StyleController = this.layoutControl1;
            this.lkp_PartType.TabIndex = 8;
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(24, 45);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Size = new System.Drawing.Size(210, 26);
            this.dateEdit1.StyleController = this.layoutControl1;
            this.dateEdit1.TabIndex = 4;
            // 
            // dateEdit2
            // 
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(24, 75);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Size = new System.Drawing.Size(210, 26);
            this.dateEdit2.StyleController = this.layoutControl1;
            this.dateEdit2.TabIndex = 5;
            // 
            // btn_Apply
            // 
            this.btn_Apply.Location = new System.Drawing.Point(166, 252);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(160, 24);
            this.btn_Apply.StyleController = this.layoutControl1;
            this.btn_Apply.TabIndex = 6;
            this.btn_Apply.Text = "تطبيق";
            this.btn_Apply.Click += new System.EventHandler(this.Btn_Apply_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(12, 252);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(150, 24);
            this.btn_Clear.StyleController = this.layoutControl1;
            this.btn_Clear.TabIndex = 7;
            this.btn_Clear.Text = "مسح الفلاتر";
            this.btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // btn_FlyClose
            // 
            this.btn_FlyClose.Location = new System.Drawing.Point(12, 280);
            this.btn_FlyClose.Name = "btn_FlyClose";
            this.btn_FlyClose.Size = new System.Drawing.Size(314, 24);
            this.btn_FlyClose.StyleController = this.layoutControl1;
            this.btn_FlyClose.TabIndex = 12;
            this.btn_FlyClose.Text = "اغلاق";
            this.btn_FlyClose.Click += new System.EventHandler(this.Btn_FlyClose_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.simpleSeparator1,
            this.layoutControlItem8,
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(338, 316);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_Apply;
            this.layoutControlItem3.Location = new System.Drawing.Point(154, 240);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(164, 28);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_Clear;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 240);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(154, 28);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 225);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(318, 14);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.Location = new System.Drawing.Point(0, 239);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(318, 1);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_FlyClose;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 268);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(318, 28);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.layoutControlGroup1.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lyc_FromDate,
            this.lyc_ToDate,
            this.lyc_PartType,
            this.lyc_Part,
            this.lyc_Branch,
            this.lyc_Drawer});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(318, 225);
            this.layoutControlGroup1.Text = "الفلاتر";
            // 
            // lyc_FromDate
            // 
            this.lyc_FromDate.Control = this.dateEdit1;
            this.lyc_FromDate.Location = new System.Drawing.Point(0, 0);
            this.lyc_FromDate.Name = "lyc_FromDate";
            this.lyc_FromDate.Size = new System.Drawing.Size(294, 30);
            this.lyc_FromDate.Text = "من تاريخ";
            this.lyc_FromDate.TextSize = new System.Drawing.Size(68, 19);
            // 
            // lyc_ToDate
            // 
            this.lyc_ToDate.Control = this.dateEdit2;
            this.lyc_ToDate.Location = new System.Drawing.Point(0, 30);
            this.lyc_ToDate.Name = "lyc_ToDate";
            this.lyc_ToDate.Size = new System.Drawing.Size(294, 30);
            this.lyc_ToDate.Text = "حتى تاريخ";
            this.lyc_ToDate.TextSize = new System.Drawing.Size(68, 19);
            // 
            // lyc_PartType
            // 
            this.lyc_PartType.Control = this.lkp_PartType;
            this.lyc_PartType.Location = new System.Drawing.Point(0, 60);
            this.lyc_PartType.Name = "lyc_PartType";
            this.lyc_PartType.Size = new System.Drawing.Size(294, 30);
            this.lyc_PartType.Text = "طرف التعامل";
            this.lyc_PartType.TextSize = new System.Drawing.Size(68, 19);
            // 
            // lyc_Part
            // 
            this.lyc_Part.Control = this.glkp_PartID;
            this.lyc_Part.Location = new System.Drawing.Point(0, 90);
            this.lyc_Part.Name = "lyc_Part";
            this.lyc_Part.Size = new System.Drawing.Size(294, 30);
            this.lyc_Part.Text = "...";
            this.lyc_Part.TextSize = new System.Drawing.Size(68, 19);
            // 
            // lyc_Branch
            // 
            this.lyc_Branch.Control = this.lkp_Branch;
            this.lyc_Branch.Location = new System.Drawing.Point(0, 120);
            this.lyc_Branch.Name = "lyc_Branch";
            this.lyc_Branch.Size = new System.Drawing.Size(294, 30);
            this.lyc_Branch.Text = "الفرع";
            this.lyc_Branch.TextSize = new System.Drawing.Size(68, 19);
            // 
            // lyc_Drawer
            // 
            this.lyc_Drawer.Control = this.lkp_Drawer;
            this.lyc_Drawer.Location = new System.Drawing.Point(0, 150);
            this.lyc_Drawer.Name = "lyc_Drawer";
            this.lyc_Drawer.Size = new System.Drawing.Size(294, 30);
            this.lyc_Drawer.Text = "الخزينة";
            this.lyc_Drawer.TextSize = new System.Drawing.Size(68, 19);
            // 
            // frm_InvoiceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 525);
            this.Controls.Add(this.flyoutPanel1);
            this.IconOptions.SvgImage = global::SalesLinq.Properties.Resources.menu;
            this.Name = "frm_InvoiceList";
            this.Text = "frm_InvoiceList";
            this.Load += new System.EventHandler(this.Frm_InvoiceList_Load);
            this.Controls.SetChildIndex(this.flyoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).EndInit();
            this.flyoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanelControl1)).EndInit();
            this.flyoutPanelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Branch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Drawer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkp_PartID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_PartType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_FromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_ToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_PartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Part)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Branch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_Drawer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.FlyoutPanel flyoutPanel1;
        private DevExpress.Utils.FlyoutPanelControl flyoutPanelControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.DateEdit dateEdit2;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem lyc_FromDate;
        private DevExpress.XtraLayout.LayoutControlItem lyc_ToDate;
        private DevExpress.XtraEditors.SimpleButton btn_Apply;
        private DevExpress.XtraEditors.SimpleButton btn_Clear;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraEditors.LookUpEdit lkp_PartType;
        private DevExpress.XtraLayout.LayoutControlItem lyc_PartType;
        private DevExpress.XtraEditors.GridLookUpEdit glkp_PartID;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lyc_Part;
        private DevExpress.XtraEditors.LookUpEdit lkp_Branch;
        private DevExpress.XtraEditors.LookUpEdit lkp_Drawer;
        private DevExpress.XtraLayout.LayoutControlItem lyc_Branch;
        private DevExpress.XtraLayout.LayoutControlItem lyc_Drawer;
        private DevExpress.XtraEditors.SimpleButton btn_FlyClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    }
}