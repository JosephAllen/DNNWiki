using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Wiki.BusinessObjects;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Modules.Wiki.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Mvp;

/*
' Copyright (c) 2013 DotNetNuke
' http://www.dotnetnuke.com
' All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using System;

namespace DotNetNuke.Modules.Wiki.Views
{
    /// -----------------------------------------------------------------------------
    /// <summary> The Edit class is used to manage content
    ///
    /// Typically your edit control would be used to create new content, or edit existing content
    /// within your module. The ControlKey for this control is "Edit", and is defined in the
    /// manifest (.dnn) file.
    ///
    /// Because the control inherits from DNNModule1ModuleBase you have access to any custom
    /// properties defined there, as well as properties from DNN such as PortalId, ModuleId, TabId,
    /// UserId and many more.
    ///
    /// </summary>
    /// -----------------------------------------------------------------------------

    public partial class Edit : WikiModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    //get a list of users to assign the user to the Object
                    ddlAssignedUser.DataSource = UserController.GetUsers(PortalId);
                    ddlAssignedUser.DataTextField = "Username";
                    ddlAssignedUser.DataValueField = "UserId";
                    ddlAssignedUser.DataBind();

                    //check if we have an ID passed in via a querystring parameter, if so, load that item to edit.
                    //ItemId is defined in the ItemModuleBase.cs file

                    if (ItemId > 0)
                    {
                        using (UnitOfWork uof = new UnitOfWork())
                        {
                            var tc = new ItemBO(uof);

                            var t = tc.Get(ItemId);
                            if (t != null)
                            {
                                txtName.Text = t.ItemName;
                                txtDescription.Text = t.ItemDescription;
                                ddlAssignedUser.Items.FindByValue(t.AssignedUserId.ToString()).Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                var t = new Item();
                var tc = new ItemBO(uof);

                if (ItemId > 0)
                {
                    t = tc.Get(ItemId);
                    t.ItemName = txtName.Text.Trim();
                    t.ItemDescription = txtDescription.Text.Trim();
                    t.LastModifiedByUserId = UserId;
                    t.LastModifiedOnDate = DateTime.Now;
                    t.AssignedUserId = Convert.ToInt32(ddlAssignedUser.SelectedValue);
                }
                else
                {
                    t = new Item()
                    {
                        AssignedUserId = Convert.ToInt32(ddlAssignedUser.SelectedValue),
                        CreatedByUserId = UserId,
                        CreatedOnDate = DateTime.Now,
                        ItemName = txtName.Text.Trim(),
                        ItemDescription = txtDescription.Text.Trim(),
                    };
                }

                t.LastModifiedOnDate = DateTime.Now;
                t.LastModifiedByUserId = UserId;
                t.ModuleId = ModuleId;

                if (t.ItemId > 0)
                {
                    tc.Update(t);
                }
                else
                {
                    tc.Add(t);
                }
            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}