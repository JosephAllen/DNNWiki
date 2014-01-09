#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikiSettings.ascx.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sub license, and/or sell copies of the Software, and to permit persons to whom the Software is
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
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Wiki Settings class
    /// </summary>
    public partial class WikiSettings : PortalModuleBase
    {
        #region Variables

        /// <summary>
        /// The string use DNN settings, indicates that DNN settings should be used instead
        /// </summary>
        private WikiModuleSettings mWikiModuleSettings;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiSettings" /> class.
        /// </summary>
        public WikiSettings()
        {
            this.Load += this.Page_Load;
            this.Init += this.Page_Init;
            this.Unload += this.Page_Unload;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the get wiki module settings.
        /// </summary>
        /// <value>The get wiki module settings.</value>
        public WikiModuleSettings GetWikiSettings
        {
            get
            {
                if (this.mWikiModuleSettings == null)
                {
                    this.mWikiModuleSettings = new WikiModuleSettings(this.ModuleId);
                }
                return mWikiModuleSettings;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the CheckedChanged event of the AllowPageRatings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected void AllowPageRatings_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AllowPageComments.Checked)
            {
                this.ActivateRatings.Enabled = true;
                this.ActivateRatings.Checked = true;

                this.DefaultRatingMode.Enabled = true;
            }
            else
            {
                this.ActivateRatings.Enabled = false;
                this.ActivateRatings.Checked = false;

                this.DefaultRatingMode.Enabled = false;
                this.DefaultRatingMode.Checked = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the AllowPageComments control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected void AllowPageComments_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.AllowPageComments.Checked)
            {
                this.ActivateComments.Enabled = true;
                this.ActivateComments.Checked = true;

                this.DefaultCommentsMode.Enabled = true;
            }
            else
            {
                this.ActivateComments.Enabled = false;
                this.ActivateComments.Checked = false;

                this.DefaultCommentsMode.Enabled = false;
                this.DefaultCommentsMode.Checked = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Handles the Load event of the CtrlPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                ////Put user code to initialize the page here

                this.ContentEditors.DataTextField = "Text";
                this.ContentEditors.DataValueField = "Text";
                this.NotifyRoles.DataTextField = "Text";
                this.NotifyRoles.DataValueField = "Text";

                if (!this.IsPostBack)
                {
                    this.DNNSecurityChk.Checked = this.GetWikiSettings.UsesDnnSettings();
                    this.AllowPageComments.Checked = this.GetWikiSettings.AllowDiscussions;
                    this.AllowPageRatings.Checked = this.GetWikiSettings.AllowRatings;
                    this.DefaultCommentsMode.Checked = this.GetWikiSettings.DefaultDiscussionMode;
                    this.DefaultRatingMode.Checked = this.GetWikiSettings.DefaultRatingMode;
                    this.NotifyMethodUserComments.Checked = this.GetWikiSettings.CommentNotifyUsers;

                    this.NotifyMethodCustomRoles.Checked =
                        !(!string.IsNullOrWhiteSpace(this.GetWikiSettings.CommentNotifyRoles) &&
                        this.GetWikiSettings.CommentNotifyRoles.StartsWith("UseDNNSettings;"));

                    if (this.NotifyMethodCustomRoles.Checked &&
                        !string.IsNullOrWhiteSpace(this.GetWikiSettings.CommentNotifyRoles))
                    {
                        this.NotifyMethodEditRoles.Checked = this.GetWikiSettings.CommentNotifyRoles.Contains(";Edit");
                        this.NotifyMethodViewRoles.Checked = this.GetWikiSettings.CommentNotifyRoles.Contains(";View");
                    }

                    // Call the BindRights method
                    this.BindRights();

                    if (this.DNNSecurityChk.Checked == true)
                    {
                        this.ContentEditors.Visible = false;
                        this.WikiSecurity.Visible = false;
                    }
                    else
                    {
                        this.ContentEditors.Visible = true;
                        this.WikiSecurity.Visible = true;
                    }

                    if (this.AllowPageComments.Checked)
                    {
                        this.ActivateComments.Enabled = true;
                        this.DefaultCommentsMode.Enabled = true;
                    }
                    else
                    {
                        this.ActivateComments.Enabled = false;
                        this.ActivateComments.Checked = false;
                        this.DefaultCommentsMode.Enabled = false;
                        this.DefaultCommentsMode.Checked = false;
                    }

                    if (this.AllowPageRatings.Checked)
                    {
                        this.ActivateRatings.Enabled = true;
                        this.DefaultRatingMode.Enabled = true;
                    }
                    else
                    {
                        this.ActivateRatings.Enabled = false;
                        this.ActivateRatings.Checked = false;
                        this.DefaultRatingMode.Enabled = false;
                        this.DefaultRatingMode.Checked = false;
                    }

                    if (this.NotifyMethodCustomRoles.Checked)
                    {
                        this.NotifyRoles.Visible = true;
                        this.lblNotifyRoles.Visible = true;
                        this.NotifyMethodEditRoles.Enabled = false;
                        this.NotifyMethodViewRoles.Enabled = false;
                        this.NotifyMethodViewRoles.Checked = false;
                        this.NotifyMethodEditRoles.Checked = false;
                    }
                    else
                    {
                        this.NotifyMethodEditRoles.Enabled = true;
                        this.NotifyMethodViewRoles.Enabled = true;

                        this.NotifyRoles.Visible = false;
                        this.lblNotifyRoles.Visible = false;
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Page_Unload(object sender, EventArgs e)
        {
            if (this.mWikiModuleSettings != null)
            {
                this.mWikiModuleSettings = null;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the DNN Security Check control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected void DNNSecurityChk_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.DNNSecurityChk.Checked == true)
            {
                this.ContentEditors.Visible = false;
                this.WikiSecurity.Visible = false;
            }
            else
            {
                this.ContentEditors.Visible = true;
                this.WikiSecurity.Visible = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the NotifyMethodCustomRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        protected void NotifyMethodCustomRoles_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.NotifyMethodCustomRoles.Checked)
            {
                this.NotifyRoles.Visible = true;
                this.lblNotifyRoles.Visible = true;

                this.NotifyMethodEditRoles.Enabled = false;
                this.NotifyMethodViewRoles.Enabled = false;
                this.NotifyMethodViewRoles.Checked = false;
                this.NotifyMethodEditRoles.Checked = false;
            }
            else
            {
                this.NotifyMethodEditRoles.Enabled = true;
                this.NotifyMethodViewRoles.Enabled = true;
                this.lblNotifyRoles.Visible = false;
                this.NotifyRoles.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Initialize event of the Page control.
        /// NOTE: The following placeholder declaration is required by the Web Form Designer.
        /// Do not delete or move it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.EventArgs" /> instance containing the event data.
        /// </param>
        private void Page_Init(object sender, System.EventArgs e)
        {
            Framework.jQuery.RequestUIRegistration();
            Framework.jQuery.RequestDnnPluginsRegistration();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Activates the items.
        /// </summary>
        /// <param name="currentUnitOfWork">The UnitOfWork.</param>
        private void ActivateItems()
        {
            if (this.ActivateComments.Checked | this.ActivateRatings.Checked)
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    TopicBO topicBo = new TopicBO(uow);

                    var alltopics = topicBo.GetAllByModuleID(this.ModuleId);

                    foreach (var topic in alltopics)
                    {
                        if (topic.AllowDiscussions == false & this.ActivateComments.Checked)
                        {
                            topic.AllowDiscussions = true;
                        }

                        if (topic.AllowRatings == false & this.ActivateRatings.Checked)
                        {
                            topic.AllowRatings = true;
                        }

                        topicBo.Update(topic);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the edit rights for user roles and binds them to the respective list control
        /// </summary>
        private void BindRights()
        {
            // declare variables
            ArrayList arrAvailableAuthViewRoles = new ArrayList();
            ArrayList arrAvailableNotifyRoles = new ArrayList();
            ArrayList arrAssignedAuthViewRoles = new ArrayList();
            ArrayList arrAssignedNotifyRoles = new ArrayList();
            Array arrAuthViewRoles = null;
            Array arrAuthNotifyRoles = null;

            // add an entry of All Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("All Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));

            // add an entry of Unauthenticated Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

            // process portal roles
            DotNetNuke.Security.Roles.RoleController objRoles = new DotNetNuke.Security.Roles.RoleController();

            var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>();
            foreach (var objRole in arrRoles)
            {
                arrAvailableAuthViewRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
                arrAvailableNotifyRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
            }

            // populate view roles
            if (this.GetWikiSettings.UsesDnnSettings())
            {
                arrAuthViewRoles = new string[] { };// this.mSettingsModel.ContentEditorRoles.Split(new string[] { StrUseDNNSettings }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrAuthViewRoles = this.GetWikiSettings.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // populate the notify roles
            if (!string.IsNullOrWhiteSpace(this.GetWikiSettings.CommentNotifyRoles))
            {
                if (this.GetWikiSettings.CommentNotifyRoles.StartsWith("UseDNNSettings;"))
                {
                    var commentNotifyRoles = this.GetWikiSettings.CommentNotifyRoles.Replace("UseDNNSettings;", string.Empty);
                    arrAuthNotifyRoles = commentNotifyRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string curRole in arrAuthNotifyRoles)
                    {
                        if (curRole.Equals("View"))
                        {
                            this.NotifyMethodViewRoles.Checked = true;
                        }
                        else if (curRole.Equals("Edit"))
                        {
                            this.NotifyMethodEditRoles.Checked = true;
                        }
                    }
                }
                else
                {
                    arrAuthNotifyRoles = this.GetWikiSettings.CommentNotifyRoles.Split(new char[] { '|' })[0].Split(new char[] { ';' });
                }
            }

            if (arrAuthViewRoles != null)
            {
                foreach (string strRole in arrAuthViewRoles)
                {
                    if (!string.IsNullOrEmpty(strRole))
                    {
                        foreach (ListItem objListItem in arrAvailableAuthViewRoles)
                        {
                            if (objListItem.Value == strRole)
                            {
                                arrAssignedAuthViewRoles.Add(objListItem);
                                arrAvailableAuthViewRoles.Remove(objListItem);
                                break;
                            }
                        }
                    }
                }
            }

            if (arrAuthNotifyRoles != null)
            {
                foreach (string strRole in arrAuthNotifyRoles)
                {
                    if (!string.IsNullOrEmpty(strRole))
                    {
                        foreach (ListItem objListItem in arrAvailableNotifyRoles)
                        {
                            if (objListItem.Value == strRole)
                            {
                                arrAssignedNotifyRoles.Add(objListItem);
                                arrAvailableNotifyRoles.Remove(objListItem);
                                break;
                            }
                        }
                    }
                }
            }

            int x = arrAvailableAuthViewRoles.Count; // TODO Do we need this?
            int y = arrAssignedAuthViewRoles.Count; // TODO Do we need this?
            this.ContentEditors.Available = arrAvailableAuthViewRoles;
            this.ContentEditors.Assigned = arrAssignedAuthViewRoles;

            this.NotifyRoles.Assigned = arrAssignedNotifyRoles;
            this.NotifyRoles.Available = arrAvailableNotifyRoles;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            if (this.DNNSecurityChk.Checked == true)
            {
                this.GetWikiSettings.ContentEditorRoles = WikiModuleSettings.StrUseDNNSettings;
            }
            else
            {
                this.GetWikiSettings.ContentEditorRoles = GetAssignedRoles(ref this.ContentEditors);
            }

            if (!this.NotifyMethodCustomRoles.Checked)
            {
                if (this.NotifyMethodEditRoles.Checked)
                {
                    this.GetWikiSettings.CommentNotifyRoles = this.GetWikiSettings.CommentNotifyRoles + ";Edit";
                }
                else if (this.NotifyMethodViewRoles.Checked)
                {
                    this.GetWikiSettings.CommentNotifyRoles = this.GetWikiSettings.CommentNotifyRoles + ";View";
                }
                else
                {
                    this.GetWikiSettings.CommentNotifyRoles = WikiModuleSettings.StrUseDNNSettings;
                }
            }
            else
            {
                this.GetWikiSettings.CommentNotifyRoles = GetAssignedRoles(ref this.NotifyRoles);
            }

            this.GetWikiSettings.AllowDiscussions = this.AllowPageComments.Checked;
            this.GetWikiSettings.AllowRatings = this.AllowPageRatings.Checked;
            this.GetWikiSettings.DefaultDiscussionMode = this.DefaultCommentsMode.Checked;
            this.GetWikiSettings.DefaultRatingMode = this.DefaultRatingMode.Checked;
            this.GetWikiSettings.CommentNotifyUsers = this.NotifyMethodUserComments.Checked;

            this.GetWikiSettings.SaveSettings();

            this.ActivateItems();
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>returns the list of roles assigned in a dual list control</returns>
        private string GetAssignedRoles(ref DualListControl control)
        {
            string list = ";";

            foreach (ListItem item in control.Assigned)
            {
                list += item.Value + ";";
            }

            return list;
        }

        #endregion Methods
    }
}