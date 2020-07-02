using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 组
    /// </summary>
    [DataContract]
    public class GroupInfo : BaseEntity
    {
        /// <summary>
        /// 组名
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }
        /// <summary>
        /// 组类型
        /// </summary>
        [DataMember]
        public string GroupType { get; set; }
    }
}
