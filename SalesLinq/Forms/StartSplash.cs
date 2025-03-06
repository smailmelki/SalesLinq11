using System;
using System.Linq;
using DevExpress.XtraSplashScreen;

namespace SalesLinq.Forms
{
    public partial class StartSplash : SplashScreen
    {
        public StartSplash()
        {            
            InitializeComponent();
            this.Load += StartSplash_Load;          
        }

        private void StartSplash_Load(object sender, EventArgs e)
        {
            using (var db = new DAL.DBDataContext() )
            {
                var info = db.CompanyInfos.FirstOrDefault();
                if (info != null && info.Logo != null)
                    this.peLogo.Image = Class.Utils.GetImageFromByteArray(info.Logo.ToArray());
                this.labelVersion.Text = "Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                this.labelCopyright.Text = "Copyright © 2011-" + DateTime.Now.Year.ToString();
            }          
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