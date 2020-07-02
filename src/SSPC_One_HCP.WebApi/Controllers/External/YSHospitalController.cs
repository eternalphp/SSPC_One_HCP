using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.External
{
    /// <summary>
    /// 云势医院接口
    /// </summary>
    public class YSHospitalController : ApiController
    {
        private readonly IYSHospitalService _iYSHospitalService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DoctorController.DoctorController(IADDoctorService)'
        public YSHospitalController(IYSHospitalService iYSHospitalService)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DoctorController.DoctorController(IADDoctorService)'
        {
            _iYSHospitalService = iYSHospitalService;
        }

        /// <summary>
        /// 获取清洗医院列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetHospitalInfo(RowNumModel<HospitalViewModel> rowNum)
        {
            var ret = _iYSHospitalService.GetHospitalInfo(rowNum);
            return Ok(ret);
        }


        /// <summary>
        /// 批量清洗医院
        /// </summary>
        /// <param name="HospitalInfo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult AddUpdateHospitalInfo(List<YXHospitalViewModel> HospitalInfo)
        {
            var ret = _iYSHospitalService.AddUpdateHospitalInfo(HospitalInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 获取已清洗医院列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateHospitalInfo(RowNumModel<YXHospitalViewModel> rowNum)
        {
            var ret = _iYSHospitalService.UpdateHospitalInfo(rowNum);
            return Ok(ret);
        }
    }
}
