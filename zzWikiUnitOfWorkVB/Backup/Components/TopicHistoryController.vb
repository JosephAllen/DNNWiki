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

Imports System.Collections
Imports System.Data
Imports DotNetNuke.Modules.Wiki.Providers.Data

Namespace DotNetNuke.Modules.Wiki.Entities
    Public Class TopicHistoryController

        Public Function Add(ByVal TopicHistoryInfo As TopicHistoryInfo) As Integer
            Return CType(DataProvider.Instance().Wiki_TopicHistoryAdd(TopicHistoryInfo.TopicID, TopicHistoryInfo.Content, TopicHistoryInfo.Cache, TopicHistoryInfo.Name, TopicHistoryInfo.Title, TopicHistoryInfo.Description, TopicHistoryInfo.Keywords, TopicHistoryInfo.UpdateDate, TopicHistoryInfo.UpdatedBy, TopicHistoryInfo.UpdatedByUserID), Integer)
        End Function

        Public Sub Update(ByVal TopicHistoryInfo As TopicHistoryInfo)
            DataProvider.Instance().Wiki_TopicHistoryUpdate(TopicHistoryInfo.TopicHistoryID, TopicHistoryInfo.TopicID, TopicHistoryInfo.Content, TopicHistoryInfo.Cache, TopicHistoryInfo.Name, TopicHistoryInfo.Title, TopicHistoryInfo.Description, TopicHistoryInfo.Keywords, TopicHistoryInfo.UpdateDate, TopicHistoryInfo.UpdatedBy, TopicHistoryInfo.UpdatedByUserID)
        End Sub

        Public Sub Delete(ByVal topicHistoryID As Integer)
            DataProvider.Instance().Wiki_TopicHistoryDelete(topicHistoryID)
        End Sub

        Public Function GetItem(ByVal topicHistoryID As Integer) As TopicHistoryInfo
            Dim infoObject As TopicHistoryInfo = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicHistoryGet(topicHistoryID)
                infoObject = CType(DotNetNuke.Common.Utilities.CBO.FillObject(idr, GetType(TopicHistoryInfo)), TopicHistoryInfo)
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoObject
        End Function

        ' Select query methods for database table TopicHistory

        Public Function GetHistoryForTopic(ByVal topicID As Integer) As ArrayList
            Dim infoList As ArrayList = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_TopicHistoryGetHistoryForTopic(topicID)
                infoList = DotNetNuke.Common.Utilities.CBO.FillCollection(idr, GetType(TopicHistoryInfo))
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
