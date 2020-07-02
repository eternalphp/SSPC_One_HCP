using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 我的学习记录
    /// </summary>
    [DataContract]
    public class MyLRecord : BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// 学习对象的Id
        /// </summary>
        [DataMember]
        public string LObjectId { get; set; }

        /// <summary>
        /// 学习对象的类型
        /// 1、文章
        /// 2、文档
        /// 3、播客
        /// 4、视频
        /// 5、会议
        /// </summary>
        [DataMember]
        public int? LObjectType { get; set; }

        /// <summary>
        /// 学习的时间
        /// </summary>
        [DataMember]
        public DateTime LDate { get; set; }
        /// <summary>
        /// 学习开始时间
        /// </summary>
        [DataMember]
        public DateTime? LDateStart { get; set; }
        /// <summary>
        /// 学习结束时间
        /// </summary>
        [DataMember]
        public DateTime? LDateEnd { get; set; }

        /// <summary>
        /// 学习的时长
        /// </summary>
        [DataMember]
        public int? LObjectDate { get; set; }
       
        /// <summary>
        /// 是否已读
        /// 1.已读
        /// </summary>
        [DataMember]
        public int? IsRead { get; set; }

        /// <summary>
        /// 微信用户（医生）的Id
        /// --以后会取代 UnionId
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }
    }
}
