using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 发现模块
    /// </summary>
    public class WxDiscoveryController : WxBaseApiController
    {
        private readonly IWxDiscoveryService _wxDiscoveryService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxDiscoveryService"></param>
        public WxDiscoveryController(IWxDiscoveryService wxDiscoveryService)
        {
            _wxDiscoveryService = wxDiscoveryService;
        }

        /// <summary>
        /// 获取发现首页的会议、学术知识
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult WxDisMainPage()
        {
            var ret = _wxDiscoveryService.WxDisMainPage(WorkUser);
            return Ok(ret);
        }
    }
}
