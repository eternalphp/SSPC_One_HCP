using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.WxModels
{
    /// <summary>
    /// 微信小程序返回基础模型
    /// </summary>
    [DataContract]
    public class WxBaseModel
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        [DataMember(Name = "errcode")]
        public string ErrCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [DataMember(Name = "errmsg")]
        public string ErrMsg { get; set; }
    }
}
