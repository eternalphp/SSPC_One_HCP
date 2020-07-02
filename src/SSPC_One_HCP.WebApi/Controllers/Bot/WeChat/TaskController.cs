using SSPC_One_HCP.KBS.InputDto;
using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// 会话任务
    /// </summary>
    public class TaskController : WxSaleBaseApiController
    {

        private readonly ITaskService _taskService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskService"></param>
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        /// <summary>
        /// 欢迎语
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BOTWelcoming([FromUri]string appId, int sex)
        {
            var ret = await _taskService.BOTWelcoming(appId, sex);
            return Ok(ret);
        }
        /// <summary>
        /// 获取销售 Bot 信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IHttpActionResult> Entrance([FromBody]TaskInputDto dto)
        {
            var ret = await _taskService.Entrance(dto, WorkUser);
            if (ret.Success == false && ret.Msg == "NOT_LOGIN")
            {
                return Content(HttpStatusCode.Unauthorized, new
                {
                    success = false,
                    errCode = "NOT_LOGIN",
                    message = "No login id.1"
                });
            }
            return Ok(ret);
        }
        /// <summary>
        /// 获取销售 Bot 信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> SatisfactionDegree([FromBody]SatisfactionDegreeInputDto dto)
        {
            var ret = await _taskService.SatisfactionDegree(dto, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取销售 Bot 信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult BOTRecommend([FromUri]string appId)
        {
            var ret =  _taskService.BOTRecommend(appId, WorkUser);
            return Ok(ret);
        }
    }
}
