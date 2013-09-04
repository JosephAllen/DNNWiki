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

Imports System.Data
Imports DotNetNuke.Modules.Wiki.Providers.Data
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.Entities.Host

Namespace DotNetNuke.Modules.Wiki.Entities

    Public Class CommentsController
        Public Function AddComment(ByVal ParentId As Integer, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Ip As String, ByVal EmailNotify As Boolean) As Boolean

            AddComment = DataProvider.Instance().Wiki_AddComment(ParentId, Name, Email, Comment, Ip, EmailNotify)
            If AddComment = True Then
                'send emails to any user that might of opted in to email notifications
                SendNotifications(ParentId, Name, Email, Comment, Ip)
            End If

        End Function

        Public Function DeleteComment(ByVal CommentId As Long) As Boolean
            Return DataProvider.Instance().Wiki_DeleteComment(CommentId)
        End Function

        Public Function DeleteComments(ByVal ParentId As Long) As Boolean
            Return DataProvider.Instance().Wiki_DeleteComments(ParentId)
        End Function

        Public Function GetCommentsByParent(ByVal ParentId As Long) As DataTable
            Return DataProvider.Instance().Wiki_GetCommentsByParent(ParentId)
        End Function

        Public Function GetComments() As DataTable
            Return DataProvider.Instance().Wiki_GetComments()
        End Function

        Public Function GetComment(ByVal CommentId As Long) As DataRow
            Return DataProvider.Instance().Wiki_GetComment(CommentId)
        End Function

        Public Function GetCommentCount(ByVal ParentId As Long) As Integer
            Return DataProvider.Instance().Wiki_GetCommentCount(ParentId)
        End Function

        Public Function ValidParent(ByVal ParentId As Long) As Boolean
            Return DataProvider.Instance().Wiki_ValidCommentParent(ParentId)
        End Function

        Public Function AddCommentParent(ByVal ParentId As Long, ByVal Name As String) As Boolean
            Return DataProvider.Instance().Wiki_AddCommentParent(ParentId, Name)
        End Function

        Public Function EditCommentParent(ByVal Id As Long, ByVal ParentId As Long, ByVal Name As String) As Boolean
            Return DataProvider.Instance().Wiki_EditCommentParent(Id, ParentId, Name)
        End Function

        Public Function DeleteCommentParent(ByVal Id As Long) As Boolean
            Return DataProvider.Instance().Wiki_DeleteCommentParent(Id)
        End Function

        Public Function GetCommentParents() As DataTable
            Return DataProvider.Instance().Wiki_GetCommentParents()
        End Function

        Public Function GetParent(ByVal Id As Long) As DataRow
            Return DataProvider.Instance().Wiki_GetCommentParent(Id)
        End Function



        Public Function GetNotificationEmails(ByVal objWikiTopic As TopicInfo) As List(Of String)
            Dim WikiSettings As Entities.SettingsInfo = New Entities.SettingsController().GetByModuleID(objWikiTopic.ModuleID)
            Dim lstUsers As New List(Of String)

            If WikiSettings IsNot Nothing Then



                'gather the email address from the roles assigned to this module... 
                If WikiSettings.CommentNotifyRoles.Length > 0 Then

                    Dim objRoles As New DotNetNuke.Security.Roles.RoleController
                    Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                    Dim objModule As DotNetNuke.Entities.Modules.ModuleInfo = objModules.GetModule(objWikiTopic.ModuleID)

                    If objModule IsNot Nothing Then

                        Dim bFetchUsingDNNRoles As Boolean = False
                        Dim bFetchUsingCustomRoles As Boolean = False
                        Dim bFetchViewUsers As Boolean = False
                        Dim bFetchEditUsers As Boolean = False

                        bFetchUsingDNNRoles = WikiSettings.ContentEditorRoles.StartsWith("UseDNNSettings;")
                        bFetchUsingCustomRoles = Not WikiSettings.CommentNotifyRoles.StartsWith("UseDNNSettings;")

                        If Not bFetchUsingCustomRoles Then
                            bFetchEditUsers = WikiSettings.CommentNotifyRoles.Contains(";Edit")
                            bFetchViewUsers = WikiSettings.CommentNotifyRoles.Contains(";View")
                        End If

                        'compile our view users, only if enabled
                        If bFetchViewUsers Then
                            For Each role As String In objModule.AuthorizedViewRoles.Trim(";").Split(";")
                                If role.ToLower = "all users" Then
                                    'trap against fake roles
                                    Dim arrUsers As ArrayList = DotNetNuke.Entities.Users.UserController.GetUsers(DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings.PortalId)
                                    For Each objUser As DotNetNuke.Entities.Users.UserInfo In arrUsers
                                        If Not lstUsers.Contains(objUser.Email) Then
                                            lstUsers.Add(objUser.Email)
                                        End If
                                    Next
                                Else
                                    'this role should be legit
                                    For Each objUserRole As DotNetNuke.Entities.Users.UserRoleInfo In objRoles.GetUserRolesByRoleName(objModule.PortalID, role)
                                        If Not lstUsers.Contains(objUserRole.Email) Then
                                            lstUsers.Add(objUserRole.Email)
                                        End If
                                    Next
                                End If

                            Next
                        End If

                        'compile our edit users, only if enabled
                        If bFetchEditUsers Then
                            If bFetchUsingDNNRoles Then
                                'fetch using dnn edit roles
                                For Each role As String In objModule.AuthorizedEditRoles.Trim(";").Split(";")
                                    If role.ToLower = "all users" Then
                                        'trap against fake roles

                                    Else
                                        'this role should be legit
                                        For Each objUserRole As DotNetNuke.Entities.Users.UserRoleInfo In objRoles.GetUserRolesByRoleName(objModule.PortalID, role)
                                            If Not lstUsers.Contains(objUserRole.Email) Then
                                                lstUsers.Add(objUserRole.Email)
                                            End If
                                        Next
                                    End If
                                Next
                            Else
                                'fetch using custom wiki edit roles
                                For Each role As String In WikiSettings.ContentEditorRoles.Trim(";").Split(";")
                                    For Each objUserRole As DotNetNuke.Entities.Users.UserRoleInfo In objRoles.GetUserRolesByRoleName(objModule.PortalID, role)
                                        If Not lstUsers.Contains(objUserRole.Email) Then
                                            lstUsers.Add(objUserRole.Email)
                                        End If
                                    Next
                                Next
                            End If

                        End If

                    End If

                End If

                'gather any users emails address from comments in this topic... 
                If WikiSettings.CommentNotifyUsers Then
                    Dim lstEmails As List(Of CommentEmails) = CBO.FillCollection(Of CommentEmails)(DataProvider.Instance().Wiki_GetCommentNotifyUsers(objWikiTopic.TopicID))
                    For Each objCommentEmail As CommentEmails In lstEmails
                        If Not lstUsers.Contains(objCommentEmail.Email) Then
                            lstUsers.Add(objCommentEmail.Email)
                        End If
                    Next
                End If

            End If

            Return lstUsers
        End Function

        Public Sub SendNotifications(ByVal iD As Long, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Ip As String)
            Dim objWikiTopics As New TopicController
            Dim objWikiTopic As TopicInfo = objWikiTopics.GetItem(iD)

            If objWikiTopic IsNot Nothing Then
                Dim lstEmailsAddresses As List(Of String) = GetNotificationEmails(objWikiTopic)

                If lstEmailsAddresses.Count > 0 Then
                    Dim objPortalSettings As DotNetNuke.Entities.Portals.PortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings
                    Dim strResourceFile As String = Common.Globals.ApplicationPath & "/DesktopModules/Wiki/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile
                    Dim strSubject As String = Localization.GetString("NotificationSubject", strResourceFile)
                    Dim strBody As String = Localization.GetString("NotificationBody", strResourceFile)

                    strBody = strBody.Replace("[URL]", DotNetNuke.Common.NavigateURL(objPortalSettings.ActiveTab.TabID, objPortalSettings, String.Empty, "topic=" & Entities.WikiData.EncodeTitle(objWikiTopic.Name)))
                    strBody = strBody.Replace("[NAME]", Name)
                    strBody = strBody.Replace("[EMAIL]", Email)
                    strBody = strBody.Replace("[COMMENT]", Comment)
                    strBody = strBody.Replace("[IP]", String.Empty)

                    Dim sbUsersToEmail As New Text.StringBuilder
                    For Each sUserToEmail As String In lstEmailsAddresses
                        sbUsersToEmail.Append(sUserToEmail)
                        sbUsersToEmail.Append(";")
                    Next

                    'remove the last ;
                    sbUsersToEmail.Remove(sbUsersToEmail.Length - 1, 1)

                    'Services.Mail.Mail.SendMail(objPortalSettings.Email, objPortalSettings.Email, sbUsersToEmail.ToString, strSubject, strBody, "", "", "", "", "", "")

                    Mail.SendMail(objPortalSettings.Email, sbUsersToEmail.ToString, "", "", MailPriority.Normal, strSubject, MailFormat.Html, Encoding.UTF8, strBody, "", Host.SMTPServer, Host.SMTPAuthentication, Host.SMTPUsername, Host.SMTPPassword, Host.EnableSMTPSSL)

                End If
            End If
        End Sub

    End Class

    Public Class CommentEmails
        Public Sub New()
        End Sub

        Public Sub New(ByVal email As String)
            Me.Email = email
        End Sub

        Private _email As String

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property
    End Class

End Namespace
