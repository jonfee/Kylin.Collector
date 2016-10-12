using ProductCollector.Data.Entity;
using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                SiteCatId = 0,
                                Layer = c.Layer,
                                Link = c.Path,
                                Name = c.Name,
                                CategoryId = c.CategoryID,
                                ParentId = c.ParentID
                            }).ToList();

                List<TempCategory> list = data.Where(p => p.ParentId == 0).ToList();

                var find = Fix<long, IEnumerable<TempCategory>, TempCategory[]>(f => (pid, sourceData) =>
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
                var update = Fix<TempCategory, int, bool>(up => (cat, deep) =>
                {
                    if (cat != null)
                    {
                        //数据库中是否存在
                        var item = db.Category.SingleOrDefault(p => p.CategoryID == cat.CategoryId);

                        if (item == null)
                        {
                            db.Add(new Category
                            {
                                CategoryID = cat.CategoryId,
                                CreateTime = DateTime.Now,
                                Icon = string.Empty,
                                IsDelete = false,
                                Layer = cat.Layer,
                                Name = cat.Name,
                                ParentID = cat.ParentId,
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

        /// <summary>
        /// 不动点算子函数
        /// </summary>
        /// <typeparam name="T1">传入参数类型</typeparam>
        /// <typeparam name="T2">传入参数类型</typeparam>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        Func<T1, T2, TResult> Fix<T1, T2, TResult>(Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> g)
        {
            return (x, y) => g(Fix(g))(x, y);
        }
    }
}
