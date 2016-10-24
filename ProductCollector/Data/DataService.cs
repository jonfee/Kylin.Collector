using ProductCollector.Core;
using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Td.Kylin.Collector.Entity;

namespace ProductCollector.Data
{
    public class DataService
    {
        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<TempCategory> GetCatetories(int type)
        {
            using (var db = new DataContext())
            {
                var data = (from c in db.Category
                            where c.Source == type
                            select new TempCategory
                            {
                                SiteCatId = c.SourceCategoryId,
                                Layer = c.Layer,
                                Link = c.Path,
                                Name = c.Name,
                                CategoryId = c.CategoryId,
                                ParentId = c.ParentId
                            }).ToList();

                List<TempCategory> list = data.Where(p => p.ParentId == 0).ToList();

                var find = Tools.Fix<long, IEnumerable<TempCategory>, TempCategory[]>(f => (pid, sourceData) =>
                {
                    //获取子类
                    var child = sourceData.Where(p => p.ParentId == pid).ToArray();

                    foreach (var cat in child)
                    {
                        cat.Child = f(cat.CategoryId, sourceData);
                    }

                    return child;
                });

                foreach (var item in list)
                {
                    item.Child = find(item.CategoryId, data);
                }

                return list;
            }
        }

        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="data"></param>
        public void UpdateCategories(IEnumerable<TempCategory> data)
        {
            using (var db = new DataContext())
            {
                var update = Tools.Fix<TempCategory, int, bool>(up => (cat, deep) =>
                {
                    if (cat != null)
                    {
                        //数据库中是否存在
                        var item = db.Category.SingleOrDefault(p => p.CategoryId == cat.CategoryId);

                        if (item == null)
                        {
                            db.Add(new Category
                            {
                                CategoryId = cat.CategoryId,
                                SourceCategoryId = cat.SiteCatId,
                                CreateTime = DateTime.Now,
                                Icon = string.Empty,
                                IsDelete = false,
                                Layer = cat.Layer,
                                Name = cat.Name,
                                ParentId = cat.ParentId,
                                Path = cat.Link,
                                Source = cat.Type,
                                UpdateTime = DateTime.Now
                            });
                        }
                        else
                        {
                            item.Path = cat.Link;
                            item.Source = cat.Type;
                            item.UpdateTime = DateTime.Now;
                            db.Category.Attach(item);
                            db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }

                        if (cat.Child != null)
                        {
                            foreach (var c in cat.Child)
                            {
                                up(c, 0);
                            }
                        }
                    }

                    return true;
                });

                foreach (var cat in data)
                {
                    update(cat, 0);
                }

                db.SaveChanges();
            }
        }

    }
}
