using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 知识库相关
    /// </summary>
    public class WxKownledgeController : WxBaseApiController
    {
        private readonly IWxKnowledgeService _knowledgeService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public WxKownledgeController(IWxKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }
        /// <summary>
        /// 知识库页面数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetWxKnowledgePage()
        {
            var ret = _knowledgeService.GetWxKnowledgePage(WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 播客列表
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult PodcastList(RowNumModel<DataInfo> rowData)
        {
            var ret = _knowledgeService.PodcastList(rowData, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 更改已读未读状态
        /// </summary>
        /// <param name="myReadRecord">阅读记录</param>
        /// <remarks>
        /// myReadRecord-DataInfoId 传入当前的播客ID
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult IsReadStatus(MyReadRecord myReadRecord)
        {
            var ret = _knowledgeService.IsReadStatus(myReadRecord, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 产品资料列表
        /// </summary>
        /// <param name="dataInfo">
        ///    非主页面 传入ProductTypeInfoId
        ///    主页面 主需要传入Title
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductMediaList(RowNumModel<MediaDataRelViewModel> dataInfo)
        {
            var ret = _knowledgeService.GetProductMediaList(dataInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 播客详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AudioMediaDetail(DataInfo dataInfo)
        {
            var ret = _knowledgeService.AudioMediaDetail(dataInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 临床指南列表
        /// </summary>
        /// <param name="dataInfo">分页、搜索</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGuideList(RowNumModel<DataInfo> dataInfo)
        {
            var ret = _knowledgeService.GetGuideList(dataInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 知识库详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult WxKnowledgeDetail(DataInfo dataInfo)
        {
            var ret = _knowledgeService.WxKnowledgeDetail(dataInfo);
            return Ok(ret);
        }
        /// <summary>
        /// 根据用户获取临床指南
        /// </summary>
        /// <param name="wxuser"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult ClinicalguidelinesByUser()
        {
            var ret = _knowledgeService.ClinicalguidelinesByUser(WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 赞同或不赞同
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult IsApproveProductDetails(ProductInfoLikeView date)
        {
            var ret = _knowledgeService.IsLike(date);
            return Ok(ret);
        }
    }
}
