<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Router.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.Router" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="WikiWrapper clearfix">
    <div class="WikiMenu">
        <asp:PlaceHolder ID="phWikiMenu" runat="server" />
    </div>
    <div class="WikiContent">
        <asp:PlaceHolder id="phWikiContent" runat="server" />
    </div>
</div>