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
    public partial class Administration : PortalModuleBase
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Administration"/> class.
        /// </summary>
        public Administration()
        {
            Load += CtrlPage_Load;
            Init += Page_Init;
        }

        #endregion Ctor

        #region Variables

        private System.Object designerPlaceholderDeclaration;
        protected Setting settings;

        #endregion Variables

        #region Events

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
        /// Handles the CheckedChanged event of the DNNSecurityChk control.
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
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Handles the Load event of the CtrlPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void CtrlPage_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                using (UnitOfWork uof = new UnitOfWork())
                {
                    var settingsBo = new SettingBO(uof);

                    //Put user code to initialize the page here

                    ContentEditors.DataTextField = "Text";
                    ContentEditors.DataValueField = "Text";
                    NotifyRoles.DataTextField = "Text";
                    NotifyRoles.DataValueField = "Text";

                    if (settings == null)
                    {
                        settings = settingsBo.GetByModuleID(ModuleId);
                        if (settings == null)
                        {
                            settings = new Setting();
                            settings.ModuleId = -1;
                            settings.ContentEditorRoles = "UseDNNSettings";
                        }
                    }
                    if (!IsPostBack)
                    {
                        DNNSecurityChk.Checked = settings.ContentEditorRoles.Equals("UseDNNSettings");
                        AllowPageComments.Checked = settings.AllowDiscussions;
                        AllowPageRatings.Checked = settings.AllowRatings;
                        DefaultCommentsMode.Checked = settings.DefaultDiscussionMode == true;
                        DefaultRatingMode.Checked = settings.DefaultRatingMode == true;
                        NotifyMethodUserComments.Checked = settings.CommentNotifyUsers == true;

                        NotifyMethodCustomRoles.Checked =
                            !string.IsNullOrWhiteSpace(settings.CommentNotifyRoles) &&
                            settings.CommentNotifyRoles.StartsWith("UseDNNSettings;") && !string.IsNullOrWhiteSpace(settings.CommentNotifyRoles);
                        if (NotifyMethodCustomRoles.Checked)
                        {
                            NotifyMethodEditRoles.Checked = settings.CommentNotifyRoles.Contains(";Edit");
                            NotifyMethodViewRoles.Checked = settings.CommentNotifyRoles.Contains(";View");
                        }

                        BindRights();
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

        //NOTE: The following placeholder declaration is required by the Web Form Designer.
        //Do not delete or move it.
        private void Page_Init(System.Object sender, System.EventArgs e)
        {
            Framework.jQuery.RequestUIRegistration();
            Framework.jQuery.RequestDnnPluginsRegistration();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Activates the items.
        /// </summary>
        /// <param name="uof">The uof.</param>
        private void ActivateItems(UnitOfWork uof)
        {
            if (ActivateComments.Checked | ActivateRatings.Checked)
            {
                TopicBO topicBo = new TopicBO(uof);

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
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                var settingsBo = new SettingBO(uof);
                if (DNNSecurityChk.Checked == true)
                {
                    settings.ContentEditorRoles = "UseDNNSettings";
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in ContentEditors.Assigned)
                    {
                        list = list + li.Value + ";";
                    }
                    settings.ContentEditorRoles = list;
                }

                if (NotifyMethodCustomRoles.Checked == false)
                {
                    settings.CommentNotifyRoles = "UseDNNSettings";
                    if (NotifyMethodEditRoles.Checked == true)
                    {
                        settings.CommentNotifyRoles = settings.CommentNotifyRoles + ";Edit";
                    }
                    if (NotifyMethodViewRoles.Checked == true)
                    {
                        settings.CommentNotifyRoles = settings.CommentNotifyRoles + ";View";
                    }
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in NotifyRoles.Assigned)
                    {
                        list = list + li.Value + ";";
                    }
                    settings.CommentNotifyRoles = list;
                }

                settings.AllowDiscussions = AllowPageComments.Checked;
                settings.AllowRatings = AllowPageRatings.Checked;
                settings.DefaultDiscussionMode = DefaultCommentsMode.Checked;
                settings.DefaultRatingMode = DefaultRatingMode.Checked;
                settings.CommentNotifyUsers = NotifyMethodUserComments.Checked;

                if (settings.ModuleId == -1)
                {
                    settings.ModuleId = ModuleId;
                    settingsBo.Add(settings);
                }
                else
                {
                    settingsBo.Update(settings);
                }
                ActivateItems(uof);
            }
        }

        /// <summary>
        /// Binds the rights.
        /// </summary>
        private void BindRights()
        {
            // declare roles
            ArrayList arrAvailableAuthViewRoles = new ArrayList();
            ArrayList arrAvailableNotifyRoles = new ArrayList();

            // add an entry of All Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("All Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));
            // add an entry of Unauthenticated Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));
            // add an entry of All Users for the Edit roles

            // process portal roles
            DotNetNuke.Security.Roles.RoleController objRoles = new DotNetNuke.Security.Roles.RoleController();

            var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>();
            foreach (var objRole in arrRoles)
            {
                arrAvailableAuthViewRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
                arrAvailableNotifyRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
            }

            // populate view roles

            ArrayList arrAssignedAuthViewRoles = new ArrayList();
            ArrayList arrAssignedNotifyRoles = new ArrayList();

            Array arrAuthViewRoles = null;
            Array arrAuthNotifyRoles = null;
            if (settings.ContentEditorRoles.Equals("UseDNNSettings"))
            {
                arrAuthViewRoles = settings.ContentEditorRoles.Split(new string[] { "UseDNNSettings" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrAuthViewRoles = settings.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (!string.IsNullOrWhiteSpace(settings.CommentNotifyRoles))
                if (settings.CommentNotifyRoles.StartsWith("UseDNNSettings;"))
                {
                    settings.CommentNotifyRoles = settings.CommentNotifyRoles.Replace("UseDNNSettings;", string.Empty);
                    arrAuthNotifyRoles = settings.CommentNotifyRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in arrAuthNotifyRoles)
                    {
                        if (s.Equals("View"))
                        {
                            NotifyMethodViewRoles.Checked = true;
                        }
                        else if (s.Equals("Edit"))
                        {
                            NotifyMethodEditRoles.Checked = true;
                        }
                    }
                }
                else
                {
                    arrAuthNotifyRoles = settings.CommentNotifyRoles.Split(new char[] { '|' })[0].Split(new char[] { ';' });
                }

            if (arrAuthViewRoles != null)
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
                                break; // TODO: might not be correct. Was : Exit For
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
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                }
            }

            int x = arrAvailableAuthViewRoles.Count;
            int y = arrAssignedAuthViewRoles.Count;
            ContentEditors.Available = arrAvailableAuthViewRoles;
            ContentEditors.Assigned = arrAssignedAuthViewRoles;

            NotifyRoles.Assigned = arrAssignedNotifyRoles;
            NotifyRoles.Available = arrAvailableNotifyRoles;
        }

        #endregion Methods
    }
}