using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.FkLibSyncModels
{
    /// <summary>
    /// 签到同步Model
    /// </summary>
    [DataContract]
    [NotMapped]
    public class CheckInSyncModel
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        [DataMember]
        public string ActivityID { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [DataMember]
        public string OpenName { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }

        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// OneHCP医生唯一ID
        /// </summary>
        [DataMember]
        public string OneHCPID { get; set; }

        /// <summary>
        /// OneHCP验证结果
        /// </summary>
        [DataMember]
        public string OneHCPState { get; set; }

        /// <summary>
        /// OneHCP理由
        /// </summary>
        [DataMember]
        public string OneHCPReason { get; set; }

        /// <summary>
        /// 云势ID
        /// </summary>
        [DataMember]
        public string YSID { get; set; }
    }
}
