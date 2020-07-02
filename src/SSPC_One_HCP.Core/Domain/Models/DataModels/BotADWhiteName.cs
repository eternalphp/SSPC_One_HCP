using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// AD 域认证白名单
    /// </summary>
    public class BotADWhiteName : BaseEntity
    {
        /// <summary>
        /// 域帐号
        /// </summary>
        [DataMember]
        public string ADAccount { get; set; }

        /// <summary>
        /// 类型 0：销售用户  1：测试用户  
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// BU名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 类型 
        /// 0：普通销售  
        /// 1：聊天室审核员  
        /// </summary>
        [DataMember]
        public int ChatAudit { get; set; }
    }
}
