using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 资料与目录关系
    /// </summary>
    [DataContract]
    public class HcpDataCatalogueRel : BaseEntity
    {
        /// <summary>
        /// BU与目录关系ID
        /// </summary>
        [DataMember]
        public string HcpCatalogueManageId { get; set; }
        /// <summary>
        /// 资料ID
        /// </summary>
        [DataMember]
        public string HcpDataInfoId { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        [DataMember]
        public string HcpCatalogueManageName { get; set; }

    }
}
