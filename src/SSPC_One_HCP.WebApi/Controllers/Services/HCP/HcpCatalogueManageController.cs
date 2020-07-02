using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.HCP
{
    /// <summary>
    /// 目录管理
    /// </summary>
    public class HcpCatalogueManageController : BaseApiController
    {
        private readonly IHcpCatalogueManageService _hcpCatalogueManageService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpCatalogueManageService"></param>
        public HcpCatalogueManageController(IHcpCatalogueManageService hcpCatalogueManageService)
        {
            _hcpCatalogueManageService = hcpCatalogueManageService;
        }

        /// <summary>
        /// 目录管理-根据BU名称获取列表
        /// </summary>
        /// <param name="buName"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetHcpCatalogueList(string buName)
        {
            var ret = _hcpCatalogueManageService.GetHcpCatalogueList(buName, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 目录管理-分页获取详情列表
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetHcpCataloguePageList(RowNumModel<HcpCatalogueManage> row)
        {
            var ret = _hcpCatalogueManageService.GetHcpCataloguePageList(row, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 目录管理-新增或修改信息
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateCatalogue(HcpCatalogueManage view)
        {
            var ret = _hcpCatalogueManageService.AddOrUpdateCatalogue(view, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 目录管理-删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteDataInfo(HcpCatalogueManage model)
        {
            var ret = _hcpCatalogueManageService.DeleteHcpCatalogue(model, WorkUser);
            return Ok(ret);
        }
    }
}
