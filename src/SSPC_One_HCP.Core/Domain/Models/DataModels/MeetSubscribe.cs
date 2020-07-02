using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 小程序会议订阅
    /// </summary>
    [DataContract]
    public class MeetSubscribe : BaseEntity
    {
        /// <summary>
        /// 关联会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
        /// <summary>
        /// 微信用户（医生）的Id
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 是否已经提醒
        /// 0.未提醒
        /// 1.已发送提醒短信
        /// </summary>
        [DataMember]
        public int HasReminded { get; set; }
        /// <summary>
        /// 提前通知分钟
        /// </summary>
        [DataMember]
        public int RemindOffsetMinutes { get; set; }
        /// <summary>
        /// 提醒结果
        /// </summary>
        [DataMember]
        public string ReminderResults { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public string TemplateId { get; set; }
    }
}
