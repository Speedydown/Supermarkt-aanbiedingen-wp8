using Supermarkt_aanbiedingenLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTaskWP
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            if (DateTime.Now.Hour > 7)
            {
                foreach (Supermarkt s in await Supermarkt.GetSelectedSupermarketsFromStorage())
                {
                    await s.GetProductpagina(true);
                }

                deferral.Complete();
            }
        }
    }
}
