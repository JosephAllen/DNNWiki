﻿//
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

using DotNetNuke.Wiki.Utilities;
using DotNetNuke.Services.Localization;
using System;
using System.Globalization;

namespace DotNetNuke.Wiki.Views
{
    partial class TopicHistory : WikiModuleBase
    {
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

        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();

            if (!this.IsPostBack)
            {
                this.RestoreLbl.Visible = false;
                this.cmdRestore.Visible = false;
                if ((this.Request.QueryString["ShowHistory"] != null))
                {
                    this.ShowOldVersion();
                }
                else
                {
                    this.ShowTopicHistoryList();
                }
            }
        }

        private void LoadLocalization()
        {
            Label1.Text = Localization.GetString("HistoryTitle", RouterResourceFile);
            BackBtn.Text = Localization.GetString("HistoryBack", RouterResourceFile);
            cmdRestore.Text = Localization.GetString("HistoryRestore", RouterResourceFile);
            RestoreLbl.Text = Localization.GetString("HistoryRestoreNotice", RouterResourceFile);
        }

        private void ShowOldVersion()
        {
            if (this.CanEdit)
            {
                this.RestoreLbl.Visible = true;
                this.cmdRestore.Visible = true;
            }
            string HistoryPK = null;
            string DateTime = null;
            string HomePage = null;
            HistoryPK = this.Request.QueryString["ShowHistory"];
            var topicHistory = TopicHistoryBo.GetItem(int.Parse(HistoryPK));
            this.lblPageTopic.Text = PageTopic.Replace(WikiHomeName, "Home");
            this.lblPageContent.Text = topicHistory.Cache;
            this.lblDateTime.Text = string.Format(Localization.GetString("HistoryAsOf", RouterResourceFile), topicHistory.UpdateDate.ToString(CultureInfo.CurrentCulture));
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, this.PortalSettings, string.Empty, "loc=TopicHistory", "topic=" +
                WikiMarkup.EncodeTitle(this.PageTopic));
        }

        private void ShowTopicHistoryList()
        {
            this.lblPageTopic.Text = PageTopic.Replace(WikiHomeName, "Home");

            this.lblDateTime.Text = "...";
            this.lblPageContent.Text = Localization.GetString("HistoryListHeader", RouterResourceFile) + " <br /> " + CreateHistoryTable();
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" +
                WikiMarkup.EncodeTitle(PageTopic));
        }

        private void cmdRestore_Click(System.Object sender, System.EventArgs e)
        {
            if ((this.Request.QueryString["ShowHistory"] != null))
            {
                string HistoryPK = this.Request.QueryString["ShowHistory"];
                var th = TopicHistoryBo.GetItem(int.Parse(HistoryPK));
                th.TabID = TabId;
                th.PortalSettings = PortalSettings;
                var tho = new DotNetNuke.Wiki.BusinessObjects.Models.TopicHistory();
                tho.TabID = TabId;
                tho.PortalSettings = PortalSettings;
                tho.Content = _Topic.Content;
                tho.TopicId = TopicId;
                tho.UpdatedBy = _Topic.UpdatedBy;
                tho.UpdateDate = _Topic.UpdateDate;
                tho.Name = this.PageTopic;
                tho.Title = _Topic.Title;
                tho.UpdatedByUserID = _Topic.UpdatedByUserID;

                _Topic.Content = th.Content;
                _Topic.Name = th.Name;
                _Topic.Title = th.Title;
                _Topic.Keywords = th.Keywords;
                _Topic.Description = th.Description;
                _Topic.UpdatedBy = UserInfo.Username;
                _Topic.UpdateDate = DateTime.Now;
                _Topic.UpdatedByUserID = UserId;
                TopicBo.Update(_Topic);
                TopicHistoryBo.Add(tho);

                Response.Redirect(HomeURL, true);
            }
        }

        public TopicHistory()
        {
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}