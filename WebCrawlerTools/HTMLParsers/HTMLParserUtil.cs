using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawlerTools
{
    public static class HTMLParserUtil
    {
        public static string GetContentAndSubstringInput(string StartHtmlTag, string EndHTMLTag, string InputSource, out string OutputSource, string SecondaryEndHTMLTag = "", bool CutToEndOfEndHTMLTag = true)
        {
            int StartIndexOFContent = GetPositionOfStringInHTMLSource(StartHtmlTag, InputSource);

            if (StartIndexOFContent == -1)
            {
                throw new Exception("Could not find StartHtmlTag in source.");
            }

            int EndIndexOfContent = -1;

            if (SecondaryEndHTMLTag.Length > 0)
            {
                EndIndexOfContent = GetPositionOfStringInHTMLSource(SecondaryEndHTMLTag, InputSource, false);
            }

            if (EndIndexOfContent == -1)
            {
                EndIndexOfContent = GetPositionOfStringInHTMLSource(EndHTMLTag, InputSource, false);
            }

            if (EndIndexOfContent == -1 || StartIndexOFContent >= EndIndexOfContent)
            {
                throw new Exception("Could not find EndHTMLTag in source.");
            }

            string Content = InputSource.Substring(StartIndexOFContent, EndIndexOfContent - StartIndexOFContent);
            OutputSource = InputSource.Substring(GetPositionOfStringInHTMLSource(EndHTMLTag, InputSource, CutToEndOfEndHTMLTag));

            return Content;
        }

        public static int GetPositionOfStringInHTMLSource(string SearchQuery, string Source, bool GetEndoFStringIndex = true)
        {
            int Index = Source.IndexOf(SearchQuery);

            if (GetEndoFStringIndex)
                return (Index != -1) ? Index + SearchQuery.Length : Index;

            return Index;
        }

        private static readonly string[] RemoveFilter = new string[] { "<br>", "<br />", "<br/>", "<BR>", "<p>", "<P>", "<em>", "</em>", "<strong>", "</strong>", "<u>", "</u>", "<b>", "</b>" };

        public static string CleanHTMLString(string Input)
        {
            foreach (string s in RemoveFilter)
            {
                Input = Input.Replace(s, "");
            }

            Input = Input.Replace("&nbsp", " ");
            Input = WebUtility.HtmlDecode(Input);

            return Input;
        }
    }
}
