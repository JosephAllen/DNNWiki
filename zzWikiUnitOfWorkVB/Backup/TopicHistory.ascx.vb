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

Namespace DotNetNuke.Modules.Wiki
    Partial Class TopicHistory
        Inherits WikiControlBase

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

        Shadows Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            LoadLocalization()

            If Not Me.IsPostBack() Then
                Me.RestoreLbl.Visible = False
                Me.cmdRestore.Visible = False
                If Not Me.Request.QueryString.Item("ShowHistory") Is Nothing Then
                    Me.ShowOldVersion()
                Else
                    Me.ShowTopicHistoryList()
                End If
            End If

        End Sub

        Private Sub LoadLocalization()
            Label1.Text = Localization.GetString("HistoryTitle", RouterResourceFile)
            BackBtn.Text = Localization.GetString("HistoryBack", RouterResourceFile)
            cmdRestore.Text = Localization.GetString("HistoryRestore", RouterResourceFile)
            RestoreLbl.Text = Localization.GetString("HistoryRestoreNotice", RouterResourceFile)
        End Sub

        Private Sub ShowOldVersion()
            If Me.CanEdit Then
                Me.RestoreLbl.Visible = True
                Me.cmdRestore.Visible = True
            End If
            Dim HistoryPK, DateTime, HomePage As String
            HistoryPK = Me.Request.QueryString.Item("ShowHistory")
            Dim th As Entities.TopicHistoryInfo = THC.GetItem(Integer.Parse(HistoryPK))
            Me.lblPageTopic.Text = PageTopic.Replace(Me.WikiHomeName, "Home")
            Me.lblPageContent.Text = th.Cache
            Me.lblDateTime.Text = String.Format(Localization.GetString("HistoryAsOf", RouterResourceFile), th.UpdateDate.ToString(CultureInfo.CurrentCulture))
            Me.BackBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "loc=TopicHistory", "topic=" & Entities.WikiData.EncodeTitle(Me.PageTopic))
        End Sub

        Private Sub ShowTopicHistoryList()
            Me.lblPageTopic.Text = PageTopic.Replace(Me.WikiHomeName, "Home")

            Me.lblDateTime.Text = "..."
            Me.lblPageContent.Text = Localization.GetString("HistoryListHeader", RouterResourceFile) + " <br /> " + CreateHistoryTable()
            Me.BackBtn.NavigateUrl = DotNetNuke.Common.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "topic=" & Entities.WikiData.EncodeTitle(PageTopic))
        End Sub

        Private Sub cmdRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestore.Click
            If Not Me.Request.QueryString.Item("ShowHistory") Is Nothing Then
                Dim HistoryPK As String = Me.Request.QueryString.Item("ShowHistory")
                Dim th As Entities.TopicHistoryInfo = THC.GetItem(Integer.Parse(HistoryPK))
                th.TabID = tabID
                th.PortalSettings = portalSettings
                Dim tho As New Entities.TopicHistoryInfo
                tho.TabID = tabID
                tho.PortalSettings = portalSettings
                tho.Content = Me.Topic.Content
                tho.TopicID = Me.TopicID
                tho.UpdatedBy = Me.Topic.UpdatedBy
                tho.UpdateDate = Me.Topic.UpdateDate
                tho.Name = Me.PageTopic
                tho.Title = Me.Topic.Title
                tho.UpdatedByUserID = Topic.UpdatedByUserID

                Me.Topic.Content = th.Content
                Me.Topic.Name = th.Name
                Me.Topic.Title = th.Title
                Me.Topic.Keywords = th.Keywords
                Me.Topic.Description = th.Description
                Me.Topic.UpdatedBy = UserInfo.Username
                Me.Topic.UpdateDate = DateTime.Now
                Me.Topic.UpdatedByUserID = UserId
                TC.Update(Me.Topic)
                THC.Add(tho)

                Response.Redirect(HomeURL, True)
            End If
        End Sub
    End Class
End Namespace