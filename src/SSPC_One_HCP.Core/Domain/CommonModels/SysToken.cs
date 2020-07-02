using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 系统授权
    /// </summary>
    [DataContract]
    public class SysToken
    {
        /// <summary>
        /// 授权码
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// 授权码类型
        /// </summary>
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
        /// <summary>
        /// 授权码时间
        /// </summary>
        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
        /// <summary>
        /// 用于刷新授权码的key
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }
        /// <summary>
        /// 用户的连接ID
        /// </summary>

        [DataMember(Name = "as:client_id")]
        public string ClientId { get; set; }
        /// <summary>
        /// 登录的用户名
        /// </summary>

        [DataMember(Name = "userName")]
        public string UserName { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>

        [DataMember(Name = "res_code")]
        public string ResCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>

        [DataMember(Name = "res_msg")]
        public string ResMsg { get; set; }
        /// <summary>
        /// 授权码生效时间
        /// </summary>

        [DataMember(Name = ".issued")]
        public string Issued { get; set; }
        /// <summary>
        /// 授权码失效时间
        /// </summary>

        [DataMember(Name = ".expires")]
        public string Expires { get; set; }
    }
}
