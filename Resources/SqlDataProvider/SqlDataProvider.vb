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


Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers

Namespace DotNetNuke.Modules.Wiki.Providers.Data

    Public Class SqlDataProvider
        Inherits DotNetNuke.Modules.Wiki.Providers.Data.DataProvider


        Private Const ProviderType As String = "data"

        Private _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String


        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)
            ' Read the attributes for this provider

            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub


        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

        Public Function GetNull(ByVal field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(field, DBNull.Value)
        End Function

        ' Data Access methods implementation for Wiki_Settings

        Public Overrides Function Wiki_SettingsAdd(ByVal moduleID As Integer, ByVal contentEditorRoles As String, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal DefaultDiscussionMode As Boolean, ByVal DefaultRatingMode As Boolean, ByVal CommentNotifyRoles As String, ByVal CommentNotifyUsers As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_SettingsAdd", moduleID, contentEditorRoles, AllowDiscussion, AllowRating, DefaultDiscussionMode, DefaultRatingMode, CommentNotifyRoles, CommentNotifyUsers), Integer)
        End Function

        Public Overrides Sub Wiki_SettingsUpdate(ByVal settingID As Integer, ByVal moduleID As Integer, ByVal contentEditorRoles As String, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal DefaultDiscussionMode As Boolean, ByVal DefaultRatingMode As Boolean, ByVal CommentNotifyRoles As String, ByVal CommentNotifyUsers As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_SettingsUpdate", settingID, moduleID, contentEditorRoles, AllowDiscussion, AllowRating, DefaultDiscussionMode, DefaultRatingMode, CommentNotifyRoles, CommentNotifyUsers)
        End Sub

        Public Overrides Sub Wiki_SettingsDelete(ByVal settingID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_SettingsDelete", settingID)
        End Sub

        Public Overrides Function Wiki_SettingsGet(ByVal settingID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_SettingsGet", settingID), IDataReader)
        End Function

        Public Overrides Function Wiki_SettingsGetByModuleID(ByVal ModuleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_SettingsGetByModuleID", ModuleID), IDataReader)
        End Function

        ' Data Access methods implementation for Wiki_Topic

        Public Overrides Function Wiki_TopicAdd(ByVal moduleID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updatedByUserId As Integer, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal RatingOneCount As Integer, ByVal RatingTwoCount As Integer, ByVal RatingThreeCount As Integer, ByVal RatingFourCount As Integer, ByVal RatingFiveCount As Integer, ByVal RatingSixCount As Integer, ByVal RatingSevenCount As Integer, ByVal RatingEightCount As Integer, ByVal RatingNineCount As Integer, ByVal RatingTenCount As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicAdd", moduleID, GetNull(content), GetNull(cache), GetNull(name), GetNull(title), GetNull(description), GetNull(keywords), updateDate, updatedBy, updatedByUserId, AllowDiscussion, AllowRating, RatingOneCount, RatingTwoCount, RatingThreeCount, RatingFourCount, RatingFiveCount, RatingSixCount, RatingSevenCount, RatingEightCount, RatingNineCount, RatingTenCount), Integer)
        End Function

        Public Overrides Sub Wiki_TopicUpdate(ByVal moduleID As Integer, ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updatedByUserId As Integer, ByVal AllowDiscussion As Boolean, ByVal AllowRating As Boolean, ByVal RatingOneCount As Integer, ByVal RatingTwoCount As Integer, ByVal RatingThreeCount As Integer, ByVal RatingFourCount As Integer, ByVal RatingFiveCount As Integer, ByVal RatingSixCount As Integer, ByVal RatingSevenCount As Integer, ByVal RatingEightCount As Integer, ByVal RatingNineCount As Integer, ByVal RatingTenCount As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicUpdate", moduleID, topicID, GetNull(content), GetNull(cache), GetNull(name), GetNull(title), GetNull(description), GetNull(keywords), updateDate, updatedBy, updatedByUserId, AllowDiscussion, AllowRating, RatingOneCount, RatingTwoCount, RatingThreeCount, RatingFourCount, RatingFiveCount, RatingSixCount, RatingSevenCount, RatingEightCount, RatingNineCount, RatingTenCount)
        End Sub

        Public Overrides Sub Wiki_TopicDelete(ByVal topicID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicDelete", topicID)
        End Sub

        Public Overrides Function Wiki_TopicGet(ByVal topicID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicGet", topicID), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicGetAll() As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicGetAll"), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicGetAllByModuleID(ByVal ModuleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicGetAllByModuleID", ModuleID), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicGetAllByModuleChangedWhen(ByVal ModuleID As Integer, ByVal DaysBack As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicGetAllByModuleChangedWhen", ModuleID, DaysBack), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicGetByNameForModule(ByVal ModuleID As Integer, ByVal name As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicGetByNameForModule", ModuleID, name), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicSearchWiki(ByVal Search As String, ByVal ModuleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicSearchWiki", Search, ModuleID), IDataReader)
        End Function

        ' Data Access methods implementation for Wiki_TopicHistory

        Public Overrides Function Wiki_TopicHistoryAdd(ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updateByUserID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicHistoryAdd", topicID, GetNull(content), GetNull(cache), GetNull(name), GetNull(title), GetNull(description), GetNull(keywords), updateDate, updatedBy, updateByUserID), Integer)
        End Function

        Public Overrides Sub Wiki_TopicHistoryUpdate(ByVal topicHistoryID As Integer, ByVal topicID As Integer, ByVal content As String, ByVal cache As String, ByVal name As String, ByVal title As String, ByVal description As String, ByVal keywords As String, ByVal updateDate As Date, ByVal updatedBy As String, ByVal updateByUserID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicHistoryUpdate", topicHistoryID, topicID, GetNull(content), GetNull(cache), GetNull(name), GetNull(title), GetNull(description), GetNull(keywords), updateDate, updatedBy, updateByUserID)
        End Sub

        Public Overrides Sub Wiki_TopicHistoryDelete(ByVal topicHistoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicHistoryDelete", topicHistoryID)
        End Sub

        Public Overrides Function Wiki_TopicHistoryGet(ByVal topicHistoryID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicHistoryGet", topicHistoryID), IDataReader)
        End Function

        Public Overrides Function Wiki_TopicHistoryGetHistoryForTopic(ByVal topicID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Wiki_TopicHistoryGetHistoryForTopic", topicID), IDataReader)
        End Function

        ' Data Access methods implementation for Wiki_TopicComments

        Public Overrides Function Wiki_AddComment(ByVal ParentId As Integer, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Ip As String, ByVal EmailNotify As Boolean) As Boolean
            Try
                Dim params(5) As SqlParameter
                params(0) = New SqlParameter("@a", ParentId)
                params(1) = New SqlParameter("@b", Name)
                params(2) = New SqlParameter("@c", Email)
                params(3) = New SqlParameter("@d", Comment)
                params(4) = New SqlParameter("@e", Ip)
                params(5) = New SqlParameter("@f", EmailNotify)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "insert into " & DatabaseOwner & ObjectQualifier & "Wiki_Comments (ParentId,Name,Email,Comment,Ip,EmailNotify) values (@a,@b,@c,@d,@e,@f)", params)
                Return True
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_DeleteComment(ByVal CommentId As Long) As Boolean
            Try
                Dim params(1) As SqlParameter
                params(0) = New SqlParameter("@a", CommentId)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "delete from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where CommentId=@a", params)
                Return True
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_DeleteComments(ByVal ParentId As Long) As Boolean
            Try
                Dim params(1) As SqlParameter
                params(0) = New SqlParameter("@a", ParentId)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "delete from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where ParentId=@a", params)
                Return True
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_GetCommentsByParent(ByVal ParentId As Long) As DataTable
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select * from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where ParentId=@a order by Datetime desc", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim SqlParameter As SqlParameter = SqlCommand.Parameters.Add("@a", System.Data.SqlDbType.Int)
                SqlParameter.Value = ParentId
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return dataTable
            Catch e As Exception
                Return New DataTable
            End Try
        End Function

        Public Overrides Function Wiki_GetComments() As DataTable
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select * from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return dataTable
            Catch e As Exception
                Return New DataTable
            End Try
        End Function

        Public Overrides Function Wiki_GetComment(ByVal CommentId As Long) As DataRow
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select * from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where CommentId=@a", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim SqlParameter As SqlParameter = SqlCommand.Parameters.Add("@a", System.Data.SqlDbType.Int)
                SqlParameter.Value = CommentId
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return dataTable.Rows(0)
            Catch e As Exception
                Return Nothing
            End Try
        End Function

        Public Overrides Function Wiki_GetCommentCount(ByVal ParentId As Long) As Integer
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select Count(CommentId) from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where ParentId=@a", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim SqlParameter As SqlParameter = SqlCommand.Parameters.Add("@a", System.Data.SqlDbType.Int)
                SqlParameter.Value = ParentId
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return CType(dataTable.Rows(0)(0), Integer)
            Catch e As Exception
                Return 0
            End Try
        End Function

        Public Overrides Function Wiki_ValidCommentParent(ByVal ParentId As Long) As Boolean
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select ParentId from " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents where ParentId=@a", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim SqlParameter As SqlParameter = SqlCommand.Parameters.Add("@a", System.Data.SqlDbType.Int)
                SqlParameter.Value = ParentId
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                If dataTable.Rows.Count = 1 Then
                    Return True
                Else
                    Return False
                End If
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_AddCommentParent(ByVal ParentId As Long, ByVal Name As String) As Boolean
            Try
                Dim params(1) As SqlParameter
                params(0) = New SqlParameter("@a", ParentId)
                params(1) = New SqlParameter("@b", Name)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "insert into " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents (ParentId,Name) values(@a,@b)", params)
                Return True
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_EditCommentParent(ByVal Id As Long, ByVal ParentId As Long, ByVal Name As String) As Boolean
            Try
                Dim params(3) As SqlParameter
                params(0) = New SqlParameter("@a", Id)
                params(1) = New SqlParameter("@b", ParentId)
                params(2) = New SqlParameter("@c", Name)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents set ParentId=@b,Name=@c where CommentParentId=@a", params)
                Return True
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_DeleteCommentParent(ByVal Id As Long) As Boolean
            Try
                Dim params(1) As SqlParameter
                params(0) = New SqlParameter("@id", Id)
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "delete from " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents where CommentParentId=@id", params)
                Return Me.Wiki_DeleteComments(Id)
            Catch e As Exception
                Return False
            End Try
        End Function

        Public Overrides Function Wiki_GetCommentParents() As DataTable
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select * from " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return dataTable
            Catch e As Exception
                Return New DataTable
            End Try
        End Function

        Public Overrides Function Wiki_GetCommentParent(ByVal Id As Long) As DataRow
            Try
                Dim SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                SqlConnection.Open()
                Dim SqlCommand As SqlCommand = New SqlCommand("select * from " & DatabaseOwner & ObjectQualifier & "Wiki_CommentParents where CommentParentId=@id", SqlConnection)
                SqlCommand.CommandType = CommandType.Text
                Dim SqlParameter As SqlParameter = SqlCommand.Parameters.Add("@id", System.Data.SqlDbType.Int)
                SqlParameter.Value = Id
                Dim dataTable As DataTable = New DataTable
                Dim SqlDataAdapter As SqlDataAdapter = New SqlDataAdapter
                SqlDataAdapter.SelectCommand = SqlCommand
                SqlDataAdapter.Fill(dataTable)
                SqlConnection.Close()
                Return dataTable.Rows(0)
            Catch e As Exception
                Return Nothing
            End Try
        End Function

        Public Overrides Function Wiki_GetCommentNotifyUsers(ByVal ParentId As Long) As IDataReader
            Dim SqlCommandText As String = "select DISTINCT(Email) from " & DatabaseOwner & ObjectQualifier & "Wiki_Comments where ParentId=" & ParentId & " AND EmailNotify = 1"

            Return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, SqlCommandText)


        End Function
    End Class

End Namespace