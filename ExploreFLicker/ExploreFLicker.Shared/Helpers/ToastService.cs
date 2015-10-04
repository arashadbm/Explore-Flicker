using System;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ExploreFlicker.Helpers;

namespace ExploreFlicker.Helpers
{
    public class ToastService : IToastService
    {

        public void ShowToastThreeLines ( string message )
        {

            var toastXmlString = string.Format("<toast><visual version='1'><binding template='ToastText01'><text id='1'>{0}</text></binding></visual></toast>", message);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastXmlString);
            var toast = new ToastNotification(xmlDoc);
#if WINDOWS_PHONE_APP
            //To hide it from action center
            toast.ExpirationTime = DateTimeOffset.UtcNow.AddMilliseconds(500);
#endif
            ToastNotificationManager.CreateToastNotifier().Show(toast);


        }


        public void ShowToastTwoLinesBody ( string title, string body )
        {
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText03);
            var toeastElement = notificationXml.GetElementsByTagName("text");
            toeastElement[0].AppendChild(notificationXml.CreateTextNode(title));
            if(!body.Equals(""))
                toeastElement[1].AppendChild(notificationXml.CreateTextNode(body));
            var toastNotification = new ToastNotification(notificationXml);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}
