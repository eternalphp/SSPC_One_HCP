using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 产品和科室关系表
    /// </summary>
    [DataContract]
    public class ProAndDepRelation : BaseEntity
    {
        /// <summary>
        /// 产品id
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// 科室Id
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }
    }
}
