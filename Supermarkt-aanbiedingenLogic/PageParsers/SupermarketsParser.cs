using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawlerTools;

namespace Supermarkt_aanbiedingenLogic
{
    class SupermarketsParser
    {
        public static IList<Supermarkt> GetSupermarkets(string Source)
        {
            List<Supermarkt> Supermarkets = new List<Supermarkt>();

            //RemoveHeader
            Source = Source.Substring(HTMLParserUtil.GetPositionOfStringInHTMLSource("<div class=\"container\"><h3>Aanbiedingen</h3", Source));

            while (Source.Length > 0)
            {
                try
                {
                    string URL = HTMLParserUtil.GetContentAndSubstringInput("<a href=\"", "\">", Source, out Source, "", false);
                    string Name = HTMLParserUtil.GetContentAndSubstringInput("\">", "</a><br/>", Source, out Source);
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
