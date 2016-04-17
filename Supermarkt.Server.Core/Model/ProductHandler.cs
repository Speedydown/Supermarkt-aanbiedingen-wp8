using System.Collections.Generic;
using System.Linq;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public class ProductHandler : DataHandler
    {
        private static ProductHandler _instance = null;
        public static ProductHandler instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductHandler();
                }

                return _instance;
            }
        }

        private static readonly Field QuantityField = new Field("Quantity", typeof(string), 50);
        private static readonly Field PriceField = new Field("Price", typeof(string), 50);
        private static readonly Field DiscountPriceField = new Field("DiscountPrice", typeof(string), 50);
        private static readonly Field NameField = new Field("Name", typeof(string), 500);
        private static readonly Field DescriptionField = new Field("Description", typeof(string), 750);
        private static readonly Field URLField = new Field("URL", typeof(string), 250);
        private static readonly Field ImageURLField = new Field("ImageURL", typeof(string), 350);
        private static readonly Field LastSeenField = new Field("LastSeen", typeof(string), 25);
        private static readonly Field ProductHashField = new Field("ProductHash", typeof(string), 250);

        private ProductHandler() : base("Products", new Field[]
            {
                QuantityField,
                PriceField,
                DiscountPriceField,
                NameField,
                DescriptionField,
                URLField,
                ImageURLField,
                LastSeenField,
                ProductHashField
            }, typeof(Product))
        {
            this.customQueries = new string[] { };
        }

        public Product AddProduct(Product product)
        {
            List<Product> SimilarProducts = base.GetObjectByFieldsAndSearchQuery(new Field[] { ProductHashField }, product.GetProductHash(), true, 0, OrderBy.ASC, NameField).Cast<Product>().ToList();

            foreach (Product p in SimilarProducts)
            {
                p.LastSeen = TimeConverter.GetDateTime().ToString("d-M-yyyy");
                this.UpdateObject(p);

                return p;
            }

            product.LastSeen = TimeConverter.GetDateTime().ToString("d-M-yyyy");
            return base.AddObject(product) as Product;
        }

        public List<Product> GetAllProducts()
        {
            return base.GetObjectList(0, OrderBy.ASC, NameField).Cast<Product>().ToList();
        }

        public List<Product> GetProductsByName(string Name, bool Exact)
        {
            //Hier moet waarschijnlijk een group by tussen
            return base.GetObjectByFieldsAndSearchQuery(new Field[] { NameField }, Name, Exact, 0, OrderBy.ASC, NameField).Cast<Product>().ToList();
        }

        public List<Product> GetProductsByProductPageID(int ID)
        {
            List<ProductLink> ProductLinks = ProductLinkHandler.instance.GetProductLinksByProductPageID(ID);
            List<int> ProductIDs = new List<int>();

            foreach (ProductLink pl in ProductLinks)
            {
                ProductIDs.Add(pl.ProductID);
            }

            return base.GetObjectsByIDArray(ProductIDs.ToArray()).Cast<Product>().ToList();
        }

        public override string ToString()
        {
            return "ProductHandler";
        }
    }
}
