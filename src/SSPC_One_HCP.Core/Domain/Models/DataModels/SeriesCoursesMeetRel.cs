using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 系列课程与会议关系
    /// </summary>
    [DataContract]
    public class SeriesCoursesMeetRel : BaseEntity
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        [DataMember]
        public string SeriesCoursesId { get; set; }
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetInfoId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }


    }
}
