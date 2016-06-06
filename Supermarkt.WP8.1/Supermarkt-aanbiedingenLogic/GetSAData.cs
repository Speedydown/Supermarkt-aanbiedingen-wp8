using BaseLogic.HtmlUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Supermarkt_aanbiedingenLogic
{
    public static class GetSAData
    {
        private static Dictionary<int, string> DiscountCache = new Dictionary<int, string>();

        private const string Host = "http://win10apps.nl/api/supermarkten/";

        public static async Task<IList<Supermarkt>> GetAllSupermarkets()
        {
            string SupermarktData = await HTTPGetUtil.GetDataAsStringFromURL(Host + "getsupermarkten");

            return JsonConvert.DeserializeObject<IList<Supermarkt>>(SupermarktData);
        }

        public static async Task<IList<Supermarkt>> GetSelectedSuperMarkets()
        {
            return await Supermarkt.GetSelectedSupermarketsFromStorage();
        }

        public static async Task<ProductPagina> GetDiscountsFromSupermarket(Supermarkt supermarkt, bool BackgroundTask)
        {
            string Cache = null;

            DiscountCache.TryGetValue(supermarkt.ID, out Cache);

            if (!string.IsNullOrWhiteSpace(Cache))
            {
                return JsonConvert.DeserializeObject<ProductPagina>(Cache);
            }

            string SupermarktData = await HTTPGetUtil.GetDataAsStringFromURL(Host + "GetProductPageBySupermarketID/" + supermarkt.ID);

            if (!string.IsNullOrWhiteSpace(SupermarktData))
            {
                DiscountCache.Add(supermarkt.ID, SupermarktData);
            }

            ProductPagina p = JsonConvert.DeserializeObject<ProductPagina>(SupermarktData);
            await NotifcationDataHandler.Update(supermarkt.Name, p.DiscountValid, BackgroundTask);
            return p;
        }
    }
}
