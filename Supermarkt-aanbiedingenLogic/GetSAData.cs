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
    }
}
