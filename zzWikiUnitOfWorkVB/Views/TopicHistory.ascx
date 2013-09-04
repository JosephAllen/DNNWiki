<%@ Control Language="vb" AutoEventWireup="false" Codebehind="TopicHistory.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.TopicHistory"%>
<asp:label id="Label1" runat="server" CssClass="Head"></asp:label>
<asp:label id="lblPageTopic" runat="server" CssClass="Head">DotWiki</asp:label>
<asp:label id="lblDateTime" runat="server" CssClass="SubSubHead"></asp:label>
<p><asp:label id="lblPageContent" runat="server" CssClass="Normal"></asp:label>&nbsp;&nbsp;</p>
<p>|<asp:HyperLink id="BackBtn" runat="server" CssClass="CommandButton"></asp:HyperLink>&nbsp;|&nbsp;&nbsp;
	<asp:LinkButton id="cmdRestore" runat="server" CssClass="CommandButton"></asp:LinkButton>&nbsp;
	<asp:Label id="RestoreLbl" runat="server" CssClass="Normal">|<br /></asp:Label></p>
