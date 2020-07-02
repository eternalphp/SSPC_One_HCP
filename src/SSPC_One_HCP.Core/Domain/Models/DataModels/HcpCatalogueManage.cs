using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 目录管理
    /// </summary>
    [DataContract]
    public class HcpCatalogueManage : BaseEntity
    {
        /// <summary>
        /// BU信息
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        [DataMember]
        public string CatalogueName { get; set; }
        
    }
}
