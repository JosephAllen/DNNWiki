'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2012
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

Imports System.Web.UI
Imports System.ComponentModel

Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Wiki.Comments

    <DefaultProperty("ID"), ToolboxData("<{0}:CommentCount runat=server></{0}:CommentCount>")> _
    Public Class CommentCount
        Inherits System.Web.UI.WebControls.Label
        Private CC As New Entities.CommentsController

        Private sharedResources = Me.TemplateSourceDirectory & "/" & "DesktopModules/Wiki/App_LocalResources/SharedResources.resx"

        <Description("The id of the parent (page) the comment count is for."), Category("Data")> _
        Public Property ParentId() As Integer
            Get
                Return Me._parentId
            End Get
            Set(ByVal Value As Integer)
                Me._parentId = Value
            End Set
        End Property
        <Description("The text for the link. {0} will be replaced with the number of comments."), Category("Appearance")> _
        Public Shadows Property Text() As String
            Get
                Return Me._text
            End Get
            Set(ByVal Value As String)
                Me._text = Value
            End Set
        End Property
        Private _linkClass As String

        Private _text As String
        Private _parentId As Integer

        Protected Overloads Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            Dim commentCount As Integer = CC.GetCommentCount(Me._parentId)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, Me.CssClass)
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            If commentCount > 0 Then
                '                writer.Write(String.Format(Me._text, commentCount))

                If Me._text Is Nothing Then
                    writer.Write(String.Format(Localization.GetString("FeedBack.Text", sharedResources), commentCount))
                Else
                    writer.Write(String.Format(Me._text, commentCount))
                End If
            Else

                writer.Write(Localization.GetString("NoComments.Text", sharedResources))
            End If
            writer.RenderEndTag()
        End Sub
    End Class
End Namespace
