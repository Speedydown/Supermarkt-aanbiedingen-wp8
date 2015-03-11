using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class ProductPagina
    {
        public Supermarkt supermarkt { get; private set; }
        public string DiscountValid { get; private set; }
        public IList<SupermarktItem> Producten { get; private set; }

        public ProductPagina(Supermarkt supermarkt, string DiscountValid)
        {
            this.supermarkt = supermarkt;
            this.DiscountValid = DiscountValid;
            this.Producten = new List<SupermarktItem>();
        }
    }
}
