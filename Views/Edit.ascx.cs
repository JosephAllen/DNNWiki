#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Edit.ascx.cs" company="DNN Corp®">
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

using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Wiki.BusinessObjects.Exceptions;
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Web;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// The Edit Class based on the Wiki Module Base
    /// </summary>
    public partial class Edit : WikiModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit"/> class.
        /// </summary>
        public Edit()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
        }

        #endregion Ctor

        #region Events

        /// <summary>
        /// Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdCancel_Click(System.Object sender, System.EventArgs e)
        {
            this.CancelChanges();
        }

        /// <summary>
        /// Creates/updates a topic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(System.Object sender, System.EventArgs e)
        {
            // If we've change the Topic Name we need to create a new topic.
            Topic ti = null;
            ////if (string.IsNullOrWhiteSpace(PageTopic) | PageTopic != WikiMarkup.DecodeTitle(txtPageName.Text.Trim()))
            ////{
            ////    PageTopic = WikiMarkup.DecodeTitle(txtPageName.Text.Trim());
            ////    _Topic.TopicID = 0;
            ////    ti = TopicBo.GetByNameForModule(ModuleId, PageTopic);
            ////}

            PageTopic = WikiMarkup.DecodeTitle(txtPageName.Text.Trim());

            if (ti == null)
            {
                this.SaveChanges();
                if (PageTopic == WikiHomeName)
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId));
                }

                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic)), false);
            }
            else
            {
                lblPageCreationError.Text = Localization.GetString("lblPageCreationError", LocalResourceFile);
            }
        }

        //// PageTopic = string.Empty LoadTopic() Me.EditPage()
        ////End Sub

        /// <summary>
        /// Handles the Click event of the Save And Continue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void cmdSaveAndContinue_Click(System.Object sender, System.EventArgs e)
        {
            PageTopic = txtPageName.Text.Trim();
            this.SaveAndContinue();
            ////Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "", "topic=" & WikiMarkup.EncodeTitle(Me.PageTopic)), False)
        }

        /// <summary>
        /// Handles the Click event of the Delete Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void DeleteBtn_Click(System.Object sender, System.EventArgs e)
        {
            var topicHistoryList = GetHistory();
            foreach (var th in topicHistoryList)
            {
                TopicHistoryBo.Delete(th);
            }

            TopicBo.Delete(this._Topic);
            Response.Redirect(this.HomeURL, true);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();

            if (CanEdit)
            {
                if (teContent != null)
                {
                    teContent.HtmlEncode = false;
                }

                LoadTopic();
                if (string.IsNullOrWhiteSpace(_Topic.Name))
                {
                    if (this.Request.QueryString["topic"] == null)
                    {
                        PageTopic = WikiHomeName.Replace("[L]", string.Empty);
                        _Topic.Name = PageTopic;
                    }
                    else
                    {
                        PageTopic = WikiMarkup.DecodeTitle(this.Request.QueryString["topic"].ToString()).Replace("[L]", string.Empty);
                        _Topic.Name = PageTopic;
                    }
                }

                this.EditPage();

                // CommentsSec.IsExpanded = FalseB
                if (this.Request.QueryString["add"] != null)
                {
                    PageTopic = string.Empty;
                    LoadTopic();
                    this.EditPage();
                }
                else
                {
                }

                // Add confirmation to the delete button.
                ClientAPI.AddButtonConfirm(DeleteBtn, Localization.GetString("ConfirmDelete", LocalResourceFile));
            }
            else
            {
                // User doesn't have edit rights to this module, load up a message stating so.
                lblMessage.Text = Localization.GetString("NoEditAccess", LocalResourceFile);
                divWikiEdit.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
        }

        ////Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        ////    'TODO: none of this is currently working..... not sure why

        #endregion Events

        #region Methods

        /// <summary>
        /// Cancels the changes.
        /// </summary>
        private void CancelChanges()
        {
            // Send back to the Page View.
            if (string.IsNullOrEmpty(this.PageTopic))
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId), false);
            }
            else
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic)), false);
            }
        }

        /// <summary>
        /// Displays the topic.
        /// </summary>
        private void DisplayTopic()
        {
            this.cmdSave.Visible = true;
            this.cmdSaveAndContinue.Visible = true;
            this.cmdCancel.Visible = true;
            this.teContent.Text = ReadTopicForEdit();

            if (this.WikiSettings.AllowDiscussions)
            {
                this.AllowDiscuss.Enabled = true;
                this.AllowDiscuss.Checked = this._Topic.AllowDiscussions || this.WikiSettings.DefaultDiscussionMode == true;
            }
            else
            {
                this.AllowDiscuss.Enabled = false;
                this.AllowDiscuss.Checked = false;
            }

            if (this.WikiSettings.AllowRatings)
            {
                this.AllowRating.Enabled = true;
                this.AllowRating.Checked = this._Topic.AllowRatings || this.WikiSettings.DefaultRatingMode == true;
            }
            else
            {
                this.AllowRating.Enabled = false;
                this.AllowRating.Checked = false;
            }

            this.DeleteBtn.Visible = false;
            this.DeleteLbl.Visible = false;
            if (this._Topic.Name != WikiHomeName)
            {
                this.DeleteBtn.Visible = true;
                this.DeleteLbl.Visible = true;
            }

            if (string.IsNullOrWhiteSpace(this._Topic.Name))
            {
                txtPageName.Text = string.Empty;
                txtPageName.ReadOnly = false;
            }
            else
            {
                txtPageName.Text = HttpUtility.HtmlDecode(_Topic.Name.Trim().Replace("[L]", string.Empty));
                txtPageName.ReadOnly = PageTopic.Equals(WikiHomeName);
            }

            if (!string.IsNullOrWhiteSpace(this._Topic.Title))
            {
                txtTitle.Text = HttpUtility.HtmlDecode(_Topic.Title.Replace("[L]", string.Empty));
            }

            if (this._Topic.Description != null)
            {
                txtDescription.Text = _Topic.Description;
            }

            if (this._Topic.Keywords != null)
            {
                txtKeywords.Text = _Topic.Keywords;
            }

            //// TODO: Fix Printer Friendly
        }

        /// <summary>
        /// Edits the page.
        /// </summary>
        private void EditPage()
        {
            // Redirect back to the topic url.
            DisplayTopic();
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            AllowDiscuss.Text = Localization.GetString("StartAllowDiscuss", LocalResourceFile);
            AllowRating.Text = Localization.GetString("StartAllowRatings", LocalResourceFile);
            cmdCancel.Text = Localization.GetString("StartCancel", LocalResourceFile);
            ////CommentsSec.Text = Localization.GetString("StartCommentsSection", LocalResourceFile)
            DeleteBtn.Text = Localization.GetString("StartDelete", LocalResourceFile);
            cmdSave.Text = Localization.GetString("StartSave", LocalResourceFile);
            cmdSaveAndContinue.Text = Localization.GetString("StartSaveAndContinue", LocalResourceFile);
            WikiTextDirections.Text = Localization.GetString("StartWikiDirections", LocalResourceFile);
            WikiDirectionsDetails.Text = Localization.GetString("StartWikiDirectionDetails", LocalResourceFile);
            ////RatingSec.Text = Localization.GetString("StartRatingSec.Text", LocalResourceFile)
        }

        /// <summary>
        /// Saves the and continue.
        /// </summary>
        private void SaveAndContinue()
        {
            try
            {
                DotNetNuke.Security.PortalSecurity objSec = new DotNetNuke.Security.PortalSecurity();
                SaveTopic(
                    HttpUtility.HtmlDecode(
                    objSec.InputFilter(objSec.InputFilter(this.teContent.Text, PortalSecurity.FilterFlag.NoMarkup), PortalSecurity.FilterFlag.NoScripting)),
                    this.AllowDiscuss.Checked,
                    this.AllowRating.Checked,
                    objSec.InputFilter(WikiMarkup.DecodeTitle(this.txtTitle.Text.Trim()), PortalSecurity.FilterFlag.NoMarkup),
                    objSec.InputFilter(this.txtDescription.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup),
                    objSec.InputFilter(this.txtKeywords.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));
            }
            catch (TopicValidationException exc)
            {
                switch (exc.CrudError)
                {
                    case DotNetNuke.Wiki.BusinessObjects.TopicBO.TopicError.DUPLICATENAME:
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private void SaveChanges()
        {
            SaveAndContinue();
            ////redirect to the topic's url
        }

        #endregion Methods
    }
}