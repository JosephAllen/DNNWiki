<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Search.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.SearchPage"%>
<p>
	<asp:Label id="Label2" runat="server" CssClass="Head"></asp:Label><br />
	<asp:Label id="Label1" runat="server" CssClass="SubHead"></asp:Label>&nbsp;
	<asp:TextBox id="txtTextToSearch" runat="server" Width="104px"></asp:TextBox>
	<asp:LinkButton id="cmdSearch" runat="server" CssClass="CommandButton"></asp:LinkButton></p>
<p>
	<asp:Literal id="HitTable" runat="server"></asp:Literal></p>
