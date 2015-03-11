using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawlerTools;

namespace SaData
{
    public static class DiscountsFromSupermarketParser
    {
        public static ProductPagina GetProductPaginaFromSourceAndURL(string Source, string URL)
        {
            ProductPagina CurrentPage = null;

            //Clear header
            Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<h1 class=\"discount-title\">", Source));

            string ValidUntil = HTMLParserUtil.GetContentAndSubstringInput("<small> - ", "</small>", Source, out Source);

            CurrentPage = new ProductPagina(URL, ValidUntil);

            //Laad producten

            return CurrentPage;
        }

    }
}
