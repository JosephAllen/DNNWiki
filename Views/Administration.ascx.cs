#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Administration.ascx.cs" company="DNN Corp®">
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
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Administration Partial Class
    /// </summary>
    public partial class Administration : PortalModuleBase
    {
        #region Variables

        private const string StrUseDNNSettings = "UseDNNSettings";

        protected Setting mSettingsModel; ////Data from the WikiSettings Busines Object

        // TODO Do we need this? This is legacy code from VB conversion
        private System.Object designerPlaceholderDeclaration;

        #endregion Variables

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Administration"/> class.
        /// </summary>
        public Administration()
        {
            Load += this.CtrlPage_Load;
            Init += this.Page_Init;
        }

        #endregion Ctor

        #region Events

        /// <summary>
        /// Handles the CheckedChanged event of the AllowPageRatings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void AllowPageRatings_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (AllowPageComments.Checked)
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
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void AllowPageComments_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (AllowPageComments.Checked)
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
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Handles the Load event of the CtrlPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        private void CtrlPage_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                using (UnitOfWork currentUnitOfWork = new UnitOfWork())
                {
                    var settingsBo = new SettingBO(currentUnitOfWork);

                    ////Put user code to initialize the page here

                    ContentEditors.DataTextField = "Text";
                    ContentEditors.DataValueField = "Text";
                    NotifyRoles.DataTextField = "Text";
                    NotifyRoles.DataValueField = "Text";

                    if (this.mSettingsModel == null)
                    {
                        this.mSettingsModel = settingsBo.GetByModuleID(ModuleId);
                        if (this.mSettingsModel == null)
                        {
                            this.mSettingsModel = new Setting();
                            this.mSettingsModel.ModuleId = -1;
                            this.mSettingsModel.ContentEditorRoles = StrUseDNNSettings;
                        }
                    }

                    if (!IsPostBack)
                    {
                        DNNSecurityChk.Checked = this.mSettingsModel.ContentEditorRoles.Equals(StrUseDNNSettings);
                        AllowPageComments.Checked = this.mSettingsModel.AllowDiscussions;
                        AllowPageRatings.Checked = this.mSettingsModel.AllowRatings;
                        DefaultCommentsMode.Checked = this.mSettingsModel.DefaultDiscussionMode == true;
                        DefaultRatingMode.Checked = this.mSettingsModel.DefaultRatingMode == true;
                        NotifyMethodUserComments.Checked = this.mSettingsModel.CommentNotifyUsers == true;

                        NotifyMethodCustomRoles.Checked =
                            !string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles) &&
                            this.mSettingsModel.CommentNotifyRoles.StartsWith("UseDNNSettings;") && !string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles);
                        if (NotifyMethodCustomRoles.Checked)
                        {
                            NotifyMethodEditRoles.Checked = this.mSettingsModel.CommentNotifyRoles.Contains(";Edit");
                            NotifyMethodViewRoles.Checked = this.mSettingsModel.CommentNotifyRoles.Contains(";View");
                        }

                        // Call the BindRights method
                        this.BindRights();

                        if (DNNSecurityChk.Checked == true)
                        {
                            ContentEditors.Visible = false;
                            WikiSecurity.Visible = false;
                        }
                        else
                        {
                            ContentEditors.Visible = true;
                            WikiSecurity.Visible = true;
                        }

                        if (AllowPageComments.Checked)
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

                        if (AllowPageRatings.Checked)
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

                        if (NotifyMethodCustomRoles.Checked)
                        {
                            NotifyRoles.Visible = true;
                            lblNotifyRoles.Visible = true;
                            this.NotifyMethodEditRoles.Enabled = false;
                            this.NotifyMethodViewRoles.Enabled = false;
                            this.NotifyMethodViewRoles.Checked = false;
                            this.NotifyMethodEditRoles.Checked = false;
                        }
                        else
                        {
                            this.NotifyMethodEditRoles.Enabled = true;
                            this.NotifyMethodViewRoles.Enabled = true;

                            NotifyRoles.Visible = false;
                            lblNotifyRoles.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the DNN Security Check control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void DNNSecurityChk_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (DNNSecurityChk.Checked == true)
            {
                ContentEditors.Visible = false;
                WikiSecurity.Visible = false;
            }
            else
            {
                ContentEditors.Visible = true;
                WikiSecurity.Visible = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the NotifyMethodCustomRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void NotifyMethodCustomRoles_CheckedChanged(object sender, System.EventArgs e)
        {
            if (NotifyMethodCustomRoles.Checked)
            {
                NotifyRoles.Visible = true;
                lblNotifyRoles.Visible = true;

                this.NotifyMethodEditRoles.Enabled = false;
                this.NotifyMethodViewRoles.Enabled = false;
                this.NotifyMethodViewRoles.Checked = false;
                this.NotifyMethodEditRoles.Checked = false;
            }
            else
            {
                this.NotifyMethodEditRoles.Enabled = true;
                this.NotifyMethodViewRoles.Enabled = true;
                lblNotifyRoles.Visible = false;
                NotifyRoles.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Initialize event of the Page control.
        /// NOTE: The following placeholder declaration is required by the Web Form Designer.
        /// Do not delete or move it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_Init(System.Object sender, System.EventArgs e)
        {
            Framework.jQuery.RequestUIRegistration();
            Framework.jQuery.RequestDnnPluginsRegistration();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
        private void ActivateItems(UnitOfWork currentUnitOfWork)
        {
            if (ActivateComments.Checked | ActivateRatings.Checked)
            {
                TopicBO topicBo = new TopicBO(currentUnitOfWork);

                var alltopics = topicBo.GetAllByModuleID(this.ModuleId);

                foreach (var topic in alltopics)
                {
                    if (topic.AllowDiscussions == false & ActivateComments.Checked)
                    {
                        topic.AllowDiscussions = true;
                    }

                    if (topic.AllowRatings == false & ActivateRatings.Checked)
                    {
                        topic.AllowRatings = true;
                    }

                    topicBo.Update(topic);
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
            if (this.mSettingsModel.ContentEditorRoles.Equals(StrUseDNNSettings))
            {
                arrAuthViewRoles = this.mSettingsModel.ContentEditorRoles.Split(new string[] { StrUseDNNSettings }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrAuthViewRoles = this.mSettingsModel.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // populate the notify roles
            if (!string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles))
            {
                if (this.mSettingsModel.CommentNotifyRoles.StartsWith("UseDNNSettings;"))
                {
                    this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Replace("UseDNNSettings;", string.Empty);
                    arrAuthNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string curRole in arrAuthNotifyRoles)
                    {
                        if (curRole.Equals("View"))
                        {
                            NotifyMethodViewRoles.Checked = true;
                        }
                        else if (curRole.Equals("Edit"))
                        {
                            NotifyMethodEditRoles.Checked = true;
                        }
                    }
                }
                else
                {
                    arrAuthNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Split(new char[] { '|' })[0].Split(new char[] { ';' });
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
            ContentEditors.Available = arrAvailableAuthViewRoles;
            ContentEditors.Assigned = arrAssignedAuthViewRoles;

            NotifyRoles.Assigned = arrAssignedNotifyRoles;
            NotifyRoles.Available = arrAvailableNotifyRoles;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            using (UnitOfWork currentUnitOfWork = new UnitOfWork())
            {
                var settingsBo = new SettingBO(currentUnitOfWork);
                if (DNNSecurityChk.Checked == true)
                {
                    this.mSettingsModel.ContentEditorRoles = StrUseDNNSettings;
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in ContentEditors.Assigned)
                    {
                        list = list + li.Value + ";";
                    }

                    this.mSettingsModel.ContentEditorRoles = list;
                }

                if (NotifyMethodCustomRoles.Checked == false)
                {
                    this.mSettingsModel.CommentNotifyRoles = StrUseDNNSettings;
                    if (NotifyMethodEditRoles.Checked == true)
                    {
                        this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles + ";Edit";
                    }

                    if (this.NotifyMethodViewRoles.Checked == true)
                    {
                        this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles + ";View";
                    }
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in NotifyRoles.Assigned)
                    {
                        list = list + li.Value + ";";
                    }

                    this.mSettingsModel.CommentNotifyRoles = list;
                }

                this.mSettingsModel.AllowDiscussions = AllowPageComments.Checked;
                this.mSettingsModel.AllowRatings = AllowPageRatings.Checked;
                this.mSettingsModel.DefaultDiscussionMode = DefaultCommentsMode.Checked;
                this.mSettingsModel.DefaultRatingMode = DefaultRatingMode.Checked;
                this.mSettingsModel.CommentNotifyUsers = NotifyMethodUserComments.Checked;

                if (this.mSettingsModel.ModuleId == -1)
                {
                    this.mSettingsModel.ModuleId = ModuleId;
                    settingsBo.Add(this.mSettingsModel);
                }
                else
                {
                    settingsBo.Update(this.mSettingsModel);
                }

                this.ActivateItems(currentUnitOfWork);
            }
        }

        /// <summary>
        /// Binds role controls.
        /// </summary>
        /// <param name="roleType">Type of the role.</param>
        private void zzBindRoleControls(string roleType)
        {
            // declare variables
            string roles = string.Empty;
            ListItem[] arrAuthRoles = null;
            ArrayList arrAssignedRoles = new ArrayList();
            ArrayList arrAvailableRoles = new ArrayList();

            if (roleType == "ContentEditors")
            {
                roles = this.mSettingsModel.ContentEditorRoles;
            }
            else if (roleType == "CommentNotifyRoles")
            {
                roles = this.mSettingsModel.CommentNotifyRoles;
            }
            else
            {
                // TODO We've got problems if its not "ContentEditors" or "CommentNotifyRoles" TODO
                // Need to raise an error
            }

            if (roles != StrUseDNNSettings)
            {
                arrAuthRoles = roles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p =>
                    new ListItem(p, p)).ToArray();

                // Convert the arrAuthRoles array to an array list
                arrAssignedRoles.AddRange(arrAuthRoles);
            }

            if (roleType == "CommentNotifyRoles")
            {
                foreach (ListItem curRole in arrAssignedRoles)
                {
                    if (curRole.Value == string.Empty)
                    {
                        arrAvailableRoles.Remove(curRole);
                    }
                }
            }

            // Call the BuildAllRolesArray method to Build an Array of All Roles ----------------
            // Available Roles = All Roles - Assigned Roles AvailableRoles(arrAssignedRoles);

            // Build Available Roles add an entry of All Users for the View roles
            arrAvailableRoles.Add(new ListItem("All Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));

            // add an entry of Unauthenticated Users for the View roles
            arrAvailableRoles.Add(new ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

            // process portal roles
            DotNetNuke.Security.Roles.RoleController objRoles = new DotNetNuke.Security.Roles.RoleController();

            var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>();
            foreach (var objRole in arrRoles)
            {
                arrAvailableRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
            }

            // Remove the Assigned Roles from the Available Roles
            if (arrAvailableRoles.Count > 0)
            {
                foreach (ListItem curRole in arrAssignedRoles)
                {
                    arrAvailableRoles.Remove(curRole);
                }
            }

            if (roleType == "ContentEditors")
            {
                ContentEditors.Assigned = arrAssignedRoles;
                ContentEditors.Available = arrAvailableRoles;
            }
            else if (roleType == "CommentNotifyRoles")
            {
                NotifyRoles.Assigned = arrAssignedRoles;
                NotifyRoles.Available = arrAvailableRoles;
            }
            else
            {
                // TODO We've got problems if its not "ContentEditors" or "CommentNotifyRoles" TODO
                // Need to raise an error
            }
        }

        /// <summary>
        /// Get the available roles.
        /// </summary>
        /// <param name="arrAssignedRoles">An array of assigned roles.</param>
        /// <returns>ArrayList of Available Roles</returns>
        private ArrayList zzAvailableRoles(ArrayList arrAssignedRoles)
        {
            // Declare Variables
            ArrayList arrAvailableRoles = new ArrayList();

            // add an entry of All Users for the View roles
            arrAvailableRoles.Add(new ListItem("All Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));

            // add an entry of Unauthenticated Users for the View roles
            arrAvailableRoles.Add(new ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

            // process portal roles
            DotNetNuke.Security.Roles.RoleController objRoles = new DotNetNuke.Security.Roles.RoleController();

            var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>();
            foreach (var objRole in arrRoles)
            {
                arrAvailableRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
            }

            // Remove the Assigned Roles from the Available Roles
            if (arrAvailableRoles.Count > 0)
            {
                foreach (ListItem curRole in arrAssignedRoles)
                {
                    arrAvailableRoles.Remove(curRole);
                }
            }

            return arrAvailableRoles;
        }

        #endregion Methods
    }
}