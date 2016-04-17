using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public sealed class Product : DataObject, SupermarktItem
    {
        public string Quantity { get; private set; }
        public string Price { get; private set; }
        public string DiscountPrice { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        [JsonIgnore]
        public string URL { get; set; }
        public string ImageURL { get; set; }
        [JsonIgnore]
        public string LastSeen { get; set; }
        [JsonIgnore]
        public string ProductHash { get; set; }

        public Product(int ID, string Quantity, string Price, string DiscountPrice, string Name, string Description, string URL, string ImageURL, string LastSeen, string ProductHash) : base(ID)
        {
            this.Quantity = Quantity;
            this.Price = WebUtility.HtmlDecode(Price);
            this.DiscountPrice = WebUtility.HtmlDecode(DiscountPrice);
            this.Name = Name;
            this.LastSeen = LastSeen;
            this.ProductHash = ProductHash;

            if (this.Name.Substring(0, 2) == "Ah")
            {
                this.Name = "AH" + this.Name.Substring(2);
            }

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

            this.URL = URL;
            this.ImageURL = ImageURL;

            try
            {
                if (ImageURL.StartsWith("/"))
                {
                    this.ImageURL = "http://www.supermarktaanbiedingen.com" + this.ImageURL;
                }
            }
            catch
            {

            }
        }

        public override string ToString()
        {
            return "Product";
        }

        public string GetProductHash()
        {
            this.ProductHash = Encrypt.EncryptPassword(Quantity + Price + DiscountPrice + Name + Description);
            return this.ProductHash;
        }
    }
}
