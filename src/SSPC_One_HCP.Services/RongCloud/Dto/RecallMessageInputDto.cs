using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto
{
    public class RecallMessageInputDto
    {
        /// <summary>
        /// 撤回消息体 发送人id
        /// </summary>
        [DataMember]
        public string SenderId { get; set; }

        /// <summary>
        /// 接收人id
        /// </summary>
        [DataMember]
        public string TargetId { get; set; }
        /// <summary>
        /// 消息唯一标识 各端 SDK 发送消息成功后会返回 uId
        /// </summary>
        [DataMember]
        public string UId { get; set; }

        /// <summary>
        /// 消息的发送时间，各端 SDK 发送消息成功后会返回 sentTime
        /// </summary>
        [DataMember]
        public string SentTime { get; set; }

        public RecallMessageInputDto()
        {
        }

        /**
         * @param senderId	String	消息发送人用户 Id。（必传）
         * @param conversationType	Int	会话类型，二人会话是 1 、讨论组会话是 2 、群组会话是 3 。（必传）
         * @param targetId	String	目标 Id，根据不同的 ConversationType，可能是用户 Id、讨论组 Id、群组 Id。（必传）
         * @param uId	String	消息唯一标识，可通过服务端实时消息路由获取，对应名称为 msgUID。（必传）
         * @param sentTime
         *
         * */
        public RecallMessageInputDto(string senderId, string conversationType, string targetId, string uId, string sentTime)
        {
            this.SenderId = senderId;
            this.TargetId = targetId;
            this.UId = uId;
            this.SentTime = sentTime;
        }
    }
}
