using BaseLogic.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace BackgroundTaskWP
{
    public static class AskNotificationHandler
    {
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static IAsyncAction AskNotificationQuestion()
        {
            return AskNotificationQuestionHelper().AsAsyncAction();
        }

        private static async Task AskNotificationQuestionHelper()
        {
            bool? AskedBefore = null;

            try
            {
                StorageFile sFile = await localFolder.GetFileAsync("BackgroundQuestion");
                AskedBefore = JsonConvert.DeserializeObject<bool>(await FileIO.ReadTextAsync(sFile));
            }
            catch (Exception)
            {

            }

            if (AskedBefore == null)
            {
                MessageDialog msg = new MessageDialog("Wilt u een notificatie ontvangen wanneer er nieuwe aanbiedingen beschikbaar zijn voor de door u geselecteerde supermarkten?", "Notificaties");

                UICommand YesButton = new UICommand("Ja");
                YesButton.Invoked = YesButtonInvoked;
                msg.Commands.Add(YesButton);

                UICommand NoButton = new UICommand("Nee");
                NoButton.Invoked = NoButtonInvoked;
                msg.Commands.Add(NoButton);

                try
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await msg.ShowAsync();
                    });

                }
                catch (Exception)
                {

                }
            }

            if (AskedBefore == true)
            {
                NotificationHandler.Run("BackgroundTaskWP.BackgroundTask", "SupermarktAanbiedingenTask", 180);
            }
        }

        private async static void YesButtonInvoked(IUICommand command)
        {
            StorageFile file = await localFolder.CreateFileAsync("BackgroundQuestion", CreationCollisionOption.ReplaceExisting);

            if (file != null)
            {
                string JsonString = JsonConvert.SerializeObject(true);

                await FileIO.WriteTextAsync(file, JsonString);
            }

            NotificationHandler.Run("BackgroundTaskWP.BackgroundTask", "SupermarktAanbiedingenTask", 180);
        }

        private async static void NoButtonInvoked(IUICommand command)
        {
            StorageFile file = await localFolder.CreateFileAsync("BackgroundQuestion", CreationCollisionOption.ReplaceExisting);

            if (file != null)
            {
                string JsonString = JsonConvert.SerializeObject(false);

                await FileIO.WriteTextAsync(file, JsonString);
            }
        }
    }
}
