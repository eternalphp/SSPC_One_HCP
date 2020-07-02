using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 小程序通用服务
    /// </summary>
    public interface IWxCommonService
    {
        /// <summary>
        /// 获取医院
        /// </summary>
        /// <param name="rowNum">模糊搜索，HospitalName</param>
        /// <returns></returns>
        Task<ReturnValueModel> GetHospital(RowNumModel<HospitalInfo> rowNum);

        /// <summary>
        /// 获取科室
        /// </summary>
        /// <returns></returns>
        Task<ReturnValueModel> GetDept(DepartmentInfo departmentInfo);
    }
}
