using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProductCollector
{
    public class WebClientHelper
    {
        public static string GetContent(string url, Encoding encoding, string cookie = null)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                client.Encoding = encoding;
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    client.Headers[HttpRequestHeader.Cookie] = cookie;
                }

                Regex reg = new Regex(@"^(?<baseAddress>(https?://)[^/]+).+$", RegexOptions.IgnoreCase);

                string baseAddress = reg.Match(url).Groups["baseAddress"].Value;

                if (!string.IsNullOrWhiteSpace(baseAddress))
                {
                    client.BaseAddress = baseAddress;
                }

                var data = client.DownloadString(url);

                return data;
            }
        }
    }
}
