﻿//
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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;

using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNuke.Modules.Wiki.Views.SharedControls
{
    public partial class WikiMenu : WikiModuleBase
    {
        private bool ShowIndex;

        private bool ShowNav;

        private void Menu_Page_Load(System.Object sender, System.EventArgs e)
        {
            //todo:we shouldn't use session

            if ((this.Session("wiki" + ModuleId.ToString + "ShowIndex") == null))
            {
                this.Session.Add("wiki" + ModuleId.ToString + "ShowIndex", false);
                ShowIndex = false;
            }
            else
            {
                ShowIndex = Convert.ToBoolean(this.Session("wiki" + ModuleId.ToString + "ShowIndex"));
            }
            if ((this.Session("wiki" + ModuleId.ToString + "ShowNav") == null))
            {
                this.Session.Add("wiki" + ModuleId.ToString + "ShowNav", true);
                ShowNav = true;
            }
            else
            {
                ShowNav = Convert.ToBoolean(this.Session("wiki" + ModuleId.ToString + "ShowNav"));
            }

            if (ShowNav)
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

        private void setURLs()
        {
            this.HomeBtn.NavigateUrl = Common.Globals.NavigateURL();
            this.HomeBtn.Text = "<img src=\"" + Parent.TemplateSourceDirectory + "/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Home", this.LocalResourceFile);
            this.SearchBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(this.TabId, this.PortalSettings, "", "loc=search");
            this.SearchBtn.Text = "<img src=\"" + Parent.TemplateSourceDirectory + "/images/Search.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Search", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Search", this.LocalResourceFile);
            this.RecChangeBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(this.TabId, this.PortalSettings, "", "loc=recentchanges");
            this.RecChangeBtn.Text = "<img src=\"" + Parent.TemplateSourceDirectory + "/images/RecentChanges.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("RecentChanges", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("RecentChanges", this.LocalResourceFile);

            this.IndexBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(this.TabId, this.PortalSettings, "", "loc=index");

            this.IndexBtn.Text = "<img src=\"" + Parent.TemplateSourceDirectory + "/images/Index.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Index", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Index", this.LocalResourceFile);
        }

        public WikiMenu()
        {
            Load += Menu_Page_Load;
        }
    }
}