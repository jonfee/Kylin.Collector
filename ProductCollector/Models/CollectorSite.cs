using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.Models
{
    /// <summary>
    /// 采集站点
    /// </summary>
    public class CollectorSite
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 站点地址
        /// </summary>
        public string Url { get; set; }
    }
}
