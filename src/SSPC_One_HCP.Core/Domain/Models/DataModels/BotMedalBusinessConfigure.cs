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
    /// 勋章业务规则配置
    /// </summary>
    [DataContract]
    public class BotMedalBusinessConfigure : BaseEntity
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
        /// KBS 知识包编号
        /// </summary>
        [DataMember]
        public string FaqPackageId { get; set; }
        /// <summary>
        /// KBS 知识包名称
        /// </summary>
        [DataMember]
        public string FaqPackageName { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        [DataMember]
        public string MedalName { get; set; }
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
