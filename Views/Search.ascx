<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Search" %>
<p>
    <asp:Label ID="Label2" runat="server" CssClass="Head"></asp:Label><br />
    <asp:Label ID="Label1" runat="server" CssClass="SubHead"></asp:Label>&nbsp;
	<asp:TextBox ID="txtTextToSearch" runat="server" Width="104px"></asp:TextBox>
    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="CommandButton"></asp:LinkButton>
</p>
<p>
    <asp:Literal ID="HitTable" runat="server"></asp:Literal>
</p>