using System;
using System.Data;
using System.Linq;
using SalesLinq.Class;
using Liphsoft.Crypto.Argon2;

namespace SalesLinq.Forms
{
    public partial class frm_User : frm_Master
    {
        
        DAL.User user;
        bool UserIsAdmin = false; 
        public frm_User()
        {
            InitializeComponent();
            RefreshData();
            New();
        }
        public frm_User(int id)
        {
            InitializeComponent();
            RefreshData();
            using (var db = new DAL.DBDataContext())
            {
                user = db.Users.SingleOrDefault(x => x.ID == id);
                GetData();
            }
        }
        public override void RefreshData()
        {
            using (var db = new DAL.DBDataContext())
            {
                lkp_UserType.InitializeData(Master.UserTypeList);
                lkp_UserType.EditValueChanged += Lkp_UserType_EditValueChanged;
                lkp_SettingProfileID.InitializeData(db.UserSettingsProfiles.Select(x => new { x.ID, x.Name }).ToList());
                lkp_ScreenProfileID.InitializeData(db.UserAccessProfileNames.Select(x => new { x.ID, x.Name }).ToList());

            }
            base.RefreshData();
        }

        private void Lkp_UserType_EditValueChanged(object sender, EventArgs e)
        {
            if (lkp_UserType.EditValue.ToString() == "1")
            {
                lkp_ScreenProfileID.Enabled = false;
                UserIsAdmin = true;
            }
            else
            {
                lkp_ScreenProfileID.Enabled = true;
                UserIsAdmin = false;
            }
        }

        public override void New()
        {
            user = new DAL.User();
            user.IsActive = true;
            base.New();
        }
        public override void GetData()
        {
            txt_Name.Text = user.Name;
            txt_UserName.Text = user.UserName;
            txt_Passwoord.Text = user.Password;
            lkp_UserType.EditValue = user.UserType;
            lkp_SettingProfileID.EditValue = user.SettingsProfileID;
            if (!UserIsAdmin)
            {
                lkp_ScreenProfileID.EditValue = user.ScreenProfileID;
            }
            toggleSwitch1.IsOn = user.IsActive;
            PartID = user.ID;     // لتسجيل حركات المستخدم
            PartName = user.Name; // لتسجيل حركات المستخدم
            base.GetData();
        }
        public override void SetData()
        {
            //تشفير الرقم السري
            if(user.Password!= txt_Passwoord.Text)
            {
                var hasher = new PasswordHasher();
                string myhash = hasher.Hash(txt_Passwoord.Text);
                txt_Passwoord.Text = myhash;
            }
            user.Name = txt_Name.Text;
            user.Password = txt_Passwoord.Text;
            user.UserName = txt_UserName.Text.Trim();
            user.UserType =Convert.ToByte( lkp_UserType.EditValue);
            user.SettingsProfileID = Convert.ToInt32(lkp_SettingProfileID.EditValue);
            if (!UserIsAdmin)
            {
                user.ScreenProfileID = Convert.ToInt32(lkp_ScreenProfileID.EditValue);
            }             
            user.IsActive = toggleSwitch1.IsOn;
            base.SetData();
        }
        public override void Save()
        {
            using (var db = new DAL.DBDataContext())
            {
                if (user.ID == 0)
                {
                    db.Users.InsertOnSubmit(user);
                }
                else
                {
                    db.Users.Attach(user);
                }
                SetData();
                    db.SubmitChanges();
                    PartID = user.ID;     // لتسجيل حركات المستخدم
                    PartName = user.Name; // لتسجيل حركات المستخدم
                    base.Save();                
            }
        }
        /// <summary>
        /// التحقق من البيانات المدخلة قبل الحفظ
        /// </summary>
        /// <returns></returns>
        public override bool IsDataValide()
        {
            int NumberOfErrors = 0;
            using (var db = new DAL.DBDataContext())
            {
                if (db.Users.Where(x => x.UserName.Trim() == txt_UserName.Text.Trim() && x.ID != user.ID).Count() > 0)
                {
                    NumberOfErrors += 1;
                    txt_UserName.ErrorText = "هذا المستخدم مسجل بالفعل";
                }
                if (db.Users.Where(x => x.Name.Trim() == txt_Name.Text.Trim() && x.ID != user.ID).Count() > 0)
                {
                    NumberOfErrors += 1;
                    txt_Name.ErrorText = "هذا المستخدم مسجل بالفعل";
                }
            }
            NumberOfErrors += txt_Name.IsTextValide() ? 0 : 1;                              // التحقق من أن النص غير فارغ 
            NumberOfErrors += txt_Passwoord.IsTextValide() ? 0 : 1;                         // التحقق من أن النص غير فارغ 
            NumberOfErrors += txt_UserName.IsTextValide() ? 0 : 1;                          // التحقق من أن النص غير فارغ 
            NumberOfErrors += lkp_UserType.IsEditValueValideAndNotZero() ? 0 : 1;           // التحقق من أن النص رقمي و ليس صفر
            if (!UserIsAdmin)
            {
                NumberOfErrors += lkp_ScreenProfileID.IsEditValueValideAndNotZero() ? 0 : 1;    // التحقق من أن النص رقمي و ليس صفر
                NumberOfErrors += lkp_SettingProfileID.IsEditValueValideAndNotZero() ? 0 : 1;   // التحقق من أن النص رقمي و ليس صفر
            }

            return (NumberOfErrors == 0);
        }

        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Users.Attach(user);
                    db.Users.DeleteOnSubmit(user);
                    db.SubmitChanges();
                }
                base.Delete();
            }
        }
    }
}