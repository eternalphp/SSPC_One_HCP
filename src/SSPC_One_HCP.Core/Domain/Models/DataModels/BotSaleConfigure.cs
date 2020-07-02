using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 销售端BOT配置
    /// </summary>
    [DataContract]
    public class BotSaleConfigure : BaseEntity
    {
        /// <summary>
        /// KBSBot主键
        /// </summary>
        [DataMember]
        public string KBSBotId { get; set; }
        /// <summary>
        /// KBSBot名称
        /// </summary>
        [DataMember]
        public string BotName { get; set; }
        /// <summary>
        /// 小程序
        /// </summary>
        [DataMember]
        public string AppId { get; set; }
        /// <summary>
        /// 小程序
        /// </summary>
        [DataMember]
        public string AppSecret { get; set; }
    }

  
}
