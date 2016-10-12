using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCollector.Models;
using ProductCollector.Collector;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProductCollector.TmallChaoShi.Result;
using ProductCollector.Core;
using ProductCollector.Data;

namespace ProductCollector.TmallChaoShi
{
    /// <summary>
    /// 天猫超市数据采集服务
    /// </summary>
    public class TmallChaoShiService : ICollectorService
    {
        const int collectorType = 1;

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

                var childUrl = TmallChaoShiConfig.Instance.ChildCategoriesDataUrl.Replace("#wh_id#", cat.Id);

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

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #region 

        /// <summary>
        /// 获取一级商品分类结果
        /// </summary>
        /// <returns></returns>
        private FirstCategoryResult GetFirstCategoryResult()
        {
            string firstDataString = CollectorFactory.GetContent(TmallChaoShiConfig.Instance.FirstCategoriesDataUrl);

            Regex reg = new Regex(TmallChaoShiConfig.Instance.FirstCategoriesPattern, RegexOptions.IgnoreCase);

            Match m = reg.Match(firstDataString);

            if (m != null)
            {
                string data = m.Groups[TmallChaoShiConfig.Instance.FirstCategoriesDataGroupName].Value;

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
            string childDataString = CollectorFactory.GetContent(url);

            Regex reg = new Regex(TmallChaoShiConfig.Instance.ChildCategoriesPattern, RegexOptions.IgnoreCase);

            Match m = reg.Match(childDataString);

            if (m != null)
            {
                string data = m.Groups[TmallChaoShiConfig.Instance.ChildCategoriesDataGroupName].Value;

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
