using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// bu及产品及科室关系
    /// </summary>
    [DataContract]
    public class BuProDeptRel : BaseEntity
    {
        /// <summary>
        /// bu名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        [DataMember]
        public string ProId { get; set; }
        /// <summary>
        /// 科室Id
        /// </summary>
        [DataMember]
        public string DeptId { get; set; }
    }
}
