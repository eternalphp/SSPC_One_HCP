using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.TagGroup;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 手动标签组
    /// </summary>
    public class TagGroupController : BaseApiController
    {
        private readonly ITagGroupService _tagGroupService;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tagGroupService"></param>
        public TagGroupController(ITagGroupService tagGroupService)
        {
            _tagGroupService = tagGroupService;
        }

        #region 标签组
        /// <summary>
        /// 新增标签和标签组关联
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateTagGroupRelation(RowNumModel<TagGroupRelViewModel> rowNum)
        {
            var ret = _tagGroupService.AddOrUpdateTagGroupRelation(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 删除标签组以及标签
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteTagGroup(TagGroup tagGroup)
        {
            var ret = _tagGroupService.DeleteTagGroup(tagGroup);
            return Ok(ret);
        }


        /// <summary>
        /// 保存-标签组编辑标签
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult TagGroupUpdateTags(TagGroupTagViewModel tagGroupTagView)
        {
            var ret = _tagGroupService.GroupUpdateTags(tagGroupTagView);
            return Ok(ret);
        }

        /// <summary>
        ///  编辑时获取标签组和标签明细
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGroupUpdateTagsList(TagGroup tagGroup)
        {
            var ret = _tagGroupService.GetGroupUpdateTagsList(tagGroup);
            return Ok(ret);
        }

        /// <summary>
        /// 获得标签组列表
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public IHttpActionResult GetTagGroupList(TagGroup model)
        {
            var ret = _tagGroupService.GetTagGroupList(model);
            return Ok(ret);
        }

        #endregion


        #region 医生标签列表
        /// <summary>
        /// 获取医生列表和标签列表
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTagSearchList(RowNumModel<TagGroupViewModel> tagGroup)
        {
            var ret = _tagGroupService.GetTagSearchList(tagGroup);
            return Ok(ret);
        }
        #endregion

        #region 获取医生手动标签列表
        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetManualTagList(RowNumModel<TagGroupViewModel> rowNum)
        {
            var ret = _tagGroupService.GetManualTagList(rowNum);
            return Ok(ret);
        }
        #endregion
    }
}
