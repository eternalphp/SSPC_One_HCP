using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto
{
    public class ChatroomStatusSyncDto
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [DataMember]
        public string SignTimestamp { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        [DataMember]
        public string Nonce { get; set; }
        /// <summary>
        /// 系统分配的 App Secret
        /// </summary>
        [DataMember]
        public string Signature { get; set; }
        //[DataMember]
        //public List<RongCloudChatroomStatus> ChatroomStatus { get; set; }
    }

    public class RongCloudChatroomStatusDto
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
        public List<string> UserIds { get; set; }
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
    }
}
