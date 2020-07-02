using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 直播在线人数记录
    /// </summary>
    [DataContract]
    public class LiveOnline : BaseEntity
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetInfoId { get; set; }

        /// <summary>
        /// 在线人数
        /// </summary>
        [DataMember]
        public int Total { get; set; }
    }
}
