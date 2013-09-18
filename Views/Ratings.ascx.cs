//
// DotNetNuke® - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
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
//

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

        public WikiModuleBase MyModule
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

        #region " Web Form Designer Generated Code "

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

        private void LoadLocalization()
        {
            RatePagelbl.Text = Localization.GetString("RatingsRateThisPage", RouterResourceFile);
            LowRating.Text = Localization.GetString("RatingsLowRating", RouterResourceFile);
            HighRating.Text = Localization.GetString("RatingsHighRating", RouterResourceFile);
            lblAverageRatingMessage.Text = Localization.GetString("RatingsAverageRatingTitle", RouterResourceFile);
            lblVoteCastMessage.Text = Localization.GetString("RatingsPageRated", RouterResourceFile);
            btnSubmit.Text = Localization.GetString("RatingsSubmitRating", RouterResourceFile);
        }

        private void DisplayHasVoted()
        {
            pnlCastVote.Visible = false;
            pnlVoteCast.Visible = true;
        }

        private void DisplayCanVote()
        {
            pnlCastVote.Visible = true;
            pnlVoteCast.Visible = false;
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
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

                                break; // TODO: might not be correct. Was : Exit Select

                            case 1:
                                currentCount = InnerTopic.RatingTwoCount;
                                break; // TODO: might not be correct. Was : Exit Select

                            case 2:
                                currentCount = InnerTopic.RatingThreeCount;
                                break; // TODO: might not be correct. Was : Exit Select

                            case 3:
                                currentCount = InnerTopic.RatingFourCount;
                                break; // TODO: might not be correct. Was : Exit Select

                            case 4:
                                currentCount = InnerTopic.RatingFiveCount;
                                break; // TODO: might not be correct. Was : Exit Select
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

        public Ratings()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}