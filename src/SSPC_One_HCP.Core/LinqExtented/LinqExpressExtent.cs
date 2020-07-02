using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.LinqExtented
{
    public static class LinqExpressExtent
    {
        /// <summary>
        /// 动态查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, T t) where T : class, new()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), typeof(T).Name);
            var properties = t.GetType().GetProperties();
            Expression orExpression = null;
            foreach (var prop in properties)
            {
                //var keyAttr = (KeyAttribute)Attribute.GetCustomAttribute(prop, typeof(KeyAttribute));
                //var pageAttr = (PageAttribute)Attribute.GetCustomAttribute(prop, typeof(PageAttribute));
                var name = prop.Name;
                var value = prop.GetValue(t);
                //if (keyAttr != null)
                //    continue;
                if ((value == null || value.ToString() == "0" || value.ToString() == ""))
                    continue;
                if (name == "PageIndex" || name == "PageSize" || name == "IsExport" || name == "IsImport")
                    continue;
                if (name.StartsWith("View_") || name.EndsWith("Str"))
                    continue;
                if (name == "OrderName" || name == "SortName")
                    continue;
                if (name.EndsWith("_Start") || name.EndsWith("_End"))
                    continue;
                Expression left = Expression.Property(param, prop);
                Expression right = Expression.Constant(value, prop.PropertyType);
                Expression filter = Expression.Equal(left, right);
                if (prop.PropertyType == typeof(string))
                {
                    filter = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
                }
                if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    if (prop.GetValue(t) == null || prop.GetValue(t).ToString() == "")
                    {
                        continue;
                    }
                    //if (!String.IsNullOrEmpty(pageAttr?.StartOrEndTime))
                    //{
                    //    var propertyInfoStart = typeof(T).GetProperty(name + "_Start");
                    //    var propertyInfoEnd = typeof(T).GetProperty(name + "_End");

                    //    DateTime startTime = DateTime.MinValue;
                    //    DateTime endTime = DateTime.MinValue;
                    //    if (propertyInfoStart != null)
                    //    {
                    //        if (propertyInfoStart.GetValue(t) == null || string.IsNullOrEmpty(propertyInfoStart.GetValue(t).ToString()))
                    //            startTime = Convert.ToDateTime(propertyInfoStart.GetValue(t));
                    //    }
                    //    if (propertyInfoEnd != null)
                    //    {
                    //        endTime = Convert.ToDateTime(propertyInfoEnd.GetValue(t));
                    //    }
                    //    if (startTime.ToString("yyyy/MM/dd").Contains("0001/01/01") || startTime.ToString("yyyy-MM-dd").Contains("0001-01-01"))
                    //        continue;
                    //    if (endTime.ToString("yyyy/MM/dd").Contains("0001/01/01") || endTime.ToString("yyyy-MM-dd").Contains("0001-01-01"))
                    //        continue;
                    //    Expression filterStart = Expression.GreaterThanOrEqual(left, Expression.Constant(startTime, prop.PropertyType));
                    //    Expression filterEnd = Expression.LessThan(left, Expression.Constant(endTime.AddDays(1), prop.PropertyType));
                    //    filter = Expression.And(filterStart, filterEnd);
                    //}
                    //else
                    //{
                    //    if (right.ToString().Contains("0001/1/1") || right.ToString().Contains("0001-1-1"))
                    //        continue;
                    //}


                }
                if (orExpression == null)
                {
                    orExpression = Expression.Constant(true);
                }
                orExpression = Expression.And(orExpression, filter);
            }
            //((ConstantExpression)orExpression).Value == false
            if (orExpression != null)
            {
                Expression pred = Expression.Lambda(orExpression, param);
                Expression expr = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(T) }, Expression.Constant(queryable), pred);
                queryable = queryable.Provider.CreateQuery<T>(expr);
            }

            return queryable;
        }
        /// <summary>
        /// 扩展排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string sort) where T : class, new()
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "Not " + "Exist");
            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);
            string methodName = sort == "asc" ? "OrderBy" : "OrderByDescending";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> ToPaginationList<T>(this IQueryable<T> queryable, int? pageIndex, int? pageSize)// where T : class, new()
        {
            int index = pageIndex ?? 0, size = pageSize ?? 0;
            if (index < 1) index = 1;
            if (size < 1) size = 100;
            return queryable.Skip((index - 1) * size).Take(size);
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToPaginationList<T>(this IEnumerable<T> queryable, int? pageIndex, int? pageSize)
        {
            int index = pageIndex ?? 0, size = pageSize ?? 0;
            if (index < 1) index = 1;
            if (size < 1) size = 100;
            return queryable.Skip((index - 1) * size).Take(size);
        }
    }
}
