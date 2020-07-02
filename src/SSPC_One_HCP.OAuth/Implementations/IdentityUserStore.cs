using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.OAuth.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.OAuth.Implementations
{
    public class IdentityUserStore : IIdentityUserStore
    {
        private readonly IEfRepository _efRepositiry;

        public IdentityUserStore(IEfRepository efRepositiry)
        {
            _efRepositiry = efRepositiry;
        }
        public void Dispose()
        {
            _efRepositiry.Dispose();
        }

        public Task CreateAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _efRepositiry.Insert(user);
            return Task.FromResult(_efRepositiry.SaveChanges());
        }

        public Task UpdateAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _efRepositiry.Update(user);
            return Task.FromResult(_efRepositiry.SaveChanges());
        }

        public Task DeleteAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _efRepositiry.Delete(user);
            return Task.FromResult(_efRepositiry.SaveChanges());
        }

        public Task<UserModel> FindByIdAsync(string userId)
        {
            var user = _efRepositiry.FirstOrDefault<UserModel>(u => u.Id.Equals(userId));
            return Task.FromResult(user);
        }

        public Task<UserModel> FindByNameAsync(string userName)
        {
            var user = _efRepositiry.FirstOrDefault<UserModel>(u => u.UserName.ToUpper() == userName.ToUpper());
            return Task.FromResult(user);
        }

        public Task AddLoginAsync(UserModel user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            this._efRepositiry.Insert<AccountLoginInfo>(new AccountLoginInfo
            {
                UserId = user.Id.ToString(),
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            });
            var result = _efRepositiry.SaveChanges();
            return Task.FromResult(result);
        }

        public Task RemoveLoginAsync(UserModel user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(WxUserModel user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(WxUserModel user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(WxUserModel user)
        {
            throw new NotImplementedException();
        }

        Task<WxUserModel> IUserLoginStore<WxUserModel, string>.FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(WxUserModel user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(WxUserModel user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(WxUserModel user)
        {
            throw new NotImplementedException();
        }

        Task<WxUserModel> IUserStore<WxUserModel, string>.FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<WxUserModel> IUserStore<WxUserModel, string>.FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
