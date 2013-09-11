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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Modules.Wiki.Entities;


namespace DotNetNuke.Modules.Wiki.Views.SharedControls
{

    public partial class Index : WikiControlBase
    {



        protected void Page_Load(object sender, System.EventArgs e)
        {
            DisplayIndex();

        }

        private void DisplayIndex()
        {
            this.Session("wiki" + ModuleId.ToString + "ShowIndex") = true;
            ArrayList ts = GetIndex();
            System.Text.StringBuilder TableTxt = new System.Text.StringBuilder("");
            //Dim TopicTable As String
            Entities.TopicInfo t = default(Entities.TopicInfo);
            int i = 0;
            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
            TableTxt.Append(HomeURL + "\"><img src=\"");
            TableTxt.Append(Parent.TemplateSourceDirectory);
            TableTxt.Append("/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;");
            TableTxt.Append(Localization.GetString("Home", this.LocalResourceFile));
            TableTxt.Append("</a><br />");
            if ((ts != null))
            {
                if (ts.Count > 0)
                {
                    for (i = 0; i <= ts.Count - 1; i++)
                    {
                        t = (Entities.TopicInfo)ts(i);
                        if (t.Name != WikiHomeName)
                        {
                            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
                            TableTxt.Append(DotNetNuke.Common.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + Entities.WikiData.EncodeTitle(t.Name)));
                            TableTxt.Append("\"><img src=\"");
                            TableTxt.Append(Parent.TemplateSourceDirectory);
                            TableTxt.Append("/images/Page.gif\" border=\"0\" align=\"middle\"  alt=\"" + Entities.WikiData.EncodeTitle(t.Name) + "\" />&nbsp;");
                            TableTxt.Append(t.Name);
                            TableTxt.Append("</a><br />");
                        }
                    }
                }
            }
            TableTxt.Append("");
            this.IndexList.Text = TableTxt.ToString();
            this.IndexList.Visible = true;
        }
        public Index()
        {
            Load += Page_Load;
        }

    }


}