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

Imports DotNetNuke.Common.Utilities

Namespace DotNetNuke.Modules.Wiki.Entities

    Public Class TopicInfo
        Inherits WikiData
        ' Entity member variables for database table Topic

        Private mModuleID As Integer
        Private mTopicID As Integer
        Private mContent As String
        Private mCache As String
        Private mName As String = String.Empty
        Private mTitle As String = String.Empty
        Private mDescription As String
        Private mKeyWords As String
        Private mUpdateDate As Date
        Private mUpdatedBy As String
        Private mUpdatedByUserID As Integer
        Private mAllowDiscussions As Boolean
        Private mAllowRatings As Boolean
        Private mRatingOneCount As Integer
        Private mRatingTwoCount As Integer
        Private mRatingThreeCount As Integer
        Private mRatingFourCount As Integer
        Private mRatingFiveCount As Integer
        Private mRatingSixCount As Integer
        Private mRatingSevenCount As Integer
        Private mRatingEightCount As Integer
        Private mRatingNineCount As Integer
        Private mRatingTenCount As Integer
        Private mUpdatedByUsername As String


        ' Calculated Expression member variables for database table Topic


        ' Entity properties for database table Topic

        Public Property ModuleID() As Integer
            Get
                Return mModuleID
            End Get
            Set(ByVal Value As Integer)
                mModuleID = Value
            End Set
        End Property


        Public Property TopicID() As Integer
            Get
                Return mTopicID
            End Get
            Set(ByVal Value As Integer)
                mTopicID = Value
            End Set
        End Property


        Public Overrides Property Content() As String
            Get
                If mContent Is Nothing Then
                    Return ""
                Else
                    Return mContent
                End If
            End Get
            Set(ByVal Value As String)
                mContent = Value 'Me.ProcessLineBreaks(Value)
                If CanUseWikiText Then Cache = RenderedContent
            End Set
        End Property

        Public Property Cache() As String
            Get
                If mCache = "" Then
                    mCache = RenderedContent
                    If TopicID <> 0 And mCache.Length > 0 Then
                        Dim tc As New TopicController
                        tc.Update(Me)
                    End If
                    Return mCache
                Else
                    Return mCache
                End If
            End Get
            Set(ByVal Value As String)
                mCache = Value
            End Set
        End Property


        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = WikiData.make50(Value)
            End Set
        End Property

        Public Property Title() As String
            Get
                Return mTitle
            End Get
            Set(ByVal Value As String)
                mTitle = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal Value As String)
                mDescription = Value
            End Set
        End Property

        Public Property Keywords() As String
            Get
                Return mKeyWords
            End Get
            Set(ByVal Value As String)
                mKeyWords = Value
            End Set
        End Property

        Public Property UpdateDate() As Date
            Get
                Return mUpdateDate
            End Get
            Set(ByVal Value As Date)
                mUpdateDate = Value
            End Set
        End Property


        Public Property UpdatedBy() As String
            Get
                Return mUpdatedBy
            End Get
            Set(ByVal Value As String)
                mUpdatedBy = Value
            End Set
        End Property

        Public Property UpdatedByUsername() As String
            Get
                Return mUpdatedByUsername
            End Get
            Set(ByVal Value As String)
                mUpdatedByUsername = Value
            End Set
        End Property

        Public Property UpdatedByUserID() As Integer
            Get
                If (mUpdatedByUserID = Null.NullInteger) Then
                    Return -9999
                End If
                Return mUpdatedByUserID
            End Get
            Set(ByVal Value As Integer)
                mUpdatedByUserID = Value
            End Set
        End Property

        Public Property AllowDiscussions() As Boolean
            Get
                Return mAllowDiscussions
            End Get
            Set(ByVal Value As Boolean)
                mAllowDiscussions = Value
            End Set
        End Property
        Public Property AllowRatings() As Boolean
            Get
                Return mAllowRatings
            End Get
            Set(ByVal Value As Boolean)
                mAllowRatings = Value
            End Set
        End Property

        Public Property RatingOneCount() As Integer
            Get
                Return mRatingOneCount
            End Get
            Set(ByVal Value As Integer)
                mRatingOneCount = Value
            End Set
        End Property

        Public Property RatingTwoCount() As Integer
            Get
                Return mRatingTwoCount
            End Get
            Set(ByVal Value As Integer)
                mRatingTwoCount = Value
            End Set
        End Property

        Public Property RatingThreeCount() As Integer
            Get
                Return mRatingThreeCount
            End Get
            Set(ByVal Value As Integer)
                mRatingThreeCount = Value
            End Set
        End Property

        Public Property RatingFourCount() As Integer
            Get
                Return mRatingFourCount
            End Get
            Set(ByVal Value As Integer)
                mRatingFourCount = Value
            End Set
        End Property
        Public Property RatingFiveCount() As Integer
            Get
                Return mRatingFiveCount
            End Get
            Set(ByVal Value As Integer)
                mRatingFiveCount = Value
            End Set
        End Property

        Public Property RatingSixCount() As Integer
            Get
                Return mRatingSixCount
            End Get
            Set(ByVal Value As Integer)
                mRatingSixCount = Value
            End Set
        End Property
        Public Property RatingSevenCount() As Integer
            Get
                Return mRatingSevenCount
            End Get
            Set(ByVal Value As Integer)
                mRatingSevenCount = Value
            End Set
        End Property

        Public Property RatingEightCount() As Integer
            Get
                Return mRatingEightCount
            End Get
            Set(ByVal Value As Integer)
                mRatingEightCount = Value
            End Set
        End Property

        Public Property RatingNineCount() As Integer
            Get
                Return mRatingNineCount
            End Get
            Set(ByVal Value As Integer)
                mRatingNineCount = Value
            End Set
        End Property

        Public Property RatingTenCount() As Integer
            Get
                Return mRatingTenCount
            End Get
            Set(ByVal Value As Integer)
                mRatingTenCount = Value
            End Set
        End Property

        Public ReadOnly Property TenPointRatingsRecorded() As Integer
            Get
                Return RatingOneCount + RatingTwoCount + RatingThreeCount + RatingFourCount + RatingFiveCount + RatingSixCount + RatingSevenCount + RatingEightCount + RatingNineCount + RatingTenCount
            End Get
        End Property

        Public ReadOnly Property FivePointRatingsRecorded() As Integer
            Get
                Return RatingOneCount + RatingTwoCount + RatingThreeCount + RatingFourCount + RatingFiveCount
            End Get
        End Property

        Public ReadOnly Property FivePointAverage() As Double
            Get
                Return (CDbl(RatingOneCount) + CDbl(RatingTwoCount * 2) + CDbl(RatingThreeCount * 3) + CDbl(RatingFourCount * 4) + CDbl(RatingFiveCount * 5)) / CDbl(FivePointRatingsRecorded)
            End Get
        End Property

        Public ReadOnly Property TenPointAverage() As Double
            Get
                Return (CDbl(RatingOneCount) + CDbl(RatingTwoCount * 2) + CDbl(RatingThreeCount * 3) + CDbl(RatingFourCount * 4) + CDbl(RatingFiveCount * 5) + CDbl(RatingSixCount * 6) + CDbl(RatingSevenCount * 7) + CDbl(RatingEightCount * 8) + CDbl(RatingNineCount * 9) + CDbl(RatingTenCount * 10)) / CDbl(TenPointRatingsRecorded)
            End Get
        End Property

    End Class

End Namespace
