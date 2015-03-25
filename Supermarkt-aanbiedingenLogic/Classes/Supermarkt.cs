using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class Supermarkt : INotifyPropertyChanged
    {
        public string Name { get; private set; }
        public string URL { get; private set; }
        public string Title { get; private set; }
        public string ImageURL { get; private set; }
        [JsonIgnore]
        public string Slogan { get; private set; }
        public ProductPagina ProductPagina { get; private set; }
        private bool _SupermarketEnabled = false;
        [JsonIgnore]
        public bool SupermarketEnabled
        {
            get
            {
                return _SupermarketEnabled;
            }
            set
            {
                _SupermarketEnabled = value;
                OnPropertyChanged("SupermarketEnabled");
                OnPropertyChanged("SelectColor");
            }
        }
        [JsonIgnore]
        public SolidColorBrush SelectColor
        {
            get
            {
                if (SupermarketEnabled)
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
        }

        public Supermarkt(string Name, string URL, string Title, ProductPagina ProductPagina)
        {
            this.Name = Name;

            if (this.Name.Contains("Aanbiedingen "))
            {
                this.Name = this.Name.Substring("Aanbiedingen ".Length);
            }

            this.URL = URL;
            this.Title = Title;
            this.SetImageURL();
            this.SetSlogan();
            this.SupermarketEnabled = false;
            this.ProductPagina = ProductPagina;
        }

        private void SetSlogan()
        {
            switch (this.Name)
            {
                case "Agrimarkt":
                    this.Slogan = "Altijd in uw voordeel!";
                    break;
                case "Albert Heijn":
                    this.Slogan = "Het alledaagse betaalbaar, het bijzondere bereikbaar";
                    break;
                case "Albert Heijn XL":
                    this.Slogan = "Het alledaagse betaalbaar, het bijzondere bereikbaar";
                    break;
                case "Aldi":
                    this.Slogan = "Hoge kwaliteit, lage prijs";
                    break;
                case "Attent":
                    this.Slogan = "Attent, super in de buurt!";
                    break;
                case "Boni":
                    this.Slogan = "Thuis bij boni";
                    break;
                case "C1000":
                    this.Slogan = "Slim bezig, C1000";
                    break;
                case "Coop":
                    this.Slogan = "Op & top Coop";
                    break;
                case "Dagwinkel":
                    this.Slogan = "Elke dag dichtbij";
                    break;
                case "Deen":
                    this.Slogan = "Geen dag zonder Deen";
                    break;
                case "Dekamarkt":
                    this.Slogan = "Vers, vriendelijk & voordelig";
                    break;
                case "Dirk":
                    this.Slogan = "Blijf je verbazen";
                    break;
                case "EMTE":
                    this.Slogan = "De lekkerste supermarkt van Nederland";
                    break;
                case "Hoogvliet":
                    this.Slogan = "Bewezen de goedkoopste!";
                    break;
                case "Jan Linders":
                    this.Slogan = "Het voordeel van het zuiden!";
                    break;
                case "Jumbo":
                    this.Slogan = "Hallo Jumbo";
                    break;
                case "Lidl":
                    this.Slogan = "De hoogste kwaliteit voor de laagste prijs!";
                    break;
                case "MCD":
                    this.Slogan = "Mijn supermarkt!";
                    break;
                case "Nettorama":
                    this.Slogan = "De aller aller goedkoopste";
                    break;
                case "Plus":
                    this.Slogan = "Plus geeft meer";
                    break;
                case "Poiesz":
                    this.Slogan = "Waar het opvalt dat het meevalt";
                    break;
                case "Spar":
                    this.Slogan = "Mijn buurt, mijn Spar";
                    break;
                case "Supercoop":
                    this.Slogan = "Op & top Coop";
                    break;
                case "Troefmarkt":
                    this.Slogan = "Hart van úw buurt";
                    break;
                case "Vomar":
                    this.Slogan = "De enige echte voordeelmarkt!";
                    break;
                default:
                    this.Slogan = string.Empty;
                    break;
            }
        }

        public IAsyncAction GetProductpagina()
        {
            return GetProductpaginaHelper().AsAsyncAction();
        }

        private async Task GetProductpaginaHelper()
        {
            this.ProductPagina = await GetSAData.GetDiscountsFromSupermarket(this);
        }

        private void SetImageURL()
        {
            switch (this.Name)
            {
                case "Agrimarkt":
                    this.ImageURL = "/Assets/Agrimarkt.gif";
                    break;
                case "Albert Heijn":
                    this.ImageURL = "/Assets/AH.png";
                    break;
                case "Lidl":
                    this.ImageURL = "/Assets/Lidl.png";
                    break;
                case "Aldi":
                    this.ImageURL = "/Assets/Aldi.jpg";
                    break;
                case "Attent":
                    this.ImageURL = "/Assets/Attent.png";
                    break;
                case "Boni":
                    this.ImageURL = "/Assets/Boni.png";
                    break;
                case "Jumbo":
                    this.ImageURL = "/Assets/Jumbo.jpg";
                    break;
                case "C1000":
                    this.ImageURL = "/Assets/c1000.png";
                    break;
                case "Coop":
                    this.ImageURL = "/Assets/coop.jpg";
                    break;
                case "Supercoop":
                    this.ImageURL = "/Assets/coop.jpg";
                    break;
                case "Dagwinkel":
                    this.ImageURL = "/Assets/dagwinkel.png";
                    break;
                case "Deen":
                    this.ImageURL = "/Assets/deen.png";
                    break;
                case "Dekamarkt":
                    this.ImageURL = "/Assets/Deka.jpg";
                    break;
                case "EMTE":
                    this.ImageURL = "/Assets/emte.gif";
                    break;
                case "Plus":
                    this.ImageURL = "/Assets/Plus_supermarkt.jpg";
                    break;
                case "Dirk":
                    this.ImageURL = "/Assets/dirkvandenbroek.png";
                    break;
                case "Hoogvliet":
                    this.ImageURL = "/Assets/Hoogvliet.jpg";
                    break;
                case "Jan Linders":
                    this.ImageURL = "/Assets/JanLinders.gif";
                    break;
                case "MCD":
                    this.ImageURL = "/Assets/MCD.gif";
                    break;
                case "Nettorama":
                    this.ImageURL = "/Assets/nettorama.jpg";
                    break;
                case "Poiesz":
                    this.ImageURL = "/Assets/poeisz.png";
                    break;
                case "Spar":
                    this.ImageURL = "/Assets/Spar.jpg";
                    break;
                case "Troefmarkt":
                    this.ImageURL = "/Assets/troefmarkt.png";
                    break;
                case "Vomar":
                    this.ImageURL = "/Assets/Vomar.png";
                    break;
                case "Albert Heijn XL":
                    this.ImageURL = "/Assets/AHXL.png";
                    break;
                default:
                    this.ImageURL = "/Assets/winkelkar.gif";
                    break;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        //---------------------------Static---------------------------
        private const string FileName = "SelectedSupermarkets.json";
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static IAsyncAction SaveSupermarketsToStorage(IList<Supermarkt> Supermarkets)
        {
            return SaveSupermarketsToStorageHelper(Supermarkets).AsAsyncAction();
        }

        private async static Task SaveSupermarketsToStorageHelper(IList<Supermarkt> Supermarkets)
        {
            try
            {
                StorageFile file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    string JsonString = JsonConvert.SerializeObject(Supermarkets);

                    await FileIO.WriteTextAsync(file, JsonString);
                }
            }
            catch (Exception)
            {
                //Could not save? OHOH
            }
        }

        public static IAsyncOperation<IList<Supermarkt>> GetSelectedSupermarketsFromStorage()
        {
            return GetSelectedSupermarketsFromStorageHelper().AsAsyncOperation();
        }

        private async static Task<IList<Supermarkt>> GetSelectedSupermarketsFromStorageHelper()
        {
            try
            {
                StorageFile sFile = await localFolder.GetFileAsync(FileName);
                return JsonConvert.DeserializeObject<List<Supermarkt>>(await FileIO.ReadTextAsync(sFile));
            }
            catch (Exception)
            {
                //file not found Go to select file dialog?
                return null;
            }
        }

        public static Supermarkt Deserialize(string Input)
        {
            return JsonConvert.DeserializeObject<Supermarkt>(Input);
        }
    }


}
