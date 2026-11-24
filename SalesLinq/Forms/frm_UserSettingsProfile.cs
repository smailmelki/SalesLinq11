using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_UserSettingsProfile : NativeMasterForm
    {
        private DAL.UserSettingsProfile profile;
        private readonly List<Control> editors = new List<Control>();
        private readonly Dictionary<string, Type> editorValueTypes = new Dictionary<string, Type>();

        public frm_UserSettingsProfile()
        {
            InitializeComponent();
            New();
        }

        public frm_UserSettingsProfile(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DBDataContext())
            {
                profile = db.UserSettingsProfiles.SingleOrDefault(x => x.ID == id);
            }

            if (profile == null)
                throw new InvalidOperationException("نموذج الإعدادات المطلوب غير موجود.");

            txt_Name.Text = profile.Name;
            BuildEditorPages();
            IsNew = false;
            btn_Delete.Enabled = true;
            PartID = profile.ID;
            PartName = profile.Name;
        }

        public override void New()
        {
            profile = new DAL.UserSettingsProfile();
            txt_Name.Clear();
            BuildEditorPages();
            IsNew = true;
            PartID = 0;
            PartName = string.Empty;
            btn_Delete.Enabled = false;
            txt_Name.Focus();
        }

        private void BuildEditorPages()
        {
            editors.Clear();
            editorValueTypes.Clear();
            navigation.Items.Clear();
            tabs.TabPages.Clear();

            var settings = new UserSettingsTemplate(profile.ID);
            foreach (var category in typeof(UserSettingsTemplate).GetProperties())
            {
                object categorySettings = category.GetValue(settings, null);
                if (categorySettings == null) continue;

                string caption = UserSettingsTemplate.GetPropCaption(category.Name);
                var page = new TabPage(caption) { Name = category.Name, AutoScroll = true };
                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 2,
                    Padding = new Padding(12),
                    RightToLeft = RightToLeft.Yes
                };
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

                foreach (var property in categorySettings.GetType().GetProperties())
                {
                    object value = property.GetValue(categorySettings, null);
                    Control editor = UserSettingsTemplate.GetPropertyControl(property.Name, value);
                    if (editor == null) continue;

                    editor.Name = property.Name;
                    editorValueTypes[property.Name] = property.PropertyType;
                    editors.Add(editor);

                    int row = table.RowCount++;
                    table.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                    var label = new Label
                    {
                        Text = UserSettingsTemplate.GetPropCaption(property.Name),
                        Dock = DockStyle.Fill,
                        TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                        AutoSize = true
                    };
                    table.Controls.Add(label, 0, row);
                    table.Controls.Add(editor, 1, row);
                }

                page.Controls.Add(table);
                tabs.TabPages.Add(page);
                navigation.Items.Add(caption);
            }

            if (tabs.TabPages.Count > 0)
            {
                tabs.SelectedIndex = 0;
                navigation.SelectedIndex = 0;
            }
        }

        private void Navigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (navigation.SelectedIndex >= 0 &&
                navigation.SelectedIndex < tabs.TabPages.Count)
                tabs.SelectedIndex = navigation.SelectedIndex;
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedIndex >= 0 && tabs.SelectedIndex < navigation.Items.Count &&
                navigation.SelectedIndex != tabs.SelectedIndex)
                navigation.SelectedIndex = tabs.SelectedIndex;
        }

        public override bool IsDataValide()
        {
            string name = txt_Name.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider.SetError(txt_Name, ErrorText);
                return false;
            }

            errorProvider.SetError(txt_Name, string.Empty);
            using (var db = new DAL.DBDataContext())
            {
                if (db.UserSettingsProfiles.Any(x => x.Name == name && x.ID != profile.ID))
                {
                    errorProvider.SetError(txt_Name, "هذا الاسم مستخدم بالفعل.");
                    return false;
                }
            }

            foreach (var editor in editors)
            {
                var combo = editor as ComboBox;
                if (combo != null && combo.SelectedValue == null)
                {
                    MessageBox.Show("يرجى تحديد قيمة الحقل: " +
                        UserSettingsTemplate.GetPropCaption(combo.Name),
                        "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    combo.Focus();
                    return false;
                }
            }

            return true;
        }

        private object GetEditorValue(Control editor)
        {
            var check = editor as CheckBox;
            if (check != null) return check.Checked;

            var combo = editor as ComboBox;
            if (combo != null)
            {
                object selected = combo.SelectedValue;
                Type targetType = editorValueTypes[editor.Name];
                if (targetType.IsEnum)
                    return Enum.ToObject(targetType, Convert.ToInt32(selected));
                return Convert.ToInt32(selected);
            }

            var numeric = editor as NumericUpDown;
            if (numeric != null) return numeric.Value;

            throw new InvalidOperationException("نوع عنصر إعدادات غير مدعوم: " + editor.Name);
        }

        public override void Save()
        {
            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            DAL.UserSettingsProfile entity;
                            if (profile.ID == 0)
                            {
                                entity = new DAL.UserSettingsProfile();
                                db.UserSettingsProfiles.InsertOnSubmit(entity);
                            }
                            else
                            {
                                entity = db.UserSettingsProfiles.Single(x => x.ID == profile.ID);
                            }

                            entity.Name = txt_Name.Text.Trim();
                            db.SubmitChanges();

                            db.UserSettingsProfileProperties.DeleteAllOnSubmit(
                                db.UserSettingsProfileProperties.Where(x => x.ProfileID == entity.ID));

                            var properties = editors.Select(editor =>
                                new DAL.UserSettingsProfileProperty
                                {
                                    ProfileID = entity.ID,
                                    PropertyName = editor.Name,
                                    PropertyValue = Master.ToByteArray<object>(GetEditorValue(editor))
                                }).ToList();

                            db.UserSettingsProfileProperties.InsertAllOnSubmit(properties);
                            db.SubmitChanges();
                            transaction.Commit();

                            profile = entity;
                            PartID = entity.ID;
                            PartName = entity.Name;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                base.Save();
                txt_Name.Text = profile.Name;
                IsNew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حفظ نموذج الإعدادات.\n" + ex.Message,
                    "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Delete()
        {
            if (profile == null || profile.ID == 0 || !AskForDeletion())
                return;

            try
            {
                using (var db = new DAL.DBDataContext())
                {
                    if (db.Users.Any(x => x.SettingsProfileID == profile.ID))
                    {
                        MessageBox.Show("لا يمكن حذف نموذج الإعدادات لأنه مرتبط بمستخدم.",
                            "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            db.UserSettingsProfileProperties.DeleteAllOnSubmit(
                                db.UserSettingsProfileProperties.Where(x => x.ProfileID == profile.ID));
                            var entity = db.UserSettingsProfiles.Single(x => x.ID == profile.ID);
                            db.UserSettingsProfiles.DeleteOnSubmit(entity);
                            db.SubmitChanges();
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                base.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر حذف نموذج الإعدادات.\n" + ex.Message,
                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}