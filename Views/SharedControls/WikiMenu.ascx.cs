#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikiMenu.ascx.cs" company="DNN Corp®">
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
using DotNetNuke.Wiki.Utilities;
using System;

namespace DotNetNuke.Wiki.Views.SharedControls
{
    public partial class WikiMenu : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiMenu"/> class.
        /// </summary>
        public WikiMenu()
        {
            Load += Menu_Page_Load;
        }

        #endregion Ctor

        #region Variables

        private bool m_ShowNav;
        private bool m_ShowIndex;

        #endregion Variables

        #region Events

        private void Menu_Page_Load(System.Object sender, System.EventArgs e)
        {
            //todo:we shouldn't use session

            if ((Session["wiki" + ModuleId.ToString() + "ShowIndex"] == null))
            {
                this.Session.Add("wiki" + ModuleId.ToString() + "ShowIndex", false);
                m_ShowIndex = false;
            }
            else
            {
                m_ShowIndex = Convert.ToBoolean(this.Session["wiki" + ModuleId.ToString() + "ShowIndex"]);
            }
            if ((this.Session["wiki" + ModuleId.ToString() + "ShowNav"] == null))
            {
                this.Session.Add("wiki" + ModuleId.ToString() + "ShowNav", true);
                m_ShowNav = true;
            }
            else
            {
                m_ShowNav = Convert.ToBoolean(this.Session["wiki" + ModuleId.ToString() + "ShowNav"]);
            }

            if (m_ShowNav)
            {
                //Me.ImageButton1.AlternateText = Localization.GetString("HideNavigation", LocalResourceFile) ' "Show Navigation"
                //Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/HideNav.gif"
                this.LinksPanel.Visible = true;
            }
            else
            {
                //Me.ImageButton1.AlternateText = Localization.GetString("ShowNavigation", LocalResourceFile) ' "Show Navigation"
                //Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/ShowNav.gif"
                this.LinksPanel.Visible = false;
            }

            setURLs();
        }

        #endregion Events

        #region Methods

        private void setURLs()
        {
            this.HomeBtn.NavigateUrl = Common.Globals.NavigateURL();
            this.HomeBtn.Text = "<img src=\"" + DNNWikiModuleRootPath + "/Resources/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Home", this.LocalResourceFile);
            this.SearchBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, "", "loc=search");
            this.SearchBtn.Text = "<img src=\"" + DNNWikiModuleRootPath + "/Resources/images/Search.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Search", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Search", this.LocalResourceFile);
            this.RecChangeBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, "", "loc=recentchanges");
            this.RecChangeBtn.Text = "<img src=\"" + DNNWikiModuleRootPath + "/Resources/images/RecentChanges.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("RecentChanges", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("RecentChanges", this.LocalResourceFile);

            this.IndexBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, "", "loc=index");

            this.IndexBtn.Text = "<img src=\"" + DNNWikiModuleRootPath + "/Resources/images/Index.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Index", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Index", this.LocalResourceFile);
        }

        #endregion Methods
    }
}