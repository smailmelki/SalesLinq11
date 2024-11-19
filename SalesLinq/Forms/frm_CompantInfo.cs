using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_CompantInfo : XtraForm
    {
        DAL.DBDataContext db ;
        DAL.CompanyInfo info ;
        public frm_CompantInfo()
        {
            InitializeComponent();
            this.Load += Frm_CompantInfo_Load;
        }

        private void Frm_CompantInfo_Load(object sender, EventArgs e)
        {
            getData();
        }      

        private void Btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            save();
        }

        private void Frm_CompantInfo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                save();
            }
        }
        void save()
        {
            if (txtName.Text.Trim() == string.Empty)
            {
                txtName.ErrorText = ("الرجاء ادخال اسم الشركة");
                return;
            }
            db = new DAL.DBDataContext();
            info = db.CompanyInfos.FirstOrDefault();
            // اذا كانت البانات فارغة نضيف جديد
            if (info == null)
            {
                info = new DAL.CompanyInfo();
                db.CompanyInfos.InsertOnSubmit(info);
            }
            setData();
            db.SubmitChanges();// اعتماد التغييرات
            Session.ResetCompanyInfo();
            XtraMessageBox.Show("تمت عملية الحفظ بنجاح");
        }

        void getData()
        {
            db = new DAL.DBDataContext();
            info = db.CompanyInfos.FirstOrDefault();
            if (info == null)
                return;
            txtName.Text = info.CompanyName;
            txtPhone.Text = info.Phone;
            txtMobile.Text = info.Mobile;
            txtAddress.Text = info.Address;
            if (info.Logo != null)
                pictureEdit1.Image = Utils.GetImageFromByteArray(info.Logo.ToArray());
        }
        void setData()
        {
            info.CompanyName = txtName.Text;
            info.Mobile = txtMobile.Text;
            info.Phone = txtPhone.Text;
            info.Address = txtAddress.Text;
            if (pictureEdit1.Image != null)
                info.Logo = Utils.GetByteFromImage(pictureEdit1.Image);
        }
    }
}