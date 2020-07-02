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
    /// 会议日程表-时间改为string
    /// </summary>
    [DataContract]
    [NotMapped]
    public class MeetScheduleView: MeetSchedule
    {
        /// <summary>
        /// 日程开始时间
        /// </summary>
        [DataMember]
        public new string ScheduleStart { get; set; }

        /// <summary>
        /// 日程结束时间
        /// </summary>
        [DataMember]
        public new string ScheduleEnd { get; set; }
    }
}
