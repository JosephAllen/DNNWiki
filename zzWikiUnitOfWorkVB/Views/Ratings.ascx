<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Ratings.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.Ratings"%>
<table width="100%" border="0">
	<tr>
		<td width="60%" align="left" valign="top">
			<asp:Panel Runat="server" ID="pnlCastVote">
				<table id="RatingsTable" Runat="server">
					<tr>
						<td nowrap="nowrap">
							<asp:label id="RatePagelbl" Runat="server" CssClass="NormalBold"></asp:label></td>
						<td>
							<asp:label id="LowRating" Runat="server" CssClass="Normal"></asp:label></td>
						<td>
							<asp:RadioButton id="rating1" Runat="server" GroupName="rating"></asp:RadioButton></td>
						<td>
							<asp:RadioButton id="rating2" Runat="server" GroupName="rating"></asp:RadioButton></td>
						<td>
							<asp:RadioButton id="rating3" Runat="server" GroupName="rating"></asp:RadioButton></td>
						<td>
							<asp:RadioButton id="rating4" Runat="server" GroupName="rating"></asp:RadioButton></td>
						<td>
							<asp:RadioButton id="rating5" Runat="server" GroupName="rating"></asp:RadioButton></td>
						<td class="Normal">
							<asp:label id="HighRating" Runat="server" CssClass="Normal"></asp:label></td>
						<td class="Normal" nowrap="nowrap">&nbsp;&nbsp;|&nbsp;
							<asp:LinkButton id="btnSubmit" Runat="server" CssClass="CommandButton"></asp:LinkButton>&nbsp;|</td>
					</tr>
				</table>
			</asp:Panel>
			<asp:Panel Runat="server" ID="pnlVoteCast" visible="false">
				<asp:Label id="lblVoteCastMessage" Runat="server" CssClass="Normal"></asp:Label>
			</asp:Panel>
		</td>
		<td align="right">
			<table border="0">
				<tr>
					<td align="left" valign="top">
						<asp:Label Runat="server" ID="lblAverageRatingMessage" CssClass="Normal"></asp:Label>&nbsp;
						<asp:Label Runat="server" CssClass="NormalBold" ID="lblAverageRating"></asp:Label>&nbsp;&nbsp;&nbsp;<br />
						<asp:Table id="RatingsGraphTable" Runat="server" Height="50px">
							<asp:TableRow>
								<asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
								<asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
								<asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
								<asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
								<asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell CssClass="normal">1</asp:TableCell>
								<asp:TableCell CssClass="normal">2</asp:TableCell>
								<asp:TableCell CssClass="normal">3</asp:TableCell>
								<asp:TableCell CssClass="normal">4</asp:TableCell>
								<asp:TableCell CssClass="normal">5</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<asp:Label Runat="server" ID="lblRatingCount" CssClass="Normal"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
