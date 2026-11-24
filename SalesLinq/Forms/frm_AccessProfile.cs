using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using SalesLinq.Class;

namespace SalesLinq.Forms
{
    public partial class frm_AccessProfile : NativeMasterForm
    {
        private DAL.UserAccessProfileName profile;
        private BindingList<ScreensAccessProfile> screenPermissions;

        public frm_AccessProfile()
        {
            InitializeComponent();
            New();
        }

        public frm_AccessProfile(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DBDataContext())
            {
                profile = db.UserAccessProfileNames.SingleOrDefault(x => x.ID == id);
            }

            if (profile == null)
                throw new InvalidOperationException("نموذج الصلاحيات المطلوب غير موجود.");

            txt_Name.Text = profile.Name;
            LoadPermissions();
            IsNew = false;
            btn_Delete.Enabled = true;
            PartID = profile.ID;
            PartName = profile.Name;
        }

        public override void New()
        {
            profile = new DAL.UserAccessProfileName();
            txt_Name.Clear();
            LoadPermissions();
            IsNew = true;
            PartID = 0;
            PartName = string.Empty;
            btn_Delete.Enabled = false;
            txt_Name.Focus();
        }

        private void LoadPermissions()
        {
            using (var db = new DAL.DBDataContext())
            {
                var saved = db.UserAccessProfileDetails
                    .Where(x => x.ProfileID == profile.ID)
                    .ToList()
                    .ToDictionary(x => x.ScreenID);

                var rows = Screens.GetScreens
                    .OrderBy(x => x.ScreenID)
                    .Select(screen =>
                    {
                        DAL.UserAccessProfileDetail detail;
                        bool exists = saved.TryGetValue(screen.ScreenID, out detail);
                        return new ScreensAccessProfile(screen.ScreenName)
                        {
                            ScreenID = screen.ScreenID,
                            ParentScreenID = screen.ParentScreenID,
                            ScreenName = screen.ScreenName,
                            ScreenCaption = screen.ScreenCaption,
                            Actions = screen.Actions,
                            CanShow = exists ? detail.CanShow : true,
                            CanOpen = exists ? detail.CanOpen : true,
                            CanAdd = exists ? detail.CanAdd : true,
                            CanEdit = exists ? detail.CanEdit : true,
                            CanDelete = exists ? detail.CanDelete : true,
                            CanPrint = exists ? detail.CanPrint : true
                        };
                    }).ToList();

                screenPermissions = new BindingList<ScreensAccessProfile>(rows);
                gridPermissions.DataSource = screenPermissions;
            }

            FormatPermissionGrid();
        }

        private void FormatPermissionGrid()
        {
            if (gridPermissions.Columns.Count == 0) return;

            string[] permissionColumns =
            {
                nameof(ScreensAccessProfile.CanShow),
                nameof(ScreensAccessProfile.CanOpen),
                nameof(ScreensAccessProfile.CanAdd),
                nameof(ScreensAccessProfile.CanEdit),
                nameof(ScreensAccessProfile.CanDelete),
                nameof(ScreensAccessProfile.CanPrint)
            };

            foreach (string column in permissionColumns)
            {
                gridPermissions.Columns[column].HeaderText = GetPermissionCaption(column);
                gridPermissions.Columns[column].Width = 65;
                gridPermissions.Columns[column].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            gridPermissions.Columns[nameof(ScreensAccessProfile.ScreenName)].Visible = false;
            gridPermissions.Columns[nameof(ScreensAccessProfile.ScreenID)].Visible = false;
            gridPermissions.Columns[nameof(ScreensAccessProfile.ParentScreenID)].Visible = false;
            gridPermissions.Columns[nameof(ScreensAccessProfile.Actions)].Visible = false;

            var captionColumn = gridPermissions.Columns[nameof(ScreensAccessProfile.ScreenCaption)];
            captionColumn.HeaderText = "الشاشة";
            captionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            captionColumn.ReadOnly = true;
            captionColumn.DisplayIndex = 0;

            foreach (DataGridViewRow row in gridPermissions.Rows)
            {
                var item = row.DataBoundItem as ScreensAccessProfile;
                if (item == null) continue;

                int depth = GetDepth(item);
                row.Cells[nameof(ScreensAccessProfile.ScreenCaption)].Value =
                    new string('　', depth * 2) + item.ScreenCaption;

                foreach (string column in permissionColumns)
                {
                    Master.Actions action = GetAction(column);
                    if (!item.Actions.Contains(action))
                    {
                        row.Cells[column].ReadOnly = true;
                        row.Cells[column].Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
        }

        private int GetDepth(ScreensAccessProfile item)
        {
            int depth = 0;
            int parentId = item.ParentScreenID;
            while (parentId != 0 && depth < 20)
            {
                var parent = screenPermissions.FirstOrDefault(x => x.ScreenID == parentId);
                if (parent == null) break;
                depth++;
                parentId = parent.ParentScreenID;
            }
            return depth;
        }

        private static string GetPermissionCaption(string property)
        {
            switch (property)
            {
                case nameof(ScreensAccessProfile.CanShow): return "عرض";
                case nameof(ScreensAccessProfile.CanOpen): return "فتح";
                case nameof(ScreensAccessProfile.CanAdd): return "إضافة";
                case nameof(ScreensAccessProfile.CanEdit): return "تعديل";
                case nameof(ScreensAccessProfile.CanDelete): return "حذف";
                case nameof(ScreensAccessProfile.CanPrint): return "طباعة";
                default: return property;
            }
        }

        private static Master.Actions GetAction(string property)
        {
            switch (property)
            {
                case nameof(ScreensAccessProfile.CanShow): return Master.Actions.Show;
                case nameof(ScreensAccessProfile.CanOpen): return Master.Actions.Open;
                case nameof(ScreensAccessProfile.CanAdd): return Master.Actions.Add;
                case nameof(ScreensAccessProfile.CanEdit): return Master.Actions.Edit;
                case nameof(ScreensAccessProfile.CanDelete): return Master.Actions.Delete;
                case nameof(ScreensAccessProfile.CanPrint): return Master.Actions.Print;
                default: throw new ArgumentOutOfRangeException(nameof(property));
            }
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
                bool duplicate = db.UserAccessProfileNames.Any(
                    x => x.Name == name && x.ID != profile.ID);
                if (duplicate)
                {
                    errorProvider.SetError(txt_Name, "هذا الاسم مستخدم بالفعل.");
                    return false;
                }
            }

            return true;
        }

        public override void Save()
        {
            try
            {
                gridPermissions.EndEdit();
                Validate();

                using (var db = new DAL.DBDataContext())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            DAL.UserAccessProfileName entity;
                            if (profile.ID == 0)
                            {
                                entity = new DAL.UserAccessProfileName();
                                db.UserAccessProfileNames.InsertOnSubmit(entity);
                            }
                            else
                            {
                                entity = db.UserAccessProfileNames.Single(
                                    x => x.ID == profile.ID);
                            }

                            entity.Name = txt_Name.Text.Trim();
                            db.SubmitChanges();

                            db.UserAccessProfileDetails.DeleteAllOnSubmit(
                                db.UserAccessProfileDetails.Where(x => x.ProfileID == entity.ID));

                            var details = screenPermissions.Select(x =>
                                new DAL.UserAccessProfileDetail
                                {
                                    ProfileID = entity.ID,
                                    ScreenID = x.ScreenID,
                                    CanShow = x.Actions.Contains(Master.Actions.Show) && x.CanShow,
                                    CanOpen = x.Actions.Contains(Master.Actions.Open) && x.CanOpen,
                                    CanAdd = x.Actions.Contains(Master.Actions.Add) && x.CanAdd,
                                    CanEdit = x.Actions.Contains(Master.Actions.Edit) && x.CanEdit,
                                    CanDelete = x.Actions.Contains(Master.Actions.Delete) && x.CanDelete,
                                    CanPrint = x.Actions.Contains(Master.Actions.Print) && x.CanPrint
                                }).ToList();

                            db.UserAccessProfileDetails.InsertAllOnSubmit(details);
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
                MessageBox.Show("تعذر حفظ نموذج الصلاحيات.\n" + ex.Message,
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
                    if (db.Users.Any(x => x.ScreenProfileID == profile.ID))
                    {
                        MessageBox.Show("لا يمكن حذف نموذج الصلاحيات لأنه مرتبط بمستخدم.",
                            "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        db.Transaction = transaction;
                        try
                        {
                            db.UserAccessProfileDetails.DeleteAllOnSubmit(
                                db.UserAccessProfileDetails.Where(x => x.ProfileID == profile.ID));
                            var entity = db.UserAccessProfileNames.Single(
                                x => x.ID == profile.ID);
                            db.UserAccessProfileNames.DeleteOnSubmit(entity);
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
                MessageBox.Show("تعذر حذف نموذج الصلاحيات.\n" + ex.Message,
                    "تعذر الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}