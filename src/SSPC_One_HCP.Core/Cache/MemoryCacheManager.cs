using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Cache
{
    public partial class MemoryCacheManager : ICacheManager
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)MemoryCache.Default[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="slidingExpiration"></param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(string key, T value, int? cacheTime = null, int? slidingExpiration = null)
        {
            if (value == null)
            {
                return;
            }
            var policy = new CacheItemPolicy();
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.UtcNow.AddDays(8) + TimeSpan.FromHours(cacheTime.Value);
            }
            if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = TimeSpan.FromHours(slidingExpiration.Value);
            }
            policy.Priority = CacheItemPriority.Default;
            MemoryCache.Default.Add(new CacheItem(key, value), policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return (MemoryCache.Default.Contains(key));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var item in MemoryCache.Default)
            {
                Remove(item.Key);
            }
        }
        #endregion
    }
}
