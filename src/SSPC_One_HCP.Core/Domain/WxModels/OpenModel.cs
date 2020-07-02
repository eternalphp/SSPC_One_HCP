using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.WxModels
{
    /// <summary>
    /// 获取openId返回
    /// </summary>
    [DataContract]
    public class OpenModel : WxBaseModel
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [DataMember(Name = "openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        [DataMember(Name = "session_key")]
        public string SessionKey { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符
        /// </summary>
        [DataMember(Name = "unionid")]
        public string UnionId { get; set; }
    }
}
