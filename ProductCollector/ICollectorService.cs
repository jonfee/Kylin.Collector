using ProductCollector.BackState;
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
        /// 检测服务是否正在动行
        /// </summary>
        /// <returns></returns>
        bool IsRunning();

        /// <summary>
        /// 获取本地存储的商品分类
        /// </summary>
        /// <returns></returns>
        IEnumerable<TempCategory> GetLocationCategories();

        /// <summary>
        /// 采集服务开始
        /// </summary>
        /// <param name="categories">需要采集的分类</param>
        /// <param name="callback">回调</param>
        void Start(IEnumerable<TempCategory> categories, Action<CallBackState> callback);

        /// <summary>
        /// 保存分类采集记录
        /// </summary>
        void SaveCategoryCollectingRecord(CollectedCategory item);

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
        /// <param name="callback">回调</param>
        void Stop(Action<CallBackState> callback);
    }
}
