using System.Collections.Generic;
using System.Linq;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public class ProductLinkHandler : DataHandler
    {
        private static ProductLinkHandler _instance = null;
        public static ProductLinkHandler instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductLinkHandler();
                }

                return _instance;
            }
        }

        private static readonly Field ProductIDField = new Field("ProductID", typeof(int), 1);
        private static readonly Field ProductPaginaIDField = new Field("ProductPaginaID", typeof(int), 1);

        private ProductLinkHandler() : base("ProductLinks", new Field[] { ProductIDField, ProductPaginaIDField}, typeof(ProductLink))
        {
            this.customQueries = new string[] { };
        }

        public List<ProductLink> GetProductLinksByProductPageID(int ID)
        {
            return base.GetObjectsByChildObjectID(ProductPaginaIDField, ID, 0, OrderBy.ASC, ProductIDField).Cast<ProductLink>().ToList();
        }

        public void DeleteProductLink(ProductLink productLink)
        {
            base.DeleteObject(productLink);
        }

        public override string ToString()
        {
            return "ProductLinkHandler";
        }
    }
}
