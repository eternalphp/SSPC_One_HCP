using SSPC_One_HCP.KBS.InputDto;
using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// Bot 标签管理
    /// </summary>
    public class BotTagManageController : BaseApiController
    {

        private readonly IBotApiService _botApiService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="botApiService"></param>
        public BotTagManageController(IBotApiService botApiService)
        {
            _botApiService = botApiService;
        }
        /// <summary>
        /// 内容自动标签-添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateAutomaticContentTag([FromBody]AutomaticContentTagInputDto dto)
        {
            var ret = await _botApiService.CreateAutomaticContentTag(dto,WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 内容自动标签-根据内容ID 获取自动标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetAutomaticContentTag([FromUri]string id)
        {
            var ret = await _botApiService.GetAutomaticContentTag(id, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 内容自动标签-删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> DeleteAutomaticContentTag([FromUri]string id)
        {
            var ret = await _botApiService.DeleteAutomaticContentTag(id, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 内容业务标签-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateBusinessContentTag([FromBody]BusinessContentTagInputDto dto)
        {
            var ret = await _botApiService.CreateBusinessContentTag(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 内容业务标签-根据内容ID 获取业务标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetBusinessContentTagDto([FromUri]string id)
        {
            var ret = await _botApiService.GetBusinessContentTagDto(id, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 内容自动标签-删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> DeleteBusinessContentTagDto([FromBody]string id)
        {
            var ret = await _botApiService.DeleteBusinessContentTagDto(id, WorkUser);
            return Ok(ret);
        }
       
    }
}
