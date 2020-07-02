using Newtonsoft.Json;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.WxModels;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Core.WeChatManage;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot
{
    public class WxManageService : IWxManageService
    {
        private readonly IEfRepository _rep;
        private readonly IWxRegisterService _wxRegisterService;
        public WxManageService(IEfRepository rep, IWxRegisterService wxRegisterService)
        {
            _rep = rep;
            _wxRegisterService = wxRegisterService;
        }
        public ReturnValueModel GetSSOId(string userId)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            if (string.IsNullOrEmpty(userId))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'userId' is required.";
                return rvm;
            }
            //var user = _rep.FirstOrDefault<WxUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == userId);
            //rvm.Result = user?.SsoId;
            return rvm;
        }
        public ReturnValueModel GetWxUserInfo(WxManageInputDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };

            //string _host1 = ConfigurationManager.AppSettings["HostUrl"];
            //var authPath1 = $@"{_host1}/auth/token/Wx";
            //var postStr1 = $@"username=8e1731d9-ce48-4ef1-9522-087c9bd5076a&grant_type=password";
            //SysToken sysToken1 = HttpUtils.PostResponse<SysToken>(authPath1, postStr1, "application/x-www-form-urlencoded");
            //var user1 = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == "8e1731d9-ce48-4ef1-9522-087c9bd5076a");
            //_wxRegisterService.CacheWxUser(user1);//必须添加到内存
            //rvm.Result = sysToken1;
            //return rvm;
            if (string.IsNullOrEmpty(dto.AppId))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'appId' is required.";
                return rvm;
            }
            var configure = _rep.FirstOrDefault<BotSaleConfigure>(o => o.IsDeleted == 0 && o.AppId == dto.AppId);
            if (configure == null)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = "获取BOT信息失败，请先配置BOT";
                return rvm;
            }

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            var appId = configure.AppId;
            var appSecret = configure.AppSecret;
            var url = string.Format(WxUrls.UnionIdUrl, appId, appSecret, dto.code);
            var openModel = JsonConvert.DeserializeObject<OpenModel>(HttpUtils.HttpGet(url, ""));
            if (string.IsNullOrEmpty(openModel?.SessionKey))
            {
                rvm.Success = false;
                rvm.Msg = "没有获取到SessionKey";
                rvm.Result = null;
                LoggerHelper.WriteLogInfo("[GetUnionId]:错误------没有获取到SessionKey");
                return rvm;
            }
            var encryptedData = dto.encryptedData;
            var iv = dto.iv;
            var openid = openModel.OpenId;
            var wxUserInfo = dto.userInfo ?? new DecodedUserInfoModel();
            var userinfo = new DecodedUserInfoModel()
            {
                openId = openid,
                nickName = wxUserInfo?.nickName,
                city = wxUserInfo?.nickName,
                country = wxUserInfo?.country,
                gender = wxUserInfo.gender,
                avatarUrl = wxUserInfo?.avatarUrl,
                province = wxUserInfo?.province,

            };
            if (!string.IsNullOrEmpty(encryptedData) && !string.IsNullOrEmpty(iv))
            {

                //var d= EncryptHelper.DecodeEncryptedData_1(openModel.SessionKey, dto.encryptedData, dto.iv);
                //userinfo = EncryptHelper.DecodeUserInfoBySessionKey(openModel.SessionKey, dto.encryptedData, dto.iv);
                //if (string.IsNullOrEmpty(userinfo?.unionId) || string.IsNullOrEmpty(userinfo?.openId))
                //{
                //    LoggerHelper.WriteLogInfo("[GetUnionId]:错误------unionId无效或openid无效");
                //    rvm.Success = false;
                //    rvm.Msg = "unionId无效或openid无效";
                //    rvm.Result = null;
                //    return rvm;
                //}
            }

            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.OpenId == openid);
            if (user == null)
            {
                user = new WxUserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "",
                    OpenId = userinfo.openId,
                    //UnionId = openModel.UnionId,
                    //目前先使用OpenId，不知道为什么没获取到我的UnionId
                    UnionId = userinfo.unionId,
                    WxCity = userinfo.city,
                    WxName = userinfo.nickName,
                    WxCountry = userinfo.country,
                    WxGender = userinfo.gender.ToString(),
                    WxPicture = userinfo.avatarUrl,
                    WxProvince = userinfo.province,
                    //CreateTime = DateTime.Now,
                    //IsDeleted = 0,
                    //IsEnabled = 0,
                    IsVerify = 0,
                    IsCompleteRegister = 1,//必须要设置为1
                    IsSalesPerson = 1,
                    CreateTime = DateTime.Now,
                    //SourceAppId = wxUserInfoRequestModel.SourceAppId,
                    // SourceType = "0",
                    WxSceneId = dto.WxSceneId

                };
                _rep.Insert(user);
                _rep.SaveChanges();
            }
            else
            {
                user.OpenId = userinfo.openId ?? user.OpenId;
                user.UnionId = userinfo.unionId ?? user.UnionId;
                user.WxCity = userinfo.city ?? user.WxCity;
                user.WxName = userinfo.nickName ?? user.WxName;
                user.WxCountry = userinfo.country ?? user.WxCountry;
                user.WxGender = userinfo.gender.ToString() ?? user.WxGender;
                user.WxPicture = userinfo.avatarUrl ?? user.WxPicture;
                user.WxProvince = userinfo.province ?? user.WxProvince;
                user.UpdateTime = DateTime.Now;
                _rep.Update(user);
                _rep.SaveChanges();
            }
            _wxRegisterService.CacheWxUser(user);//必须添加到内存

            string _host = ConfigurationManager.AppSettings["HostUrl"];
            var authPath = $@"{_host}/auth/token/Wx";
            var postStr = $@"username={user.Id}&grant_type=password";
            SysToken sysToken = HttpUtils.PostResponse<SysToken>(authPath, postStr, "application/x-www-form-urlencoded");

            //string hostUrl = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}";
            string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
            string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
            string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
            string OAuthServerScope = ConfigurationManager.AppSettings["OAuthServerScope"];

            var _loginConfirmUrl = ConfigurationManager.AppSettings["loginConfirmUrl"];
            //验证地址
            var redirect_uri = $"{_loginConfirmUrl}/{user.Id}";
            //登录获取Code地址
            var authorizeurl = $"{OAuthServerUrl}/authorize?client_id={OAuthAppId}&scope={OAuthServerScope}&response_type=code&state={OAuthServerState}&redirect_uri={redirect_uri}";


            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                openModel = openModel,
                sysToken = sysToken,
                user = user,
                authorizeurl = authorizeurl,
                sysTokenUrl = authPath,
                username = user.Id,
                grant_type = "password"
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }
        /// <summary>
        /// 获取用户已获取的勋章 和 未获取的勋章
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public ReturnValueModel GetMedal(string appId, WorkUser workUser)
        {
            string _host = ConfigurationManager.AppSettings["HostUrl"];
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(appId))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'appId' is required.";
                return rvm;
            }
            var configure = _rep.FirstOrDefault<BotSaleConfigure>(o => o.IsDeleted == 0 && o.AppId == appId);
            if (configure == null)
            {
                rvm.Success = false;
                rvm.Msg = "获取BOT信息失败，请先配置BOT";
                return rvm;
            }

            List<SaleUserMedalOutDto> unlocklist = new List<SaleUserMedalOutDto>();
            //销售用户已经获取 勋章
            var saleUserMedalInfos = _rep.Where<BotSaleUserMedalInfo>(o => o.IsDeleted != 1 && o.SaleUserId == workUser.WxSaleUser.Id);
            foreach (var item in saleUserMedalInfos)
            {
                unlocklist.Add(new SaleUserMedalOutDto { name = item.MedalName, url = $"{_host}/{item.MedalSrc}" });
            }


            List<SaleUserMedalOutDto> locklist = new List<SaleUserMedalOutDto>();
            // 勋章次数
            var standardConfigures = _rep.Where<BotMedalStandardConfigure>(o => o.IsDeleted != 1 && o.KBSBotId == configure.KBSBotId);
            foreach (var item in standardConfigures)
            {
                var saleusermedalinfo = saleUserMedalInfos.FirstOrDefault(o=>o.BotMedalRuleId== item.Id);
                if (saleusermedalinfo==null)
                {
                    locklist.Add(new SaleUserMedalOutDto { name = item.MedalName, url = $"{_host}/{item.MedalNSrc}" });
                }
            }

            var businessConfigures = _rep.Where<BotMedalBusinessConfigure>(o => o.IsDeleted != 1 && o.KBSBotId == configure.KBSBotId);
            foreach (var item in businessConfigures)
            {
                var saleusermedalinfo = saleUserMedalInfos.FirstOrDefault(o => o.BotMedalRuleId == item.Id);
                if (saleusermedalinfo == null)
                {
                    locklist.Add(new SaleUserMedalOutDto { name = item.MedalName, url = $"{_host}/{item.MedalNSrc}" });
                }
            }

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new { unlocklist, locklist };
            return rvm;
        }


       
    }
}
