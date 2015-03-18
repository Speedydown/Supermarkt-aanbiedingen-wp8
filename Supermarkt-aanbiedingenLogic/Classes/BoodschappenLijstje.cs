using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class BoodschappenLijstje
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

        public static IAsyncAction AddProductToBoodschappenLijstje(Supermarkt supermarkt)
        {
            return AddProductToBoodschappenLijstjeHelper(supermarkt).AsAsyncAction();
        }

        private static async Task AddProductToBoodschappenLijstjeHelper(Supermarkt supermarkt)
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

            BoodschappenLijstje.Producten.Add(supermarkt.ProductPagina.SelectedItem);

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

        public string SupermarktNaam { get; private set; }
        public IList<Product> Producten { get; private set; }

        public BoodschappenLijstje(string SupermarktNaam)
        {
            this.SupermarktNaam = SupermarktNaam;
            this.Producten = new List<Product>();

            //alleen naam opslaan
        }
    }
}
