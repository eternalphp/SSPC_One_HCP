using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
   public class MeetSubscribeInputDto
    {
        /// <summary>
        /// 关联会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
        /// <summary>
        /// 微信用户（医生）的Id
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public List<string> TemplateId { get; set; }
    }
}
