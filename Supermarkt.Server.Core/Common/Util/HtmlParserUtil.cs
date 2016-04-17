using System;
using System.Net;

namespace SupermarktCore.Common.Util
{
    internal static class HtmlParserUtil
    {
        public static string GetContentAndSubstringInput(string StartHtmlTag, string EndHTMLTag, string InputSource, out string OutputSource, string SecondaryEndHTMLTag = "", bool CutToEndOfEndHTMLTag = true)
        {
            int StartIndexOFContent = GetPositionOfStringInHTMLSource(StartHtmlTag, InputSource);

            if (StartIndexOFContent == -1)
            {
                throw new Exception("Could not find StartHtmlTag in source.");
            }

            int EndIndexOfContent = -1;

            EndIndexOfContent = GetPositionOfStringInHTMLSource(EndHTMLTag, InputSource, false);

            if (EndIndexOfContent == -1 && SecondaryEndHTMLTag.Length > 0)
            {
                EndIndexOfContent = GetPositionOfStringInHTMLSource(SecondaryEndHTMLTag, InputSource, false);
                EndHTMLTag = SecondaryEndHTMLTag;
            }

            if (EndIndexOfContent == -1 || StartIndexOFContent > EndIndexOfContent)
            {
                throw new Exception("Could not find EndHTMLTag in source.");
            }

            if (StartIndexOFContent == EndIndexOfContent)
            {
                OutputSource = InputSource.Substring(GetPositionOfStringInHTMLSource(EndHTMLTag, InputSource, CutToEndOfEndHTMLTag));
                return string.Empty;
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

        private static readonly string[] RemoveFilter = new string[] { "<br>", "<br />", "\r", "<br/>", "</span>", "<BR>", "<p>", "<P>", "</p>", "</P>", "<em>", "</em>", "<strong>", "</strong>", "<u>", "</u>", "<b>", "</b>", ";" };

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

        public static string CleanDoubleBreakLinesFromInput(string Input)
        {
            while (Input.Contains("\n\n\n"))
            {
                Input = Input.Replace("\n\n\n", "\n\n");
            }

            return Input;
        }

        //Needs Revision
        public static string CleanHTTPTagsFromInput(string Input)
        {
            while (Input.Contains("http"))
            {
                string Temp = Input.Substring(0, Input.IndexOf("http"));
                string TailOfArticle = Input.Substring(Input.IndexOf("http"));
                Temp += TailOfArticle.Substring(TailOfArticle.IndexOf(" "));
                Input = Temp;
            }

            return Input;
        }

        public static string CleanHTMLTagsFromString(string Input)
        {
            Input = CleanHTMLString(Input);

            while (true)
            {
                int IndexOfOpenBacket = Input.IndexOf("<");
                int IndexOfCloseBracket = Input.IndexOf(">");

                if (IndexOfCloseBracket == -1 ||
                    IndexOfOpenBacket == -1 ||
                    IndexOfCloseBracket < IndexOfOpenBacket)
                {
                    break;
                }

                string CleanInput = Input.Substring(0, IndexOfOpenBacket);
                Input = CleanInput + Input.Substring(IndexOfCloseBracket + 1);
            }

            return Input;
        }
    }

}
