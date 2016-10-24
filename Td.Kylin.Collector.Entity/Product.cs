using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Td.Kylin.Collector.Entity
{
    [Table("Product")]
   public class Product
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductID { get; set; }

        /// <summary>
        /// 源商品ID
        /// </summary>
        public long SourceProductID { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public long CategoryID { get; set; }

        /// <summary>
        /// 品牌ID
        /// </summary>
        public long BrandID { get; set; }

        /// <summary>
        /// 商品标题
        /// </summary>
        [Column(TypeName ="nvarchar(500)")]
        public string Title { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        [Column(TypeName ="varchar(200)")]
        public string mainPic { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Column(TypeName ="varchar(1000)")]
        public string Pics { get; set; }

        /// <summary>
        /// 重量（kg)
        /// </summary>
        public float Weight { get; set; }
        
        /// <summary>
        /// 商品来源
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 商品详情源地址
        /// </summary>
        [Column(TypeName = "varchar(500)")]
        public string Path { get; set; }

        /// <summary>
        /// 属性（JSON格式）
        /// </summary>
        [Column(TypeName = "nvarchar(2000)")]
        public string Properties { get; set; }

        /// <summary>
        /// 详情描述
        /// </summary>
        [Column(TypeName = "text")]
        public string Intro { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
