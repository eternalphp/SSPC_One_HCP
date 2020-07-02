using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 融云-用户聊天信息
    /// </summary>
    [DataContract]
    public class RongCloudChatroomStatus : BaseEntity
    {

        /// <summary>
        /// 聊天室ID
        /// </summary>
        [DataMember]
        public string ChatRoomId { get; set; }
        /// <summary>
        /// 用户 Id 数据。
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 操作状态：0 直接调用接口、1 触发融云退出聊天室机制将用户踢出、2 用户被封禁、3 触发融云销毁聊天室机制自动销毁
        /// </summary>
        [DataMember]
        public string Status { get; set; }
        /// <summary>
        /// 聊天室事件类型：0 创建聊天室、1 加入聊天室、2 退出聊天室、3 销毁聊天室
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// 发生时间。
        /// </summary>
        [DataMember]
        public long Time { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
       
        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        public string HospitalName { get; set; }
    }
}
