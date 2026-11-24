namespace SalesLinq.Forms
{
    partial class frm_AccessProfile
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.DataGridView gridPermissions;
        private System.Windows.Forms.ErrorProvider errorProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private static System.Windows.Forms.DataGridViewCheckBoxColumn CreatePermissionColumn(string property, string caption)
        {
            return new System.Windows.Forms.DataGridViewCheckBoxColumn
            {
                Name = property,
                DataPropertyName = property,
                HeaderText = caption,
                Width = 65,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            };
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            layout = new System.Windows.Forms.TableLayoutPanel();
            lbl_Name = new System.Windows.Forms.Label();
            txt_Name = new System.Windows.Forms.TextBox();
            gridPermissions = new System.Windows.Forms.DataGridView();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridPermissions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.RowCount = 2;
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.Padding = new System.Windows.Forms.Padding(12);
            layout.Controls.Add(lbl_Name, 0, 0);
            layout.Controls.Add(txt_Name, 1, 0);
            layout.Controls.Add(gridPermissions, 0, 1);
            layout.SetColumnSpan(gridPermissions, 2);

            lbl_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_Name.Text = "اسم النموذج";
            lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            txt_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            txt_Name.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);

            gridPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            gridPermissions.AllowUserToAddRows = false;
            gridPermissions.AllowUserToDeleteRows = false;
            gridPermissions.AllowUserToResizeRows = false;
            gridPermissions.AutoGenerateColumns = false;
            gridPermissions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = nameof(SalesLinq.Class.ScreensAccessProfile.ScreenCaption),
                    DataPropertyName = nameof(SalesLinq.Class.ScreensAccessProfile.ScreenCaption),
                    HeaderText = "الشاشة",
                    ReadOnly = true,
                    AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                },
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanShow), "عرض"),
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanOpen), "فتح"),
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanAdd), "إضافة"),
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanEdit), "تعديل"),
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanDelete), "حذف"),
                CreatePermissionColumn(nameof(SalesLinq.Class.ScreensAccessProfile.CanPrint), "طباعة")
            });
            gridPermissions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            gridPermissions.BackgroundColor = System.Drawing.SystemColors.Window;
            gridPermissions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            gridPermissions.MultiSelect = false;
            gridPermissions.RowHeadersVisible = false;
            gridPermissions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridPermissions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            gridPermissions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;

            errorProvider.ContainerControl = this;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1050, 650);
            Controls.Add(layout);
            Name = "frm_AccessProfile";
            Text = "نموذج صلاحيات الوصول";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;

            layout.ResumeLayout(false);
            layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridPermissions).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            toolStrip.BringToFront();
        }
    }
}