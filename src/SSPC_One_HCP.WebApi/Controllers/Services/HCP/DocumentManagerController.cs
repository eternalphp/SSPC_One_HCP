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
    /// 文献管理
    /// </summary>
    public class HcpDocumentManagerController : BaseApiController
    {
        private readonly IDocumentManagerService _documentManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentManager"></param>
        public HcpDocumentManagerController(IDocumentManagerService documentManager)
        {
            _documentManager = documentManager;
        }
        /// <summary>
        /// 文献管理-获取详情列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetList(RowNumModel<DocumentManagerSearchInputDto> rowNum)
        {
            var ret = _documentManager.GetList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 文献管理-新增或修改资料信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(DocumentManagerInputDto inputDto)
        {
            var ret = _documentManager.AddOrUpdate(inputDto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 文献管理-删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Delete(DocumentManager dataInfo)
        {
            var ret = _documentManager.Delete(dataInfo, WorkUser);
            return Ok(ret);
        }
    }


}
