using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    public class WxManageController : WxSaleBaseApiController
    {

        private readonly IWxManageService _wxManageService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxManageService"></param>
        public WxManageController(IWxManageService wxManageService)
        {
            _wxManageService = wxManageService;
        }
        /// <summary>
        /// 小程序获取用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetWxUserInfo([FromBody]WxManageInputDto dto)
        {
            var ret = _wxManageService.GetWxUserInfo(dto);
            return Ok(ret);
        }
        /// <summary>
        /// 获取用户SSOID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetSSOId(string userId)
        {
            var ret = _wxManageService.GetSSOId(userId);
            return Ok(ret);
        }

        /// <summary>
        /// 获取用户已获取的勋章 和 未获取的勋章
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMedal(string appId)
        {
            var ret = _wxManageService.GetMedal(appId, WorkUser);
            return Ok(ret);
        }
    }
}
