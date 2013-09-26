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

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Linq;

namespace DotNetNuke.Wiki.Views.SharedControls
{
    public partial class Index : WikiModuleBase
    {
        #region Ctor

        public Index()
        {
            Load += Page_Load;
        }

        #endregion Ctor

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            DisplayIndex();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Displays the index.
        /// </summary>
        private void DisplayIndex()
        {
            this.Session["wiki" + ModuleId.ToString() + "ShowIndex"] = true;
            var topics = GetIndex().ToArray();
            System.Text.StringBuilder TableTxt = new System.Text.StringBuilder();
            //Dim TopicTable As String
            Topic t = default(Topic);
            int i = 0;
            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
            TableTxt.Append(HomeURL + "\"><img src=\"");
            TableTxt.Append(DNNWikiModuleRootPath);
            TableTxt.Append("/Resources/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;");
            TableTxt.Append(Localization.GetString("Home", this.LocalResourceFile));
            TableTxt.Append("</a><br />");
            if ((topics != null))
            {
                if (topics.Count() > 0)
                {
                    for (i = 0; i <= topics.Count() - 1; i++)
                    {
                        t = (Topic)topics[i];
                        if (t.Name != WikiHomeName)
                        {
                            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
                            TableTxt.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(t.Name)));
                            TableTxt.Append("\"><img src=\"");
                            TableTxt.Append(DNNWikiModuleRootPath);
                            TableTxt.Append("/Resources/images/Page.gif\" border=\"0\" align=\"middle\"  alt=\"" + WikiMarkup.EncodeTitle(t.Name) + "\" />&nbsp;");
                            TableTxt.Append(t.Name);
                            TableTxt.Append("</a><br />");
                        }
                    }
                }
            }
            TableTxt.Append(string.Empty);
            this.IndexList.Text = TableTxt.ToString();
            this.IndexList.Visible = true;
        }

        #endregion Methods
    }
}