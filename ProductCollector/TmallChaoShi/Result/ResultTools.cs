using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.TmallChaoShi.Result
{
    public static class ResultTools
    {
        /// <summary>
        /// 从分类商品链接中获取分类ID
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static long GetCategoryByLink(this string link)
        {
            if (!string.IsNullOrWhiteSpace(link))
            {
                Uri uri = new Uri(link);
                var queryDic = (from q in uri.Query.TrimStart('?').Split('&')
                                select new
                                {
                                    Name = q.Split('=')[0],
                                    Value = q.Split('=')[1]
                                }).ToDictionary(k => k.Name, v => v.Value);

                long cateId = 0;

                if (queryDic != null && queryDic.ContainsKey(TmallChaoShiConfig.Instance.CategoryOption.CategoryIdQueryName))
                {
                    long.TryParse(queryDic[TmallChaoShiConfig.Instance.CategoryOption.CategoryIdQueryName], out cateId);
                }

                return cateId;
            }

            return 0;
        }

        /// <summary>
        /// 获取完整链接
        /// </summary>
        /// <param name="link"></param>
        /// <param name="starts"></param>
        /// <returns></returns>
        public static string GetFullLink(this string link, string starts = null)
        {
            if (string.IsNullOrWhiteSpace(starts)) starts = TmallChaoShiConfig.Instance.HttpProtocols;

            if (!string.IsNullOrWhiteSpace(link) && !link.StartsWith(starts))
            {
                link = starts + link.TrimStart('/');
            }

            return link;
        }
    }
}
