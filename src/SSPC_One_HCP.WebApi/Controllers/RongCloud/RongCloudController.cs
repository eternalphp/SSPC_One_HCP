using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.RongCloud;
using SSPC_One_HCP.Services.RongCloud.Dto;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.HCPData
{
    /// <summary>
    /// 融云API
    /// </summary>
    public class RongCloudController : BaseApiController
    {
        private readonly IRongCloudService _rongCloudService;
        /// <summary>
        /// 融云API
        /// </summary>
        /// <param name="rongCloudService"></param>
        public RongCloudController(IRongCloudService rongCloudService)
        {
            _rongCloudService = rongCloudService;
        }

        /// <summary>
        ///  融云-模板路由回调
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult TemplateRouteCallback([FromUri]TemplateRouteInputDto inputDto)
        {
            var ret = _rongCloudService.TemplateRouteCallback(inputDto);
            return Ok(ret);
        }

        /// <summary>
        ///  融云-聊天室 状态同步
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ChatroomStatusSync([FromUri]ChatroomStatusSyncDto inputDto )
        {
            var body = Request.Content.ReadAsStringAsync().Result;
            var ret = _rongCloudService.ChatroomStatusSync(inputDto, body);
            return Ok(ret);
        }
        /// <summary>
        /// 融云-消息撤回
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
       
        public IHttpActionResult MessageRecall(RecallMessageInputDto inputDto)
        {
            var ret = _rongCloudService.MessageRecall(inputDto,WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 以发送聊天室消息方法实现：业务处理聊天室禁言与解禁
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ChatroomSend(ChatroomSendInputDto inputDto)
        {
            var ret = _rongCloudService.ChatroomSend(inputDto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 消息历史记录下载地址获取
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult MessageHistory(string date)
        {
            var ret = _rongCloudService.MessageHistory(date);
            return Ok(ret);
        }
        #region 聊天室
        /// <summary>
        /// 融云-创建聊天室
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ChatroomCreate(ChatroomInputDto inputDto)
        {
            var ret = _rongCloudService.ChatroomCreate(inputDto, WorkUser);
            return Ok(ret);

        }
        /// <summary>
        /// 融云-聊天室ID查询聊天室
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ChatroomQuery(string chatroomId)
        {
            var ret = _rongCloudService.ChatroomQuery(chatroomId, new WorkUser());
            return Ok(ret);
        }
        /// <summary>
        /// 融云-销毁聊天室
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ChatroomDestroy(ChatroomInputDto inputDto)
        {
            var ret = _rongCloudService.ChatroomDestroy(inputDto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 融云-添加-聊天室保活服务
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ChatroomKeepaliveAdd(ChatroomInputDto inputDto)
        {
            var ret = _rongCloudService.ChatroomKeepaliveAdd(inputDto, new WorkUser());
            return Ok(ret);
        }
        /// <summary>
        /// 融云-删除-聊天室保活服务
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ChatroomKeepaliveRemove(ChatroomInputDto inputDto)
        {
            var ret = _rongCloudService.ChatroomKeepaliveRemove(inputDto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 融云-删除-获取聊天室保活
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ChatroomKeepaliveGetList()
        {
            var ret = _rongCloudService.ChatroomKeepaliveGetList();
            return Ok(ret);
        }
        #endregion
    }
}
