using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 通用返回模型
    /// </summary>
    [DataContract]
    public class ReturnValueModel
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        [DataMember(Name ="success")]
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name ="msg")]
        public string Msg { get; set; }
        /// <summary>
        /// 返回参数
        /// </summary>
        [DataMember(Name ="result")]
        public object Result { get; set; }

        /// <summary>
        /// 响应时间(秒)
        /// </summary>
        [DataMember(Name = "time")]
        public double? ResponseTime { get; set; }
    }
}
