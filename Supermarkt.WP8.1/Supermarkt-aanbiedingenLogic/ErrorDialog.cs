using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Supermarkt_aanbiedingenLogic
{
    static class ErrorDialog
    {
        public static bool displayedError = false;

        /// <summary>
        /// Displays an generic error with a close button
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Body"></param>
        public static async Task ShowError(string Title, string Body)
        {
            if (!displayedError)
            {
                MessageDialog msg = new MessageDialog(Body, Title);
                UICommand closeBtn = new UICommand("Sluiten");
                closeBtn.Invoked = CloseButton;
                msg.Commands.Add(closeBtn);
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

                displayedError = true;
            }
        }

        /// <summary>
        /// Method for ErrorMessages
        /// </summary>
        /// <param name="command"></param>
        private static void CloseButton(IUICommand command)
        {

        }

    }
}
