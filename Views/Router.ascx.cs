//
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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Wiki.Utilities;
using DotNetNuke.Services.Localization;
using System.Web.UI;

namespace DotNetNuke.Wiki.Views
{
    //Implements ISearchable
    //Implements IPortable
    partial class Router : WikiModuleBase, IActionable
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

        private void Router_Page_Load(System.Object sender, System.EventArgs e)
        {
            //load the menu on the left
            string leftControl = "Controls/WikiMenu.ascx";
            WikiModuleBase mbl = (WikiModuleBase)TemplateControl.LoadControl(leftControl);
            mbl.ModuleConfiguration = ModuleConfiguration;
            mbl.ID = System.IO.Path.GetFileNameWithoutExtension(leftControl);
            phWikiMenu.Controls.Add(mbl);

            string controlToLoad = GetControlString(Request.QueryString["loc"]);
            WikiModuleBase wikiContent = (WikiModuleBase)LoadControl(controlToLoad);
            wikiContent.ModuleConfiguration = ModuleConfiguration;
            wikiContent.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
            phWikiContent.Controls.Add(wikiContent);

            if ((controlToLoad.ToLower().Equals("start.ascx")))
            {
                string buttonControlToLoad = "Controls/WikiButton.ascx";
                WikiModuleBase wikiButton = (WikiModuleBase)LoadControl(buttonControlToLoad);

                wikiButton.ModuleConfiguration = ModuleConfiguration;
                wikiButton.ID = System.IO.Path.GetFileNameWithoutExtension(buttonControlToLoad);
                phWikiContent.Controls.Add(wikiButton);
            }

            //print
            foreach (ModuleAction objAction in Actions)
            {
                if (objAction.CommandName.Equals(ModuleActionType.PrintModule))
                {
                    objAction.Url += "&topic=" + WikiMarkup.EncodeTitle(PageTopic);
                }
            }
        }

        private string GetControlString(string loc)
        {
            if (loc == null)
            {
                return "Start.ascx";
            }
            else
            {
                switch ((loc.ToLower()))
                {
                    case "start":
                        return "Start.ascx";

                    case "edit":
                        return "Edit.ascx";

                    case "topichistory":
                        return "topichistory.ascx";

                    case "search":
                        return "search.ascx";

                    case "recentchanges":
                        return "recentchanges.ascx";

                    case "index":
                        return "controls/index.ascx";

                    default:
                        return "start.ascx";
                }
            }
        }

        //Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        //    If ShowNav Then

        // Me.ImageButton1.AlternateText = Localization.GetString("ShowNavigation",
        // LocalResourceFile) ' "Show Navigation" Me.ImageButton1.ImageUrl = TemplateSourceDirectory
        // + "/images/ShowNav.gif" Me.LinksPanel.Visible = False ShowNav = False Me.Session("wiki" +
        // ModuleId.ToString() + "ShowNav") = False Else Me.ImageButton1.AlternateText =
        // Localization.GetString("HideNavigation", LocalResourceFile) '"Hide Navigation"
        // Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/HideNav.gif"
        // Me.LinksPanel.Visible = True ShowNav = False Me.Session("wiki" + ModuleId.ToString() +
        // "ShowNav") = True End If
        //End Sub

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                Actions.Add(GetNextActionID(),
                    Localization.GetString("Administration", LocalResourceFile).ToString(), "", "", "", EditUrl("Administration"), false, Security.SecurityAccessLevel.Admin, true, false);
                return Actions;
            }
        }

        public Router()
        {
            Load += Router_Page_Load;
            Init += Page_Init;
        }
    }
}