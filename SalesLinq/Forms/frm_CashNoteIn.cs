using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLinq.Forms
{
    public class frm_CashNoteIn : frm_CashNote
    {
        public frm_CashNoteIn() : base(true)
        {
            lyc_Type.Text = this.Text = "سند قبض نقدي";
        }

    }
    public class frm_CashNoteOut : frm_CashNote
    {
        public frm_CashNoteOut() : base(false)
        {
            lyc_Type.Text = this.Text = "سند دفع نقدي";
        }

    }
}
