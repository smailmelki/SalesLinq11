using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using static SalesLinq.Class.Master;

namespace SalesLinq.Forms
{
    public partial class frm_Product : frm_Master
    {

        DAL.ProductUnit ins = new DAL.ProductUnit();//nameof انشاء نسخة من أجل استخراج الاسماء منها فقط باستعمال 
        DAL.Product product;
        RepositoryItemLookUpEdit repoUOM = new RepositoryItemLookUpEdit();//gridControl1 لاضافتها في LookUpEdit من نوع RepositoryItem انشاء نسخة من
        DAL.DBDataContext udb = new DAL.DBDataContext();

        public frm_Product()
        {
            InitializeComponent();
            RefreshData();
            New();
        }

        /// <summary>
        /// id من اجل تعديل الصنف الذي رقمه
        /// </summary>
        /// <param name="id"></param>
        public frm_Product(int id)
        {
            InitializeComponent();
            RefreshData();
            LoadProduct(id);
        }
        /// <summary>
        /// دالة تتحقق من أن كود وحدة الصنف موجود
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="prdID"></param>
        /// <returns>ترجع صح اذا كان موجود وخطأ اذا كان جديد</returns>
        Boolean CheckIfBarcodeExist(string barcode, int prdID)
        {
            if (string.IsNullOrEmpty(barcode))
                return false;
            using (var db = new DAL.DBDataContext())
                return db.ProductUnits.Where(x => x.Barcode == barcode && x.ProductID != prdID).Count() > 0;
        }

        private void Frm_Product_Load(object sender, EventArgs e)
        {
            /////////////////////////  عند عرض التصنيف /////////////
            lkp_Category.Properties.DisplayMember = "Name"; // لجع الاسم هو الظاهر 
            lkp_Category.Properties.ValueMember = "ID"; // القيمة الفعلية للتصنيف
            lkp_Category.Properties.PopulateColumns();  // 
            lkp_Category.Properties.Columns["Name"].Caption = "اسم التصنيف"; // 
            lkp_Category.Properties.Columns["ID"].Visible = false; //ID اخفاء عمود  
            lkp_Category.Properties.Columns["ParentID"].Visible = false; // اخفاء عمود الأب
            lkp_Category.Properties.Columns["Number"].Visible = false; // اخفاء عمود الرقم 
            /////////////////////////
            lkp_Category.ProcessNewValue += Lkp_Category_ProcessNewValue;//lkp_Category حدث مغادرة 
            lkp_Category.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;//للسماح بالكتابة على العنصر

            lkp_Type.Properties.DataSource = ProductTypesList;
            lkp_Type.Properties.DisplayMember = "Name";
            lkp_Type.Properties.ValueMember = "ID";

            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;//للسماح باضافة سطر جديد

            gridView1.Columns[nameof(ins.ID)].Visible = false; //ID اخفاء 
            gridView1.Columns[nameof(ins.ProductID)].Visible = false;//ProductID اخفاء

            RepositoryItemCalcEdit calcEdit = new RepositoryItemCalcEdit();//gridControl1 لاضافتها في calcEdit من نوع RepositoryItem انشاء نسخة من

            gridControl1.RepositoryItems.Add(calcEdit); //gridView1 لاستعمالها في gridControl1 الى  calcEdit  اضافة
            gridControl1.RepositoryItems.Add(repoUOM); //gridView1 لاستعمالها في gridControl1 الى  repoUOM  اضافة

            gridView1.Columns[nameof(ins.SellPrice)].ColumnEdit = calcEdit; //gridView1 في calcEdit على شكل SellPrice اظهار  
            gridView1.Columns[nameof(ins.BuyPrice)].ColumnEdit = calcEdit; //gridView1 في calcEdit على شكل BuyPrice اظهار
            gridView1.Columns[nameof(ins.SellDiscount)].ColumnEdit = calcEdit; //gridView1 في calcEdit على شكل SellDiscount اظهار
            gridView1.Columns[nameof(ins.Factor)].ColumnEdit = calcEdit; //gridView1 في calcEdit على شكل Factor اظهار
            gridView1.Columns[nameof(ins.UnitID)].ColumnEdit = repoUOM; //gridView1 في repoUOM على شكل UnitID اظهار

            gridView1.Columns[nameof(ins.Barcode)].Caption = "الباركود";
            gridView1.Columns[nameof(ins.BuyPrice)].Caption = "سعر الشراء";
            gridView1.Columns[nameof(ins.Factor)].Caption = "معامل التحويل";
            gridView1.Columns[nameof(ins.SellDiscount)].Caption = "خصم البيع";
            gridView1.Columns[nameof(ins.SellPrice)].Caption = "سعر البيع";
            gridView1.Columns[nameof(ins.UnitID)].Caption = "اسم الوحدة";

            repoUOM.ValueMember = "ID";
            repoUOM.DisplayMember = "Name";
            repoUOM.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;// للسماح بالكتابة في خانة الوحدات
            repoUOM.ProcessNewValue += RepoUOM_ProcessNewValue;

            gridView1.ValidateRow += GridView1_ValidateRow;//gridView1 انشاء حدث التحقق في
            gridView1.RowDeleting += GridView1_RowDeleting;//gridView1 حدث حذف سطر(وحدة) في 
            gridView1.InvalidRowException += GridView1_InvalidRowException; // حدث لمعالجة الغاء الرسالة المزعجة والاكتفاء بنص رسالة الخطأ
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;// انشاء حدث لمنع النعديل في أول سطر فقط
            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;// انشاء حدث يمنع تكرار الوحدة في نفس المنتج

            //اضافة ريبوزيتوري ايتم من أجل أن يحتوي على زر الحذف 
            RepositoryItemButtonEdit buttonEdit = new RepositoryItemButtonEdit();
            gridControl1.RepositoryItems.Add(buttonEdit);
            buttonEdit.Buttons.Clear();
            buttonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
            buttonEdit.ButtonClick += ButtonEdit_ButtonClick;
            // انشاء زر الحذف 
            GridColumn clmnDelete = new GridColumn()
            {
                Name = "clmnDelete",
                Caption = "حذف",
                FieldName = "Delete",
                ColumnEdit = buttonEdit,
                VisibleIndex = 10,
                Width = 30,
                MaxWidth = 30
            };
            //لاظهار زر الحذف بدون نص
            buttonEdit.TextEditStyle = TextEditStyles.HideTextEditor;
            // اضافة زر الحذف في الجدول في اخر السطر
            gridView1.Columns.Add(clmnDelete);
        }
        /// <summary>
        /// منع حذف أول سطر
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_RowDeleting(object sender, DevExpress.Data.RowDeletingEventArgs e)
        {
            if (e.ListSourceIndex==0)
            {
                e.Cancel = true;
                XtraMessageBox.Show(
                      text: "لا يمكن حذف أول سطر \n يمكنك التعديل عليه",
                      caption: string.Empty,
                      buttons: MessageBoxButtons.OK,
                      icon: MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// دالة حذف وحدة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
            if (view.FocusedRowHandle >= 0)
            {
                view.DeleteSelectedRows();
            }
        }

        private string GetNewBarcode()
        {
            string maxCode;
            using (var db = new DAL.DBDataContext())
            {
                maxCode = db.ProductUnits.Select(x => x.Barcode).Max();
            }

            return GetNextNumberInString(maxCode);
        }

        /// <summary>
        /// دالة ترجع اكبر كود في قاعدة البيانات من جدول المنتجات وتضيف عليه 1
        /// </summary>
        /// <returns></returns>
        private string GetNewProductCode()
        {
            string maxCode;
            using (var db = new DAL.DBDataContext())
            {
                maxCode = db.Products.OrderByDescending(x => x.ID).Select(x => x.Code ?? "0").FirstOrDefault();
                //maxCode = db.Products.Select(x => x.Code).Max();
            }

            return GetNextNumberInString(maxCode);
        }

        private void GridControl1_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// منع تكرار استخدام وحدات القياس المستخدمة في المنتج
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == nameof(ins.UnitID))
            {
                var ids = ((Collection<DAL.ProductUnit>)gridView1.DataSource).Select(x => x.UnitID).ToList();
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();// خاص بالخلية المحددة
                using (var db = new DAL.DBDataContext())
                {
                    var currentID = (Int32?)e.CellValue;// متغير ممكن يقبل القيمة الفارغة
                    ids.Remove(currentID ?? 0);// اذا كانت القيمة فارغة يحولها الى 0
                    repo.DataSource = db.UnitNames.Where(x => ids.Contains(x.ID) == false).ToList();// اظهار الوحدات غير المختارة فقط
                    repo.ValueMember = "ID";
                    repo.DisplayMember = "Name";
                    repo.PopulateColumns();
                    repo.Columns[nameof(ins.ID)].Visible = false;
                    repo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;// للسماح بالكتابة في خانة الوحدات
                    repo.ProcessNewValue += RepoUOM_ProcessNewValue;
                    e.RepositoryItem = repo;// اعتمد هذا الريبوزيتوري ايتم بدل الاول
                }
            }
        }

        private void GridView1_FocusedRowChanged(object sender,DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridView1.Columns[nameof(ins.Factor)].OptionsColumn.AllowEdit = !(e.FocusedRowHandle == 0);//منع النعديل في أول سطر فقط
        }

        private void GridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        /// <summary>
        /// دالة التحقق من القيم المدخلة للوحدات
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {            
            var row = e.Row as DAL.ProductUnit;
            var view = sender as GridView;
            if (row == null)
                return;
            //في أول خانة يجب أن تكون 1 (Factor) قيمة المعامل  
            if (view.RowCount == 0)
            {
                row.Factor = 1;
            }
            else if (row.Factor <= 1 && e.RowHandle != 0)
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.Factor)], "يجب أن تكون القيمة أكبر من 1");
            }             
            // يجب أن تكون أكبر من 0 (UnitID) قيمة الوحدة
            if (row.UnitID <= 0)
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.UnitID)], ErrorText);
            }
            // التحقق من أن الباركود غير موجود في قاعدة البيانات
            if (CheckIfBarcodeExist(row.Barcode, product.ID))
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.Barcode)], "هذا الكود موجود بالفعل");
            }
            //gridView1 التحقق من أن الباركود غير مكرر في 
            if (row.Barcode != null && IsBarcodExistInGrid(row.Barcode, e.RowHandle))
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.Barcode)], "هذا الكود مكرر");
            }

        }
        /// <summary>
        ///gridView1 منع تكرار الباركود في 
        /// </summary>
        Boolean IsBarcodExistInGrid(string Barcode,int indexRow)
        {
            List<string> vals = new List<string>();
            vals.Clear();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (vals.Contains(Barcode))
                {
                    return true;
                }
                if (indexRow != i && gridView1.GetRowCellValue(i, "Barcode") != null)
                    vals.Add(gridView1.GetRowCellValue(i, "Barcode").ToString());
            }
            return false;
           
        }


        /// <summary>
        /// التحقق من البيانات غير فارغة
        /// </summary>
        /// <returns>يرجع خطأ في حالة وجود أي حقل فارغ</returns>
        public override bool IsDataValide()
        {
            bool IsValid = true;
            if (lkp_Category.EditValue is int == false || Convert.ToInt32(lkp_Category.EditValue) <= 0) //  اذا لم يحدد فئة الصنف يرجع خطأ ويظهر رسالة الخطأ 
            {
                lkp_Category.ErrorText = ErrorText;
                IsValid = false;
            }
            if (lkp_Type.EditValue is byte == false) // اذا لم يحدد نوع الصنف يرجع خطأ ويظهر رسالة الخطأ
            {
                lkp_Type.ErrorText = ErrorText;
                IsValid = false;
            }
            if (lkp_CostCalculationMethod.EditValue.ToString() == "0") // اذا لم يحدد طريقة حساب التكلفة يرجع خطأ ويظهر رسالة الخطأ
            {
                lkp_CostCalculationMethod.ErrorText = ErrorText;
                IsValid = false;
            }
            if (txt_Name.Text.Trim() == string.Empty) // اذا كان فارغا يرجع خطأ ويظهر رسالة الخطأ
            {
                txt_Name.ErrorText = ErrorText;
                IsValid = false;
            }
            if (txt_Code.Text.Trim() == string.Empty) // اذا كان فارغا يرجع خطأ ويظهر رسالة الخطأ
            {
                txt_Code.ErrorText = ErrorText;
                IsValid = false;
            }

            var db = new DAL.DBDataContext();
            if (db.Products.Where(x => x.ID != product.ID && x.Name.Trim() == txt_Name.Text.Trim()).Count() > 0) // اذا كان الاسم مسجل مسبقا يرجع خطأ ويظهر رسالة الخطأ
            {
                txt_Name.ErrorText = "هذا الاسم مسجل بالفعل";
                IsValid = false;
            }
            if (db.Products.Where(x => x.ID != product.ID && x.Code.Trim() == txt_Code.Text.Trim()).Count() > 0) // اذا كان كود الصنف مسجل مسبقا يرجع خطأ ويظهر رسالة الخطأ 
            {
                txt_Code.ErrorText = "هذا الكود مسجل بالفعل";
                IsValid = false;
            }
            if (gridView1.RowCount < 1)
            {
                XtraMessageBox.Show(
                       text: "الرجاء اضافة وحدة أو أكثر الى الصنف",
                       caption: string.Empty,
                       buttons: MessageBoxButtons.OK,
                       icon: MessageBoxIcon.Error);
                IsValid = false;
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.FocusedRowHandle = i;
                GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));
                if (gridView1.HasColumnErrors) // اللتحقق من أن الفاتورة لا تحتوي على اخطاء
                {
                    XtraMessageBox.Show(
                       text: "الرجاء التحقق من الأخطاء في الجدول",
                       caption: string.Empty,
                       buttons: MessageBoxButtons.OK,
                       icon: MessageBoxIcon.Error);
                    GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));
                    IsValid = false;
                }
            }
            return IsValid;
        }
        
        /// <summary>
        /// اضافة تصنيف جديد اذا لم يكن موجود
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lkp_Category_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string st && st.Trim() != string.Empty)
            {
                var newobject = new DAL.ProductCategory() { Name = st, ParentID = 0, Number = "0" };//انشاء نسخة من الجدول واسناد القيم مباشرة
                using (var db = new DAL.DBDataContext())
                {
                    db.ProductCategories.InsertOnSubmit(newobject);
                    db.SubmitChanges();
                }
               ((List<DAL.ProductCategory>)lkp_Category.Properties.DataSource).Add(newobject);//اضافة العنصر الجديد الى القائمة بدون اعادة تحميل
                e.Handled = true;// اعتماد العنصر
            }
        }

        void LoadProduct(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                product = db.Products.Single(x => x.ID == id);
            }
            this.Text = string.Format("بيانات صنف: {0}", product.Name);// تغيير عنوان الفورم
            GetData();
        }

        /// <summary>
        /// اضافة وحدة جديدة اذا لم تكن موجودة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepoUOM_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string value && value.Trim() != string.Empty)
            {
                // اضافة نسخة من جدول الوحدات واسناد قيمة لها
                var NewObject = new DAL.UnitName() { Name = value.Trim() };
                using (DAL.DBDataContext db = new DAL.DBDataContext())
                {
                    //حفظ النسخة في قاعدة البيانات
                    db.UnitNames.InsertOnSubmit(NewObject);
                    db.SubmitChanges();
                }
                // ادراج النسخة الى وحدات القياس مباشرة بدون اغلاق وفتح الفورم
                ((List<DAL.UnitName>)repoUOM.DataSource).Add(NewObject);
                ((List<DAL.UnitName>)(((LookUpEdit)sender).Properties.DataSource)).Add(NewObject);
                e.Handled = true;
            }
        }
       

        /// <summary>
        /// حذف عنصر من قاعدة البيانات
        /// </summary>
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    //التأكد من أن العنصر غير مستخدم قبل حذفه
                    var accountLog = db.StoreLogs.Where(x => x.ProductID == product.ID).Count();

                    if (accountLog > 0)
                    {
                        XtraMessageBox.Show(
                            text: "عفوا لا يمكن حذف هذا العنصر حيث أنه تم استخدامه في عدة فواتير \n يمكنك الغاء تنشيطه بدلا من حذفه",
                            caption: string.Empty,
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
                        return;
                    }
                    db.Products.Attach(product);
                    db.Products.DeleteOnSubmit(product);
                    db.ProductUnits.DeleteAllOnSubmit(db.ProductUnits.Where(x => x.ProductID == product.ID));
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }


        public override void New()
        {
            product = new DAL.Product()
            {
                Code = GetNewProductCode(),
                IsActive = true,
                Type = (byte)Class.Master.ProductType.Inventory
            };
            var db = new DAL.DBDataContext();
            // لاضافة التصنيف الأول في قاعدة البيانات افتراضيا عند اضافة منتج جديد اذا كانت قاعدة البيانات تحتوي تصنيفات  
            var categ = db.ProductCategories
                .Where(x => db.ProductCategories.Where(w => w.ParentID == x.ID).Count() == 0)
                .FirstOrDefault();
            if (categ != null)
                product.CategoryID = categ.ID;
            base.New();
            this.Text = "اضافة صنف جديد";// تغيير عنوان الفورم
            // نعمل وحدف افتراضية معاملها 1 getData بعد عمل
            var data = gridView1.DataSource as BindingList<DAL.ProductUnit>;//gridView1 للتغيير عليها وارجاعها الى gridView1 انشاء نسخة من البيانات الموجودة في

            //"اذا كان جدول الوحدات فارغا نضيف وحدة افتراضية... "قطعة 
            if (db.UnitNames.Count() == 0)
            {
                db.UnitNames.InsertOnSubmit(new DAL.UnitName() { Name = "قطعة" });
                db.SubmitChanges();
                RefreshData();
            }
            data.Add(new DAL.ProductUnit() { Factor = 1, UnitID = db.UnitNames.First().ID, Barcode = GetNewBarcode() });//قيم افتراضية و ابتدائية
        }


        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                lkp_Category.Properties.DataSource = db.ProductCategories
                    .Where(x => db.ProductCategories.Where(w => w.ParentID == x.ID).Count() == 0)
                    .ToList();//تحديد الفئات التي لا تحتوي على أبناء فقط
                repoUOM.DataSource = db.UnitNames.ToList(); // gridView1 بالبيانات لاظهارها في repoUOM تعبئة
            }
            lkp_CostCalculationMethod.InitializeData(Class.MasterInventory.CostCalculationMethodList);
            base.RefreshData();
        }

        /// <summary>
        /// حفظ البيانات أو تعديلها
        /// </summary>
        public override void Save()
        {
            gridView1.UpdateCurrentRow();
            //if (!IsDataValide())
            //   return;
            var db = new DAL.DBDataContext();
            if (product.ID == 0)
                db.Products.InsertOnSubmit(product);
            else
                db.Products.Attach(product);
            SetData();

            db.SubmitChanges();

            var data = gridView1.DataSource as BindingList<DAL.ProductUnit>;//gridView1 للتغيير عليها وارجاعها الى gridView1 انشاء نسخة من البيانات الموجودة في
            foreach (var item in data)
            {
                item.ProductID = product.ID;//بعد عملية الحفظ ليأخذ الرقم الجديد ID تغيير
                if (string.IsNullOrEmpty(item.Barcode))
                    item.Barcode = string.Empty;//اذا كان الباركود فارغ يعطيه نص فارغ
                else
                {

                }
            }
            udb.SubmitChanges();
            PartID = product.ID;     // لتسجيل حركات المستخدم
            PartName = product.Name; // لتسجيل حركات المستخدم
            base.Save();
            this.Text = string.Format("بيانات صنف: {0}", product.Name);// تغيير عنوان الفورم
        }

        public override void SetData()
        {
            product.Code = txt_Code.Text;
            product.Name = txt_Name.Text;
            product.Type = Convert.ToByte(lkp_Type.EditValue);
            product.CostCalculationMethod = Convert.ToByte(lkp_CostCalculationMethod.EditValue);
            product.Descreption = memoEdit1.Text;
            product.IsActive = checkEdit1.Checked;
            product.CategoryID = Convert.ToInt32(lkp_Category.EditValue);
            product.Image = Class.Utils.GetByteFromImage(pictureEdit1.Image);

            base.SetData();
        }

        /// <summary>
        /// جلب بيانات الصنف
        /// </summary>
        public override void GetData()
        {
            txt_Code.Text = product.Code;
            txt_Name.Text = product.Name;
            lkp_Type.EditValue = product.Type;
            lkp_CostCalculationMethod.EditValue = product.CostCalculationMethod;
            memoEdit1.Text = product.Descreption;
            checkEdit1.Checked = product.IsActive;
            lkp_Category.EditValue = product.CategoryID;
            if (product.Image != null)
                pictureEdit1.Image = Class.Utils.GetImageFromByteArray(product.Image.ToArray());
            else
                pictureEdit1.Image = null;
            gridControl1.DataSource = udb.ProductUnits.Where(x => x.ProductID == product.ID);//جلب الوحدات الخاصة بالعنصر
            PartID = product.ID;     // لتسجيل حركات المستخدم
            PartName = product.Name; // لتسجيل حركات المستخدم
            base.GetData();
        }
    }
}