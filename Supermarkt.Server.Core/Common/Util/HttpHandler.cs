using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SupermarktCore.Common.Util
{
    internal static class HttpHandler
    {
        public static string GetDataFromWebPage(string URL)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                return client.DownloadString(URL);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}