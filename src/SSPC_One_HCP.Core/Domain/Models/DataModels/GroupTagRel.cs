using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 标签组和标签关联表
    /// </summary>
    public class GroupTagRel:BaseEntity
    {
        /// <summary>
        /// 标签组Id
        /// </summary>
        [DataMember]
        public string TagGroupId { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        [DataMember]
        public string TagId { get; set; }
    }
}
