using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信对接外部接口 记录行为模式
    /// </summary>
    public class WechatActionHistoryController : WxBaseApiController
    {
        private readonly IWechatActionHistoryService _iWechatActionHistoryService;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="iWechatActionHistoryService"></param>
        public WechatActionHistoryController(IWechatActionHistoryService iWechatActionHistoryService)
        {
            _iWechatActionHistoryService = iWechatActionHistoryService;
        }
        /// <summary>
        /// 微信接口-新增用户行为记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddActionHistory(RowNumModel<WechatActionHistory> model)
        {
            var ret = _iWechatActionHistoryService.AddActionHistory(model, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取期刊热搜词
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetMagaZineList()
        {
            var ret = _iWechatActionHistoryService.GetMagaZineList();
            return Ok(ret);
        }
    }
}
