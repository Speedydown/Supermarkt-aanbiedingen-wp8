using SupermarktCore.Common.Util;
using SupermarktCore.Model;
using System;
using System.Collections.Generic;

namespace SupermarktCore.Logics
{
    class SupermarketsParser
    {
        public static IList<Supermarkt> GetSupermarkets(string Source)
        {
            List<Supermarkt> Supermarkets = new List<Supermarkt>();

            //RemoveHeader
            Source = Source.Substring(HtmlParserUtil.GetPositionOfStringInHTMLSource("<div class=\"container\"><h3>Aanbiedingen</h3", Source));

            while (Source.Length > 0)
            {
                try
                {
                    string URL = HtmlParserUtil.GetContentAndSubstringInput("<a href=\"", "\">", Source, out Source, "", false);
                    string Name = HtmlParserUtil.GetContentAndSubstringInput("\">", "</a><br/>", Source, out Source);
                    string Title = "";

                    Supermarkets.Add(new Supermarkt(Name, URL, Title, null));
                }
                catch (Exception)
                {
                    break;
                }
            }

            return Supermarkets;
        }
    }
}
