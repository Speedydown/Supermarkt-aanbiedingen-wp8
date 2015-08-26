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
using Windows.UI.Core;
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
    public sealed partial class SupermarketDiscounts : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Supermarkt supermarkt;
        private static Product SelectedItem;

        public SupermarketDiscounts()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            Task t = ArticleCounter.AddArticleCount();
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
            LoadingControl.DisplayLoadingError(false);
            LoadingControl.SetLoadingStatus(true);

            try
            {
                supermarkt = Supermarkt.Deserialize(e.NavigationParameter as string);
                this.DataContext = supermarkt;
                supermarkt.ProductPagina = await GetSAData.GetDiscountsFromSupermarket(supermarkt, false);
                this.ProductsLV.DataContext = supermarkt.ProductPagina.Producten;

                try
                {
                    if (SelectedItem != null)
                    {
                        foreach (Product p in supermarkt.ProductPagina.Producten)
                        {
                            if (p.ID == SelectedItem.ID)
                            {
                                supermarkt.ProductPagina.SelectedItem = p;
                                SelectedItem = null;
                                ProductsLV.SelectedItem = p;
                                break;
                            }
                        }

                        ProductsLV.ScrollIntoView(supermarkt.ProductPagina.SelectedItem);
                    }
                }
                catch
                {

                }

            }
            catch
            {
                LoadingControl.DisplayLoadingError(true);
            }
            finally
            {
                LoadingControl.SetLoadingStatus(false);
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

        private void ProductsLV_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.supermarkt.ProductPagina.SelectedItem = e.ClickedItem as Product;
            SelectedItem = e.ClickedItem as Product;

            if (!Frame.Navigate(typeof(ProductPage), this.supermarkt.Serialize()))
            {

            }
        }

        private async void ProductsLV_Loaded(object sender, RoutedEventArgs e)
        {



        }
    }
}
