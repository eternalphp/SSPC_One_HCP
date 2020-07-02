using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
    public class SeriesCoursesOutDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 课程标题
        /// </summary>
        public string CourseTitle { get; set; }
        /// <summary>
        /// 主讲人
        /// </summary>
        public string Speaker { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string Hospital { get; set; }
        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        public int IsHot { get; set; }
        /// <summary>
        /// 封面图（小）
        /// </summary>
        public string CourseCoverSmall { get; set; }
        /// <summary>
        /// 封面图（大）
        /// </summary>
        public string CourseCoverBig { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        //public string StateName { get; set; }

        public DateTime? MeetStartTime { get; set; }
        public DateTime? MeetEndTime { get; set; }
    }
}
