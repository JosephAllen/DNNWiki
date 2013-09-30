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
//--------------------------------------------------------------------------------------------------------

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

        private const string STR_UseDNNSettings = "UseDNNSettings";

        protected Setting m_settings; //Data from the WikiSettings Busines Object

        // TODO Do we need this? This is legacy code from VB conversion
        private System.Object designerPlaceholderDeclaration;

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
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        private void CtrlPage_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                using (UnitOfWork currentUnitOfWork = new UnitOfWork())
                {
                    var settingsBo = new SettingBO(currentUnitOfWork);

                    //Put user code to initialize the page here

                    ContentEditors.DataTextField = "Text";
                    ContentEditors.DataValueField = "Text";
                    NotifyRoles.DataTextField = "Text";
                    NotifyRoles.DataValueField = "Text";

                    if (m_settings == null)
                    {
                        m_settings = settingsBo.GetByModuleID(ModuleId);
                        if (m_settings == null)
                        {
                            m_settings = new Setting();
                            m_settings.ModuleId = -1;
                            m_settings.ContentEditorRoles = STR_UseDNNSettings;
                        }
                    }
                    if (!IsPostBack)
                    {
                        DNNSecurityChk.Checked = m_settings.ContentEditorRoles.Equals(STR_UseDNNSettings);
                        AllowPageComments.Checked = m_settings.AllowDiscussions;
                        AllowPageRatings.Checked = m_settings.AllowRatings;
                        DefaultCommentsMode.Checked = m_settings.DefaultDiscussionMode == true;
                        DefaultRatingMode.Checked = m_settings.DefaultRatingMode == true;
                        NotifyMethodUserComments.Checked = m_settings.CommentNotifyUsers == true;

                        NotifyMethodCustomRoles.Checked =
                            !string.IsNullOrWhiteSpace(m_settings.CommentNotifyRoles) &&
                            m_settings.CommentNotifyRoles.StartsWith("UseDNNSettings;") && !string.IsNullOrWhiteSpace(m_settings.CommentNotifyRoles);
                        if (NotifyMethodCustomRoles.Checked)
                        {
                            NotifyMethodEditRoles.Checked = m_settings.CommentNotifyRoles.Contains(";Edit");
                            NotifyMethodViewRoles.Checked = m_settings.CommentNotifyRoles.Contains(";View");
                        }

                        // Call the BindRights method this.BindRights();
                        this.BindEditRights();
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
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            using (UnitOfWork currentUnitOfWork = new UnitOfWork())
            {
                var settingsBo = new SettingBO(currentUnitOfWork);
                if (DNNSecurityChk.Checked == true)
                {
                    m_settings.ContentEditorRoles = STR_UseDNNSettings;
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in ContentEditors.Assigned)
                    {
                        list = list + li.Value + ";";
                    }
                    m_settings.ContentEditorRoles = list;
                }

                if (NotifyMethodCustomRoles.Checked == false)
                {
                    m_settings.CommentNotifyRoles = STR_UseDNNSettings;
                    if (NotifyMethodEditRoles.Checked == true)
                    {
                        m_settings.CommentNotifyRoles = m_settings.CommentNotifyRoles + ";Edit";
                    }
                    if (NotifyMethodViewRoles.Checked == true)
                    {
                        m_settings.CommentNotifyRoles = m_settings.CommentNotifyRoles + ";View";
                    }
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in NotifyRoles.Assigned)
                    {
                        list = list + li.Value + ";";
                    }
                    m_settings.CommentNotifyRoles = list;
                }

                m_settings.AllowDiscussions = AllowPageComments.Checked;
                m_settings.AllowRatings = AllowPageRatings.Checked;
                m_settings.DefaultDiscussionMode = DefaultCommentsMode.Checked;
                m_settings.DefaultRatingMode = DefaultRatingMode.Checked;
                m_settings.CommentNotifyUsers = NotifyMethodUserComments.Checked;

                if (m_settings.ModuleId == -1)
                {
                    m_settings.ModuleId = ModuleId;
                    settingsBo.Add(m_settings);
                }
                else
                {
                    settingsBo.Update(m_settings);
                }
                ActivateItems(currentUnitOfWork);
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
            if (m_settings.ContentEditorRoles.Equals(STR_UseDNNSettings))
            {
                arrAuthViewRoles = m_settings.ContentEditorRoles.Split(new string[] { STR_UseDNNSettings }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrAuthViewRoles = m_settings.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // populate the notify roles
            if (!string.IsNullOrWhiteSpace(m_settings.CommentNotifyRoles))
                if (m_settings.CommentNotifyRoles.StartsWith("UseDNNSettings;"))
                {
                    m_settings.CommentNotifyRoles = m_settings.CommentNotifyRoles.Replace("UseDNNSettings;", string.Empty);
                    arrAuthNotifyRoles = m_settings.CommentNotifyRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

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
                    arrAuthNotifyRoles = m_settings.CommentNotifyRoles.Split(new char[] { '|' })[0].Split(new char[] { ';' });
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
                                break;
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
        /// Sets the list control data.
        /// </summary>
        private void BindEditRights()
        {
            // declare variables
            ListItem[] arrAuthRoles = null;
            ArrayList arrAssignedRoles = new ArrayList();
            ArrayList arrAvailableRoles = new ArrayList();

            // populate edit roles
            if (m_settings.ContentEditorRoles.Equals(STR_UseDNNSettings))
            {
                arrAuthRoles = m_settings.ContentEditorRoles.Split(new string[] { STR_UseDNNSettings }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p =>
                    new ListItem(p, p)).ToArray();
            }
            else
            {
                arrAuthRoles = m_settings.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p =>
                    new ListItem(p, p)).ToArray();
            }

            // Convert the arrAuthRoles array to an array list
            arrAssignedRoles.AddRange(arrAuthRoles);

            // Call the BuildAllRolesArray method to Build an Array of All Roles
            arrAvailableRoles = AvailableRoles(arrAssignedRoles);

            ContentEditors.Available = arrAvailableRoles;
            ContentEditors.Assigned = arrAssignedRoles;
        }

        /// <summary>
        /// Builds an array of Available roles.
        /// </summary>
        /// <returns>An array of Available Roles in the portal</returns>
        private ArrayList AvailableRoles(ArrayList arrAssignedRoles)
        {
            //Declare Variables
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
            //if (arrAvailableRoles.Count > 0)
            //{
            //   foreach (string curRole in arrAssignedRoles)
            //    {
            //        arrAvailableRoles.Remove(curRole);
            //    }
            //}

            return arrAvailableRoles;
        }

        #endregion Methods
    }
}