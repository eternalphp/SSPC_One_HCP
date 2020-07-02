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
    /// 菜单展示
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MenuInfoViewModel : MenuInfo
    {
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        [DataMember]
        public string ParentMenuName { get; set; }
    }
}
