using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 我的已读记录
    /// </summary>
    [DataContract]
    public class MyReadRecord : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 数据Id
        /// </summary>
        [DataMember]
        public string DataInfoId { get; set; }
        /// <summary>
        /// 是否已读
        /// 1.已读
        /// 2.未读
        /// </summary>
        [DataMember]
        public int? IsRead { get; set; }
        /// <summary>
        /// 微信用户（医生）的Id
        /// --以后会取代 UnionId
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }
    }
}
