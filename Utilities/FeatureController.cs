/*
' Copyright (c) 2013 DotNetNuke
' http://www.dotnetnuke.com
' All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Wiki.BusinessObjects;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Search;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Xml;

namespace DotNetNuke.Modules.Wiki.Utilities
{
    /// -----------------------------------------------------------------------------
    /// <summary> The Controller class for DNNModule1
    ///
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements.
    ///
    /// The IPortable interface is used to import/export content from a DNN module
    ///
    /// The ISearchable interface is used by DNN to index the content of a module
    ///
    /// The IUpgradeable interface allows module developers to execute code during the upgrade
    /// process for a module.
    ///
    /// Below you will find stubbed out implementations of each, uncomment and populate with your
    /// own data </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController : IPortable, ISearchable//, IUpgradeable
    {
        //Implements IUpgradeable

        private string SharedResourceFile =
            DotNetNuke.Common.Globals.ApplicationPath + "/DesktopModules/Wiki/" + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile;

        public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                TopicBO topicBo = new TopicBO(uof);

                SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
                var topics = topicBo.GetAllByModuleID(ModInfo.ModuleID);
                UserController uc = new UserController();

                foreach (var topic in topics)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo();

                    string strContent = null;
                    string strDescription = null;
                    string strTitle = null;
                    if (!topic.Title.Trim().Equals(string.Empty))
                    {
                        strTitle = topic.Title;
                    }
                    else
                    {
                        strTitle = topic.Name;
                    }

                    if ((topic.Cache != null))
                    {
                        strContent = topic.Cache;
                        strContent += " " + topic.Keywords;
                        strContent += " " + topic.Description;

                        strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Cache), false), 100,
                            Localization.GetString("Dots", SharedResourceFile));
                    }
                    else
                    {
                        strContent = topic.Content;
                        strContent += " " + topic.Keywords;
                        strContent += " " + topic.Description;

                        strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Content), false), 100,
                            Localization.GetString("Dots", SharedResourceFile));
                    }
                    int userID = 0;

                    userID = Null.NullInteger;
                    if (topic.UpdatedByUserID != -9999)
                    {
                        userID = topic.UpdatedByUserID;
                    }
                    SearchItem = new SearchItemInfo(strTitle, strDescription, userID, topic.UpdateDate, ModInfo.ModuleID, topic.Name, strContent,
                        "topic=" + WikiMarkup.EncodeTitle(topic.Name));

                    // New SearchItemInfo(ModInfo.ModuleTitle & "-" & strTitle, strDescription,
                    // userID, topic.UpdateDate, ModInfo.ModuleID, topic.Name, strContent, _
                    // "topic=" & WikiMarkup.EncodeTitle(topic.Name))

                    SearchItemCollection.Add(SearchItem);
                }

                return SearchItemCollection;
            }
        }

        public string ExportModule(int ModuleID)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                TopicBO topicBo = new TopicBO(uof);
                var topics = topicBo.GetAllByModuleID(ModuleID);

                ModuleController mc = new ModuleController();
                Hashtable Settings = mc.GetModuleSettings(ModuleID);

                StringWriter strXML = new StringWriter();
                XmlWriter Writer = new XmlTextWriter(strXML);
                Writer.WriteStartElement("Wiki");

                Writer.WriteStartElement("Settings");
                foreach (DictionaryEntry item in Settings)
                {
                    Writer.WriteStartElement("Setting");
                    Writer.WriteAttributeString("Name", Convert.ToString(item.Key));
                    Writer.WriteAttributeString("Value", Convert.ToString(item.Value));
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();

                Writer.WriteStartElement("Topics");
                foreach (var topic in topics)
                {
                    Writer.WriteStartElement("Topic");
                    Writer.WriteAttributeString("AllowDiscussions", topic.AllowDiscussions.ToString());
                    Writer.WriteAttributeString("AllowRatings", topic.AllowRatings.ToString());
                    Writer.WriteAttributeString("Content", topic.Content);
                    Writer.WriteAttributeString("Description", topic.Description);
                    Writer.WriteAttributeString("Keywords", topic.Keywords);
                    Writer.WriteAttributeString("Name", topic.Name);
                    Writer.WriteAttributeString("Title", topic.Title);
                    Writer.WriteAttributeString("UpdateDate", topic.UpdateDate.ToString("g"));
                    Writer.WriteAttributeString("UpdatedBy", topic.UpdatedBy);
                    Writer.WriteAttributeString("UpdatedByUserID", topic.UpdatedByUserID.ToString("g"));
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();

                Writer.WriteEndElement();
                Writer.Close();

                return strXML.ToString();
            }
        }

        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                XmlNode node = null;
                XmlNode nodes = Globals.GetContent(Content, "Wiki");
                ModuleController objModules = new ModuleController();
                foreach (XmlNode node_loopVariable in nodes.SelectSingleNode("Settings"))
                {
                    node = node_loopVariable;
                    objModules.UpdateModuleSetting(ModuleID, node.Attributes["Name"].Value, node.Attributes["Value"].Value);
                }
                TopicBO topicBo = new TopicBO(uof);

                //clean up
                var topics = topicBo.GetAllByModuleID(ModuleID);
                foreach (var topic in topics)
                {
                    //TODO - On the old version topics where deleted via the SPROC [Wiki_TopicDelete], it should be dropped in this new version
                    topicBo.Delete(new Topic { TopicID = topic.TopicID });
                }

                try
                {
                    foreach (XmlNode node_loopVariable in nodes.SelectNodes("Topics/Topic"))
                    {
                        node = node_loopVariable;
                        var topic = new Topic();
                        topic.PortalSettings = PortalController.GetCurrentPortalSettings();
                        topic.AllowDiscussions = bool.Parse(node.Attributes["AllowDiscussions"].Value);
                        topic.AllowRatings = bool.Parse(node.Attributes["AllowRatings"].Value);
                        topic.Content = node.Attributes["Content"].Value;
                        topic.Description = node.Attributes["Description"].Value;
                        topic.Keywords = node.Attributes["Keywords"].Value;
                        topic.ModuleId = ModuleID;
                        //Here we need to define the TabID otherwise the import won't work until the content is saved again.
                        ModuleController mc = new ModuleController();
                        ModuleInfo mi = mc.GetModule(ModuleID, -1);
                        topic.TabID = mi.TabID;

                        topic.Name = node.Attributes["Name"].Value;
                        topic.RatingOneCount = 0;
                        topic.RatingTwoCount = 0;
                        topic.RatingThreeCount = 0;
                        topic.RatingFourCount = 0;
                        topic.RatingFiveCount = 0;
                        topic.RatingSixCount = 0;
                        topic.RatingSevenCount = 0;
                        topic.RatingEightCount = 0;
                        topic.RatingNineCount = 0;
                        topic.RatingTenCount = 0;
                        topic.Title = node.Attributes["Title"].Value;
                        topic.UpdateDate = DateTime.Parse(node.Attributes["UpdateDate"].Value);
                        topic.UpdatedBy = node.Attributes["UpdatedBy"].Value;
                        topic.UpdatedByUserID = int.Parse(node.Attributes["UpdatedByUserID"].Value);
                        topicBo.Add(topic);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        //Public Function UpgradeModule(ByVal Version As String) As String Implements IUpgradeable.UpgradeModule
        //    InitPermissions()
        //    Return Version
        //End Function

        //Private Sub InitPermissions()
        //    Dim EditContent As Boolean

        // Dim moduleDefId As Integer Dim pc As New PermissionController Dim permissions As
        // ArrayList = pc.GetPermissionByCodeAndKey("WIKI", Nothing) Dim dc As New
        // DesktopModuleController Dim desktopInfo As DesktopModuleInfo desktopInfo =
        // dc.GetDesktopModuleByModuleName("Wiki") Dim mc As New ModuleDefinitionController Dim
        // mInfo As ModuleDefinitionInfo mInfo =
        // mc.GetModuleDefinitionByName(desktopInfo.DesktopModuleID, "Wiki") moduleDefId =
        // mInfo.ModuleDefID For Each p As PermissionInfo In permissions If p.PermissionKey =
        // "EDIT_CONTENT" And p.ModuleDefID = moduleDefId Then _ EditContent = True Next If Not
        // EditContent Then Dim p As New PermissionInfo p.ModuleDefID = moduleDefId p.PermissionCode
        // = "WIKI" p.PermissionKey = "EDIT_CONTENT" p.PermissionName = "Edit Content"
        // pc.AddPermission(p) End If
        //End Sub
    }
}