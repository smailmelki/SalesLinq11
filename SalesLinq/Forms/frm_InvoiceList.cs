using SalesLinq.Class;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    /// <summary>
    /// قائمة الفواتير بعناصر WinForms القياسية. تبقى عمليات الحفظ والحذف والطباعة
    /// في شاشة الفاتورة الحالية حفاظاً على قواعد المحاسبة والمخزون.
    /// </summary>
    public partial class frm_InvoiceList : NativeMasterListForm
    {
        private readonly Master.InvoiceType Type;
        public Master.InvoiceType InvoiceType { get { return Type; } }
        private readonly DateTimePicker fromDate = new DateTimePicker();
        private readonly DateTimePicker toDate = new DateTimePicker();
        private readonly ComboBox partType = new ComboBox();
        private readonly ComboBox part = new ComboBox();
        private readonly ComboBox branch = new ComboBox();
        private readonly ComboBox drawer = new ComboBox();
        private readonly Button applyFilters = new Button();
        private readonly Button clearFilters = new Button();
        private readonly Button toggleFilters = new Button();
        private readonly Label filterStatus = new Label();
        private readonly Panel filterPanel = new Panel();
        private readonly DataGridView detailsGrid = new DataGridView();
        private readonly Label detailsCaption = new Label();
        private bool isRefreshing;
        private readonly SplitContainer contentSplit = new SplitContainer();
        private readonly Dictionary<int, string> partNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> branchNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> drawerNames = new Dictionary<int, string>();
        private readonly PrintDocument listPrint = new PrintDocument();
        private int printRow;

        public frm_InvoiceList(Master.InvoiceType type)
        {
            InitializeComponent();
            ClientSize = new Size(1080, 700);
            MinimumSize = new Size(900, 600);
            Type = type;
            Text = GetTitle(type);
            Name = GetScreenName(type);
            ConfigureControls();
            LoadFilterData();
            Load += InvoiceList_Load;
        }

        private static string GetTitle(Master.InvoiceType type)
        {
            switch (type)
            {
                case Master.InvoiceType.Purchase: return "فواتير المشتريات";
                case Master.InvoiceType.Sales: return "فواتير المبيعات";
                case Master.InvoiceType.PurchaseReturn: return "فواتير مردود المشتريات";
                case Master.InvoiceType.SalesReturn: return "فواتير مردود المبيعات";
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private static string GetScreenName(Master.InvoiceType type)
        {
            switch (type)
            {
                case Master.InvoiceType.Purchase: return Screens.ViewPurchaseInvoice.ScreenName;
                case Master.InvoiceType.Sales: return Screens.ViewSalesInvoice.ScreenName;
                case Master.InvoiceType.PurchaseReturn: return Screens.ViewPurchaseReturnInvoice.ScreenName;
                case Master.InvoiceType.SalesReturn: return Screens.ViewSalesReturnInvoice.ScreenName;
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private void ConfigureControls()
        {
            gridView1.MultiSelect = true;
            gridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridView1.Columns.Clear();
            gridView1.CellFormatting += Grid_CellFormatting;
            gridView1.SelectionChanged += Grid_SelectionChanged;
            gridView1.ContextMenuStrip = CreateContextMenu();
            listPrint.DefaultPageSettings.Landscape = true;
            listPrint.BeginPrint += (s, e) => printRow = 0;
            listPrint.PrintPage += PrintListPage;

            contentSplit.Dock = DockStyle.Fill;
            contentSplit.Orientation = Orientation.Horizontal;
            contentSplit.SplitterDistance = 300;
            contentSplit.Panel1.Controls.Add(gridView1);
            contentSplit.Panel2.Controls.Add(detailsGrid);
            detailsGrid.Dock = DockStyle.Fill;
            detailsCaption.Text = "تفاصيل أصناف الفاتورة المحددة";
            detailsCaption.Dock = DockStyle.Top;
            detailsCaption.Height = 30;
            detailsCaption.TextAlign = ContentAlignment.MiddleRight;
            detailsCaption.Padding = new Padding(8, 0, 8, 0);
            detailsCaption.BackColor = Color.FromArgb(244, 247, 251);
            detailsCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            contentSplit.Panel2.Controls.Add(detailsCaption);
            detailsCaption.BringToFront();
            detailsGrid.ReadOnly = true;
            detailsGrid.AllowUserToAddRows = false;
            detailsGrid.AllowUserToDeleteRows = false;
            detailsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            detailsGrid.RowHeadersVisible = false;
            detailsGrid.RightToLeft = RightToLeft.Yes;
            detailsGrid.BackgroundColor = SystemColors.Window;
            detailsGrid.BorderStyle = BorderStyle.None;
            detailsGrid.EnableHeadersVisualStyles = false;
            detailsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(232, 238, 246);
            detailsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(38, 52, 72);
            detailsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            detailsGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            filterPanel.Dock = DockStyle.Top;
            filterPanel.Height = 150;
            filterPanel.Padding = new Padding(8);
            filterPanel.BackColor = Color.White;
            Controls.Add(contentSplit);
            Controls.Add(filterPanel);
            filterPanel.BringToFront();

            ConfigureFilterControl(fromDate, "من تاريخ", 8, 6, 245);
            ConfigureFilterControl(toDate, "حتى تاريخ", 275, 6, 245);
            ConfigureFilterControl(partType, "نوع الطرف", 542, 6, 245);
            ConfigureFilterControl(part, "الطرف", 8, 62, 245);
            ConfigureFilterControl(branch, "الفرع", 275, 62, 245);
            ConfigureFilterControl(drawer, "الخزنة", 542, 62, 245);

            applyFilters.Text = "تطبيق";
            applyFilters.SetBounds(8, 118, 85, 26);
            applyFilters.Click += (s, e) => RefreshData();
            clearFilters.Text = "مسح";
            clearFilters.SetBounds(100, 118, 85, 26);
            clearFilters.Click += (s, e) => ClearFilters();
            toggleFilters.Text = "إخفاء الفلاتر";
            toggleFilters.SetBounds(192, 118, 120, 26);
            toggleFilters.Click += (s, e) => filterPanel.Visible = !filterPanel.Visible;
            filterStatus.AutoSize = false;
            filterStatus.SetBounds(325, 118, 450, 26);
            filterStatus.TextAlign = ContentAlignment.MiddleRight;
            filterPanel.Controls.AddRange(new Control[] { applyFilters, clearFilters, toggleFilters, filterStatus });

            fromDate.Format = DateTimePickerFormat.Custom;
            fromDate.CustomFormat = "yyyy/MM/dd";
            toDate.Format = DateTimePickerFormat.Custom;
            toDate.CustomFormat = "yyyy/MM/dd";
            fromDate.ShowCheckBox = toDate.ShowCheckBox = true;
            fromDate.Checked = toDate.Checked = false;
            partType.DropDownStyle = part.DropDownStyle = branch.DropDownStyle = drawer.DropDownStyle = ComboBoxStyle.DropDownList;
            part.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private static void ConfigureFilterControl(Control control, string caption, int x, int y, int width)
        {
            var label = new Label
            {
                Text = caption,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Bounds = new Rectangle(x, y, width, 22),
                ForeColor = Color.FromArgb(38, 52, 72),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
            control.SetBounds(x, y + 25, width, 30);
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            control.Font = new Font("Segoe UI", 9F);
            control.RightToLeft = RightToLeft.Yes;
            // Parent is assigned by the caller after this helper returns.
            control.Tag = label;
        }

        private void LoadFilterData()
        {
            partType.Items.Clear();
            partType.Items.Add(new FilterChoice(null, "الكل"));
            partType.Items.Add(new FilterChoice((int)Master.PartType.Customer, "عميل"));
            partType.Items.Add(new FilterChoice((int)Master.PartType.Vendor, "مورد"));
            partType.SelectedIndex = 0;
            partType.SelectedIndexChanged += (s, e) => LoadPartChoices();

            var stores = Session.Store.ToList();
            branch.DataSource = stores.ToList();
            branch.DisplayMember = "Name";
            branch.ValueMember = "ID";
            SetComboAll(branch);

            var drawers = Session.Drawer.ToList();
            drawer.DataSource = drawers.ToList();
            drawer.DisplayMember = "Name";
            drawer.ValueMember = "ID";
            SetComboAll(drawer);

            // Add the filter labels and controls together, respecting RTL layout.
            foreach (Control c in new Control[] { fromDate, toDate, partType, part, branch, drawer })
            {
                var label = c.Tag as Label;
                if (label != null) filterPanel.Controls.Add(label);
                filterPanel.Controls.Add(c);
            }
            LoadPartChoices();
        }

        private static void SetComboAll(ComboBox combo)
        {
            combo.SelectedIndex = -1;
        }

        private void LoadPartChoices()
        {
            var customersAndVendors = new List<DAL.CustomersAndVendore>();
            if (Session.Customers != null) customersAndVendors.AddRange(Session.Customers);
            if (Session.vendores != null) customersAndVendors.AddRange(Session.vendores);

            int? selectedType = (partType.SelectedItem as FilterChoice)?.Value;
            var choices = customersAndVendors
                .Where(x => !selectedType.HasValue || (selectedType.Value == (int)Master.PartType.Customer && x.IsCustomer) || (selectedType.Value == (int)Master.PartType.Vendor && !x.IsCustomer))
                .Select(x => new FilterChoice(x.ID, x.Name))
                .OrderBy(x => x.Text)
                .ToList();
            choices.Insert(0, new FilterChoice(null, "الكل"));
            part.DataSource = choices;
            part.DisplayMember = "Text";
            part.ValueMember = "Value";
            part.SelectedIndex = 0;
        }

        private void InvoiceList_Load(object sender, EventArgs e)
        {
            btn_Delete.Visible = true;
            btn_Print.Visible = true;
            btn_Refresh.Visible = true;
            btn_New.Visible = true;
            toolStrip.BringToFront();
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                var query = db.InvoiceHeaders.Where(x => x.InvoiceType == (byte)Type);
                if (fromDate.Checked)
                    query = query.Where(x => x.Date >= fromDate.Value.Date);
                if (toDate.Checked)
                {
                    var endDate = toDate.Value.Date.AddDays(1);
                    query = query.Where(x => x.Date < endDate);
                }

                var selectedPartType = (partType.SelectedItem as FilterChoice)?.Value;
                if (selectedPartType.HasValue)
                    query = query.Where(x => x.PartType == selectedPartType.Value);

                var selectedPart = (part.SelectedItem as FilterChoice)?.Value;
                if (selectedPart.HasValue)
                    query = query.Where(x => x.PartID == selectedPart.Value);

                if (branch.SelectedValue != null && branch.SelectedValue != DBNull.Value)
                {
                    int id;
                    if (int.TryParse(Convert.ToString(branch.SelectedValue), out id))
                        query = query.Where(x => x.Branch == id);
                }
                if (drawer.SelectedValue != null && drawer.SelectedValue != DBNull.Value)
                {
                    int id;
                    if (int.TryParse(Convert.ToString(drawer.SelectedValue), out id))
                        query = query.Where(x => x.Drawer == id);
                }

                var rows = query.OrderByDescending(x => x.Date).ThenByDescending(x => x.ID)
                    .Select(x => new InvoiceListRow
                    {
                        ID = x.ID,
                        Code = x.Code,
                        Date = x.Date,
                        PartType = x.PartType,
                        PartID = x.PartID,
                        Branch = x.Branch,
                        ItemsCount = db.InvoiceDetails.Count(d => d.InvoiceID == x.ID),
                        PostedToStore = x.PostedToStore,
                        Total = x.Total,
                        DiscountRation = x.DiscountRation,
                        DiscountValue = x.DiscountValue,
                        Tax = x.Tax,
                        TaxValue = x.TaxValue,
                        Expences = x.Expences,
                        Net = x.Net,
                        Paid = x.Paid,
                        Drawer = x.Drawer,
                        Remaing = x.Remaing,
                        PayStatus = x.Remaing == 0 ? "مسددة" : x.Remaing == x.Net ? "غير مسددة" : "مسددة جزئياً"
                    }).ToList();

                partNames.Clear();
                branchNames.Clear();
                drawerNames.Clear();
                if (Session.Customers != null)
                    foreach (var item in Session.Customers) partNames[item.ID] = item.Name;
                if (Session.vendores != null)
                    foreach (var item in Session.vendores) partNames[item.ID] = item.Name;
                if (Session.Store != null)
                    foreach (var item in Session.Store) branchNames[item.ID] = item.Name;
                if (Session.Drawer != null)
                    foreach (var item in Session.Drawer) drawerNames[item.ID] = item.Name;

                isRefreshing = true;
                gridView1.DataSource = rows;
                ConfigureColumns();
                isRefreshing = false;
                filterStatus.Text = "عدد الفواتير: " + rows.Count;
                LoadDetailsForSelectedRow();
            }
        }

        private void ConfigureColumns()
        {
            string[] captions =
            {
                "ID|كود", "Code|الرقم", "Date|التاريخ", "PartType|نوع الطرف", "PartID|اسم الطرف",
                "Branch|الفرع", "ItemsCount|عدد الأصناف", "PostedToStore|مرحل للمخزن",
                "Total|الإجمالي", "DiscountValue|قيمة الخصم", "DiscountRation|نسبة الخصم",
                "Tax|نسبة الضريبة", "TaxValue|قيمة الضريبة", "Expences|مصروفات",
                "Net|الصافي", "Paid|المدفوع", "Drawer|الخزنة", "Remaing|المتبقي", "PayStatus|حالة السداد"
            };
            foreach (var pair in captions)
            {
                var values = pair.Split('|');
                if (gridView1.Columns.Contains(values[0]))
                    gridView1.Columns[values[0]].HeaderText = values[1];
            }
            if (gridView1.Columns.Contains("Date"))
                gridView1.Columns["Date"].DefaultCellStyle.Format = "dd-MM-yyyy hh:mm tt";
            if (gridView1.Columns.Contains("PostedToStore"))
                gridView1.Columns["PostedToStore"].HeaderText = "مرحل للمخزن";
            gridView1.RightToLeft = RightToLeft.Yes;
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var columnName = gridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "PayStatus")
            {
                var status = Convert.ToString(e.Value);
                e.CellStyle.ForeColor = Color.FromArgb(38, 52, 72);
                e.CellStyle.BackColor = status == "مسددة" ? Color.FromArgb(218, 240, 226) :
                    status == "مسددة جزئياً" ? Color.FromArgb(255, 242, 204) : Color.FromArgb(255, 224, 224);
                return;
            }
            if (columnName == "PostedToStore")
            {
                e.Value = Convert.ToBoolean(e.Value) ? "نعم" : "لا";
                e.FormattingApplied = true;
            }
            if (columnName == "PartType")
            {
                int value;
                if (int.TryParse(Convert.ToString(e.Value), out value))
                {
                    e.Value = value == (int)Master.PartType.Customer ? "عميل" :
                              value == (int)Master.PartType.Vendor ? "مورد" : Convert.ToString(e.Value);
                    e.FormattingApplied = true;
                }
            }
            else if (columnName == "PartID")
            {
                int id;
                if (int.TryParse(Convert.ToString(e.Value), out id) && partNames.ContainsKey(id))
                {
                    e.Value = partNames[id];
                    e.FormattingApplied = true;
                }
            }
            else if (columnName == "Branch")
            {
                int id;
                if (int.TryParse(Convert.ToString(e.Value), out id) && branchNames.ContainsKey(id))
                {
                    e.Value = branchNames[id];
                    e.FormattingApplied = true;
                }
            }
            else if (columnName == "Drawer")
            {
                int id;
                if (int.TryParse(Convert.ToString(e.Value), out id) && drawerNames.ContainsKey(id))
                {
                    e.Value = drawerNames[id];
                    e.FormattingApplied = true;
                }
            }
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (!isRefreshing) LoadDetailsForSelectedRow();
        }

        private void LoadDetailsForSelectedRow()
        {
            if (gridView1.CurrentRow == null || !gridView1.Columns.Contains("ID"))
            {
                detailsGrid.DataSource = null;
                return;
            }
            int invoiceId;
            if (!int.TryParse(Convert.ToString(gridView1.CurrentRow.Cells["ID"].Value), out invoiceId))
                return;

            using (var db = new DAL.DBDataContext())
            {
                var details = (from item in db.InvoiceDetails.Where(x => x.InvoiceID == invoiceId)
                               join product in db.Products on item.ItemID equals product.ID into products
                               from product in products.DefaultIfEmpty()
                               join unit in db.UnitNames on item.ItemUnitID equals unit.ID into units
                               from unit in units.DefaultIfEmpty()
                               select new InvoiceDetailListRow
                               {
                                   Product = product == null ? "" : product.Name,
                                   Unit = unit == null ? "" : unit.Name,
                                   Price = item.Price,
                                   Quantity = item.ItemQty,
                                   Discount = item.DiscountValue,
                                   Total = item.TotalCostValue
                               }).ToList();
                detailsGrid.DataSource = details;
                if (detailsGrid.Columns.Contains("Product")) detailsGrid.Columns["Product"].HeaderText = "الصنف";
                if (detailsGrid.Columns.Contains("Unit")) detailsGrid.Columns["Unit"].HeaderText = "الوحدة";
                if (detailsGrid.Columns.Contains("Price")) detailsGrid.Columns["Price"].HeaderText = "السعر";
                if (detailsGrid.Columns.Contains("Quantity")) detailsGrid.Columns["Quantity"].HeaderText = "الكمية";
                if (detailsGrid.Columns.Contains("Discount")) detailsGrid.Columns["Discount"].HeaderText = "الخصم";
                if (detailsGrid.Columns.Contains("Total")) detailsGrid.Columns["Total"].HeaderText = "الإجمالي";
            }
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("فتح الفاتورة", null, (s, e) => OpenSelectedInvoice());
            menu.Items.Add("طباعة الفواتير المحددة", null, (s, e) => PrintSelectedInvoices());
            menu.Items.Add("حذف المحدد", null, (s, e) => Delete());
            return menu;
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_Invoice(Type, id));
        }

        public override void New()
        {
            frm_Main.OpenForm(new frm_Invoice(Type));
        }

        private void OpenSelectedInvoice()
        {
            if (gridView1.CurrentRow == null) return;
            int id;
            if (int.TryParse(Convert.ToString(gridView1.CurrentRow.Cells["ID"].Value), out id) && id > 0)
                OpenForm(id);
        }

        private List<int> GetSelectedInvoiceIds()
        {
            var ids = new List<int>();
            foreach (DataGridViewRow row in gridView1.SelectedRows)
            {
                int id;
                if (row.IsNewRow || !int.TryParse(Convert.ToString(row.Cells["ID"].Value), out id) ||
                    id <= 0 || ids.Contains(id)) continue;
                ids.Add(id);
            }
            if (ids.Count == 0)
            {
                MessageBox.Show("حدد فاتورة واحدة على الأقل.", "قائمة الفواتير",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return ids;
        }

        public override void Print()
        {
            if (!CheckActionAuthorization(Name, Master.Actions.Print)) return;
            if (gridView1.Rows.Count == 0) return;
            printRow = 0;
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = listPrint;
                preview.Width = 1050;
                preview.Height = 720;
                preview.ShowDialog(this);
            }
        }

        private void PrintSelectedInvoices()
        {
            var ids = GetSelectedInvoiceIds();
            if (ids != null) frm_Invoice.Print(ids, Type, Name);
        }

        private void PrintListPage(object sender, PrintPageEventArgs e)
        {
            using (var title = new Font("Segoe UI", 13F, FontStyle.Bold))
            using (var header = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            using (var cellFont = new Font("Segoe UI", 7F))
            using (var pen = new Pen(Color.LightGray))
            {
                float y = e.MarginBounds.Top;
                e.Graphics.DrawString("كشف " + Text, title, Brushes.Black, e.MarginBounds, new StringFormat { Alignment = StringAlignment.Center });
                y += 35;
                var cols = gridView1.Columns.Cast<DataGridViewColumn>().Where(x => x.Visible).ToList();
                int sum = cols.Sum(x => Math.Max(35, x.Width));
                float scale = e.MarginBounds.Width / (float)sum;
                int[] widths = cols.Select(x => Math.Max(28, (int)(Math.Max(35, x.Width) * scale))).ToArray();
                float tableWidth = widths.Sum();
                float xPos = e.MarginBounds.Right - tableWidth;
                for (int c = 0; c < cols.Count; c++)
                {
                    var rect = new RectangleF(xPos, y, widths[c], 25);
                    e.Graphics.FillRectangle(Brushes.AliceBlue, rect);
                    e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    e.Graphics.DrawString(cols[c].HeaderText, header, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    xPos += widths[c];
                }
                y += 25;
                while (printRow < gridView1.Rows.Count)
                {
                    var row = gridView1.Rows[printRow++];
                    if (row.IsNewRow) continue;
                    if (y + 22 > e.MarginBounds.Bottom)
                    {
                        printRow--;
                        e.HasMorePages = true;
                        return;
                    }
                    xPos = e.MarginBounds.Right - tableWidth;
                    for (int c = 0; c < cols.Count; c++)
                    {
                        var rect = new RectangleF(xPos, y, widths[c], 22);
                        e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                        e.Graphics.DrawString(Convert.ToString(row.Cells[cols[c].Index].FormattedValue), cellFont, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
                        xPos += widths[c];
                    }
                    y += 22;
                }
                e.HasMorePages = false;
            }
        }

        public override void Delete()
        {
            if (!CheckActionAuthorization(Name, Master.Actions.Delete)) return;
            var ids = GetSelectedInvoiceIds();
            if (ids != null) frm_Invoice.Delete(ids, Type, Name);
            RefreshData();
        }

        private void ClearFilters()
        {
            fromDate.Checked = toDate.Checked = false;
            partType.SelectedIndex = 0;
            LoadPartChoices();
            part.SelectedIndex = 0;
            branch.SelectedIndex = -1;
            drawer.SelectedIndex = -1;
            RefreshData();
        }

        private sealed class FilterChoice
        {
            public int? Value { get; private set; }
            public string Text { get; private set; }
            public FilterChoice(int? value, string text) { Value = value; Text = text; }
            public override string ToString() { return Text; }
        }

        private sealed class InvoiceListRow
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public DateTime Date { get; set; }
            public int PartType { get; set; }
            public int PartID { get; set; }
            public int Branch { get; set; }
            public int ItemsCount { get; set; }
            public bool PostedToStore { get; set; }
            public double Total { get; set; }
            public double DiscountRation { get; set; }
            public double DiscountValue { get; set; }
            public double Tax { get; set; }
            public double TaxValue { get; set; }
            public double Expences { get; set; }
            public double Net { get; set; }
            public double Paid { get; set; }
            public int Drawer { get; set; }
            public double Remaing { get; set; }
            public string PayStatus { get; set; }
        }

        private sealed class InvoiceDetailListRow
        {
            public string Product { get; set; }
            public string Unit { get; set; }
            public double Price { get; set; }
            public double Quantity { get; set; }
            public double Discount { get; set; }
            public double Total { get; set; }
        }
    }
}
