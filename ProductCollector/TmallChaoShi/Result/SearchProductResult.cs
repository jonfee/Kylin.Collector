using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.TmallChaoShi.Result
{
    /// <summary>
    /// 商品分类检索出的商品信息
    /// </summary>
    public class SearchProductResult
    {
        public long SiteCatId { get; set; }

        public long CategoryId { get; set; }

        public string Name { get; set; }

        private string _link;
        public string Link
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_link) && !_link.StartsWith("https://"))
                {
                    _link = "https://" + _link.TrimStart('/');
                }
                return _link;
            }
            set
            {
                _link = value;
            }
        }
    }
}
