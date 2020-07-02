using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IYSHospitalService
    {
        /// <summary>
        /// 获取清洗医院列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetHospitalInfo(RowNumModel<HospitalViewModel> rowNum);

        /// <summary>
        /// 批量清洗医院
        /// </summary>
        /// <param name="DoctorInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddUpdateHospitalInfo(List<YXHospitalViewModel> HospitalInfo);

        /// <summary>
        /// 获取已清洗医院列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel UpdateHospitalInfo(RowNumModel<YXHospitalViewModel> rowNum);
    }
}
