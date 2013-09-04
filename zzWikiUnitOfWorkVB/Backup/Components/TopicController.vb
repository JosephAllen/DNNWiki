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

Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Modules.Wiki.Providers.Data

Namespace DotNetNuke.Modules.Wiki.Entities
    Public Class TopicController
        Private mAppCache As System.Web.Caching.Cache = Nothing
        Public Sub New(ByRef AppCache As System.Web.Caching.Cache)
            mAppCache = AppCache
        End Sub
        Public Sub New()

        End Sub
        Private Sub InsertUpdateAppCache(ByRef TopicInfo As TopicInfo)
            If Not mAppCache Is Nothing Then
                If (mAppCache.Get("WWWiki-Topic-" + TopicInfo.TopicID.ToString) Is Nothing) Then
                    mAppCache.Insert("WWWiki-Topic-" + TopicInfo.TopicID.ToString, TopicInfo, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(1, 0, 0), System.Web.Caching.CacheItemPriority.Low, Nothing)
                Else
                    mAppCache.Item("WWWiki-Topic-" + TopicInfo.TopicID.ToString) = TopicInfo
                End If
            End If
        End Sub
        Public Function Add(ByVal TopicInfo As TopicInfo) As Integer
            Dim id As Integer = CType(DataProvider.Instance().Wiki_TopicAdd(TopicInfo.ModuleID, TopicInfo.Content, TopicInfo.Cache, TopicInfo.Name, TopicInfo.Title, TopicInfo.Description, TopicInfo.Keywords, TopicInfo.UpdateDate, TopicInfo.UpdatedBy, TopicInfo.UpdatedByUserID, TopicInfo.AllowDiscussions, TopicInfo.AllowRatings, TopicInfo.RatingOneCount, TopicInfo.RatingTwoCount, TopicInfo.RatingThreeCount, TopicInfo.RatingFourCount, TopicInfo.RatingFiveCount, TopicInfo.RatingSixCount, TopicInfo.RatingSevenCount, TopicInfo.RatingEightCount, TopicInfo.RatingNineCount, TopicInfo.RatingTenCount), Integer)
            TopicInfo.TopicID = id
            InsertUpdateAppCache(TopicInfo)
            Return id
        End Function

        Public Sub Update(ByVal TopicInfo As TopicInfo)
            DataProvider.Instance().Wiki_TopicUpdate(TopicInfo.ModuleID, TopicInfo.TopicID, TopicInfo.Content, TopicInfo.Cache, TopicInfo.Name, TopicInfo.Title, TopicInfo.Description, TopicInfo.Keywords, TopicInfo.UpdateDate, TopicInfo.UpdatedBy, TopicInfo.UpdatedByUserID, TopicInfo.AllowDiscussions, TopicInfo.AllowRatings, TopicInfo.RatingOneCount, TopicInfo.RatingTwoCount, TopicInfo.RatingThreeCount, TopicInfo.RatingFourCount, TopicInfo.RatingFiveCount, TopicInfo.RatingSixCount, TopicInfo.RatingSevenCount, TopicInfo.RatingEightCount, TopicInfo.RatingNineCount, TopicInfo.RatingTenCount)
            InsertUpdateAppCache(TopicInfo)
        End Sub

        Public Sub Delete(ByVal topicID As Integer)
            DataProvider.Instance().Wiki_TopicDelete(topicID)
            If Not mAppCache Is Nothing Then mAppCache.Remove("WWWiki-Topic-" + topicID.ToString)
        End Sub

        Public Function GetItem(ByVal topicID As Integer) As TopicInfo
            Dim infoObject As TopicInfo = Nothing
            If Not mAppCache Is Nothing Then
                infoObject = CType(mAppCache.Get("WWWiki-Topic-" + topicID.ToString), TopicInfo)
            End If
            If infoObject Is Nothing Then
                Dim idr As IDataReader = Nothing
                Try
                    idr = DataProvider.Instance().Wiki_TopicGet(topicID)
                    infoObject = CType(DotNetNuke.Common.Utilities.CBO.FillObject(idr, GetType(TopicInfo)), TopicInfo)
                Finally
                    If Not idr Is Nothing Then
                        idr.Close()
                        idr.Dispose()
                        idr = Nothing
                    End If
                End Try
            End If
            Return infoObject
        End Function


        ' Select query methods for database table Topic

        Public Function GetAll() As ArrayList
            Dim infoList As ArrayList = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicGetAll()
                infoList = DotNetNuke.Common.Utilities.CBO.FillCollection(idr, GetType(TopicInfo))
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoList
        End Function

        Public Function GetAllByModuleID(ByVal ModuleID As Integer) As ArrayList
            Dim infoList As ArrayList = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicGetAllByModuleID(ModuleID)
                infoList = DotNetNuke.Common.Utilities.CBO.FillCollection(idr, GetType(TopicInfo))
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoList
        End Function

        Public Function GetAllByModuleChangedWhen(ByVal ModuleID As Integer, ByVal DaysBack As Integer) As ArrayList
            Dim infoList As ArrayList = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicGetAllByModuleChangedWhen(ModuleID, DaysBack)
                infoList = DotNetNuke.Common.Utilities.CBO.FillCollection(idr, GetType(TopicInfo))
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoList
        End Function

        Public Function GetByNameForModule(ByVal ModuleID As Integer, ByVal name As String) As TopicInfo
            Dim infoObject As TopicInfo = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicGetByNameForModule(ModuleID, name)
                infoObject = CType(DotNetNuke.Common.Utilities.CBO.FillObject(idr, GetType(TopicInfo)), TopicInfo)
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoObject
        End Function

        Public Function SearchWiki(ByVal Search As String, ByVal ModuleID As Integer) As ArrayList
            Dim infoList As ArrayList = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicSearchWiki(Search, ModuleID)
                infoList = DotNetNuke.Common.Utilities.CBO.FillCollection(idr, GetType(TopicInfo))
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoList
        End Function
    End Class

End Namespace
