using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.OAuth.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using log4net;

namespace SSPC_One_HCP.OAuth.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILog _errLogger = LogManager.GetLogger("ErrorFileLogger");
        private readonly IEfRepository _efRepositiry;
        private readonly UserManager<UserModel> _userManager;
        private readonly UserManager<WxUserModel> _WxuserManager;
        public AuthRepository(IEfRepository efRepositiry, IIdentityUserStore userStore)
        {
            _efRepositiry = efRepositiry;
            _userManager = new UserManager<UserModel>(userStore);
            _WxuserManager = new UserManager<WxUserModel>(userStore);
        }

        public async Task<UserModel> FindUserSSO(string id, string aDAccount, string code, string employeeNo, string chineseName, string englishName, string companyCode)
        {
            try
            {
                var user = new UserModel();

                user = await _efRepositiry.FirstOrDefaultAsync<UserModel>(s => s != null && s.IsDeleted != 1 && (s.Id == id || s.ADAccount == aDAccount));
                if (user == null)
                {
                    user = new UserModel
                    {
                        Id = id,
                        ADAccount = aDAccount,
                        Code = code,
                        EmployeeNo = employeeNo,
                        ChineseName = chineseName,
                        EnglishName = englishName,
                        CompanyCode = companyCode,
                        IsDeleted = 0,
                        IsEnabled = 0,
                        Password = Guid.NewGuid().ToString(),
                    };
                    _efRepositiry.Insert<UserModel>(user);
                    _efRepositiry.SaveChanges();
                }
                else
                {
                    if (user.Id != id || user.ADAccount != aDAccount
                        || user.Code != code || user.EmployeeNo != employeeNo
                        || user.ChineseName != chineseName || user.EnglishName != englishName)
                    {
                        user.Id = id;
                        user.ADAccount = aDAccount;
                        // user.Code = code;
                        user.EmployeeNo = employeeNo;
                        user.ChineseName = chineseName;
                        user.EnglishName = englishName;
                        user.CompanyCode = companyCode;
                        _efRepositiry.Update(user);
                        _efRepositiry.SaveChanges();
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{ex.Message};\n");
                _errLogger.Error($"[Source]:{ex.Source}\n");
                _errLogger.Error($"[StackTrace]:{ex.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                _errLogger.Error($"[Method]: AuthRepository.FindUser \n");
                _errLogger.Error($"[IEfRepository]: _efRepositiry is null? {_efRepositiry == null} \n");
                _errLogger.Error($"[id]: {id} \n");
                _errLogger.Error($"[ADAccount]: {aDAccount} \n");
                _errLogger.Error($"--------------------------------------------------------------------------------");
            }
            return null;
        }

        public async Task<UserModel> FindUser(string userName, string password, string companyCode)
        {
            try
            {
                var user = await
                    _efRepositiry.FirstOrDefaultAsync<UserModel>(s => s != null && s.IsDeleted != 1 && (s.ADAccount == userName || s.Code == userName) && s.Password == password  /*&& s.CompanyCode == companyCode*/);

                return user;
            }
            catch (Exception ex)
            {
                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{ex.Message};\n");
                _errLogger.Error($"[Source]:{ex.Source}\n");
                _errLogger.Error($"[StackTrace]:{ex.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                _errLogger.Error($"[Method]: AuthRepository.FindUser \n");
                _errLogger.Error($"[IEfRepository]: _efRepositiry is null? {_efRepositiry == null} \n");
                _errLogger.Error($"[userName]: {userName} \n");
                _errLogger.Error($"[password]: {password} \n");
                _errLogger.Error($"[companyCode]: {companyCode} \n");
                _errLogger.Error($"--------------------------------------------------------------------------------");
            }
            return null;
        }

        public async Task<WxUserModel> FindWxUser(string id, string companyCode)
        {
            try
            {
                var user = new WxUserModel();
                user = await
                     _efRepositiry.FirstOrDefaultAsync<WxUserModel>(s => s.IsDeleted != 1 && s.Id == id);

                return user;
            }
            catch (Exception ex)
            {
                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{ex.Message};\n");
                _errLogger.Error($"[Source]:{ex.Source}\n");
                _errLogger.Error($"[StackTrace]:{ex.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                _errLogger.Error($"[Method]: AuthRepository.FindWxUser \n");
                _errLogger.Error($"[IEfRepository]: _efRepositiry is null? {_efRepositiry == null} \n");
                _errLogger.Error($"[UnionId]: {id} \n");
                _errLogger.Error($"[companyCode]: {companyCode} \n");
                _errLogger.Error($"--------------------------------------------------------------------------------");
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public UserModel FindUser(string userId, string companyCode)
        {

            var user =
                _efRepositiry.FirstOrDefault<UserModel>(s => s.IsDeleted != 1 && s.Id == userId && s.CompanyCode == companyCode);
            //TODO: 加入密码验证
            //var user = await _repository.FirstOrDefaultAsync<UserModel>(s => s.UserName == userName );

            return user;
        }

        public AppClientInfo FindClient(string clientId)
        {
            var appClientInfo = _efRepositiry.FirstOrDefault<AppClientInfo>(s => s.IsDeleted != 1 && s.AppName == clientId);
            return appClientInfo;
        }

        public Task<bool> AddRefreshToken(RefreshTokenInfo token)
        {
            _efRepositiry.Insert(token);
            return Task.FromResult(_efRepositiry.SaveChanges() > 0);
        }
        public Task<bool> RemoveRefreshToken(RefreshTokenInfo refreshToken)
        {
            if (refreshToken != null)
                _efRepositiry.Delete(refreshToken);
            return Task.FromResult(_efRepositiry.SaveChanges() > 0);
        }

        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshTokenInfo = _efRepositiry.FirstOrDefault<RefreshTokenInfo>(s => s.AppId == refreshTokenId);
            if (refreshTokenInfo != null)
                _efRepositiry.Delete(refreshTokenInfo);
            return Task.FromResult(_efRepositiry.SaveChanges() > 0);
        }
        public async Task<RefreshTokenInfo> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _efRepositiry.FirstOrDefaultAsync<RefreshTokenInfo>(s => s.TokenId == refreshTokenId);

            return refreshToken;
        }

        public List<RefreshTokenInfo> GetAllRefreshTokens()
        {
            //SingletonManager<SqlServerRepository>.Instance.ReadValues<RefreshTokenInfo>(
            //    "SELECT * FROM TS_RefreshTokenInfo");
            return _efRepositiry.All<RefreshTokenInfo>().ToList();
        }

        public async Task<UserModel> FindAsync(UserLoginInfo loginInfo)
        {
            var user = await _userManager.FindAsync(loginInfo);

            return user;
        }
        public async Task<IdentityResult> CreateAsync(UserModel user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
