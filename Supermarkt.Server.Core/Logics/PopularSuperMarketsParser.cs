using SupermarktCore.Common.Util;
using SupermarktCore.Model;
using System;
using System.Collections.Generic;

namespace SupermarktCore.Logics
{
    public static class PopularSuperMarketsParser
    {
        public static IList<Supermarkt> GetPopularSuperMarkets(string Source)
        {
            List<Supermarkt> Supermarkets = new List<Supermarkt>();

            //RemoveHeader
            Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<ol id=\"supermarkets\"><li>", Source));

            while (Source.Length > 0)
            {
                try
                {
                    string URL = HtmlParserUtil.GetContentAndSubstringInput("<a href=\"", "\" title=\"", Source, out Source, "", false);
                    string Title = HtmlParserUtil.GetContentAndSubstringInput("\" title=\"", "\"><span class=\"shop-large", Source, out Source);
                    string Name = HtmlParserUtil.GetContentAndSubstringInput("</span>", "</a></li>", Source, out Source);

                    Supermarkets.Add(new Supermarkt(Name, URL, Title, null));
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
