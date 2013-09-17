using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Wiki.Views
{
    partial class PageRatings : WikiModuleBase
    {
        private WikiModuleBase mModule;
        private Topic mTopic;

        public Topic InnerTopic
        {
            get
            {
                if (mTopic == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }
                    mTopic = ((WikiModuleBase)uplevel)._Topic;
                }
                return mTopic;
            }
        }

        public WikiModuleBase ParentModule
        {
            get
            {
                if (mModule == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }
                    mModule = (WikiModuleBase)uplevel;
                    mModule.ModuleConfiguration = this.ModuleConfiguration;
                }
                return mModule;
            }
        }

        #region " Web Form Designer Generated Code "

        //This call is required by the Web Form Designer.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
        }

        private void Page_Init(System.Object sender, System.EventArgs e)
        {
            //CODEGEN: This method call is required by the Web Form Designer
            //Do not modify it using the code editor.
            InitializeComponent();
        }

        #endregion " Web Form Designer Generated Code "

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (InnerTopic.FivePointRatingsRecorded == 0)
            {
                RatingBar.Visible = false;
                NoRating.Visible = true;
            }
            else
            {
                RatingBar.Visible = true;
                NoRating.Visible = false;
                RatingBar.Src = this.TemplateSourceDirectory + "/RatingBar.aspx?rating=" + InnerTopic.FivePointAverage.ToString("#.#");
                RatingBar.Alt = InnerTopic.FivePointAverage.ToString("#.#");
            }
        }

        private void LoadLocalization()
        {
            NoRating.Text = Localization.GetString("PageRatingsNotRatedYet", RouterResourceFile);
            RatingLbl.Text = Localization.GetString("PageRatingsTitle", RouterResourceFile);
        }

        public PageRatings()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}