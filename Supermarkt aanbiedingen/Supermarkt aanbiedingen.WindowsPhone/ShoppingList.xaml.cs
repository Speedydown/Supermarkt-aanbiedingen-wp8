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
        RelayCommand _checkedGoBackCommand;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private BoodschappenLijstje boodschappenlijstje;
        private static Product SelectedItem;
        private bool IsAddProductAvtive = false;

        public ShoppingList()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            _checkedGoBackCommand = new RelayCommand(
                                   () => this.CheckGoBack(),
                                   () => this.CanCheckGoBack()
                               );

            navigationHelper.GoBackCommand = _checkedGoBackCommand;
        }

        private bool CanCheckGoBack()
        {
            return true;
        }

        private void CheckGoBack()
        {
            if (this.IsAddProductAvtive)
            {
                this.ShowAndHideAddProduct();   
            }
            else
            {
                NavigationHelper.GoBack();
            }
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
            CountCombovox.ItemsSource = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            CountCombovox.SelectedItem = (CountCombovox.ItemsSource as string[])[0];
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
                    if (bl.supermarkt.Name == boodschappenlijstje.supermarkt.Name)
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

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductTextbox.Text.Length > 2)
            {
                try
                {
                    await BoodschappenLijstje.AddProductToBoodschappenLijstje(this.boodschappenlijstje.supermarkt, new Product(0, "Betreft eigen product", "Prijs (nog) niet bekend", "", ProductTextbox.Text, "Betreft een door de gebruiker toegevoegd product", "", this.boodschappenlijstje.supermarkt.ImageURL), CountCombovox.SelectedIndex + 1);
                    this.ShowAndHideAddProduct();

                    this.boodschappenlijstje = await BoodschappenLijstje.GetBoodschappenLijstjeByName(boodschappenlijstje.supermarkt.Name);
                    this.DataContext = this.boodschappenlijstje;
                }
                catch
                {

                }
            }
            else
            {
                ProductTextbox.Text = "";
            }
        }

        private void ShowAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowAndHideAddProduct();   
        }

        private void ShowAndHideAddProduct()
        {
            AddButton.IsEnabled = this.IsAddProductAvtive;
            this.IsAddProductAvtive = !this.IsAddProductAvtive;
            AddProductGrid.Visibility = this.IsAddProductAvtive ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;


            if (IsAddProductAvtive)
            {
                ProductTextbox.Focus(Windows.UI.Xaml.FocusState.Pointer);
            }

            ShowAddButton.Visibility = this.IsAddProductAvtive ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
            HideAddButton.Visibility = this.IsAddProductAvtive ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            AddProductButtonAppbar.Visibility = this.IsAddProductAvtive ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            ProductTextbox.Text = "";
        }

        private void ProductTextbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CountCombovox.Focus(Windows.UI.Xaml.FocusState.Pointer);
            }
        }

        private async void AddProductButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (ProductTextbox.Text.Length > 2)
            {
                await BoodschappenLijstje.AddProductToBoodschappenLijstje(this.boodschappenlijstje.supermarkt, new Product(0, "Betreft eigen product", "Prijs (nog) niet bekend", "", ProductTextbox.Text, "Betreft een door de gebruiker toegevoegd product", "", this.boodschappenlijstje.supermarkt.ImageURL), CountCombovox.SelectedIndex + 1);
                this.ShowAndHideAddProduct();

                this.boodschappenlijstje = await BoodschappenLijstje.GetBoodschappenLijstjeByName(boodschappenlijstje.supermarkt.Name);
                this.DataContext = this.boodschappenlijstje;
            }
            else
            {
                ProductTextbox.Text = "";
            }
        }

        private async void DeleteALlButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (BoodschappenlijstjeItem bi in this.boodschappenlijstje.Producten)
            {
                await BoodschappenLijstje.DeleteProductFromBoodschappenLijstje(boodschappenlijstje.supermarkt, bi.SupermarktItem);
            }

            Frame.GoBack();
        }
    }
}
