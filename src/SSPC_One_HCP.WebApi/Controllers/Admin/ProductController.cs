using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 产品控制器
    /// </summary>
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="rowPro">分页、搜索</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductList(RowNumModel<SearchProductViewModel> rowPro)
        {
            var ret = _productService.GetProductList(rowPro, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或更新产品信息
        /// </summary>
        /// <param name="productInfo">
        /// 产品信息，Id存在则更新，否则新增
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateProduct(ProductDetailViewModel productInfo)
        {
            var ret = _productService.AddOrUpdateProduct(productInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="proList">传入模型数组，只需要Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DelProduct(List<ProductInfo> proList)
        {
            var ret = _productService.DelProduct(proList, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="productInfo">只需传入Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ProductDetail(ProductInfo productInfo)
        {
            var ret = _productService.ProductDetail(productInfo);
            return Ok(ret);
        }
        ///// <summary>
        ///// 产品绑定bu列表
        ///// </summary>
        ///// <param name="rowBuPro">分页、搜索</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult BuProList(RowNumModel<BuProDeptRelViewModel> rowBuPro)
        //{
        //    var ret = _productService.BuProList(rowBuPro, WorkUser);
        //    return Ok(ret);
        //}
        ///// <summary>
        ///// 添加关系
        ///// </summary>
        ///// <param name="buProDeptModel">关系信息</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult AddOrUpdateBuProDeptRel(BuProDeptModel buProDeptModel)
        //{
        //    var ret = _productService.AddOrUpdateBuProDeptRel(buProDeptModel, WorkUser);
        //    return Ok(ret);
        //}
        ///// <summary>
        ///// 关系详情
        ///// </summary>
        ///// <param name="buProDeptModel">传入BuName和ProId</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult BuProDeptDetail(BuProDeptModel buProDeptModel)
        //{
        //    var ret = _productService.BuProDeptDetail(buProDeptModel, WorkUser);
        //    return Ok(ret);
        //}

        /// <summary>
        /// 获取BuProDept对应关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult BuProDeptRelMap(ProductBuDeptSelectionViewModel buProDeptModel)
        {
            var ret = _productService.BuProDeptRelMap(buProDeptModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 产品列表（下拉框）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProList()
        {
            var ret = _productService.GetProList();
            return Ok(ret);
        }
    }
}
