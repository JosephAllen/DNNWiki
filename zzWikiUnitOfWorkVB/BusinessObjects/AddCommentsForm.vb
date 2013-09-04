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


Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Wiki.Comments

    Public Delegate Sub CancelHandler(ByRef s As Object)
    Public Delegate Sub SubmitHandler(ByRef s As Object)

    <ToolboxData("<{0}:AddCommentsForm runat=server></{0}:AddCommentsForm>")> _
    Public Class AddCommentsForm
        Inherits System.Web.UI.WebControls.WebControl
        Protected WithEvents Name As System.Web.UI.WebControls.TextBox
        Protected WithEvents Email As System.Web.UI.WebControls.TextBox
        Protected WithEvents Comment As System.Web.UI.WebControls.TextBox
        Protected WithEvents SubmitButton As System.Web.UI.WebControls.LinkButton
        Protected WithEvents CancelButton As System.Web.UI.WebControls.LinkButton
        Protected WithEvents LblParentID As System.Web.UI.WebControls.Label
        Protected WithEvents SubscribeToNotifications As System.Web.UI.WebControls.CheckBox
        Public Event PostCanceled As CancelHandler
        Public Event PostSubmitted As SubmitHandler
        Private controller As New Entities.CommentsController
        Private Success As Boolean = True

        Private sharedResources = Me.TemplateSourceDirectory & "/" & "DesktopModules/Wiki/App_LocalResources/SharedResources.resx"


        Protected Overloads Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                Return HtmlTextWriterTag.Table
            End Get
        End Property

        <Description("The id of the parent (page) the comment is being added to."), Category("Data")> _
        Public Property ParentId() As Integer
            Get
                Return Convert.ToInt32(Me.LblParentID.Text)
            End Get
            Set(ByVal Value As Integer)
                Me.LblParentID.Text = Value.ToString
            End Set
        End Property

        <Description("Whether to check (clientside) the email address is valid before the user can submit the form."), Category("Behaviour")> _
        Public Property CheckEmail() As Boolean
            Get
                Return Me._checkEmail
            End Get
            Set(ByVal Value As Boolean)
                Me._checkEmail = Value
            End Set
        End Property

        <Description("Whether to check if the name field is empty before submitting."), Category("Behaviour")> _
        Public Property CheckName() As Boolean
            Get
                Return Me._checkName
            End Get
            Set(ByVal Value As Boolean)
                Me._checkName = Value
            End Set
        End Property

        <Description("Whether to check if the comments field is empty before submitting."), Category("Behaviour")> _
        Public Property CheckComments() As Boolean
            Get
                Return Me._checkComments
            End Get
            Set(ByVal Value As Boolean)
                Me._checkComments = Value
            End Set
        End Property

        <Description("The maximum length (in characters) the comment can be. Enter 0 for unlimited length."), Category("Behaviour")> _
        Public Property CommentsMaxLength() As Integer
            Get
                Return Me._commentsMaxLength
            End Get
            Set(ByVal Value As Integer)
                Me._commentsMaxLength = Value
            End Set
        End Property

        Public Property EnableComment() As Boolean
            Get
                Return Me.Comment.Enabled
            End Get
            Set(ByVal Value As Boolean)
                Me.Comment.Enabled = Value
            End Set
        End Property
        Public Property EnableName() As Boolean
            Get
                Return Me.Name.Enabled
            End Get
            Set(ByVal Value As Boolean)
                Me.Name.Enabled = Value
            End Set
        End Property
        Public Property EnableEmail() As Boolean
            Get
                Return Me.Email.Enabled
            End Get
            Set(ByVal Value As Boolean)
                Me.Email.Enabled = Value
            End Set
        End Property

        Public Property EmailText() As String
            Get
                Return Me.Email.Text
            End Get
            Set(ByVal Value As String)
                Me.Email.Text = Value
            End Set
        End Property
        Public Property NameText() As String
            Get
                Return Me.Name.Text
            End Get
            Set(ByVal Value As String)
                Me.Name.Text = Value
            End Set
        End Property
        Public Property CommentText() As String
            Get
                Return Me.Comment.Text
            End Get
            Set(ByVal Value As String)
                Me.Comment.Text = Value
            End Set
        End Property
        Public Property EnableNotify() As Boolean
            Get
                Return Me.SubscribeToNotifications.Visible
            End Get
            Set(ByVal Value As Boolean)
                Me.SubscribeToNotifications.Visible = Value
            End Set
        End Property
        Private _parentId As Integer
        Private _checkComments As Boolean = True
        Private _checkName As Boolean = True
        Private _checkEmail As Boolean = True
        Private _commentsMaxLength As Integer = 500

        Public Overloads Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)
            If Me._checkComments OrElse Me._checkEmail OrElse Me._checkName OrElse Me._commentsMaxLength > 0 Then
                writer.WriteLine("<script language=""JavaScript"">")
                writer.WriteLine("function wikiFormCheck(form)")
                writer.WriteLine("{")
                Dim clause As String = ""
                If Me._checkName Then
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "if ( form." & Name.ClientID & ".value == """" )")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "{")

                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "alert(""" & Localization.GetString("EnterAName.Text", sharedResources) & """);")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "return false;")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "}")
                    clause = "else "
                End If
                If Me._checkEmail Then
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & clause & "if ( form." & Email.ClientID & ".value == """" )")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "{")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "alert(""" & Localization.GetString("EnterAnEmailAddress.Text", sharedResources) & """);")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "return false;")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "}")
                    clause = "else "
                End If
                If Me._checkComments Then
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & clause & "if ( form." & Comment.ClientID & ".value == """" )")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "{")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "alert(""" & Localization.GetString("EnterSomeComments.Text", sharedResources) & """);")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "return false;")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "}")
                    clause = "else "
                End If
                If Me._commentsMaxLength > 0 Then
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & clause & "if ( form." & Comment.ClientID & ".value.length > " & Me._commentsMaxLength & " )")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "{")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "alert(""" & String.Format(Localization.GetString("EnterAName.Text", sharedResources), _commentsMaxLength) & """);")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "" & Microsoft.VisualBasic.Chr(9) & "return false;")
                    writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "}")
                End If
                writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "")
                writer.WriteLine("" & Microsoft.VisualBasic.Chr(9) & "return true;")
                writer.WriteLine("}")
                writer.WriteLine("</script>")
            End If
            'MyBase.RenderBeginTag(writer)
        End Sub

        Protected Overloads Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            MyBase.RenderBeginTag(writer)
            If Me._checkComments OrElse Me._checkEmail OrElse Me._checkName Then
                SubmitButton.Attributes.Add(HtmlTextWriterAttribute.Onclick, "return wikiFormCheck(this.form)")
            End If
            If Not Success Then
                writer.RenderBeginTag(HtmlTextWriterTag.Tr)
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed")
                writer.RenderBeginTag(HtmlTextWriterTag.Span)
                writer.Write(Localization.GetString("Failed.Text", sharedResources))
                writer.RenderEndTag()
                writer.RenderEndTag()
                writer.RenderEndTag()
            End If
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            writer.Write(Localization.GetString("Name", sharedResources))
            writer.RenderBeginTag(HtmlTextWriterTag.Br)
            Name.RenderControl(writer)
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()
            'writer.RenderEndTag()

            'writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            writer.Write(Localization.GetString("Email", sharedResources))
            writer.RenderBeginTag(HtmlTextWriterTag.Br)
            Email.RenderControl(writer)
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")

            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            writer.Write(Localization.GetString("Comments", sharedResources))
            writer.RenderBeginTag(HtmlTextWriterTag.Br)
            Comment.RenderControl(writer)
            writer.RenderBeginTag(HtmlTextWriterTag.Br)
            SubscribeToNotifications.RenderControl(writer)

            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write("&nbsp|&nbsp")
            SubmitButton.RenderControl(writer)
            writer.Write("&nbsp|&nbsp")
            CancelButton.RenderControl(writer)
            writer.Write("&nbsp|&nbsp")
            writer.RenderEndTag()
            writer.RenderEndTag()
        End Sub
        Private Sub AddCommentsForm_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init
            Name = New System.Web.UI.WebControls.TextBox
            Name.ID = "Name"
            Name.EnableViewState = True
            Name.CssClass = "NormalTextBox"
            Email = New System.Web.UI.WebControls.TextBox
            Email.ID = "Email"
            Email.EnableViewState = True
            Email.CssClass = "NormalTextBox"
            Comment = New System.Web.UI.WebControls.TextBox
            Comment.ID = "Comment"
            Comment.EnableViewState = True
            Comment.CssClass = "NormalTextBox"
            Comment.TextMode = TextBoxMode.MultiLine
            Comment.Width = New System.Web.UI.WebControls.Unit(350)
            Comment.Height = New System.Web.UI.WebControls.Unit(50)
            Comment.MaxLength = CommentsMaxLength
            SubmitButton = New System.Web.UI.WebControls.LinkButton
            SubmitButton.CssClass = "CommandButton"
            SubmitButton.Text = Localization.GetString("PostComment", sharedResources) '"Post Comment"
            CancelButton = New System.Web.UI.WebControls.LinkButton
            CancelButton.CssClass = "CommandButton"
            CancelButton.Text = Localization.GetString("Cancel", sharedResources) '"Cancel"
            LblParentID = New System.Web.UI.WebControls.Label
            LblParentID.ID = "CurrParent"
            LblParentID.Visible = False
            LblParentID.EnableViewState = True

            SubscribeToNotifications = New System.Web.UI.WebControls.CheckBox
            SubscribeToNotifications.ID = "EmailNotify"
            SubscribeToNotifications.Text = Localization.GetString("CommentsNotification", sharedResources)

            'If EnableNotify = False Then SubscribeToNotifications.Visible = False Else SubscribeToNotifications.Visible = True

            Me.Controls.Add(Name)
            Me.Controls.Add(Email)
            Me.Controls.Add(Comment)
            Me.Controls.Add(SubmitButton)
            Me.Controls.Add(CancelButton)
            Me.Controls.Add(LblParentID)
            Me.Controls.Add(SubscribeToNotifications)
        End Sub

        Private Sub SubmitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubmitButton.Click

            Dim CommentText As String = Comment.Text
            Dim objSec As New DotNetNuke.Security.PortalSecurity


            If CommentText.Length > Me.CommentsMaxLength Then CommentText = CommentText.Substring(0, Me.CommentsMaxLength)
            '4.8.3 has better control for NoMarkup
            Success = controller.AddComment(Me.ParentId, objSec.InputFilter(Name.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup), objSec.InputFilter(Email.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup), objSec.InputFilter(CommentText, Security.PortalSecurity.FilterFlag.NoMarkup), objSec.InputFilter(Me.Context.Request.ServerVariables("REMOTE_ADDR"), DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup), SubscribeToNotifications.Checked)

            If Success Then
                Name.Text = ""
                Email.Text = ""
                Comment.Text = ""
                Me.Context.Cache.Remove("WikiComments" + Me.ParentId.ToString)
                RaiseEvent PostSubmitted(Me)
            End If
        End Sub

        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
            Name.Text = ""
            Email.Text = ""
            Comment.Text = ""
            RaiseEvent PostCanceled(Me)
        End Sub
    End Class
End Namespace
