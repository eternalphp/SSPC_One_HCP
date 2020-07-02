using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.External
{
    [Authorize]
    public class YSUserInfoController : ApiController
    {
        private readonly IYSUserInfoService _iYSUserInfoService;

        /// <summary>
        /// 云势医生接口构造函数
        /// </summary>
        /// <param name="iYSDoctoService"></param>
        public YSUserInfoController(IYSUserInfoService iYSUserInfoService)
        {
            _iYSUserInfoService = iYSUserInfoService;
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserInfo(string unionid)
        {
            var ret = _iYSUserInfoService.GetUserInfo(unionid);
            return Ok(ret);
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult DelUserInfo(string unionid)
        {
            var ret = _iYSUserInfoService.DelUserInfo(unionid);
            return Ok(ret);
        }

    }
}