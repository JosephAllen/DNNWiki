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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Modules.Wiki.Entities


Namespace DotNetNuke.Modules.Wiki.Controls

    Partial Public Class Index
        Inherits WikiControlBase


        Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            DisplayIndex()

        End Sub

        Private Sub DisplayIndex()
            Me.Session("wiki" + ModuleId.ToString + "ShowIndex") = True
            Dim ts As ArrayList = GetIndex()
            Dim TableTxt As New System.Text.StringBuilder("")
            'Dim TopicTable As String
            Dim t As Entities.TopicInfo
            Dim i As Integer
            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=""CommandButton"" href=""")
            TableTxt.Append(HomeURL + """><img src=""")
            TableTxt.Append(Parent.TemplateSourceDirectory)
            TableTxt.Append("/images/Home.gif"" border=""0"" align=""middle"" alt=""" + Localization.GetString("Home", Me.LocalResourceFile) + """ />&nbsp;")
            TableTxt.Append(Localization.GetString("Home", Me.LocalResourceFile))
            TableTxt.Append("</a><br />")
            If Not ts Is Nothing Then
                If ts.Count > 0 Then
                    For i = 0 To ts.Count - 1
                        t = CType(ts(i), Entities.TopicInfo)
                        If t.Name <> WikiHomeName Then
                            TableTxt.Append("&nbsp;&nbsp;&nbsp<a class=""CommandButton"" href=""")
                            TableTxt.Append(DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, String.Empty, "topic=" & Entities.WikiData.EncodeTitle(t.Name)))
                            TableTxt.Append("""><img src=""")
                            TableTxt.Append(Parent.TemplateSourceDirectory)
                            TableTxt.Append("/images/Page.gif"" border=""0"" align=""middle""  alt=""" + Entities.WikiData.EncodeTitle(t.Name) + """ />&nbsp;")
                            TableTxt.Append(t.Name)
                            TableTxt.Append("</a><br />")
                        End If
                    Next i
                End If
            End If
            TableTxt.Append("")
            Me.IndexList.Text = TableTxt.ToString()
            Me.IndexList.Visible = True
        End Sub

    End Class


End Namespace
