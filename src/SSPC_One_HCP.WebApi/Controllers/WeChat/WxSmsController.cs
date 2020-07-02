using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 短信验证码
    /// </summary>
    public class WxSmsController : ApiController
    {
        private readonly ISmsService _smsService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="smsService"></param>
        public WxSmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        ///// <summary>
        ///// 发送验证码
        ///// </summary>
        ///// <param name="wxUserModel">传入Mobile</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult SendSms(WxUserModel wxUserModel)
        //{
        //    var ret = _smsService.SendSms(wxUserModel).Result;
        //    return Ok(ret);
        //}
        ///// <summary>
        ///// 验证码是否正确
        ///// </summary>
        ///// <param name="wxUserModel">传入Code、Mobile</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult VerifySmsCode(WxUserModel wxUserModel)
        //{
        //    var ret = _smsService.VerifySmsCode(wxUserModel).Result;
        //    return Ok(ret);
        //}
    }
}
