using System;
using System.Collections.Generic;
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

            CurrentPage = new ProductPagina(supermarkt, ValidUntil);

            GetProductsFromSource(Source, CurrentPage);

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

                    if (IndexOfBR < 15 && IndexOfBR != -1)
                    {
                        ProductQuantity = HTMLParserUtil.GetContentAndSubstringInput("<small>", "<br \\>", Source, out Source);
                    }

                    string ProductPrice = HTMLParserUtil.GetContentAndSubstringInput("<del>", "</del>", Source, out Source);
                    string ProductDiscountPrice = HTMLParserUtil.GetContentAndSubstringInput("<span class=\"price\">", "</span>", Source, out Source);
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
