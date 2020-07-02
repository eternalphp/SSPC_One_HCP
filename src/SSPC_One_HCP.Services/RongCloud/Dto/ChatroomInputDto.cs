using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto
{
   public class ChatroomInputDto
    {
        /// <summary>
        /// 聊天室ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 聊天室名。
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
