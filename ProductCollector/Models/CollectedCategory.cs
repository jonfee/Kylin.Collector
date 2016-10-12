using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.Models
{
    /// <summary>
    /// 已采集过的商品分类
    /// </summary>
    public class CollectedCategory
    {
        /// <summary>
        /// 分类ID（采集站的分类ID）
        /// </summary>
        public long CatId { get; set; }

        /// <summary>
        /// 分类名称（采集站的分类名称）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最后采集时间
        /// </summary>
        public DateTime LastCollectTime { get; set; }

        /// <summary>
        /// 最后采集的商品ID（采集站的商品ID）
        /// </summary>
        public long LastCollectProductId { get; set; }
    }
}
