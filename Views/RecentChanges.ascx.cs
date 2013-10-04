#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="RecentChanges.ascx.cs" company="DNN Corp®">
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

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Recent changes class based on WikiModuleBase
    /// </summary>
    partial class RecentChanges : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentChanges"/> class.
        /// </summary>
        public RecentChanges()
        {
            Load += Page_Load;
        }

        #endregion Ctor

        #region Events

        /// <summary>
        /// Handles the Click event of the Last 7 Days control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdLast7Days_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(7);
        }

        /// <summary>
        /// Handles the Click event of the Last 24 Hrs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdLast24Hrs_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(1);
        }

        /// <summary>
        /// Handles the Click event of the Last Month control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdLastMonth_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(31);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            this.LoadLocalization();
            if (!this.IsPostBack)
            {
                HitTable.Text = CreateRecentChangeTable(1);
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            TitleLbl.Text = Localization.GetString("RCTitle", RouterResourceFile);
            cmdLast24Hrs.Text = Localization.GetString("RCLast24h", RouterResourceFile);
            cmdLast7Days.Text = Localization.GetString("RCLast7d", RouterResourceFile);
            cmdLastMonth.Text = Localization.GetString("RCLastMonth", RouterResourceFile);
        }

        #endregion Methods
    }
}