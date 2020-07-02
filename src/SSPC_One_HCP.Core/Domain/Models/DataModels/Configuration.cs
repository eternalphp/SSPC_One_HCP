using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [DataContract]
    public class Configuration: BaseEntity
    {
        /// <summary>
        /// 配置标识
        /// </summary>
        [DataMember]
        public string ConfigureName { get; set; }
        /// <summary>
        /// 配置标识值
        /// </summary>
        [DataMember]
        public string ConfigureValue { get; set; }
    }
}
