using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductInfoModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Web.Http;
namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 短信管理
    /// </summary>
    public class ManagementController : BaseApiController
    {
        private readonly IManagementService _ManagementService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ManagementService"></param>
        public ManagementController(IManagementService ManagementService)
        {
            _ManagementService = ManagementService;
        }
        /// <summary>
        /// 新增或修改短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddorUpdateManagement(Management manage) {
            var ret = _ManagementService.AddorUpdateManagement(manage, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteManagement(Management manage) {
            var ret = _ManagementService.DeleteManagement(manage, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 获取所有短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetManagementList(RowNumModel<Management> manage) {
            var ret = _ManagementService.GetManagementList(manage, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 提交审核结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ManagementApproval(ApprovalResultViewModel approvalResult) {
            var ret = _ManagementService.ManagementApproval(approvalResult, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 撤销修改
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RevokeUpdateManagement(Management manage) {
            var ret = _ManagementService.RevokeUpdateManagement(manage,WorkUser);
            return Ok(ret);
        }
    }
}
