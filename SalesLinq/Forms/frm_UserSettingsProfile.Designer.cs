namespace SalesLinq.Forms
{
    partial class frm_UserSettingsProfile
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.ListBox navigation;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.ErrorProvider errorProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            layout = new System.Windows.Forms.TableLayoutPanel();
            lbl_Name = new System.Windows.Forms.Label();
            txt_Name = new System.Windows.Forms.TextBox();
            split = new System.Windows.Forms.SplitContainer();
            navigation = new System.Windows.Forms.ListBox();
            tabs = new System.Windows.Forms.TabControl();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.Padding = new System.Windows.Forms.Padding(12);
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.RowCount = 2;
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.Controls.Add(lbl_Name, 0, 0);
            layout.Controls.Add(txt_Name, 1, 0);
            layout.Controls.Add(split, 0, 1);
            layout.SetColumnSpan(split, 2);

            lbl_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_Name.Text = "اسم النموذج";
            lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            txt_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            txt_Name.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);

            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Orientation = System.Windows.Forms.Orientation.Vertical;
            split.SplitterDistance = 230;
            split.Panel1.Controls.Add(navigation);
            split.Panel2.Controls.Add(tabs);

            navigation.Dock = System.Windows.Forms.DockStyle.Fill;
            navigation.IntegralHeight = false;
            navigation.SelectedIndexChanged += Navigation_SelectedIndexChanged;
            tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            tabs.RightToLeftLayout = true;
            tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;

            errorProvider.ContainerControl = this;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1120, 720);
            Controls.Add(layout);
            Name = "frm_UserSettingsProfile";
            Text = "نموذج إعدادات المستخدم";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;

            layout.ResumeLayout(false);
            layout.PerformLayout();
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            toolStrip.BringToFront();
        }
    }
}