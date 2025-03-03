using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using SalesLinq.Class;
using SalesLinq.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SalesLinq.Forms
{
    public partial class frm_Main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        // Static من أجل جعل الفورم الرئيسية ثابتة
        private static frm_Main _Instance;
        public static frm_Main Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new frm_Main();
                return _Instance;
            }
        }
        public frm_Main()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }
        /// <summary>
        /// حدث الضغط على Element في الفورم الرئيسية
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            var tag = e.Element.Tag as string;
            if (tag != string.Empty)
            {
                OpenFormByName(tag);
            }
        }
        /// <summary>
        /// دالة لفتح الفورم باسمها فقط
        /// </summary>
        /// <param name="name"> ارسال اسم الفورم </param>
        public static void OpenFormByName(string name)
        {
            Form frm = null;
            switch (name)
            {
                case "frm_Vendor":
                    frm = new frm_CustomerVendor(false);
                    break;
                case "frm_Customer":
                    frm = new frm_CustomerVendor(true);
                    break;
                case "frm_VendorList":
                    frm = new frm_CustomerVendorList(false);
                    break;
                case "frm_CustomerList":
                    frm = new frm_CustomerVendorList(true);
                    break;
                case "frm_PurchaseInvoice":
                    frm = new frm_Invoice(Class.Master.InvoiceType.Purchase);
                    break;
                case "frm_SalesInvoice":
                    frm = new frm_Invoice(Class.Master.InvoiceType.Sales);
                    break;
                case "frm_PurchaseInvoiceList":
                    frm = new frm_InvoiceList(Class.Master.InvoiceType.Purchase);
                    break;
                case "frm_SalesInvoiceList":
                    frm = new frm_InvoiceList(Class.Master.InvoiceType.Sales);
                    break;
                case "frm_PurchaseReturnInvoiceList":
                    frm = new frm_InvoiceList(Class.Master.InvoiceType.PurchaseReturn);
                    break;
                case "frm_SalesReturnInvoiceList":
                    frm = new frm_InvoiceList(Class.Master.InvoiceType.SalesReturn);
                    break;
                case "frm_PurchaseReturnInvoice":
                    frm = new frm_Invoice(Class.Master.InvoiceType.PurchaseReturn);
                    break;
                case "frm_SalesReturnInvoice":
                    frm = new frm_Invoice(Class.Master.InvoiceType.SalesReturn);
                    break;
                default:
                    var ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == name);
                    if (ins != null)
                    {
                        frm = Activator.CreateInstance(ins) as Form;
                        if (Application.OpenForms[frm.Name] != null)
                        {
                            frm = Application.OpenForms[frm.Name];
                        }
                        else
                        {
                            //frm.Show();
                        }
                        frm.BringToFront();

                    }
                    break;
            }
            if (frm != null)
            {
                frm.Name = name;
                OpenForm(frm);
            }
        }

        public static void OpenForm(Form frm, bool OpenInDialog = false)
        {
            if (Session.User.UserType == (byte)Master.UserType.Admin)
            {
                if (OpenInDialog)
                    frm.ShowDialog();
                else
                    frm.Show();
                return;
            }
            var screen = Session.ScreenAccesses.SingleOrDefault(x => x.ScreenName == frm.Name);
            if (screen != null)
            {
                if (screen.CanOpen == true)
                {
                    if (OpenInDialog)
                        frm.ShowDialog();
                    else
                        frm.Show();
                    frm.Icon = Icon.ExtractAssociatedIcon("SalesLinq.exe");
                    return;
                }
                else
                {
                    XtraMessageBox.Show(
                                        text: " غير مصرح لك",
                                        caption: string.Empty,
                                        icon: MessageBoxIcon.Error,
                                        buttons: MessageBoxButtons.OK
                                        );
                    return;
                }
            }
        }

        /// <summary>
        /// دالة لحفظ السمة عند اغلاق البرنامج
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // التحقق إذا كان الإغلاق قد تم إلغاؤه بالفعل
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // عرض رسالة تأكيد للمستخدم قبل الإغلاق
                DialogResult result = XtraMessageBox.Show(
                    "هل أنت متأكد أنك تريد إغلاق التطبيق؟",  // نص الرسالة
                    "تنبيه",  // عنوان الرسالة
                    MessageBoxButtons.YesNo,  // الأزرار التي ستظهر للمستخدم
                    MessageBoxIcon.Question,  // نوع الأيقونة التي ستظهر
                    MessageBoxDefaultButton.Button2
                );

                // إذا اختار المستخدم نعم، يتم حفظ الإعدادات وإغلاق التطبيق
                if (result == DialogResult.Yes)
                {
                    Settings.Default.SkinName = UserLookAndFeel.Default.SkinName;  // حفظ اسم السمة
                    Settings.Default.PaletteName = UserLookAndFeel.Default.ActiveSvgPaletteName;  // حفظ لوحة الألوان
                    Settings.Default.Save();  // حفظ الإعدادات
                    Application.Exit();  // إغلاق التطبيق
                }
                else
                {
                    e.Cancel = true;  // إلغاء عملية الإغلاق إذا اختار المستخدم لا
                }
            }
        }


        private void Frm_Main_Load(object sender, EventArgs e)
        {
            // تحميل السمة عند فتح البرنامج
            UserLookAndFeel.Default.SkinName = Settings.Default.SkinName.ToString();
            UserLookAndFeel.Default.SetSkinStyle(Settings.Default.SkinName.ToString(), Settings.Default.PaletteName.ToString());
            //DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = Settings.Default.SystemFont;
            this.Icon = Icon.ExtractAssociatedIcon("SalesLinq.exe");
            //frm_Main.Icon = this.Icon;
            //AddAccordionElement();            
        }

        /// <summary>
        /// اضافة الأزرار للقائمة الرئيسية
        /// تصميم الشاشة برمجيا
        /// </summary>
        public void AddAccordionElement()
        {
            // اضافة ازرار المجموعات الرئيسية في الفورم الرئيسي بحسب نوع المستخدم
            accordionControl1.Elements.Clear();
            var screens = Session.ScreenAccesses.Where(x => x.CanShow == true || Session.User.UserType == (byte)Master.UserType.Admin);// اذا كان مدير أو مستخدم لديه صلاحية الوصول للزر
            screens.Where(s => s.ParentScreenID == 0).ToList().ForEach(s =>
            {
                AccordionControlElement elm = new AccordionControlElement();
                elm.Text = s.ScreenCaption;
                elm.Name = s.ScreenName;
                elm.Tag = s.ScreenName;
                elm.Style = ElementStyle.Group;
                accordionControl1.Elements.Add(elm);
                AddSubAccordionElement(elm, s.ScreenID);
            });
        }

        /// <summary>
        /// اضافة الأزرار الفرعية الى أزرار المجموعات الرئيسية في الفورم الرئيسي بحسب نوع المستخدم
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parentID"></param>
        public void AddSubAccordionElement(AccordionControlElement parent, int parentID)
        {
            var screens = Session.ScreenAccesses.Where(x => x.CanShow == true || Session.User.UserType == (byte)Master.UserType.Admin);// اذا كان مدير أو مستخدم لديه صلاحية الوصول للزر
            screens.Where(s => s.ParentScreenID == parentID).ToList().ForEach(s =>
            {
                AccordionControlElement elm = new AccordionControlElement();
                elm.Text = s.ScreenCaption;
                elm.Name = s.ScreenName;
                elm.Tag = s.ScreenName;
                elm.Style = ElementStyle.Item;
                parent.Elements.Add(elm);
            });
        }

        private void Btn_SignOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new Forms.frm_Login();
            frm.ShowDialog();
        }
    }
}