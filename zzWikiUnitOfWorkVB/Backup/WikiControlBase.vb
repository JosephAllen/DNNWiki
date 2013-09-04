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


Imports DotNetNuke.Services.Localization
Imports System.Globalization
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Wiki
    Public Class WikiControlBase
        Inherits PortalModuleBase

        Public UserName As String
        Public FirstName As String
        Public LastName As String

        Public IsAdmin As Boolean = False
        Public PageTopic As String
        Public PageTitle As String
        Public TopicID As Integer

        Public Topic As Entities.TopicInfo
        Protected mTC As Entities.TopicController
        Public THC As New Entities.TopicHistoryController
        Public HomeURL As String
        Public WikiSettings As Entities.SettingsInfo

        Public CanEdit As Boolean = False
        Public ReadOnly WikiHomeName As String = "WikiHomePage"
        Private mModule As DotNetNuke.Entities.Modules.PortalModuleBase

        Public myPage As WikiControlBase


        Public ReadOnly Property TC() As Entities.TopicController
            Get
                If mTC Is Nothing Then
                    mTC = New Entities.TopicController(Me.Cache)
                End If
                Return mTC
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'congfigure the URL to the home page (the wiki without any parameters)
            HomeURL = DotNetNuke.Common.NavigateURL()

            If Me.Request.QueryString.Item("topic") Is Nothing Then
                If Me.Request.QueryString.Item("add") Is Nothing And Me.Request.QueryString.Item("loc") Is Nothing Then
                    PageTopic = WikiHomeName
                Else
                    PageTopic = ""
                End If
            Else
                PageTopic = Entities.WikiData.DecodeTitle(Me.Request.QueryString.Item("topic").ToString())
            End If

            If WikiSettings Is Nothing Then
                Dim WikiController As New Entities.SettingsController
                WikiSettings = WikiController.GetByModuleID(ModuleId)
                If WikiSettings Is Nothing Then
                    WikiSettings = New Entities.SettingsInfo
                    WikiSettings.ContentEditorRoles = "UseDNNSettings"
                End If
            End If


            If WikiSettings.ContentEditorRoles = "UseDNNSettings" Then
                CanEdit = Me.IsEditable
            Else
                If Request.IsAuthenticated Then
                    If Me.UserInfo.IsSuperUser Then
                        CanEdit = True
                        IsAdmin = True
                    ElseIf WikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1 Then
                        CanEdit = True
                    Else
                        Dim Roles As String() = WikiSettings.ContentEditorRoles.Split("|")(0).Split(";")
                        For Each role As String In Roles
                            If UserInfo.IsInRole(role) Then
                                CanEdit = True
                                Exit For
                            End If
                        Next
                    End If
                Else
                    If (WikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) Or (WikiSettings.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleUnauthUserName + ";") > -1) Then
                        CanEdit = True
                    End If
                End If
            End If
            LoadTopic()
        End Sub
        Protected Sub LoadTopic()
            Topic = TC.GetByNameForModule(ModuleID, PageTopic)
            If Topic Is Nothing Then
                Topic = New Entities.TopicInfo
                Topic.TopicID = 0
            End If
            Topic.TabID = tabID
            Topic.PortalSettings = portalSettings
            TopicID = Topic.TopicID
        End Sub
        Protected Function ReadTopic() As String
            Return HttpUtility.HtmlEncode(Topic.Cache)
        End Function

        Protected Function ReadTopicForEdit() As String
            Return Topic.Content
        End Function

        Protected Sub SaveTopic(ByVal Content As String, ByVal AllowDiscuss As Boolean, ByVal AllowRating As Boolean, ByVal Title As String, ByVal Description As String, ByVal Keywords As String)
            Dim th As Entities.TopicHistoryInfo = New Entities.TopicHistoryInfo
            th.TabID = tabID
            th.PortalSettings = portalSettings
            If Topic.TopicID <> 0 Then
                If (Not Content.Equals(Topic.Content) Or Not Title.Equals(Topic.Title) Or Not Description.Equals(Topic.Description) Or Not Keywords.Equals(Topic.Keywords)) Then
                    th.Name = Topic.Name
                    th.TopicID = Topic.TopicID
                    th.Content = Topic.Content
                    th.UpdatedBy = Topic.UpdatedBy
                    th.UpdateDate = DateTime.Now
                    th.UpdatedByUserID = Topic.UpdatedByUserID
                    th.Title = Topic.Title
                    th.Description = Topic.Description
                    th.Keywords = Topic.Keywords

                    Topic.UpdateDate = DateTime.Now
                    If (UserInfo.UserID = -1) Then
                        Topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile)
                    Else
                        Topic.UpdatedBy = UserInfo.Username
                    End If

                    Topic.UpdatedByUserID = UserId
                    Topic.Content = Content
                    Topic.Title = Title
                    Topic.Description = Description
                    Topic.Keywords = Keywords

                    THC.Add(th)
                End If
                Topic.Name = PageTopic
                Topic.Title = Title
                Topic.Description = Description
                Topic.Keywords = Keywords
                Topic.AllowDiscussions = AllowDiscuss
                Topic.AllowRatings = AllowRating
                Topic.Content = Content

                TC.Update(Topic)
            Else
                Topic = New Entities.TopicInfo
                Topic.TabID = tabID
                Topic.PortalSettings = portalSettings
                Topic.Content = Content
                Topic.Name = PageTopic
                Topic.ModuleID = ModuleID
                If (UserInfo.UserID = -1) Then
                    Topic.UpdatedBy = Localization.GetString("Anonymous", RouterResourceFile)
                Else
                    Topic.UpdatedBy = UserInfo.Username
                End If

                Topic.UpdatedByUserID = UserID
                Topic.UpdateDate = DateTime.Now
                Topic.AllowDiscussions = AllowDiscuss
                Topic.AllowRatings = AllowRating
                Topic.Title = Title
                Topic.Description = Description
                Topic.Keywords = Keywords

                Topic.TopicID = TC.Add(Topic)

                TopicID = Topic.TopicID
            End If
        End Sub

        Public Function GetIndex() As ArrayList
            Return TC.GetAllByModuleID(ModuleID)
        End Function

        Protected Function GetRecentlyChanged(ByVal DaysBack As Integer)
            Return TC.GetAllByModuleChangedWhen(ModuleID, DaysBack)
        End Function

        Protected Function GetHistory() As ArrayList
            Return THC.GetHistoryForTopic(TopicID)
        End Function

        Protected Function Search(ByVal SearchString As String) As ArrayList
            Return TC.SearchWiki(SearchString, ModuleID)
        End Function

        'Protected Function ReadTopicHistory(ByVal TopicHistoryID As Integer) As String
        '    Dim th As Entities.TopicHistoryInfo
        '    th = THC.GetItem(TopicHistoryID)
        '    If Not th Is Nothing Then
        '        Return th.Content
        '    Else
        '        Return ""
        '    End If
        'End Function

        Protected Function CreateTable(ByRef ts As ArrayList) As String
            Dim TableTxt As New System.Text.StringBuilder("<table><tr><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableTopic", RouterResourceFile))
            TableTxt.Append("</th><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableModBy", RouterResourceFile))
            TableTxt.Append("</th><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableModDate", RouterResourceFile))
            TableTxt.Append("</th></tr>")
            'Dim TopicTable As String
            Dim t As Entities.TopicInfo
            Dim i As Integer
            If ts.Count > 0 Then
                For i = 0 To ts.Count - 1
                    t = CType(ts(i), Entities.TopicInfo)
                    t.TabID = tabID
                    t.PortalSettings = portalSettings

                    Dim nameToUse As String = String.Empty
                    If t.Title.ToString <> String.Empty 
                        nameToUse = t.Title.Replace(Me.WikiHomeName, "Home")
                    Else
                        nameToUse = t.Name.Replace(Me.WikiHomeName, "Home")
                    End If

                    TableTxt.Append("<tr>")
                    TableTxt.Append("<td><a class=""CommandButton"" href=""")
                    TableTxt.Append(DotNetNuke.Common.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "topic=" & Entities.WikiData.EncodeTitle(t.Name)))
                    TableTxt.Append(""">")
                    TableTxt.Append(nameToUse)
                    TableTxt.Append("</a></td>")
                    TableTxt.Append("<td class=""Normal"">")
                    TableTxt.Append(t.UpdatedByUsername)
                    TableTxt.Append("</td>")
                    TableTxt.Append("<td class=""Normal"">")
                    TableTxt.Append(t.UpdateDate.ToString(CultureInfo.CurrentCulture))
                    TableTxt.Append("</td>")
                    TableTxt.Append("</tr>")
                Next i
            Else
                TableTxt.Append("<tr><td colspan=3 class=""Normal"">")
                TableTxt.Append(Localization.GetString("BaseCreateTableNoResults", RouterResourceFile))
                TableTxt.Append("</td></tr>")
            End If
            TableTxt.Append("</table>")
            Return TableTxt.ToString()
        End Function

        Protected Function CreateRecentChangeTable(ByVal DaysBack As Integer) As String
            Return CreateTable(GetRecentlyChanged(DaysBack))
        End Function
        Protected Function CreateSearchTable(ByVal SearchString As String) As String
            Return CreateTable(Search(SearchString))
        End Function
        Protected Function CreateHistoryTable() As String

            Dim TableTxt As New System.Text.StringBuilder(1000)
            TableTxt.Append("<table><tr><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableTopic", RouterResourceFile))
            TableTxt.Append("</th><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableTitle", RouterResourceFile))
            TableTxt.Append("</th><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableModBy", RouterResourceFile))
            TableTxt.Append("</th><th>")
            TableTxt.Append(Localization.GetString("BaseCreateTableModDate", RouterResourceFile))
            TableTxt.Append("</th></tr>")
            Dim th As ArrayList = GetHistory()
            'Dim TopicTable As StringBuilder = New StringBuilder(500)
            Dim history As Entities.TopicHistoryInfo
            Dim i As Integer
            If th.Count > 0 Then
                i = th.Count
                While (i > 0)
                    history = CType(th(i - 1), Entities.TopicHistoryInfo)
                    history.TabID = tabID
                    history.PortalSettings = portalSettings
                    TableTxt.Append("<tr><td><a class=""CommandButton"" rel=""noindex,nofollow"" href=""")
                    TableTxt.Append(DotNetNuke.Common.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "topic=" & Entities.WikiData.EncodeTitle(PageTopic), "loc=TopicHistory", "ShowHistory=" & history.TopicHistoryID.ToString()))
                    TableTxt.Append(""">")
                    TableTxt.Append(history.Name.Replace(Me.WikiHomeName, "Home"))

                    TableTxt.Append("</a></td>")
                    TableTxt.Append("<td class=""Normal"">")
                    If history.Title.ToString <> String.Empty Then
                        TableTxt.Append(history.Title.Replace(Me.WikiHomeName, "Home"))
                    Else
                        TableTxt.Append(history.Name.Replace(Me.WikiHomeName, "Home"))
                    End If
                    TableTxt.Append("</td>")
                    TableTxt.Append("<td class=""Normal"">")
                    TableTxt.Append(history.UpdatedByUsername)
                    TableTxt.Append("</td>")
                    TableTxt.Append("<td Class=""Normal"">")
                    TableTxt.Append(history.UpdateDate.ToString(CultureInfo.CurrentCulture))
                    TableTxt.Append("</td>")
                    TableTxt.Append("</tr>")
                    i = i - 1
                End While
            Else
                TableTxt.Append("<tr><td colspan=""3"" class=""Normal"">")
                TableTxt.Append(Localization.GetString("BaseCreateHistoryTableEmpty", RouterResourceFile))
                TableTxt.Append("</td></tr>")
            End If
            TableTxt.Append("</table>")
            Return TableTxt.ToString()
        End Function
        
        Public Property RouterResourceFile() As String
            Get
                Return DotNetNuke.Services.Localization.Localization.GetResourceFile(Me, "Router.ascx.resx")
            End Get
            Set(ByVal value As String)
                value = value
            End Set
        End Property

    End Class
End Namespace