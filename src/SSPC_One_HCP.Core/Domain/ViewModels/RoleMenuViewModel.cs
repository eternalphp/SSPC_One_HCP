using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 权限菜单
    /// </summary>
    [NotMapped]
    [DataContract]
    public class RoleMenuViewModel : RoleMenu
    {
        /// <summary>
        /// 所选的菜单主键的集合
        /// </summary>
        [DataMember]
        public List<string> MenuIds { get; set; }
    }
}
