using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// 根据Key获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">key值</param>
        /// <returns>返回结果</returns>
        T Get<T>(string key);

        /// <summary>
        /// 设定缓存值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">key值</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(小时)</param>
        /// <param name="slidingExpiration">在指定(小时)内未被使用则移除缓存</param>
        void Set<T>(string key, T value, int? cacheTime = null, int? slidingExpiration = null);

        /// <summary>
        /// 判断key是否已经设定
        /// </summary>
        /// <param name="key">key值</param>
        /// <returns>判断结果</returns>
        bool IsSet(string key);

        /// <summary>
        /// 根据key移除缓存值
        /// </summary>
        /// <param name="key">key值</param>
        void Remove(string key);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();
    }
}
