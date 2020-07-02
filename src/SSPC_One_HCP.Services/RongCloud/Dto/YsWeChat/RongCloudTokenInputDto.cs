using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto.YsWeChat
{
    public class RongCloudTokenInputDto
    {
        /// <summary>
        /// 用户 Id（必传）
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称（必传）
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 用户头像 URI（必传）
        /// </summary>
        [DataMember]
        public string PortraitUri { get; set; }
    }
}
