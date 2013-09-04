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


Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Wiki



    Partial Class Administration
        Inherits PortalModuleBase
        Protected WikiSettings As Entities.SettingsInfo
        Protected WikiController As New Entities.SettingsController
#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Protected WithEvents ContentEditors As DotNetNuke.UI.UserControls.DualListControl
        Protected WithEvents NotifyRoles As DotNetNuke.UI.UserControls.DualListControl

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
            Framework.jQuery.RequestUIRegistration()
            Framework.jQuery.RequestDnnPluginsRegistration()
        End Sub

#End Region

        Private Sub CtrlPage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
           
            ContentEditors.DataTextField = "Text"
            ContentEditors.DataValueField = "Text"
            NotifyRoles.DataTextField = "Text"
            NotifyRoles.DataValueField = "Text"
            If WikiSettings Is Nothing Then

                WikiSettings = WikiController.GetByModuleID(ModuleId)
                If WikiSettings Is Nothing Then
                    WikiSettings = New Entities.SettingsInfo
                    WikiSettings.ModuleID = -1
                    WikiSettings.ContentEditorRoles = "UseDNNSettings"
                End If
            End If
            If Not IsPostBack Then
                DNNSecurityChk.Checked = WikiSettings.ContentEditorRoles.Equals("UseDNNSettings")
                AllowPageComments.Checked = WikiSettings.AllowDiscussions
                AllowPageRatings.Checked = WikiSettings.AllowRatings
                DefaultCommentsMode.Checked = WikiSettings.DefaultDiscussionMode
                DefaultRatingMode.Checked = WikiSettings.DefaultRatingMode
                NotifyMethodUserComments.Checked = WikiSettings.CommentNotifyUsers

                NotifyMethodCustomRoles.Checked = Not WikiSettings.CommentNotifyRoles.StartsWith("UseDNNSettings;") AndAlso WikiSettings.CommentNotifyRoles.Length > 0
                If NotifyMethodCustomRoles.Checked Then
                    NotifyMethodEditRoles.Checked = WikiSettings.CommentNotifyRoles.Contains(";Edit")
                    NotifyMethodViewRoles.Checked = WikiSettings.CommentNotifyRoles.Contains(";View")
                End If

                BindRights()
                If DNNSecurityChk.Checked = True Then
                    ContentEditors.Visible = False
                    WikiSecurity.Visible = False
                Else
                    ContentEditors.Visible = True
                    WikiSecurity.Visible = True
                End If
                If AllowPageComments.Checked Then
                    Me.ActivateComments.Enabled = True
                    Me.DefaultCommentsMode.Enabled = True
                Else
                    Me.ActivateComments.Enabled = False
                    Me.ActivateComments.Checked = False
                    Me.DefaultCommentsMode.Enabled = False
                    Me.DefaultCommentsMode.Checked = False
                End If

                If AllowPageRatings.Checked Then
                    Me.ActivateRatings.Enabled = True
                    Me.DefaultRatingMode.Enabled = True
                Else
                    Me.ActivateRatings.Enabled = False
                    Me.ActivateRatings.Checked = False
                    Me.DefaultRatingMode.Enabled = False
                    Me.DefaultRatingMode.Checked = False
                End If
                If NotifyMethodCustomRoles.Checked Then
                    NotifyRoles.Visible = True
                    lblNotifyRoles.Visible = True
                    Me.NotifyMethodEditRoles.Enabled = False
                    Me.NotifyMethodViewRoles.Enabled = False
                    Me.NotifyMethodViewRoles.Checked = False
                    Me.NotifyMethodEditRoles.Checked = False
                Else
                    Me.NotifyMethodEditRoles.Enabled = True
                    Me.NotifyMethodViewRoles.Enabled = True

                    NotifyRoles.Visible = False
                    lblNotifyRoles.Visible = False
                End If
            End If
        End Sub


        Private Sub DNNSecurityChk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DNNSecurityChk.CheckedChanged
            If DNNSecurityChk.Checked = True Then
                ContentEditors.Visible = False
                WikiSecurity.Visible = False
            Else
                ContentEditors.Visible = True
                WikiSecurity.Visible = True
            End If
        End Sub

        Private Sub BindRights()

            ' declare roles
            Dim arrAvailableAuthViewRoles As New ArrayList
            Dim arrAvailableNotifyRoles As New ArrayList

            ' add an entry of All Users for the View roles
            arrAvailableAuthViewRoles.Add(New ListItem("All Users", Common.glbRoleAllUsersName))
            ' add an entry of Unauthenticated Users for the View roles
            arrAvailableAuthViewRoles.Add(New ListItem("Unauthenticated Users", glbRoleUnauthUserName))
            ' add an entry of All Users for the Edit roles

            ' process portal roles
            Dim objRoles As New DotNetNuke.Security.Roles.RoleController
            Dim objRole As DotNetNuke.Security.Roles.RoleInfo
            Dim arrRoles As ArrayList = objRoles.GetPortalRoles(PortalId)
            For Each objRole In arrRoles
                arrAvailableAuthViewRoles.Add(New ListItem(objRole.RoleName, objRole.RoleName))
                arrAvailableNotifyRoles.Add(New ListItem(objRole.RoleName, objRole.RoleName))
            Next

            ' populate view roles

            Dim arrAssignedAuthViewRoles As New ArrayList
            Dim arrAssignedNotifyRoles As New ArrayList

            Dim arrAuthViewRoles As Array
            Dim arrAuthNotifyRoles As Array
            If WikiSettings.ContentEditorRoles = "UseDNNSettings" Then
                arrAuthViewRoles = Split(WikiSettings.ContentEditorRoles, "UseDNNSettings")
            Else
                arrAuthViewRoles = Split(WikiSettings.ContentEditorRoles.Split("|")(0), ";")
            End If
            If WikiSettings.CommentNotifyRoles.StartsWith("UseDNNSettings;") Then
                WikiSettings.CommentNotifyRoles = WikiSettings.CommentNotifyRoles.Replace("UseDNNSettings;", String.Empty)
                arrAuthNotifyRoles = Split(WikiSettings.CommentNotifyRoles, ";")

                For Each s As String In arrAuthNotifyRoles
                    If s = "View" Then
                        NotifyMethodViewRoles.Checked = True
                    ElseIf s = "Edit" Then
                        NotifyMethodEditRoles.Checked = True
                    End If
                Next
            Else
                arrAuthNotifyRoles = Split(WikiSettings.CommentNotifyRoles.Split("|")(0), ";")
            End If

            For Each strRole As String In arrAuthViewRoles
                If strRole <> "" Then
                    For Each objListItem As ListItem In arrAvailableAuthViewRoles
                        If objListItem.Value = strRole Then
                            arrAssignedAuthViewRoles.Add(objListItem)
                            arrAvailableAuthViewRoles.Remove(objListItem)
                            Exit For
                        End If
                    Next
                End If
            Next

            For Each strRole As String In arrAuthNotifyRoles
                If strRole <> "" Then
                    For Each objListItem As ListItem In arrAvailableNotifyRoles
                        If objListItem.Value = strRole Then
                            arrAssignedNotifyRoles.Add(objListItem)
                            arrAvailableNotifyRoles.Remove(objListItem)
                            Exit For
                        End If
                    Next
                End If
            Next

            Dim x As Integer = arrAvailableAuthViewRoles.Count
            Dim y As Integer = arrAssignedAuthViewRoles.Count
            ContentEditors.Available = arrAvailableAuthViewRoles
            ContentEditors.Assigned = arrAssignedAuthViewRoles

            NotifyRoles.Assigned = arrAssignedNotifyRoles
            NotifyRoles.Available = arrAvailableNotifyRoles
        End Sub

        Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
            SaveSettings()
            Response.Redirect(NavigateURL(), True)
        End Sub

        Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
            Response.Redirect(NavigateURL(), True)
        End Sub

        Private Sub AllowPageComments_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllowPageComments.CheckedChanged
            If AllowPageComments.Checked Then
                Me.ActivateComments.Enabled = True
                Me.ActivateComments.Checked = True

                Me.DefaultCommentsMode.Enabled = True
            Else
                Me.ActivateComments.Enabled = False
                Me.ActivateComments.Checked = False

                Me.DefaultCommentsMode.Enabled = False
                Me.DefaultCommentsMode.Checked = False
            End If
        End Sub
        Private Sub SaveSettings()
            If DNNSecurityChk.Checked = True Then
                WikiSettings.ContentEditorRoles = "UseDNNSettings"
            Else
                Dim list As String = ";"
                For Each li As ListItem In ContentEditors.Assigned
                    list = list + li.Value + ";"
                Next
                WikiSettings.ContentEditorRoles = list
            End If

            If NotifyMethodCustomRoles.Checked = False Then
                WikiSettings.CommentNotifyRoles = "UseDNNSettings"
                If NotifyMethodEditRoles.Checked = True Then
                    WikiSettings.CommentNotifyRoles = WikiSettings.CommentNotifyRoles & ";Edit"
                End If
                If NotifyMethodViewRoles.Checked = True Then
                    WikiSettings.CommentNotifyRoles = WikiSettings.CommentNotifyRoles & ";View"
                End If
            Else
                Dim list As String = ";"
                For Each li As ListItem In NotifyRoles.Assigned
                    list = list + li.Value + ";"
                Next
                WikiSettings.CommentNotifyRoles = list
            End If

            WikiSettings.AllowDiscussions = AllowPageComments.Checked
            WikiSettings.AllowRatings = AllowPageRatings.Checked
            WikiSettings.DefaultDiscussionMode = DefaultCommentsMode.Checked
            WikiSettings.DefaultRatingMode = DefaultRatingMode.Checked
            WikiSettings.CommentNotifyUsers = NotifyMethodUserComments.Checked

            If WikiSettings.ModuleID = -1 Then
                WikiSettings.ModuleID = ModuleId
                WikiController.Add(WikiSettings)
            Else
                WikiController.Update(WikiSettings)
            End If
            ActivateItems()
        End Sub

        Private Sub ActivateItems()
            If ActivateComments.Checked Or ActivateRatings.Checked Then
                Dim TC As New Entities.TopicController
                Dim alltopics As ArrayList
                alltopics = TC.GetAllByModuleID(Me.ModuleId)
                Dim topic As Entities.TopicInfo
                For Each topic In alltopics
                    If topic.AllowDiscussions = False And ActivateComments.Checked Then
                        topic.AllowDiscussions = True
                    End If
                    If topic.AllowRatings = False And ActivateRatings.Checked Then
                        topic.AllowRatings = True
                    End If
                    TC.Update(topic)
                Next
            End If
        End Sub


        Private Sub AllowPageRatings_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllowPageRatings.CheckedChanged
            If AllowPageComments.Checked Then
                Me.ActivateRatings.Enabled = True
                Me.ActivateRatings.Checked = True

                Me.DefaultRatingMode.Enabled = True
            Else
                Me.ActivateRatings.Enabled = False
                Me.ActivateRatings.Checked = False

                Me.DefaultRatingMode.Enabled = False
                Me.DefaultRatingMode.Checked = False
            End If
        End Sub

        Private Sub NotifyMethodCustomRoles_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyMethodCustomRoles.CheckedChanged
            If NotifyMethodCustomRoles.Checked Then
                NotifyRoles.Visible = True
                lblNotifyRoles.Visible = True

                Me.NotifyMethodEditRoles.Enabled = False
                Me.NotifyMethodViewRoles.Enabled = False
                Me.NotifyMethodViewRoles.Checked = False
                Me.NotifyMethodEditRoles.Checked = False
            Else
                Me.NotifyMethodEditRoles.Enabled = True
                Me.NotifyMethodViewRoles.Enabled = True
                lblNotifyRoles.Visible = False
                NotifyRoles.Visible = False
            End If
        End Sub
    End Class
End Namespace