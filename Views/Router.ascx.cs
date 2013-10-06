#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Router.ascx.cs" company="DNN Corp®">
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
////--------------------------------------------------------------------------------------------------------

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
    /// <summary>
    /// Router Class based on the WikiModuleBase Class
    /// </summary>
    internal partial class Router : WikiModuleBase, IActionable
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        public Router()
        {
            this.Load += this.Router_Page_Load;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the module actions.
        /// </summary>
        /// <value>The module actions.</value>
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                actions.Add(
                    this.GetNextActionID(),
                    Localization.GetString("Administration", LocalResourceFile).ToString(),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    this.EditUrl("Administration"),
                    false,
                    Security.SecurityAccessLevel.Admin,
                    true,
                    false);
                return actions;
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
        private void Router_Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Load the menu on the left
                string leftControl = "SharedControls//WikiMenu.ascx";
                WikiModuleBase wikiModuleBase = (WikiModuleBase)TemplateControl.LoadControl(leftControl);
                wikiModuleBase.ModuleConfiguration = this.ModuleConfiguration;
                wikiModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(leftControl);
                phWikiMenu.Controls.Add(wikiModuleBase);

                string controlToLoad = this.GetControlString(Request.QueryString["loc"]);
                WikiModuleBase wikiContent = (WikiModuleBase)LoadControl(controlToLoad);
                wikiContent.ModuleConfiguration = this.ModuleConfiguration;
                wikiContent.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phWikiContent.Controls.Add(wikiContent);

                if (controlToLoad.ToLower().Equals("start.ascx"))
                {
                    string buttonControlToLoad = "SharedControls//WikiButton.ascx";
                    WikiModuleBase wikiButton = (WikiModuleBase)LoadControl(buttonControlToLoad);

                    wikiButton.ModuleConfiguration = this.ModuleConfiguration;
                    wikiButton.ID = System.IO.Path.GetFileNameWithoutExtension(buttonControlToLoad);
                    phWikiContent.Controls.Add(wikiButton);
                }

                // Print the Topic
                foreach (ModuleAction objAction in this.Actions)
                {
                    if (objAction.CommandName.Equals(ModuleActionType.PrintModule))
                    {
                        objAction.Url += "&topic=" + WikiMarkup.EncodeTitle(this.PageTopic);
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
        /// <param name="locationString">The location.</param>
        /// <returns>Control Name</returns>
        private string GetControlString(string locationString)
        {
            if (locationString == null)
            {
                return "Start.ascx";
            }
            else
            {
                switch (locationString.ToLower())
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