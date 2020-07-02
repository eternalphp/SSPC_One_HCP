using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.Dto
{
    public class TemplateRouteInputDto
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

    }

    public class ContentInputDto
    {
        public string Content { get; set; }

        public ContentUser User { get; set; }
    }

    public class ContentUser
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
