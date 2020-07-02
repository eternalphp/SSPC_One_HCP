using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议问卷列表
    /// </summary>
    [DataContract]
    public class MeetQARelationViewModel
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
        
        /// <summary>
        /// 会议问卷类型
        /// 1、会前问卷
        /// 2、会后问卷
        /// </summary>
        [DataMember]
        public int? QAType { get; set; }
        
        /// <summary>
        /// 问卷来源会议ID
        /// </summary>
        [DataMember]
        public string FromMeetId { get; set; }

        /// <summary>
        /// 问卷来源会议问卷类型
        /// 1、会前问卷
        /// 2、会后问卷
        /// </summary>
        [DataMember]
        public int? FromQAType { get; set; }

        /// <summary>
        /// 会议和问题关系列表
        /// </summary>
        [DataMember]
        public IEnumerable<MeetQAModel> meetQAs { get; set; }
    }
}
