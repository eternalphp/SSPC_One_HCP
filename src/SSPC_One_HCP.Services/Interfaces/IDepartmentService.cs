using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetDepartmentList(DepartmentInfo department);

        ReturnValueModel AddOrUpdateDepartmentInfo(DepartmentInfo department, WorkUser workUser);

        /// <summary>
        /// 获取BU列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetBUList(WorkUser workUser = null);
    }
}
