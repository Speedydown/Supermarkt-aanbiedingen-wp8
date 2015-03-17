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
        public static IAsyncOperation<IList<Supermarkt>> GetAllSupermarkets()
        {
            return GetAllSupermarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetAllSupermarketsHelper()
        {
            return JsonConvert.DeserializeObject<IList<Supermarkt>>(await HTTPGetUtil.GetDataAsStringFromURL("http://speedydown-001-site2.smarterasp.net/api.ashx?Query=GetAllSupermarkets"));
        }

        public static IAsyncOperation<IList<Supermarkt>> GetSelectedSuperMarkets()
        {
            return GetSelectedSuperMarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetSelectedSuperMarketsHelper()
        {
            IList<Supermarkt> Supermarkets = await Supermarkt.GetSelectedSupermarketsFromStorage();



            //foreach (Supermarkt s in Supermarkets)
            //{
            //    await s.GetProductpagina();
            //}

            return await GetDiscountsFromSupermarketsHelper(Supermarkets);
        }

        public static IAsyncOperation<ProductPagina> GetDiscountsFromSupermarket(Supermarkt supermarkt)
        {
            return GetDiscountsFromSupermarketHelper(supermarkt).AsAsyncOperation();
        }

        private static async Task<ProductPagina> GetDiscountsFromSupermarketHelper(Supermarkt supermarkt)
        {
            return JsonConvert.DeserializeObject<ProductPagina>(await HTTPGetUtil.GetDataAsStringFromURL("http://speedydown-001-site2.smarterasp.net/api.ashx?Query=GetDiscountsFromSupermarket&Supermarket=" + JsonConvert.SerializeObject(supermarkt)));
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

                string input = await HTTPGetUtil.GetDataAsStringFromURL("http://speedydown-001-site2.smarterasp.net/api.ashx?Query=GetDiscountsFromSupermarkets&Supermarkets=" + JsonConvert.SerializeObject((supermarkts as List<Supermarkt>).GetRange(Currentpos, NumberofSupermarketsInQuery)));
                CompletedSupermarkets.AddRange(JsonConvert.DeserializeObject<List<Supermarkt>>(input));
                Currentpos += NumberofSupermarketsInQuery;
            }

            return CompletedSupermarkets;
        }
    }
}
