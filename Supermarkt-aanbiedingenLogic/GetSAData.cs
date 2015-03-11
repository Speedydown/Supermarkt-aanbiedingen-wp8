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
        public static IAsyncOperation<IList<Supermarkt>> GetPopularSuperMarkets()
        {
            return GetPopularSuperMarketsHelper().AsAsyncOperation();
        }

        private static async Task<IList<Supermarkt>> GetPopularSuperMarketsHelper()
        {
            string PageSource = await HTTPGetUtil.GetDataAsStringFromURL("http://www.supermarktaanbiedingen.com/");

            return PopularSuperMarketsParser.GetPopularSuperMarkets(PageSource);
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
