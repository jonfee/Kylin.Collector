using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector
{
    public interface ICollectorService
    {
        /// <summary>
        /// 获取本地存储的商品分类
        /// </summary>
        /// <returns></returns>
        IEnumerable<TempCategory> GetLocationCategories();

        /// <summary>
        /// 采集服务开始
        /// </summary>
        void Start();

        /// <summary>
        /// 下载最新商品分类信息
        /// </summary>
        IEnumerable<TempCategory> UpdateCategories();

        /// <summary>
        /// 获取已采集过的商品分类信息
        /// </summary>
        IEnumerable<CollectedCategory> GetCollectedCategories();

        /// <summary>
        /// 采集服务停止
        /// </summary>
        void Stop();
    }
}
