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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Search
Imports DotNetNuke.Modules.Wiki.Entities
Imports DotNetNuke.Entities.Users
Imports System.IO
Imports System.Xml

Namespace DotNetNuke.Modules.Wiki
    Public Class Controller
        Implements ISearchable
        Implements IPortable
        'Implements IUpgradeable

        Dim SharedResourceFile As String = Common.Globals.ApplicationPath & "/DesktopModules/Wiki/" & Localization.LocalResourceDirectory & "/" & Localization.LocalSharedResourceFile


        Public Function GetSearchItems(ByVal ModInfo As ModuleInfo) As SearchItemInfoCollection Implements ISearchable.GetSearchItems

            Dim tc As New TopicController
            Dim SearchItemCollection As New SearchItemInfoCollection
            Dim Topics As ArrayList = tc.GetAllByModuleID(ModInfo.ModuleID)
            Dim uc As New UserController
            Dim topic As TopicInfo
            For Each topic In Topics
                Dim SearchItem As New SearchItemInfo

                Dim strContent As String
                Dim strDescription As String
                Dim strTitle As String
                If topic.Title.Trim() > String.Empty Then
                    strTitle = topic.Title
                Else
                    strTitle = topic.Name
                End If
                If (topic.Cache <> Nothing) Then
                    strContent = topic.Cache
                    strContent += " " + topic.Keywords
                    strContent += " " + topic.Description

                    strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Cache), False), 100, Localization.GetString("Dots", SharedResourceFile))

                Else
                    strContent = topic.Content
                    strContent += " " + topic.Keywords
                    strContent += " " + topic.Description

                    strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Content), False), 100, Localization.GetString("Dots", SharedResourceFile))
                End If
                Dim userID As Integer

                userID = Null.NullInteger
                If topic.UpdatedByUserID <> -9999 Then
                    userID = topic.UpdatedByUserID
                End If
                SearchItem = _
                 New SearchItemInfo(strTitle, strDescription, userID, topic.UpdateDate, ModInfo.ModuleID, topic.Name, strContent, "topic=" & DotNetNuke.Modules.Wiki.Entities.WikiData.EncodeTitle(topic.Name))
                '    New SearchItemInfo(ModInfo.ModuleTitle & "-" & strTitle, strDescription, userID, topic.UpdateDate, ModInfo.ModuleID, topic.Name, strContent, _
                '                       "topic=" & DotNetNuke.Modules.Wiki.Entities.WikiData.EncodeTitle(topic.Name))

               
                SearchItemCollection.Add(SearchItem)
            Next

            Return SearchItemCollection

        End Function


        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule
            Dim tc As New TopicController
            Dim Topics As ArrayList = tc.GetAllByModuleID(ModuleID)

            Dim mc As New ModuleController()
            Dim Settings As Hashtable = mc.GetModuleSettings(ModuleID)

            Dim strXML As New StringWriter
            Dim Writer As XmlWriter = New XmlTextWriter(strXML)
            Writer.WriteStartElement("Wiki")

            Writer.WriteStartElement("Settings")
            For Each item As DictionaryEntry In Settings
                Writer.WriteStartElement("Setting")
                Writer.WriteAttributeString("Name", CStr(item.Key))
                Writer.WriteAttributeString("Value", CStr(item.Value))
                Writer.WriteEndElement()
            Next
            Writer.WriteEndElement()

            Writer.WriteStartElement("Topics")
            For Each Topic As TopicInfo In Topics
                Writer.WriteStartElement("Topic")
                Writer.WriteAttributeString("AllowDiscussions", Topic.AllowDiscussions.ToString)
                Writer.WriteAttributeString("AllowRatings", Topic.AllowRatings.ToString)
                Writer.WriteAttributeString("Content", Topic.Content)
                Writer.WriteAttributeString("Description", Topic.Description)
                Writer.WriteAttributeString("Keywords", Topic.Keywords)
                Writer.WriteAttributeString("Name", Topic.Name)
                Writer.WriteAttributeString("Title", Topic.Title)
                Writer.WriteAttributeString("UpdateDate", Topic.UpdateDate.ToString("g"))
                Writer.WriteAttributeString("UpdatedBy", Topic.UpdatedBy)
                Writer.WriteAttributeString("UpdatedByUserID", Topic.UpdatedByUserID.ToString("g"))
                Writer.WriteEndElement()
            Next
            Writer.WriteEndElement()

            Writer.WriteEndElement()
            Writer.Close()

            Return strXML.ToString
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) _
            Implements IPortable.ImportModule
            Dim node As XmlNode
            Dim nodes As XmlNode = GetContent(Content, "Wiki")
            Dim objModules As New ModuleController
            For Each node In nodes.SelectSingleNode("Settings")
                objModules.UpdateModuleSetting(ModuleID, node.Attributes("Name").Value, _
                                                node.Attributes("Value").Value)
            Next
            Dim tc As New TopicController

            'clean up
            Dim Topics As ArrayList = tc.GetAllByModuleID(ModuleID)
            For Each Topic As TopicInfo In Topics
                tc.Delete(Topic.TopicID)
            Next

            Try

                For Each node In nodes.SelectNodes("Topics/Topic")
                    Dim topic As New TopicInfo
                    topic.PortalSettings = Portals.PortalController.GetCurrentPortalSettings()
                    topic.AllowDiscussions = Boolean.Parse(node.Attributes("AllowDiscussions").Value)
                    topic.AllowRatings = Boolean.Parse(node.Attributes("AllowRatings").Value)
                    topic.Content = node.Attributes("Content").Value
                    topic.Description = node.Attributes("Description").Value
                    topic.Keywords = node.Attributes("Keywords").Value
                    topic.ModuleID = ModuleID
                    'Here we need to define the TabID otherwise the import won't work until the content is saved again.
                    Dim mc As ModuleController = New ModuleController()
                    Dim mi As ModuleInfo = mc.GetModule(ModuleID, -1)
                    topic.TabID = mi.TabID

                    topic.Name = node.Attributes("Name").Value
                    topic.RatingOneCount = 0
                    topic.RatingTwoCount = 0
                    topic.RatingThreeCount = 0
                    topic.RatingFourCount = 0
                    topic.RatingFiveCount = 0
                    topic.RatingSixCount = 0
                    topic.RatingSevenCount = 0
                    topic.RatingEightCount = 0
                    topic.RatingNineCount = 0
                    topic.RatingTenCount = 0
                    topic.Title = node.Attributes("Title").Value
                    topic.UpdateDate = DateTime.Parse(node.Attributes("UpdateDate").Value)
                    topic.UpdatedBy = node.Attributes("UpdatedBy").Value
                    topic.UpdatedByUserID = Integer.Parse(node.Attributes("UpdatedByUserID").Value)
                    tc.Add(topic)
                Next

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex)
            End Try

        End Sub

        'Public Function UpgradeModule(ByVal Version As String) As String Implements IUpgradeable.UpgradeModule
        '    InitPermissions()
        '    Return Version
        'End Function

        'Private Sub InitPermissions()
        '    Dim EditContent As Boolean

        '    Dim moduleDefId As Integer
        '    Dim pc As New PermissionController
        '    Dim permissions As ArrayList = pc.GetPermissionByCodeAndKey("WIKI", Nothing)
        '    Dim dc As New DesktopModuleController
        '    Dim desktopInfo As DesktopModuleInfo
        '    desktopInfo = dc.GetDesktopModuleByModuleName("Wiki")
        '    Dim mc As New ModuleDefinitionController
        '    Dim mInfo As ModuleDefinitionInfo
        '    mInfo = mc.GetModuleDefinitionByName(desktopInfo.DesktopModuleID, "Wiki")
        '    moduleDefId = mInfo.ModuleDefID
        '    For Each p As PermissionInfo In permissions
        '        If p.PermissionKey = "EDIT_CONTENT" And p.ModuleDefID = moduleDefId Then _
        '            EditContent = True
        '    Next
        '    If Not EditContent Then
        '        Dim p As New PermissionInfo
        '        p.ModuleDefID = moduleDefId
        '        p.PermissionCode = "WIKI"
        '        p.PermissionKey = "EDIT_CONTENT"
        '        p.PermissionName = "Edit Content"
        '        pc.AddPermission(p)
        '    End If
        'End Sub
    End Class
End Namespace
