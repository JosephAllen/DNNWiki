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
using System.Globalization;

namespace DotNetNuke.Modules.Wiki.Views
{
    partial class TopicHistory : WikiControlBase
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

        #endregion

        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();

            if (!this.IsPostBack())
            {
                this.RestoreLbl.Visible = false;
                this.cmdRestore.Visible = false;
                if ((this.Request.QueryString.Item("ShowHistory") != null))
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
            HistoryPK = this.Request.QueryString.Item("ShowHistory");
            Entities.TopicHistoryInfo th = THC.GetItem(int.Parse(HistoryPK));
            this.lblPageTopic.Text = PageTopic.Replace(this.WikiHomeName, "Home");
            this.lblPageContent.Text = th.Cache;
            this.lblDateTime.Text = string.Format(Localization.GetString("HistoryAsOf", RouterResourceFile), th.UpdateDate.ToString(CultureInfo.CurrentCulture));
            this.BackBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(this.tabID, this.portalSettings, string.Empty, "loc=TopicHistory", "topic=" + Entities.WikiData.EncodeTitle(this.PageTopic));
        }

        private void ShowTopicHistoryList()
        {
            this.lblPageTopic.Text = PageTopic.Replace(this.WikiHomeName, "Home");

            this.lblDateTime.Text = "...";
            this.lblPageContent.Text = Localization.GetString("HistoryListHeader", RouterResourceFile) + " <br /> " + CreateHistoryTable();
            this.BackBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(this.tabID, this.portalSettings, string.Empty, "topic=" + Entities.WikiData.EncodeTitle(PageTopic));
        }

        private void cmdRestore_Click(System.Object sender, System.EventArgs e)
        {
            if ((this.Request.QueryString.Item("ShowHistory") != null))
            {
                string HistoryPK = this.Request.QueryString.Item("ShowHistory");
                Entities.TopicHistoryInfo th = THC.GetItem(int.Parse(HistoryPK));
                th.TabID = tabID;
                th.PortalSettings = portalSettings;
                Entities.TopicHistoryInfo tho = new Entities.TopicHistoryInfo();
                tho.TabID = tabID;
                tho.PortalSettings = portalSettings;
                tho.Content = this.Topic.Content;
                tho.TopicID = this.TopicID;
                tho.UpdatedBy = this.Topic.UpdatedBy;
                tho.UpdateDate = this.Topic.UpdateDate;
                tho.Name = this.PageTopic;
                tho.Title = this.Topic.Title;
                tho.UpdatedByUserID = Topic.UpdatedByUserID;

                this.Topic.Content = th.Content;
                this.Topic.Name = th.Name;
                this.Topic.Title = th.Title;
                this.Topic.Keywords = th.Keywords;
                this.Topic.Description = th.Description;
                this.Topic.UpdatedBy = UserInfo.Username;
                this.Topic.UpdateDate = DateTime.Now;
                this.Topic.UpdatedByUserID = UserId;
                TC.Update(this.Topic);
                THC.Add(tho);

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