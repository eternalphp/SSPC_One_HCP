using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 主页接口
    /// </summary>
    public class HomeController : BaseApiController
    {
        private readonly ICommonService _commonService;

        /// <summary>
        /// 主页接口
        /// </summary>
        /// <param name="commonService"></param>
        public HomeController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetCurrentUser()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var user = WorkUser;

            if (user == null)
            {
                rvm.Msg = "No user information.";
                rvm.Success = false;
                return Ok(rvm);
            }

            rvm.Success = true;
            rvm.Result = new {
                user.User?.Id,
                user.User?.ADAccount,
                user.User?.CompanyCode,
                user.User?.ChineseName,
                user.User?.EnglishName,
                user.User?.MobileNo,
                user.User?.PersonalEmail,
                user.User?.CompanyEmail,
                OrganizationName = user.Organization?.Name,
                user.Organization?.BuName,
                isAdmin = _commonService.IsAdmin(user)
            };
            return Ok(rvm);
        }
    }
}
