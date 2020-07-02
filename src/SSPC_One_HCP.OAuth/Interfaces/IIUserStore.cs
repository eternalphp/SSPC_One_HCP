using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.OAuth.Interfaces
{
    public partial interface IIUserStore<TUser> : IUserStore<TUser, string>, IDisposable where TUser : class, IUser<string>
    {
    }
}
