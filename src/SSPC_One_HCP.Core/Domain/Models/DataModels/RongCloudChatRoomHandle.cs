using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 融云-聊天室事件
    /// </summary>
    [DataContract]
    public class RongCloudChatRoomHandle : BaseEntity
    {
        /// <summary>
        /// 聊天室ID
        /// </summary>
        [DataMember]
        public string ChatRoomId { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        [DataMember]
        public string Event { get; set; }
    }
}
