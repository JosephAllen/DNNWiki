'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2013
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Modules.Wiki.Entities

Namespace DotNetNuke.Modules.Wiki

    Partial Class Start
        Inherits WikiControlBase
        Protected WithEvents cmdHistory As System.Web.UI.WebControls.Button

        Protected WithEvents RatingSec As UI.UserControls.SectionHeadControl
        Protected WithEvents pageRating As DotNetNuke.Modules.Wiki.PageRatings
        Protected WithEvents ratings As DotNetNuke.Modules.Wiki.Ratings
        Protected WithEvents CommentsSec As UI.UserControls.SectionHeadControl
        Protected WithEvents WikiTextDirections As UI.UserControls.SectionHeadControl


#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub
#End Region

#Region "Form Events"

        Shadows Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If UserId = -1 Then
                UserName = "Anonymous"
            Else
                UserName = Me.UserInfo.Username
            End If

            Me.AddCommentsForm1.EnableNotify = WikiSettings.CommentNotifyUsers

            If Request.IsAuthenticated Then
                If Me.UserInfo.IsSuperUser Or Me.UserInfo.IsInRole(PortalSettings.AdministratorRoleName) Then
                    IsAdmin = True
                End If
            End If


            LoadLocalization()
            Me.AddCommentPane.Visible = False
            Me.Comments2.Visible = True
            Me.AddCommentCommand.Visible = True
            'Me.DeleteCommentCommand.Visible = True

            Me.lblPageTopic.Text = Me.PageTopic.Replace(Me.WikiHomeName, Localization.GetString("Home", Me.RouterResourceFile))

            If Me.Topic.Title.Trim().Length > 0 Then
                Me.lblPageTopic.Text = Topic.Title
            End If

            Me.AddCommentsForm1.ParentId = Topic.TopicID
            CommentCount1.ParentId = Topic.TopicID

            Comments2.IsAdmin = Me.IsAdmin
            Comments2.ParentId = Topic.TopicID

            'CommentsSec.IsExpanded = False
            DisplayTopic()

        End Sub


#End Region

        Private Sub LoadLocalization()
            AddCommentCommand.Text = Localization.GetString("StartAddComment", Me.RouterResourceFile)

            CommentCount1.Text = Localization.GetString("StartCommentCount", Me.RouterResourceFile)
            CommentsSec.Text = Localization.GetString("StartCommentsSection", Me.RouterResourceFile)

            PostCommentLbl.Text = Localization.GetString("StartPostComment", Me.RouterResourceFile)

            RatingSec.Text = Localization.GetString("StartRatingSec.Text", Me.RouterResourceFile)
            

        End Sub

        Private Sub DisplayTopic()


            Me.lblPageContent.Visible = True
            Dim Content As String = ReadTopic()
            Me.lblPageContent.Text = HttpUtility.HtmlDecode(Content).ToString()
            Me.ratingTbl.Visible = Topic.AllowRatings AndAlso WikiSettings.AllowRatings
            Me.RatingSec.Visible = Topic.AllowRatings AndAlso WikiSettings.AllowRatings
            Me.pageRating.Visible = Topic.AllowRatings AndAlso WikiSettings.AllowRatings
            Me.ratings.Visible = Topic.AllowRatings
            Me.AddCommentCommand.Visible = Topic.AllowDiscussions AndAlso WikiSettings.AllowDiscussions
            Me.CommentCount1.Visible = False
            Me.Comments2.Visible = Topic.AllowDiscussions AndAlso WikiSettings.AllowDiscussions
            Me.CommentsSec.Visible = Topic.AllowDiscussions AndAlso WikiSettings.AllowDiscussions
            Me.CommentsTbl.Visible = Topic.AllowDiscussions AndAlso WikiSettings.AllowDiscussions

            Dim p As DotNetNuke.Framework.CDefault
            p = CType(Me.Page, DotNetNuke.Framework.CDefault)

            'set the page title, check for the Topic.Title, Topic.Name, then use PageTopic parameter if all else fails
            If Me.Topic.Title.Trim().Length > 0 Then
                p.Title = p.Title & " > " & Me.Topic.Title
            ElseIf Me.Topic.Name.Trim().Length > 0 Then
                p.Title = p.Title & " > " & Me.Topic.Name
            Else
                p.Title = p.Title & " > " & Me.PageTopic
            End If

            If Not Me.Topic.Description Is Nothing And Not Me.Topic.Description = String.Empty Then
                p.Description = Me.Topic.Description & " " & p.Description

            End If

            If Not Me.Topic.Keywords Is Nothing And Not Me.Topic.Keywords = String.Empty Then
                p.KeyWords = Me.Topic.Keywords & " " & p.KeyWords

            End If


        End Sub

        Private Sub EditPage()
            'Me.chkPageInEditMode.Checked = True
            Me.DisplayTopic()
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If ratings.HasVoted Then
                RatingSec.IsExpanded = False
            End If
            Me.CommentCount1.Visible = False
            'CommentsSec.IsExpanded = False
            If Request.IsAuthenticated Then
                Me.AddCommentsForm1.NameText = UserInfo.DisplayName
                Me.AddCommentsForm1.EnableName = False
                Me.AddCommentsForm1.EmailText = UserInfo.Membership.Email
                Me.AddCommentsForm1.EnableEmail = False

                'Dim lstEmails As List(Of String) = New Entities.CommentsController().GetNotificationEmails(Me.Topic)
                'Me.AddCommentsForm1.CommentText = "Total: " & lstEmails.Count & vbCrLf
                'Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & "-----------" & vbCrLf
                'For Each s As String In lstEmails
                'Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & s & vbCrLf
                'Next

            End If
        End Sub

        Private Sub AddCommentCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCommentCommand.Click
            Me.AddCommentPane.Visible = True
            Me.Comments2.Visible = False
            Me.AddCommentCommand.Visible = False
            CommentsSec.IsExpanded = True

        End Sub

        Private Sub AddCommentsForm1_PostCanceled(ByRef s As Object) Handles AddCommentsForm1.PostCanceled
            Me.AddCommentPane.Visible = False
            Me.Comments2.Visible = True
            Me.AddCommentCommand.Visible = True
        End Sub

        Private Sub AddCommentsForm1_PostSubmitted(ByRef s As Object) Handles AddCommentsForm1.PostSubmitted
            Me.AddCommentPane.Visible = False
            Me.Comments2.Visible = True
            Me.AddCommentCommand.Visible = True
        End Sub
    End Class
End Namespace
