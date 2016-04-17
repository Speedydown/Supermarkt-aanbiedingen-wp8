using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class ProductPagina
    {
        public int ID { get; private set; }
        [JsonIgnore]
        public Supermarkt supermarkt { get; private set; }
        public string DiscountValid { get; private set; }
        public IList<Product> Producten { get; private set; }
        public Product SelectedItem { get; set; }

        [JsonIgnore]
        public string ProductCountText
        {
            get
            {
                return "Aantal aanbiedingen: " + Producten.Count;
            }
        }

        public ProductPagina(int ID, string DiscountValid, IList<Product> Producten, Supermarkt supermarkt)
        {
            this.ID = ID;
            this.supermarkt = supermarkt;
            this.DiscountValid = DiscountValid;
            this.Producten = Producten;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ProductPagina Deserialize(string Input)
        {
            return JsonConvert.DeserializeObject<ProductPagina>(Input);
        }
    }
}
