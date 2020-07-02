using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto
{
    public class ChatroomUserGagAddDto
    {
        /// <summary>
        /// 用户 Id，可同时禁言多个用户，最多不超过 20 个。（必传）
        /// </summary>
        public List<string> UserIds { get; set; }
        /// <summary>
        /// 聊天室 Id。（必传）
        /// </summary>
        public string ChatroomId { get; set; }
        /// <summary>
        /// 禁言时长，以分钟为单位，最大值为43200分钟。（必传）
        /// </summary>
        public string Minute { get; set; } = "43200";
    }
}
