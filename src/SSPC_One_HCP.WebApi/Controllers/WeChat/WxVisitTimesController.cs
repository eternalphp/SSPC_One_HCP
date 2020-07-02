using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{

    /// <summary>
    /// 微信小程序访问次数
    /// </summary>
    public class WxVisitTimesController : WxBaseApiController
    {
        private readonly IVisitTimesService _iVisitTimesService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wxHomeService"></param>
        public WxVisitTimesController(IVisitTimesService iVisitTimesService)
        {
            _iVisitTimesService = iVisitTimesService;
        }
        /// <summary>
        /// 新增访问次数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddVisitTimes(RowNumModel<VisitTimesViewModel> rowNum)
        {
            var ret = _iVisitTimesService.AddVisitTimes(rowNum, WorkUser);
            return Ok(ret);
        }
    }
}
