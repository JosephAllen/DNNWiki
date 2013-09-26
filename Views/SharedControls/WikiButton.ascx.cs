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
using DotNetNuke.Wiki.Utilities;

namespace DotNetNuke.Wiki.Views.SharedControls
{
    partial class WikiButton : WikiModuleBase
    {
        #region Ctor

        public WikiButton()
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
            LocalResourceFile = DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "WikiButton.ascx.resx");

            lnkEdit.Text = Localization.GetString("StartEdit.Text", LocalResourceFile);
            cmdAdd.Text = Localization.GetString("StartAdd", LocalResourceFile);
            txtViewHistory.Text = Localization.GetString("StartViewHistory", LocalResourceFile);

            SetDisplay();
        }

        #endregion Events

        #region Events

        /// <summary>
        /// Sets the display.
        /// </summary>
        private void SetDisplay()
        {
            this.ViewPipe1.Visible = false;
            this.ViewPipe2.Visible = false;

            this.EditPipe.Visible = false;

            this.cmdAdd.Visible = this.CanEdit;
            this.lnkEdit.Visible = false;

            this.AddPipe.Visible = false;
            this.txtViewHistory.Visible = false;

            if ((_Topic.TopicID >= 0 | (Request.QueryString["topic"] != null)))
            {
                this.ViewPipe1.Visible = true;
                this.ViewPipe2.Visible = true;

                this.EditPipe.Visible = this.CanEdit;

                this.cmdAdd.Visible = this.CanEdit;
                this.lnkEdit.Visible = this.CanEdit;

                this.AddPipe.Visible = this.CanEdit;
                this.txtViewHistory.Visible = true;
                txtViewHistory.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "loc=TopicHistory", "topic=" + WikiMarkup.EncodeTitle(PageTopic));
                lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, string.Empty, "topic=" + WikiMarkup.EncodeTitle(PageTopic) + "&loc=edit");
            }

            cmdAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, string.Empty, "&loc=edit&add=true");
        }

        #endregion Events
    }
}