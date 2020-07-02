/* ---------------------------------------------------------------------    
 * Copyright:
 * Wuxi ZoomSoft Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2016/3/16 10:08:06 
 *
 * ------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Singleton;

namespace SSPC_One_HCP.Core.ReflectionExtend
{
    /// <summary>
    /// 属性反射工厂
    /// </summary>
    public static class ReflectionFactory
    {
        /// <summary>
        /// 创建属性的访问接口
        /// </summary>
        /// <typeparam name="TEntity">实例类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="property">属性对象</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        /// <returns>创建的属性访问接口</returns>
        public static IPropertyAccessor<TFrom, TTo, TValue> Create<TFrom, TTo, TValue>(PropertyInfo getProperty, PropertyInfo setProperty, bool isCache)
            where TFrom : class
            where TTo : class
        {
            if (isCache)
            {

                var key = typeof(TFrom).Name + "_" + typeof(TTo).Name + getProperty.Name + "_" + setProperty.Name;
                IPropertyAccessor<TFrom, TTo, TValue> propertyAccessor = null;

                if (!SingletonManager<ConcurrentDictionary<string, IPropertyAccessor<TFrom, TTo, TValue>>>.Instance.TryGetValue(key, out propertyAccessor) || propertyAccessor == null)
                {
                    propertyAccessor = new PropertyAccessor<TFrom, TTo, TValue>(getProperty, setProperty);
                    SingletonManager<ConcurrentDictionary<string, IPropertyAccessor<TFrom, TTo, TValue>>>.Instance.TryAdd(key, propertyAccessor);
                }
                return propertyAccessor;
            }
            return new PropertyAccessor<TFrom, TTo, TValue>(getProperty, setProperty);
        }
    }
}
