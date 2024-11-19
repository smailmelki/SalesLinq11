using System.Drawing;
using DevExpress.XtraReports.UI;
using SalesLinq.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;

namespace SalesLinq.Reporting
{
    public partial class rpt_GridPrint : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_GridPrint()
        {
            InitializeComponent();
            //Session جلب البيانات من 
            if (Session.CompanyInfo != null)
            {
                lbl_CompanyAddress.Text = Class.Session.CompanyInfo.Address;
                lbl_CompanyName.Text = Class.Session.CompanyInfo.CompanyName;
                lbl_CompanyPhone.Text = Class.Session.CompanyInfo.Phone + " - " + Class.Session.CompanyInfo.Mobile;
            }
            if (Class.Session.CompanyInfo.Logo != null)
                xrPictureBox1.Image = Utils.GetImageFromByteArray(Class.Session.CompanyInfo.Logo.ToArray());
        }
        public static void Print(GridControl gridControl,string ReportName,string Filters)
        {
            GridView view = gridControl.MainView as GridView;
            GridMultiSelectMode mode = view.OptionsSelection.MultiSelectMode;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;                                    //Selection اخفاء عمود 

            view.AppearancePrint.EvenRow.BackColor = Color.LightGray;                                                // تغيير لون الأسطر الفردية
            view.AppearancePrint.OddRow.BackColor = Color.WhiteSmoke;                                                // تغيير لون الأسطر الزوجية
            view.OptionsPrint.EnableAppearanceEvenRow =
            view.OptionsPrint.EnableAppearanceOddRow = true;                                                         // يطبق تغييرات لون الأسطر الفردية والزوجية

            view.AppearancePrint.HeaderPanel.BackColor = Color.FromArgb(43,87,151);                                  // تغيير لون رؤوس الأعمدة
            view.AppearancePrint.HeaderPanel.Font = new Font(view.AppearancePrint.HeaderPanel.Font, FontStyle.Bold); //تغيير الخط (bold)
            view.AppearancePrint.HeaderPanel.ForeColor = Color.WhiteSmoke;                                           //لون الخط الخاص برؤوس الاعمدة

            view.AppearancePrint.HeaderPanel.Options.UseBackColor = true;                                            //استعمال لون رؤوس الأعمدة
            view.AppearancePrint.HeaderPanel.Options.UseFont = true;                                                 //استعمال الخط
            view.AppearancePrint.HeaderPanel.Options.UseForeColor = true;                                            //استعمال لون الخط الخاص برؤوس الاعمدة

            view.OptionsPrint.AllowMultilineHeaders = true;                                                          // يرجع للسطر
            view.OptionsPrint.AutoWidth = true;                                                                      // يملأ المكان كله
            view.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;                  // يرجع للسطر
            view.AppearancePrint.Lines.BackColor = Color.LightGray;                                                  // لون الخطوط التي تفصل بين الأسطر والاعمدة

            view.OptionsPrint.UsePrintStyles = true;                                                                 // يطبق تغييرات الطباعة

            rpt_GridPrint rpt = new rpt_GridPrint();
            rpt.Cell_ReportFilter.Text = Filters;
            rpt.Cell_ReportName.Text = ReportName;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = gridControl;
            rpt.printableComponentContainer1.PrintableComponent = link;
            rpt.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            rpt.ShowRibbonPreviewDialog();                                      // يطبع التقرير وبعد اغلاقه يرجع المود السابق في اختيار الأعمدة

            view.OptionsSelection.MultiSelectMode = mode;
        }
    }
}
