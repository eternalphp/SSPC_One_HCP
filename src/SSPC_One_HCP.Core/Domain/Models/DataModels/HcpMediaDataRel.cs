using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 关系表
    /// </summary>
    [DataContract]
    public class HcpMediaDataRel : BaseEntity
    {
        /// <summary>
        /// BU信息
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string DeptId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [DataMember]
        public string ProId { get; set; }
        /// <summary>
        /// 媒体主键
        /// </summary>
        [DataMember]
        public string DataInfoId { get; set; }
    }
}
