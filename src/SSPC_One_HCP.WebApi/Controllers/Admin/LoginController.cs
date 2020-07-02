using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
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
    /// 登录
    /// </summary>
    public class LoginController : ApiController
    {
        private readonly ILoginService _loginService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginService"></param>
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        /// <summary>
        /// 邮件链接登录
        /// </summary>
        /// <param name="info">信息加密字符串</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public IHttpActionResult MailLogin([FromUri]string info)
        {
            var ret = _loginService.MailLogin(info);
            return Ok(ret);
        }
        /// <summary>
        /// UN登录
        /// </summary>
        /// <param name="info">信息加密字符串</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public IHttpActionResult UnLogin([FromUri] string info)
        {
            var ret = _loginService.UnLogin(info);
            return Ok(ret);
        }
        /// <summary>
        /// 域账户登录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LoginSys(UserModel userModel)
        {
            var ret = _loginService.LoginSys(userModel);
            return Ok(ret);
        }
        /// <summary>
        /// 修改账户密码
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdatePassword(UserModel userModel) {
            var ret = _loginService.UpdatePassword(userModel);
            return Ok(ret);
        }
    }
}
