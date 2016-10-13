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
            this.CategoryOption = new CategoryOption();

            this.SearchOption = new SearchOption();

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
            this.CategoryOption.CategoryIdQueryName = em.Attribute("categoryIdQueryName").Value;

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
                else if (name == "search")
                {
                    GetSearchProductConfig(e);
                }
            }
        }

        /// <summary>
        /// 一级分类相关配置
        /// </summary>
        /// <param name="em"></param>
        private void GetFirstCategoryConfig(XElement em)
        {
            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "url")
                {
                    this.CategoryOption.FirstCategoriesDataUrl = e.Value;
                }
                else if (name == "pattern")
                {
                    this.CategoryOption.FirstCategoriesPattern = e.Value;
                    this.CategoryOption.FirstCategoriesDataGroupName = e.Attribute("dataGroup").Value;
                }
            }
        }

        /// <summary>
        /// 子分类相关配置
        /// </summary>
        /// <param name="em"></param>
        private void GetChildCategoryConfig(XElement em)
        {
            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "url")
                {
                    this.CategoryOption.ChildCategoriesDataUrl = e.Value;
                }
                else if (name == "pattern")
                {
                    this.CategoryOption.ChildCategoriesPattern = e.Value;
                    this.CategoryOption.ChildCategoriesDataGroupName = e.Attribute("dataGroup").Value;
                }
            }
        }

        /// <summary>
        /// 从分类搜索商品时的相关配置
        /// </summary>
        /// <param name="em"></param>
        private void GetSearchProductConfig(XElement em)
        {
            string charset = em.Attribute("charset").Value;
            if (string.IsNullOrWhiteSpace(charset)) charset = "utf-8";

            this.SearchOption.Encoding = Encoding.GetEncoding(charset);
            this.SearchOption.MaxSearchPages = int.Parse(em.Attribute("maxPages").Value);

            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "cookie")
                {
                    this.SearchOption.CookieString = e.Value;
                }
                else if (name == "nextpattern")
                {
                    this.SearchOption.NextPagePattern = e.Value;
                    this.SearchOption.NextPageLinkGroupName = e.Attribute("dataGroup").Value;
                }
                else if (name == "productpattern")
                {
                    this.SearchOption.ProductItemPattern = e.Value;
                    this.SearchOption.ProductTitleGroupName = e.Attribute("titleGroup").Value;
                    this.SearchOption.ProductItemLinkGroupName = e.Attribute("linkGroup").Value;
                }
            }
        }

        #region 配置项

        /// <summary>
        /// 抓取分类相关配置项
        /// </summary>
        public CategoryOption CategoryOption { get; set; }

        /// <summary>
        /// 获取分类下的商品列表配置项
        /// </summary>
        public SearchOption SearchOption { get; set; }


        #endregion
    }

    #region 配置类
    /// <summary>
    /// 分类相关配置
    /// </summary>
    public class CategoryOption
    {
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
        /// 子分类数据的正则捕获分组名
        /// </summary>
        public string ChildCategoriesDataGroupName { get; set; }

        /// <summary>
        /// 分类ID在链接地址中的参数名称
        /// </summary>
        public string CategoryIdQueryName { get; set; }
    }

    /// <summary>
    /// 分类搜索商品配置
    /// </summary>
    public class SearchOption
    {
        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        public string CookieString { get; set; }

        /// <summary>
        /// 下一页正则式
        /// </summary>
        public string NextPagePattern { get; set; }

        /// <summary>
        /// 下一页链接的正则捕获分组名
        /// </summary>
        public string NextPageLinkGroupName { get; set; }

        /// <summary>
        /// 列表页中单个商品正则式
        /// </summary>
        public string ProductItemPattern { get; set; }

        /// <summary>
        /// 列表页中单个商品链接正则捕获分组名
        /// </summary>
        public string ProductItemLinkGroupName { get; set; }

        /// <summary>
        /// 列表页中单个商品标题正则捕获分组名
        /// </summary>
        public string ProductTitleGroupName { get; set; }

        /// <summary>
        /// 最多获取页数
        /// </summary>
        public int MaxSearchPages { get; set; }
    }
    #endregion
}
