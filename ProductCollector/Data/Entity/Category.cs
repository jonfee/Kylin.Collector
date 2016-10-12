using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.Data.Entity
{
    [Table("Category")]
    public class Category
    {
        public long CategoryID { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Layer { get; set; }
        public long ParentID { get; set; }
        public string Path { get; set; }
        public int Source { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
