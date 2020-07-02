using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SSPC_One_HCP.KBS.Helpers
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(object target, bool isConvertToSingleQuotes = false)
        {
            if (target == null)
                return string.Empty;
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var result = JsonConvert.SerializeObject(target, serializerSettings);
            if (isConvertToSingleQuotes)
                result = result.Replace("\"", "'");
            return result;
        }
        /// <summary>
        /// 将Json字符串转换为动态对象
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(string jsonString)
        {
            return JToken.Parse(jsonString) as dynamic;
        }
        /// <summary>
        /// 将动态对象转换为Json字符串
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static dynamic ToJArray(string jsonString)
        {
            return JArray.Parse(jsonString) as JArray;
        }
    }
}
