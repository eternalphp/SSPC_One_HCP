using System;
using System.Collections.Generic;
using System.Reflection;

namespace SSPC_One_HCP.KBS.Helpers
{
    public static class EnumTool
    {
        /// <summary>
        /// 从枚举中获取Description 
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>枚举值的描述</returns>
        public static string GetDescription<TEnum>(object @value)
        {
            List<System.Collections.DictionaryEntry> lists = GetBindData<TEnum>();
            var a = lists.Find(item => Convert.ToInt32(item.Value) == Convert.ToInt32(@value));
            return a.Key + "";
        }
        public static string GetName<TEnum>(object @value)
        {
            return Enum.GetName(typeof(TEnum), @value);
        }
        /// <summary>  
        /// 获取自定义属性的值 
        /// </summary>  
        /// <param name="value">值</param>  
        /// <returns>枚举值的描述</returns>  
        public static string GetImgSizeAttribute<T>(object @value)
        {
            Type type = @value.GetType();
            MemberInfo[] memInfos = type.GetMember(@value.ToString());
            if (memInfos != null && memInfos.Length > 0)
            {
                object[] customEnumDesc = memInfos[0].GetCustomAttributes(typeof(T), false);

                if (customEnumDesc != null && customEnumDesc.Length > 0)
                {
                    return ((T)customEnumDesc[0]).GetType().GetFields(
                        BindingFlags.DeclaredOnly
                        | BindingFlags.Static
                        | BindingFlags.Instance
                        | BindingFlags.Public
                        | BindingFlags.NonPublic)[0].GetValue(customEnumDesc[0]).ToString();
                }
            }
            return "";
        }

        public static string GetControlNameById(string ControlId)
        {
            return ControlId.Replace("_", "$");
        }

        /// <summary>
        /// 获取某个枚举类型的数据源，数据源是描述和值的组合，返回字段是 Key(description)/Value(int)
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns>集合</returns>
        public static List<System.Collections.DictionaryEntry> GetBindData<TEnum>()
        {
            var obj = ResolveEnum<TEnum>();

            return obj;
        }

        static List<System.Collections.DictionaryEntry> ResolveEnum<TEnum>()
        {
            Type type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new ArgumentException("TEnum requires a Enum", "TEnum");
            }

            FieldInfo[] fields = type.GetFields();

            List<System.Collections.DictionaryEntry> col = new List<System.Collections.DictionaryEntry>();

            foreach (FieldInfo field in fields)
            {
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (objs != null && objs.Length > 0)
                {
                    System.ComponentModel.DescriptionAttribute attr = objs[0] as System.ComponentModel.DescriptionAttribute;

                    col.Add(new System.Collections.DictionaryEntry(attr.Description, ((int)System.Enum.Parse(type, field.Name)).ToString()));
                }

            }

            return col;
        }
    }
}
