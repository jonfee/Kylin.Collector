using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.TmallChaoShi.Result
{
    public class ChildCategoryResult
    {
        public ChildCategoryData Data { get; set; }

        public string Code { get; set; }

        public string Desc { get; set; }
    }

    public class ChildCategoryData
    {
        public ChildCategoryItem[] Cats { get; set; }
    }

    public class ChildCategoryItem
    {
        public CategoryItem[] Cats { get; set; }

        public string Title { get; set; }

        public long CatId
        {
            get
            {
                return ResultTools.GetCategoryByLink(Link);
            }
        }

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

    public class CategoryItem
    {
        public string Text { get; set; }

        public long CatId
        {
            get
            {
                return ResultTools.GetCategoryByLink(Link);
            }
        }

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
