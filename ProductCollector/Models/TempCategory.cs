using ProductCollector.TmallChaoShi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.Models
{
    public class TempCategory
    {
        /// <summary>
        /// 分类ID（采集站的分类ID）
        /// </summary>
        public long SiteCatId { get; set; }

        /// <summary>
        /// 采集站类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 分类名称（采集站的分类名称）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前分类下的商品地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 采集库中分类ID
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 采集库中的父分类ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 分类层级（采集库中分类ID）
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// 下级分类
        /// </summary>
        public TempCategory[] Child { get; set; }
    }
}
