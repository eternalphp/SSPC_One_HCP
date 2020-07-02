using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// PV
    /// </summary>
    public class LiveBroadcastPVController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcLiveBroadcastPVService _liveBroadcastPVService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="liveBroadcastPVService">小程序通用服务</param>
        public LiveBroadcastPVController(IWcLiveBroadcastPVService liveBroadcastPVService)
        {
            _liveBroadcastPVService = liveBroadcastPVService;
        }

        /// <summary>
        /// 记录明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddLiveBroadcastPV(LiveBroadcastPV dto)
        {
            var result = _liveBroadcastPVService.AddLiveBroadcastPV(dto);
            return Ok(result);
        }
        /// <summary>
        /// 查询累计点击数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult AddPneumoniaBotOperationRecord(string id)
        {
            var result = _liveBroadcastPVService.GetLiveBroadcastPV(id);
            return Ok(result);
        }
    }
}
