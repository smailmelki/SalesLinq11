using SalesLinq.Class;
using SalesLinq.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    /// <summary>
    /// النافذة الرئيسية ونقطة التنقل الموحدة في التطبيق.
    /// </summary>
    public partial class frm_Main : Form
    {
        private static frm_Main _Instance;
        private readonly List<Form> _openForms = new List<Form>();
        private FlowLayoutPanel quickActionsPanel;

        private static readonly Color Navy = Color.FromArgb(27, 45, 72);
        private static readonly Color NavyLight = Color.FromArgb(39, 61, 94);
        private static readonly Color Accent = Color.FromArgb(18, 145, 139);
        private static readonly Color PageBackground = Color.FromArgb(244, 247, 251);
        private static readonly Color TextPrimary = Color.FromArgb(38, 52, 72);
        private static readonly Color TextMuted = Color.FromArgb(112, 126, 145);

        public static frm_Main Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                    _Instance = new frm_Main();
                return _Instance;
            }
        }

        public frm_Main()
        {
            InitializeComponent();
            BuildDashboard();
            Load += Frm_Main_Load;
            FormClosing += Frm_Main_FormClosing;
            Shown += (s, e) => RefreshOpenWindows();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch { }

            LoadCompanyIdentity();
            RefreshOpenWindows();
        }

        private void LoadCompanyIdentity()
        {
            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    var info = db.CompanyInfos.FirstOrDefault();
                    if (info == null) return;

                    if (!string.IsNullOrWhiteSpace(info.CompanyName))
                        lblBrandName.Text = info.CompanyName;

                    if (info.Logo != null && info.Logo.Length > 0)
                    {
                        using (var stream = new System.IO.MemoryStream(info.Logo.ToArray()))
                        using (Image image = Image.FromStream(stream))
                        {
                            if (peLogo.Image != null)
                                peLogo.Image.Dispose();
                            peLogo.Image = new Bitmap(image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Company identity: " + ex.Message);
            }
        }

        /// <summary>
        /// يعاد بناؤها بعد تسجيل الدخول أو تغيير المستخدم.
        /// </summary>
        public void AddAccordionElement()
        {
            BuildNavigation();
            RefreshQuickActions();
            Lbl_UserName.Text = Session.User == null ? "غير مسجل" : Session.User.Name;
        }

        private void BuildNavigation()
        {
            navFlow.SuspendLayout();
            navFlow.Controls.Clear();

            if (Session.User == null)
            {
                navFlow.Controls.Add(CreateNavigationHint("يرجى تسجيل الدخول لعرض القوائم."));
                navFlow.ResumeLayout();
                return;
            }

            var allScreens = Session.ScreenAccesses ?? Screens.GetScreens;
            bool isAdmin = Session.User.UserType == (byte)Master.UserType.Admin;
            var visibleScreens = allScreens
                .Where(x => isAdmin || x.CanShow)
                .OrderBy(x => x.ScreenID)
                .ToList();

            string query = txtNavigationSearch.Text.Trim();
            if (query == "بحث في القوائم") query = string.Empty;

            foreach (var group in visibleScreens.Where(x => x.ParentScreenID == 0))
            {
                var children = visibleScreens
                    .Where(x => x.ParentScreenID == group.ScreenID)
                    .OrderBy(x => x.ScreenID)
                    .ToList();

                var matchingChildren = string.IsNullOrWhiteSpace(query)
                    ? children
                    : children.Where(x =>
                        (x.ScreenCaption ?? string.Empty).IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        (x.ScreenName ?? string.Empty).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                bool groupMatches = (group.ScreenCaption ?? string.Empty)
                    .IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0;

                if (!string.IsNullOrWhiteSpace(query) && !groupMatches && matchingChildren.Count == 0)
                    continue;

                AddNavigationGroup(group, string.IsNullOrWhiteSpace(query) || groupMatches
                    ? children : matchingChildren);
            }

            if (navFlow.Controls.Count == 0)
                navFlow.Controls.Add(CreateNavigationHint("لا توجد قوائم متاحة أو لا توجد نتائج مطابقة."));

            navFlow.ResumeLayout();
        }

        private void AddNavigationGroup(ScreensAccessProfile group, IList<ScreensAccessProfile> children)
        {
            var groupContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Width = Math.Max(190, navFlow.ClientSize.Width - 8),
                Margin = new Padding(0, 0, 0, 9),
                Padding = new Padding(0),
                BackColor = Navy
            };

            var groupButton = new Button
            {
                Text = "▾   " + group.ScreenCaption,
                Tag = group.ScreenName,
                Width = Math.Max(190, navFlow.ClientSize.Width - 18),
                Height = 43,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(12, 0, 16, 0),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = NavyLight,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            groupButton.FlatAppearance.BorderSize = 0;

            var childPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Width = Math.Max(180, groupContainer.Width),
                BackColor = Navy,
                Margin = new Padding(0),
                Padding = new Padding(5, 5, 5, 4),
                Visible = true
            };

            foreach (var child in children)
            {
                bool canOpen = Session.User != null &&
                    (Session.User.UserType == (byte)Master.UserType.Admin || child.CanOpen);
                var button = new Button
                {
                    Text = "     " + child.ScreenCaption,
                    Tag = child.ScreenName,
                    Width = Math.Max(175, childPanel.Width - 10),
                    Height = 38,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(8, 0, 12, 0),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = canOpen ? Color.FromArgb(226, 234, 244) : Color.FromArgb(118, 134, 154),
                    BackColor = Navy,
                    Enabled = canOpen,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(48, 72, 105);
                button.Click += NavigationItem_Click;
                childPanel.Controls.Add(button);
            }

            groupButton.Click += (s, e) =>
            {
                childPanel.Visible = !childPanel.Visible;
                groupButton.Text = (childPanel.Visible ? "▾   " : "▸   ") + group.ScreenCaption;
            };

            groupContainer.Controls.Add(groupButton);
            groupContainer.Controls.Add(childPanel);
            navFlow.Controls.Add(groupContainer);
        }

        private Control CreateNavigationHint(string message)
        {
            return new Label
            {
                Text = message,
                AutoSize = false,
                Width = Math.Max(180, navFlow.ClientSize.Width - 20),
                Height = 58,
                Padding = new Padding(12),
                ForeColor = Color.FromArgb(205, 216, 229),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleRight
            };
        }

        private void NavigationItem_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null || button.Tag == null) return;
            OpenFormByName(Convert.ToString(button.Tag));
        }

        private void TxtNavigationSearch_TextChanged(object sender, EventArgs e)
        {
            BuildNavigation();
        }

        private void BuildDashboard()
        {
            dashboardPanel.SuspendLayout();
            dashboardPanel.Controls.Clear();

            var intro = new Panel
            {
                Dock = DockStyle.Top,
                Height = 138,
                BackColor = Color.White,
                Padding = new Padding(26, 20, 26, 18),
                Margin = new Padding(0, 0, 0, 18)
            };

            var title = new Label
            {
                Text = "مرحباً بك في SalesLinq",
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 19F, FontStyle.Bold),
                ForeColor = Navy,
                TextAlign = ContentAlignment.MiddleRight
            };
            var subtitle = new Label
            {
                Text = "إدارة المبيعات والمشتريات والمخزون والحسابات من مكان واحد.",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextMuted,
                TextAlign = ContentAlignment.MiddleRight
            };
            var date = new Label
            {
                Text = DateTime.Now.ToString("dddd، dd MMMM yyyy"),
                Dock = DockStyle.Bottom,
                Height = 26,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Accent,
                TextAlign = ContentAlignment.MiddleRight
            };
            intro.Controls.Add(date);
            intro.Controls.Add(subtitle);
            intro.Controls.Add(title);

            var quickTitle = new Label
            {
                Text = "الوصول السريع",
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextPrimary,
                TextAlign = ContentAlignment.MiddleRight
            };

            quickActionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 230,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                Padding = new Padding(0, 4, 0, 8),
                BackColor = PageBackground
            };
            AddQuickAction(quickActionsPanel, "فاتورة مبيعات", "frm_SalesInvoice", "إنشاء فاتورة جديدة");
            AddQuickAction(quickActionsPanel, "فاتورة مشتريات", "frm_PurchaseInvoice", "تسجيل عملية شراء");
            AddQuickAction(quickActionsPanel, "الأصناف", "frm_ProductList", "عرض وإدارة الأصناف");
            AddQuickAction(quickActionsPanel, "العملاء", "frm_CustomerList", "قائمة العملاء");

            var openTitle = new Label
            {
                Text = "النوافذ المفتوحة",
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextPrimary,
                TextAlign = ContentAlignment.MiddleRight
            };

            dashboardPanel.Controls.Add(openFormsPanel);
            dashboardPanel.Controls.Add(openTitle);
            dashboardPanel.Controls.Add(quickActionsPanel);
            dashboardPanel.Controls.Add(quickTitle);
            dashboardPanel.Controls.Add(intro);
            dashboardPanel.ResumeLayout();
        }

        private void RefreshQuickActions()
        {
            if (quickActionsPanel == null) return;

            foreach (Button button in quickActionsPanel.Controls.OfType<Button>())
            {
                string screenName = Convert.ToString(button.Tag);
                button.Enabled = Session.User != null &&
                    (Session.User.UserType == (byte)Master.UserType.Admin ||
                     (Session.ScreenAccesses != null &&
                      Session.ScreenAccesses.Any(x => x.ScreenName == screenName && x.CanShow && x.CanOpen)));
            }
        }

        private void AddQuickAction(FlowLayoutPanel panel, string caption, string screenName, string description)
        {
            var button = new Button
            {
                Text = caption + Environment.NewLine + description,
                Tag = screenName,
                Width = 215,
                Height = 88,
                Margin = new Padding(0, 0, 12, 10),
                Padding = new Padding(12),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 239);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 246, 245);
            button.Click += (s, e) => OpenFormByName(screenName);
            bool allowed = Session.User != null &&
                (Session.User.UserType == (byte)Master.UserType.Admin ||
                 (Session.ScreenAccesses != null && Session.ScreenAccesses.Any(x => x.ScreenName == screenName && x.CanShow)));
            button.Enabled = allowed;
            panel.Controls.Add(button);
        }

        private void RefreshOpenWindows()
        {
            if (openFormsPanel == null || openFormsPanel.IsDisposed) return;

            openFormsPanel.SuspendLayout();
            openFormsPanel.Controls.Clear();
            var alive = _openForms.Where(x => x != null && !x.IsDisposed).ToList();
            _openForms.Clear();
            _openForms.AddRange(alive);

            if (alive.Count == 0)
            {
                openFormsPanel.Controls.Add(new Label
                {
                    Text = "لا توجد نوافذ مفتوحة حالياً. اختر إحدى الوظائف من القائمة الجانبية.",
                    AutoSize = false,
                    Width = 680,
                    Height = 52,
                    ForeColor = TextMuted,
                    Font = new Font("Segoe UI", 9F),
                    TextAlign = ContentAlignment.MiddleRight
                });
            }
            else
            {
                foreach (var form in alive)
                {
                    var item = new Button
                    {
                        Text = "▣  " + form.Text,
                        Tag = form,
                        Width = 220,
                        Height = 42,
                        Margin = new Padding(0, 0, 10, 8),
                        TextAlign = ContentAlignment.MiddleRight,
                        Padding = new Padding(8),
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = TextPrimary,
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    item.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 239);
                    item.Click += (s, e) =>
                    {
                        var target = (s as Button)?.Tag as Form;
                        if (target != null && !target.IsDisposed)
                        {
                            if (target.WindowState == FormWindowState.Minimized)
                                target.WindowState = FormWindowState.Normal;
                            target.Activate();
                        }
                    };
                    openFormsPanel.Controls.Add(item);
                }
            }

            openFormsPanel.ResumeLayout();
        }

        private void RegisterOpenForm(Form form)
        {
            if (!_openForms.Contains(form))
                _openForms.Add(form);

            form.FormClosed += (s, e) =>
            {
                _openForms.Remove(form);
                RefreshOpenWindows();
            };
            form.TextChanged += (s, e) => RefreshOpenWindows();
            RefreshOpenWindows();
        }

        public static void OpenFormByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            var existing = Application.OpenForms.Cast<Form>()
                .FirstOrDefault(x => x != Instance && !x.IsDisposed && x.Name == name);
            if (existing != null)
            {
                if (existing.WindowState == FormWindowState.Minimized)
                    existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                return;
            }

            Form form = null;
            try
            {
                switch (name)
                {
                case "frm_Vendor": form = new frm_CustomerVendor(false); break;
                case "frm_Customer": form = new frm_CustomerVendor(true); break;
                case "frm_VendorList": form = new frm_CustomerVendorList(false); break;
                case "frm_CustomerList": form = new frm_CustomerVendorList(true); break;
                case "frm_PurchaseInvoice": form = new frm_Invoice(Master.InvoiceType.Purchase); break;
                case "frm_SalesInvoice": form = new frm_Invoice(Master.InvoiceType.Sales); break;
                case "frm_PurchaseReturnInvoice": form = new frm_Invoice(Master.InvoiceType.PurchaseReturn); break;
                case "frm_SalesReturnInvoice": form = new frm_Invoice(Master.InvoiceType.SalesReturn); break;
                case "frm_PurchaseInvoiceList": form = new frm_InvoiceList(Master.InvoiceType.Purchase); break;
                case "frm_SalesInvoiceList": form = new frm_InvoiceList(Master.InvoiceType.Sales); break;
                case "frm_PurchaseReturnInvoiceList": form = new frm_InvoiceList(Master.InvoiceType.PurchaseReturn); break;
                case "frm_SalesReturnInvoiceList": form = new frm_InvoiceList(Master.InvoiceType.SalesReturn); break;
                default:
                    var type = Assembly.GetExecutingAssembly().GetTypes()
                        .FirstOrDefault(x => x.Name == name && typeof(Form).IsAssignableFrom(x));
                    if (type != null)
                        form = Activator.CreateInstance(type) as Form;
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر إنشاء الشاشة المطلوبة.\\n" + ex.Message,
                    "التنقل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (form == null)
            {
                MessageBox.Show("تعذر العثور على الشاشة المطلوبة: " + name,
                    "التنقل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            form.Name = name;
            OpenForm(form);
        }

        public static void OpenForm(Form form, bool OpenInDialog = false)
        {
            if (form == null) return;

            var user = Session.User;
            if (user == null)
            {
                MessageBox.Show("يجب تسجيل الدخول أولاً.", "الصلاحيات",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                form.Dispose();
                return;
            }

            if (user.UserType != (byte)Master.UserType.Admin)
            {
                var screen = Session.ScreenAccesses == null
                    ? null
                    : Session.ScreenAccesses.SingleOrDefault(x => x.ScreenName == form.Name);

                if (screen == null || !screen.CanOpen)
                {
                    MessageBox.Show("غير مصرح لك بفتح هذه الشاشة.", "الصلاحيات",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    form.Dispose();
                    return;
                }
            }

            var main = Instance;
            try
            {
                form.Icon = main.Icon;
                if (OpenInDialog)
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(main);
                    form.Dispose();
                    return;
                }

                var existing = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(x => x != main && !x.IsDisposed && x.Name == form.Name);
                if (existing != null)
                {
                    if (existing == form)
                    {
                        existing.Activate();
                        return;
                    }
                    form.Dispose();
                    if (existing.WindowState == FormWindowState.Minimized)
                        existing.WindowState = FormWindowState.Normal;
                    existing.Activate();
                    return;
                }

                form.StartPosition = FormStartPosition.CenterScreen;
                main.RegisterOpenForm(form);
                form.Show(main);
            }
            catch (Exception ex)
            {
                if (!form.IsDisposed) form.Dispose();
                MessageBox.Show("تعذر فتح الشاشة.\n" + ex.Message,
                    "التنقل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_SignOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل تريد تسجيل الخروج من النظام؟", "تسجيل الخروج",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            foreach (var form in _openForms.ToList())
            {
                if (form != null && !form.IsDisposed)
                    form.Close();
            }
            _openForms.Clear();
            Session.ClearUser();
            Hide();

            using (var login = new frm_Login())
                login.ShowDialog(this);

            if (Session.User == null)
            {
                Application.Exit();
                return;
            }

            Show();
            AddAccordionElement();
        }

        private void Btn_CloseCurrent_Click(object sender, EventArgs e)
        {
            var active = _openForms.LastOrDefault(x => x != null && !x.IsDisposed && x.ContainsFocus);
            if (active == null)
                active = _openForms.LastOrDefault(x => x != null && !x.IsDisposed);
            if (active != null)
                active.Close();
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;

            if (MessageBox.Show("هل أنت متأكد أنك تريد إغلاق التطبيق؟", "تأكيد الإغلاق",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            e.Cancel = false;
            Application.Exit();
        }
    }
}