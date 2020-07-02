using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations.Dto
{
    public class DoctorLearnDto
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 文档
        /// </summary>
        public string DocLearnTime { get; set; }
        /// <summary>
        /// 博客
        /// </summary>
        public string PodcastLearnTime { get; set; }
        /// <summary>
        /// 产品视频
        /// </summary>
        public string VideoLearnTime { get; set; }
        /// <summary>
        /// 参会次数
        /// </summary>
        public int? MeetCount { get; set; }
        /// <summary>
        /// 录播视频
        /// </summary>
        public string BroadcastTime { get; set; }
        /// <summary>
        /// 临床指南
        /// </summary>
        public string GuidVistTime { get; set; }
        /// <summary>
        /// 用药参考
        /// </summary>
        public string MedicineVistTime { get; set; }
        /// <summary>
        /// 期刊
        /// </summary>
        public string BookVisitTime { get; set; }
        /// <summary>
        /// 手动标签
        /// </summary>
        public string DocTags { get; set; }
    }
}
