using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 报表日期参数模型
    /// </summary>
    public class StatisticsTimeViewModel
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
        /// 是否游客 是|否  1|0
        /// </summary>
        [DataMember]
        public string IsVistor { get; set; }

        /// <summary>
        /// 学习种类
        /// 1、文章
        /// 2、文档
        /// 3、播客
        /// 4、视频
        /// 5、会议 
        /// </summary>
        public string StudyType { get; set; }
    }
}
