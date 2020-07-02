using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// Bot 管理
    /// </summary>
    public class BotManageController : BaseApiController
    {
        private readonly IBotApiService  _botApiService;
        private readonly IBotManageService  _botManageService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="botApiService"></param>
        public BotManageController(IBotApiService  botApiService, IBotManageService botManageService)
        {
            _botApiService = botApiService;
            _botManageService = botManageService;
        }
        /// <summary>
        ///  获取KBS bot信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetBotInfo()
        {
            var ret = await _botApiService.BotInfo(WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 根据KBS BOT ID查询知识包
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetByBotIdPacks(string id)
        {
            var ret = await _botApiService.GetByBotIdPacks(id,WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// BOT配置-新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateSaleConfigure([FromBody]BotSaleConfigure dto)
        {
            var ret = _botManageService.AddOrUpdateSaleConfigure(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// BOT配置- 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMenusSaleConfigure()
        {
            var ret = _botManageService.GetMenusSaleConfigure();
            return Ok(ret);
        }
        /// <summary>
        /// BOT配置- 获取配置列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMenusSaleConfigureList(RowNumModel<BotSaleConfigure> row)
        {
            var ret = _botManageService.GetMenusSaleConfigureList(row, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        ///  BOT配置- 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMenusSaleConfigure(BotSaleConfigure row)
        {
            var ret = _botManageService.DeleteMenusSaleConfigure(row, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章标准规则配置-新增或修改(次数)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMedalStandardConfigure([FromBody]BotMedalStandardConfigure dto)
        {
            var ret = _botManageService.AddOrUpdateMedalStandardConfigure(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章标准规则配置- 根据ID获取配置信息(次数)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
       // [AllowAnonymous]
        public IHttpActionResult GetMedalStandardConfigure(string id)
        {
            var ret = _botManageService.GetMedalStandardConfigure(id);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章标准规则配置- 分页查询(次数)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMedalStandardConfigureList(RowNumModel<BotMedalStandardConfigure> row)
        {
            var ret = _botManageService.GetMedalStandardConfigureList(row, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章标准规则配置- 删除(次数)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMedalStandardConfigure(BotMedalStandardConfigure dto)
        {
            var ret = _botManageService.DeleteMedalStandardConfigure(dto, WorkUser);
            return Ok(ret);
        }


        /// <summary>
        /// 勋章业务规则配置-新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMedalBusinessConfigure([FromBody]BotMedalBusinessConfigure dto)
        {
            var ret = _botManageService.AddOrUpdateMedalBusinessConfigure(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章业务规则配置- 根据ID获取配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMedalBusinessConfigure(string id)
        {
            var ret = _botManageService.GetMedalBusinessConfigure(id);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章业务规则配置- 分页查询
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMedalBusinessConfigureList(RowNumModel<BotMedalBusinessConfigure> row)
        {
            var ret = _botManageService.GetMedalBusinessConfigureList(row, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 勋章标准规则配置- 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMedalBusinessConfigure(BotMedalBusinessConfigure dto)
        {
            var ret = _botManageService.DeleteMedalBusinessConfigure(dto, WorkUser);
            return Ok(ret);
        }


        /// <summary>
        /// KBS上传文件 通用接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Upload()
        {
            var req = HttpContext.Current.Request;
            var files = req.Files;
            var ret = await _botApiService.Upload(files, null);
            return Ok(ret);
        }
    }
}
