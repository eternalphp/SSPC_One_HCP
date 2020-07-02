using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 公众号推广
    /// </summary>
    public class WxExtensionController : WxBaseApiController
    {
        private readonly IWxExtensionService _WxExtensionService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="WxExtensionService"></param>
        public WxExtensionController(IWxExtensionService WxExtensionService)
        {
            _WxExtensionService = WxExtensionService;
        }
        /// <summary>
        /// 根据微信用户科室获取公众号推广信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult WxGetPublicAccount(RowNumModel<PublicAccount> publicaccount)
        {
            var ret = _WxExtensionService.WxGetPublicAccount(publicaccount, WorkUser);
            return Ok(ret);
        }
        //[HttpPost]
        //[AllowUnregistered]
        //public IHttpActionResult WxGetRecordCount(QRcodeRecord qRcodeRecord) {
        //    var ret = _WxExtensionService.WxGetRecordCount(qRcodeRecord,WorkUser);
        //    return Ok(ret);
        //}

    }
}