using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{

    /// <summary>
    /// 肺炎BOT转发记录
    /// </summary>
    [DataContract]
    public class PneumoniaBotForward : BaseEntity
    {
        /// <summary>
        /// 转发用户ID
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 转发用户Unionid
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 转发用户 OpenId
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 转发页面名称
        /// </summary>
        [DataMember]
        public string PageName { get; set; }


    }
}
