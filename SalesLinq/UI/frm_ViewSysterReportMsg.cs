using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLinq.UI
{
    public static class ViewActionReport
    {
        /// <summary>
        /// دالة تعرض رسالة حذف عدة فواتير
        /// </summary>
        /// <param name="msg"></param>
        public static void ViewMSG(string msg)
        {
            XtraForm frm = new XtraForm()
            {
                Size = new System.Drawing.Size(400, 600),
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false

            };
            frm.IconOptions.ShowIcon = false;
            MemoEdit memoEdit = new MemoEdit()
            {
                Text = msg,
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            frm.Controls.Add(memoEdit);
            frm.ShowDialog();
        }
    }
}
