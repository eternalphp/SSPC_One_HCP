using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// DataTable、List互转
    /// </summary>
    public static  class DataTableUtils
    {
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="sqlBulkCopy"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection,SqlBulkCopy sqlBulkCopy)
        {
            var props = typeof(T).GetProperties().ToList();
            var dt = new DataTable();
            foreach (var prop in props)
            {
                dt.Columns.Add(new DataColumn(prop.Name, prop.PropertyType.GetNoNullType()));
                sqlBulkCopy.ColumnMappings.Add(prop.Name, prop.Name);
            }
            if (collection.Any())
            {
                foreach (T item in collection)
                {
                    DataRow row = dt.NewRow();
                    foreach (var prop in props)
                    {
                        row[prop.Name] = prop.GetValue(item,null) == null ? DBNull.Value : prop.GetValue(item,null);
                    }
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        private static Type GetNoNullType(this Type type)
        {
            var retType = typeof(string);
            if (type.UnderlyingSystemType.ToString() == "System.Nullable`1[System.DateTime]")
            {
                retType = typeof(DateTime);
            }
            if (type.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int64]")
            {
                retType = typeof(long);
            }
            if (type.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int32]")
            {
                retType = typeof(int);
            }
            return retType;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }
        /// <summary>
        /// DataRowList转list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取DataRow数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="row">DataRow</param>
        /// <returns></returns>
        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }
    }
}
