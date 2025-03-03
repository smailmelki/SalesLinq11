using System;
using DevExpress.XtraSplashScreen;

namespace SalesLinq.Forms
{
    public partial class StartSplash : SplashScreen
    {
        public StartSplash()
        {
            InitializeComponent();
            this.labelCopyright.Text = "Copyright © 2011-" + DateTime.Now.Year.ToString();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}