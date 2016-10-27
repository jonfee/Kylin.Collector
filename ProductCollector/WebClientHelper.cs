using System;
using System.Collections.Generic;
using System.IO;
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

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                var data = client.DownloadString(url);

                return data ?? string.Empty;
            }
        }


        /// <summary>
        /// 下载网络图片
        /// </summary>
        /// <param name="imgDic">网络图片标识与图片地址（key为上传后的文件名，value为网络图片地址），建议key为“ID_序号”（如：3412312_v1）</param>
        /// <param name="visitUrl">下载后的访问地址</param>
        /// <param name="saveDirectory">要保存的目录，物理目录（如：D:\\upload）</param>
        /// <param name="originalIfFailure">下载失败时是否保留原图地址</param>
        /// <returns></returns>
        public static Dictionary<string, string> DownloadFile(Dictionary<string, string> imgDic, string visitUrl, string saveDirectory, bool originalIfFailure = false)
        {
            Dictionary<string, string> newImgDic = new Dictionary<string, string>();

            for (int i = 0; i < imgDic.Count; i++)
            {
                var img = imgDic.ElementAt(i);

                string appendPath = DateTime.Now.ToString("yy/MM/dd");
                string _tempSaveDirectory = Path.Combine(saveDirectory, appendPath);

                try
                {
                    if (!Directory.Exists(_tempSaveDirectory))
                    {
                        Directory.CreateDirectory(_tempSaveDirectory);
                    }

                    var newFileName = $"{img.Key}{Path.GetExtension(img.Value)}";

                    //保存路径，如：D:\\upload\\16\\10\\10\\12312.jpg
                    string savePath = Path.Combine(_tempSaveDirectory, newFileName);

                    //指定目录后追加的带文件名的路径
                    appendPath = Path.Combine(appendPath, newFileName);

                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";

                        Regex reg = new Regex(@"^(?<baseAddress>(https?://)[^/]+).+$", RegexOptions.IgnoreCase);

                        string baseAddress = reg.Match(img.Value).Groups["baseAddress"].Value;

                        if (!string.IsNullOrWhiteSpace(baseAddress))
                        {
                            client.BaseAddress = baseAddress;
                        }

                        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                        client.DownloadFile(img.Value, savePath);
                    }

                    //上传后的访问地址
                    string newVisitUrl = Path.Combine(visitUrl, appendPath);
                    newVisitUrl = newVisitUrl.Replace(@"\", "/");

                    newImgDic.Add(img.Key, newVisitUrl);
                }
                catch
                {
                    if (originalIfFailure)
                    {
                        newImgDic.Add(img.Key, img.Value);
                    }
                }
            }

            return newImgDic;
        }
    }
}
