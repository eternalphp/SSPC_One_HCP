using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 系列课程
    /// </summary>
    [DataContract]
    public class SeriesCourses : BaseEntity
    {
        /// <summary>
        /// 课程标题
        /// </summary>
        [DataMember]
        public string CourseTitle { get; set; }
        /// <summary>
        /// 主讲人
        /// </summary>
        [DataMember]
        public string Speaker { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        [DataMember]
        public string Hospital { get; set; }
        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int IsHot { get; set; }
        /// <summary>
        /// 封面图（小）
        /// </summary>
        [DataMember]
        public string CourseCoverSmall { get; set; }
        /// <summary>
        /// 封面图（大）
        /// </summary>
        [DataMember]
        public string CourseCoverBig { get; set; }
      
    }
}
