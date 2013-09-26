#region Copyright

//
// DotNetNuke� - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion Copyright

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