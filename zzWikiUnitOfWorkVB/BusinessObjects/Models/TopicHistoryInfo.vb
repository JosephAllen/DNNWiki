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


Namespace DotNetNuke.Modules.Wiki.Entities

    Public Class TopicHistoryInfo
        Inherits WikiData
        ' Entity member variables for database table TopicHistory

        Private mTopicHistoryID As Integer
        Private mTopicID As Integer
        Private mContent As String
        Private mCache As String
        Private mName As String
        Private mTitle As String
        Private mDescription As String
        Private mKeywords As String
        Private mUpdateDate As Date
        Private mUpdatedBy As String
        Private mUpdatedByUserID As Integer
        Private mUpdatedByUsername As String

        ' Calculated Expression member variables for database table TopicHistory


        ' Entity properties for database table TopicHistory

        Public Property TopicHistoryID() As Integer
            Get
                Return mTopicHistoryID
            End Get
            Set(ByVal Value As Integer)
                mTopicHistoryID = Value
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
                Return mContent
            End Get
            Set(ByVal Value As String)
                mContent = Value 'Me.ProcessLineBreaks(Value)
                If CanUseWikiText Then Cache = RenderedContent
            End Set
        End Property

        Public Property Cache() As String
            Get
                If mCache Is Nothing Then
                    mCache = RenderedContent
                    If TopicHistoryID <> 0 And mCache.Length > 0 Then
                        Dim thc As New TopicHistoryController
                        thc.Update(Me)
                    End If
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
                mName = WikiData.make255(Value)
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

        Public Property UpdatedByUserID() As Integer
            Get
                Return mUpdatedByUserID
            End Get
            Set(ByVal Value As Integer)
                mUpdatedByUserID = Value
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
    End Class

End Namespace
