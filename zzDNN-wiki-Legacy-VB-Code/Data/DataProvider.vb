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

Imports System.Data

'DotNetNuke.Modules.Wiki
Namespace DotNetNuke.Modules.Wiki.Providers.Data

    Public MustInherit Class DataProvider

        ' Provider constants - eliminates need for Reflection later
        Private Const dataProviderType As String = "data"
        Private Const dataNameSpace As String = "DotNetNuke.Modules.Wiki.Providers.Data"
        Private Const dataAssemblyName As String = ""


        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject(dataProviderType, dataNameSpace, dataAssemblyName), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function


        'This is old code removed from the days of KTomics.
        'Public Shared Shadows Function Instance() As DataProvider
        '    Dim strCacheKey As String = dataNameSpace + "." + dataProviderType + "provider"

        '    ' Use the cache because the reflection used later is expensive
        '    Dim constructor As ConstructorInfo = CType(DotNetNuke.Common.Utilities.DataCache.GetCache(strCacheKey), ConstructorInfo)
        '    If constructor Is Nothing Then
        '        Dim providerConfiguration As DotNetNuke.Framework.Providers.ProviderConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(dataProviderType)
        '        Dim strTypeName As String = Nothing
        '        Try
        '            ' Override the typename if a ProviderName is specified ( this allows the application to load a different DataProvider assembly for custom modules)
        '            strTypeName = dataNameSpace & "." & providerConfiguration.DefaultProvider & ", " & dataAssemblyName & "." & providerConfiguration.DefaultProvider
        '            ' Use reflection to store the constructor of the class that implements DataProvider
        '            Dim t As Type = Type.GetType(strTypeName, True)
        '            constructor = t.GetConstructor(System.Type.EmptyTypes)
        '            ' Insert the type into the cache
        '            DotNetNuke.Common.Utilities.DataCache.SetCache(strCacheKey, constructor)
        '        Catch e As Exception
        '            Throw New ApplicationException("Could not load data provider " + strTypeName, e)
        '        End Try
        '    End If
        '    Return CType(constructor.Invoke(Nothing), DataProvider)
        'End Function

        ' Prototype method descriptions

        Public MustOverride Function Wiki_SettingsAdd(ByVal moduleID As Integer, ByVal contentEditorRoles As String, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal DefaultDiscussionMode As Boolean, ByVal DefaultRatingMode As Boolean, ByVal CommentNotifyRoles As String, ByVal CommentNotifyUsers As Boolean) As Integer
        Public MustOverride Sub Wiki_SettingsUpdate(ByVal settingID As Integer, ByVal moduleID As Integer, ByVal contentEditorRoles As String, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal DefaultDiscussionMode As Boolean, ByVal DefaultRatingMode As Boolean, ByVal CommentNotifyRoles As String, ByVal CommentNotifyUsers As Boolean)
        Public MustOverride Sub Wiki_SettingsDelete(ByVal settingID As Integer)
        Public MustOverride Function Wiki_SettingsGet(ByVal settingID As Integer) As IDataReader
        Public MustOverride Function Wiki_SettingsGetByModuleID(ByVal ModuleID As Integer) As IDataReader

        Public MustOverride Function Wiki_TopicAdd(ByVal moduleID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updatedByUserID As Integer, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal RatingOneCount As Integer, ByVal RatingTwoCount As Integer, ByVal RatingThreeCount As Integer, ByVal RatingFourCount As Integer, ByVal RatingFiveCount As Integer, ByVal RatingSixCount As Integer, ByVal RatingSevenCount As Integer, ByVal RatingEightCount As Integer, ByVal RatingNineCount As Integer, ByVal RatingTenCount As Integer) As Integer
        Public MustOverride Sub Wiki_TopicUpdate(ByVal moduleID As Integer, ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updatedByUserID As Integer, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal RatingOneCount As Integer, ByVal RatingTwoCount As Integer, ByVal RatingThreeCount As Integer, ByVal RatingFourCount As Integer, ByVal RatingFiveCount As Integer, ByVal RatingSixCount As Integer, ByVal RatingSevenCount As Integer, ByVal RatingEightCount As Integer, ByVal RatingNineCount As Integer, ByVal RatingTenCount As Integer)
        Public MustOverride Sub Wiki_TopicDelete(ByVal topicID As Integer)
        Public MustOverride Function Wiki_TopicGet(ByVal topicID As Integer) As IDataReader
        Public MustOverride Function Wiki_TopicGetAll() As IDataReader
        Public MustOverride Function Wiki_TopicGetAllByModuleID(ByVal ModuleID As Integer) As IDataReader
        Public MustOverride Function Wiki_TopicGetAllByModuleChangedWhen(ByVal ModuleID As Integer, ByVal DaysBack As Integer) As IDataReader
        Public MustOverride Function Wiki_TopicGetByNameForModule(ByVal ModuleID As Integer, ByVal name As String) As IDataReader

        Public MustOverride Function Wiki_TopicSearchWiki(ByVal Search As String, ByVal ModuleID As Integer) As IDataReader

        Public MustOverride Function Wiki_TopicHistoryAdd(ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updateByUserID As Integer) As Integer
        Public MustOverride Sub Wiki_TopicHistoryUpdate(ByVal topicHistoryID As Integer, ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updateByUserID As Integer)
        Public MustOverride Sub Wiki_TopicHistoryDelete(ByVal topicHistoryID As Integer)
        Public MustOverride Function Wiki_TopicHistoryGet(ByVal topicHistoryID As Integer) As IDataReader
        Public MustOverride Function Wiki_TopicHistoryGetHistoryForTopic(ByVal topicID As Integer) As IDataReader

        Public MustOverride Function Wiki_GetCommentParent(ByVal Id As Long) As DataRow
        Public MustOverride Function Wiki_GetCommentParents() As DataTable
        Public MustOverride Function Wiki_DeleteCommentParent(ByVal Id As Long) As Boolean
        Public MustOverride Function Wiki_EditCommentParent(ByVal Id As Long, ByVal ParentId As Long, ByVal Name As String) As Boolean
        Public MustOverride Function Wiki_AddCommentParent(ByVal ParentId As Long, ByVal Name As String) As Boolean
        Public MustOverride Function Wiki_ValidCommentParent(ByVal ParentId As Long) As Boolean
        Public MustOverride Function Wiki_GetCommentCount(ByVal ParentId As Long) As Integer
        Public MustOverride Function Wiki_GetComment(ByVal CommentId As Long) As DataRow
        Public MustOverride Function Wiki_GetComments() As DataTable
        Public MustOverride Function Wiki_GetCommentsByParent(ByVal ParentId As Long) As DataTable
        Public MustOverride Function Wiki_DeleteComments(ByVal ParentId As Long) As Boolean
        Public MustOverride Function Wiki_DeleteComment(ByVal CommentId As Long) As Boolean
        Public MustOverride Function Wiki_AddComment(ByVal ParentId As Integer, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Ip As String, ByVal EmailNotify As Boolean) As Boolean
        Public MustOverride Function Wiki_GetCommentNotifyUsers(ByVal ParentId As Long) As IDataReader

    End Class
End Namespace
