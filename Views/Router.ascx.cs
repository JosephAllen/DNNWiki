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
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Web.UI;

namespace DotNetNuke.Wiki.Views
{
    partial class Router : WikiModuleBase, IActionable
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        public Router()
        {
            Load += Router_Page_Load;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Gets the module actions.
        /// </summary>
        /// <value>The module actions.</value>
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                Actions.Add(GetNextActionID(),
                    Localization.GetString("Administration", LocalResourceFile).ToString(), string.Empty, string.Empty, string.Empty, EditUrl("Administration"), false, Security.SecurityAccessLevel.Admin, true, false);
                return Actions;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Load event of the Router_Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Router_Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //load the menu on the left
                string leftControl = "SharedControls//WikiMenu.ascx";
                WikiModuleBase _wikiModuleBase = (WikiModuleBase)TemplateControl.LoadControl(leftControl);
                _wikiModuleBase.ModuleConfiguration = ModuleConfiguration;
                _wikiModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(leftControl);
                phWikiMenu.Controls.Add(_wikiModuleBase);

                string controlToLoad = GetControlString(Request.QueryString["loc"]);
                WikiModuleBase wikiContent = (WikiModuleBase)LoadControl(controlToLoad);
                wikiContent.ModuleConfiguration = ModuleConfiguration;
                wikiContent.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phWikiContent.Controls.Add(wikiContent);

                if ((controlToLoad.ToLower().Equals("start.ascx")))
                {
                    string buttonControlToLoad = "SharedControls//WikiButton.ascx";
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

            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Gets the control string.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns></returns>
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
                        return "SharedControls//index.ascx";

                    default:
                        return "start.ascx";
                }
            }
        }

        #endregion Methods
    }
}