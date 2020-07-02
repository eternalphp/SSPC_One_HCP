using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.IdentityEntities
{
    /// <summary>
    /// 后台用户角色表
    /// </summary>
    [DataContract]
    public class UserRole:BaseEntity
    {
        /// <summary>
        /// 用户sap编码
        /// </summary>
        [DataMember]
        public string SapCode { get; set; }
        /// <summary>
        /// 用户主键
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 角色主键
        /// </summary>
        [DataMember]
        public string RoleId { get; set; }
    }
}
