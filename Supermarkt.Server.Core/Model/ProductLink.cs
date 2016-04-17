using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public class ProductLink : DataObject
    {
        public int ProductID { get; private set; }
        public int ProductPaginaID { get; private set; }

        public ProductLink(int ID, int ProductID, int ProductPaginaID) : base(ID)
        {
            this.ProductID = ProductID;
            this.ProductPaginaID = ProductPaginaID;
        }

        public override string ToString()
        {
            return "ProductLink";
        }
    }
}
