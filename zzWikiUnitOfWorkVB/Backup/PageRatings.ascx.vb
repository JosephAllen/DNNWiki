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

Namespace DotNetNuke.Modules.Wiki
    Partial Class PageRatings
        Inherits WikiControlBase
        Private mModule As DotNetNuke.Modules.Wiki.WikiControlBase
        Private mTopic As DotNetNuke.Modules.Wiki.Entities.TopicInfo
        Public ReadOnly Property Topic() As DotNetNuke.Modules.Wiki.Entities.TopicInfo
            Get
                If mTopic Is Nothing Then
                    Dim uplevel As System.Web.UI.Control
                    uplevel = Me.Parent
                    While Not TypeOf uplevel Is DotNetNuke.Modules.Wiki.WikiControlBase
                        uplevel = uplevel.Parent
                    End While
                    mTopic = CType(uplevel, DotNetNuke.Modules.Wiki.WikiControlBase).Topic
                End If
                Return mTopic
            End Get
        End Property

        Public ReadOnly Property ParentModule() As DotNetNuke.Modules.Wiki.WikiControlBase
            Get
                If mModule Is Nothing Then
                    Dim uplevel As System.Web.UI.Control
                    uplevel = Me.Parent
                    While Not TypeOf uplevel Is DotNetNuke.Modules.Wiki.WikiControlBase
                        uplevel = uplevel.Parent
                    End While
                    mModule = CType(uplevel, DotNetNuke.Modules.Wiki.WikiControlBase)
                    mModule.ModuleConfiguration = Me.ModuleConfiguration
                End If
                Return mModule
            End Get
        End Property

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

        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            LoadLocalization()
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If Topic.FivePointRatingsRecorded = 0 Then
                RatingBar.Visible = False
                NoRating.Visible = True
            Else
                RatingBar.Visible = True
                NoRating.Visible = False
                RatingBar.Src = Me.TemplateSourceDirectory + "/RatingBar.aspx?rating=" + Topic.FivePointAverage.ToString("#.#")
                RatingBar.Alt = Topic.FivePointAverage.ToString("#.#")
            End If
        End Sub

        Private Sub LoadLocalization()
            NoRating.Text = Localization.GetString("PageRatingsNotRatedYet", RouterResourceFile)
            RatingLbl.Text = Localization.GetString("PageRatingsTitle", RouterResourceFile)
        End Sub
    End Class
End Namespace
