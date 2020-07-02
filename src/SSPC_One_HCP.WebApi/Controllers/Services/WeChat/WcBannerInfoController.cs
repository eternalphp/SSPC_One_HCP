using System.Web.Http;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using SSPC_One_HCP.WebApi.CustomerAuth;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// 小程序 横幅管理
    /// </summary>
    public class WcBannerInfoController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcBannerInfoService  _wcBannerInfoService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bannerInfoService">小程序通用服务</param>
        public WcBannerInfoController(IWcBannerInfoService bannerInfoService)
        {
            _wcBannerInfoService = bannerInfoService;
        }

        /// <summary>
        /// 横幅管理- 归属获取明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowUnregistered]
        public IHttpActionResult GetByBusinessTag(string input)
        {
            var result = _wcBannerInfoService.GetByBusinessTag(input, WorkUser);
            return Ok(result);
        }
    }
}