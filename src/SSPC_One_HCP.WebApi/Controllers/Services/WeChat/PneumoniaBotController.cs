using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// 肺炎Bot
    /// </summary>
    public class PneumoniaBotController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcPneumoniaBotService _pneumoniaBotService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pneumoniaBotService">小程序通用服务</param>
        public PneumoniaBotController(IWcPneumoniaBotService pneumoniaBotService)
        {
            _pneumoniaBotService = pneumoniaBotService;
        }
        /// <summary>
        /// 肺炎Bot转发记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddPneumoniaBotForward(PneumoniaBotForward dto)
        {
            var result = _pneumoniaBotService.AddPneumoniaBotForward(dto);
            return Ok(result);
        }
        /// <summary>
        /// 新增 肺炎Bot操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddPneumoniaBotOperationRecord(PneumoniaBotOperationRecordInputDto dto)
        {
            var result = _pneumoniaBotService.AddPneumoniaBotOperationRecord(dto);
            return Ok(result);
        }

        /// <summary>
        /// 分页查询AI主播知识播报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetAiBroadcastPageList(RowNumModel<AiBroadcastInputDto> dto)
        {
            var ret = _pneumoniaBotService.GetAiBroadcastPageList(dto);
            return Ok(ret);
        }
        /// <summary>
        /// 获取音频媒体详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetAiBroadcast(DataInfo dataInfo)
        {
            var ret = _pneumoniaBotService.GetAiBroadcast(dataInfo);
            return Ok(ret);
        }

    }
}
