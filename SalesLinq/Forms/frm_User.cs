using System;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;
using Liphsoft.Crypto.Argon2;

namespace SalesLinq.Forms
{
    public partial class frm_User : NativeMasterForm
    {
        private DAL.User user;

        public frm_User()
        {
            InitializeComponent();
            cmb_UserType.SelectedValueChanged += (s, e) => UpdateProfileControls();
            RefreshData();
            New();
        }

        public frm_User(int id)
        {
            InitializeComponent();
            RefreshData();
            LoadUser(id);
        }

        private void LoadUser(int id)
        {
            using (var db = new DAL.DBDataContext())
            {
                user = db.Users.SingleOrDefault(x => x.ID == id);
                if (user == null)
                    throw new InvalidOperationException("المستخدم المطلوب غير موجود.");

                GetData();
                IsNew = false;
                btn_Delete.Enabled = true;
            }
        }

        public override void RefreshData()
        {
            cmb_UserType.DataSource = Master.UserTypeList.ToList();
            cmb_UserType.DisplayMember = "Name";
            cmb_UserType.ValueMember = "ID";

            using (var db = new DAL.DBDataContext())
            {
                cmb_SettingProfile.DataSource = db.UserSettingsProfiles
                    .OrderBy(x => x.Name)
                    .Select(x => new ProfileOption { ID = x.ID, Name = x.Name })
                    .ToList();
                cmb_SettingProfile.DisplayMember = "Name";
                cmb_SettingProfile.ValueMember = "ID";

                cmb_ScreenProfile.DataSource = db.UserAccessProfileNames
                    .OrderBy(x => x.Name)
                    .Select(x => new ProfileOption { ID = x.ID, Name = x.Name })
                    .ToList();
                cmb_ScreenProfile.DisplayMember = "Name";
                cmb_ScreenProfile.ValueMember = "ID";
            }

            UpdateProfileControls();
        }

        private void UpdateProfileControls()
        {
            bool isAdmin = Convert.ToInt32(cmb_UserType.SelectedValue ?? 0) ==
                           (int)Master.UserType.Admin;

            cmb_ScreenProfile.Enabled = !isAdmin;
            lbl_ScreenProfile.Enabled = !isAdmin;
        }

        public override void New()
        {
            user = new DAL.User { IsActive = true };

            txt_Name.Clear();
            txt_UserName.Clear();
            txt_Password.Clear();
            cmb_UserType.SelectedValue = (int)Master.UserType.User;
            if (cmb_SettingProfile.Items.Count > 0)
                cmb_SettingProfile.SelectedIndex = 0;
            if (cmb_ScreenProfile.Items.Count > 0)
                cmb_ScreenProfile.SelectedIndex = 0;
            chk_IsActive.Checked = true;

            IsNew = true;
            PartID = 0;
            PartName = string.Empty;
            btn_Delete.Enabled = false;
            UpdateProfileControls();
            txt_Name.Focus();
        }

        public override void GetData()
        {
            if (user == null) return;

            txt_Name.Text = user.Name;
            txt_UserName.Text = user.UserName;
            // لا نعرض قيمة التجزئة المخزنة؛ ترك كلمة المرور فارغة يعني الإبقاء عليها.
            txt_Password.Clear();
            cmb_UserType.SelectedValue = (int)user.UserType;
            cmb_SettingProfile.SelectedValue = user.SettingsProfileID;
            if (user.ScreenProfileID > 0)
                cmb_ScreenProfile.SelectedValue = user.ScreenProfileID;
            chk_IsActive.Checked = user.IsActive;

            PartID = user.ID;
            PartName = user.Name;
            UpdateProfileControls();
        }

        public override bool IsDataValide()
        {
            bool valid = true;
            string name = txt_Name.Text.Trim();
            string userName = txt_UserName.Text.Trim();
            bool isNew = user == null || user.ID == 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider.SetError(txt_Name, ErrorText);
                valid = false;
            }
            else errorProvider.SetError(txt_Name, string.Empty);

            if (string.IsNullOrWhiteSpace(userName))
            {
                errorProvider.SetError(txt_UserName, ErrorText);
                valid = false;
            }
            else errorProvider.SetError(txt_UserName, string.Empty);

            if (isNew && string.IsNullOrWhiteSpace(txt_Password.Text))
            {
                errorProvider.SetError(txt_Password, "كلمة المرور مطلوبة للمستخدم الجديد.");
                valid = false;
            }
            else errorProvider.SetError(txt_Password, string.Empty);

            if (cmb_UserType.SelectedValue == null)
            {
                errorProvider.SetError(cmb_UserType, "يرجى تحديد نوع الدخول.");
                valid = false;
            }

            if (cmb_SettingProfile.SelectedValue == null)
            {
                errorProvider.SetError(cmb_SettingProfile, "يرجى تحديد نموذج الإعدادات.");
                valid = false;
            }

            bool isAdmin = Convert.ToInt32(cmb_UserType.SelectedValue ?? 0) ==
                           (int)Master.UserType.Admin;
            if (!isAdmin && cmb_ScreenProfile.SelectedValue == null)
            {
                errorProvider.SetError(cmb_ScreenProfile, "يرجى تحديد نموذج الوصول.");
                valid = false;
            }

            if (!valid) return false;

            using (var db = new DAL.DBDataContext())
            {
                if (db.Users.Any(x => x.UserName.Trim() == userName && x.ID != user.ID))
                {
                    errorProvider.SetError(txt_UserName, "اسم الدخول مستخدم بالفعل.");
                    valid = false;
                }

                if (db.Users.Any(x => x.Name.Trim() == name && x.ID != user.ID))
                {
                    errorProvider.SetError(txt_Name, "اسم المستخدم مسجل بالفعل.");
                    valid = false;
                }
            }

            return valid;
        }

        public override void Save()
        {
            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    DAL.User entity;
                    if (user.ID == 0)
                    {
                        entity = new DAL.User();
                        db.Users.InsertOnSubmit(entity);
                    }
                    else
                    {
                        entity = db.Users.SingleOrDefault(x => x.ID == user.ID);
                        if (entity == null)
                            throw new InvalidOperationException("تعذر العثور على المستخدم لتحديثه.");
                    }

                    entity.Name = txt_Name.Text.Trim();
                    entity.UserName = txt_UserName.Text.Trim();
                    entity.UserType = Convert.ToByte(cmb_UserType.SelectedValue);
                    entity.SettingsProfileID = Convert.ToInt32(cmb_SettingProfile.SelectedValue);

                    bool isAdmin = entity.UserType == (byte)Master.UserType.Admin;
                    if (!isAdmin)
                        entity.ScreenProfileID = Convert.ToInt32(cmb_ScreenProfile.SelectedValue);

                    entity.IsActive = chk_IsActive.Checked;

                    if (!string.IsNullOrWhiteSpace(txt_Password.Text))
                        entity.Password = new PasswordHasher().Hash(txt_Password.Text);
                    else if (entity.ID == 0)
                        throw new InvalidOperationException("كلمة المرور مطلوبة للمستخدم الجديد.");

                    db.SubmitChanges();

                    user = entity;
                    PartID = entity.ID;
                    PartName = entity.Name;
                }

                base.Save();
                txt_Password.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حفظ المستخدم.\n" + ex.Message,
                    "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Delete()
        {
            if (user == null || user.ID == 0 || !AskForDeletion())
                return;

            if (Session.User != null && Session.User.ID == user.ID)
            {
                MessageBox.Show("لا يمكن حذف حساب المستخدم الذي سجل الدخول حالياً.",
                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    var entity = db.Users.SingleOrDefault(x => x.ID == user.ID);
                    if (entity == null) return;
                    db.Users.DeleteOnSubmit(entity);
                    db.SubmitChanges();
                }

                base.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حذف المستخدم. قد يكون مرتبطاً بسجلات أخرى.\n" + ex.Message,
                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private sealed class ProfileOption
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}