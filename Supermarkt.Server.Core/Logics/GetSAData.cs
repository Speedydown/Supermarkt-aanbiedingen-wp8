using SupermarktCore.Common.Util;
using SupermarktCore.Model;
using System.Collections.Generic;

namespace SupermarktCore.Logics
{
    public static class GetSAData
    {
        public static int ConfigRequests = 0;
        public static int Requests = 0;
        public static int CrawledFromInternetRequests = 0;

        public static IList<Supermarkt> GetAllSupermarkets()
        {
            ConfigRequests++;
            IList<Supermarkt> Supermarkets = CacheHandler.GetData("GetAllSupermarkets", "", new string[] { }) as IList<Supermarkt>;

            if (Supermarkets != null && Supermarkets.Count > 1)
            {
                return Supermarkets;
            }

            string PageSource = HttpHandler.GetDataFromWebPage("http://www.supermarktaanbiedingen.com/");
            Supermarkets = SupermarketsParser.GetSupermarkets(PageSource);

            CacheHandler.AddToCache("GetAllSupermarkets", "", new string[] { }, Supermarkets);

            return Supermarkets;
        }

        public static ProductPagina GetDiscountsFromSupermarket(Supermarkt supermarkt)
        {
            ProductPagina productPagina = CacheHandler.GetData("GetDiscountsFromSupermarket", supermarkt.URL, new string[] { }) as ProductPagina;

            if (productPagina != null)
            {
                return productPagina;
            }

            bool DateUndetermined = supermarkt.URL.Contains("lidl");

            string PageSource = HttpHandler.GetDataFromWebPage("http://www.supermarktaanbiedingen.com" + supermarkt.URL);

            productPagina = DiscountsFromSupermarketParser.GetProductPaginaFromSourceAndURL(PageSource, supermarkt);

            CrawledFromInternetRequests++;
            CacheHandler.AddToCache("GetDiscountsFromSupermarket", supermarkt.URL, new string[] { }, productPagina);

            return productPagina;
        }

        public static List<Supermarkt> GetDiscountsFromSupermarketList(List<Supermarkt> Supermarkets)
        { 
            foreach (Supermarkt s in Supermarkets)
            {
                s.GetProductpaginaOld();
            }

            return Supermarkets;
        }
    }
}
