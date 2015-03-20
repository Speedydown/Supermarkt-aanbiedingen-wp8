using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class BoodschappenlijstjeItem
    {
        public int Count { get; private set; }
        public Product SupermarktItem { get; private set; }

        public BoodschappenlijstjeItem(int Count, Product SupermarktItem)
        {
            this.Count = Count;
            this.SupermarktItem = SupermarktItem;
        }

        public override string ToString()
        {
            return Count + ": " + SupermarktItem.Name;
        }
    }
}
