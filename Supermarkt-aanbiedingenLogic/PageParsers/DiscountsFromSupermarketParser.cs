using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawlerTools;

namespace Supermarkt_aanbiedingenLogic
{
    public static class DiscountsFromSupermarketParser
    {
        public static ProductPagina GetProductPaginaFromSourceAndURL(string Source, Supermarkt supermarkt, bool DateIndetermined)
        {
            ProductPagina CurrentPage = null;

            //Clear header
            Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<h1 class=\"discount-title\">", Source));

            string ValidUntil = HTMLParserUtil.GetContentAndSubstringInput("<small> - ", "</small>", Source, out Source);

            if (DateIndetermined)
            {
                ValidUntil = "Geen einddatum van de aanbiedingen bekend.";
            }

            CurrentPage = new ProductPagina(ValidUntil, new List<Product>(), supermarkt);

            GetProductsFromSource(Source, CurrentPage);

            Debug.WriteLine(supermarkt.Name + ": " + (CurrentPage.Producten.Last() as Product).Name);

            return CurrentPage;
        }

        private static void GetProductsFromSource(string Source, ProductPagina Page)
        {
            Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<ol class=\"products-table\">", Source));

            while(true)
            {
                try
                {
                    //jump to start:
                    Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<span class=\"product-data\">", Source));

                    string ProductQuantity = string.Empty;

                    int IndexOfBR = Source.IndexOf("<br \\>");

                    bool SkipPrice = false;

                    if (IndexOfBR < 15 && IndexOfBR != -1)
                    {
                        try
                        {
                            ProductQuantity = HTMLParserUtil.GetContentAndSubstringInput("<small>", "<br \\>", Source, out Source);
                        }
                        catch
                        {
                            try
                            {
                                ProductQuantity = HTMLParserUtil.GetContentAndSubstringInput("<b>", "</b>", Source, out Source);
                            }
                            catch
                            {

                            }

                            SkipPrice = true;
                        }
                    }

                    string ProductPrice = string.Empty;
                    string ProductDiscountPrice = string.Empty;

                    if (!SkipPrice)
                    {
                        ProductPrice = HTMLParserUtil.GetContentAndSubstringInput("<del>", "</del>", Source, out Source);
                        ProductDiscountPrice = HTMLParserUtil.GetContentAndSubstringInput("<span class=\"price\">", "</span>", Source, out Source);
                    }

                    string ProductName = HTMLParserUtil.GetContentAndSubstringInput("<a class=\"product-title\" title=\"", "\" href=\"", Source, out Source, "", false);
                    string ProductURL = HTMLParserUtil.GetContentAndSubstringInput(" href=\"", "\">", Source, out Source);
                   
                    
                    

                    //Skip script:
                    Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<img ", Source, false));
                    string ProductImageURL = HTMLParserUtil.GetContentAndSubstringInput("=\"", "\" alt=\"", Source, out Source);

                    //Skip to description
                    Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<p class=\"product-description\" title=\"", Source, false));

                    string ProductDescription = HTMLParserUtil.GetContentAndSubstringInput("title=\"", "\">", Source, out Source);

                    Page.Producten.Add(new Product(ProductQuantity, ProductPrice, ProductDiscountPrice, ProductName, ProductDescription, ProductURL, ProductImageURL));
                }
                catch(Exception)
                {
                    break;
                }
            }
        }

    }
}
