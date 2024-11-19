using DevExpress.LookAndFeel;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SalesLinq.Class
{
    public static class Utils
    {
        /// <summary>
        ///invoice Layouts دالة انشاء مسار حفظ 
        /// </summary>
        public static string ApplicationLayoutsPath
        {
            get
            {
                var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);//المسار
                var appName = "xtraPOS";//اسم الملف
                var path= Path.Combine(myDocuments, appName, "Layouts");           // المسار كامل
                //var path= myDocuments + "\\Layouts\\" + appName;                 // طريقة أخرى
                Directory.CreateDirectory(path);                                   // انششاء المسار في حالة كان غير موجود
                return path;
            }
        }

        public static void AddButtonToGroupHeader(this GridView view, SvgImage img, EventHandler eventHandler)
        {
            var CustomClass = new CustomHeaderButtonClass(view, img, eventHandler);
        }
        public static void AddClearValueButton(this PopupBaseEdit edit)
        {
            edit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear)});
            //اضافة حدث الضغط على الزر الموجود داخل الأداة
            edit.ButtonPressed += (sender,e)=> {
                if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Clear)
                {
                    ((PopupBaseEdit)sender).EditValue = null;
                }
            };
        }

        /// <summary>
        /// تحويل الصورة الى النظام الثنائي
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Byte[] GetByteFromImage(Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (image == null)
                    return stream.ToArray();

                try
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    return stream.ToArray();
                }
                catch
                {
                    return stream.ToArray();
                }

            }
        }
        /// <summary>
        /// تحويل النظام الثنائي الى صورة
        /// </summary>
        /// <param name="ByteArray"></param>
        /// <returns></returns>
        public static Image GetImageFromByteArray(Byte[] ByteArray)
        {
            Image img;
            try
            {
                Byte[] imgbyte = ByteArray;
                MemoryStream stream = new MemoryStream(imgbyte, false);
                img = Image.FromStream(stream);
            }
            catch { img = null; }
            return img;
        }
        /// <summary>
        ///invoice Layouts دالة حفظ
        /// </summary>
        /// <param name="view"></param>
        /// <param name="ParentName"></param>
        public static void SaveLayouts(this GridView view, string ParentName)
        {
            var filePath = $"{ApplicationLayoutsPath}\\{ParentName}_{view.Name}";
            view.SaveLayoutToXml(filePath);
        }
        /// <summary>
        ///invoice Layouts دالة استرجاع
        ///الخاصة بالغريد فيو
        /// </summary>
        /// <param name="view"></param>
        /// <param name="ParentName"></param>
        public static void RestorLayouts(this GridView view, string ParentName)
        {
            try
            {
                var filePath = $"{ApplicationLayoutsPath}\\{ParentName}_{view.Name}";
                if (File.Exists(filePath))
                    view.RestorLayouts(filePath);
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// invoice Layouts دالة استرجاع
        /// الخاصة بالكونترول
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ParentName"></param>
        public static void RestoreLayout(this LayoutControl control, string ParentName)
        {
            try
            {
                var filePath = $"{ApplicationLayoutsPath}\\{ParentName}_{control.Name}";
                if (File.Exists(filePath))
                    control.RestoreLayoutFromXml(filePath);
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// invoice Layouts دالة حفظ
        /// الخاصة بالكونترول
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ParentName"></param>
        public static void SaveLayout(this LayoutControl control, string ParentName)
        {
            try
            {
                var filePath = $"{ApplicationLayoutsPath}\\{ParentName}_{control.Name}";
                control.SaveLayoutToXml(filePath);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Add Button
        /// </summary>
        public class CustomHeaderButtonClass
        {
            GridView View;
            SvgImage svgImage;
            EventHandler Event;
            int svgSize = 16;
            public CustomHeaderButtonClass(GridView _view, SvgImage _img, EventHandler _eventHandler)
            {
                View = _view;
                svgImage = _img;
                Event = _eventHandler;
                View.CustomDrawGroupPanel += View_CustomDrawGroupPanel;
                View.MouseMove += View_MouseMove;
                View.Click += View_Click;
            }

            private void View_Click(object sender, EventArgs e)
            {
                DevExpress.Utils.DXMouseEventArgs ea = e as DevExpress.Utils.DXMouseEventArgs;
                if (r.Contains(ea.Location))
                    Event(sender, e);
            }

            private void View_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                IsInRectangle = r.Contains(e.Location);
                View.Invalidate();
            }

            bool IsInRectangle;
            Rectangle r;
            private void View_CustomDrawGroupPanel(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
            {
                Brush brush2 = e.Cache.GetGradientBrush(e.Bounds, Color.LightYellow, Color.WhiteSmoke
                    , System.Drawing.Drawing2D.LinearGradientMode.Horizontal);

                e.Cache.FillRectangle(brush2, e.Bounds);


                SvgBitmap bitmap = SvgBitmap.Create(svgImage);
                r = new Rectangle(
                   /* e.Bounds.X +*/ e.Bounds.Width - (svgSize * 3),
                    e.Bounds.Y + ((e.Bounds.Height - svgSize) / 2),
                    svgSize, svgSize);
                var palette = SvgPaletteHelper.GetSvgPalette(UserLookAndFeel.Default,
                    DevExpress.Utils.Drawing.ObjectState.Normal);
                e.Cache.DrawImage(bitmap.Render(palette), r);


                int thickness = IsInRectangle ? 2 : 1;
                int Offset = thickness + 1;
                e.Cache.DrawRectangle(r.X - Offset, r.Y - Offset, r.Width + (Offset * 2), r.Height + (Offset * 2),
                    Color.Black, thickness);

                e.Handled = true;
            }
        }
    }
}
