using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [DataContract]
    public class RoleInfo : BaseEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        [DataMember]
        public string RoleDesc { get; set; }
    }
}
