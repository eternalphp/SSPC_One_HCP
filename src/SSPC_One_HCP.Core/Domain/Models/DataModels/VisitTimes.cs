using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 访问次数
    /// </summary>
    public class VisitTimes : BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        [DisplayName("UnionId")]
        public string UnionId { get; set; }

        /// <summary>
        /// WxuserId
        /// </summary>
        [DataMember]
        [DisplayName("WxuserId")]
        public string WxuserId { get; set; }


        /// <summary>
        /// 打开开始时间
        /// </summary>
        [DataMember]
        [DisplayName("VisitStart")]
        public DateTime? VisitStart { get; set; }


        /// <summary>
        /// 打开结束时间
        /// </summary>
        [DataMember]
        [DisplayName("VisitEnd")]
        public DateTime? VisitEnd { get; set; }


        /// <summary>
        /// 停留时间（秒）
        /// </summary>
        [DataMember]
        [DisplayName("StaySeconds")]
        public int StaySeconds { get; set; }

        /// <summary>
        /// 是否注册用户
        /// </summary>
        [DataMember]
        [DisplayName("Isvisitor")]
        public int Isvisitor { get; set; }
    }
}
