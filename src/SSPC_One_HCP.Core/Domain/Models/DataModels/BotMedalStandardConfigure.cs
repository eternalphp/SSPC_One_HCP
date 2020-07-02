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
    /// 勋章标准规则配置
    /// </summary>
    [DataContract]
    public class BotMedalStandardConfigure : BaseEntity
    {
        /// <summary>
        /// KBS BOT编号
        /// </summary>
        [DataMember]
        public string KBSBotId { get; set; }
        /// <summary>
        /// KBS BOT名称
        /// </summary>
        [DataMember]
        public string KBSBotName { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        [DataMember]
        public string MedalName { get; set; }
        /// <summary>
        /// FAQ总次数规则
        /// </summary>
        [DataMember]
        public int Ruletotal { get; set; }
        /// <summary>
        /// 已获取勋章图片地址
        /// </summary>
        [DataMember]
        public string MedalYSrc { get; set; }
        /// <summary>
        /// 未获取勋章图片地址
        /// </summary>
        [DataMember]
        public string MedalNSrc { get; set; }
        
     
    }
}
