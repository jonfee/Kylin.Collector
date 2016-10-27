using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Td.Kylin.Collector.Entity
{
    [Table("Category")]
    public class Category
    {
        public long CategoryId { get; set; }
        public long SourceCategoryId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(300)")]
        public string Icon { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Layer { get; set; }
        public long ParentId { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Path { get; set; }
        public int Source { get; set; }
        public bool IsDelete { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
    }
}
