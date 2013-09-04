<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Index.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.Controls.Index" %>
<p><asp:Label id="TitleLbl" runat="server" CssClass="Head" resourcekey="TitleLbl"></asp:Label></p>

<asp:Panel id="IndexPanel" runat="server" CssClass="WikiIndexPanel">
    	<asp:Literal id="IndexList" runat="server"></asp:Literal>	
</asp:Panel>