using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议相关图片
    /// </summary>
    [DataContract]
    public class MeetPic : BaseEntity
    {
        /// <summary>
        /// 关联会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
        /// <summary>
        /// 封面图名称
        /// </summary>
        [DataMember]
        public string MeetPicName { get; set; }
        /// <summary>
        /// 封面图类型 
        /// L: 大
        /// S: 小
        /// </summary>
        [DataMember]
        public string MeetPicType { get; set; }
        /// <summary>
        /// 封面图地址
        /// </summary>
        [DataMember]
        public string MeetPicUrl { get; set; }
    }
}
