﻿#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="PageRatings.ascx.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//
//      The above copyright notice and this permission notice shall be included in all copies or
//      substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//      NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//      DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//      OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//--------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;

namespace DotNetNuke.Wiki.Views
{
    partial class PageRatings : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRatings"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets the parent module.
        /// </summary>
        /// <value>The parent module.</value>
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

        /// <summary>
        /// Gets the inner topic.
        /// </summary>
        /// <value>The inner topic.</value>
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

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
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

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            NoRating.Text = Localization.GetString("PageRatingsNotRatedYet", RouterResourceFile);
            RatingLbl.Text = Localization.GetString("PageRatingsTitle", RouterResourceFile);
        }

        #endregion Methods
    }
}