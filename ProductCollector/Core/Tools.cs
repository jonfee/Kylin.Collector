using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Td.AspNet.Utils;

namespace ProductCollector.Core
{
    public class Tools
    {
        /// <summary>
        /// 生成一个新的ID
        /// </summary>
        /// <returns></returns>
        public static long NewId()
        {
            return (long)IDCreater.NewId(1, 1);
        }

        #region 不动点求子函数
        public static Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> g)
        {
            return (x) => g(Fix(g))(x);
        }
        public static Func<T1, T2, TResult> Fix<T1, T2, TResult>(Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> g)
        {
            return (x, y) => g(Fix(g))(x, y);
        }
        public static Func<T1, T2, T3, TResult> Fix<T1, T2, T3, TResult>(Func<Func<T1, T2, T3, TResult>, Func<T1, T2, T3, TResult>> g)
        {
            return (x, y, z) => g(Fix(g))(x, y, z);
        }
        #endregion

        /// <summary>
        /// 忽略HTML标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IgnoreHtmlTag(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            Regex tagRegex = new Regex(@"</?[^>]+>", RegexOptions.IgnoreCase);

            return tagRegex.Replace(str, "");
        }
    }
}
