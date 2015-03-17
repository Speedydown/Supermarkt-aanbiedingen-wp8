using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace WebCrawlerTools
{
    public static class HTTPGetUtil
    {
        private static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        private static System.Net.Http.HttpResponseMessage response = null;


        public static async Task<string> GetDataAsStringFromURL(string URL, Encoding encoding = null)
        {
            string output = await Task<string>.Run(async () => 
            {

                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                string Output = string.Empty;

                try
                {

                    response = await client.GetAsync(new Uri(URL));

                    var ByteArray = await response.Content.ReadAsByteArrayAsync();
                    Output = encoding.GetString(ByteArray, 0, ByteArray.Length);
                }
                catch (Exception)
                {

                }

                return Output;

            });

            return output;
        }
    }
}
