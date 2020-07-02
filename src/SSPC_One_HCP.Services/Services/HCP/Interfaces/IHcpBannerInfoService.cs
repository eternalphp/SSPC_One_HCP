using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Interfaces
{
    public interface IHcpBannerInfoService
    {
        /// <summary>
        /// 横幅管理-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetList(RowNumModel<BannerInfo> rowNum, WorkUser workUser);
        /// <summary>
        /// 横幅管理- ID获取明细
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetById(string id, WorkUser workUser);

        /// <summary>
        /// 横幅管理- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdate(BannerInfo dto, WorkUser workUser);
        /// <summary>
        /// 横幅管理- 明细新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>

        ReturnValueModel AddOrUpdateBannerInfoItem(BannerInfoItem dto, WorkUser workUser);
        /// <summary>
        /// 横幅管理- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel Delete(BannerInfo dto, WorkUser workUser);
        /// <summary>
        ///  横幅管理- 明细删除
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteBannerInfoItem(BannerInfoItem dto, WorkUser workUser);
    }
}
