using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using SSPC_One_HCP.Services.Services.HCP.Dto;

namespace SSPC_One_HCP.WebApi.Controllers.Services.HCP
{
    /// <summary>
    /// 资料库管理
    /// </summary>
    public class HcpDataInfoController : BaseApiController
    {
        private readonly IHcpDataInfoService  _hcpDataInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
        public HcpDataInfoController(IHcpDataInfoService hcpDataInfoService)
        {
            _hcpDataInfoService = hcpDataInfoService;
        }
        /// <summary>
        /// 资料库-获取详情列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductInfoList(RowNumModel<DataInfoSearchViewModel> rowNum)
        {
            var ret = _hcpDataInfoService.GetHcpDataInfoList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 资料库-新增或修改资料信息
        /// </summary>
        /// <param name="productInfoView"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateProductInfo(HcpDataInfoInputDto productInfoView)
        {
            var ret = _hcpDataInfoService.AddOrUpdateProductInfo(productInfoView, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 资料库-删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteDataInfo(HcpDataInfo dataInfo)
        {
            var ret = _hcpDataInfoService.DeleteHcpDataInfo(dataInfo, WorkUser);
            return Ok(ret);
        }
    }


}
