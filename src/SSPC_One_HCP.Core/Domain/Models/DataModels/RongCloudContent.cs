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
    public class RongCloudContent : BaseEntity
    {

        /// <summary>
        /// 发送用户 Id
        /// </summary>
        [DataMember]
        public string FromUserId { get; set; }
        /// <summary>
        /// 目标 Id，即为客户端 targetId，根据会话类型 channelType 的不同，可能为二人会话 Id、群聊 Id、聊天室 Id、客服 Id 等。
        /// </summary>
        [DataMember]
        public string ToUserId { get; set; }
        /// <summary>
        /// 消息类型，
        /// 文本消息 RC:TxtMsg 、 
        /// 图片消息 RC:ImgMsg 、
        /// 语音消息 RC:VcMsg 、
        /// 图文消息 RC:ImgTextMsg 、
        /// 位置消息 RC:LBSMsg 、
        /// 添加联系人消息 RC:ContactNtf 、
        /// 提示条通知消息 RC:InfoNtf 、
        /// 资料通知消息 RC:ProfileNtf 、
        /// 通用命令通知消息 RC:CmdNtf ，
        /// 详细请参见消息类型说明文档。
        /// </summary>
        [DataMember]
        public string ObjectName { get; set; }
        /// <summary>
        /// 发送消息内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
        /// <summary>
        /// 会话类型，
        /// 二人会话是 PERSON 、
        /// 讨论组会话是 PERSONS 、
        /// 群组会话是 GROUP 、
        /// 聊天室会话是 TEMPGROUP 、
        /// 客服会话是 CUSTOMERSERVICE 、 
        /// 系统通知是 NOTIFY 、
        /// 应用公众服务是 MC 、
        /// 公众服务是 MP。
        /// 对应客户端 SDK 中 ConversationType 类型，
        /// 二人会话是 
        /// 1 、讨论组会话是 
        /// 2 、群组会话是 
        /// 3 、聊天室会话是 
        /// 4 、客服会话是 
        /// 5 、 系统通知是 
        /// 6 、应用公众服务是 
        /// 7 、公众服务是 
        /// 8。
        /// </summary>
        [DataMember]
        public string ChannelType { get; set; }
        /// <summary>
        /// 服务端收到客户端发送消息时的服务器时间（1970年到现在的毫秒数）。
        /// </summary>
        [DataMember]
        public string MsgTimeStamp { get; set; }
        /// <summary>
        /// 可通过 msgUID 确定消息唯一。
        /// </summary>
        [DataMember]
        public string MsgUID { get; set; }
        /// <summary>
        /// 消息中是否含有敏感词标识
        /// ，0 为不含有敏感词，
        /// 1 为含有屏蔽敏感词，
        /// 2 为含有替换敏感词。
        /// </summary>
        [DataMember]
        public string SensitiveType { get; set; }
        /// <summary>
        /// 标识消息的发送源头，包括：iOS、Android、Websocket、MiniProgram（小程序）、
        /// Server（通过 Server API 发送，需要开通 Server API 发送消息进行消息路由功能）。
        /// 目前支持单聊、群聊会话类型，其他会话类型为空。
        /// </summary>
        [DataMember]
        public string Source { get; set; }
        /// <summary>
        /// channelType 为 GROUP 时此参数有效，
        /// 显示为群组中指定接收消息的用户 ID 数组，
        /// 该条消息为群组定向消息。非定向消息时内容为空
        /// ，如指定的用户不在群组中内容也为空
        /// </summary>
        [DataMember]
        public string GroupUserIds { get; set; }
        /// <summary>
        /// 微信名称
        /// </summary>
        [DataMember]
        public string WxName { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [DataMember]
        public string WxPicture { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        public string HospitalName { get; set; }
        /// <summary>
        /// 审计
        /// 0：通过
        /// 1：禁用
        /// </summary>
        [DataMember]
        public int Audit { get; set; }
        /// <summary>
        /// 禁用原因
        /// </summary>
        [DataMember]
        public string Reason { get; set; }
        /// <summary>
        /// 审计禁用原因
        /// </summary>
        [DataMember]
        public DateTime? ReasonDateTime { get; set; }
        /// <summary>
        /// 审计用户
        /// </summary>
        [DataMember]
        public string ReasonUser { get; set; }
    }
}
