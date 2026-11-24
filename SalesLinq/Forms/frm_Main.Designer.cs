namespace SalesLinq.Forms
{
    partial class frm_Main
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel brandPanel;
        private System.Windows.Forms.Panel workspacePanel;
        private System.Windows.Forms.Panel navigationScroll;
        private System.Windows.Forms.FlowLayoutPanel navFlow;
        private System.Windows.Forms.TabControl workspaceTabs;
        private System.Windows.Forms.TabPage dashboardTab;
        private System.Windows.Forms.Panel dashboardPanel;
        private System.Windows.Forms.FlowLayoutPanel openFormsPanel;
        private System.Windows.Forms.PictureBox peLogo;
        private System.Windows.Forms.Label lblBrandName;
        private System.Windows.Forms.Label lblBrandCaption;
        private System.Windows.Forms.Label lblAppTitle;
        public System.Windows.Forms.Label Lbl_UserName;
        private System.Windows.Forms.Button btnSignOut;
        private System.Windows.Forms.Button btnCloseCurrent;
        private System.Windows.Forms.TextBox txtNavigationSearch;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            headerPanel = new System.Windows.Forms.Panel();
            lblAppTitle = new System.Windows.Forms.Label();
            Lbl_UserName = new System.Windows.Forms.Label();
            btnCloseCurrent = new System.Windows.Forms.Button();
            btnSignOut = new System.Windows.Forms.Button();
            sidebarPanel = new System.Windows.Forms.Panel();
            brandPanel = new System.Windows.Forms.Panel();
            peLogo = new System.Windows.Forms.PictureBox();
            lblBrandName = new System.Windows.Forms.Label();
            lblBrandCaption = new System.Windows.Forms.Label();
            txtNavigationSearch = new System.Windows.Forms.TextBox();
            navigationScroll = new System.Windows.Forms.Panel();
            navFlow = new System.Windows.Forms.FlowLayoutPanel();
            workspacePanel = new System.Windows.Forms.Panel();
            workspaceTabs = new System.Windows.Forms.TabControl();
            dashboardTab = new System.Windows.Forms.TabPage();
            dashboardPanel = new System.Windows.Forms.Panel();
            openFormsPanel = new System.Windows.Forms.FlowLayoutPanel();

            headerPanel.SuspendLayout();
            sidebarPanel.SuspendLayout();
            brandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)peLogo).BeginInit();
            navigationScroll.SuspendLayout();
            workspacePanel.SuspendLayout();
            workspaceTabs.SuspendLayout();
            dashboardTab.SuspendLayout();
            dashboardPanel.SuspendLayout();
            SuspendLayout();

            // Header
            headerPanel.BackColor = System.Drawing.Color.White;
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 72;
            headerPanel.Padding = new System.Windows.Forms.Padding(22, 10, 22, 10);
            headerPanel.Controls.Add(btnSignOut);
            headerPanel.Controls.Add(btnCloseCurrent);
            headerPanel.Controls.Add(Lbl_UserName);
            headerPanel.Controls.Add(lblAppTitle);

            lblAppTitle.Dock = System.Windows.Forms.DockStyle.Right;
            lblAppTitle.Width = 330;
            lblAppTitle.Text = "SalesLinq  |  إدارة المبيعات والمخزون";
            lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(27, 45, 72);
            lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            Lbl_UserName.Dock = System.Windows.Forms.DockStyle.Right;
            Lbl_UserName.Width = 190;
            Lbl_UserName.Text = "غير مسجل";
            Lbl_UserName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            Lbl_UserName.ForeColor = System.Drawing.Color.FromArgb(70, 87, 108);
            Lbl_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            btnSignOut.Dock = System.Windows.Forms.DockStyle.Left;
            btnSignOut.Width = 112;
            btnSignOut.Text = "تسجيل الخروج";
            btnSignOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnSignOut.ForeColor = System.Drawing.Color.White;
            btnSignOut.BackColor = System.Drawing.Color.FromArgb(18, 145, 139);
            btnSignOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSignOut.FlatAppearance.BorderSize = 0;
            btnSignOut.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSignOut.Click += new System.EventHandler(Btn_SignOut_Click);

            btnCloseCurrent.Dock = System.Windows.Forms.DockStyle.Left;
            btnCloseCurrent.Width = 112;
            btnCloseCurrent.Text = "إغلاق النافذة";
            btnCloseCurrent.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnCloseCurrent.ForeColor = System.Drawing.Color.FromArgb(38, 52, 72);
            btnCloseCurrent.BackColor = System.Drawing.Color.FromArgb(238, 242, 247);
            btnCloseCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCloseCurrent.FlatAppearance.BorderSize = 0;
            btnCloseCurrent.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCloseCurrent.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            btnCloseCurrent.Click += new System.EventHandler(Btn_CloseCurrent_Click);

            // Sidebar
            sidebarPanel.BackColor = System.Drawing.Color.FromArgb(27, 45, 72);
            sidebarPanel.Dock = System.Windows.Forms.DockStyle.Right;
            sidebarPanel.Width = 285;
            sidebarPanel.Padding = new System.Windows.Forms.Padding(0);

            brandPanel.Dock = System.Windows.Forms.DockStyle.Top;
            brandPanel.Height = 104;
            brandPanel.BackColor = System.Drawing.Color.FromArgb(22, 37, 60);
            brandPanel.Padding = new System.Windows.Forms.Padding(14);
            brandPanel.Controls.Add(lblBrandCaption);
            brandPanel.Controls.Add(lblBrandName);
            brandPanel.Controls.Add(peLogo);

            peLogo.Dock = System.Windows.Forms.DockStyle.Right;
            peLogo.Width = 64;
            peLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            peLogo.BackColor = System.Drawing.Color.Transparent;

            lblBrandName.Dock = System.Windows.Forms.DockStyle.Top;
            lblBrandName.Height = 39;
            lblBrandName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblBrandName.ForeColor = System.Drawing.Color.White;
            lblBrandName.Text = "SalesLinq";
            lblBrandName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblBrandCaption.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblBrandCaption.Height = 28;
            lblBrandCaption.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblBrandCaption.ForeColor = System.Drawing.Color.FromArgb(184, 199, 218);
            lblBrandCaption.Text = "نظام إدارة الأعمال";
            lblBrandCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            txtNavigationSearch.Dock = System.Windows.Forms.DockStyle.Top;
            txtNavigationSearch.Height = 38;
            txtNavigationSearch.Margin = new System.Windows.Forms.Padding(12);
            txtNavigationSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtNavigationSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            txtNavigationSearch.Text = "بحث في القوائم";
            txtNavigationSearch.ForeColor = System.Drawing.Color.Gray;
            txtNavigationSearch.Enter += (s, e) =>
            {
                if (txtNavigationSearch.Text == "بحث في القوائم")
                {
                    txtNavigationSearch.Clear();
                    txtNavigationSearch.ForeColor = System.Drawing.Color.FromArgb(38, 52, 72);
                }
            };
            txtNavigationSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNavigationSearch.Text))
                {
                    txtNavigationSearch.Text = "بحث في القوائم";
                    txtNavigationSearch.ForeColor = System.Drawing.Color.Gray;
                }
            };
            txtNavigationSearch.TextChanged += new System.EventHandler(TxtNavigationSearch_TextChanged);

            navigationScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            navigationScroll.AutoScroll = true;
            navigationScroll.Padding = new System.Windows.Forms.Padding(8, 8, 8, 12);
            navigationScroll.Controls.Add(navFlow);

            navFlow.Dock = System.Windows.Forms.DockStyle.Top;
            navFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            navFlow.WrapContents = false;
            navFlow.AutoSize = true;
            navFlow.BackColor = System.Drawing.Color.FromArgb(27, 45, 72);
            navFlow.Padding = new System.Windows.Forms.Padding(0);
            navFlow.SizeChanged += (s, e) => navFlow.Width = System.Math.Max(150, navigationScroll.ClientSize.Width - 8);

            sidebarPanel.Controls.Add(navigationScroll);
            sidebarPanel.Controls.Add(txtNavigationSearch);
            sidebarPanel.Controls.Add(brandPanel);

            // Workspace
            workspacePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            workspacePanel.BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            workspacePanel.Padding = new System.Windows.Forms.Padding(22);

            workspaceTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            workspaceTabs.Font = new System.Drawing.Font("Segoe UI", 9F);
            workspaceTabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            workspaceTabs.RightToLeftLayout = true;
            workspaceTabs.Controls.Add(dashboardTab);

            dashboardTab.Text = "الرئيسية";
            dashboardTab.BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            dashboardTab.Padding = new System.Windows.Forms.Padding(16);
            dashboardTab.Controls.Add(dashboardPanel);

            dashboardPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            dashboardPanel.AutoScroll = true;
            dashboardPanel.BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            dashboardPanel.Padding = new System.Windows.Forms.Padding(8);
            dashboardPanel.Controls.Add(openFormsPanel);

            openFormsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            openFormsPanel.Height = 150;
            openFormsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            openFormsPanel.WrapContents = true;
            openFormsPanel.AutoScroll = true;
            openFormsPanel.BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            openFormsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);

            workspacePanel.Controls.Add(workspaceTabs);

            // Main form
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(244, 247, 251);
            ClientSize = new System.Drawing.Size(1440, 900);
            Controls.Add(workspacePanel);
            Controls.Add(sidebarPanel);
            Controls.Add(headerPanel);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            MinimumSize = new System.Drawing.Size(1100, 700);
            Name = "frm_Main";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SalesLinq - نظام إدارة الأعمال";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;

            dashboardPanel.ResumeLayout(false);
            dashboardTab.ResumeLayout(false);
            workspaceTabs.ResumeLayout(false);
            workspacePanel.ResumeLayout(false);
            navigationScroll.ResumeLayout(false);
            brandPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)peLogo).EndInit();
            sidebarPanel.ResumeLayout(false);
            sidebarPanel.PerformLayout();
            headerPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}