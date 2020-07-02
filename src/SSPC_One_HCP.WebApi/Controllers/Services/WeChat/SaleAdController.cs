using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using SSPC_One_HCP.WebApi.CustomerAuth;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.Services.Services.WeChat.Dto;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// 小程序 销售注册
    /// </summary>
    public class SaleAdController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcSaleAdService _saleAdService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="saleAdService">小程序通用服务</param>
        public SaleAdController(IWcSaleAdService saleAdService)
        {
            _saleAdService = saleAdService;
        }

        /// <summary>
        /// 内部员工 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        [AllowUnregistered]
        public async Task<IHttpActionResult> Login([FromBody]LoginInputDto dto)
        {
            var result = await _saleAdService.Login(dto, WorkUser);
            return Ok(result);
        }
    }
}