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

Namespace DotNetNuke.Modules.Wiki

    Partial Class Router
        Inherits WikiControlBase
        'Implements ISearchable
        'Implements IPortable
        Implements IActionable


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

        Private Sub Router_Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'load the menu on the left
            Dim leftControl As String = "Controls/WikiMenu.ascx"
            Dim mbl As WikiControlBase = CType(TemplateControl.LoadControl(leftControl), WikiControlBase)
            mbl.ModuleConfiguration = ModuleConfiguration
            mbl.ID = System.IO.Path.GetFileNameWithoutExtension(leftControl)
            phWikiMenu.Controls.Add(mbl)

            Dim controlToLoad As String = GetControlString(Request.QueryString("loc"))
            Dim wikiContent As WikiControlBase = CType(LoadControl(controlToLoad), WikiControlBase)
            wikiContent.ModuleConfiguration = ModuleConfiguration
            wikiContent.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad)
            phWikiContent.Controls.Add(wikiContent)


            If (controlToLoad.ToLower <> "edit.ascx") Then
                Dim buttonControlToLoad As String = "Controls/WikiButton.ascx"
                Dim wikiButton As WikiControlBase = CType(LoadControl(buttonControlToLoad), WikiControlBase)

                wikiButton.ModuleConfiguration = ModuleConfiguration
                wikiButton.ID = System.IO.Path.GetFileNameWithoutExtension(buttonControlToLoad)
                phWikiContent.Controls.Add(wikiButton)
            End If

            'print
            Dim objAction As ModuleAction

            For Each objAction In Actions
                If (objAction.CommandName = ModuleActionType.PrintModule) Then
                    objAction.Url += "&topic=" + WikiData.EncodeTitle(PageTopic)
                End If
            Next

        End Sub

        Private Function GetControlString(ByVal loc As String) As String
            If loc Is Nothing Then
                Return "Start.ascx"
            Else
                Select Case (loc.ToLower())
                    Case "start"
                        Return "Start.ascx"
                    Case "edit"
                        Return "Edit.ascx"
                    Case "topichistory"
                        Return "topichistory.ascx"
                    Case "search"
                        Return "search.ascx"
                    Case "recentchanges"
                        Return "recentchanges.ascx"
                    Case "index"
                        Return "controls/index.ascx"
                    Case Else
                        Return "start.ascx"
                End Select
            End If
        End Function



        'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '    If ShowNav Then

        '        Me.ImageButton1.AlternateText = Localization.GetString("ShowNavigation", LocalResourceFile) ' "Show Navigation"
        '        Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/ShowNav.gif"
        '        Me.LinksPanel.Visible = False
        '        ShowNav = False
        '        Me.Session("wiki" + ModuleId.ToString + "ShowNav") = False
        '    Else
        '        Me.ImageButton1.AlternateText = Localization.GetString("HideNavigation", LocalResourceFile) '"Hide Navigation"
        '        Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/HideNav.gif"
        '        Me.LinksPanel.Visible = True
        '        ShowNav = False
        '        Me.Session("wiki" + ModuleId.ToString + "ShowNav") = True
        '    End If
        'End Sub


        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements DotNetNuke.Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New DotNetNuke.Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString("Administration", LocalResourceFile).ToString(), "", "", "", EditUrl("Administration"), False, Security.SecurityAccessLevel.Admin, True, False)
                Return Actions
            End Get
        End Property

    End Class
End Namespace