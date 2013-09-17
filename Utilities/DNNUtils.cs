using DotNetNuke.Wiki.BusinessObjects.Models;
using System.Collections.Generic;

namespace DotNetNuke.Wiki.Utilities
{
    public class DNNUtils
    {
        public static void SendNotifications(Topic topic, string Name, string Email, string Comment, string Ip)
        {
            if (topic != null)
            {
                List<string> lstEmailsAddresses = GetNotificationEmails(topic);

                if (lstEmailsAddresses.Count > 0)
                {
                    DotNetNuke.Entities.Portals.PortalSettings objPortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings;
                    string strResourceFile = System.Data.Common.Globals.ApplicationPath + "/DesktopModules/Wiki/" + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile;
                    string strSubject = Localization.GetString("NotificationSubject", strResourceFile);
                    string strBody = Localization.GetString("NotificationBody", strResourceFile);

                    strBody = strBody.Replace("[URL]", DotNetNuke.Common.NavigateURL(objPortalSettings.ActiveTab.TabID, objPortalSettings, string.Empty, "topic=" + Entities.WikiData.EncodeTitle(topic.Name)));
                    strBody = strBody.Replace("[NAME]", Name);
                    strBody = strBody.Replace("[EMAIL]", Email);
                    strBody = strBody.Replace("[COMMENT]", Comment);
                    strBody = strBody.Replace("[IP]", string.Empty);

                    System.Text.StringBuilder sbUsersToEmail = new System.Text.StringBuilder();
                    foreach (string sUserToEmail in lstEmailsAddresses)
                    {
                        sbUsersToEmail.Append(sUserToEmail);
                        sbUsersToEmail.Append(";");
                    }

                    //remove the last ;
                    sbUsersToEmail.Remove(sbUsersToEmail.Length - 1, 1);

                    //Services.Mail.Mail.SendMail(objPortalSettings.Email, objPortalSettings.Email, sbUsersToEmail.ToString, strSubject, strBody, "", "", "", "", "", "")

                    Mail.SendMail(objPortalSettings.Email, sbUsersToEmail.ToString(), "", "", MailPriority.Normal, strSubject, MailFormat.Html, Encoding.UTF8, strBody, "",
                    Host.SMTPServer, Host.SMTPAuthentication, Host.SMTPUsername, Host.SMTPPassword, Host.EnableSMTPSSL);
                }
            }
        }
    }
}