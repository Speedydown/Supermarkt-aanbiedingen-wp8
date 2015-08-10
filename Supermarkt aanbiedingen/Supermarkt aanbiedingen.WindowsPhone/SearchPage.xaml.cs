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
    public sealed partial class SearchPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        IList<SupermarketSearchResult> searchresult = new List<SupermarketSearchResult>();

        public SearchPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
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
            LoadingControl.SetLoadingStatus(false);
            SearchTextbox.Focus(Windows.UI.Xaml.FocusState.Pointer);
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Product p = (Product)e.ClickedItem;

            foreach (SupermarketSearchResult ssr in this.searchresult)
            {
                foreach (Product product in ssr.producten)
                {
                    if (p == product)
                    {
                        ssr.supermarkt.ProductPagina.SelectedItem = product;

                        if (!Frame.Navigate(typeof(ProductPage), ssr.supermarkt.Serialize()))
                        {

                        }
                    }
                }
            }


        }

        private void SearchTextbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.Search();
            }
        }

        private async void Search()
        {
            LoadingControl.DisplayLoadingError(false);
            LoadingControl.SetLoadingStatus(true);

            try
            {
                SearchTextbox.IsEnabled = false;
                SearchButton.IsEnabled = false;

                IList<Supermarkt> supermarkten = await GetSAData.GetSelectedSuperMarkets();

                foreach (Supermarkt s in supermarkten)
                {
                    if (s.ProductPagina == null)
                    {
                        s.ProductPagina = await GetSAData.GetDiscountsFromSupermarket(s);
                    }
                }

                searchresult = SearchHandler.SearchForProductenInDiscounts(supermarkten, SearchTextbox.Text);


                if (searchresult.Count > 0)
                {
                    NoResultsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    NoResultsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }

                this.DataContext = searchresult;
            }
            catch
            {
                NoResultsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                LoadingControl.SetLoadingStatus(false);
            }



            SearchTextbox.IsEnabled = true;
            SearchButton.IsEnabled = true;
            SearchButton.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Search();
        }
    }
}
