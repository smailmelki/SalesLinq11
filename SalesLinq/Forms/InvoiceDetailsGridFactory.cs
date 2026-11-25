using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SalesLinq.Forms
{
    /// <summary>تهيئة جدول تفاصيل الفاتورة بعناصر WinForms القياسية فقط.</summary>
    internal static class InvoiceDetailsGridFactory
    {
        internal static DataGridView Create()
        {
            var grid = new DataGridView
            {
                Name = "invoiceDetailsGrid", Dock = DockStyle.Fill,
                AutoGenerateColumns = false, AllowUserToAddRows = true,
                AllowUserToDeleteRows = true, AllowUserToResizeRows = false,
                MultiSelect = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EditMode = DataGridViewEditMode.EditOnEnter, RowHeadersVisible = false,
                RightToLeft = RightToLeft.Yes, BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.None, GridColor = Color.FromArgb(225, 231, 239),
                EnableHeadersVisualStyles = false, ColumnHeadersHeight = 40,
                RowTemplate = { Height = 34 }, Font = new Font("Segoe UI", 9F),
                ColumnHeadersDefaultCellStyle =
                {
                    BackColor = Color.FromArgb(232, 238, 246),
                    ForeColor = Color.FromArgb(38, 52, 72),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleRight
                },
                DefaultCellStyle =
                {
                    BackColor = Color.White, ForeColor = Color.FromArgb(38, 52, 72),
                    SelectionBackColor = Color.FromArgb(218, 240, 238),
                    SelectionForeColor = Color.FromArgb(27, 45, 72),
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(4, 1, 4, 1)
                },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(248, 250, 252) }
            };

            Text(grid, "Index", "م", 35, false);
            Text(grid, "Code", "الكود", 85, true);
            Text(grid, "ItemID", "الصنف", 210, true);
            Text(grid, "ItemUnitID", "الوحدة", 85, true);
            Number(grid, "ItemQty", "الكمية", 85, true, "N3");
            Number(grid, "Price", "السعر", 100, true, "N2");
            Number(grid, "Discount", "نسبة الخصم", 95, true, "P2");
            Number(grid, "DiscountValue", "قيمة الخصم", 100, true, "N2");
            Number(grid, "TotalPrice", "الإجمالي", 110, false, "N2");
            Number(grid, "CostValue", "التكلفة", 100, false, "N2");
            Number(grid, "TotalCostValue", "إجمالي التكلفة", 115, false, "N2");
            Text(grid, "StoreID", "المخزن", 120, true);
            Number(grid, "Balance", "الرصيد", 90, false, "N3");
            Hidden(grid, "ID");
            Hidden(grid, "InvoiceID");
            Hidden(grid, "sourceRowID");
            return grid;
        }

        /// <summary>
        /// ربط الجدول بكيانات تفاصيل الفاتورة وقوائم الأصناف والوحدات والمخازن.
        /// تبقى الكيانات نفسها مصدر البيانات حتى لا ينفصل تعديل المستخدم عن كيان LINQ to SQL.
        /// </summary>
        internal static void Bind(
            DataGridView grid,
            IList<DAL.InvoiceDetail> details,
            object products,
            object units,
            object stores)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (details == null) throw new ArgumentNullException(nameof(details));

            var source = new BindingSource { DataSource = details };
            grid.DataSource = source;

            ReplaceWithLookup(grid, "ItemID", products, "Name", "ID");
            ReplaceWithLookup(grid, "ItemUnitID", units, "Name", "ID");
            ReplaceWithLookup(grid, "StoreID", stores, "Name", "ID");
            grid.DataError += (sender, e) => { e.ThrowException = false; };
        }

        private static void ReplaceWithLookup(
            DataGridView grid, string field, object source, string displayMember, string valueMember)
        {
            if (!grid.Columns.Contains(field) || source == null) return;

            int index = grid.Columns[field].Index;
            int width = grid.Columns[field].Width;
            string header = grid.Columns[field].HeaderText;
            bool readOnly = grid.Columns[field].ReadOnly;
            grid.Columns.Remove(field);

            var lookup = new DataGridViewComboBoxColumn
            {
                Name = field,
                DataPropertyName = field,
                HeaderText = header,
                DataSource = source,
                DisplayMember = displayMember,
                ValueMember = valueMember,
                Width = width,
                ReadOnly = readOnly,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            grid.Columns.Insert(index, lookup);
        }

        internal static void AddReturnColumns(DataGridView grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (!grid.Columns.Contains("SourceQty"))
                Number(grid, "SourceQty", "الكمية الأصلية", 105, false, "N3");
            if (!grid.Columns.Contains("OtherQty"))
                Number(grid, "OtherQty", "المردود سابقاً", 110, false, "N3");
        }

        private static void Text(DataGridView grid, string field, string caption, int width, bool editable)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = field, DataPropertyName = field, HeaderText = caption, Width = width,
                ReadOnly = !editable, SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        private static void Number(DataGridView grid, string field, string caption, int width, bool editable, string format)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = field, DataPropertyName = field, HeaderText = caption, Width = width,
                ReadOnly = !editable, SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = { Format = format, Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private static void Hidden(DataGridView grid, string field)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = field, DataPropertyName = field, Visible = false
            });
        }
    }
}
