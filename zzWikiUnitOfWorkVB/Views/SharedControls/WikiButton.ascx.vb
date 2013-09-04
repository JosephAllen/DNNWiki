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


    Partial Class WikiButton
        Inherits WikiControlBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            LocalResourceFile = DotNetNuke.Services.Localization.Localization.GetResourceFile(Me, "WikiButton.ascx.resx")

            lnkEdit.Text = Localization.GetString("StartEdit.Text", LocalResourceFile)
            cmdAdd.Text = Localization.GetString("StartAdd", LocalResourceFile)
            txtViewHistory.Text = Localization.GetString("StartViewHistory", LocalResourceFile)

            SetDisplay()

        End Sub

        Private Sub SetDisplay()

            Me.ViewPipe1.Visible = False
            Me.ViewPipe2.Visible = False

            Me.EditPipe.Visible = False

            Me.cmdAdd.Visible = Me.CanEdit
            Me.lnkEdit.Visible = False

            Me.AddPipe.Visible = False
            Me.txtViewHistory.Visible = False


            If (Topic.TopicID >= 0 Or Not Request.QueryString.Item("topic") Is Nothing) Then
                Me.ViewPipe1.Visible = True
                Me.ViewPipe2.Visible = True

                Me.EditPipe.Visible = Me.CanEdit

                Me.cmdAdd.Visible = Me.CanEdit
                Me.lnkEdit.Visible = Me.CanEdit

                Me.AddPipe.Visible = Me.CanEdit
                Me.txtViewHistory.Visible = True
                txtViewHistory.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, "", "loc=TopicHistory", "topic=" & Entities.WikiData.EncodeTitle(PageTopic))
                lnkEdit.NavigateUrl = NavigateURL(TabId, "", "topic=" & WikiData.EncodeTitle(PageTopic) & "&loc=edit")
            End If

            cmdAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "&loc=edit&add=true")


        End Sub

    End Class
End Namespace