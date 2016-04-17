using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public sealed class ProductPagina : DataObject
    {
        public int SuperMarktID { get; set; }
        [JsonIgnore]
        public Supermarkt supermarkt { get; private set; }

        private string _DiscountValid = null;
        public string DiscountValid
        {
            get
            {
                if (supermarkt.URL.Contains("lidl"))
                {
                    return "Er is geen einddatum bekend.";
                }
                else
                {
                    return _DiscountValid;
                }
            }
            private set
            {
                _DiscountValid = value;
            }
        }

        [JsonIgnore]
        public string RealDiscountValid
        {
            get
            {
                return _DiscountValid;
            }
        }

        public IList<Product> Producten { get; private set; }
        public bool Expired {get; set;}
        public int CreatedHour { get; set; }
        public int ProductPageDay { get; set; }

        public ProductPagina(string DiscountValid, IList<Product> Producten, Supermarkt supermarkt) : base(0)
        {
            this.SuperMarktID = supermarkt.ID;
            this.supermarkt = supermarkt;
            this.DiscountValid = DiscountValid;
            this.Producten = Producten;
            this.CreatedHour = TimeConverter.GetDateTime().Hour;
            this.ProductPageDay = TimeConverter.GetDateTime().Day;
        }

        public ProductPagina(int ID, int SuperMarktID, string DiscountValid, bool Expired, int CreatedHour, int ProductPageDay) : base(ID)
        {
            this.SuperMarktID = SuperMarktID;
            this.supermarkt = SupermarktHandler.instance.GetSupermarketByID(this.SuperMarktID);
            this.DiscountValid = DiscountValid;
            this.Expired = Expired;
            this.CreatedHour = CreatedHour;
            this.ProductPageDay = ProductPageDay;

            this.Producten = ProductHandler.instance.GetProductsByProductPageID(this.ID);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ProductPagina Deserialize(string Input)
        {
            return JsonConvert.DeserializeObject<ProductPagina>(Input);
        }

        public string CreateHash()
        {
            string Hash = string.Empty;

            if (this.Producten.Count == 0)
            {
                return Hash;
            }

            foreach (Product p in this.Producten)
            {
                Hash += Encrypt.EncryptPassword(p.Name);
            }

            return Hash;
        }
    }
}
