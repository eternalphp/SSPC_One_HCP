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
    /// 微信小程序访问模块记录
    /// </summary>
    public class WxVisitModulesController : WxBaseApiController
    {
        private readonly IVisitModulesService _iVisitModulesService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxHomeService"></param>
        public WxVisitModulesController(IVisitModulesService iVisitModulesService)
        {
            _iVisitModulesService = iVisitModulesService;
        }
       


        /// <summary>
        /// 新增用户访问模块记录
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddVisitModules(RowNumModel<VisitModulesViewModel> rowNum)
        {
            var ret = _iVisitModulesService.AddVisitModules(rowNum, WorkUser);
            return Ok(ret);
        }

        
    }
}

