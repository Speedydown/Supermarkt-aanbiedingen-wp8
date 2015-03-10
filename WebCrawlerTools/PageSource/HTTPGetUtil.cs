using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawlerTools
{
    public static class HTTPGetUtil
    {
        public static async Task<string> GetDataAsStringFromURL(string URL, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            string Output = string.Empty;

            try
            {
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(new Uri(URL));

                var ByteArray = await response.Content.ReadAsByteArrayAsync();
                Output = encoding.GetString(ByteArray, 0, ByteArray.Length);
            }
            catch (Exception)
            {

            }

            return Output;
        }
    }
}
