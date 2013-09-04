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


Namespace DotNetNuke.Modules.Wiki.Entities

    Public Class SettingsInfo

        ' Entity member variables for database table Settings

        Private mSettingID As Integer
        Private mModuleID As Integer
        Private mContentEditorRoles As String
        Private mAllowDiscussions As Boolean
        Private mAllowRatings As Boolean
        Private mDefaultRatingsMode As Boolean
        Private mDefaultCommentsMode As Boolean
        Private mCommentNotifyRoles As String = String.Empty
        Private mCommentNotifyUsers As Boolean

        ' Calculated Expression member variables for database table Settings


        ' Entity properties for database table Settings

        Public Property SettingID() As Integer
            Get
                Return mSettingID
            End Get
            Set(ByVal Value As Integer)
                mSettingID = Value
            End Set
        End Property


        Public Property ModuleID() As Integer
            Get
                Return mModuleID
            End Get
            Set(ByVal Value As Integer)
                mModuleID = Value
            End Set
        End Property


        Public Property ContentEditorRoles() As String
            Get
                Return mContentEditorRoles
            End Get
            Set(ByVal Value As String)
                mContentEditorRoles = Value
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

        Public Property DefaultDiscussionMode() As Boolean
            Get
                Return mDefaultCommentsMode
            End Get
            Set(ByVal Value As Boolean)
                mDefaultCommentsMode = Value
            End Set
        End Property

        Public Property DefaultRatingMode() As Boolean
            Get
                Return mDefaultRatingsMode
            End Get
            Set(ByVal Value As Boolean)
                mDefaultRatingsMode = Value
            End Set
        End Property

        Public Property CommentNotifyRoles() As String
            Get
                Return mCommentNotifyRoles
            End Get
            Set(ByVal value As String)
                mCommentNotifyRoles = value
            End Set
        End Property

        Public Property CommentNotifyUsers() As Boolean
            Get
                Return mCommentNotifyUsers
            End Get
            Set(ByVal Value As Boolean)
                mCommentNotifyUsers = Value
            End Set
        End Property

    End Class

End Namespace
