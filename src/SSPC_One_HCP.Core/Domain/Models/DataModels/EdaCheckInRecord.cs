using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// Eda签到记录表
    /// </summary>
    public class EdaCheckInRecord : BaseEntity
    {
        /// <summary>
        /// 公众号APPId
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [DataMember]
        public string WxName { get; set; }

        /// <summary>
        /// 科内会编号
        /// </summary>
        [DataMember]
        public string ActivityID { get; set; }
  
        /// <summary>
        /// 访问时间
        /// </summary>
        [DataMember]
        public DateTime? VisitTime { get; set; }



    }
}
