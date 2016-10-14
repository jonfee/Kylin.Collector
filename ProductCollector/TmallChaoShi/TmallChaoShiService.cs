using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCollector.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProductCollector.TmallChaoShi.Result;
using ProductCollector.Core;
using ProductCollector.Data;
using ProductCollector.BackState;
using System.Threading;

namespace ProductCollector.TmallChaoShi
{
    /// <summary>
    /// 天猫超市数据采集服务
    /// </summary>
    public class TmallChaoShiService : ICollectorService, IDisposable
    {
        SearchOption searchOption = TmallChaoShiConfig.Instance.SearchOption;

        CategoryOption categoryOption = TmallChaoShiConfig.Instance.CategoryOption;

        /// <summary>
        /// 采集类型
        /// </summary>
        const int collectorType = 1;

        /// <summary>
        /// 是否已经被释放过,默认是false
        /// </summary>
        bool m_disposed;

        /// <summary>
        /// 需要采集的分类
        /// </summary>
        IEnumerable<TempCategory> needCollectCategories;

        /// <summary>
        /// 采集任务
        /// </summary>
        Task collectorTask;

        /// <summary>
        /// 队列-需要采集的商品
        /// </summary>
        volatile Queue<SearchProductResult> collectorQueue;

        /// <summary>
        /// 是否运行中
        /// </summary>
        bool isRunning = false;

        public IEnumerable<TempCategory> UpdateCategories()
        {
            List<TempCategory> categories = new List<TempCategory>();

            //本地数据库存储的商品分类
            var locationCategories = GetLocationCategories();

            //一级分类
            var first = GetFirstCategoryResult();

            foreach (var cat in first.Data)
            {
                //本地数据库中副本
                var dbItem = locationCategories.FirstOrDefault(p => p.Name.Equals(cat.Name, StringComparison.OrdinalIgnoreCase));

                long newCatId = dbItem != null ? dbItem.CategoryId : Tools.NewId();

                TempCategory category = new TempCategory
                {
                    Link = cat.Link,
                    SiteCatId = cat.CatId,
                    Layer = newCatId.ToString(),
                    Name = cat.Name,
                    CategoryId = newCatId,
                    ParentId = 0,
                    Type = collectorType
                };

                var childUrl = categoryOption.ChildCategoriesDataUrl.Replace("#wh_id#", cat.Id);

                var childData = GetChildCategoryResult(childUrl);

                if (childData != null && childData.Data != null)
                {
                    var child = GetChildTempCategoryList(category, childData.Data.Cats, locationCategories);

                    category.Child = child.ToArray();
                }

                categories.Add(category);
            }

            //更新本地商品分类
            new DataService().UpdateCategories(categories);
            
            return categories;
        }

        public IEnumerable<TempCategory> GetLocationCategories()
        {
            return new DataService().GetCatetories(collectorType);
        }

        public IEnumerable<CollectedCategory> GetCollectedCategories()
        {
            return null;
        }

        public void Start(IEnumerable<TempCategory> categories)
        {
            this.needCollectCategories = categories ?? new List<TempCategory>();

            if (needCollectCategories.Any())
            {
                isRunning = true;

                Writer.writeInvoke(new MessageState { Text = "天猫超市数据采集开始……", PadTime = true });
                Writer.writeInvoke(new MessageState { Text = "数据正在分析……", PadTime = true });
                Thread.Sleep(1000);
                //分析数据
                DataAnalyzing();
                //输出统计结果
                Writer.writeInvoke(new StatisticsState { TotalProducts = collectorQueue.Count() });
                Writer.writeInvoke(new MessageState { Text = $"分析完成，结果：共有{collectorQueue.Count()}条商品数据待抓取。", PadTime = true });

                Thread.Sleep(1000);
                Writer.writeInvoke(new MessageState { Text = "商品采集开始……", PadTime = true });

                //采集工作开始
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    CollectorWork();
                }, null);
            }
            else
            {
                Writer.writeInvoke(new MessageState { Text = "没有可采集的分类！", PadTime = true });
            }
        }

        public void Stop()
        {
            Dispose(true);
            isRunning = false;
            Writer.writeInvoke(new MessageState
            {
                Text = "天猫超市数据采集服务停止！",
                PadTime = true
            });
        }

        /// <summary>
        /// 采集工作开始
        /// </summary>
        void CollectorWork()
        {
            int max = 100;// collectorQueue.Count();
            int collectedNum = 0;

            while (collectorQueue.Any())
            {
                var searchRst = collectorQueue.Any() ? collectorQueue.Dequeue() : null;

                if (searchRst != null)
                {
                    Writer.writeInvoke(new MessageState { Text = $"开始抓取“{searchRst.Name}”……" });

                    getProduct(searchRst);

                    Thread.Sleep(1000);
                }

                Writer.writeInvoke(new ProgressState { Max = max, Value = ++collectedNum });
                Writer.writeInvoke(new StatisticsState { TotalProducts = max, FinishProducts = collectedNum });

                Thread.Sleep(1000);
            }

            Writer.writeInvoke(new MessageState { Text = $"所有商品抓取完成！" });
        }

        /// <summary>
        /// 抓取单个商品
        /// </summary>
        /// <param name="searchRst"></param>
        /// <returns></returns>
        void getProduct(SearchProductResult searchRst)
        {
            //TODO 抓取商品数据

            //TODO 解析成产品库数据

            //TODO 保存到数据库
        }

        public void SaveCategoryCollectingRecord(CollectedCategory item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检测服务是否正在动行
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return isRunning;
        }

        #region 分析要抓取的商品信息

        /// <summary>
        /// 分析要抓取的商品并写入队列
        /// </summary>
        private void DataAnalyzing()
        {
            collectorQueue = new Queue<SearchProductResult>();

            //已抓取的页数
            int pageNum = 0;

            //下一页处理
            var next = Tools.Fix<TempCategory, string, string>(f => (cat, url) =>
            {
                Thread.Sleep(1000);

                string nextLink = null;

                //是否继续抓取
                bool isContinue = (searchOption.MaxSearchPages < 0 || pageNum < searchOption.MaxSearchPages) && !string.IsNullOrWhiteSpace(url);

                if (isContinue)
                {
                    //获取默认商品列表页HTML
                    string searchHtml = WebClientHelper.GetContent(url, searchOption.Encoding, searchOption.CookieString);

                    //获取下一页链接
                    Regex nextPageRegex = new Regex(searchOption.NextPagePattern, RegexOptions.IgnoreCase);

                    //匹配下一页html
                    Match nextMatch = nextPageRegex.Match(searchHtml);

                    nextLink = nextMatch.Groups[searchOption.NextPageLinkGroupName].Value;

                    if (nextLink.StartsWith("?"))
                    {
                        nextLink = url.Split('?')[0] + nextLink;
                    }

                    //捕获当前页所有商品链接
                    Regex productRegex = new Regex(searchOption.ProductItemPattern, RegexOptions.IgnoreCase);

                    //匹配所有的商品区html
                    MatchCollection proMatchs = productRegex.Matches(searchHtml);

                    //将匹配到要抓取的商品信息加入队列
                    foreach (Match m in proMatchs)
                    {
                        collectorQueue.Enqueue(new SearchProductResult
                        {
                            SiteCatId = cat.SiteCatId,
                            CategoryId = cat.CategoryId,
                            Name = m.Groups[searchOption.ProductTitleGroupName].Value,
                            Link = m.Groups[searchOption.ProductItemLinkGroupName].Value
                        });
                    }

                    pageNum++;//抓取的页数+1

                    //存在下一页，则继续下一页数据抓取
                    if (!string.IsNullOrWhiteSpace(nextLink))
                    {
                        nextLink = f(cat, nextLink);
                    }
                }

                return nextLink;
            });

            int catIndex = 0;
            //遍历采集的分类
            foreach (var cat in needCollectCategories)
            {
                pageNum = 0;

                Writer.writeInvoke(new MessageState { Text = $"分析“{cat.Name}”下的商品……" });

                next(cat, cat.Link);

                Writer.writeInvoke(new ProgressState { Max = needCollectCategories.Count(), Value = ++catIndex });

                Writer.writeInvoke(new MessageState { Text = $"“{cat.Name}”下的商品分析完成！" });
            }
        }

        #endregion

        #region 释放资源

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    if (collectorTask != null) collectorTask.Dispose();
                }

                m_disposed = true;
            }
        }

        ~TmallChaoShiService()
        {
            Dispose();
        }

        #endregion

        #region 抓取分类

        /// <summary>
        /// 获取一级商品分类结果
        /// </summary>
        /// <returns></returns>
        private FirstCategoryResult GetFirstCategoryResult()
        {
            string firstDataString = WebClientHelper.GetContent(categoryOption.FirstCategoriesDataUrl, Encoding.UTF8);

            Regex reg = new Regex(categoryOption.FirstCategoriesPattern, RegexOptions.IgnoreCase);

            Match m = reg.Match(firstDataString);

            if (m != null)
            {
                string data = m.Groups[categoryOption.FirstCategoriesDataGroupName].Value;

                return JsonConvert.DeserializeObject<FirstCategoryResult>(data);
            }

            return null;
        }

        /// <summary>
        /// 获取子分类结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private ChildCategoryResult GetChildCategoryResult(string url)
        {
            string childDataString = WebClientHelper.GetContent(url, Encoding.UTF8);

            Regex reg = new Regex(categoryOption.ChildCategoriesPattern, RegexOptions.IgnoreCase);

            Match m = reg.Match(childDataString);

            if (m != null)
            {
                string data = m.Groups[categoryOption.ChildCategoriesDataGroupName].Value;

                return JsonConvert.DeserializeObject<ChildCategoryResult>(data);
            }

            return null;
        }

        /// <summary>
        /// 获取一级分类下的子分类（获取二级）
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="cats"></param>
        /// <param name="location">本地存储的分类数据</param>
        /// <returns></returns>
        private List<TempCategory> GetChildTempCategoryList(TempCategory parent, ChildCategoryItem[] cats, IEnumerable<TempCategory> location)
        {
            if (cats == null) return null;

            List<TempCategory> list = new List<TempCategory>();

            foreach (var cat in cats)
            {
                //本地数据库中副本
                var dbItem = location.FirstOrDefault(p => p.Name.Equals(cat.Title, StringComparison.OrdinalIgnoreCase));

                long newCatId = dbItem != null ? dbItem.CategoryId : Tools.NewId();

                TempCategory category = new TempCategory
                {
                    Link = cat.Link,
                    SiteCatId = cat.CatId,
                    Layer = parent.Layer + "," + newCatId.ToString(),
                    Name = cat.Title,
                    CategoryId = newCatId,
                    ParentId = parent.ParentId,
                    Type = collectorType
                };

                var child = GetChildTempCategoryList(category, cat.Cats, location);

                category.Child = child.ToArray();

                list.Add(category);
            }

            return list;
        }

        /// <summary>
        /// 获取二级分类下的子分类（获取三级）
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="cats"></param>
        /// <returns></returns>
        private List<TempCategory> GetChildTempCategoryList(TempCategory parent, CategoryItem[] cats, IEnumerable<TempCategory> location)
        {
            if (cats == null) return null;

            List<TempCategory> list = new List<TempCategory>();

            foreach (var cat in cats)
            {
                //本地数据库中副本
                var dbItem = location.FirstOrDefault(p => p.Name.Equals(cat.Text, StringComparison.OrdinalIgnoreCase));

                long newCatId = dbItem != null ? dbItem.CategoryId : Tools.NewId();

                TempCategory category = new TempCategory
                {
                    Link = cat.Link,
                    SiteCatId = cat.CatId,
                    Layer = parent.Layer + "," + newCatId.ToString(),
                    Name = cat.Text,
                    CategoryId = newCatId,
                    ParentId = parent.ParentId,
                    Type = collectorType
                };

                list.Add(category);
            }

            return list;
        }

        #endregion
    }
}
