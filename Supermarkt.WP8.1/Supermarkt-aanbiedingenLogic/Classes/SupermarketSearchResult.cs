using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class SupermarketSearchResult
    {
        public Supermarkt supermarkt { get; private set; }
        public IList<Product> producten { get; private set; }

        public SupermarketSearchResult(Supermarkt supermarkt, IList<Product> producten)
        {
            this.supermarkt = supermarkt;
            this.producten = producten;
        }
    }
}
