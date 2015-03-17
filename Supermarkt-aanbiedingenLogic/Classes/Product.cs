using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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

        public Visibility PriceBlockVisibility
        {
            get
            {
                return (Price.Trim().Length > 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility DiscountPriceBlockVisiblity
        {
            get
            {
                return (DiscountPrice.Trim().Length > 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Product(string Quantity, string Price, string DiscountPrice, string Name, string Description, string URL, string ImageURL)
        {
            this.Quantity = Quantity;
            this.Price = WebUtility.HtmlDecode(Price);
            this.DiscountPrice = WebUtility.HtmlDecode(DiscountPrice);
            this.Name = WebUtility.HtmlDecode(Name);

            Description = WebUtility.HtmlDecode(Description);

            try
            {
                Description = Description.Trim();

                if (Description.Length > 0 && Description[Description.Length - 1] == ',')
                {
                    this.Description = Description.Substring(0, Description.Length - 1);
                }
                else
                {
                    this.Description = Description;
                }
            }
            catch(Exception)
            {
                this.Description = Description;
            }

            //this.Description = this.Description.Replace(",", "\n");
            this.Description = string.Join(
                             "\n",
                             this.Description.Split('\n').Select(s => s.Trim()));



            this.URL = URL;
            this.ImageURL =  ImageURL;

            //verschil nog berekenen?
        }
    }
}
