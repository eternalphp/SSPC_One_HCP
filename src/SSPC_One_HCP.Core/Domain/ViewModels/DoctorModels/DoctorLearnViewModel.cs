using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 医生学习列表模型
    /// </summary>
    [DataContract]
    public class DoctorLearnViewModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        [DisplayName("序号")]
        public string Id { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        [DisplayName("医生姓名")]
        [DataMember]
        public string DoctorName { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        [DisplayName("医院名称")]
        public string HospitalName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        [DisplayName("科室名称")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 文档学习时间
        /// </summary>
        [DataMember]
        [DisplayName("文档学习时间")]
        public int? DocLearnTime { get; set; }

        /// <summary>
        /// 视频学习时间
        /// </summary>
        [DataMember]
        [DisplayName("视频学习时间")]
        public int? VideoLearnTime { get; set; }

        /// <summary>
        /// 文章学习时间
        /// </summary>
        [DataMember]
        [DisplayName("文章学习时间")]
        public int? ArticleLearnTime { get; set; }

        /// <summary>
        /// 播客学习时间
        /// </summary>
        [DataMember]
        [DisplayName("播客学习时间")]
        public int? PodcastLearnTime { get; set; }

        /// <summary>
        /// 临床指南学习时间
        /// </summary>
        [DataMember]
        [DisplayName("临床指南学习时间")]
        public int? GuidVistTime { get; set; }

        /// <summary>
        /// 录播时间
        /// </summary>
        [DataMember]
        [DisplayName("录播时间")]
        public int? BroadcastTime { get; set; }


        /// <summary>
        /// 用药参考学习时间
        /// </summary>
        [DataMember]
        [DisplayName("用药参考学习时间")]
        public int? MedicineVistTime { get; set; }


        /// <summary>
        /// 期刊查询学习时间
        /// </summary>
        [DataMember]
        [DisplayName("期刊查询学习时间")]
        public int? BookVisitTime { get; set; }


        /// <summary>
        /// 参会次数
        /// </summary>
        [DataMember]
        [DisplayName("参会次数")]
        public int? MeetCount { get; set; }

        /// <summary>
        /// 医生标签
        /// </summary>
        [DataMember]
        [DisplayName("标签")]
        public IList<string> Lable { get; set; }

        /// <summary>
        /// 查询的开始时间
        /// </summary>
        [DataMember]
        [DisplayName("开始时间")]
        public DateTime? Learn_Start { get; set; }

        /// <summary>
        /// 查询的结束时间
        /// </summary>
        [DataMember]
        [DisplayName("结束时间")]
        public DateTime? Learn_End { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [DataMember]
        [DisplayName("省份")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [DataMember]
        [DisplayName("城市")]
        public string City { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [DataMember]
        [DisplayName("毕业院校")]
        public string School { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        [DataMember]
        [DisplayName("职称")]
        public string Title { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        [DataMember]
        [DisplayName("数据来源")]
        public string DataSource { get; set; }
        
        /// <summary>
        /// 是否已认证过
        /// 1、已认证
        /// 2、不确定
        /// 3、认证失败
        /// 4、申诉中
        /// 5、认证中
        /// 6、申诉拒绝
        /// </summary>
        [DataMember]
        [DisplayName("状态")]
        public string IsVerify { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [DataMember]
        [DisplayName("标签")]
        public IList<string> DocTags { get; set; }
    }
}
