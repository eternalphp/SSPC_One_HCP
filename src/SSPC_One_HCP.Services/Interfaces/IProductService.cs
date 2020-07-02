using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="rowPro">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetProductList(RowNumModel<SearchProductViewModel> rowPro, WorkUser workUser);

        /// <summary>
        /// 新增或更新产品信息
        /// </summary>
        /// <param name="productInfo">
        /// 产品信息，Id存在则更新，否则新增
        /// </param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateProduct(ProductDetailViewModel productInfo, WorkUser workUser);

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="proList">传入模型数组，只需要Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DelProduct(List<ProductInfo> proList, WorkUser workUser);

        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="productInfo">传入Id</param>
        /// <returns></returns>
        ReturnValueModel ProductDetail(ProductInfo productInfo);

        ///// <summary>
        ///// 产品绑定bu列表
        ///// </summary>
        ///// <param name="rowBuPro">分页、搜索</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //ReturnValueModel BuProList(RowNumModel<BuProDeptRelViewModel> rowBuPro, WorkUser workUser);

        ///// <summary>
        ///// 添加关系
        ///// </summary>
        ///// <param name="buProDeptModel">关系信息</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //ReturnValueModel AddOrUpdateBuProDeptRel(BuProDeptModel buProDeptModel, WorkUser workUser);

        ///// <summary>
        ///// 关系详情
        ///// </summary>
        ///// <param name="buProDeptModel">传入BuName和ProId</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //ReturnValueModel BuProDeptDetail(BuProDeptModel buProDeptModel, WorkUser workUser);

        /// <summary>
        /// buprodept关系
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel BuProDeptRelMap(WorkUser workUser);

        /// <summary>
        /// buprodept关系 
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel BuProDeptRelMap(ProductBuDeptSelectionViewModel buProDeptModel, WorkUser workUser);

        /// <summary>
        /// 产品列表（下拉框）
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetProList();
    }
}
