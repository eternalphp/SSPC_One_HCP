using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Collection
{
    /// <summary>
    /// 列表接口扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 列表深度拷贝（相同对象）
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="sourceList">源列表</param>
        /// <param name="action">自定义赋值</param>
        /// <returns>深度拷贝结果</returns>
        public static List<T> DeepCopy<T>(this IEnumerable<T> sourceList, Action<T> action = null)
            where T : class, new()
        {
            var result = new List<T>();

            if (sourceList.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var item in sourceList)
            {
                var addItem = item.CopyItem(action);
                result.Add(addItem);
            }

            return result;
        }

        /// <summary>
        /// 列表深度拷贝（不同对象）
        /// </summary>
        /// <typeparam name="TFrom">源对象类型</typeparam>
        /// <typeparam name="TTo">目标对象类型</typeparam>
        /// <param name="sourceList">源列表</param>
        /// <param name="action">自定义赋值</param>
        /// <returns>深度拷贝结果</returns>
        public static List<TTo> DeepCopy<TFrom, TTo>(this IEnumerable<TFrom> sourceList, Action<TFrom, TTo> action = null)
            where TFrom : class, new()
            where TTo : class, new()
        {
            var result = new List<TTo>();
            if (sourceList.IsNullOrEmpty())
            {
                return result;
            }
            foreach (var item in sourceList)
            {
                var addItem = item.CopyItem<TFrom, TTo>(action);
                result.Add(addItem);
            }
            return result;
        }

        /// <summary>
        /// 判断列表是否为null或者空
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">判断源</param>
        /// <returns>判断结果</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        /// <summary>
        /// 返回列表默认值
        /// </summary>
        /// <typeparam name="TEntity">泛型类型</typeparam>
        /// <param name="source">源列表</param>
        /// <param name="defaultItem">默认值</param>
        /// <returns>列表的第一个对象</returns>
        public static TEntity FirstOrDefaultWithNullList<TEntity>(this IEnumerable<TEntity> source, TEntity defaultItem = default(TEntity))
        {
            if (!source.IsNullOrEmpty())
            {
                return source.FirstOrDefault();
            }

            return defaultItem;
        }

    }
}
