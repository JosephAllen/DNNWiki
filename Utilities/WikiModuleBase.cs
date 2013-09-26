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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace DotNetNuke.Wiki.Utilities
{
    public class WikiModuleBase : PortalModuleBase
    {
        #region Variables

        public const string WikiHomeName = "WikiHomePage";

        private string userName;
        private string firstName;
        private string lastName;
        private bool isAdmin = false;
        private string pageTopic;
        private int topicId;
        private Topic topic;
        private string homeURL;
        private Setting wikiSettings;
        private bool canEdit = false;

        private UnitOfWork _uof;
        private TopicBO topicBo;
        private TopicHistoryBO topicHistoryBo;

        private const string CSS_WikiModuleCssId = "WikiModuleCss";
        private const string CSS_WikiModuleCssPath = "/Resources/Css/module.css";

        #endregion Variables

        #region Ctor

        public WikiModuleBase()
        {
            Load += Page_Load;
            Unload += Page_Unload;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// The path for the module
        /// </summary>
        public string DNNWikiModuleRootPath
        {
            get
            {
                if (this.TemplateSourceDirectory.EndsWith(@"/Views"))
                    return this.TemplateSourceDirectory.Substring(0, this.TemplateSourceDirectory.IndexOf(@"/Views"));
                else if (this.TemplateSourceDirectory.IndexOf(@"/Views/") > 0)
                    return this.TemplateSourceDirectory.Substring(0, this.TemplateSourceDirectory.IndexOf(@"/Views/"));
                return this.TemplateSourceDirectory;
            }
        }

        public Setting WikiSettings
        {
            get { return wikiSettings; }
            set { wikiSettings = value; }
        }

        public string PageTopic
        {
            get { return pageTopic; }
            set { pageTopic = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                this.lastName = value;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                this.firstName = value;
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                this.userName = value;
            }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                this.isAdmin = value;
            }
        }

        public string HomeURL
        {
            get { return homeURL; }
        }

        public int TopicId
        {
            get { return topicId; }
        }

        public bool CanEdit
        {
            get { return canEdit; }
        }

        public Topic _Topic
        {
            get { return topic; }
        }

        public string RouterResourceFile
        {
            get { return DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "Router.ascx.resx"); }
        }

        /// <summary>
        /// Gets an instance of the topic history business object
        /// </summary>
        internal TopicHistoryBO TopicHistoryBo
        {
            get
            {
                if (topicHistoryBo == null)
                {
                    topicHistoryBo = new TopicHistoryBO(Uof);
                }
                return topicHistoryBo;
            }
        }

        /// <summary>
        /// Gets an instance of the topic business object
        /// </summary>
        internal TopicBO TopicBo
        {
            get
            {
                if (topicBo == null)
                {
                    topicBo = new TopicBO(Uof);
                }
                return topicBo;
            }
        }

        /// <summary>
        /// Gets an instance of the unit of work object
        /// </summary>
        internal UnitOfWork Uof
        {
            get
            {
                if (this._uof == null)
                {
                    _uof = new UnitOfWork();
                }
                return _uof;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Unload event of the Page control and gets rid of initiated objects on the
        /// page startup.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this._uof != null)
            {
                this._uof.Dispose();
                this._uof = null;
            }

            if (this.topicBo != null)
            {
                this.topicBo = null;
            }

            if (this.topicHistoryBo != null)
            {
                this.topicHistoryBo = null;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Include Css files
                AddStylePageHeader(CSS_WikiModuleCssId, CSS_WikiModuleCssPath);

                //congfigure the URL to the home page (the wiki without any parameters)
                homeURL = DotNetNuke.Common.Globals.NavigateURL();

                //Get the pageTopic
                if (this.Request.QueryString["topic"] == null)
                {
                    if (this.Request.QueryString["add"] == null & this.Request.QueryString["loc"] == null)
                    {
                        pageTopic = WikiHomeName;
                    }
                    else
                    {
                        pageTopic = string.Empty;
                    }
                }
                else
                {
                    pageTopic = WikiMarkup.DecodeTitle(this.Request.QueryString["topic"].ToString());
                }

                //Get the wikiSettings
                if (wikiSettings == null)
                {
                    SettingBO WikiController = new SettingBO(Uof);
                    wikiSettings = WikiController.GetByModuleID(ModuleId);
                    if (wikiSettings == null)
                    {
                        wikiSettings = new Setting();
                        wikiSettings.ContentEditorRoles = "UseDNNSettings";
                    }
                }

                //Get the edit rights
                if (wikiSettings.ContentEditorRoles.Equals("UseDNNSettings"))
                {
                    canEdit = this.IsEditable;
                }
                else
                {
                    if (Request.IsAuthenticated) //User is logged in
                    {
                        if (this.UserInfo.IsSuperUser)
                        {
                            canEdit = true;
                            isAdmin = true;
                        }
                        else if (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1)
                        {
                            canEdit = true;
                        }
                        else
                        {
                            string[] editorRoles = wikiSettings.ContentEditorRoles.Split(new char[] { '|' },
                                StringSplitOptions.RemoveEmptyEntries)[0].Split(new char[] { ';' },
                                StringSplitOptions.RemoveEmptyEntries);
                            foreach (string role in editorRoles)
                            {
                                if (UserInfo.IsInRole(role))
                                {
                                    canEdit = true;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                    }
                    else //User is NOT logged in
                    {
                        if ((wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) | (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleUnauthUserName + ";") > -1))
                        {
                            canEdit = true;
                        }
                    }
                }
                LoadTopic();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion Events

        #region Aux Functions

        /// <summary>
        /// Adds a page header of type css
        /// </summary>
        /// <param name="cssId">the id of the header tag</param>
        /// <param name="cssPath">the css path to the source file</param>
        private void AddStylePageHeader(string cssId, string cssPath)
        {
            HtmlGenericControl scriptInclude = (HtmlGenericControl)Page.Header.FindControl(cssId);
            if (scriptInclude == null)
            {
                scriptInclude = new HtmlGenericControl("link");
                scriptInclude.Attributes["rel"] = "stylesheet";
                scriptInclude.Attributes["type"] = "text/css";
                scriptInclude.Attributes["href"] = this.DNNWikiModuleRootPath + cssPath;
                scriptInclude.ID = cssId;

                Page.Header.Controls.Add(scriptInclude);
            }
        }

        protected void LoadTopic()
        {
            topic = TopicBo.GetByNameForModule(ModuleId, pageTopic);
            if (topic == null)
            {
                topic = new Topic();
                topic.TopicID = 0;
            }
            topic.TabID = TabId;
            topic.PortalSettings = PortalSettings;
            topicId = topic.TopicID;
        }

        protected string ReadTopic()
        {
            return HttpUtility.HtmlEncode(topic.Cache) ?? string.Empty;
        }

        protected string ReadTopicForEdit()
        {
            return topic.Content;
        }

        protected void SaveTopic(string Content, bool AllowDiscuss, bool AllowRating, string Title, string Description, string Keywords)
        {
            TopicHistory topicHistory = new TopicHistory();
            topicHistory.TabID = TabId;
            topicHistory.PortalSettings = PortalSettings;
            if (topic.TopicID != 0)
            {
                if ((!Content.Equals(topic.Content) | !Title.Equals(topic.Title) | !Description.Equals(topic.Description) | !Keywords.Equals(topic.Keywords)))
                {
                    topicHistory.Name = topic.Name;
                    topicHistory.TopicId = topic.TopicID;
                    topicHistory.Content = topic.Content;
                    topicHistory.UpdatedBy = topic.UpdatedBy;
                    topicHistory.UpdateDate = DateTime.Now;
                    topicHistory.UpdatedByUserID = topic.UpdatedByUserID;
                    topicHistory.Title = topic.Title;
                    topicHistory.Description = topic.Description;
                    topicHistory.Keywords = topic.Keywords;

                    topic.UpdateDate = DateTime.Now;
                    if ((UserInfo.UserID == -1))
                    {
                        topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile);
                    }
                    else
                    {
                        topic.UpdatedBy = UserInfo.Username;
                    }

                    topic.UpdatedByUserID = UserId;
                    topic.Content = Content;
                    topic.Title = Title;
                    topic.Description = Description;
                    topic.Keywords = Keywords;

                    TopicHistoryBo.Add(topicHistory);
                }
                topic.Name = pageTopic;
                topic.Title = Title;
                topic.Description = Description;
                topic.Keywords = Keywords;
                topic.AllowDiscussions = AllowDiscuss;
                topic.AllowRatings = AllowRating;
                topic.Content = Content;

                TopicBo.Update(topic);
            }
            else
            {
                topic = new Topic();
                topic.TabID = TabId;
                topic.PortalSettings = PortalSettings;
                topic.Content = Content;
                topic.Name = pageTopic;
                topic.ModuleId = ModuleId;
                if ((UserInfo.UserID == -1))
                {
                    topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile);
                }
                else
                {
                    topic.UpdatedBy = UserInfo.Username;
                }

                topic.UpdatedByUserID = UserId;
                topic.UpdateDate = DateTime.Now;
                topic.AllowDiscussions = AllowDiscuss;
                topic.AllowRatings = AllowRating;
                topic.Title = Title;
                topic.Description = Description;
                topic.Keywords = Keywords;

                topic = TopicBo.Add(topic);

                topicId = topic.TopicID;
            }
        }

        public IEnumerable<Topic> GetIndex()
        {
            return TopicBo.GetAllByModuleID(ModuleId);
        }

        protected IEnumerable<Topic> GetRecentlyChanged(int DaysBack)
        {
            return TopicBo.GetAllByModuleChangedWhen(ModuleId, DaysBack);
        }

        protected IEnumerable<TopicHistory> GetHistory()
        {
            return TopicHistoryBo.GetHistoryForTopic(topicId);
        }

        protected IEnumerable<Topic> Search(string SearchString)
        {
            return TopicBo.SearchWiki(SearchString, ModuleId);
        }

        protected string CreateTable(List<Topic> topicCollection)
        {
            System.Text.StringBuilder TableTxt = new System.Text.StringBuilder("<table><tr><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableTopic", RouterResourceFile));
            TableTxt.Append("</th><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableModBy", RouterResourceFile));
            TableTxt.Append("</th><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableModDate", RouterResourceFile));
            TableTxt.Append("</th></tr>");
            //Dim TopicTable As String
            Topic localTopic = new Topic();
            int i = 0;
            if (topicCollection.Count > 0)
            {
                for (i = 0; i <= topicCollection.Count - 1; i++)
                {
                    localTopic = (Topic)topicCollection[i];
                    localTopic.TabID = TabId;
                    localTopic.PortalSettings = PortalSettings;

                    string nameToUse = string.Empty;
                    if (!localTopic.Title.ToString().Equals(string.Empty))
                    {
                        nameToUse = localTopic.Title.Replace(WikiHomeName, "Home");
                    }
                    else
                    {
                        nameToUse = localTopic.Name.Replace(WikiHomeName, "Home");
                    }

                    TableTxt.Append("<tr>");
                    TableTxt.Append("<td><a class=\"CommandButton\" href=\"");
                    TableTxt.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(localTopic.Name)));
                    TableTxt.Append("\">");
                    TableTxt.Append(nameToUse);
                    TableTxt.Append("</a></td>");
                    TableTxt.Append("<td class=\"Normal\">");
                    TableTxt.Append(localTopic.UpdatedByUsername);
                    TableTxt.Append("</td>");
                    TableTxt.Append("<td class=\"Normal\">");
                    TableTxt.Append(localTopic.UpdateDate.ToString(CultureInfo.CurrentCulture));
                    TableTxt.Append("</td>");
                    TableTxt.Append("</tr>");
                }
            }
            else
            {
                TableTxt.Append("<tr><td colspan=3 class=\"Normal\">");
                TableTxt.Append(Localization.GetString("BaseCreateTableNoResults", RouterResourceFile));
                TableTxt.Append("</td></tr>");
            }
            TableTxt.Append("</table>");
            return TableTxt.ToString();
        }

        protected string CreateRecentChangeTable(int DaysBack)
        {
            return CreateTable(GetRecentlyChanged(DaysBack).ToList());
        }

        protected string CreateSearchTable(string SearchString)
        {
            return CreateTable(Search(SearchString).ToList());
        }

        protected string CreateHistoryTable()
        {
            System.Text.StringBuilder TableTxt = new System.Text.StringBuilder(1000);
            TableTxt.Append("<table><tr><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableTopic", RouterResourceFile));
            TableTxt.Append("</th><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableTitle", RouterResourceFile));
            TableTxt.Append("</th><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableModBy", RouterResourceFile));
            TableTxt.Append("</th><th>");
            TableTxt.Append(Localization.GetString("BaseCreateTableModDate", RouterResourceFile));
            TableTxt.Append("</th></tr>");
            var topicHistoryCollection = GetHistory().ToArray();
            //Dim TopicTable As StringBuilder = New StringBuilder(500)
            TopicHistory history = default(TopicHistory);
            int i = 0;
            if (topicHistoryCollection.Any())
            {
                i = topicHistoryCollection.Count();
                while ((i > 0))
                {
                    history = (TopicHistory)topicHistoryCollection[i - 1];
                    history.TabID = TabId;
                    history.PortalSettings = PortalSettings;
                    TableTxt.Append("<tr><td><a class=\"CommandButton\" rel=\"noindex,nofollow\" href=\"");
                    TableTxt.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(pageTopic), "loc=TopicHistory", "ShowHistory=" + history.TopicHistoryId.ToString()));
                    TableTxt.Append("\">");
                    TableTxt.Append(history.Name.Replace(WikiHomeName, "Home"));

                    TableTxt.Append("</a></td>");
                    TableTxt.Append("<td class=\"Normal\">");
                    if (!history.Title.ToString().Equals(string.Empty))
                    {
                        TableTxt.Append(history.Title.Replace(WikiHomeName, "Home"));
                    }
                    else
                    {
                        TableTxt.Append(history.Name.Replace(WikiHomeName, "Home"));
                    }
                    TableTxt.Append("</td>");
                    TableTxt.Append("<td class=\"Normal\">");
                    TableTxt.Append(history.UpdatedByUsername);
                    TableTxt.Append("</td>");
                    TableTxt.Append("<td Class=\"Normal\">");
                    TableTxt.Append(history.UpdateDate.ToString(CultureInfo.CurrentCulture));
                    TableTxt.Append("</td>");
                    TableTxt.Append("</tr>");
                    i = i - 1;
                }
            }
            else
            {
                TableTxt.Append("<tr><td colspan=\"3\" class=\"Normal\">");
                TableTxt.Append(Localization.GetString("BaseCreateHistoryTableEmpty", RouterResourceFile));
                TableTxt.Append("</td></tr>");
            }
            TableTxt.Append("</table>");
            return TableTxt.ToString();
        }

        #endregion Aux Functions
    }
}