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
    public interface IYSDoctoService
    {
        /// <summary>
        /// 获取清洗医生列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetDoctorInfo(RowNumModel<DoctorViewModel> rowNum);

        /// <summary>
        /// 批量清洗医生
        /// </summary>
        /// <param name="DoctorInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddUpdateDoctorInfo(List<YXDoctorViewModel> DoctorInfo);

        /// <summary>
        /// 獲取已清洗醫生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel UpdateDoctorInfo(RowNumModel<YXDoctorViewModel> rowNum);

        /// <summary>
        /// 获取批量申诉医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetAppealInfo();

        /// <summary>
        /// 批量保存申诉医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel UpdateApperalInfo(List<YXDoctorViewModel> DoctorInfo);
    }
}
