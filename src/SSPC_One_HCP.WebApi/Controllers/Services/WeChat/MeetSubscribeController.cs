using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// 小程序会议订阅
    /// </summary>
    public class MeetSubscribeController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcMeetSubscribeService _meetSubscribeService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="meetInfoSubscribeService">小程序通用服务</param>
        public MeetSubscribeController(IWcMeetSubscribeService meetInfoSubscribeService)
        {
            _meetSubscribeService = meetInfoSubscribeService;
        }


        /// <summary>
        /// 小程序会议订阅-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult AddMeetSubscribe([FromBody]MeetSubscribeInputDto dto)
        {
            var result = _meetSubscribeService.AddMeetSubscribe(dto, WorkUser);
            return Ok(result);
        }

    }
}
