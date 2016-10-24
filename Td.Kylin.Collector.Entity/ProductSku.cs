using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Td.Kylin.Collector.Entity
{
    [Table("ProductSku")]
    public class ProductSku
    {
        public long SkuID { get; set; }

        public long ProductID { get; set; }

        [Column(TypeName ="nvarchar(500)")]
        public string Name { get; set; }

        public decimal SalePrice { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
