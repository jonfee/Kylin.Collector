using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Td.Kylin.Collector.Entity
{
    [Table("ProductSku")]
    public class ProductSku
    {
        /// <summary>
        /// SKUID
        /// </summary>
        public long SkuID { get; set; }

        /// <summary>
        /// 源SKUID
        /// </summary>
        public long SourceSkuId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductID { get; set; }

        /// <summary>
        /// SKU名称
        /// </summary>
        [Column(TypeName ="nvarchar(500)")]
        public string Name { get; set; }

        /// <summary>
        /// 重量（kg)
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
    }
}
