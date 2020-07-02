using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 医生标签关系
    /// </summary>
    [DataContract]
    public class DocTag : BaseEntity
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        [DataMember]
        public string DocId { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        [DataMember]
        public string TagId { get; set; }
    }
}
