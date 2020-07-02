using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 医生分组
    /// </summary>
    [DataContract]
    public class DocGroup : BaseEntity
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        [DataMember]
        public string DocId { get; set; }
        /// <summary>
        /// 组Id
        /// </summary>
        [DataMember]
        public string GroupId { get; set; }
    }
}
