using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 微信API实现
    /// </summary>
    public class WxOpenService : IWxOpenService
    {
        #region 声明
        private readonly IEfRepository _rep;
        private readonly IConfig _config;
        private readonly IWxRegisterService _wxRegisterService;
        //private readonly ICacheManager _cacheManager;
        private readonly string _host = ConfigurationManager.AppSettings["HostUrl"];
        private readonly string _clientId = ConfigurationManager.AppSettings["ClientId"];
        private readonly string _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep"></param>
        public WxOpenService(IEfRepository rep, IConfig config, IWxRegisterService wxRegisterService)
        {
            _rep = rep;
            _config = config;
            //_cacheManager = cacheManager;
            _wxRegisterService = wxRegisterService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ReturnValueModel FormID(TemplateForm form)
        {
            ReturnValueModel returnValue = new ReturnValueModel();
            var userid = form.CreateUser;
            if (string.IsNullOrEmpty(form.FormID) || string.IsNullOrEmpty(form.OpenID))
            {
                returnValue.Success = false;
                return returnValue;
            }
            form.Id = Guid.NewGuid().ToString();
            form.CreateTime = DateTime.Now;
            _rep.Insert(form);
            _rep.SaveChanges();
            returnValue.Success = true;
            returnValue.Result = form;
            return returnValue;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取UnionID
        /// </summary>
        /// <param name="wxUserModel"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ReturnValueModel GetUnionId(WxUserInfoRequestModel wxUserInfoRequestModel)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            //LoggerHelper.WriteLogInfo("[GetUnionId]:******获取UnionID开始******");
            ReturnValueModel rvm = new ReturnValueModel();
            var appId = _config.GetAppIdHcp();
            var appSecret = _config.GetAppSecretHcp();
            var url = string.Format(WxUrls.UnionIdUrl, appId, appSecret, wxUserInfoRequestModel.code);
            var openModel = JsonConvert.DeserializeObject<OpenModel>(HttpUtils.HttpGet(url, ""));
            //LoggerHelper.WriteLogInfo("[GetUnionId]:openModel.SessionKey------" + openModel.SessionKey);            
            if (string.IsNullOrEmpty(openModel?.SessionKey))
            {
                rvm.Success = false;
                rvm.Msg = "没有获取到SessionKey";
                rvm.Result = null;
                LoggerHelper.WriteLogInfo("[GetUnionId]:错误------没有获取到SessionKey");
                return rvm;
            }
            var encryptedData = wxUserInfoRequestModel.encryptedData;
            var iv = wxUserInfoRequestModel.iv;
            var openid = openModel.OpenId;
            var userinfo = new DecodedUserInfoModel() { openId = openid };


            //如果用户授权获取信息
            if (!string.IsNullOrEmpty(encryptedData) && !string.IsNullOrEmpty(iv))
            {
                userinfo = EncryptHelper.DecodeUserInfoBySessionKey(openModel.SessionKey, wxUserInfoRequestModel.encryptedData, wxUserInfoRequestModel.iv);
                if (string.IsNullOrEmpty(userinfo?.unionId) || string.IsNullOrEmpty(userinfo?.openId))
                {
                    LoggerHelper.WriteLogInfo("[GetUnionId]:错误------unionId无效或openid无效");
                    rvm.Success = false;
                    rvm.Msg = "unionId无效或openid无效";
                    rvm.Result = null;
                    return rvm;
                }
            }

            WxUserModel user = new WxUserModel();

          
            if (!string.IsNullOrEmpty(userinfo.unionId))
            {
                user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == userinfo.unionId);
            }
            else
            {
                user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.OpenId == openid);
            }

          
            #region 文库验证
            bool FKLogin = false; //是否显示FK登录页
            var isSalerPerson = 0;//是否销售
            var isReg = 0;//是否注册
            var isVerify = 0;
            var edaUrl = ConfigurationManager.AppSettings["WKUrl"] ?? "";
            //指定 1035
            if (wxUserInfoRequestModel.WxSceneId.Equals("1035") && wxUserInfoRequestModel.SourceAppId.Equals("wxeeefb3bc11af968d"))
            {
                var edaResult = HttpUtils.HttpGet(edaUrl + $"?Method=LoginCheck&unionid={userinfo.unionId}", "");
                //用户UnionID 访问文库接口判断是否有效
                if (edaResult.Equals("0"))
                {
                    //设置此人为销售
                    isSalerPerson = 1;
                    isReg = 1;
                    isVerify = 1;
                    FKLogin = false;
                }
                else
                {
                    isSalerPerson = 0;
                    isReg = 0;
                    isVerify = 0;
                    FKLogin = true;
                    //展示废卡登录页面                   
                }
            }
            #endregion
            if (user == null)
            {
                user = new WxUserModel()
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
                    IsDeleted = 0,
                    IsEnabled = 0,
                    IsVerify = isVerify,
                    IsCompleteRegister = isReg,
                    IsSalesPerson = isSalerPerson,
                    CreateTime = DateTime.Now,
                    SourceAppId = wxUserInfoRequestModel.SourceAppId,
                    SourceType = wxUserInfoRequestModel.SourceType,
                    WxSceneId = wxUserInfoRequestModel.WxSceneId,
                };
                _rep.Insert(user);
                _rep.SaveChanges();
            }
            else
            {


                //如果是销售 从费卡文库重新验证
                /*
                if ((user.IsSalesPerson ?? 0) == 1)
                {                    
                    var edaResult = HttpUtils.HttpGet(edaUrl + $"?Method=LoginCheck&unionid={userinfo.unionId}", "");
                    //用户UnionID 访问文库接口判断是否有效
                    if (edaResult.Equals("0"))
                    {
                        user.IsSalesPerson = 1;
                        user.IsCompleteRegister = 1;
                        user.IsVerify = 1;
                        FKLogin = false;
                    }
                    else
                    {
                        user.IsSalesPerson = 0;
                        user.IsCompleteRegister = 0;
                        user.IsVerify = 0;
                        FKLogin = true;
                    }

                }
                */
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
            _wxRegisterService.CacheWxUser(user);

            var postStr = $@"username={user.Id}&grant_type=password";
            var authPath = $@"{_host}/auth/token/Wx";
            //sysToken = JsonConvert.DeserializeObject<SysToken>(HttpUtils.HttpPost(authPath, postStr, "application/x-www-form-urlencoded"));
            SysToken sysToken = HttpUtils.PostResponse<SysToken>(authPath, postStr, "application/x-www-form-urlencoded");

            var issignup = 0;
            if (!string.IsNullOrEmpty(encryptedData) && !string.IsNullOrEmpty(iv))
            {
                //判断该用户是否完成签名
                issignup = _rep.Table<RegisterModel>().Where(s => s.WxUserId == user.Id).Count() > 0 ? 1 : 0;
            }

            #region 判断用户是否扫描了二维码
            if (wxUserInfoRequestModel.SourceAppId != null)
            {
                //向访问记录表推送数据
                QRcodeRecord addRecord = new QRcodeRecord();
                addRecord.Id = Guid.NewGuid().ToString();
                addRecord.AppId = wxUserInfoRequestModel.SourceAppId;
                addRecord.CreateTime = DateTime.Now;
                addRecord.CreateUser = user.Id;
                addRecord.UnionId = userinfo.unionId;
                addRecord.SourceType = wxUserInfoRequestModel.SourceType;
                addRecord.WxSceneId = wxUserInfoRequestModel.WxSceneId;
                //判断用户是否完成了注册
                addRecord.Isregistered = issignup == 0 ? 0 : 1;
                _rep.Insert(addRecord);
                _rep.SaveChanges();
            }
            #endregion
            #region 签到进入小程序记录 防止总总原因进入签到失败
            if (!string.IsNullOrEmpty(wxUserInfoRequestModel?.ActivityID))
            {
                try
                {
                    var edaCheckInRecord = new EdaCheckInRecord()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ActivityID = wxUserInfoRequestModel.ActivityID,
                        AppId = wxUserInfoRequestModel.SourceAppId,
                        UnionId = userinfo.unionId,
                        OpenId = userinfo.openId,
                        //UserName = workUser?.WxUser?.UserName,
                        WxName = userinfo.nickName,
                        VisitTime = DateTime.Now
                    };
                    _rep.Insert(edaCheckInRecord);
                    _rep.SaveChanges();
                }
                catch (Exception)
                {
                    LoggerHelper.WriteLogInfo("防止总总原因进入签到-----失败");
                }
            }

            #endregion

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                openModel = openModel,
                sysToken = sysToken,
                user = user,
                issignup = issignup,
                // isSalerPerson=isSalerPerson,
                FKLogin = FKLogin
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        public ReturnValueModel WKLogin(WKUser user)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var edaUrl = ConfigurationManager.AppSettings["WKUrl"] ?? "";
            rvm.Success = false;
            if (!string.IsNullOrEmpty(user.Unionid) && !string.IsNullOrEmpty(user.UserCode) && !string.IsNullOrEmpty(user.UserName))
            {
                var edaResult = HttpUtils.HttpGet(edaUrl + $"?Method=Login&unionid={user.Unionid}&loginName={user.UserName}&loginCode={user.UserCode}", "");
                if (edaResult.Equals("0"))
                {
                    var wxuser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == user.Unionid);
                    wxuser.IsSalesPerson = 1;
                    wxuser.IsCompleteRegister = 1;
                    wxuser.IsVerify = 1;
                    _rep.SaveChanges();
                    rvm.Success = true;
                }
            }
            return rvm;
        }

        public ReturnValueModel GetWXPhone(DecodePhoneModel phoneModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && (s.Id == phoneModel.userId || s.UnionId == phoneModel.unionId));
            if (user?.IsSalesPerson == 2)
            {
                rvm.Success = false;
                rvm.Msg = "员工请走内部员工通道";
                rvm.Result = null;
                return rvm;
            }


            var appId = _config.GetAppIdHcp();
            var appSecret = _config.GetAppSecretHcp();
            var url = string.Format(WxUrls.UnionIdUrl, appId, appSecret, phoneModel.code);
            var openModel = JsonConvert.DeserializeObject<OpenModel>(HttpUtils.HttpGet(url, ""));
            if (string.IsNullOrEmpty(openModel?.SessionKey))
            {
                rvm.Success = false;
                rvm.Msg = "没有获取到SessionKey";
                rvm.Result = null;
                return rvm;
            }
            var encryptedData = phoneModel.encryptedData;
            var iv = phoneModel.iv;
            if (string.IsNullOrEmpty(encryptedData) && string.IsNullOrEmpty(iv))
            {
                rvm.Success = false;
                rvm.Result = null;
                return rvm;
            }
            var phone = EncryptHelper.DecodeUserPhoneBySessionKey(openModel.SessionKey, phoneModel.encryptedData, phoneModel.iv);
            rvm.Success = true;
            rvm.Result = phone;
            return rvm;
        }
        #endregion
    }
}
