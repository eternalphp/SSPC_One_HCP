using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
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
    /// 用户角色
    /// </summary>
    [NotMapped]
    [DataContract]
    public class UserRoleViewModel : UserRole
    {
        /// <summary>
        /// 绑定的用户编码集合
        /// </summary>
        [DataMember]
        public List<string> SapCodeList { get; set; }
    }
}
