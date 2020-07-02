using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 多语言配置
    /// </summary>
    [DataContract]
    public class LanguageConfig : BaseEntity
    {
        /// <summary>
        /// 语言key
        /// </summary>
        [DataMember]
        public string LanKey { get; set; }
        /// <summary>
        /// 语言展示
        /// </summary>
        [DataMember]
        public string LanValue { get; set; }
        /// <summary>
        /// 语言类型
        /// </summary>
        [DataMember]
        public string LanType { get; set; }
    }
}
