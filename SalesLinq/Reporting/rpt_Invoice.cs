using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SalesLinq.Class;

namespace SalesLinq.Reporting
{
    public partial class rpt_Invoice : DevExpress.XtraReports.UI.XtraReport
    {
        Master.InvoiceType Type;
        public rpt_Invoice()
        {
            InitializeComponent();
            if (Session.CompanyInfo != null)
            {
                lbl_CompanyName.Text = Session.CompanyInfo.CompanyName;
                lbl_CompanyAddress.Text = Session.CompanyInfo.Address;
                lbl_CompanyPhone.Text = Session.CompanyInfo.Phone;
            }
            
        }

        /// <summary>
        /// دالة لربط التقرير بقاعدة البيانات
        /// </summary>
        void BindData()
        {
            //  ربط العناوين في التقرير بقاعدة البيانات
            lbl_Code.DataBindings.Add("Text", this.DataSource, "Code");
            xrBarCode1.DataBindings.Add("Text", this.DataSource, "ID");
            lbl_Branche.DataBindings.Add("Text", this.DataSource, "Store");
            lbl_Date.DataBindings.Add("Text", this.DataSource, "Date");
            lbl_Discount.DataBindings.Add("Text", this.DataSource, "DiscountValue");
            lbl_Expences.DataBindings.Add("Text", this.DataSource, "Expences");
            lbl_InvoiceType.DataBindings.Add("Text", this.DataSource, "InvoiceType");
            lbl_Net.DataBindings.Add("Text", this.DataSource, "Net");
            lbl_Paid.DataBindings.Add("Text", this.DataSource, "Paid");
            lbl_PartName.DataBindings.Add("Text", this.DataSource, "PartName");
            lbl_PartType.DataBindings.Add("Text", this.DataSource, "PartType");
            lbl_Phone.DataBindings.Add("Text", this.DataSource, "Phone");
            lbl_Qty.DataBindings.Add("Text", this.DataSource, "ProductCount");
            lbl_Remining.DataBindings.Add("Text", this.DataSource, "Remaing");
            lbl_Tax.DataBindings.Add("Text", this.DataSource, "TaxValue");
            lbl_Total.DataBindings.Add("Text", this.DataSource, "Total");
            //lbl_NetText.DataBindings.Add("Text", this.DataSource, "Net");

            //  ربط الأصناف في التقرير بقاعدة البيانات
            cell_Price.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Price"));
            cell_Product.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "ProductName"));
            cell_Qty.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "ItemQty"));
            cell_Unit.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "UnitName"));
            cell_Total.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "TotalPrice"));
        }
        /// <summary>
        /// دالة ارسال البيانات للتقرير
        /// </summary>
        /// <param name="ds"></param>
        public static void Print(object ds)
        {
            Reporting.rpt_Invoice rpt = new Reporting.rpt_Invoice();
            rpt.DataSource = ds;
            rpt.DetailReport.DataSource = rpt.DataSource;
            rpt.DetailReport.DataMember = "Products";
            rpt.BindData();
            // الطباعة بحسب اعدادات المستخدم
            switch (Session.CurrentSettings.InvoicePrintMode)
            {
                case Master.PrintMode.Direct:
                    rpt.Print();
                    break;
                case Master.PrintMode.ShowPreview:
                    rpt.ShowPreview();
                    break;
                case Master.PrintMode.ShowDialog:
                    rpt.PrintDialog();
                    break;
                default:
                    break;
            }
            rpt.ShowPreview();
        }
        /// <summary>
        /// دالة التفقيط في الفاتورة
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lbl_NetText_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_NetText.Text = NumberToText.Utils.ConvertMoneyToArabicText(lbl_Net.Text);
        }

        /// <summary>
        /// دالة الترقيم المتسلسل في الفاتورة
        /// </summary>
        int index = 1;
        private void Cell_Index_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cell_Index.Text = (index++).ToString();
        }
        /// <summary>
        /// بدأ الترقيم من جديد لكل فاتورة عند طباعة مجموعة من الفواتير مع بعض
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lbl_Code_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            index = 1;
        }
    }
}
