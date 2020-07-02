using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Interfaces
{
    public interface IHcpCatalogueManageService
    {
        /// <summary>
        /// 新增修改目录
        /// </summary>
        /// <param name="view"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateCatalogue(HcpCatalogueManage view, WorkUser workUser);
        /// <summary>
        /// 查询目录列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetHcpCatalogueList(string buName, WorkUser workUser);
        /// <summary>
        /// 分页查询目录列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetHcpCataloguePageList(RowNumModel<HcpCatalogueManage> row, WorkUser workUser);
        /// <summary>
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteHcpCatalogue(HcpCatalogueManage model, WorkUser workUser);
    }
}
