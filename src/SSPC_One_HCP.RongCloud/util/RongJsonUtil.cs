using System;
using Newtonsoft.Json;

namespace SSPC_One_HCP.RongCloud.util
{
    public class RongJsonUtil
    {
        public static ObjType JsonStringToObj<ObjType>(string JsonString) where ObjType : class
        {
            ObjType s = JsonConvert.DeserializeObject<ObjType>(JsonString);
            return s;
        }

        public static String ObjToJsonString(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}