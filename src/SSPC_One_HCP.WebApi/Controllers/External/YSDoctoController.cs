using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.External
{
    /// <summary>
    /// 云势医生接口
    /// </summary>
    public class YSDoctoController : ApiController
    {
        private readonly IYSDoctoService _iYSDoctoService;

        /// <summary>
        /// 云势医生接口构造函数
        /// </summary>
        /// <param name="iYSDoctoService"></param>
        public YSDoctoController(IYSDoctoService iYSDoctoService)
        {
            _iYSDoctoService = iYSDoctoService;
        }

        /// <summary>
        /// 获取清洗医生列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetDoctorInfo(RowNumModel<DoctorViewModel> rowNum)
        {
            var ret = _iYSDoctoService.GetDoctorInfo(rowNum);
            return Ok(ret);
        }


        /// <summary>
        /// 批量清洗医生
        /// </summary>
        /// <param name="DoctorInfo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult AddUpdateDoctorInfo(List<YXDoctorViewModel> DoctorInfo)
        {
            var ret = _iYSDoctoService.AddUpdateDoctorInfo(DoctorInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 獲取已清洗醫生列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateDoctorInfo(RowNumModel<YXDoctorViewModel> rowNum)
        {
            var ret = _iYSDoctoService.UpdateDoctorInfo(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 获取批量申诉医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetAppealInfo()
        {
            var ret = _iYSDoctoService.GetAppealInfo();
            return Ok(ret);
        }

        /// <summary>
        /// 批量保存申诉医生列表
        /// </summary>
        /// <param name="DoctorInfo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateApperalInfo(List<YXDoctorViewModel> DoctorInfo)
        {
            var ret = _iYSDoctoService.UpdateApperalInfo(DoctorInfo);
            return Ok(ret);
        }
    }
}
