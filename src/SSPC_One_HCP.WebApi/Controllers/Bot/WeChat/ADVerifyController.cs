
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.Services.Bot.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    ///  销售：AD认证
    /// </summary>
    public class ADVerifyController : ApiController
    {
        private readonly IADVerifyService _aDVerifyService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDVerifyService"></param>
        public ADVerifyController(IADVerifyService aDVerifyService)
        {
            _aDVerifyService = aDVerifyService;
        }
        /// <summary>
        ///  销售：管理后台AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> AdminVerify([FromBody]VerifyAdminInputDto dto)
        {
            var ret = await _aDVerifyService.AdminVerifyAsync(dto);
            return Ok(ret);
        }

        /// <summary>
        /// 销售：验证小程序AD是否授权过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> WxVerify([FromBody]AuthorizedOrNotInputDto dto)
        {
            var ret = await _aDVerifyService.WxVerify(dto);
            //if (ret.Success == false && ret.Msg == "NOT_LOGIN")
            //{
            //    return Content(HttpStatusCode.Unauthorized, new
            //    {
            //        success = false,
            //        errCode = "NOT_LOGIN",
            //        message = "No login id."
            //    });
            //}
            return Ok(ret);
        }
        /// <summary>
        /// 销售：小程序AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> GetSaleUserInfo([FromBody]VerifyInputDto dto)
        {
            var ret = await _aDVerifyService.GetSaleUserInfo(dto);
            return Ok(ret);
        }
    }
}
