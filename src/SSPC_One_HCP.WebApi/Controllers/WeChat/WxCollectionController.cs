using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 收藏控制器
    /// </summary>
    public class WxCollectionController : WxBaseApiController
    {
        private readonly ICollectionService _collectionService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectionService"></param>
        public WxCollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        /// <summary>
        /// 收藏会议
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CollectionMeet(MyCollectionInfo collectionInfo)
        {
            var ret = _collectionService.CollectionMeet(collectionInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 收藏会议
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Collection(MyCollectionInfo collectionInfo)
        {
            var ret = _collectionService.CollectionMeet(collectionInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 收藏列表
        /// </summary>
        /// <param name="rowCollection">分页、搜索</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CollectionList(RowNumModel<CollectionViewModel> rowCollection)
        {
            var ret = _collectionService.CollectionList(rowCollection, WorkUser);
            return Ok(ret);
        }
    }
}
