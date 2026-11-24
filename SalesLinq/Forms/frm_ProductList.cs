using System;
using System.Collections.Generic;
using System.Linq;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_ProductList : NativeMasterListForm
    {
        private sealed class ProductListRow
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public string Type { get; set; }
            public string Units { get; set; }
            public string Descreption { get; set; }
            public bool IsActive { get; set; }
        }

        public frm_ProductList()
        {
            InitializeComponent();
        }

        private void Frm_ProductList_Load(object sender, EventArgs e)
        {
            if (gridView1.Columns.Contains("ID"))
                gridView1.Columns["ID"].Visible = false;

            SetHeader("Code", "الكود");
            SetHeader("Name", "الاسم");
            SetHeader("CategoryName", "الفئة");
            SetHeader("Type", "النوع");
            SetHeader("Units", "وحدات القياس");
            SetHeader("Descreption", "الوصف");
            SetHeader("IsActive", "نشط");
            gridView1.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            gridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void SetHeader(string columnName, string caption)
        {
            if (gridView1.Columns.Contains(columnName))
                gridView1.Columns[columnName].HeaderText = caption;
        }

        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                var products = db.Products.OrderBy(x => x.Name).ToList();
                var categoryNames = db.ProductCategories.ToDictionary(x => x.ID, x => x.Name);
                var unitNames = db.UnitNames.ToDictionary(x => x.ID, x => x.Name);
                var unitsByProduct = db.ProductUnits.ToList().GroupBy(x => x.ProductID)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var rows = products.Select(product =>
                {
                    List<DAL.ProductUnit> units;
                    if (!unitsByProduct.TryGetValue(product.ID, out units))
                        units = new List<DAL.ProductUnit>();

                    string categoryName;
                    categoryNames.TryGetValue(product.CategoryID, out categoryName);

                    string typeName = Master.ProductTypesList
                        .Where(x => x.ID == product.Type)
                        .Select(x => x.Name)
                        .FirstOrDefault();

                    string unitSummary = string.Join(Environment.NewLine, units.Select(unit =>
                    {
                        string unitName;
                        if (!unitNames.TryGetValue(unit.UnitID, out unitName))
                            unitName = "وحدة غير معروفة";

                        return string.Format(
                            "{0} | معامل: {1} | بيع: {2:N2} | شراء: {3:N2} | خصم: {4:N2} | باركود: {5}",
                            unitName, unit.Factor, unit.SellPrice, unit.BuyPrice,
                            unit.SellDiscount, unit.Barcode);
                    }));

                    return new ProductListRow
                    {
                        ID = product.ID,
                        Code = product.Code,
                        Name = product.Name,
                        CategoryName = categoryName,
                        Type = typeName,
                        Units = unitSummary,
                        Descreption = product.Descreption,
                        IsActive = product.IsActive
                    };
                }).ToList();

                gridView1.DataSource = rows;
            }

            base.RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_Product(id), true);
        }

        public override void New()
        {
            frm_Main.OpenForm(new frm_Product(), true);
            RefreshData();
        }
    }
}