//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Wiki.Views
{
    partial class Ratings : WikiControlBase
    {
        private DotNetNuke.Modules.Wiki.WikiControlBase mModule;
        private DotNetNuke.Modules.Wiki.Entities.TopicInfo mTopic;
        public DotNetNuke.Modules.Wiki.Entities.TopicInfo Topic
        {
            get
            {
                if (mTopic == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!uplevel is DotNetNuke.Modules.Wiki.WikiControlBase)
                    {
                        uplevel = uplevel.Parent;
                    }
                    mTopic = ((DotNetNuke.Modules.Wiki.WikiControlBase)uplevel).Topic;
                }
                return mTopic;
            }
        }
        public DotNetNuke.Modules.Wiki.WikiControlBase MyModule
        {
            get
            {
                if (mModule == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!uplevel is DotNetNuke.Modules.Wiki.WikiControlBase)
                    {
                        uplevel = uplevel.Parent;
                    }
                    mModule = (DotNetNuke.Modules.Wiki.WikiControlBase)uplevel;
                    mModule.ModuleConfiguration = this.ModuleConfiguration;
                }
                return mModule;
            }
        }

        public bool HasVoted
        {
            get
            {
                if (Request.Cookies("WikiRating") == null)
                {
                    return false;
                }
                else
                {
                    if (Request.Cookies("WikiRating")("ContentID-" + Topic.TopicID.ToString) == null)
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
                if (Request.Cookies("WikiRating") == null)
                {
                    Response.Cookies.Add(new HttpCookie("WikiRating"));
                }
                Response.Cookies("WikiRating")("ContentID-" + Topic.TopicID.ToString) = "true";
                Response.Cookies("WikiRating").Expires = DateTime.Now.AddYears(1);
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

        #endregion

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
                Topic.RatingOneCount = Topic.RatingOneCount + 1;
                save = true;
            }
            else if (rating2.Checked)
            {
                Topic.RatingTwoCount = Topic.RatingTwoCount + 1;
                save = true;
            }
            else if (rating3.Checked)
            {
                Topic.RatingThreeCount = Topic.RatingThreeCount + 1;
                save = true;
            }
            else if (rating4.Checked)
            {
                Topic.RatingFourCount = Topic.RatingFourCount + 1;
                save = true;
            }
            else if (rating5.Checked)
            {
                Topic.RatingFiveCount = Topic.RatingFiveCount + 1;
                save = true;
            }
            if (save)
            {
                DotNetNuke.Modules.Wiki.Entities.TopicController tc = new DotNetNuke.Modules.Wiki.Entities.TopicController();
                tc.Update(Topic);
            }
            HasVoted = true;
            DisplayHasVoted();
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                if (Topic.FivePointRatingsRecorded > 0)
                {
                    lblAverageRating.Text = Topic.FivePointAverage.ToString("#.#");
                    lblRatingCount.Text = string.Format(Localization.GetString("RatingsNumberOf", RouterResourceFile), Topic.FivePointRatingsRecorded.ToString);

                    int i = 0;
                    i = 0;
                    for (i = 0; i <= 4; i++)
                    {
                        System.Web.UI.WebControls.Image bcImg = new System.Web.UI.WebControls.Image();
                        bcImg.ImageUrl = this.TemplateSourceDirectory + "/images/bcImage.gif";
                        bcImg.Width = Unit.Pixel(10);

                        int currentCount = 0;
                        switch ((i))
                        {
                            case 0:
                                currentCount = Topic.RatingOneCount;

                                break; // TODO: might not be correct. Was : Exit Select

                                break;
                            case 1:
                                currentCount = Topic.RatingTwoCount;
                                break; // TODO: might not be correct. Was : Exit Select

                                break;
                            case 2:
                                currentCount = Topic.RatingThreeCount;
                                break; // TODO: might not be correct. Was : Exit Select

                                break;
                            case 3:
                                currentCount = Topic.RatingFourCount;
                                break; // TODO: might not be correct. Was : Exit Select

                                break;
                            case 4:
                                currentCount = Topic.RatingFiveCount;
                                break; // TODO: might not be correct. Was : Exit Select

                                break;
                        }
                        bcImg.Height = Unit.Pixel(Convert.ToInt32(25f * (Convert.ToDouble(currentCount) / (Convert.ToDouble(Topic.FivePointRatingsRecorded)))));
                        bcImg.AlternateText = currentCount.ToString();
                        RatingsGraphTable.Rows(0).Cells(i).Controls.Add(bcImg);
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