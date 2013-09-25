<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentChanges.ascx.cs" Inherits="DotNetNuke.Wiki.Views.RecentChanges" %>
<p>
    <asp:Label ID="TitleLbl" runat="server" CssClass="Head"></asp:Label>
</p>
<div id="divHitTable">
    <asp:Literal ID="HitTable" runat="server" Visible="True"></asp:Literal>
</div>
<p>
    |&nbsp;

    <asp:LinkButton ID="cmdLast24Hrs" OnClick="cmdLast24Hrs_Click" runat="server" Text="Last 24 Hours" CssClass="CommandButton"></asp:LinkButton>
    &nbsp;|&nbsp;

    <asp:LinkButton ID="cmdLast7Days" OnClick="cmdLast7Days_Click" runat="server" Text="Last 7 days" CssClass="CommandButton"></asp:LinkButton>
    &nbsp;|&nbsp;

    <asp:LinkButton ID="cmdLastMonth" OnClick="cmdLastMonth_Click" runat="server" Text="Last Month" CssClass="CommandButton"></asp:LinkButton>
    &nbsp;|
</p>