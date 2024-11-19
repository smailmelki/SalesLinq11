using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_ProductCategory : frm_Master
    {
        DAL.ProductCategory category;

        public frm_ProductCategory()
        {
            InitializeComponent();
            New();
        }

        private void Frm_ProductCategory_Load(object sender, EventArgs e)
        {
            RefreshData();
            lookUpEdit1.Properties.DisplayMember = nameof(category.Name);
            lookUpEdit1.Properties.ValueMember = nameof(category.ID);
            treeList1.ParentFieldName = nameof(category.ParentID);
            treeList1.KeyFieldName = nameof(category.ID);
            treeList1.OptionsBehavior.Editable = false;
            treeList1.Columns[nameof(category.Number)].Visible = false;
            treeList1.Columns[nameof(category.Name)].Caption = "الاسم";
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;

        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(e.Node.GetValue("ID").ToString(), out id))//id اذا تم التحويل ترجع صحيح وتسند رقم النود في المتغير 
            {
                var db = new DAL.DBDataContext();
                category = db.ProductCategories.Single(x => x.ID == id);
                btn_Delete.Enabled = true;
                GetData();
            }
        }


        public override void New()
        {
            category = new DAL.ProductCategory();
            base.New();
        }
        public override void GetData()
        {
            textEdit1.Text = category.Name;//عرض الابن
            lookUpEdit1.EditValue = category.ParentID;//عرض الأب
            PartID = category.ID;     // لتسجيل حركات المستخدم
            PartName = category.Name; // لتسجيل حركات المستخدم
            base.GetData();
        }

        public override void SetData()
        {
            category.Name = textEdit1.Text;
            category.ParentID = (lookUpEdit1.EditValue as int?) ?? 0;// ادا كانت القيمة فارغة تعوض بالصفر
            category.Number = "0";// مأقتا
            base.SetData();
        }

        bool IsDataValid()
        {
            // التحقق من أن الاسم مكتوب
            if (textEdit1.Text.Trim() == string.Empty)
            {
                textEdit1.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            // التحقق من أن الفئة ليست تابعة لنفسها
            if (textEdit1.Text.Trim() == lookUpEdit1.Text.Trim())
            {
                textEdit1.ErrorText = "لا يمكن للفئة أن تكون تابعة لنفسها";
                return false;
            }
            // التحقق من أن الاسم غير مسجل في قاعدة البيانات مسبقا
            var db = new DAL.DBDataContext();
            var oldObj = db.ProductCategories.Where(x => x.Name.Trim() == textEdit1.Text.Trim() && x.ID != category.ID);
            if (oldObj.Count() > 0)
            {
                textEdit1.ErrorText = "هذا الاسم مسجل مسبقا";
                return false;
            }
            return true;
        }

        public override void Save()
        {
            var db = new DAL.DBDataContext();
            if (category.ID == 0)
            {
                db.ProductCategories.InsertOnSubmit(category);
            }
            else
            {
                db.ProductCategories.Attach(category);
            }
            SetData();
            db.SubmitChanges();
            PartID = category.ID;     // لتسجيل حركات المستخدم
            PartName = category.Name; // لتسجيل حركات المستخدم
            base.Save();
        }
        /// <summary>
        /// دالة حذف تصنيف
        /// </summary>
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    if (db.ProductCategories.Where(x => x.ParentID == category.ID).Count() > 0)
                    {
                        XtraMessageBox.Show(text: "عفوا لا يمكن حذف هذا التصنيف لأنه يحتوي على تفرعات"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (db.Products.Where(x => x.CategoryID == category.ID).Count() > 0)
                        {
                            XtraMessageBox.Show(text: "عفوا لا يمكن حذف هذا التصنيف لأن هناك أصناف تحت هذا التصنيف"
                            , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        }
                        else
                        {
                            db.ProductCategories.Attach(category);
                            db.ProductCategories.DeleteOnSubmit(category);
                            db.SubmitChanges();
                            base.Delete();
                        }
                    }
                }
            }
            RefreshData();
        }

        public override void RefreshData()
        {
            var db = new DAL.DBDataContext();
            var groups = db.ProductCategories;
            lookUpEdit1.Properties.DataSource = groups;

            treeList1.DataSource = groups;
            treeList1.ExpandAll();
            base.Refresh();
        }
    }
}