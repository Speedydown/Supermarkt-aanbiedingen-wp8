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
    public sealed partial class ConfigureSupermarkets : Page
    {
        private List<Supermarkt> Supermarkets = new List<Supermarkt>();
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ConfigureSupermarkets()
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
            Supermarkets = (List<Supermarkt>)await GetSAData.GetAllSupermarkets();

            IList<Supermarkt> AanwezigeSupermarkten = null;

            try
            {
                AanwezigeSupermarkten = await GetSAData.GetSelectedSuperMarkets();
            }
            catch
            {

            }

            if (AanwezigeSupermarkten != null)
            {
                NextButton.IsEnabled = true;

                foreach (Supermarkt SA in Supermarkets)
                {
                    foreach (Supermarkt SB in AanwezigeSupermarkten)
                    {
                        if (SB.Name == SA.Name)
                        {
                            SA.SupermarketEnabled = true;
                        }
                    }
                }
            }


            SupermarktetsListview.ItemsSource = Supermarkets;

            await Task.Delay(3000);

            LoadingGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ContentGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
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

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            List<Supermarkt> SelectedSupermarkets = new List<Supermarkt>();

            foreach (Supermarkt s in Supermarkets)
            {
                if (s.SupermarketEnabled)
                {
                    SelectedSupermarkets.Add(s);
                }
            }

            await Supermarkt.SaveSupermarketsToStorage(SelectedSupermarkets);

            if (!Frame.Navigate(typeof(MainPage)));
            {

            }
        }

        private void SupermarktetsListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            (e.ClickedItem as Supermarkt).SupermarketEnabled = !(e.ClickedItem as Supermarkt).SupermarketEnabled;

            foreach (Supermarkt s in Supermarkets)
            {
                if (s.SupermarketEnabled)
                {

                    NextButton.IsEnabled = true;
                    return;
                }
            }

            NextButton.IsEnabled = false;
        }
    }
}
