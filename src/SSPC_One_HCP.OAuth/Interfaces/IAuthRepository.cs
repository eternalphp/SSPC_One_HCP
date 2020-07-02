using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.OAuth.Interfaces
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<bool> AddRefreshToken(RefreshTokenInfo token);
        Task<IdentityResult> CreateAsync(UserModel user);
        Task<UserModel> FindAsync(UserLoginInfo loginInfo);
        AppClientInfo FindClient(string clientId);
        Task<RefreshTokenInfo> FindRefreshToken(string refreshTokenId);
        Task<UserModel> FindUserSSO(string id, string aDAccount, string code, string employeeNo, string chineseName, string englishName, string companyCode);

        Task<UserModel> FindUser(string userName, string password, string companyCode);

        Task<WxUserModel> FindWxUser(string UnionId, string companyCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        UserModel FindUser(string userId, string companyCode);
        List<RefreshTokenInfo> GetAllRefreshTokens();
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshTokenInfo refreshToken);
    }
}
