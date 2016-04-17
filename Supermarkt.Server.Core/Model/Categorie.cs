using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarktCore.Model
{
    public sealed class Categorie : SupermarktItem
    {
        public string URL { get;  set; }
        public string ImageURL { get;  set; }
        public IList<SupermarktItem> Producten { get; private set; }

        public Categorie()
        {

        }
    }
}
