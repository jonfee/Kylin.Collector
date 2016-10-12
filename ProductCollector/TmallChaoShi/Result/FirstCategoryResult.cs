﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.TmallChaoShi.Result
{
    public class FirstCategoryResult
    {
        public FirstCategoryItem[] Data { get; set; }

        public string Code { get; set; }

        public string Desc { get; set; }
    }

    public class FirstCategoryItem
    {
        public string Area { get; set; }

        public string Id { get; set; }

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

        public string Name { get; set; }
    }
}
