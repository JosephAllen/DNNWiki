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
Imports DotNetNuke.Modules.Wiki.Providers.Data

Namespace DotNetNuke.Modules.Wiki.Entities

    Public Class SettingsController

        Public Function Add(ByVal SettingsInfo As SettingsInfo) As Integer
            Return CType(DataProvider.Instance().Wiki_SettingsAdd(SettingsInfo.ModuleID, SettingsInfo.ContentEditorRoles, SettingsInfo.AllowDiscussions, SettingsInfo.AllowRatings, SettingsInfo.DefaultDiscussionMode, SettingsInfo.DefaultRatingMode, SettingsInfo.CommentNotifyRoles, SettingsInfo.CommentNotifyUsers), Integer)
        End Function

        Public Sub Update(ByVal SettingsInfo As SettingsInfo)
            DataProvider.Instance().Wiki_SettingsUpdate(SettingsInfo.SettingID, SettingsInfo.ModuleID, SettingsInfo.ContentEditorRoles, SettingsInfo.AllowDiscussions, SettingsInfo.AllowRatings, SettingsInfo.DefaultDiscussionMode, SettingsInfo.DefaultRatingMode, SettingsInfo.CommentNotifyRoles, SettingsInfo.CommentNotifyUsers)
        End Sub

        Public Sub Delete(ByVal settingID As Integer)
            DataProvider.Instance().Wiki_SettingsDelete(settingID)
        End Sub

        Public Function GetItem(ByVal settingID As Integer) As SettingsInfo
            Dim infoObject As SettingsInfo = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_SettingsGet(settingID)
                infoObject = CType(DotNetNuke.Common.Utilities.CBO.FillObject(idr, GetType(SettingsInfo)), SettingsInfo)
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoObject
        End Function
        Public Function GetByModuleID(ByVal ModuleID As Integer) As SettingsInfo
            Dim infoObject As SettingsInfo = Nothing
            Dim idr As IDataReader = Nothing
            Try
                idr = DataProvider.Instance().Wiki_SettingsGetByModuleID(ModuleID)
                infoObject = CType(DotNetNuke.Common.Utilities.CBO.FillObject(idr, GetType(SettingsInfo)), SettingsInfo)
            Finally
                If Not idr Is Nothing Then
                    idr.Close()
                    idr.Dispose()
                    idr = Nothing
                End If
            End Try
            Return infoObject
        End Function
    End Class

End Namespace
