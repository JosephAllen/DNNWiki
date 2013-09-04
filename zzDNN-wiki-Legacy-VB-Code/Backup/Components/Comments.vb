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

Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Data
Imports System.Text
Imports DotNetNuke.Services.Localization
Imports System.Globalization

Namespace DotNetNuke.Modules.Wiki.Comments

    <DefaultProperty("ID"), ToolboxData("<{0}:Comments runat=server></{0}:Comments>")> _
    Public Class Comments
        Inherits System.Web.UI.WebControls.Table


        Private sharedResources = Me.TemplateSourceDirectory & "/" & "DesktopModules/Wiki/App_LocalResources/SharedResources.resx"
        Private controller As New Entities.CommentsController

        Public IsAdmin As Boolean = False

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            ''Check for delete flag on query string via commentid (cid)
            If Context.Request.QueryString("cid") <> Nothing And IsAdmin Then

                Dim commentId As Integer = CType(Context.Request.QueryString("cid"), Integer)
                controller.DeleteComment(Convert.ToInt32(commentId))
                Me.Context.Cache.Remove("WikiComments" + _parentId.ToString)

                Me.Context.Response.Redirect(ReconstructQueryStringWithoutId())
            End If

        End Sub

        Private Function ReconstructQueryStringWithoutId() As String


            'TODO: make this use NavigateURL
            Dim url As New StringBuilder(128)

            url.Append(Context.Request.Url.Scheme)
            url.Append("://")
            url.Append(Context.Request.Url.Authority)
            url.Append(Context.Request.ApplicationPath)
            url.Append("/Default.aspx?")
            Dim i As Integer
            For i = 0 To Context.Request.QueryString.Keys.Count - 1
                Dim key As String = Context.Request.QueryString.Keys(i).ToString()
                If key <> "cid" Then
                    url.Append(key)
                    url.Append("=")
                    url.Append(Context.Request.QueryString.Item(i))
                    url.Append("&")
                End If
            Next
            'strip last &
            url.Remove(url.Length - 1, 1)

            Return url.ToString()

        End Function

        Protected Overloads Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)

            If Not (Me.Context Is Nothing) Then
                Dim dataTable As DataTable = Nothing
                If Me._cacheItems Then
                    If Not (Me.Context.Cache("WikiComments" + Me._parentId.ToString) Is Nothing) Then
                        dataTable = CType(Me.Context.Cache("WikiComments" + Me._parentId.ToString), DataTable)
                    Else
                        dataTable = controller.GetCommentsByParent(Me._parentId)
                        Me.Context.Cache.Insert("WikiComments" + Me._parentId.ToString, dataTable)
                    End If
                Else
                    dataTable = controller.GetCommentsByParent(Me._parentId)
                End If

                If Not (dataTable Is Nothing) Then
                    If dataTable.Rows.Count > 0 Then
                        For Each dataRow As DataRow In dataTable.Rows
                            Me.renderRow(writer, CType(dataRow("CommentId"), Integer), CType(dataRow("Name"), String), CType(dataRow("Email"), String), CType(dataRow("Comment"), String), CType(dataRow("Datetime"), DateTime))
                        Next
                    Else
                        CssClass = "WikiTable"
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr)
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
                        writer.RenderBeginTag(HtmlTextWriterTag.Td)

                        writer.Write(Localization.GetString("NoComments.Text", sharedResources))
                        Dim breakcount2 = _breakCount
                        _breakCount = 0
                        writer.RenderEndTag()

                        Me.RenderEndTag(writer)
                        _breakCount = breakcount2
                    End If
                Else
                    Me.renderRow(writer, 1, Localization.GetString("ExampleName.Text", sharedResources), Localization.GetString("ExampleEmail.Text", sharedResources), Localization.GetString("ExampleComments.Text", sharedResources), DateTime.Now)
                End If
            End If
        End Sub

        Private Sub renderRow(ByVal writer As HtmlTextWriter, ByVal commentId As Integer, ByVal name As String, ByVal email As String, ByVal comments As String, ByVal postDate As DateTime)
            'Delete the comments
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)

            If Me._hideEmailAddress Then
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
                writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format(Me._hideEmailUrl, commentId))
                writer.RenderBeginTag(HtmlTextWriterTag.A)
                writer.Write(name)
                writer.RenderEndTag()
            Else
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold")
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "mailto:" + email)
                writer.RenderBeginTag(HtmlTextWriterTag.A)
                writer.Write(name)
                writer.RenderEndTag()
            End If

            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            comments = System.Web.HttpUtility.HtmlDecode(comments)
            comments = comments.Replace("" & Microsoft.VisualBasic.Chr(10) & "", "<br />")
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(comments)

            writer.RenderEndTag()
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            If isValidDateFormat(Me._dateFormat) Then
                writer.Write(Localization.GetString("PostedAt", sharedResources) + " " + postDate.ToString(Me._dateFormat))
            Else
                writer.Write(Localization.GetString("PostedAt", sharedResources) + " " + postDate.ToString(CultureInfo.CurrentCulture))
            End If
            writer.RenderEndTag()
            writer.RenderEndTag()

            If IsAdmin Then
                'add delete link here.
                writer.RenderBeginTag(HtmlTextWriterTag.Tr)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal")
                writer.RenderBeginTag(HtmlTextWriterTag.Td)
                writer.AddAttribute(HtmlTextWriterAttribute.Href, BuildDeleteQueryString(commentId))
                writer.RenderBeginTag(HtmlTextWriterTag.A)

                'writer.Write("Delete Comment")
                writer.Write(Localization.GetString("DeleteComment", sharedResources))
                writer.RenderEndTag()
                writer.RenderEndTag()
                writer.RenderEndTag()
            End If

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write("<hr align=""left"" width=""90%"" size=""1"" noshade=""noshade"" />")
            writer.RenderEndTag()
            writer.RenderEndTag()
            'Me.RenderEndTag(writer)
            'Me.RenderBeginTag(writer)

        End Sub

        Private Function BuildDeleteQueryString(ByVal commentId As Integer) As String

            'Build correct url
            Dim href As String = Context.Request.Url.ToString()
            href = href + "&cid=" + commentId.ToString()

            Return href

        End Function

        Private Function isValidDateFormat(ByVal format As String) As Boolean
            Try
                Dim f As String = DateTime.Now.ToString(format)
                Return True
            Catch
                Return False
            End Try
        End Function

        Public Overloads Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)

            'writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Me.CellSpacing.ToString)
            'writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Me.CellPadding.ToString)
            'writer.AddAttribute(HtmlTextWriterAttribute.Width, Me.Width.ToString)
            'writer.AddAttribute(HtmlTextWriterAttribute.Height, Me.Height.ToString)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, Me.CssClass.ToString)
            'writer.AddAttribute(HtmlTextWriterAttribute.Bgcolor, ColorTranslator.ToHtml(Me.BackColor))
            'writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            'writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Me.BorderStyle.ToString)
            'writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            'writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(Me.BorderColor))
            writer.RenderBeginTag(Me.TagName)

        End Sub

        Public Overloads Overrides Sub RenderEndTag(ByVal writer As HtmlTextWriter)
            MyBase.RenderEndTag(writer)
            If Not (Me.Context Is Nothing) Then
                Dim i As Integer = 0
                While i < Me._breakCount
                    writer.Write("<br />")
                    writer.WriteLine("")
                    System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
                End While
            End If
        End Sub

        Public Shared Sub WriteErrors(ByVal writer As HtmlTextWriter, ByVal message As String)

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed")
            'writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial,helvetica")
            'writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10pt")
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            writer.Write(message)
            writer.RenderEndTag()
            writer.RenderBeginTag(HtmlTextWriterTag.Br)
            writer.RenderEndTag()

        End Sub

#Region "Properties"

        <Browsable(False)> _
        Public Overloads Overrides ReadOnly Property Rows() As TableRowCollection
            Get
                Return MyBase.Rows
            End Get
        End Property

        <Browsable(False)> _
        Public Overloads Overrides Property BackImageUrl() As String
            Get
                Return MyBase.BackImageUrl
            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Protected Overloads Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
            Get
                Return HtmlTextWriterTag.Table
            End Get
        End Property

        Private _parentId As Integer
        <Description("The id of the parent (page) the comments are for."), Category("Data")> _
        Public Property ParentId() As Integer
            Get
                Return Me._parentId
            End Get
            Set(ByVal Value As Integer)
                Me._parentId = Value
            End Set
        End Property

        Private _hideEmailAddress As Boolean
        <Description("Whether to suppress displaying the email address. This option should be used in conjunction with the HideEmailUrl property."), Category("Behaviour")> _
        Public Property HideEmailAddress() As Boolean
            Get
                Return Me._hideEmailAddress
            End Get
            Set(ByVal Value As Boolean)
                Me._hideEmailAddress = Value
            End Set
        End Property


        Private _hideEmailUrl As String = "http://localhost/getemail.aspx?commentid={0}"
        <Description("The url that the email address will point to. This enables you to create a page that will show the email address (after a turing test is performed), to stop the email address being 'spam harvested'."), Category("Behaviour")> _
        Public Property HideEmailUrl() As String
            Get
                Return Me._hideEmailUrl
            End Get
            Set(ByVal Value As String)
                Me._hideEmailUrl = Value
            End Set
        End Property

        Private _breakCount As Integer = 1
        <Description("The number of breaks (<br />) tags between each table."), Category("Appearance")> _
        Public Property BreakCount() As Integer
            Get
                Return Me._breakCount
            End Get
            Set(ByVal Value As Integer)
                Me._breakCount = Value
            End Set
        End Property

        Private _cacheItems As Boolean
        <Description("Caches the comments indefinitely using ASP.NET's caching mechanism. The cache is cleared when a new item is added, but NOT when a comment is deleted in the Manager application."), Category("Behaviour")> _
        Public Property CacheItems() As Boolean
            Get
                Return Me._cacheItems
            End Get
            Set(ByVal Value As Boolean)
                Me._cacheItems = Value
            End Set
        End Property

        'Private _dateFormat As String = "dd/MM/yyyy HH:mm"
        'TODO: create a module setting for the date format
        Private _dateFormat As String = "dd/MM/yyyy HH:mm"
        <Description("The format that the date the comment was posted displays in. See the DateTimeFormatInfo for details of the tokens available."), Category("Behaviour")> _
        Public Property DateFormat() As String
            Get
                Return Me._dateFormat
            End Get
            Set(ByVal Value As String)
                Me._dateFormat = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
