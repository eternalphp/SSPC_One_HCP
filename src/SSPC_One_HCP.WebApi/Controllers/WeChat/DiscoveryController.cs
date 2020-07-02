using SSPC_One_HCP.Services.Interfaces;
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
    public class DiscoveryController : WxBaseApiController
    {
        private readonly IWxDiscoveryService _discoveryService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DiscoveryController.DiscoveryController(IWxDiscoveryService)'
        public DiscoveryController(IWxDiscoveryService discoveryService)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DiscoveryController.DiscoveryController(IWxDiscoveryService)'
        {
            _discoveryService = discoveryService;
        }

        /// <summary>
        /// 获取发现首页的会议、学术知识
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult WxDisMainPage()
        {
            var ret = _discoveryService.WxDisMainPage(WorkUser);
            return Ok(ret);
        }
    }
}
