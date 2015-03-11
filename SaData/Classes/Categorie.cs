using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaData
{
    public class Categorie : SupermarktItem
    {
        public List<SupermarktItem> Producten { get; protected set; }

        public Categorie()
            : base("", "")
        {

        }
    }
}
