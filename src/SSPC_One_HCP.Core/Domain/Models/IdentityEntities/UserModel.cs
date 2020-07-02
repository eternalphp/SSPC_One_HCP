using SSPC_One_HCP.Core.Domain.Models.DataModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSPC_One_HCP.Core.Domain.Models.IdentityEntities
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserModel : UserInfo, IUser<string>
    {
        /// <summary>
        /// 此处废弃
        /// </summary>
        public string UserName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WxUserModel : DoctorModel, IUser<string>
    {

    }
    public class WxSaleUserModel : BotSaleUserInfo, IUser<string>
    {
        public string UserName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleView
    {
        public string key { get; set; }
        public string label { get; set; }
    }
}
