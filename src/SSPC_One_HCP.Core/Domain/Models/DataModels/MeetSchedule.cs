using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议日程模型
    /// </summary>
    [DataContract]
    public class MeetSchedule : BaseEntity
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 日程开始时间
        /// </summary>
        [DataMember]
        public DateTime? ScheduleStart { get; set; }

        /// <summary>
        /// 日程结束时间
        /// </summary>
        [DataMember]
        public DateTime? ScheduleEnd { get; set; }

        /// <summary>
        /// 讲题
        /// </summary>
        [DataMember]
        public string ScheduleContent { get; set; }

        /// <summary>
        /// 会议讲者Id
        /// </summary>
        [DataMember]
        public string MeetSpeakerId { get; set; }

        /// <summary>
        /// 上午下午
        /// </summary>
        [DataMember]
        public int AMPM { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }
        /// <summary>
        /// 课题
        /// </summary>
        [DataMember]
        public string Topic { get; set; }
        /// <summary>
        /// 课题讲者（主席）
        /// </summary>
        [DataMember]
        public string Speaker { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        [DataMember]
        public string Hospital { get; set; }

    }
}
