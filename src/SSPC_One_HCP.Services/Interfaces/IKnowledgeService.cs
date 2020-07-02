using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductInfoModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 知识库管理
    /// </summary>
    public interface IKnowledgeService
    {
        /// <summary>
        /// 获取学术知识列表
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetAcademicList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser);

        /// <summary>
        /// 获取播客列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetPodcastList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser);

        /// <summary>
        /// 获取产品资料列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetProductInfoList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser);
        /// <summary>
        /// 获取临床指南列表
        /// </summary>
        /// <param name="fsysArtice">分页参数</param>
        /// <returns></returns>
        ReturnValueModel GetClinicalguidelines(RowNumModel<FsysArticle> fsysArtice);

        /// <summary>
        /// 新增或修改产品、学术、播客
        /// </summary>
        /// <param name="product"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateProduct(ProductTypeViewModel product, WorkUser workUser);

        /// <summary>
        /// 新增或修改资料信息
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateProductInfo(ProductInfoViewModel dataInfoview, WorkUser workUser);

        /// <summary>
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteDataInfo(DataInfo dataInfo, WorkUser workUser);
        /// <summary>
        /// 删除临床指南
        /// </summary>
        /// <param name="fsysArticle">要删除的数据</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteClinicalguidelines(FsysArticle fsysArticle,WorkUser workUser);
        /// <summary>
        /// 新增或修改学术知识
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateAcademic(DataInfo dataInfo, WorkUser workUser);

        /// <summary>
        /// 新增或修改播客
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdatePodcast(ProductInfoViewModel productInfoView, WorkUser workUser);

        /// <summary>
        /// 删除产品、学术、播客
        /// </summary>
        /// <param name="product"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteProduct(ProductTypeInfo product, WorkUser workUser);

        /// <summary>
        /// 获取文档类型列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetDocumentType(RowNumModel<DocumentType> rowNum);

        /// <summary>
        /// 新增或更新文档类型
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateDocumentType(DocumentType documentType, WorkUser workUser);
        /// <summary>
        /// 新增或修改临床指南
        /// </summary>
        /// <param name="fsysArticle">要添加的临床指南</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateClinicalguidelines(FsysArticle fsysArticle,WorkUser workUser);

        /// <summary>
        /// 删除文档类
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteDocumentType(DocumentType documentType, WorkUser workUser);

        /// <summary>
        /// 提交播客审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel PodcastApproval(ApprovalResultViewModel approvalResult, WorkUser workUser);

        /// <summary>
        /// 提交产品资料审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel ProductDataApproval(ApprovalResultViewModel approvalResult, WorkUser workUser);

        /// <summary>
        /// 赞同不赞同列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel ApproveProductList(RowNumModel<ProductInfoLike> rowNum);

    }
}
