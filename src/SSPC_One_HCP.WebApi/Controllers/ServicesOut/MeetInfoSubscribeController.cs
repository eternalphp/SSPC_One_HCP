using SSPC_One_HCP.ServicesOut.Dto;
using SSPC_One_HCP.ServicesOut.Interfaces;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.ServicesOut
{
    /// <summary>
    /// 
    /// </summary>
    public class MeetInfoSubscribeController : ApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IMeetInfoSubscribeService _meetInfoSubscribeService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="meetInfoSubscribeService">小程序通用服务</param>
        public MeetInfoSubscribeController(IMeetInfoSubscribeService meetInfoSubscribeService)
        {
            _meetInfoSubscribeService = meetInfoSubscribeService;
        }

        [HttpPost]
        public IHttpActionResult BuProDeptRelMap([FromUri]MeetInfoSubscribeDto dto)
        {
            var result = _meetInfoSubscribeService.GetBu(dto);
            return Ok(result);
        }
        /// <summary>
        /// 新增会议
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMeetInfo([FromUri]MeetInfoSubscribeDto dto)
        {
            var body = Request.Content.ReadAsStringAsync().Result;
            var result = _meetInfoSubscribeService.AddMeetInfo(dto, body);
            return Ok(result);
        }

        /// <summary>
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult MeetListOfQA(MeetInfoSubscribeDto dto)
        {
            var body = Request.Content.ReadAsStringAsync().Result;
            var result = _meetInfoSubscribeService.GetMeetListOfQA(dto, body);
            return Ok(result);
        }
        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QuestionList(MeetInfoSubscribeDto dto)
        {
            var body = Request.Content.ReadAsStringAsync().Result;
            var result = _meetInfoSubscribeService.GetQuestionList(dto, body);
            return Ok(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMeetQA(MeetInfoSubscribeDto dto)
        {
            var body = Request.Content.ReadAsStringAsync().Result;
            var result = _meetInfoSubscribeService.AddOrUpdateMeetQA(dto, body);
            return Ok(result);
        }
    }
}
