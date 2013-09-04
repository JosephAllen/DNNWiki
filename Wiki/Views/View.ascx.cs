using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Modules.Wiki.BusinessObjects;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Modules.Wiki.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Mvp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DotNetNuke.Modules.Wiki.Views
{
    /// -----------------------------------------------------------------------------
    /// <summary> The View class displays the content
    ///
    /// Typically your view control would be used to display content or functionality in your
    /// module.
    ///
    /// View may be the only control you have in your project depending on the complexity of your
    /// module
    ///
    /// Because the control inherits from DNNModule1ModuleBase you have access to any custom
    /// properties defined there, as well as properties from DNN such as PortalId, ModuleId, TabId,
    /// UserId and many more.
    ///
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : WikiModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (UnitOfWork uof = new UnitOfWork())
                {
                    var itemBo = new ItemBO(uof);
                    rptItemList.DataSource = itemBo.GetAll();
                    rptItemList.DataBind();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void rptItemListOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var lnkEdit = e.Item.FindControl("lnkEdit") as HyperLink;
                var lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

                var pnlAdminControls = e.Item.FindControl("pnlAdmin") as Panel;

                var t = (Item)e.Item.DataItem;

                if (IsEditable && lnkDelete != null && lnkEdit != null && pnlAdminControls != null)
                {
                    pnlAdminControls.Visible = true; lnkDelete.CommandArgument = t.ItemId.ToString();
                    lnkDelete.Enabled = lnkDelete.Visible = lnkEdit.Enabled = lnkEdit.Visible = true;

                    lnkEdit.NavigateUrl = EditUrl(string.Empty, string.Empty, "Edit", "tid=" + t.ItemId);

                    ClientAPI.AddButtonConfirm(lnkDelete, Localization.GetString("ConfirmDelete",
                    LocalResourceFile));
                }
                else { pnlAdminControls.Visible = false; }
            }
        }

        public void rptItemListOnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect(EditUrl(string.Empty, string.Empty, "Edit", "tid=" + e.CommandArgument));
            }

            if (e.CommandName == "Delete")
            {
                using (UnitOfWork uof = new UnitOfWork())
                {
                    uof.BeginTransaction();
                    try
                    {
                        var itemBo = new ItemBO(uof);
                        itemBo.Delete(new Item { ItemId = Convert.ToInt32(e.CommandArgument), ModuleId = ModuleId });
                        uof.CommitTransaction();
                    }
                    catch (Exception)
                    {
                        uof.RollbackTransaction();
                    }
                }
            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}