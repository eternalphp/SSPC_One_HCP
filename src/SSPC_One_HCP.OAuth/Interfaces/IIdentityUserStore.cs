using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.OAuth.Interfaces
{
    public interface IIdentityUserStore : IUserLoginStore<UserModel, string>, IIUserStore<UserModel>,IUserLoginStore<WxUserModel, string>,IIUserStore<WxUserModel>
    {
    }
}
