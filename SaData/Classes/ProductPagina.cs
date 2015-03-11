using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaData
{
    public sealed class ProductPagina
    {
        public string URL { get; private set; }
        public string DiscountValid { get; private set; }
        public List<SupermarktItem> Producten { get; private set; }

        public ProductPagina(string URL, string DiscountValid)
        {
            this.URL = URL;
            this.DiscountValid = DiscountValid;
        }
    }
}
