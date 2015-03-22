using Supermarkt_aanbiedingen.Common;
using Supermarkt_aanbiedingenLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Supermarkt_aanbiedingen
{
    public sealed partial class ShoppingList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private BoodschappenLijstje boodschappenlijstje;
        private static Product SelectedItem;

        public ShoppingList()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            boodschappenlijstje = BoodschappenLijstje.Deserialize(e.NavigationParameter as string);

            this.DataContext = boodschappenlijstje;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void ProductsLV_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.boodschappenlijstje.SelectedItem = (e.ClickedItem as BoodschappenlijstjeItem).SupermarktItem;
            SelectedItem = this.boodschappenlijstje.SelectedItem;

            var messageDialog = new Windows.UI.Popups.MessageDialog("Weet u zeker dat u het product '" + boodschappenlijstje.SelectedItem.Name + "' wilt verwijderen?", "Verwijderen?");
            messageDialog.Commands.Add(
            new Windows.UI.Popups.UICommand("Verwijderen", CommandInvokedHandler));
            messageDialog.Commands.Add(
            new Windows.UI.Popups.UICommand("Annuleren", CommandInvokedHandler));
            await messageDialog.ShowAsync();
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Verwijderen")
            {
                await BoodschappenLijstje.DeleteProductFromBoodschappenLijstje(boodschappenlijstje.supermarkt, SelectedItem);

                IList<BoodschappenLijstje> Lijstjes = await BoodschappenLijstje.GetBoodschappenLijstjes();

                foreach (BoodschappenLijstje bl in Lijstjes)
                {
                    if (bl.SupermarktNaam == boodschappenlijstje.SupermarktNaam)
                    {
                        this.boodschappenlijstje = bl;
                        this.DataContext = bl;
                        return;
                    }
                }

                if (!Frame.Navigate(typeof(MainPage)))
                {

                }
            }
        }

        private async void ProductsLV_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                foreach (BoodschappenlijstjeItem BLI in boodschappenlijstje.Producten)
                {
                    if (BLI.SupermarktItem.URL == SelectedItem.URL)
                    {
                        boodschappenlijstje.SelectedItem = BLI.SupermarktItem;
                        SelectedItem = null;
                        (sender as ListView).SelectedItem = BLI.SupermarktItem;
                        break;
                    }
                }

                (sender as ListView).ScrollIntoView(boodschappenlijstje.SelectedItem);
            }


        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
