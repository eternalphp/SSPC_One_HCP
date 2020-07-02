using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 临床指南 浏览记录
    /// </summary>
    public class GuidVisit : BaseEntity
    {
        /// <summary>
        /// userid
        /// </summary>
        [DataMember]
        public string userid { get; set; }

        /// <summary>
        /// 行为类型
        /// </summary>
        [DataMember]
        public string ActionType { get; set; }

        /// <summary>
        /// 指南Id
        /// </summary>
        [DataMember]
        public int GuideId { get; set; }

        /// <summary>
        /// 指南类型
        /// </summary>
        [DataMember]
        public string GuideType { get; set; }

        /// <summary>
        /// 指南内容
        /// </summary>
        [DataMember]
        public string GuideName { get; set; }

        /// <summary>
        /// 指南邮件
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 指南关键字
        /// </summary>
        [DataMember]
        public string Keyword { get; set; }

        /// <summary>
        /// 停留开始时间
        /// </summary>
        [DataMember]
        public DateTime? VisitStart { get; set; }

        /// <summary>
        /// 停留结束时间
        /// </summary>
        [DataMember]
        public DateTime? VisitEnd { get; set; }

        /// <summary>
        /// 停留时长
        /// </summary>
        [DataMember]
        public int? StaySeconds { get; set; }
    }
}
