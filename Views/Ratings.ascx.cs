#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Ratings.ascx.cs" company="DNN Corp®">
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
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Views
{
    partial class Ratings : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ratings"/> class.
        /// </summary>
        public Ratings()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
        }

        #endregion Ctor

        #region Variables

        private WikiModuleBase m_Module;
        private Topic m_Topic;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [has voted].
        /// </summary>
        /// <value><c>true</c> if [has voted]; otherwise, /c>.</value>
        public bool HasVoted
        {
            get
            {
                if (Request.Cookies["WikiRating"] == null)
                {
                    return false;
                }
                else
                {
                    if (Request.Cookies["WikiRating"]["ContentID-" + InnerTopic.TopicID.ToString()] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            set
            {
                if (Request.Cookies["WikiRating"] == null)
                {
                    Response.Cookies.Add(new HttpCookie("WikiRating"));
                }
                Response.Cookies["WikiRating"]["ContentID-" + InnerTopic.TopicID.ToString()] = "true";
                Response.Cookies["WikiRating"].Expires = DateTime.Now.AddYears(1);
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
                if (m_Topic == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }
                    m_Topic = ((WikiModuleBase)uplevel)._Topic;
                }
                return m_Topic;
            }
        }

        /// <summary>
        /// Gets my module.
        /// </summary>
        /// <value>My module.</value>
        public WikiModuleBase MyModule
        {
            get
            {
                if (m_Module == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }
                    m_Module = (WikiModuleBase)uplevel;
                    m_Module.ModuleConfiguration = this.ModuleConfiguration;
                }
                return m_Module;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();

            if (this.Visible)
            {
                if (HasVoted)
                {
                    DisplayHasVoted();
                }
                else
                {
                    DisplayCanVote();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            bool save = false;
            save = false;
            if (rating1.Checked)
            {
                InnerTopic.RatingOneCount = InnerTopic.RatingOneCount + 1;
                save = true;
            }
            else if (rating2.Checked)
            {
                InnerTopic.RatingTwoCount = InnerTopic.RatingTwoCount + 1;
                save = true;
            }
            else if (rating3.Checked)
            {
                InnerTopic.RatingThreeCount = InnerTopic.RatingThreeCount + 1;
                save = true;
            }
            else if (rating4.Checked)
            {
                InnerTopic.RatingFourCount = InnerTopic.RatingFourCount + 1;
                save = true;
            }
            else if (rating5.Checked)
            {
                InnerTopic.RatingFiveCount = InnerTopic.RatingFiveCount + 1;
                save = true;
            }
            if (save)
            {
                TopicBo.Update(InnerTopic);
            }
            HasVoted = true;
            DisplayHasVoted();
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                if (InnerTopic.FivePointRatingsRecorded > 0)
                {
                    lblAverageRating.Text = InnerTopic.FivePointAverage.ToString("#.#");
                    lblRatingCount.Text = string.Format(Localization.GetString("RatingsNumberOf", RouterResourceFile),
                        InnerTopic.FivePointRatingsRecorded.ToString());

                    int i = 0;
                    i = 0;
                    for (i = 0; i <= 4; i++)
                    {
                        System.Web.UI.WebControls.Image bcImg = new System.Web.UI.WebControls.Image();
                        bcImg.ImageUrl = DNNWikiModuleRootPath + "/Resources/images/bcImage.gif";
                        bcImg.Width = Unit.Pixel(10);

                        int currentCount = 0;
                        switch ((i))
                        {
                            case 0:
                                currentCount = InnerTopic.RatingOneCount;
                                break;

                            case 1:
                                currentCount = InnerTopic.RatingTwoCount;
                                break;

                            case 2:
                                currentCount = InnerTopic.RatingThreeCount;
                                break;

                            case 3:
                                currentCount = InnerTopic.RatingFourCount;
                                break;

                            case 4:
                                currentCount = InnerTopic.RatingFiveCount;
                                break;
                        }
                        bcImg.Height = Unit.Pixel(Convert.ToInt32(25f * (Convert.ToDouble(currentCount) / (Convert.ToDouble(InnerTopic.FivePointRatingsRecorded)))));
                        bcImg.AlternateText = currentCount.ToString();
                        RatingsGraphTable.Rows[0].Cells[i].Controls.Add(bcImg);
                    }
                }
                else
                {
                    lblAverageRating.Text = Localization.GetString("RatingsNotRatedYet", RouterResourceFile);
                    lblRatingCount.Text = string.Format(Localization.GetString("RatingsNumberOf", RouterResourceFile), "0");

                    RatingsGraphTable.Visible = false;
                }
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Displays the can vote.
        /// </summary>
        private void DisplayCanVote()
        {
            pnlCastVote.Visible = true;
            pnlVoteCast.Visible = false;
        }

        /// <summary>
        /// Displays the has voted.
        /// </summary>
        private void DisplayHasVoted()
        {
            pnlCastVote.Visible = false;
            pnlVoteCast.Visible = true;
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            RatePagelbl.Text = Localization.GetString("RatingsRateThisPage", RouterResourceFile);
            LowRating.Text = Localization.GetString("RatingsLowRating", RouterResourceFile);
            HighRating.Text = Localization.GetString("RatingsHighRating", RouterResourceFile);
            lblAverageRatingMessage.Text = Localization.GetString("RatingsAverageRatingTitle", RouterResourceFile);
            lblVoteCastMessage.Text = Localization.GetString("RatingsPageRated", RouterResourceFile);
            btnSubmit.Text = Localization.GetString("RatingsSubmitRating", RouterResourceFile);
        }

        #endregion Methods
    }
}