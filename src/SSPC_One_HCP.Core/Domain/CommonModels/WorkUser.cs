using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 操作用户
    /// </summary>
    public class WorkUser
    {
        /// <summary>
        /// 微信用户基本信息
        /// </summary>
        public WxUserModel WxUser { get; set; }
        /// <summary>
        /// 小程序销售用户基本信息
        /// </summary>
        public WxSaleUserModel WxSaleUser { get; set; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// 用户角色基本信息
        /// </summary>
        public List<RoleInfo> Roles { get; set; }

        /// <summary>
        /// 用户所在的组织
        /// </summary>
        public Organization Organization { get; set; }
    }
}
