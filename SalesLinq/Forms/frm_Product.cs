using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_Product : NativeMasterForm
    {
        private DAL.Product product;
        private BindingList<DAL.ProductUnit> unitRows = new BindingList<DAL.ProductUnit>();
        private List<DAL.ProductCategory> categories = new List<DAL.ProductCategory>();
        private List<DAL.UnitName> unitNames = new List<DAL.UnitName>();

        public frm_Product()
        {
            InitializeComponent();
            ConfigureUnitsGrid();
            RefreshData();
            New();
        }

        public frm_Product(int id)
        {
            InitializeComponent();
            ConfigureUnitsGrid();
            RefreshData();
            LoadProduct(id);
        }

        private void ConfigureUnitsGrid()
        {
            gridView1.AutoGenerateColumns = false;
            gridView1.Columns.Clear();
            gridView1.AllowUserToAddRows = true;
            gridView1.AllowUserToDeleteRows = false;
            gridView1.MultiSelect = false;
            gridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            gridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridView1.RowHeadersVisible = false;
            gridView1.RightToLeft = RightToLeft.Yes;

            var idColumn = new DataGridViewTextBoxColumn
            {
                Name = "ID",
                DataPropertyName = nameof(DAL.ProductUnit.ID),
                Visible = false
            };
            gridView1.Columns.Add(idColumn);

            var productIdColumn = new DataGridViewTextBoxColumn
            {
                Name = "ProductID",
                DataPropertyName = nameof(DAL.ProductUnit.ProductID),
                Visible = false
            };
            gridView1.Columns.Add(productIdColumn);

            var unitColumn = new DataGridViewComboBoxColumn
            {
                Name = nameof(DAL.ProductUnit.UnitID),
                HeaderText = "اسم الوحدة",
                DataPropertyName = nameof(DAL.ProductUnit.UnitID),
                ValueMember = nameof(DAL.UnitName.ID),
                DisplayMember = nameof(DAL.UnitName.Name),
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 125
            };
            unitColumn.DataSource = unitNames;
            gridView1.Columns.Add(unitColumn);

            AddNumberColumn(nameof(DAL.ProductUnit.Factor), "معامل التحويل", 85);
            AddNumberColumn(nameof(DAL.ProductUnit.SellPrice), "سعر البيع", 95);
            AddNumberColumn(nameof(DAL.ProductUnit.BuyPrice), "سعر الشراء", 95);
            AddNumberColumn(nameof(DAL.ProductUnit.SellDiscount), "خصم البيع", 85);

            gridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(DAL.ProductUnit.Barcode),
                HeaderText = "الباركود",
                DataPropertyName = nameof(DAL.ProductUnit.Barcode),
                FillWeight = 125
            });

            gridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "DeleteRow",
                HeaderText = "حذف",
                Text = "حذف",
                UseColumnTextForButtonValue = true,
                FillWeight = 55
            });

            gridView1.CellContentClick += GridView1_CellContentClick;
            gridView1.DataError += (s, e) => { e.ThrowException = false; };
            gridView1.CellValidating += GridView1_CellValidating;
        }

        private void AddNumberColumn(string propertyName, string caption, float width)
        {
            gridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = propertyName,
                HeaderText = caption,
                DataPropertyName = propertyName,
                ValueType = typeof(double),
                FillWeight = width
            });
        }

        private void GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridView1.Columns[e.ColumnIndex].Name != "DeleteRow")
                return;

            if (e.RowIndex == 0)
            {
                MessageBox.Show("لا يمكن حذف أول وحدة؛ يمكنك تعديلها بدلاً من ذلك.",
                    "وحدات القياس", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.RowIndex < unitRows.Count)
                unitRows.RemoveAt(e.RowIndex);
        }

        private void GridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= unitRows.Count)
                return;

            if (gridView1.Columns[e.ColumnIndex].Name == nameof(DAL.ProductUnit.Factor) &&
                e.RowIndex == 0)
            {
                e.Cancel = false;
            }
        }

        private void LoadProduct(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                product = db.Products.SingleOrDefault(x => x.ID == id);
                if (product == null)
                    throw new InvalidOperationException("الصنف المطلوب غير موجود.");

                unitRows = new BindingList<DAL.ProductUnit>(db.ProductUnits
                    .Where(x => x.ProductID == id)
                    .OrderBy(x => x.ID)
                    .ToList());
            }

            BindProductControls();
            gridView1.DataSource = unitRows;
            PartID = product.ID;
            PartName = product.Name;
            Text = "بيانات صنف: " + product.Name;
            btn_Delete.Enabled = true;
        }

        private void BindProductControls()
        {
            txt_Code.Text = product.Code;
            txt_Name.Text = product.Name;
            lkp_Type.SelectedValue = (int)product.Type;
            lkp_CostCalculationMethod.SelectedValue = (int)product.CostCalculationMethod;
            memoEdit1.Text = product.Descreption;
            checkEdit1.Checked = product.IsActive;
            lkp_Category.SelectedValue = product.CategoryID;

            if (product.Image != null)
            {
                using (var stream = new System.IO.MemoryStream(product.Image.ToArray()))
                    pictureEdit1.Image = new Bitmap(Image.FromStream(stream));
            }
            else
            {
                pictureEdit1.Image = null;
            }
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                categories = db.ProductCategories
                    .Where(x => !db.ProductCategories.Any(w => w.ParentID == x.ID))
                    .OrderBy(x => x.Name)
                    .ToList();

                unitNames = db.UnitNames.OrderBy(x => x.Name).ToList();
            }

            lkp_Category.DataSource = null;
            lkp_Category.DisplayMember = nameof(DAL.ProductCategory.Name);
            lkp_Category.ValueMember = nameof(DAL.ProductCategory.ID);
            lkp_Category.DataSource = categories;

            lkp_Type.DisplayMember = nameof(Master.ValueAndID.Name);
            lkp_Type.ValueMember = nameof(Master.ValueAndID.ID);
            lkp_Type.DataSource = Master.ProductTypesList;

            lkp_CostCalculationMethod.DisplayMember = nameof(Master.ValueAndID.Name);
            lkp_CostCalculationMethod.ValueMember = nameof(Master.ValueAndID.ID);
            lkp_CostCalculationMethod.DataSource = MasterInventory.CostCalculationMethodList;

            var unitColumn = gridView1.Columns[nameof(DAL.ProductUnit.UnitID)] as DataGridViewComboBoxColumn;
            if (unitColumn != null)
            {
                unitColumn.DataSource = null;
                unitColumn.DataSource = unitNames;
            }

            base.RefreshData();
        }

        private void EnsureCategoryExists()
        {
            if (lkp_Category.SelectedValue != null)
                return;

            string categoryName = lkp_Category.Text.Trim();
            if (categoryName.Length == 0)
                return;

            using (var db = new DAL.DBDataContext())
            {
                var category = db.ProductCategories.FirstOrDefault(x => x.Name == categoryName);
                if (category == null)
                {
                    category = new DAL.ProductCategory
                    {
                        Name = categoryName,
                        ParentID = 0,
                        Number = "0"
                    };
                    db.ProductCategories.InsertOnSubmit(category);
                    db.SubmitChanges();
                }

                if (!categories.Any(x => x.ID == category.ID))
                    categories.Add(category);

                lkp_Category.DataSource = null;
                lkp_Category.DisplayMember = nameof(DAL.ProductCategory.Name);
                lkp_Category.ValueMember = nameof(DAL.ProductCategory.ID);
                lkp_Category.DataSource = categories;
                lkp_Category.SelectedValue = category.ID;
            }
        }

        private void AddUnit()
        {
            string name = txt_NewUnit.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("اكتب اسم وحدة القياس أولاً.", "وحدات القياس",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_NewUnit.Focus();
                return;
            }

            using (var db = new DAL.DBDataContext())
            {
                if (db.UnitNames.Any(x => x.Name == name))
                {
                    MessageBox.Show("وحدة القياس موجودة بالفعل.", "وحدات القياس",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var unit = new DAL.UnitName { Name = name };
                db.UnitNames.InsertOnSubmit(unit);
                db.SubmitChanges();
            }

            txt_NewUnit.Clear();
            RefreshData();
            if (gridView1.CurrentCell != null)
                gridView1.BeginEdit(true);
        }

        private void LoadImage()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "اختيار صورة الصنف";
                dialog.Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                using (var source = Image.FromFile(dialog.FileName))
                    pictureEdit1.Image = new Bitmap(source);
            }
        }

        public override bool IsDataValide()
        {
            EnsureCategoryExists();

            if (string.IsNullOrWhiteSpace(txt_Code.Text) ||
                string.IsNullOrWhiteSpace(txt_Name.Text))
            {
                MessageBox.Show("يرجى إدخال كود الصنف واسمه.", "بيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (lkp_Category.SelectedValue == null ||
                lkp_Type.SelectedValue == null ||
                lkp_CostCalculationMethod.SelectedValue == null)
            {
                MessageBox.Show("يرجى تحديد الفئة ونوع الصنف وطريقة حساب التكلفة.",
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                gridView1.EndEdit();
                var manager = BindingContext[unitRows] as CurrencyManager;
                if (manager != null)
                    manager.EndCurrentEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر اعتماد بيانات وحدات القياس: " + ex.Message,
                    "بيانات غير صحيحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (unitRows.Count == 0)
            {
                MessageBox.Show("الرجاء إضافة وحدة قياس واحدة على الأقل.",
                    "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var usedUnitIds = new HashSet<int>();
            var usedBarcodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < unitRows.Count; i++)
            {
                var row = unitRows[i];
                if (i == 0)
                    row.Factor = 1;
                else if (row.Factor <= 1)
                {
                    MessageBox.Show("يجب أن يكون معامل تحويل الوحدات الإضافية أكبر من 1.",
                        "بيانات وحدات القياس", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gridView1.CurrentCell = gridView1.Rows[i].Cells[nameof(DAL.ProductUnit.Factor)];
                    return false;
                }

                if (row.UnitID <= 0 || !unitNames.Any(x => x.ID == row.UnitID))
                {
                    MessageBox.Show("يرجى اختيار وحدة قياس صحيحة لكل سطر.",
                        "بيانات وحدات القياس", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!usedUnitIds.Add(row.UnitID))
                {
                    MessageBox.Show("لا يمكن تكرار وحدة القياس داخل الصنف نفسه.",
                        "بيانات وحدات القياس", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string barcode = (row.Barcode ?? string.Empty).Trim();
                row.Barcode = barcode;
                if (barcode.Length > 0 && !usedBarcodes.Add(barcode))
                {
                    MessageBox.Show("الباركود مكرر داخل وحدات الصنف.",
                        "بيانات وحدات القياس", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            using (var db = new DAL.DBDataContext())
            {
                bool duplicateName = db.Products.Any(x =>
                    x.ID != product.ID && x.Name.Trim() == txt_Name.Text.Trim());
                bool duplicateCode = db.Products.Any(x =>
                    x.ID != product.ID && x.Code.Trim() == txt_Code.Text.Trim());

                if (duplicateName)
                {
                    MessageBox.Show("اسم الصنف مسجل بالفعل.", "بيانات مكررة",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Name.Focus();
                    return false;
                }

                if (duplicateCode)
                {
                    MessageBox.Show("كود الصنف مسجل بالفعل.", "بيانات مكررة",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Code.Focus();
                    return false;
                }

                foreach (var row in unitRows)
                {
                    if (!string.IsNullOrEmpty(row.Barcode) &&
                        db.ProductUnits.Any(x => x.Barcode == row.Barcode &&
                                                x.ProductID != product.ID))
                    {
                        MessageBox.Show("الباركود " + row.Barcode + " مسجل لصنف آخر.",
                            "باركود مكرر", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }

            return true;
        }

        public override void SetData()
        {
            product.Code = txt_Code.Text.Trim();
            product.Name = txt_Name.Text.Trim();
            product.Type = Convert.ToByte(lkp_Type.SelectedValue);
            product.CostCalculationMethod = Convert.ToByte(lkp_CostCalculationMethod.SelectedValue);
            product.Descreption = memoEdit1.Text;
            product.IsActive = checkEdit1.Checked;
            product.CategoryID = Convert.ToInt32(lkp_Category.SelectedValue);
            product.Image = Class.Utils.GetByteFromImage(pictureEdit1.Image);
            base.SetData();
        }

        public override void GetData()
        {
            if (product == null)
                return;

            BindProductControls();
            gridView1.DataSource = unitRows;
            PartID = product.ID;
            PartName = product.Name;
            base.GetData();
        }

        public override void Save()
        {
            try
            {
                gridView1.EndEdit();
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;

                        DAL.Product savedProduct;
                        if (product.ID == 0)
                        {
                            savedProduct = product;
                            db.Products.InsertOnSubmit(savedProduct);
                        }
                        else
                        {
                            savedProduct = db.Products.Single(x => x.ID == product.ID);
                        }

                        product = savedProduct;
                        SetData();
                        db.SubmitChanges();

                        var existingUnits = db.ProductUnits
                            .Where(x => x.ProductID == savedProduct.ID)
                            .ToList();

                        foreach (var oldUnit in existingUnits)
                        {
                            if (!unitRows.Any(x => x.ID == oldUnit.ID))
                                db.ProductUnits.DeleteOnSubmit(oldUnit);
                        }

                        foreach (var row in unitRows)
                        {
                            if (row.ID > 0)
                            {
                                var tracked = existingUnits.SingleOrDefault(x => x.ID == row.ID);
                                if (tracked == null)
                                    throw new InvalidOperationException("إحدى وحدات الصنف لم تعد موجودة في قاعدة البيانات.");

                                tracked.UnitID = row.UnitID;
                                tracked.Factor = row.Factor;
                                tracked.SellPrice = row.SellPrice;
                                tracked.BuyPrice = row.BuyPrice;
                                tracked.SellDiscount = row.SellDiscount;
                                tracked.Barcode = row.Barcode ?? string.Empty;
                            }
                            else
                            {
                                db.ProductUnits.InsertOnSubmit(new DAL.ProductUnit
                                {
                                    ProductID = savedProduct.ID,
                                    UnitID = row.UnitID,
                                    Factor = row.Factor,
                                    SellPrice = row.SellPrice,
                                    BuyPrice = row.BuyPrice,
                                    SellDiscount = row.SellDiscount,
                                    Barcode = row.Barcode ?? string.Empty
                                });
                            }
                        }

                        db.SubmitChanges();
                        transaction.Commit();
                        product = savedProduct;
                    }
                }

                PartID = product.ID;
                PartName = product.Name;
                base.Save();
                Text = "بيانات صنف: " + product.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حفظ الصنف ووحدات قياسه: " + ex.Message,
                    "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Delete()
        {
            if (!AskForDeletion())
                return;

            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;

                    if (db.StoreLogs.Any(x => x.ProductID == product.ID))
                    {
                        transaction.Rollback();
                        MessageBox.Show("لا يمكن حذف الصنف لأنه مستخدم في حركات المخزون. يمكنك إلغاء تنشيطه بدلاً من حذفه.",
                            "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var units = db.ProductUnits.Where(x => x.ProductID == product.ID).ToList();
                    db.ProductUnits.DeleteAllOnSubmit(units);
                    db.SubmitChanges();

                    var trackedProduct = db.Products.Single(x => x.ID == product.ID);
                    db.Products.DeleteOnSubmit(trackedProduct);
                    db.SubmitChanges();

                        transaction.Commit();
                    }
                }

                base.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حذف الصنف: " + ex.Message,
                    "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void New()
        {
            using (var db = new DAL.DBDataContext())
            {
                if (!db.UnitNames.Any())
                {
                    db.UnitNames.InsertOnSubmit(new DAL.UnitName { Name = "قطعة" });
                    db.SubmitChanges();
                }

                product = new DAL.Product
                {
                    Code = GetNewProductCode(),
                    IsActive = true,
                    Type = (byte)Master.ProductType.Inventory,
                    CostCalculationMethod = (byte)MasterInventory.CostCalculationMethod.FIFO
                };

                var category = db.ProductCategories
                    .Where(x => !db.ProductCategories.Any(w => w.ParentID == x.ID))
                    .OrderBy(x => x.ID)
                    .FirstOrDefault();

                if (category != null)
                    product.CategoryID = category.ID;
            }

            RefreshData();
            unitRows = new BindingList<DAL.ProductUnit>();
            var firstUnit = unitNames.FirstOrDefault();
            if (firstUnit != null)
            {
                unitRows.Add(new DAL.ProductUnit
                {
                    Factor = 1,
                    UnitID = firstUnit.ID,
                    Barcode = GetNewBarcode()
                });
            }

            base.New();
            gridView1.DataSource = unitRows;
            Text = "إضافة صنف جديد";
            PartID = 0;
            PartName = string.Empty;
        }

        private string GetNewProductCode()
        {
            using (var db = new DAL.DBDataContext())
            {
                string lastCode = db.Products.OrderByDescending(x => x.ID)
                    .Select(x => x.Code ?? "0").FirstOrDefault();
                return Master.GetNextNumberInString(lastCode);
            }
        }

        private string GetNewBarcode()
        {
            using (var db = new DAL.DBDataContext())
            {
                string lastBarcode = db.ProductUnits.Where(x => x.Barcode != null)
                    .Select(x => x.Barcode).OrderByDescending(x => x).FirstOrDefault();
                return Master.GetNextNumberInString(lastBarcode);
            }
        }

        private void BtnAddUnit_Click(object sender, EventArgs e)
        {
            AddUnit();
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void BtnClearImage_Click(object sender, EventArgs e)
        {
            pictureEdit1.Image = null;
        }
    }
}