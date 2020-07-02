using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductInfoModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 知识库管理
    /// </summary>
    public class KnowledgeController : BaseApiController
    {
        private readonly IKnowledgeService _knowledgeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="knowledgeService"></param>
        public KnowledgeController(IKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }
        /// <summary>
        /// 获取资料详情列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductInfoList(RowNumModel<DataInfoSearchViewModel> rowNum)
        {
            var ret = _knowledgeService.GetProductInfoList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 获取学术资料列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAcademicList(RowNumModel<DataInfoSearchViewModel> rowNum)
        {
            var ret = _knowledgeService.GetAcademicList(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取播客列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPodcastList(RowNumModel<DataInfoSearchViewModel> rowNum)
        {
            var ret = _knowledgeService.GetPodcastList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 获取临床指南列表
        /// </summary>
        /// <param name="fsysArticle">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetClinicalguidelines(RowNumModel<FsysArticle> fsysArticle)
        {
            var ret = _knowledgeService.GetClinicalguidelines(fsysArticle);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或修改产品资料信息
        /// </summary>
        /// <param name="productInfoView"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateProductInfo(ProductInfoViewModel productInfoView)
        {
            var ret = _knowledgeService.AddOrUpdateProductInfo(productInfoView, WorkUser);
            return Ok(ret);
        }
       
        /// <summary>
        /// 新增或修改播客
        /// </summary>
        /// <param name="productInfoView"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdatePodcast(ProductInfoViewModel productInfoView)
        {
            var ret = _knowledgeService.AddOrUpdatePodcast(productInfoView, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或修改临床指南
        /// </summary>
        /// <param name="fsysArticle"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateClinicalguidelines(FsysArticle fsysArticle) {
            var ret = _knowledgeService.AddOrUpdateClinicalguidelines(fsysArticle,WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 删除临床指南
        /// </summary>
        /// <param name="fsysArticle"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteClinicalguidelines(FsysArticle fsysArticle)
        {
            var ret = _knowledgeService.DeleteClinicalguidelines(fsysArticle, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteDataInfo(DataInfo dataInfo)
        {
            var ret = _knowledgeService.DeleteDataInfo(dataInfo, WorkUser);
            return Ok(ret);
        }
        ///// <summary>
        ///// 获取产品列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult GetProductList()
        //{
        //    var ret = _knowledgeService.GetProductList(WorkUser);
        //    return Ok(ret);
        //}



        ///// <summary>
        ///// 新增或修改产品
        ///// </summary>
        ///// <param name="productTypeInfo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult AddOrUpdateProduct(ProductViewModel productTypeInfo)
        //{
        //    var ret = _knowledgeService.AddOrUpdateProduct(productTypeInfo, WorkUser);
        //    return Ok(ret);
        //}




        ///// <summary>
        ///// 新增或修改临床指南
        ///// </summary>
        ///// <param name="dataInfo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult AddOrUpdateAcademic(DataInfo dataInfo)
        //{
        //    var ret = _knowledgeService.AddOrUpdateAcademic(dataInfo, WorkUser);
        //    return Ok(ret);
        //}



        ///// <summary>
        ///// 删除产品
        ///// </summary>
        ///// <param name="productTypeInfo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult DeleteProduct(ProductTypeInfo productTypeInfo)
        //{
        //    var ret = _knowledgeService.DeleteProduct(productTypeInfo, WorkUser);
        //    return Ok(ret);
        //}

        /// <summary>
        /// 获取文档类型列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDocumentType(RowNumModel<DocumentType> rowNum)
        {
            var ret = _knowledgeService.GetDocumentType(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或修改文档类型
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateDocumentType(DocumentType documentType)
        {
            var ret = _knowledgeService.AddOrUpdateDocumentType(documentType, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除文档类型
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteDocumentType(DocumentType documentType)
        {
            var ret = _knowledgeService.DeleteDocumentType(documentType, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 提交播客审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PodcastApproval(ApprovalResultViewModel approvalResult)
        {
            var ret = _knowledgeService.PodcastApproval(approvalResult, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 提交产品资料审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ProductDataApproval(ApprovalResultViewModel approvalResult)
        {
            var ret = _knowledgeService.ProductDataApproval(approvalResult, WorkUser);
            return Ok(ret);
        }


        /// <summary>
        /// 赞同不赞同人员列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ApproveProductList(RowNumModel<ProductInfoLike> rowNum)
        {
            var ret = _knowledgeService.ApproveProductList(rowNum);
            return Ok(ret);
        }


    }
}
