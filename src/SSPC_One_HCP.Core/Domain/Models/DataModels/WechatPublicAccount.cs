using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 微信公众号
    /// </summary>
    [DataContract]
    public class WechatPublicAccount : BaseEntity
    {
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 微信公众号名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 微信公众号简介
        /// </summary>
        [DataMember]
        public string Summary { get; set; }
        
        /// <summary>
        /// 点击量
        /// </summary>
        [DataMember]
        public long? ClickVolume { get; set; }
    }
}
