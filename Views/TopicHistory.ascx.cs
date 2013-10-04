#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="TopicHistory.ascx.cs" company="DNN Corp®">
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
////--------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Globalization;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Topic History Control Class
    /// </summary>
    partial class TopicHistory : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicHistory"/> class.
        /// </summary>
        public TopicHistory()
        {
            Load += Page_Load;
        }

        #endregion Ctor

        #region Events

        /// <summary>
        /// Handles the Click event of the Restore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdRestore_Click(System.Object sender, System.EventArgs e)
        {
            if (this.Request.QueryString["ShowHistory"] != null)
            {
                string historyPK = this.Request.QueryString["ShowHistory"];
                var topicHistoryItem = TopicHistoryBo.GetItem(int.Parse(historyPK));
                topicHistoryItem.TabID = TabId;
                topicHistoryItem.PortalSettings = PortalSettings;
                var topicHistoryBO = new DotNetNuke.Wiki.BusinessObjects.Models.TopicHistory();
                topicHistoryBO.TabID = TabId;
                topicHistoryBO.PortalSettings = PortalSettings;
                topicHistoryBO.Content = _Topic.Content;
                topicHistoryBO.TopicId = TopicId;
                topicHistoryBO.UpdatedBy = _Topic.UpdatedBy;
                topicHistoryBO.UpdateDate = _Topic.UpdateDate;
                topicHistoryBO.Name = this.PageTopic;
                topicHistoryBO.Title = _Topic.Title;
                topicHistoryBO.UpdatedByUserID = _Topic.UpdatedByUserID;

                _Topic.Content = topicHistoryItem.Content;
                _Topic.Name = topicHistoryItem.Name;
                _Topic.Title = topicHistoryItem.Title;
                _Topic.Keywords = topicHistoryItem.Keywords;
                _Topic.Description = topicHistoryItem.Description;
                _Topic.UpdatedBy = UserInfo.Username;
                _Topic.UpdateDate = DateTime.Now;
                _Topic.UpdatedByUserID = UserId;
                TopicBo.Update(_Topic);
                TopicHistoryBo.Add(topicHistoryBO);

                Response.Redirect(HomeURL, true);
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();

            if (!this.IsPostBack)
            {
                this.RestoreLbl.Visible = false;
                this.cmdRestore.Visible = false;
                if (this.Request.QueryString["ShowHistory"] != null)
                {
                    this.ShowOldVersion();
                }
                else
                {
                    this.ShowTopicHistoryList();
                }
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            Label1.Text = Localization.GetString("HistoryTitle", RouterResourceFile);
            BackBtn.Text = Localization.GetString("HistoryBack", RouterResourceFile);
            cmdRestore.Text = Localization.GetString("HistoryRestore", RouterResourceFile);
            RestoreLbl.Text = Localization.GetString("HistoryRestoreNotice", RouterResourceFile);
        }

        /// <summary>
        /// Shows the old version.
        /// </summary>
        private void ShowOldVersion()
        {
            if (this.CanEdit)
            {
                this.RestoreLbl.Visible = true;
                this.cmdRestore.Visible = true;
            }

            string historyPK = null;
            historyPK = this.Request.QueryString["ShowHistory"];
            var topicHistory = TopicHistoryBo.GetItem(int.Parse(historyPK));
            this.lblPageTopic.Text = PageTopic.Replace(WikiHomeName, "Home");
            this.lblPageContent.Text = topicHistory.Cache;
            this.lblDateTime.Text = string.Format(Localization.GetString("HistoryAsOf", RouterResourceFile), topicHistory.UpdateDate.ToString(CultureInfo.CurrentCulture));
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(
                TabId,
                this.PortalSettings,
                string.Empty,
                "loc=TopicHistory",
                "topic=" + WikiMarkup.EncodeTitle(this.PageTopic));
        }

        /// <summary>
        /// Shows the topic history list.
        /// </summary>
        private void ShowTopicHistoryList()
        {
            this.lblPageTopic.Text = PageTopic.Replace(WikiHomeName, "Home");

            this.lblDateTime.Text = "...";
            this.lblPageContent.Text = Localization.GetString("HistoryListHeader", RouterResourceFile) + " <br /> " + CreateHistoryTable();
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(
                this.TabId,
                this.PortalSettings,
                string.Empty,
                "topic=" + WikiMarkup.EncodeTitle(PageTopic));
        }

        #endregion Methods
    }
}