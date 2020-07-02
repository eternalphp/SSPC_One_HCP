using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 小程序通用
    /// </summary>
    public class WxCommonController : ApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWxCommonService _wxCommonService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wxCommonService">小程序通用服务</param>
        public WxCommonController(IWxCommonService wxCommonService)
        {
            _wxCommonService = wxCommonService;
        }
        /// <summary>
        /// 获取医院
        /// </summary>
        /// <param name="rowNum">模糊搜索，HospitalName</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> GetHospital(RowNumModel<HospitalInfo> rowNum)
        {
            var ret = await _wxCommonService.GetHospital(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 获取科室
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> GetDept(DepartmentInfo departmentInfo)
        {
            var ret = await _wxCommonService.GetDept(departmentInfo);
            return Ok(ret);
        }

      
    }
}
