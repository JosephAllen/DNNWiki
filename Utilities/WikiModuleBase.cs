using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Globalization;

namespace DotNetNuke.Modules.Wiki.Utilities
{
    public class WikiModuleBase : PortalModuleBase
	{
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

		public WikiControlBase myPage;

		public Entities.TopicController TC {
			get {
				if (topicBo == null) {
					topicBo = new Entities.TopicController(this.Cache);
				}
				return topicBo;
			}
		}

		protected void Page_Load(System.Object sender, System.EventArgs e)
		{
			//congfigure the URL to the home page (the wiki without any parameters)
			HomeURL = DotNetNuke.Common.NavigateURL();

			if (this.Request.QueryString.Item("topic") == null) {
				if (this.Request.QueryString.Item("add") == null & this.Request.QueryString.Item("loc") == null) {
					PageTopic = WikiHomeName;
				} else {
					PageTopic = "";
				}
			} else {
				PageTopic = Entities.WikiData.DecodeTitle(this.Request.QueryString.Item("topic").ToString());
			}

			if (wikiSettings == null) {
				Entities.SettingsController WikiController = new Entities.SettingsController();
				wikiSettings = WikiController.GetByModuleID(ModuleId);
				if (wikiSettings == null) {
					wikiSettings = new Entities.SettingsInfo();
					wikiSettings.ContentEditorRoles = "UseDNNSettings";
				}
			}

			if (wikiSettings.ContentEditorRoles == "UseDNNSettings") {
				CanEdit = this.IsEditable;
			} else {
				if (Request.IsAuthenticated) {
					if (this.UserInfo.IsSuperUser) {
						CanEdit = true;
						IsAdmin = true;
					} else if (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) {
						CanEdit = true;
					} else {
						string[] Roles = wikiSettings.ContentEditorRoles.Split("|")(0).Split(";");
						foreach (string role in Roles) {
							if (UserInfo.IsInRole(role)) {
								CanEdit = true;
								break; // TODO: might not be correct. Was : Exit For
							}
						}
					}
				} else {
					if ((wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) | (wikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleUnauthUserName + ";") > -1)) {
						CanEdit = true;
					}
				}
			}
			LoadTopic();
		}

		protected void LoadTopic()
		{
			topic = TC.GetByNameForModule(ModuleID, PageTopic);
			if (topic == null) {
				topic = new Entities.TopicInfo();
				topic.TopicID = 0;
			}
			topic.TabID = tabID;
			topic.PortalSettings = portalSettings;
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
			Entities.TopicHistoryInfo th = new Entities.TopicHistoryInfo();
			th.TabID = tabID;
			th.PortalSettings = portalSettings;
			if (topic.TopicID != 0) {
				if ((!Content.Equals(topic.Content) | !Title.Equals(topic.Title) | !Description.Equals(topic.Description) | !Keywords.Equals(topic.Keywords))) {
					th.Name = topic.Name;
					th.TopicID = topic.TopicID;
					th.Content = topic.Content;
					th.UpdatedBy = topic.UpdatedBy;
					th.UpdateDate = DateTime.Now;
					th.UpdatedByUserID = topic.UpdatedByUserID;
					th.Title = topic.Title;
					th.Description = topic.Description;
					th.Keywords = topic.Keywords;

					topic.UpdateDate = DateTime.Now;
					if ((UserInfo.UserID == -1)) {
						topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile);
					} else {
						topic.UpdatedBy = UserInfo.Username;
					}

					topic.UpdatedByUserID = UserId;
					topic.Content = Content;
					topic.Title = Title;
					topic.Description = Description;
					topic.Keywords = Keywords;

					topicHistoryBo.Add(th);
				}
				topic.Name = PageTopic;
				topic.Title = Title;
				topic.Description = Description;
				topic.Keywords = Keywords;
				topic.AllowDiscussions = AllowDiscuss;
				topic.AllowRatings = AllowRating;
				topic.Content = Content;

				TC.Update(topic);
			} else {
				topic = new Entities.TopicInfo();
				topic.TabID = tabID;
				topic.PortalSettings = portalSettings;
				topic.Content = Content;
				topic.Name = PageTopic;
				topic.ModuleID = ModuleID;
				if ((UserInfo.UserID == -1)) {
					topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile);
				} else {
					topic.UpdatedBy = UserInfo.Username;
				}

				topic.UpdatedByUserID = UserID;
				topic.UpdateDate = DateTime.Now;
				topic.AllowDiscussions = AllowDiscuss;
				topic.AllowRatings = AllowRating;
				topic.Title = Title;
				topic.Description = Description;
				topic.Keywords = Keywords;

				topic.TopicID = TC.Add(topic);

				TopicID = topic.TopicID;
			}
		}

		public ArrayList GetIndex()
		{
			return TC.GetAllByModuleID(ModuleID);
		}

		protected object GetRecentlyChanged(int DaysBack)
		{
			return TC.GetAllByModuleChangedWhen(ModuleID, DaysBack);
		}

		protected ArrayList GetHistory()
		{
			return topicHistoryBo.GetHistoryForTopic(TopicID);
		}

		protected ArrayList Search(string SearchString)
		{
			return TC.SearchWiki(SearchString, ModuleID);
		}

		//Protected Function ReadTopicHistory(ByVal TopicHistoryID As Integer) As String
		//    Dim th As Entities.TopicHistoryInfo
		//    th = THC.GetItem(TopicHistoryID)
		//    If Not th Is Nothing Then
		//        Return th.Content
		//    Else
		//        Return ""
		//    End If
		//End Function

		protected string CreateTable(ref ArrayList ts)
		{
			System.Text.StringBuilder TableTxt = new System.Text.StringBuilder("<table><tr><th>");
			TableTxt.Append(Localization.GetString("BaseCreateTableTopic", RouterResourceFile));
			TableTxt.Append("</th><th>");
			TableTxt.Append(Localization.GetString("BaseCreateTableModBy", RouterResourceFile));
			TableTxt.Append("</th><th>");
			TableTxt.Append(Localization.GetString("BaseCreateTableModDate", RouterResourceFile));
			TableTxt.Append("</th></tr>");
			//Dim TopicTable As String
			Entities.TopicInfo t = default(Entities.TopicInfo);
			int i = 0;
			if (ts.Count > 0) {
				for (i = 0; i <= ts.Count - 1; i++) {
					t = (Entities.TopicInfo)ts[i];
					t.TabID = tabID;
					t.PortalSettings = portalSettings;

					string nameToUse = string.Empty;
					if (t.Title.ToString != string.Empty) {
						nameToUse = t.Title.Replace(this.WikiHomeName, "Home");
					} else {
						nameToUse = t.Name.Replace(this.WikiHomeName, "Home");
					}

					TableTxt.Append("<tr>");
					TableTxt.Append("<td><a class=\"CommandButton\" href=\"");
					TableTxt.Append(DotNetNuke.Common.NavigateURL(this.tabID, this.portalSettings, string.Empty, "topic=" + Entities.WikiData.EncodeTitle(t.Name)));
					TableTxt.Append("\">");
					TableTxt.Append(nameToUse);
					TableTxt.Append("</a></td>");
					TableTxt.Append("<td class=\"Normal\">");
					TableTxt.Append(t.UpdatedByUsername);
					TableTxt.Append("</td>");
					TableTxt.Append("<td class=\"Normal\">");
					TableTxt.Append(t.UpdateDate.ToString(CultureInfo.CurrentCulture));
					TableTxt.Append("</td>");
					TableTxt.Append("</tr>");
				}
			} else {
				TableTxt.Append("<tr><td colspan=3 class=\"Normal\">");
				TableTxt.Append(Localization.GetString("BaseCreateTableNoResults", RouterResourceFile));
				TableTxt.Append("</td></tr>");
			}
			TableTxt.Append("</table>");
			return TableTxt.ToString();
		}

		protected string CreateRecentChangeTable(int DaysBack)
		{
			return CreateTable(ref GetRecentlyChanged(DaysBack));
		}

		protected string CreateSearchTable(string SearchString)
		{
			return CreateTable(ref Search(SearchString));
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
			ArrayList th = GetHistory();
			//Dim TopicTable As StringBuilder = New StringBuilder(500)
			Entities.TopicHistoryInfo history = default(Entities.TopicHistoryInfo);
			int i = 0;
			if (th.Count > 0) {
				i = th.Count;
				while ((i > 0)) {
					history = (Entities.TopicHistoryInfo)th[i - 1];
					history.TabID = tabID;
					history.PortalSettings = portalSettings;
					TableTxt.Append("<tr><td><a class=\"CommandButton\" rel=\"noindex,nofollow\" href=\"");
					TableTxt.Append(DotNetNuke.Common.NavigateURL(this.tabID, this.portalSettings, string.Empty, "topic=" + Entities.WikiData.EncodeTitle(PageTopic), "loc=TopicHistory", "ShowHistory=" + history.TopicHistoryID.ToString()));
					TableTxt.Append("\">");
					TableTxt.Append(history.Name.Replace(this.WikiHomeName, "Home"));

					TableTxt.Append("</a></td>");
					TableTxt.Append("<td class=\"Normal\">");
					if (history.Title.ToString != string.Empty) {
						TableTxt.Append(history.Title.Replace(this.WikiHomeName, "Home"));
					} else {
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
			} else {
				TableTxt.Append("<tr><td colspan=\"3\" class=\"Normal\">");
				TableTxt.Append(Localization.GetString("BaseCreateHistoryTableEmpty", RouterResourceFile));
				TableTxt.Append("</td></tr>");
			}
			TableTxt.Append("</table>");
			return TableTxt.ToString();
		}

		public string RouterResourceFile {
			get { return DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "Router.ascx.resx"); }
			set { value = value; }
		}

		public WikiControlBase()
		{
			Load += Page_Load;
		}
	}
}