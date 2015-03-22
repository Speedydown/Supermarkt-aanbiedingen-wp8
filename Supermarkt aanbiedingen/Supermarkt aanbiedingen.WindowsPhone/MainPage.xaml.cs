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
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private static int PivotItemClicked = -1;

        public MainPage()
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
            if (PivotItemClicked != -1)
            {
                pivot.SelectedIndex = PivotItemClicked;
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

        private async void PopularSupermarketsLV_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Supermarkt> supermarkten = (List<Supermarkt>)await GetSAData.GetSelectedSuperMarkets();
                (sender as ListView).ItemsSource = supermarkten;

                ((sender as ListView).Parent as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                
                foreach (UIElement u in (((sender as ListView).Parent as Grid).Parent as Grid).Children)
                {
                    if ((u as Grid).Name == "LoadingGrid")
                    {
                        (u as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
            }
            catch(Exception)
            {
                //iets doen
            }

            

        }

        private void PopularSupermarketsLV_ItemClick(object sender, ItemClickEventArgs e)
        {
            PivotItemClicked = 0;

            if (!Frame.Navigate(typeof(SupermarketDiscounts), (e.ClickedItem as Supermarkt).Serialize()))
            {

            }
        }

        private async void BLListview_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).ItemsSource = await BoodschappenLijstje.GetBoodschappenLijstjes();

            if (((sender as ListView).ItemsSource as List<BoodschappenLijstje>).Count != 0)
            {
                (sender as ListView).Visibility = Windows.UI.Xaml.Visibility.Visible;

                foreach (UIElement u in (((sender as ListView).Parent as Grid).Parent as Grid).Children)
                {
                    if ((u as Grid).Name == "NoItemsGrid")
                    {
                        (u as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PivotItemClicked = pivot.SelectedIndex;

            if (!Frame.Navigate(typeof(SearchPage)))
            {

            }
        }

        private void PrivacyPolicyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BLListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            PivotItemClicked = 1;

            BoodschappenLijstje bl = e.ClickedItem as BoodschappenLijstje;
            Supermarkt supermarkt = null;

            foreach (Supermarkt s in await GetSAData.GetAllSupermarkets())
            {
                if (s.Name == bl.SupermarktNaam)
                {
                    supermarkt = s;
                    break;
                }
            }

            if (supermarkt == null)
            {
                return;
            }

            bl.supermarkt = supermarkt;

            if (!Frame.Navigate(typeof(ShoppingList), bl.Serialize()))
            {

            }
        }
    }
}
