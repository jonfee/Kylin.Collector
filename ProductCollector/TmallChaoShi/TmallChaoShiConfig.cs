using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            this.DetailsOption = new DetailsOption();

            this.UploadOption = new UploadOption();

            //加载采集器配置信息xml文档
            string xmlPath = Path.Combine(AppContext.BaseDirectory, "tmallchaoshi.xml");
            var xml = XElement.Load(xmlPath);

            //遍历配置项
            foreach (var c in xml.Elements())
            {
                switch (c.Name.ToString())
                {
                    case "protocols":
                        this.HttpProtocols = c.Value;
                        break;
                    case "category":
                        GetCategoryConfig(c);
                        break;
                    case "details":
                        GetProductDetailsConfig(c);
                        break;
                    case "upload":
                        GetUploadConfig(c);
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
                    this.CategoryOption.FirstCategoriesRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("dataGroup").Value
                    };
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
                    this.CategoryOption.ChildCategoriesRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("dataGroup").Value
                    };
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
                    this.SearchOption.NextPageRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("dataGroup").Value
                    };
                }
                else if (name == "productpattern")
                {
                    this.SearchOption.ItemRegex = new SearchItemRegexPattern
                    {
                        Pattern = e.Value,
                        LinkGroupName = e.Attribute("linkGroup").Value,
                        TitleGroupName = e.Attribute("titleGroup").Value
                    };
                }
            }
        }

        /// <summary>
        /// 获取商品详情数据时的相关配置
        /// </summary>
        /// <param name="em"></param>
        private void GetProductDetailsConfig(XElement em)
        {
            string charset = em.Attribute("charset").Value;
            if (string.IsNullOrWhiteSpace(charset)) charset = "utf-8";

            this.DetailsOption.Encoding = Encoding.GetEncoding(charset);

            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "setuppattern")
                {
                    this.DetailsOption.SetupRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("group").Value
                    };
                }
                else if (name == "imagespattern")
                {
                    this.DetailsOption.ImagesDataRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("group").Value
                    };
                }
                else if (name == "singleimagepattern")
                {
                    this.DetailsOption.SingleImageRegex = new TmallChaoShi.DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("group").Value
                    };
                }
                else if (name == "imageurlremovepattern")
                {
                    this.DetailsOption.ImageSrcRemoveRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value
                    };
                }
                else if (name == "descpattern")
                {
                    this.DetailsOption.DescRegex = new DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("group").Value
                    };
                }
                else if (name == "descimgpattern")
                {
                    this.DetailsOption.DescImageRegex = new TmallChaoShi.DefaultRegexPattern
                    {
                        Pattern = e.Value,
                        GroupName = e.Attribute("group").Value
                    };
                }
            }
        }

        /// <summary>
        /// 获取文件上传相关配置
        /// </summary>
        /// <param name="em"></param>
        private void GetUploadConfig(XElement em)
        {
            foreach (var e in em.Elements())
            {
                string name = e.Name.ToString().ToLower();

                if (name == "visitaddress")
                {
                    this.UploadOption.VisitAddress = e.Value;
                }
                else if (name == "savedirectory")
                {
                    this.UploadOption.SaveDirectory = e.Value;
                }
            }
        }

        #region 配置项

        /// <summary>
        /// http协议头
        /// </summary>
        public string HttpProtocols { get; set; }

        /// <summary>
        /// 抓取分类相关配置项
        /// </summary>
        public CategoryOption CategoryOption { get; set; }

        /// <summary>
        /// 获取分类下的商品列表配置项
        /// </summary>
        public SearchOption SearchOption { get; set; }

        /// <summary>
        /// 获取商品详情数据配置项
        /// </summary>
        public DetailsOption DetailsOption { get; set; }

        /// <summary>
        /// 文件上传配置项
        /// </summary>
        public UploadOption UploadOption { get; set; }

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
        public DefaultRegexPattern FirstCategoriesRegex { get; set; }

        /// <summary>
        /// 子分类数据请求的Url模板
        /// </summary>
        public string ChildCategoriesDataUrl { get; set; }

        /// <summary>
        ///  子分类数据的正则式
        /// </summary>
        public DefaultRegexPattern ChildCategoriesRegex { get; set; }

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
        public DefaultRegexPattern NextPageRegex { get; set; }

        /// <summary>
        /// 搜索列表中单项正则式
        /// </summary>
        public SearchItemRegexPattern ItemRegex { get; set; }

        /// <summary>
        /// 最多获取页数
        /// </summary>
        public int MaxSearchPages { get; set; }
    }

    /// <summary>
    /// 商品详情配置
    /// </summary>
    public class DetailsOption
    {
        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 商品装载信息正则式
        /// </summary>
        public DefaultRegexPattern SetupRegex { get; set; }

        /// <summary>
        /// 商品图片区正则式
        /// </summary>
        public DefaultRegexPattern ImagesDataRegex { get; set; }

        /// <summary>
        /// 单个图片地址正则式
        /// </summary>
        public DefaultRegexPattern SingleImageRegex { get; set; }

        /// <summary>
        /// 图片地址移除部分正则式
        /// </summary>
        public DefaultRegexPattern ImageSrcRemoveRegex { get; set; }

        /// <summary>
        /// 详情描述正则式
        /// </summary>
        public DefaultRegexPattern DescRegex { get; set; }

        /// <summary>
        /// 详情描述中的图片正则式
        /// </summary>
        public DefaultRegexPattern DescImageRegex { get; set; }
    }

    /// <summary>
    /// 文件上传配置
    /// </summary>
    public class UploadOption
    {
        public string VisitAddress { get; set; }

        public string SaveDirectory { get; set; }
    }

    /// <summary>
    /// 正则式基类
    /// </summary>
    public abstract class BaseRegexPattern
    {
        public string Pattern { get; set; }
    }

    /// <summary>
    /// 默认正则规则
    /// </summary>
    public class DefaultRegexPattern : BaseRegexPattern
    {
        public string GroupName { get; set; }
    }

    /// <summary>
    /// 搜索列表中单个商品项正则规则
    /// </summary>
    public class SearchItemRegexPattern : BaseRegexPattern
    {
        public string TitleGroupName { get; set; }

        public string LinkGroupName { get; set; }
    }

    #endregion
}
