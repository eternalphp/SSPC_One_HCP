using Newtonsoft.Json;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.WxModels;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Core.WeChatManage;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.Helpers;
using SSPC_One_HCP.KBS.InputDto;
using SSPC_One_HCP.KBS.OutDto;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.Bot
{
    public class ADVerifyService : IADVerifyService
    {
        private readonly IEfRepository _rep;
        private readonly IWxRegisterService _wxRegisterService;
        private readonly ICacheManager _cacheManager;
        public ADVerifyService(IEfRepository rep, IWxRegisterService wxRegisterService, ICacheManager cacheManager)
        {
            _rep = rep;
            _wxRegisterService = wxRegisterService;
            _cacheManager = cacheManager;

        }
        /// <summary>
        /// 管理后台AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> AdminVerifyAsync(VerifyAdminInputDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            try
            {
                //if (string.IsNullOrEmpty(dto?.Value))
                //{
                //    rvm.Success = false;
                //    rvm.Msg = "请输入账号和密码。";
                //    return rvm;
                //}
                //string a = Encoding.Default.GetString(Convert.FromBase64String(dto.Value));
                //string pattern = @"(\\[^bfrnt\\/‘\""])";
                //var result = System.Text.RegularExpressions.Regex.Replace(a, pattern, "\\$1");
                //VerifyBase verifyAdmin = Json.ToObject<VerifyBase>(result);

                if (string.IsNullOrEmpty(dto?.UserName))
                {
                    rvm.Success = false;
                    rvm.Msg = "请输入账号。";
                    return rvm;
                }
                if (string.IsNullOrEmpty(dto?.Password))
                {
                    rvm.Success = false;
                    rvm.Msg = "请输入密码。";
                    return rvm;
                }

                var user = await _rep.FirstOrDefaultAsync<UserModel>(s => s != null && s.IsDeleted != 1 && s.ADAccount == dto.UserName.ToUpper());
                //是否系统管理员
                if (user?.Id == "00000000-0000-0000-0000-000000000000")
                {
                    if (user?.Password != dto.Password)
                    {
                        rvm.Success = false;
                        rvm.Msg = "登录失败。";
                        return rvm;
                    }
                }
                else
                {
                    //匹配AD白名单
                    var whiteName = await _rep.FirstOrDefaultAsync<BotADWhiteName>(s => s.IsDeleted != 1 && s.ADAccount == dto.UserName);
                    //var isWhiteName = await CheckWhiteName(dto.UserName);
                    if (whiteName == null)
                    {
                        rvm.Success = false;
                        rvm.Msg = "您输入的账号无权限访问或账号密码不正确。";
                        rvm.Result = null;
                        LoggerHelper.WriteLogInfo("[CheckWhiteName]:错误------不在白名单");
                        return rvm;
                    }

                    //验证AD域
                    var verify = await GetVerifyApi(dto.UserName, dto?.Password);
                    if (verify.Success == false) return verify;

                    //var user = await _rep.FirstOrDefaultAsync<UserModel>(s => s != null && s.IsDeleted != 1 && s.ADAccount == dto.UserName.ToUpper());
                    if (user == null)
                    {
                        var id = Guid.NewGuid().ToString();
                        user = new UserModel
                        {
                            Id = id,
                            ADAccount = dto.UserName.ToUpper(),
                            Code = id,
                            IsDeleted = 0,
                            IsEnabled = 0,
                            Password = Guid.NewGuid().ToString(),
                            CreateTime = DateTime.UtcNow.AddHours(8),
                            CreateUser = id,
                        };
                        _rep.Insert<UserModel>(user);
                    }
                    else
                    {
                        if (user.ADAccount != dto.UserName)
                        {
                            user.ADAccount = dto.UserName.ToUpper();
                            _rep.Update(user);
                        }
                    }
                    string roleId = "002222B5-C4D4-4DD7-9FEE-53201BD2BA55";
                    var userRole = await _rep.FirstOrDefaultAsync<UserRole>(s => s.IsDeleted != 1 && s.UserId == user.Id && s.RoleId == roleId);
                    //添加审核员权限
                    if (whiteName.ChatAudit == 1 && userRole == null)
                    {
                        _rep.Insert<UserRole>(new UserRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            SapCode = user.Code,
                            UserId = user.Id,
                            RoleId = roleId,
                            CreateTime = DateTime.UtcNow.AddHours(8),
                            CreateUser = user.Id,
                        });
                    }
                    _rep.SaveChanges();
                }
                string _host = ConfigurationManager.AppSettings["HostUrl"];
                var buToken = await new WebClient<string>()
                                          .Post($"{_host}/auth/token")
                                          .Data(new Dictionary<string, object>
                                          {
                                           { "username", user.ADAccount },
                                           { "password", user.Password },
                                           { "grant_type", "password" },
                                           { "CompanyCode", user.CompanyCode??"" },
                                          }).ResultAsync();

                if (string.IsNullOrEmpty(buToken.ToString()))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "获取HCP，token失败" + _host;
                    return rvm;
                }
                dynamic obj = new ExpandoObject();
                obj.BuToken = Json.ToObject<HCPTokenOutDto>(buToken.ToString());

                //生成KBS系统Token
                string kbsHost = ConfigurationManager.AppSettings["KBSUrl"];
                string loginSecretkey = ConfigurationManager.AppSettings["LoginSecretkey"];
                string sign = Tool.Sign(new Dictionary<string, object>
                {
                 { "Id", user.Id }
                }, loginSecretkey);
                var kbsToken = await new WebClient<Result>()
                                          .Post($"{kbsHost}Account/LoginAD")
                                          .JsonData(new LoginADInputDto
                                          {
                                              Sign = sign,
                                              Id = user.Id
                                          }).ResultFromJsonAsync();
                if (kbsToken?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = kbsToken?.Message;
                    return rvm;
                }
                obj.KbsToken = Json.ToObject<KBSTokenOutDto>(kbsToken.Data.ToString());
                _cacheManager.Set(user.Id.ToString() + "KBS", obj.KbsToken, 12);

                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = obj;
            }
            catch (Exception ex)
            {
                string error = string.Empty;
                error += ($"--------------------------------------------------------------------------------");
                error += ($"[MSG]:{ex.Message};\n");
                error += ($"[Source]:{ex.Source}\n");
                error += ($"[StackTrace]:{ex.StackTrace}\n");
                error += ($"[StackTrace]:{ex.TargetSite.Name}\n");
                error += ($"--------------------------------------------------------------------------------");
                rvm.Msg = "fail_";
                rvm.Success = false;
                rvm.Result = error;
            }

            return rvm;
        }

        /// <summary>
        /// 验证小程序AD是否授权过
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> WxVerify(AuthorizedOrNotInputDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            try
            {
                var configure = await _rep.FirstOrDefaultAsync<BotSaleConfigure>(o => o.IsDeleted == 0 && o.AppId == dto.appid);
                if (configure == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                    return rvm;
                }
                if (string.IsNullOrEmpty(configure.AppId))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                    return rvm;
                }
                if (string.IsNullOrEmpty(configure.AppSecret))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                    return rvm;
                }

                var appId = configure.AppId;
                var appSecret = configure.AppSecret;
                var url = string.Format(WxUrls.UnionIdUrl, appId, appSecret, dto.code);
                var openModel = JsonConvert.DeserializeObject<OpenModel>(HttpUtils.HttpGet(url, ""));
                if (string.IsNullOrEmpty(openModel?.OpenId))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = new
                    {
                        sysTokenUrl = "",
                        sysToken = "",
                        username = "",
                        grant_type = "",
                        verify = false,
                        openId = "openid失败，重新授权"
                    };
                    return rvm;
                }
                var query = _rep.Where<WxSaleUserModel>(o => o != null && o.IsDeleted != 1);
                if (!string.IsNullOrEmpty(dto?.username))
                {
                    query = query.Where(o => o.Id == dto.username);
                }
                else
                {
                    query = query.Where(o => o.OpenId == openModel.OpenId);
                }

                var saleUser = query.FirstOrDefault<WxSaleUserModel>();

                if (saleUser == null)
                {
                    rvm.Success = true;
                    rvm.Msg = "success";
                    rvm.Result = new
                    {
                        sysTokenUrl = "",
                        sysToken = "",
                        username = "",
                        grant_type = "",
                        verify = false,
                        openId = openModel.OpenId
                    };
                    return rvm;
                }
                if (string.IsNullOrEmpty(saleUser.ADAccount))
                {
                    rvm.Msg = "NOT_LOGIN";
                    rvm.Success = false;
                    rvm.Result = new
                    {
                        sysTokenUrl = "",
                        sysToken = "",
                        username = "",
                        grant_type = "",
                        verify = false,
                        openId = openModel.OpenId
                    };
                    return rvm;
                }
                _wxRegisterService.CacheWxSaleUser(saleUser);//必须添加到内存

                string _host = ConfigurationManager.AppSettings["HostUrl"];
                var authPath = $@"{_host}/api/auth/token/WxSale";
                var postStr = $@"username={saleUser.Id}&grant_type=password";
                SysToken sysToken = HttpUtils.PostResponse<SysToken>(authPath, postStr, "application/x-www-form-urlencoded");
                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = new
                {
                    sysTokenUrl = authPath,
                    sysToken,//管理平台 token
                    username = saleUser.Id,
                    grant_type = "password",
                    verify = true,
                    openId = openModel.OpenId
                };
            }
            catch (Exception ex)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = ex.Message;
            }

            return rvm;
        }
        /// <summary>
        /// 小程序AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> GetSaleUserInfo(VerifyInputDto dto)
        {

            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.ADAccount))
            {
                rvm.Success = false;
                rvm.Msg = "fail";
                rvm.Result = "请输入账号。";
                return rvm;
            }
            if (string.IsNullOrEmpty(dto?.Password))
            {
                rvm.Success = false;
                rvm.Msg = "fail";
                rvm.Result = "请输入密码。";
                return rvm;
            }
            if (string.IsNullOrEmpty(dto?.openId))
            {
                rvm.Success = false;
                rvm.Msg = "fail";
                rvm.Result = "请输入密码。";
                return rvm;
            }

            var encryptedData = dto.encryptedData;
            var iv = dto.iv;

            //小程序用户基本信息
            var wxUserInfo = dto.userInfo ?? new DecodedUserInfoModel();

            //匹配AD白名单
            var isWhiteName = await CheckWhiteName(dto.ADAccount);
            if (!isWhiteName)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = "您输入的账号无权限访问。";
                LoggerHelper.WriteLogInfo("[CheckWhiteName]:错误------不在白名单");
                return rvm;
            }

            //验证AD域
            var verify = await GetVerifyApi(dto.ADAccount, dto?.Password);
            if (verify.Success == false) return verify;

            var saleADAccountUser = _rep.FirstOrDefault<WxSaleUserModel>(s => s.IsDeleted != 1 && s.ADAccount == dto.ADAccount.ToUpper());
            if (saleADAccountUser != null)
            {
                saleADAccountUser.ADAccount = null;
                saleADAccountUser.Remark = dto.ADAccount.ToUpper();
                _rep.Update(saleADAccountUser);
            }
            var saleUser = _rep.FirstOrDefault<WxSaleUserModel>(s => s.IsDeleted != 1 && s.OpenId == dto.openId);
            if (saleUser == null)
            {
                saleUser = new WxSaleUserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    OpenId = dto.openId,
                    UnionId = wxUserInfo.unionId,
                    WxCity = wxUserInfo.city,
                    WxName = wxUserInfo.nickName,
                    WxCountry = wxUserInfo.country,
                    WxGender = wxUserInfo.gender.ToString(),
                    WxPicture = wxUserInfo.avatarUrl,
                    WxProvince = wxUserInfo.province,
                    CreateTime = DateTime.Now,
                    ADAccount = dto.ADAccount.ToUpper(),
                };
                _rep.Insert(saleUser);
                _rep.SaveChanges();
            }
            else
            {
                saleUser.OpenId = dto.openId;
                saleUser.UnionId = wxUserInfo.unionId;
                saleUser.WxCity = wxUserInfo.city;
                saleUser.WxName = wxUserInfo.nickName;
                saleUser.WxCountry = wxUserInfo.country;
                saleUser.WxGender = wxUserInfo.gender.ToString();
                saleUser.WxPicture = wxUserInfo.avatarUrl;
                saleUser.WxProvince = wxUserInfo.province;
                saleUser.UpdateTime = DateTime.Now;
                saleUser.ADAccount = dto.ADAccount.ToUpper();
                saleUser.Remark = null;
                _rep.Update(saleUser);
                _rep.SaveChanges();

            }
            _wxRegisterService.CacheWxSaleUser(saleUser);//必须添加到内存

            string _host = ConfigurationManager.AppSettings["HostUrl"];
            var authPath = $@"{_host}/api/auth/token/WxSale";
            var postStr = $@"username={saleUser.Id}&grant_type=password";
            SysToken sysToken = HttpUtils.PostResponse<SysToken>(authPath, postStr, "application/x-www-form-urlencoded");
            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                sysTokenUrl = authPath,
                sysToken,//管理平台 token
                username = saleUser.Id,
                grant_type = "password",
                verify = true,
            };
            return rvm;
        }


        async Task<ReturnValueModel> GetVerifyApi(string adaccount, string password)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(password);
                //转成 Base64 形式的 System.String  

                var pwd = Convert.ToBase64String(b);
                string _host = ConfigurationManager.AppSettings["ADVerifyUrl"];
                var result = await new WebClient<AdResult>()
                            .Post(_host)
                            .JsonData(new { adaccount = adaccount, password = pwd })
                            .ResultFromJsonAsync();
                //成功
                if (result?.Error_code == 0)
                {
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = "成功";
                    return rvm;
                }
                if (result?.Error_code == -1)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;

                }
                if (result?.Error_code == 10001001)
                {
                    rvm.Success = false;
                    rvm.Msg = "数据库操作失败";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031001)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031004)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031011)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031012)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                else
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = "系统忙，请稍后再试！";
            }
            return rvm;
        }

        /// <summary>
        /// 验证白名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async Task<bool> CheckWhiteName(string adaccount)
        {
            var user = await _rep.FirstOrDefaultAsync<BotADWhiteName>(s => s.IsDeleted != 1 && s.ADAccount == adaccount);
            return user == null ? false : true;
        }
    }
}
