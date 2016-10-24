using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Td.Kylin.Collector.Entity
{
    [Table("Product")]
   public class Product
    {
        public long ProductID { get; set; }

        public long CategoryID { get; set; }

        public long BrandID { get; set; }

        [Column(TypeName ="nvarchar(500)")]
        public string Title { get; set; }

        [Column(TypeName ="varchar(200)")]
        public string mainPic { get; set; }

        [Column(TypeName ="varchar(1000)")]
        public string Pics { get; set; }

        public float Weight { get; set; }

        public int Source { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Path { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string Properties { get; set; }

        [Column(TypeName = "text")]
        public string Intro { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
