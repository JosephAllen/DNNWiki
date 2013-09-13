using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Wiki.BusinessObjects;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.Wiki.Utilities
{
    public class WikiModuleBase : PortalModuleBase
    {
        #region Variables

        public string UserName;
        public string FirstName;

        public string LastName;
        public bool IsAdmin = false;
        public string PageTopic;
        public string PageTitle;

        public int TopicID;
        public Topic topic;

        public string HomeURL;

        public Setting wikiSettings;
        public bool CanEdit = false;
        public readonly string WikiHomeName = "WikiHomePage";

        private DotNetNuke.Entities.Modules.PortalModuleBase mModule;

        private UnitOfWork uof;
        private TopicBO topicBo;
        private TopicHistoryBO topicHistoryBo;

        #endregion Variables

        #region Ctor

        public WikiModuleBase()
        {
            Load += Page_Load;
            Unload += Page_Unload;
        }

        #endregion Ctor

        #region Properties

        public string RouterResourceFile
        {
            get { return DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "Router.ascx.resx"); }
            set { value = value; }
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
                    topicHistoryBo = new TopicHistoryBO(uof);
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
                    topicBo = new TopicBO(uof);
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
                if (this.uof == null)
                {
                    uof = new UnitOfWork();
                }
                return uof;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Get ride of initiated objects on the page startup
        /// </summary>
        /// <param name="e"></param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            base.OnUnload(e);
            if (this.uof != null)
            {
                this.uof.Dispose();
                this.uof = null;
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

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            //congfigure the URL to the home page (the wiki without any parameters)
            HomeURL = DotNetNuke.Common.Globals.NavigateURL();

            if (this.Request.QueryString["topic"] == null)
            {
                if (this.Request.QueryString["add"] == null & this.Request.QueryString["loc"] == null)
                {
                    PageTopic = WikiHomeName;
                }
                else
                {
                    PageTopic = "";
                }
            }
            else
            {
                PageTopic = WikiMarkup.DecodeTitle(this.Request.QueryString["topic"].ToString());
            }

            if (wikiSettings == null)
            {
                SettingBO WikiController = new SettingBO(uof);
                wikiSettings = WikiController.GetByModuleID(ModuleId);
                if (wikiSettings == null)
                {
                    wikiSettings = new Setting();
                    wikiSettings.ContentEditorRoles = "UseDNNSettings";
                }
            }

            if (wikiSettings.ContentEditorRoles == "UseDNNSettings")
            {
                CanEdit = this.IsEditable;
            }
            else
            {
                if (Request.IsAuthenticated)
                {
                    if (this.UserInfo.IsSuperUser)
                    {
                        CanEdit = true;
                        IsAdmin = true;
                    }
                    else if (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1)
                    {
                        CanEdit = true;
                    }
                    else
                    {
                        string[] Roles = wikiSettings.ContentEditorRoles.Split(new char[] { '|' },
                            StringSplitOptions.RemoveEmptyEntries)[0].Split(new char[] { ';' },
                            StringSplitOptions.RemoveEmptyEntries);
                        foreach (string role in Roles)
                        {
                            if (UserInfo.IsInRole(role))
                            {
                                CanEdit = true;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                }
                else
                {
                    if ((wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) | (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleUnauthUserName + ";") > -1))
                    {
                        CanEdit = true;
                    }
                }
            }
            LoadTopic();
        }

        #endregion Events

        #region Aux Functions

        protected void LoadTopic()
        {
            topic = TopicBo.GetByNameForModule(ModuleId, PageTopic);
            if (topic == null)
            {
                topic = new Topic();
                topic.TopicID = 0;
            }
            topic.TabID = TabId;
            topic.PortalSettings = PortalSettings;
            TopicID = topic.TopicID;
        }

        protected string ReadTopic()
        {
            return HttpUtility.HtmlEncode(topic.Cache);
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

                    topicHistoryBo.Add(topicHistory);
                }
                topic.Name = PageTopic;
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
                topic.Name = PageTopic;
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

                TopicID = topic.TopicID;
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
            return topicHistoryBo.GetHistoryForTopic(TopicID);
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
                        nameToUse = localTopic.Title.Replace(this.WikiHomeName, "Home");
                    }
                    else
                    {
                        nameToUse = localTopic.Name.Replace(this.WikiHomeName, "Home");
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
                    TableTxt.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(PageTopic), "loc=TopicHistory", "ShowHistory=" + history.TopicHistoryId.ToString()));
                    TableTxt.Append("\">");
                    TableTxt.Append(history.Name.Replace(this.WikiHomeName, "Home"));

                    TableTxt.Append("</a></td>");
                    TableTxt.Append("<td class=\"Normal\">");
                    if (!history.Title.ToString().Equals(string.Empty))
                    {
                        TableTxt.Append(history.Title.Replace(this.WikiHomeName, "Home"));
                    }
                    else
                    {
                        TableTxt.Append(history.Name.Replace(this.WikiHomeName, "Home"));
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