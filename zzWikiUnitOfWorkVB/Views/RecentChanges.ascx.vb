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

Namespace DotNetNuke.Modules.Wiki

    Partial Class RecentChanges
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
                HitTable.Text = CreateRecentChangeTable(1)
            End If
        End Sub

        Private Sub LoadLocalization()
            TitleLbl.Text = Localization.GetString("RCTitle", RouterResourceFile)
            cmdLast24Hrs.Text = Localization.GetString("RCLast24h", RouterResourceFile)
            cmdLast7Days.Text = Localization.GetString("RCLast7d", RouterResourceFile)
            cmdLastMonth.Text = Localization.GetString("RCLastMonth", RouterResourceFile)

        End Sub
        Private Sub cmdLast24Hrs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast24Hrs.Click
            HitTable.Text = CreateRecentChangeTable(1)
        End Sub

        Private Sub cmdLast7Days_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast7Days.Click
            HitTable.Text = CreateRecentChangeTable(7)
        End Sub

        Private Sub cmdLastMonth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLastMonth.Click
            HitTable.Text = CreateRecentChangeTable(31)
        End Sub
    End Class
End Namespace