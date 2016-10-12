using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.Collector
{
    public class CollectorFactory
    {
        public static string GetContent(string url)
        {
            using(var client=new HttpClient())
            {
                var response =  client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }

                return string.Empty;
            }
        }
    }
}
