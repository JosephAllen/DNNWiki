<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.Edit" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div id="DnnWiki">
    <asp:Label ID="lblMessage" runat="server" />
    <div class="WikiTable" id="divWikiEdit" runat="server">
        <table border="0">
            <tr>
                <td valign="top" width="400">
                    <dnn:label id="plPageName" CssClass="SubHead" runat="server">
                    </dnn:label>
                    <asp:TextBox ID="txtPageName" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PageNameValidator" runat="server" ControlToValidate="txtPageName"
                        ErrorMessage="You Must Enter a Page Name" SetFocusOnError="True">You Must Enter a Page Name</asp:RequiredFieldValidator>&nbsp;<br />
                    <dnn:SectionHead ID="shSeo" cssclass="NormalBold" runat="server" IsExpanded="false"
                        ResourceKey="shSeo" Section="wikiSeo" />
                    <div id="wikiSeo" runat="server">
                        <dnn:label id="plTitle" CssClass="SubHead" runat="server">
                        </dnn:label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox><br />
                        <dnn:label id="plDescription" CssClass="SubHead" runat="server">
                        </dnn:label>
                        <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" TextMode="multiLine" runat="server"
                            Columns="50"></asp:TextBox><br />
                        <dnn:label id="plKeywords" CssClass="SubHead" runat="server">
                        </dnn:label>
                        <asp:TextBox ID="txtKeywords" CssClass="NormalTextBox" TextMode="multiLine" runat="server"
                            Columns="50"></asp:TextBox><br />
                    </div>
                    <dnn:TextEditor id="teContent" runat="server" Width="600" Height="500">
                    </dnn:TextEditor><br />
                    <dnn:sectionhead id="WikiTextDirections" runat="server" includerule="false" isexpanded="false"
                        section="TextDirections" cssclass="NormalBold">
                    </dnn:sectionhead>
                    <table id="TextDirections" cellspacing="0" cellpadding="0" width="100%" summary="Text Directions Design Table"
                        border="0" runat="server">
                        <tr>
                            <td class="inlineStyle">
                                <span class="normal">
                                    <asp:Literal ID="WikiDirectionsDetails" runat="server"></asp:Literal>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="AllowDiscuss" runat="server" CssClass="NormalBold"></asp:CheckBox>&nbsp;
                    <asp:CheckBox ID="AllowRating" runat="server" CssClass="NormalBold"></asp:CheckBox>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblPageCreationError" CssClass="NormalRed" runat="server"></asp:Label>
        <br />
        <asp:LinkButton ID="cmdSave" runat="server" CssClass="dnnPrimaryAction"></asp:LinkButton>
        <asp:LinkButton ID="cmdSaveAndContinue" runat="server" CssClass="dnnSecondaryAction"></asp:LinkButton>
        <asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="DeleteBtn" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false"></asp:LinkButton>
        <asp:Label ID="DeleteLbl" runat="server"></asp:Label>
    </div>
</div>