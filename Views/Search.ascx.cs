﻿#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Search.ascx.cs" company="DNN Corp®">
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

// description: Allows users to search for topics. Needs to be rewriten but for now this is OK.

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;
using System.Linq;

namespace DotNetNuke.Wiki.Views
{
    partial class Search : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class.
        /// </summary>
        public Search()
        {
            Load += Page_Load;
        }

        #endregion Ctor

        #region Variables

        protected System.Web.UI.WebControls.Label lblPageContent;

        #endregion Variables

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        /// <summary>
        /// Handles the Click event of the cmdSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdSearch_Click(System.Object sender, System.EventArgs e)
        {
            this.SearchTopics();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            Label2.Text = Localization.GetString("SearchTitleBasic", RouterResourceFile);
            Label2.Text = Localization.GetString("SearchFieldLabel", RouterResourceFile);
            cmdSearch.Text = Localization.GetString("SearchExec", RouterResourceFile);
        }

        private void SearchTopics()
        {
            HitTable.Text = CreateTable(Search("%" + this.txtTextToSearch.Text + "%").ToList());
        }

        #endregion Methods
    }
}