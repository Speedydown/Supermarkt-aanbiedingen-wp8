using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Supermarkt_aanbiedingenLogic
{
    public static class NotifcationDataHandler
    {
        private const string FileName = "SupermarktDict.json";
        private static Dictionary<string, string> ProductPageExpirationDateDict = null;
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static IAsyncAction Update(string SupermarktName, string SupermarktExpirationDate, bool BackgroundTask)
        {
            return UpdateHelper(SupermarktName, SupermarktExpirationDate, BackgroundTask).AsAsyncAction();
        }

        private static async Task UpdateHelper(string SupermarktName, string SupermarktExpirationDate, bool BackgroundTask)
        {
            await Load();

            try
            {
                ProductPageExpirationDateDict.Add(SupermarktName, SupermarktExpirationDate);
            }
            catch
            {
                try
                {
                    string Value = string.Empty;
                    ProductPageExpirationDateDict.TryGetValue(SupermarktName, out Value);

                    ProductPageExpirationDateDict.Remove(SupermarktName);
                    ProductPageExpirationDateDict.Add(SupermarktName, SupermarktExpirationDate);

                    if (BackgroundTask && Value != string.Empty && Value != SupermarktExpirationDate)
                    {
                        CreateNotification(SupermarktName);
                    }
                }
                catch
                {

                }
            }

            await Save();
        }

        public static IAsyncAction Delete(string SupermarktName)
        {
            return DeleteHelper(SupermarktName).AsAsyncAction();
        }

        private static async Task DeleteHelper(string SupermarktName)
        {
            try
            {
                await Load();
                ProductPageExpirationDateDict.Remove(SupermarktName);
                await Save();
            }
            catch
            {

            }
        }

        private static void CreateNotification(string SupermarktName)
        {

            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(SupermarktName));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode("Er zijn nieuwe aanbiedingen!"));

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.Default");

            toastNode.AppendChild(audio);

            ((XmlElement)toastNode).SetAttribute("launch", SupermarktName);
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private static async Task Load()
        {
            if (ProductPageExpirationDateDict == null)
            {
                try
                {
                    StorageFile sFile = await localFolder.GetFileAsync(FileName);
                    string FileAsText = await FileIO.ReadTextAsync(sFile);
                    ProductPageExpirationDateDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(FileAsText);
                }
                catch (Exception)
                {
                    ProductPageExpirationDateDict = new Dictionary<string, string>();
                }

                if (ProductPageExpirationDateDict == null)
                {
                    ProductPageExpirationDateDict = new Dictionary<string, string>();
                }
            }
        }

        private static async Task Save()
        {
            StorageFile file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

            if (file != null)
            {
                string JsonString = JsonConvert.SerializeObject(ProductPageExpirationDateDict);

                await FileIO.WriteTextAsync(file, JsonString);
            }
        }
    }
}
