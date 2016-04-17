using SupermarktCore.Common.Util;
using SupermarktCore.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WZWVAPI;

namespace SupermarktCore.Logics
{
    public static class DiscountsFromSupermarketParser
    {
        public static ProductPagina GetProductPaginaFromSourceAndURL(string Source, Supermarkt supermarkt)
        {
            ProductPagina CurrentPage = null;
            try
            {
                //Clear header
                Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<h1 class=\"discount-title\">", Source));

                string ValidUntil = HtmlParserUtil.GetContentAndSubstringInput("<small> - ", "</small>", Source, out Source);

                while (ValidUntil.Contains('<') && ValidUntil.Contains('>'))
                {
                    try
                    {
                        int Index1 = ValidUntil.IndexOf('<');
                        int Index2 = ValidUntil.IndexOf('>', Index1);

                        ValidUntil = ValidUntil.Replace(ValidUntil.Substring(Index1, Index2 + 1 - Index1), "");
                    }
                    catch
                    {
                        break;
                    }
                }

                CurrentPage = new ProductPagina(ValidUntil, new List<Product>(), supermarkt);

                GetProductsFromSource(Source, CurrentPage);

                Debug.WriteLine(supermarkt.Name + ": " + (CurrentPage.Producten.Last() as Product).Name);
            }
            catch (Exception e)
            {
                throw e;
            }

            return CurrentPage;
        }

        private static void GetProductsFromSource(string Source, ProductPagina Page)
        {
            Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<ol class=\"products-table\">", Source));

            while(true)
            {
                try
                {
                    //jump to start:
                    Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<span class=\"product-data\">", Source));

                    string ProductQuantity = string.Empty;

                    int IndexOfBR = Source.IndexOf("<br \\>");

                    bool SkipPrice = false;

                    if (IndexOfBR < 15 && IndexOfBR != -1)
                    {
                        try
                        {
                            ProductQuantity = HtmlParserUtil.GetContentAndSubstringInput("<small>", "<br \\>", Source, out Source);
                        }
                        catch
                        {
                            try
                            {
                                ProductQuantity = HtmlParserUtil.GetContentAndSubstringInput("<b>", "</b>", Source, out Source);
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
                        ProductPrice = HtmlParserUtil.GetContentAndSubstringInput("<del>", "</del>", Source, out Source);
                        ProductDiscountPrice = HtmlParserUtil.GetContentAndSubstringInput("<span class=\"price\">", "</span>", Source, out Source);
                    }

                    string ProductName = HtmlParserUtil.GetContentAndSubstringInput("<a class=\"product-title\" title=\"", "\" href=\"", Source, out Source, "", false);
                    string ProductURL = HtmlParserUtil.GetContentAndSubstringInput(" href=\"", "\">", Source, out Source); 

                    //Skip script:
                    Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<img ", Source, false));
                    string ProductImageURL = HtmlParserUtil.GetContentAndSubstringInput("=\"", "\" alt=\"", Source, out Source);

                    //Skip to description
                    Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<p class=\"product-description\" title=\"", Source, false));

                    string ProductDescription = HtmlParserUtil.GetContentAndSubstringInput("title=\"", "\">", Source, out Source);

                    Page.Producten.Add(new Product(0, ProductQuantity, ProductPrice, ProductDiscountPrice, ProductName, ProductDescription, ProductURL, ProductImageURL, TimeConverter.GetDateTime().ToString("d-M-yyyy"), ""));
                }
                catch(Exception)
                {
                    break;
                }
            }
        }

    }
}
