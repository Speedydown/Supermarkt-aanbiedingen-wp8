using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class BoodschappenLijstje : INotifyPropertyChanged
    {
        private static IList<BoodschappenLijstje> _BoodschappenLijstjes = null;
        private const string FileName = "BoodschappenLijstjes.json";
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public static IAsyncOperation<IList<BoodschappenLijstje>> GetBoodschappenLijstjes()
        {
            return GetBoodschappenLijstjesHelper().AsAsyncOperation();
        }

        private static async Task<IList<BoodschappenLijstje>> GetBoodschappenLijstjesHelper()
        {
            if (_BoodschappenLijstjes == null)
            {
                try
                {
                    StorageFile sFile = await localFolder.GetFileAsync(FileName);
                    _BoodschappenLijstjes = JsonConvert.DeserializeObject<IList<BoodschappenLijstje>>(await FileIO.ReadTextAsync(sFile));
                }
                catch (Exception)
                {
                    _BoodschappenLijstjes = new List<BoodschappenLijstje>();
                }
            }

            return _BoodschappenLijstjes;
        }

        public static IAsyncOperation<BoodschappenLijstje> GetBoodschappenLijstjeByName(string Name)
        {
            return GetBoodschappenLijstjeByNameHelper(Name).AsAsyncOperation();
        }

        private static async Task<BoodschappenLijstje> GetBoodschappenLijstjeByNameHelper(string Name)
        {
            if (_BoodschappenLijstjes == null)
            {
                try
                {
                    StorageFile sFile = await localFolder.GetFileAsync(FileName);
                    _BoodschappenLijstjes = JsonConvert.DeserializeObject<IList<BoodschappenLijstje>>(await FileIO.ReadTextAsync(sFile));
                }
                catch (Exception)
                {
                    _BoodschappenLijstjes = new List<BoodschappenLijstje>();
                }
            }

            foreach (BoodschappenLijstje b in _BoodschappenLijstjes)
            {
                if (b.supermarkt.Name == Name)
                {
                    return b;
                }
            }

            return null;
        }

        public static IAsyncAction AddProductToBoodschappenLijstje(Supermarkt supermarkt, int Count)
        {
            return AddProductToBoodschappenLijstjeHelper(supermarkt, Count).AsAsyncAction();
        }

        private static async Task AddProductToBoodschappenLijstjeHelper(Supermarkt supermarkt, int Count)
        {
            if (supermarkt.ProductPagina.SelectedItem == null)
            {
                return;
            }

            IList<BoodschappenLijstje> Boodschappenlijstjes = await GetBoodschappenLijstjes();
            BoodschappenLijstje BoodschappenLijstje = null;

            foreach (BoodschappenLijstje b in Boodschappenlijstjes)
            {
                if (b.SupermarktNaam == supermarkt.Name)
                {
                    BoodschappenLijstje = b;
                    break;
                }
            }

            if (BoodschappenLijstje == null)
            {
                BoodschappenLijstje = new BoodschappenLijstje(supermarkt.Name);
                Boodschappenlijstjes.Add(BoodschappenLijstje);
            }

            foreach (BoodschappenlijstjeItem bi in BoodschappenLijstje.Producten)
            {
                if (bi.SupermarktItem.Name == supermarkt.ProductPagina.SelectedItem.Name)
                {
                    BoodschappenLijstje.Producten.Remove(bi);
                    break;
                }
            }

            if (Count > 0)
            {
                BoodschappenLijstje.Producten.Add(new BoodschappenlijstjeItem(Count, supermarkt.ProductPagina.SelectedItem));
            }
            else
            {
                if (BoodschappenLijstje.Producten.Count == 0)
                {
                    Boodschappenlijstjes.Remove(BoodschappenLijstje);
                }
            }

            try
            {
                StorageFile file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    string JsonString = JsonConvert.SerializeObject(await GetBoodschappenLijstjes());

                    await FileIO.WriteTextAsync(file, JsonString);
                }
            }
            catch (Exception)
            {
                //Could not save? OHOH
            }

            return;
        }

        public static IAsyncAction AddProductToBoodschappenLijstje(Supermarkt supermarkt, Product product, int Count)
        {
            return AddProductToBoodschappenLijstjeHelper(supermarkt, product, Count).AsAsyncAction();
        }

        private static async Task AddProductToBoodschappenLijstjeHelper(Supermarkt supermarkt, Product product, int Count)
        {
            if (supermarkt == null || product == null)
            {
                return;
            }

            IList<BoodschappenLijstje> Boodschappenlijstjes = await GetBoodschappenLijstjes();
            BoodschappenLijstje BoodschappenLijstje = null;

            foreach (BoodschappenLijstje b in Boodschappenlijstjes)
            {
                if (b.SupermarktNaam == supermarkt.Name)
                {
                    BoodschappenLijstje = b;
                    break;
                }
            }

            if (BoodschappenLijstje == null)
            {
                BoodschappenLijstje = new BoodschappenLijstje(supermarkt.Name);
                Boodschappenlijstjes.Add(BoodschappenLijstje);
            }

            foreach (BoodschappenlijstjeItem bi in BoodschappenLijstje.Producten)
            {
                if (bi.SupermarktItem.Name == product.Name)
                {
                    BoodschappenLijstje.Producten.Remove(bi);
                    break;
                }
            }

            if (Count > 0)
            {
                BoodschappenLijstje.Producten.Add(new BoodschappenlijstjeItem(Count, product));
            }
            else
            {
                if (BoodschappenLijstje.Producten.Count == 0)
                {
                    Boodschappenlijstjes.Remove(BoodschappenLijstje);
                }
            }

            try
            {
                StorageFile file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    string JsonString = JsonConvert.SerializeObject(await GetBoodschappenLijstjes());

                    await FileIO.WriteTextAsync(file, JsonString);
                }
            }
            catch (Exception)
            {
                //Could not save? OHOH
            }

            BoodschappenLijstje.OnPropertyChanged("Producten");
            return;
        }

        public static IAsyncAction DeleteProductFromBoodschappenLijstje(Supermarkt supermarkt, Product product)
        {
            return DeleteProductFromBoodschappenLijstjeHelper(supermarkt, product).AsAsyncAction();
        }

        private static async Task DeleteProductFromBoodschappenLijstjeHelper(Supermarkt supermarkt, Product product)
        {
            IList<BoodschappenLijstje> Boodschappenlijstjes = await GetBoodschappenLijstjes();
            BoodschappenLijstje BoodschappenLijstje = null;

            foreach (BoodschappenLijstje b in Boodschappenlijstjes)
            {
                if (b.SupermarktNaam == supermarkt.Name)
                {
                    BoodschappenLijstje = b;
                    break;
                }
            }

            foreach (BoodschappenlijstjeItem bi in BoodschappenLijstje.Producten)
            {
                if (bi.SupermarktItem.Name == product.Name)
                {
                    BoodschappenLijstje.Producten.Remove(bi);
                    break;
                }
            }

            if (BoodschappenLijstje.Producten.Count == 0)
            {
                Boodschappenlijstjes.Remove(BoodschappenLijstje);
            }

            BoodschappenLijstje.Notify();

            try
            {
                StorageFile file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    string JsonString = JsonConvert.SerializeObject(await GetBoodschappenLijstjes());

                    await FileIO.WriteTextAsync(file, JsonString);
                }
            }
            catch (Exception)
            {
                //Could not save? OHOH
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string SupermarktNaam { get; private set; }

        public IList<BoodschappenlijstjeItem> Producten { get; private set; }
        public Supermarkt supermarkt { get; set; }
        public Product SelectedItem { get; set; }

        public string LijstText
        {
            get
            {
                if (this.Producten.Count == 1)
                {
                    return this.Producten.Count + " product";
                }
                else
                {
                    return this.Producten.Count + " producten";
                }
            }
        }

        public BoodschappenLijstje(string SupermarktNaam)
        {
            this.SupermarktNaam = SupermarktNaam;
            this.Producten = new ObservableCollection<BoodschappenlijstjeItem>();
        }

        public void Notify()
        {
            OnPropertyChanged("Producten");
            OnPropertyChanged("LijstText");
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static BoodschappenLijstje Deserialize(string Input)
        {
            return JsonConvert.DeserializeObject<BoodschappenLijstje>(Input);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
