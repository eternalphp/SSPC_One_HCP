using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.InputDto;
using SSPC_One_HCP.KBS.OutDto;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.OAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers
{
    public class SSOController : ApiController
    {
        //static string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
        //static string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
        //static string OAuthAppSecret = ConfigurationManager.AppSettings["OAuthAppSecret"];
        //static string OAuthServerAuthorizeUrl = OAuthServerUrl + "/authorize";
        //static string OAuthServerTokenUrl = OAuthServerUrl + "/token";
        //static string OAuthServerRefreshUrl = OAuthServerUrl + "/refreshtoken";
        //static string OAuthServerUserInfoUrl = OAuthServerUrl + "/userinfo";
        //static string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
        //static string OAuthServerScope = ConfigurationManager.AppSettings["OAuthServerScope"];

        [HttpGet]
        public IHttpActionResult Index()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                string hostUrl = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}";
                string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
                string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
                string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
                string OAuthServerScope = ConfigurationManager.AppSettings["OAuthServerScope"];

                string HCPUrl = ConfigurationManager.AppSettings["HCPUIUrl"];
                //验证地址
                var redirect_uri = HCPUrl;//$"{hostUrl}/api/SSO/Callback";
                                          //登录获取Code地址
                var authorizeurl = $"{OAuthServerUrl}/authorize?client_id={OAuthAppId}&scope={OAuthServerScope}&response_type=code&state={OAuthServerState}&redirect_uri={redirect_uri}";
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = authorizeurl;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return Ok(rvm);
        }
        [HttpGet]
        public async Task<IHttpActionResult> Callback(string code, string state)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
                string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
                string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
                string OAuthAppSecret = ConfigurationManager.AppSettings["OAuthAppSecret"];
                string hostUrl = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}";
                //string hostUrl = $"http://13.90.59.215:9090/";
                //验证登录获取Code地址
                var redirect_uri = $"{hostUrl}/api/SSO/index";
                //code或者state为空则跳登录
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                //state值不对，跳登录
                if (!state.Equals(OAuthServerState, StringComparison.InvariantCultureIgnoreCase))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                //请求token
                var dic = new Dictionary<string, object>
            {
                { "code", code },
                { "client_id", OAuthAppId },
                { "client_secret", OAuthAppSecret },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirect_uri },
                { "state", state }
            };

                var ret = await new WebClient<SSOTokenOutDto>()
                       .Post($"{OAuthServerUrl}/token").Data(dic).ResultFromJsonAsync();

                if (ret == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (!ret.success)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (string.IsNullOrEmpty(ret.access_token) || string.IsNullOrEmpty(ret.refresh_token))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }

                //请求用户信息
                var infodata = await new WebClient<SSOUserInfoOutDto>()
                   .Post($"{OAuthServerUrl}/userinfo")
                   .Header("Authorization", $"{"Bear "}{ret.access_token}")
                   .Data(dic).ResultFromJsonAsync();

                if (infodata == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (!infodata.success)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }

                var user = await ContainerManager.Resolve<IAuthRepository>().FindUserSSO
                    (
                    infodata?.user_info?.Id,
                    infodata?.user_info?.ADAccount,
                    infodata?.user_info?.Code,
                    infodata?.user_info?.EmployeeNo,
                    infodata?.user_info?.ChineseName,
                    infodata?.user_info?.EnglishName,
                    infodata?.user_info?.CompanyCode
                    );

                dynamic obj = new ExpandoObject();


                //生成业务系统Token
                var buToken = await new WebClient<string>()
                     .Post($"{hostUrl}/auth/token")
                     .Data(
                            new Dictionary<string, object>
                            {
                            { "username", user.ADAccount },
                            { "password", user.Password },
                            { "grant_type", "password" },
                            { "CompanyCode", user.CompanyCode },
                            }
                    ).ResultAsync();
                var token = SSPC_One_HCP.KBS.Helpers.Json.ToObject<HCPTokenOutDto>(buToken.ToString());
                obj.BuToken = token;

                //生成KBS系统Token
                string kbsHost = ConfigurationManager.AppSettings["KBSUrl"];
                string loginSecretkey = ConfigurationManager.AppSettings["LoginSecretkey"];
                string sign = SSPC_One_HCP.KBS.Tool.Sign(new Dictionary<string, object>
            {
                { "Id", user.Id },
                { "Code", user.Code??"" },
                { "EmployeeNo", user.EmployeeNo??"" },
                { "ADAccount", user.ADAccount??"" },
                { "ChineseName", user.ChineseName??"" },
                { "EnglishName", user.EnglishName??"" },
                { "CompanyCode", user.CompanyCode??"" },
            }, loginSecretkey);

                var kbsToken = await new WebClient<Result>()
                        .Post($"{kbsHost}Account/Login")
                        .JsonData(new LoginInputDto
                        {
                            Sign = sign,
                            Id = user.Id,
                            Code = user.Code ?? "",
                            EmployeeNo = user.EmployeeNo ?? "",
                            ADAccount = user.ADAccount ?? "",
                            ChineseName = user.ChineseName ?? "",
                            EnglishName = user.EnglishName ?? "",
                            CompanyCode = user.CompanyCode ?? "",
                        }).ResultFromJsonAsync();

                if (kbsToken?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = kbsToken?.Message;
                    return Ok(rvm);
                }
                var tokenkbs = SSPC_One_HCP.KBS.Helpers.Json.ToObject<KBSTokenOutDto>(kbsToken.Data.ToString());
                obj.KbsToken = tokenkbs;

                var cache = ContainerManager.Resolve<ICacheManager>();
                cache.Set(user.Id.ToString() + "KBS", obj.KbsToken, 12);

                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = obj;
            }
            catch (Exception e)
            {

                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }

            return Ok(rvm);
        }


        [HttpGet]
        public IHttpActionResult WxIndex(string userId)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
                string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
                string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
                string OAuthServerScope = ConfigurationManager.AppSettings["OAuthServerScope"];

                var _loginConfirmUrl = ConfigurationManager.AppSettings["loginConfirmUrl"];

                var redirect_uri = $"{_loginConfirmUrl}/{userId}";
                //登录获取Code地址
                var authorizeurl = $"{OAuthServerUrl}/authorize?client_id={OAuthAppId}&scope={OAuthServerScope}&response_type=code&state={OAuthServerState}&redirect_uri={redirect_uri}";
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = authorizeurl;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return Ok(rvm);
        }
        [HttpGet]
        [Route("api/SSO/WxCallback/{userId}")]
        public async Task<IHttpActionResult> WxCallback(string userId, string code, string state)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                string OAuthServerUrl = ConfigurationManager.AppSettings["OAuthServerUrl"];
                string OAuthServerState = ConfigurationManager.AppSettings["OAuthServerState"];
                string OAuthAppId = ConfigurationManager.AppSettings["OAuthAppId"];
                string OAuthAppSecret = ConfigurationManager.AppSettings["OAuthAppSecret"];
                string _host = ConfigurationManager.AppSettings["HostUrl"];
                //验证登录获取Code地址
                var redirect_uri = $"{_host}/api/SSO/WxIndex?userId={userId}";
                //code或者state为空则跳登录
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                //state值不对，跳登录
                if (!state.Equals(OAuthServerState, StringComparison.InvariantCultureIgnoreCase))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                //请求token
                var dic = new Dictionary<string, object>
            {
                { "code", code },
                { "client_id", OAuthAppId },
                { "client_secret", OAuthAppSecret },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirect_uri },
                { "state", state }
            };

                var ret = await new WebClient<SSOTokenOutDto>()
                      .Post($"{OAuthServerUrl}/token").Data(dic).ResultFromJsonAsync();

                if (ret == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (!ret.success)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (string.IsNullOrEmpty(ret.access_token) || string.IsNullOrEmpty(ret.refresh_token))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }

                //请求用户信息
                var infodata = await new WebClient<SSOUserInfoOutDto>()
                   .Post($"{OAuthServerUrl}/userinfo")
                   .Header("Authorization", $"{"Bear "}{ret.access_token}")
                   .Data(dic).ResultFromJsonAsync();

                if (infodata == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }
                if (!infodata.success)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "登录失败";
                    return Ok(rvm);
                }

               // ContainerManager.Resolve<IAuthRepository>().UpdateWxUser(userId, infodata.user_info.Id);


                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = "";
            }
            catch (Exception e)
            {

                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }

            return Ok(rvm);
        }



    }
}
