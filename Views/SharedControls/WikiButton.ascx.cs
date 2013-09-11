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
using DotNetNuke.Modules.Wiki.Entities;


namespace DotNetNuke.Modules.Wiki.Views.SharedControls
{


    partial class WikiButton : WikiControlBase
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            LocalResourceFile = DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "WikiButton.ascx.resx");

            lnkEdit.Text = Localization.GetString("StartEdit.Text", LocalResourceFile);
            cmdAdd.Text = Localization.GetString("StartAdd", LocalResourceFile);
            txtViewHistory.Text = Localization.GetString("StartViewHistory", LocalResourceFile);

            SetDisplay();
        }
        
        private void SetDisplay()
        {
            this.ViewPipe1.Visible = false;
            this.ViewPipe2.Visible = false;

            this.EditPipe.Visible = false;

            this.cmdAdd.Visible = this.CanEdit;
            this.lnkEdit.Visible = false;

            this.AddPipe.Visible = false;
            this.txtViewHistory.Visible = false;


            if ((Topic.TopicID >= 0 | (Request.QueryString.Item("topic") != null)))
            {
                this.ViewPipe1.Visible = true;
                this.ViewPipe2.Visible = true;

                this.EditPipe.Visible = this.CanEdit;

                this.cmdAdd.Visible = this.CanEdit;
                this.lnkEdit.Visible = this.CanEdit;

                this.AddPipe.Visible = this.CanEdit;
                this.txtViewHistory.Visible = true;
                txtViewHistory.NavigateUrl = DotNetNuke.Common.NavigateURL(this.TabId, this.PortalSettings, "", "loc=TopicHistory", "topic=" + Entities.WikiData.EncodeTitle(PageTopic));
                lnkEdit.NavigateUrl = NavigateURL(TabId, "", "topic=" + WikiData.EncodeTitle(PageTopic) + "&loc=edit");
            }

            cmdAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "&loc=edit&add=true");

        }
        public WikiButton()
        {
            Load += Page_Load;
        }
    }
}