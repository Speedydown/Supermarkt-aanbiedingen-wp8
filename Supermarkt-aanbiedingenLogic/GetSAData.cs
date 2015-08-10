using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawlerTools;
using Windows.Foundation;

namespace Supermarkt_aanbiedingenLogic
{
    public static class GetSAData
    {
        private const string Host = "http://speedydown-001-site2.smarterasp.net";
       // private const string Host = "http://localhost:43112";

        public static IAsyncOperation<IList<Supermarkt>> GetAllSupermarkets()
        {
            return GetAllSupermarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetAllSupermarketsHelper()
        {
            return JsonConvert.DeserializeObject<IList<Supermarkt>>(await HTTPGetUtil.GetDataAsStringFromURL(Host + "/api.ashx?Query=V2GetSupermarkten"));
        }

        public static IAsyncOperation<IList<Supermarkt>> GetSelectedSuperMarkets()
        {
            return GetSelectedSuperMarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetSelectedSuperMarketsHelper()
        {
            return await Supermarkt.GetSelectedSupermarketsFromStorage();
        }

        public static IAsyncOperation<ProductPagina> GetDiscountsFromSupermarket(Supermarkt supermarkt)
        {
            return GetDiscountsFromSupermarketHelper(supermarkt).AsAsyncOperation();
        }

        private static async Task<ProductPagina> GetDiscountsFromSupermarketHelper(Supermarkt supermarkt)
        {
            return JsonConvert.DeserializeObject<ProductPagina>(await HTTPGetUtil.GetDataAsStringFromURL(Host + "/api.ashx?Query=V2GetProductPageBySupermarketID?ID=" + supermarkt.ID));
        }

        public static IAsyncOperation<IList<Supermarkt>> GetDiscountsFromSupermarkets(IList<Supermarkt> supermarkts)
        {
            return GetDiscountsFromSupermarketsHelper(supermarkts).AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetDiscountsFromSupermarketsHelper(IList<Supermarkt> supermarkts)
        {
            int Currentpos = 0;
            List<Supermarkt> CompletedSupermarkets = new List<Supermarkt>();

            while (supermarkts.Count != CompletedSupermarkets.Count)
            {
                int NumberofSupermarketsInQuery = 9;

                if (supermarkts.Count - CompletedSupermarkets.Count <= 9)
                {
                    NumberofSupermarketsInQuery = supermarkts.Count - CompletedSupermarkets.Count;
                }

                string Query = Host + "/api.ashx?Query=GetDiscountsFromSupermarkets&Supermarkets=" + JsonConvert.SerializeObject((supermarkts as List<Supermarkt>).GetRange(Currentpos, NumberofSupermarketsInQuery));
                string input = await HTTPGetUtil.GetDataAsStringFromURL(Query);
                CompletedSupermarkets.AddRange(JsonConvert.DeserializeObject<List<Supermarkt>>(input));
                Currentpos += NumberofSupermarketsInQuery;
            }

            return CompletedSupermarkets;
        }
    }
}
