using Newtonsoft.Json;
using SupermarktCore.Logics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public sealed class Supermarkt : DataObject
    {
        public string Name { get; private set; }
        public string URL { get; private set; }
        public string Title { get; private set; }
        public string ImageURL { get; private set; }
        public ProductPagina ProductPagina { get; private set; }
        private bool _SupermarketEnabled = false;
        public bool Deleted { get; set; }

        [JsonConstructor]
        public Supermarkt(string Name, string URL, string Title, ProductPagina ProductPagina)
            : this(0, Name, URL, Title, string.Empty, false)
        {
            this.ProductPagina = ProductPagina;
        }

        public Supermarkt(int ID, string Name, string URL, string Title, string ImageURL, bool Deleted)
            : base(ID)
        {
            this.Name = Name;

            if (this.Name.Contains("Aanbiedingen "))
            {
                this.Name = this.Name.Substring("Aanbiedingen ".Length);
            }

            this.URL = URL;
            this.Title = Title;
            this.ImageURL = ImageURL;
            this.Deleted = Deleted;
        }

        public void GetProductpagina()
        {
            this.ProductPagina = ProductPaginaHandler.instance.GetActiveProductPaginaBySupermarktID(this.ID);
        }

        public void GetProductpaginaOld()
        {
            this.ProductPagina = GetSAData.GetDiscountsFromSupermarket(this);
        }  

        public override string ToString()
        {
            return "Supermarkt";
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Supermarkt Deserialize(string Input)
        {
            return JsonConvert.DeserializeObject<Supermarkt>(Input);
        }
    }


}
