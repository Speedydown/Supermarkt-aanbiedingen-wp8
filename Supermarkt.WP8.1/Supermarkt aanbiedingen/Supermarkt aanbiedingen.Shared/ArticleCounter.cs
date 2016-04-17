using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.UI.Popups;

namespace Supermarkt_aanbiedingen
{
    public static class ArticleCounter
    {
        public static async Task AddArticleCount()
        {
            int Counter = GetCurrentCount() + 1;

            ApplicationData applicationData = ApplicationData.Current;
            ApplicationDataContainer localSettings = applicationData.LocalSettings;

            try
            {
                localSettings.Values["NumberOfArticles"] = Counter;
            }
            catch
            {

            }

            if (Counter == 25)
            {
                await ShowRateDialog();
            }
        }

        private static int GetCurrentCount()
        {
            ApplicationData applicationData = ApplicationData.Current;
            ApplicationDataContainer localSettings = applicationData.LocalSettings;

            try
            {
                return (int)localSettings.Values["NumberOfArticles"];
            }
            catch
            {
                return 0;
            }
        }

        private static async Task ShowRateDialog()
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog("Wij bieden Supermarkt aanbiedingen kostenloos aan en we zouden het op prijs stellen als u onze app een positieve review geeft in de Windows store.", "Bedankt");
            messageDialog.Commands.Add(
            new UICommand("Review", CommandInvokedHandler));
            messageDialog.Commands.Add(
            new UICommand("Annuleren", CommandInvokedHandler));
            await messageDialog.ShowAsync();
        }


        private static async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Review")
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
            }
        }
    }
}
