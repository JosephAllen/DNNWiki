using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;

namespace DotNetNuke.Wiki.Views
{
    partial class PageRatings : WikiModuleBase
    {
        #region Ctor

        public PageRatings()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
        }

        #endregion Ctor

        #region Variables

        private Topic mTopic;
        private WikiModuleBase mModule;

        #endregion Variables

        #region Properties

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

        #endregion Properties

        #region Events

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

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        #endregion Events

        #region Methods

        private void LoadLocalization()
        {
            NoRating.Text = Localization.GetString("PageRatingsNotRatedYet", RouterResourceFile);
            RatingLbl.Text = Localization.GetString("PageRatingsTitle", RouterResourceFile);
        }

        #endregion Methods
    }
}