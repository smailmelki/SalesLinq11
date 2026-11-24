using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    /// <summary>
    /// قائمة العملاء أو الموردين بحسب النوع المحدد عند الإنشاء.
    /// </summary>
    public partial class frm_CustomerVendorList : NativeMasterListForm
    {
        private readonly bool IsCustomer;
        private bool isSubscribed;

        public frm_CustomerVendorList(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            Text = IsCustomer ? "قائمة العملاء" : "قائمة الموردين";
            RefreshData();
        }

        private void Frm_CustomerVendorList_Load(object sender, EventArgs e)
        {
            if (gridView1.Columns.Contains(nameof(DAL.CustomersAndVendore.ID)))
                gridView1.Columns[nameof(DAL.CustomersAndVendore.ID)].Visible = false;

            if (gridView1.Columns.Contains(nameof(DAL.CustomersAndVendore.AccountID)))
                gridView1.Columns[nameof(DAL.CustomersAndVendore.AccountID)].Visible = false;

            if (gridView1.Columns.Contains(nameof(DAL.CustomersAndVendore.IsCustomer)))
                gridView1.Columns[nameof(DAL.CustomersAndVendore.IsCustomer)].Visible = false;

            SetHeader(nameof(DAL.CustomersAndVendore.Name), "الاسم");
            SetHeader(nameof(DAL.CustomersAndVendore.Phone), "الهاتف");
            SetHeader(nameof(DAL.CustomersAndVendore.Mobile), "الهاتف المحمول");
            SetHeader(nameof(DAL.CustomersAndVendore.Address), "العنوان");
            SetHeader(nameof(DAL.CustomersAndVendore.MaxCredit), "حد الائتمان");

            if (IsCustomer)
                Session.Customers.ListChanged += Parties_ListChanged;
            else
                Session.vendores.ListChanged += Parties_ListChanged;

            isSubscribed = true;
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
                var data = db.CustomersAndVendores
                    .Where(x => x.IsCustomer == IsCustomer)
                    .OrderBy(x => x.Name)
                    .ToList();

                gridView1.DataSource = data;
            }
            base.RefreshData();
        }

        private void Parties_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action(RefreshData)); }
                catch (InvalidOperationException) { }
                return;
            }

            RefreshData();
        }

        public override void OpenForm(int id)
        {
            frm_Main.OpenForm(new frm_CustomerVendor(id), true);
        }

        public override void New()
        {
            frm_Main.OpenForm(new frm_CustomerVendor(IsCustomer));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (isSubscribed)
            {
                if (IsCustomer)
                    Session.Customers.ListChanged -= Parties_ListChanged;
                else
                    Session.vendores.ListChanged -= Parties_ListChanged;

                isSubscribed = false;
            }

            base.OnFormClosed(e);
        }
    }
}