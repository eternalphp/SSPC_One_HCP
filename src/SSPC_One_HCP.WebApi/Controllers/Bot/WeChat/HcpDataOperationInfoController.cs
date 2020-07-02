using System.Web.Http;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// 资料库操作记录
    /// </summary>
    public class HcpDataOperationInfoController : WxSaleBaseApiController
    {
        private readonly IWcHcpDataOperationInfoService _hcpDataInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
        public HcpDataOperationInfoController(IWcHcpDataOperationInfoService hcpDataInfoService)
        {
            _hcpDataInfoService = hcpDataInfoService;
        }
        /// <summary>
        /// 新增 资料库操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddHcpDataOperationInfo(HcpDataOperationInfo dto)
        {
            var ret = _hcpDataInfoService.AddHcpDataOperationInfo(dto, WorkUser);
            return Ok(ret);
        }
        
    }


}
