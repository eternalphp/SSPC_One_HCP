 
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.WxMedicine;
using System.Collections.Generic;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxMedicineService
    {
        /// <summary>
        /// 新增热搜和个人搜索
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddHotSearch(RowNumModel<MedicineHotSearchViewModel> model, WorkUser workUser);

        /// <summary>
        /// 显示热搜列表和个人搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetHotSearchList(RowNumModel<MedicineHotSearch> model, WorkUser workUser);


        /// <summary>
        /// 删除个人搜索列表记录
        /// </summary>
        /// <param name="historyId"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteSearchHistory(RowNumModel<WxMedicineDelViewModel> model);
    }
}
