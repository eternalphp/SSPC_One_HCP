using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.OAuth.Implementations
{
    public class UserManager<TUser> : UserManager<TUser, string> where TUser : class, IUser<string>
    {
        public UserManager(IUserStore<TUser, string> store) : base(store)
        {
        }
    }
}
