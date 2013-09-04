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

Imports DotNetNuke.Modules.Wiki.Entities
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Wiki

    Partial Class Edit
        Inherits WikiControlBase
        Protected WithEvents cmdHistory As System.Web.UI.WebControls.Button
        Protected WithEvents teContent As UI.UserControls.TextEditor

        Protected WithEvents WikiTextDirections As UI.UserControls.SectionHeadControl



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

#Region "Form Events"

        Shadows Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            LoadLocalization()

            If (CanEdit) Then
                If Not teContent Is Nothing Then
                    teContent.HtmlEncode = False
                End If

                LoadTopic()
                If (Topic.Name = String.Empty) Then
                    If (Me.Request.QueryString.Item("topic") Is Nothing) Then
                        PageTopic = WikiHomeName.Replace("[L]", "")
                        Topic.Name = PageTopic
                    Else
                        PageTopic = Entities.WikiData.DecodeTitle(Me.Request.QueryString.Item("topic").ToString()).Replace("[L]", "")
                        Topic.Name = PageTopic
                    End If
                End If

                Me.EditPage()

                'CommentsSec.IsExpanded = FalseB
                If Not Me.Request.QueryString.Item("add") Is Nothing Then
                    PageTopic = ""
                    LoadTopic()
                    Me.EditPage()
                Else

                End If
                'add confirmation to the delete button
                ClientAPI.AddButtonConfirm(DeleteBtn, Localization.GetString("ConfirmDelete", LocalResourceFile))

            Else
                'user doesn't have edit rights to this module, load up a message stating so.
                lblMessage.Text = Localization.GetString("NoEditAccess", LocalResourceFile)
                divWikiEdit.Visible = False
            End If
        End Sub



        'Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        '    'TODO: none of this is currently working..... not sure why

        '    PageTopic = ""
        '    LoadTopic()
        '    Me.EditPage()
        'End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            'if we've change the Topic Name we need to create a new topic
            Dim ti As TopicInfo = Nothing
            'The following lines remmed by JMA on 7/15/2013 becuase we don't need duplicate topcis
            'If PageTopic = String.Empty Or PageTopic <> WikiData.DecodeTitle(txtPageName.Text.Trim()) Then
            '    PageTopic = WikiData.DecodeTitle(txtPageName.Text.Trim())
            '    Topic.TopicID = 0
            '    ti = TC.GetByNameForModule(ModuleId, PageTopic)
            'End If
            PageTopic = WikiData.DecodeTitle(txtPageName.Text.Trim())
            If ti Is Nothing Then
                Me.SaveChanges()
                If (PageTopic = WikiHomeName) Then
                    Response.Redirect(DotNetNuke.Common.NavigateURL(Me.TabId))
                End If
                'The following lines remmed by JMA on 7/15/2013 becuase we don't need duplicate topcis
                'Response.Redirect(DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, String.Empty, "", "topic=" & WikiData.EncodeTitle(Me.PageTopic)), False)
                Response.Redirect(DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, String.Empty, "", "topic=" & WikiData.EncodeTitle(Me.PageTopic)), False)
            Else
                lblPageCreationError.Text = Localization.GetString("lblPageCreationError", LocalResourceFile)
            End If

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Me.CancelChanges()
        End Sub

        Private Sub cmdSaveAndContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveAndContinue.Click
            PageTopic = txtPageName.Text.Trim()
            Me.SaveAndContinue()
            'Response.Redirect(DotNetNuke.Common.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "", "topic=" & Entities.WikiData.EncodeTitle(Me.PageTopic)), False)
        End Sub


#End Region

        Private Sub LoadLocalization()
            AllowDiscuss.Text = Localization.GetString("StartAllowDiscuss", LocalResourceFile)
            AllowRating.Text = Localization.GetString("StartAllowRatings", LocalResourceFile)
            cmdCancel.Text = Localization.GetString("StartCancel", LocalResourceFile)
            'CommentsSec.Text = Localization.GetString("StartCommentsSection", LocalResourceFile)
            DeleteBtn.Text = Localization.GetString("StartDelete", LocalResourceFile)
            cmdSave.Text = Localization.GetString("StartSave", LocalResourceFile)
            cmdSaveAndContinue.Text = Localization.GetString("StartSaveAndContinue", LocalResourceFile)
            WikiTextDirections.Text = Localization.GetString("StartWikiDirections", LocalResourceFile)
            WikiDirectionsDetails.Text = Localization.GetString("StartWikiDirectionDetails", LocalResourceFile)
            'RatingSec.Text = Localization.GetString("StartRatingSec.Text", LocalResourceFile)
        End Sub

        Private Sub DisplayTopic()
            Me.cmdSave.Visible = True
            Me.cmdSaveAndContinue.Visible = True
            Me.cmdCancel.Visible = True
            Me.teContent.Text = ReadTopicForEdit()

            If Me.WikiSettings.AllowDiscussions Then
                Me.AllowDiscuss.Enabled = True
                Me.AllowDiscuss.Checked = Me.Topic.AllowDiscussions OrElse Me.WikiSettings.DefaultDiscussionMode
            Else
                Me.AllowDiscuss.Enabled = False
                Me.AllowDiscuss.Checked = False
            End If

            If Me.WikiSettings.AllowRatings Then
                Me.AllowRating.Enabled = True
                Me.AllowRating.Checked = Me.Topic.AllowRatings OrElse Me.WikiSettings.DefaultRatingMode
            Else
                Me.AllowRating.Enabled = False
                Me.AllowRating.Checked = False
            End If

            Me.DeleteBtn.Visible = False
            Me.DeleteLbl.Visible = False
            If Me.Topic.Name <> WikiHomeName Then
                Me.DeleteBtn.Visible = True
                Me.DeleteLbl.Visible = True
            End If

            If Me.Topic.Name.Trim().Length < 1 Then
                txtPageName.Text = String.Empty
                txtPageName.ReadOnly = False
            Else
            	txtPageName.Text = HttpUtility.HtmlDecode(Topic.Name.Trim().Replace("[L]", ""))
            	If PageTopic = WikiHomeName Then
            		txtPageName.ReadOnly = True
            	Else
            		txtPageName.ReadOnly = False
            	End If
            End If


            If Me.Topic.Title.Trim().Length > 0 Then
                txtTitle.Text = HttpUtility.HtmlDecode(Topic.Title.Replace("[L]", ""))
            End If

            If Not Me.Topic.Description Is Nothing Then
                txtDescription.Text = Topic.Description
            End If

            If Not Me.Topic.Keywords Is Nothing Then
                txtKeywords.Text = Topic.Keywords
            End If

            'TODO: Fix Printer Friendly


        End Sub

        Private Sub CancelChanges()
            'SEND BACK TO THE VIEW PAGE
            If String.IsNullOrEmpty(Me.PageTopic) Then
                Response.Redirect(DotNetNuke.Common.NavigateURL(Me.TabId), False)
            Else
                Response.Redirect(DotNetNuke.Common.NavigateURL(Me.TabId, Me.PortalSettings, String.Empty, "", "topic=" & Entities.WikiData.EncodeTitle(Me.PageTopic)), False)
            End If
        End Sub

        Private Sub SaveChanges()
            SaveAndContinue()
            'redirect to the topic's url
        End Sub

        Private Sub SaveAndContinue()
            Dim objSec As New DotNetNuke.Security.PortalSecurity
            SaveTopic(HttpUtility.HtmlDecode(objSec.InputFilter(objSec.InputFilter(Me.teContent.Text, PortalSecurity.FilterFlag.NoMarkup), PortalSecurity.FilterFlag.NoScripting)) _
                      , Me.AllowDiscuss.Checked _
                      , Me.AllowRating.Checked _
                      , objSec.InputFilter(WikiData.DecodeTitle(Me.txtTitle.Text.Trim()) _
                        , PortalSecurity.FilterFlag.NoMarkup) _
                      , objSec.InputFilter(Me.txtDescription.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup) _
                      , objSec.InputFilter(Me.txtKeywords.Text.Trim() _
                      , PortalSecurity.FilterFlag.NoMarkup))
        End Sub

        Private Sub EditPage()
            'redirect back to the topic url
            DisplayTopic()
        End Sub

        Private Sub DeleteBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteBtn.Click
            Dim thlist As ArrayList = Me.GetHistory()
            For Each th As Entities.TopicHistoryInfo In thlist
                THC.Delete(th.TopicHistoryID)
            Next
            TC.Delete(Me.TopicID)
            Response.Redirect(Me.HomeURL, True)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            'If ratings.HasVoted Then
            '    RatingSec.IsExpanded = False
            'End If

        End Sub

    End Class
End Namespace
