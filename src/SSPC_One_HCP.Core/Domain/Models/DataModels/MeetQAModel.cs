using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议问卷
    /// </summary>
    [DataContract]
    public class MeetQAModel:BaseEntity
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 会议问卷类型
        /// 1.会前问卷
        /// 2.会后问卷
        /// </summary>
        [DataMember]
        public int? QAType { get; set; }

        /// <summary>
        /// 问题Id
        /// </summary>
        [DataMember]
        public string QuestionId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int? Sort { get; set; }
    }
}
