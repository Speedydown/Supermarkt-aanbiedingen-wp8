using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public static class SearchHandler
    {
        public static IList<SupermarketSearchResult> SearchForProductenInDiscounts(IList<Supermarkt> supermarkten, string SearchPhrase)
        {
            List<SupermarketSearchResult> Producten = new List<SupermarketSearchResult>();

            if (SearchPhrase.Length < 3)
            {
                return Producten;
            }

            SearchPhrase = SearchPhrase.ToLower();

            foreach (Supermarkt s in supermarkten)
            {
                List<Product> FoundProducts = new List<Product>();

                foreach (Product p in s.ProductPagina.Producten)
                {
                    if (p.Name.ToLower().Contains(SearchPhrase) || p.Description.ToLower().Contains(SearchPhrase))
                    {
                        FoundProducts.Add(p);
                    }
                }

                if (FoundProducts.Count > 0)
                {
                    Producten.Add(new SupermarketSearchResult(s, FoundProducts));
                }
            }

            return Producten;
        }
    }
}
