using SSPC_One_HCP.Services.RongCloud.Dto;
using SSPC_One_HCP.Services.RongCloud.YsWeChat;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using SSPC_One_HCP.WebApi.CustomerAuth;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.HCPData
{
    /// <summary>
    /// 融云-医生端API
    /// </summary>
    public class WxRongCloudController : WxBaseApiController
    {
        private readonly IWxRongCloudService _wxRongCloudService;
        /// <summary>
        /// 融云API
        /// </summary>
        /// <param name="wxRongCloudService"></param>
        public WxRongCloudController(IWxRongCloudService wxRongCloudService)
        {
            _wxRongCloudService = wxRongCloudService;
        }

        /// <summary>
        ///  融云-获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        //[AllowAnonymous]
        public IHttpActionResult GetToken()
        {
            var ret = _wxRongCloudService.GetToken(WorkUser);
            return Ok(ret);
        }
        /// <summary>
        ///  融云-用户是否在聊天室
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserChatroom([FromBody]UserChatroomDto dto)
        {
            var ret = _wxRongCloudService.UserChatroom(dto.ChatRoomId, WorkUser);
            return Ok(ret);
        }

    }
}
