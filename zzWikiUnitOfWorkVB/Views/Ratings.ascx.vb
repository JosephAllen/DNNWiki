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
    Partial Class Ratings
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
        Public ReadOnly Property MyModule() As DotNetNuke.Modules.Wiki.WikiControlBase
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

        Public Property HasVoted() As Boolean
            Get
                If Request.Cookies("WikiRating") Is Nothing Then
                    Return False
                Else
                    If Request.Cookies("WikiRating")("ContentID-" + Topic.TopicID.ToString) Is Nothing Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            End Get
            Set(ByVal Value As Boolean)
                If Request.Cookies("WikiRating") Is Nothing Then
                    Response.Cookies.Add(New HttpCookie("WikiRating"))
                End If
                Response.Cookies("WikiRating")("ContentID-" + Topic.TopicID.ToString) = "true"
                Response.Cookies("WikiRating").Expires = DateTime.Now.AddYears(1)
            End Set
        End Property

#Region " Web Form Designer Generated Code "


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

            If Me.Visible Then
                If HasVoted Then
                    DisplayHasVoted()
                Else
                    DisplayCanVote()
                End If
            End If
        End Sub

        Private Sub LoadLocalization()
            RatePagelbl.Text = Localization.GetString("RatingsRateThisPage", RouterResourceFile)
            LowRating.Text = Localization.GetString("RatingsLowRating", RouterResourceFile)
            HighRating.Text = Localization.GetString("RatingsHighRating", RouterResourceFile)
            lblAverageRatingMessage.Text = Localization.GetString("RatingsAverageRatingTitle", RouterResourceFile)
            lblVoteCastMessage.Text = Localization.GetString("RatingsPageRated", RouterResourceFile)
            btnSubmit.Text = Localization.GetString("RatingsSubmitRating", RouterResourceFile)
        End Sub

        Private Sub DisplayHasVoted()
            pnlCastVote.Visible = False
            pnlVoteCast.Visible = True
        End Sub
        Private Sub DisplayCanVote()
            pnlCastVote.Visible = True
            pnlVoteCast.Visible = False
        End Sub
        Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim save As Boolean
            save = False
            If rating1.Checked Then
                Topic.RatingOneCount = Topic.RatingOneCount + 1
                save = True
            ElseIf rating2.Checked Then
                Topic.RatingTwoCount = Topic.RatingTwoCount + 1
                save = True
            ElseIf rating3.Checked Then
                Topic.RatingThreeCount = Topic.RatingThreeCount + 1
                save = True
            ElseIf rating4.Checked Then
                Topic.RatingFourCount = Topic.RatingFourCount + 1
                save = True
            ElseIf rating5.Checked Then
                Topic.RatingFiveCount = Topic.RatingFiveCount + 1
                save = True
            End If
            If save Then
                Dim tc As New DotNetNuke.Modules.Wiki.Entities.TopicController
                tc.Update(Topic)
            End If
            HasVoted = True
            DisplayHasVoted()
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If Me.Visible Then
                If Topic.FivePointRatingsRecorded > 0 Then
                    lblAverageRating.Text = Topic.FivePointAverage.ToString("#.#")
                    lblRatingCount.Text = String.Format(Localization.GetString("RatingsNumberOf", RouterResourceFile), Topic.FivePointRatingsRecorded.ToString)

                    Dim i As Integer
                    i = 0
                    For i = 0 To 4
                        Dim bcImg As New System.Web.UI.WebControls.Image
                        bcImg.ImageUrl = Me.TemplateSourceDirectory + "/images/bcImage.gif"
                        bcImg.Width = Unit.Pixel(10)

                        Dim currentCount As Integer
                        Select Case (i)
                            Case 0
                                currentCount = Topic.RatingOneCount

                                Exit Select
                            Case 1
                                currentCount = Topic.RatingTwoCount
                                Exit Select
                            Case 2
                                currentCount = Topic.RatingThreeCount
                                Exit Select
                            Case 3
                                currentCount = Topic.RatingFourCount
                                Exit Select
                            Case 4
                                currentCount = Topic.RatingFiveCount
                                Exit Select
                        End Select
                        bcImg.Height = Unit.Pixel(Convert.ToInt32(25.0F * (CDbl(currentCount) / (CDbl(Topic.FivePointRatingsRecorded)))))
                        bcImg.AlternateText = currentCount.ToString()
                        RatingsGraphTable.Rows(0).Cells(i).Controls.Add(bcImg)
                    Next
                Else
                    lblAverageRating.Text = Localization.GetString("RatingsNotRatedYet", RouterResourceFile)
                    lblRatingCount.Text = String.Format(Localization.GetString("RatingsNumberOf", RouterResourceFile), "0")

                    RatingsGraphTable.Visible = False
                End If
            End If
        End Sub
    End Class
End Namespace