using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 小程序对接接口 用户行为记录
    /// </summary>
    [DataContract]
    public class WechatActionHistory : BaseEntity
    {
        /// <summary>
        /// 行为类别 1| 用药参考  2|期刊查询
        /// </summary>
        [DataMember]
        public int ActionType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 内容Id
        /// </summary>
        [DataMember]
        public string ContentId { get; set; }

        /// <summary>
        /// 微信 UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// WxuserId
        /// </summary>
        [DataMember]
        public string WxuserId { get; set; }

        /// <summary>
        /// 停留时长
        /// </summary>
        [DataMember]
        public int? StaySeconds { get; set; }
    }
}
