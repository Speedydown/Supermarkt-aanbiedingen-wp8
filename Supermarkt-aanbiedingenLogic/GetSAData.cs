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
            string PageSource = await HTTPGetUtil.GetDataAsStringFromURL("http://www.supermarktaanbiedingen.com/");
            IList<Supermarkt> Supermarkets = SupermarketsParser.GetSupermarkets(PageSource);

            return Supermarkets;
        }

        public static IAsyncOperation<IList<Supermarkt>> GetSelectedSuperMarkets()
        {
            return GetSelectedSuperMarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetSelectedSuperMarketsHelper()
        {
            IList<Supermarkt> Supermarkets = await Supermarkt.GetSelectedSupermarketsFromStorage();

            foreach (Supermarkt s in Supermarkets)
            {
                await s.GetProductpagina();
            }

            return Supermarkets;
        }

        public static IAsyncOperation<ProductPagina> GetDiscountsFromSupermarket(Supermarkt supermarkt)
        {
            return GetDiscountsFromSupermarketHelper(supermarkt).AsAsyncOperation();
        }

        private static async Task<ProductPagina> GetDiscountsFromSupermarketHelper(Supermarkt supermarkt)
        {
            bool DateUndetermined = supermarkt.URL.Contains("lidl");

            string PageSource = await HTTPGetUtil.GetDataAsStringFromURL("http://www.supermarktaanbiedingen.com" + supermarkt.URL);

            return DiscountsFromSupermarketParser.GetProductPaginaFromSourceAndURL(PageSource, supermarkt, DateUndetermined);
        }
    }
}
