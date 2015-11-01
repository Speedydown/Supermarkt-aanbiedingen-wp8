using Supermarkt_aanbiedingen.Common;
using Supermarkt_aanbiedingenLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
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
    public sealed partial class ProductPage : Page
    {
        private static readonly string[] ItemCount = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Supermarkt supermarkt;

        public ProductPage()
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

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Exception AppException = null;

            try
            {
                CountCombovox.ItemsSource = ItemCount;
                CountCombovox.SelectedItem = (CountCombovox.ItemsSource as string[])[0];

                supermarkt = Supermarkt.Deserialize(e.NavigationParameter as string);
                this.DataContext = supermarkt;

                //GetBoodschappenlijstje
                IList<BoodschappenLijstje> lijstjes = await BoodschappenLijstje.GetBoodschappenLijstjes();

                foreach (BoodschappenLijstje b in lijstjes)
                {
                    if (b.supermarkt.Name == supermarkt.Name)
                    {
                        foreach (BoodschappenlijstjeItem BItem in b.Producten)
                        {
                            if (BItem.SupermarktItem.Name == supermarkt.ProductPagina.SelectedItem.Name)
                            {
                                DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                                AddButtonText.Text = "Wijzig";
                                BoodschappenlijstTextblock.Text = "Verander aantal in boodschappenlijst:";
                                CountCombovox.SelectedIndex = BItem.Count - 1;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                AppException = ex;
            }

            if (AppException != null)
            {
                await GetSAData.SendException(AppException.Message);
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddButtonText.Text != "Wijzig")
            {
                AddButtonText.Text = "Wijzig";
                DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                if (CountCombovox.SelectedIndex == 0)
                {
                    StatusTextblock.Text = "Verwijderd!";
                    AddButtonText.Text = "Toevoegen";
                    BoodschappenlijstTextblock.Text = "Voeg toe aan boodschappenlijst:";
                }
                else
                {
                    StatusTextblock.Text = "Aantal gewijzigd!";
                }
            }

            
            BoodschappenlijstTextblock.Text = "Verander aantal in boodschappenlijst:";
            StatusTextblock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await BoodschappenLijstje.AddProductToBoodschappenLijstje(supermarkt, CountCombovox.SelectedIndex + 1);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BoodschappenlijstTextblock.Text = "Voeg toe aan boodschappenlijst:";
            AddButtonText.Text = "Toevoegen";
            StatusTextblock.Text = "Verwijderd!";
            await BoodschappenLijstje.AddProductToBoodschappenLijstje(supermarkt, 0);
            StatusTextblock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

    }
}
