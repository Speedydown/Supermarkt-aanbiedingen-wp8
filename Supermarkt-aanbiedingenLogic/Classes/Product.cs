using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class Product : SupermarktItem
    {
        public string Quantity { get; private set; }
        public string Price { get; private set; }
        public string DiscountPrice { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string URL { get; set; }
        public string ImageURL { get; set; }

        public Product(string Quantity, string Price, string DiscountPrice, string Name, string Description, string URL, string ImageURL)
        {
            this.Quantity = Quantity;
            this.Price = Price;
            this.DiscountPrice = DiscountPrice;
            this.Name = Name;
            this.Description = Description;
            this.URL = URL;
            this.ImageURL =  ImageURL;
        }
    }
}
