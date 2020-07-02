using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [DataContract]
    public class RoleMenu : BaseEntity
    {
        /// <summary>
        /// 菜单主键
        /// </summary>
        [DataMember]
        public string MenuId { get; set; }
        /// <summary>
        /// 角色主键
        /// </summary>
        [DataMember]
        public string RoleId { get; set; }
    }
}
