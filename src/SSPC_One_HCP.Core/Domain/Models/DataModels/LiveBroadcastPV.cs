using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 直播PV
    /// </summary>
    public class LiveBroadcastPV : BaseEntity
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetInfoId { get; set; }
        /// <summary>
        /// Unionid
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [DataMember]
        public string WxName { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [DataMember]
        public string WxPicture { get; set; }
       
    }
}
