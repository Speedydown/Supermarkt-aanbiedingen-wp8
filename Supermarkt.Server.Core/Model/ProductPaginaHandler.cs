using SupermarktCore.Logics;
using System.Collections.Generic;
using System.Linq;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public class ProductPaginaHandler : DataHandler
    {
        private static ProductPaginaHandler _instance = null;
        public static ProductPaginaHandler instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductPaginaHandler();
                }

                return _instance;
            }
        }

        private static readonly Field SupermarktIDField = new Field("SuperMarktID", typeof(int), 1);
        private static readonly Field DiscountValidField = new Field("DiscountValid", typeof(string), 100);
        private static readonly Field ExpiredField = new Field("Expired", typeof(bool), 1);
        private static readonly Field CreatedHourField = new Field("CreatedHour", typeof(int), 1);
        private static readonly Field ProductPageDayField = new Field("ProductPageDay", typeof(int), 1);

        private ProductPaginaHandler()
            : base("ProductPage", new Field[] { SupermarktIDField, DiscountValidField, ExpiredField, CreatedHourField, ProductPageDayField }, typeof(ProductPagina))
        {
            this.customQueries = new string[] { };
        }

        public ProductPagina GetProductPageBySupermarketIDAndDiscountDate(int SupermarketID, string DiscountValid)
        {
            List<ProductPagina> ProductPaginas = base.GetObjectsByChildObjectID(SupermarktIDField, SupermarketID, 0, OrderBy.DESC, IDField).Cast<ProductPagina>().ToList();

            foreach (ProductPagina p in ProductPaginas)
            {
                if (p.RealDiscountValid == DiscountValid)
                {
                    return p;
                }
            }

            return null;
        }

        public ProductPagina GetActiveProductPaginaBySupermarktID(int ID)
        {
            List<ProductPagina> ProductPaginas = base.GetObjectsByChildObjectID(SupermarktIDField, ID, 0, OrderBy.DESC, IDField).Cast<ProductPagina>().ToList();

            for (int i = 0; i < ProductPaginas.Count; i++)
            {
                if (ProductPaginas[i].Expired)
                {
                    ProductPaginas.RemoveAt(i);
                    i--;
                    continue;
                }

                if (TimeConverter.GetDateTime().Hour > ProductPaginas[i].CreatedHour + 3 ||
                   ProductPaginas[i].ProductPageDay != TimeConverter.GetDateTime().Day)
                {
                    ProductPaginas[i].Expired = true;
                    this.UpdateObject(ProductPaginas[i]);
                    ProductPaginas.RemoveAt(i);
                    i--;
                }
            }

            if (ProductPaginas.Count == 0)
            {
                ProductPagina NewProductPagina = GetSAData.GetDiscountsFromSupermarket(SupermarktHandler.instance.GetObjectByID(ID) as Supermarkt);
                NewProductPagina.SuperMarktID = ID;
                ProductPagina OldProductPagina = this.GetProductPageBySupermarketIDAndDiscountDate(ID, NewProductPagina.RealDiscountValid);

                if (OldProductPagina == null)
                {
                    this.AddObject(NewProductPagina);

                    foreach (Product p in NewProductPagina.Producten)
                    {
                        p.GetProductHash();
                        Product NewProduct = ProductHandler.instance.AddProduct(p);
                        ProductLinkHandler.instance.AddObject(new ProductLink(0, NewProduct.ID, NewProductPagina.ID));
                    }

                    //Delete old records but keep product data
                    ProductPaginas = base.GetObjectsByChildObjectID(SupermarktIDField, ID, 0, OrderBy.DESC, IDField).Cast<ProductPagina>().ToList();

                    foreach (ProductPagina p in ProductPaginas)
                    {
                        if (p.ID == NewProductPagina.ID)
                        {
                            continue;
                        }

                        foreach (ProductLink l in ProductLinkHandler.instance.GetProductLinksByProductPageID(p.ID))
                        {
                            ProductLinkHandler.instance.DeleteProductLink(l);
                        }

                        base.DeleteObject(p);
                    }

                    return NewProductPagina;
                }
                else
                {
                    OldProductPagina.Expired = false;
                    OldProductPagina.CreatedHour = TimeConverter.GetDateTime().Hour;
                    OldProductPagina.ProductPageDay = TimeConverter.GetDateTime().Day;
                    this.UpdateObject(OldProductPagina);
                    return OldProductPagina;
                }

            }
            else
            {
                return ProductPaginas.First();
            }
        }

        public override string ToString()
        {
            return "ProductPaginaHandler";
        }
    }
}
