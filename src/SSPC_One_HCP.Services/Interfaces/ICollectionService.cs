using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface ICollectionService
    {
        /// <summary>
        /// 收藏功能
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <param name="workUser">当前操作用户</param>
        /// <returns></returns>
        ReturnValueModel CollectionMeet(MyCollectionInfo collectionInfo, WorkUser workUser);

        /// <summary>
        /// 收藏功能
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <param name="workUser">当前操作用户</param>
        /// <returns></returns>
        ReturnValueModel CollectionMeet(MyCollectionInfo collectionInfo);

        /// <summary>
        /// 收藏列表
        /// </summary>
        /// <param name="rowCollection">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel CollectionList(RowNumModel<CollectionViewModel> rowCollection, WorkUser workUser);
    }
}
