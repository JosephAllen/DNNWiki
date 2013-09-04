<%@ Control Language="vb" AutoEventWireup="false" Codebehind="RecentChanges.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.RecentChanges"%>
<p><asp:Label id="TitleLbl" runat="server" CssClass="Head"></asp:Label></p>
<div id="divHitTable">
	<asp:literal id="HitTable" runat="server" Visible="True"></asp:literal>
</div>
<p>
	|&nbsp;
	<asp:LinkButton id="cmdLast24Hrs" runat="server" Text="Last 24 Hours" CssClass="CommandButton"></asp:LinkButton>
	&nbsp;|&nbsp;
	<asp:LinkButton id="cmdLast7Days" runat="server" Text="Last 7 days" CssClass="CommandButton"></asp:LinkButton>
	&nbsp;|&nbsp;
	<asp:LinkButton id="cmdLastMonth" runat="server" Text="Last Month" CssClass="CommandButton"></asp:LinkButton>
	&nbsp;|</p>
