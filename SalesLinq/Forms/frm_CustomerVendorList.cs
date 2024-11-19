using System;
using System.ComponentModel;
using System.Linq;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_CustomerVendorList : frm_MasterList
    {
        bool IsCustomer;
        public frm_CustomerVendorList(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            RefreshData();
            this.Text = IsCustomer ? "قائمة العملاء" : "قائمة الموردين";
        }

        private void Frm_CustomerVendorList_Load(object sender, EventArgs e)
        {
            var ins = new DAL.CustomersAndVendore();
            gridView1.Columns[nameof(ins.ID)].Visible = false;
            gridView1.Columns[nameof(ins.AccountID)].Visible = false;
            gridView1.Columns[nameof(ins.IsCustomer)].Visible = false;
            gridView1.Columns[nameof(ins.Name)].Caption = "الاسم";
            gridView1.Columns[nameof(ins.Phone)].Caption = "الهاتف";
            gridView1.Columns[nameof(ins.Mobile)].Caption = "الموبايل";
            gridView1.Columns[nameof(ins.Address)].Caption = "العنوان";
            // من أجل تشغسل الحدث الخاص بتحديث البيانات في الفورم مباشرة عند التغير في قاعدة البيانات
            if (IsCustomer)
                Session.Customers.ListChanged += Vendores_ListChanged;
            else
                Session.vendores.ListChanged += Vendores_ListChanged;
        }

        public override void RefreshData()
        {
            gridControl1.DataSource =(IsCustomer)? Session.Customers.ToList() : Session.vendores.ToList();
            base.RefreshData();
        }

        private void Vendores_ListChanged(object sender, ListChangedEventArgs e)
        {
            RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_CustomerVendor(id), true);
        }

        public override void New()
        {
            var frm = new frm_CustomerVendor(IsCustomer);
            frm_Main.OpenForm(frm);
            //base.New();
        }
    }
}