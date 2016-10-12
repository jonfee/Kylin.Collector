using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductCollector.TmallChaoShi
{
    public class TmallChaoShiConfig
    {
        private static TmallChaoShiConfig _instance;

        private static readonly object Locker = new object();

        public static TmallChaoShiConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new TmallChaoShiConfig();
                        }
                    }
                }

                return _instance;
            }
        }

        private TmallChaoShiConfig()
        {
            //加载采集器配置信息xml文档
            string xmlPath = Path.Combine(AppContext.BaseDirectory, "tmallchaoshi.xml");
            var xml = XElement.Load(xmlPath);

            //遍历配置项
            foreach (var c in xml.Elements())
            {
                switch (c.Name.ToString())
                {
                    case "category":
                        GetCategoryConfig(c);
                        break;

                }
            }
        }

        private void GetCategoryConfig(XElement em)
        {
            this.CategoryIdQueryName = em.Attribute("categoryIdQueryName").Value;

            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "first")
                {
                    GetFirstCategoryConfig(e);
                }
                else if (name == "child")
                {
                    GetChildCategoryConfig(e);
                }
            }
        }

        private void GetFirstCategoryConfig(XElement em)
        {
            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "url")
                {
                    this.FirstCategoriesDataUrl = e.Value;
                }
                else if (name == "pattern")
                {
                    this.FirstCategoriesPattern = e.Value;
                    this.FirstCategoriesDataGroupName = e.Attribute("dataGroup").Value;
                }
            }
        }

        private void GetChildCategoryConfig(XElement em)
        {
            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "url")
                {
                    this.ChildCategoriesDataUrl = e.Value;
                }
                else if (name == "pattern")
                {
                    this.ChildCategoriesPattern = e.Value;
                    this.ChildCategoriesDataGroupName = e.Attribute("dataGroup").Value;
                }
            }
        }

        #region 配置项

        /// <summary>
        /// 一级商品分类数据请求的Url
        /// </summary>
        public string FirstCategoriesDataUrl { get; set; }

        /// <summary>
        ///  一级商品分类数据的正则式
        /// </summary>
        public string FirstCategoriesPattern { get; set; }

        /// <summary>
        /// 一级商品分类数据的正则捕获分组名
        /// </summary>
        public string FirstCategoriesDataGroupName { get; set; }

        /// <summary>
        /// 子分类数据请求的Url模板
        /// </summary>
        public string ChildCategoriesDataUrl { get; set; }

        /// <summary>
        ///  子分类数据的正则式
        /// </summary>
        public string ChildCategoriesPattern { get; set; }

        /// <summary>
        /// 一子分类数据的正则捕获分组名
        /// </summary>
        public string ChildCategoriesDataGroupName { get; set; }

        /// <summary>
        /// 分类ID在链接地址中的参数名称
        /// </summary>
        public string CategoryIdQueryName { get; set; }

        #endregion
    }
}
