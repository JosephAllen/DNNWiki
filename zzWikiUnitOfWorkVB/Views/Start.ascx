<%@ control Language="vb" AutoEventWireup="false" Codebehind="Start.ascx.vb" Inherits="DotNetNuke.Modules.Wiki.Start" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="wiki" TagName="PageRating" Src="PageRatings.ascx" %>
<%@ Register TagPrefix="wiki" TagName="Ratings" Src="Ratings.ascx" %>
<%@ Register Tagprefix="comment" Namespace="DotNetNuke.Modules.Wiki.Comments" Assembly="DotNetnuke.Modules.Wiki" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div id="DnnWiki">
    <div class="WikiHeader clearfix">
        <div class="WikiTitle">
            <h1 class="Head"><asp:label id="lblPageTopic" runat="server"></asp:label></h1>
        </div>
        <div class="WikiRating">
            <wiki:pagerating id="pageRating" runat="server"></wiki:pagerating><br />
			<comment:CommentCount id="CommentCount1" runat="server" cssClass="Normal"></comment:CommentCount>
        </div>
    </div>

	<div class="Normal">
		<asp:Literal id="lblPageContent" runat="server" EnableViewState="False"></asp:Literal>
	</div>
	<br />
	<br />
	<dnn:sectionhead id="RatingSec" runat="server" includerule="false" isexpanded="true" section="ratingTbl"
		cssclass="NormalBold"></dnn:sectionhead>
	<table id="ratingTbl" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
		<tr>
			<td>
				<wiki:Ratings id="ratings" runat="server"></wiki:Ratings></td>
		</tr>
	</table>
	<dnn:sectionhead id="CommentsSec" runat="server" includerule="false" isexpanded="true" section="CommentsTbl"
		cssclass="NormalBold"></dnn:sectionhead>
	<table id="CommentsTbl" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
		<tr>
			<td>
				<asp:LinkButton id="AddCommentCommand" runat="server" CssClass="CommandButton"></asp:LinkButton><br />
				
				<comment:comments id="Comments2" runat="server" parentid="1" dateformat="dd-MM-yyyy HH:mm:ss" cacheitems="true"
					hideemailaddress="False" cellspacing="5" hideemailurl="/getemail.aspx?commentid={0}" useoledb="True" 
					CssClass="Wiki_CommentsTable"></comment:comments>
					<br />
				<asp:Panel id="AddCommentPane" Runat="server" Visible="false">
					<asp:Label id="PostCommentLbl" CssClass="SubHead" Runat="server"></asp:Label>
					<br />
					<comment:addcommentsform id="AddCommentsForm1" runat="server" checkemail="True" checkname="True" checkcomments="False"></comment:addcommentsform>
				</asp:Panel></td>
		</tr>
	</table>

</div>