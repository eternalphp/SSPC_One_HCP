using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议日程视图
    /// </summary>
    [DataContract]
    [NotMapped]
    public class MeetScheduleViewModel
    {
        /// <summary>
        /// 日程当前天
        /// </summary>
        [DataMember]
        public string ScheduleDate { get; set; }

        /// <summary>
        /// 日程安排和讲者信息
        /// </summary>
        [DataMember]
        public IEnumerable<ScheduleView> ScheduleViews { get; set; }

        /// <summary>
        /// 日程安排和讲者信息
        /// </summary>
        [DataMember]
        public IEnumerable<IEnumerable<ScheduleView>> ScheduleViewsList { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Topic { get; set; }
    }
}
