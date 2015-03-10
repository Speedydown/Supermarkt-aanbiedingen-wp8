using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawlerTools;

namespace Supermarkt_aanbiedingenLogic
{
    public static class PopularSuperMarketsParser
    {
        public static IList<Supermarkt> GetPopularSuperMarkets(string Source)
        {
            List<Supermarkt> Supermarkets = new List<Supermarkt>();

            //RemoveHeader
            Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<ol id=\"supermarkets\"><li>", Source));

            while (Source.Length > 0)
            {
                try
                {
                    string URL = HTMLParserUtil.GetContentAndSubstringInput("<a href=\"", "\" title=\"", Source, out Source, "", false);
                    string Title = HTMLParserUtil.GetContentAndSubstringInput("\" title=\"", "\"><span class=\"shop-large", Source, out Source);
                    string Name = HTMLParserUtil.GetContentAndSubstringInput("</span>", "</a></li>", Source, out Source);

                    Supermarkets.Add(new Supermarkt(Name, URL, Title));
                }
                catch(Exception)
                {
                    break;
                }
            }

            return Supermarkets;
        }
    }
}
