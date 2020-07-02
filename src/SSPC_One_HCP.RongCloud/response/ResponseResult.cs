using Newtonsoft.Json;
using SSPC_One_HCP.RongCloud.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSPC_One_HCP.RongCloud.response
{
    public class ResponseResult : Result

    {
        public ResponseResult(int code, string msg) : base(code, msg)
        {
            this.code = code;
            this.msg = msg;
        }

        override
        public String ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

    }
}
