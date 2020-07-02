using Newtonsoft.Json;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Core.WeChatManage;
using SSPC_One_HCP.KBS.Helpers;
using SSPC_One_HCP.Services.Services.WebView.Dto;
using SSPC_One_HCP.Services.Services.WebView.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WebView.Implementations
{
    /// <summary>
    /// H5-参会报名表
    /// </summary>
    public class RegistrationFormService : IRegistrationFormService
    {
        private readonly IEfRepository _rep;
        private readonly IConfig _config;
        public RegistrationFormService(IEfRepository rep, IConfig config)
        {
            _rep = rep;
            _config = config;
        }
        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetUserInfo(string code)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var appId = _config.GetAppIdDF();
            var appSecret = _config.GetAppSecretDF();
            var accessTokenUrl = string.Format(WxUrls.AccessTokenUrl, appId, appSecret, code);
            var wxAccessTokenDto = JsonConvert.DeserializeObject<WxAccessTokenDto>(HttpUtils.HttpGet(accessTokenUrl, ""));

            if (!string.IsNullOrEmpty(wxAccessTokenDto?.errcode))
            {
                rvm.Success = false;
                rvm.Msg = wxAccessTokenDto?.errcode;
                rvm.Result = wxAccessTokenDto;
                return rvm;
            }

            var userinfoUrl = string.Format(WxUrls.UserinfoUrl, wxAccessTokenDto.access_token, wxAccessTokenDto.openid);
            var wxUserInfoDto = JsonConvert.DeserializeObject<WxUserInfoDto>(HttpUtils.HttpGet(userinfoUrl, ""));
            if (!string.IsNullOrEmpty(wxUserInfoDto?.errcode))
            {
                rvm.Success = false;
                rvm.Msg = wxUserInfoDto?.errcode;
                rvm.Result = wxUserInfoDto;
                return rvm;
            }
            //LoggerHelper.WriteLogInfo("[BBBBBBBBB]" + code);
            //LoggerHelper.WriteLogInfo("[BBBBBBBBB]" + wxUserInfoDto.Unionid);
            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == wxUserInfoDto.Unionid);
            rvm.Success = true;
            rvm.Result = new
            {
                WxUserInfo = new
                {
                    Openid = wxUserInfoDto.Openid,
                    WxNickname = wxUserInfoDto.Nickname,
                    WxSex = wxUserInfoDto.Sex,
                    WxProvince = wxUserInfoDto.Province,
                    WxCity = wxUserInfoDto.City,
                    WxCountry = wxUserInfoDto.Country,
                    WxPicture = wxUserInfoDto.Headimgurl,
                    Unionid = wxUserInfoDto.Unionid,
                },
                HcpUserInfo = new
                {
                    UserName = user?.UserName,
                    RegistrationAge = user?.RegistrationAge,
                    RegistrationGender = user?.RegistrationGender,
                    Title = user?.Title,
                    HospitalName = user?.HospitalName,
                    DepartmentName = user?.DepartmentName,
                    RegistrationIsBasicLevel = user?.RegistrationIsBasicLevel,
                    Province = user?.Province,
                    City = user?.City,
                    Area = user?.Area,
                    Mobile = user?.Mobile,
                    SourceAppId = user?.SourceAppId,
                    WxSceneId = user?.WxSceneId,
                },
                IsRegister = user?.RegistrationIsBasicLevel == null ? false : true,

            };
            return rvm;
        }
        /// <summary>
        /// 新增修改 参会报名表
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdate(RegistrationFormInputDto inputDto)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Success = true,
                Msg = "",

            };
            if (string.IsNullOrEmpty(inputDto.Unionid))
            {
                rvm.Success = false;
                rvm.Msg = "Unionid为空";
                rvm.Result = "表单已过期请重新刷新页面";
                return rvm;
            }
            //LoggerHelper.WriteLogInfo("[AAAAAAA]" + inputDto.Unionid /*Json.ToJson(inputDto)*/);
            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == inputDto.Unionid);
            if (user == null)
            {
                user = new WxUserModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = inputDto.UserName,
                    RegistrationAge = inputDto.RegistrationAge,
                    RegistrationGender = inputDto.RegistrationGender,
                    Title = inputDto.Title,
                    HospitalName = inputDto.HospitalName,
                    DepartmentName = inputDto.DepartmentName,
                    RegistrationIsBasicLevel = inputDto.RegistrationIsBasicLevel,
                    Province = inputDto.Province,
                    City = inputDto.City,
                    Area = inputDto.Area,
                    Mobile = $"{inputDto.Mobile}_H5",
                    SourceAppId = inputDto.SourceAppId,
                    SourceType = "5",
                    //OpenId = inputDto.Openid,
                    WxSceneId = inputDto.WxSceneId,
                    UnionId = inputDto.Unionid,
                    WxCity = inputDto.WxCity,
                    WxName = inputDto.WxNickname,
                    WxCountry = inputDto.WxCountry,
                    WxGender = inputDto.WxSex.ToString(),
                    WxPicture = inputDto.WxPicture,
                    WxProvince = inputDto.WxProvince,
                    CreateTime = DateTime.Now,
                    IsDeleted = 0,
                    IsEnabled = 0,
                    IsVerify = 5,
                    IsCompleteRegister = 1,
                    IsSalesPerson = 0,

                };
                _rep.Insert(user);
                _rep.SaveChanges();
            }
            else
            {
                user.UserName = inputDto.UserName;
                user.RegistrationAge = inputDto.RegistrationAge;
                user.RegistrationGender = inputDto.RegistrationGender;
                user.Title = inputDto.Title;
                user.HospitalName = inputDto.HospitalName;
                user.DepartmentName = inputDto.DepartmentName;
                user.RegistrationIsBasicLevel = inputDto.RegistrationIsBasicLevel;
                user.Province = inputDto.Province;
                user.City = inputDto.City;
                user.Area = inputDto.Area;
                user.Mobile = $"{inputDto.Mobile}_H5";
                user.SourceAppId = inputDto.SourceAppId;
                // user.OpenId = inputDto.Openid;
                user.WxSceneId = inputDto.WxSceneId;
                // user.UnionId = inputDto.Unionid;
                user.WxCity = inputDto.WxCity;
                user.WxName = inputDto.WxNickname;
                user.WxCountry = inputDto.WxCountry;
                user.WxGender = inputDto.WxSex.ToString();
                user.WxPicture = inputDto.WxPicture;
                user.WxProvince = inputDto.WxProvince;
                user.UpdateTime = DateTime.Now;
                _rep.Update(user);
                _rep.SaveChanges();
            }



            rvm.Result = user;
            return rvm;
        }
    }
}
