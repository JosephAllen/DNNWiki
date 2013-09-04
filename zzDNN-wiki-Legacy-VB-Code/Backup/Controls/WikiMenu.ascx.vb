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

    Partial Public Class WikiMenu
        Inherits WikiControlBase

        Dim ShowIndex As Boolean

        Dim ShowNav As Boolean

        Private Sub Menu_Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'todo:we shouldn't use session

            If (Me.Session("wiki" + ModuleId.ToString + "ShowIndex") Is Nothing) Then
                Me.Session.Add("wiki" + ModuleId.ToString + "ShowIndex", False)
                ShowIndex = False
            Else
                ShowIndex = CBool(Me.Session("wiki" + ModuleId.ToString + "ShowIndex"))
            End If
            If (Me.Session("wiki" + ModuleId.ToString + "ShowNav") Is Nothing) Then
                Me.Session.Add("wiki" + ModuleId.ToString + "ShowNav", True)
                ShowNav = True
            Else
                ShowNav = CBool(Me.Session("wiki" + ModuleId.ToString + "ShowNav"))
            End If

            If ShowNav Then
                'Me.ImageButton1.AlternateText = Localization.GetString("HideNavigation", LocalResourceFile) ' "Show Navigation"
                'Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/HideNav.gif"
                Me.LinksPanel.Visible = True
            Else
                'Me.ImageButton1.AlternateText = Localization.GetString("ShowNavigation", LocalResourceFile) ' "Show Navigation"
                'Me.ImageButton1.ImageUrl = TemplateSourceDirectory + "/images/ShowNav.gif"
                Me.LinksPanel.Visible = False
            End If

            setURLs()

        End Sub

        Private Sub setURLs()
            Me.HomeBtn.NavigateUrl = Common.Globals.NavigateURL()
            Me.HomeBtn.Text = "<img src=""" + Parent.TemplateSourceDirectory + "/images/Home.gif"" border=""0"" align=""middle"" alt=""" + Localization.GetString("Home", Me.LocalResourceFile) + """ />&nbsp;" + Localization.GetString("Home", Me.LocalResourceFile)
            Me.SearchBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, "", "loc=search")
            Me.SearchBtn.Text = "<img src=""" + Parent.TemplateSourceDirectory + "/images/Search.gif"" border=""0"" align=""middle"" alt=""" + Localization.GetString("Search", Me.LocalResourceFile) + """ />&nbsp;" + Localization.GetString("Search", Me.LocalResourceFile)
            Me.RecChangeBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, "", "loc=recentchanges")
            Me.RecChangeBtn.Text = "<img src=""" + Parent.TemplateSourceDirectory + "/images/RecentChanges.gif"" border=""0"" align=""middle"" alt=""" + Localization.GetString("RecentChanges", Me.LocalResourceFile) + """ />&nbsp;" + Localization.GetString("RecentChanges", Me.LocalResourceFile)

            Me.IndexBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, "", "loc=index")

            Me.IndexBtn.Text = "<img src=""" + Parent.TemplateSourceDirectory + "/images/Index.gif"" border=""0"" align=""middle"" alt=""" + Localization.GetString("Index", Me.LocalResourceFile) + """ />&nbsp;" + Localization.GetString("Index", Me.LocalResourceFile)

        End Sub



    End Class


End Namespace
