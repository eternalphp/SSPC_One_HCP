using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 个人会议模型--已报名的会议
    /// </summary>
    [DataContract]
    public class MyMeetOrder:BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 是否设置了提醒
        /// 1.是
        /// 2.否
        /// </summary>
        [DataMember]
        public int IsRemind { get; set; }

        /// <summary>
        /// 是否已经提醒
        /// 0.未提醒
        /// 1.已发送提醒短信
        /// </summary>
        [DataMember]
        public int HasReminded { get; set; }

        /// <summary>
        /// 1、15分钟前提醒
        /// 2、一天前提醒
        /// 通过计算得出提醒的时间：
        /// 会议开始时间-24H 或者 -0.25H
        /// </summary>
        [DataMember]
        public DateTime? RemindTime { get; set; }

        /// <summary>
        /// 以会议开始时间为基准，会议提醒需要偏移的分钟数，默认为提前30分钟(-30)
        /// </summary>
        [DataMember]
        public int RemindOffsetMinutes { get; set; }

        /// <summary>
        /// 参加会议的时间
        /// 在会议中的时间
        /// </summary>
        [DataMember]
        public DateTime? JoinInMeetTime { get; set; }

        /// <summary>
        /// 微信用户（医生）的Id
        /// --以后会取代 UnionId
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }
    }
}
