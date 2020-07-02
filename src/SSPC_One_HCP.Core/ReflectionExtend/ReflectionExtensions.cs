/* ---------------------------------------------------------------------    
 * Copyright:
 * 
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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.ReflectionExtend;
using SSPC_One_HCP.Core.Singleton;

namespace SSPC_One_HCP.Core.ReflectionExtend
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 设定属性值的扩展
        /// </summary>
        /// <typeparam name="TEntity">实例的类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="property">需要设定的属性</param>
        /// <param name="instance">实例对象</param>
        /// <param name="value">需要设定的值</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        public static void SetPropertyValue<TEntity, TValue>(this PropertyInfo property, TEntity instance,
            TValue value, bool isCache = true)
            where TEntity : class
        {
            var propertyInfo = ReflectionFactory.Create<TEntity, TEntity, TValue>(property, property, isCache);

            propertyInfo.SetValue(instance, value);
        }

        /// <summary>
        /// 获取属性值的扩展
        /// </summary>
        /// <typeparam name="TEntity">实例的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="property">需要获取的属性</param>
        /// <param name="instance">实例对象</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        /// <returns>获取的值</returns>
        public static TValue GetPropertyValue<TEntity, TValue>(this PropertyInfo property, TEntity instance, bool isCache = true)
             where TEntity : class
        {
            var propertyInfo = ReflectionFactory.Create<TEntity, TEntity, TValue>(property, property, isCache);

            return propertyInfo.GetValue(instance);
        }

        /// <summary>
        /// 获取值再设定值
        /// </summary>
        /// <typeparam name="TFrom">源实例的类型</typeparam>
        /// <typeparam name="TTo">目标实例的类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="property">需要设定的属性</param>
        /// <param name="fromEntity">源实例</param>
        /// <param name="toEntity">目标实例</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        public static void GetAndSetProperty<TFrom, TTo, TValue>(this TFrom fromEntity, TTo toEntity, PropertyInfo getProperty, PropertyInfo setProperty,
             bool isCache)
            where TFrom : class
            where TTo : class
        {
            var propertyAccessor = ReflectionFactory.Create<TFrom, TTo, TValue>(getProperty, setProperty, isCache);
            propertyAccessor.SetValue(toEntity, propertyAccessor.GetValue(fromEntity));
        }

        /// <summary>
        /// 获取特定特性名称
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetAttributeName(this PropertyInfo property)
        {
            object[] attrs = property.GetCustomAttributes(true);

            if (attrs == null)
            {
                return string.Empty;
            }
            foreach (object attr in attrs)
            {
                var dataMember = attr as DataMemberAttribute;

                if (dataMember != null && !string.IsNullOrWhiteSpace(dataMember.Name))
                {
                    return dataMember.Name;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过特性名称获取属性名称
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPropertyNameByAttribute(this Type type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }
            string propertyName = string.Empty;
            var key = type.Name + "_" + name;
            if (!SingletonManager<ConcurrentDictionary<string, string>>.Instance.TryGetValue(key, out propertyName))
            {
                foreach (var property in type.GetProperties())
                {
                    var dataMemberName = property.GetAttributeName();
                    if (dataMemberName.ToLower() == name.ToLower())
                    {
                        propertyName = property.Name;
                        SingletonManager<ConcurrentDictionary<string, string>>.Instance.TryAdd(key, propertyName);
                        return propertyName;
                    }
                }

            }
            return propertyName;
        }
    }
}
