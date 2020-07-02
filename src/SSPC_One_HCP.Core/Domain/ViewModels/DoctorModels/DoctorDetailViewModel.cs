using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels
{

    public class DoctorDetailViewModel
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [DataMember]
        public DateTime begin_date { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public DateTime end_date { get; set; }

        /// <summary>
        /// wxuserid
        /// </summary>
        [DataMember]
        public string wxuserid { get; set; }

        /// <summary>
        /// 学习资料类别
        /// 1、文章
        /// 2、文档
        /// 3、播客
        /// 4、视频
        /// 5、会议 
        /// </summary>
        [DataMember]
        public string MediaType { get; set; }
    }
}
