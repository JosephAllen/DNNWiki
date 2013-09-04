<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WikiButton.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.WikiButton" %>

	<div class="WikiButtons CommandButton">
		<asp:literal id="AddPipe" runat="server">&nbsp;|&nbsp;</asp:literal>
		<asp:HyperLink id="cmdAdd" runat="server" CssClass="CommandButton"></asp:HyperLink>
		<asp:literal id="EditPipe" runat="server">&nbsp;|&nbsp;</asp:literal>
		<asp:HyperLink ID="lnkEdit" runat="server"  CssClass="CommandButton"/>
		<asp:literal id="ViewPipe1" runat="server">&nbsp;|&nbsp;</asp:literal>
		<asp:hyperlink id="txtViewHistory" runat="server" rel="noindex,nofollow" CssClass="CommandButton"></asp:hyperlink>
		<asp:literal id="ViewPipe2" runat="server">&nbsp;|</asp:literal>
	</div>

