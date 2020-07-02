using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.TagGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{

    public interface ITagGroupService
    {
        /// <summary>
        /// 新增标签组
        /// </summary>
        /// <param name="TagGroupName"></param>
        /// <returns></returns>
        ReturnValueModel AddTagGroup(string TagGroupName);

        /// <summary>
        /// 获取标签组列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetTagGroupList(TagGroup model);


        /// <summary>
        /// 新增或编辑 标签组以及新增标签组和标签的关联
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateTagGroupRelation(RowNumModel<TagGroupRelViewModel> rowNum);


        /// <summary>
        /// 获取标签组对应的标签列表
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        ReturnValueModel GetTagGroupRelationList(TagGroup tagGroup);


        /// <summary>
        /// 标签组增加标签
        /// </summary>
        /// <param name="tagGroupTagView"></param>
        /// <returns></returns>
        ReturnValueModel GroupAddTags(TagGroupTagViewModel tagGroupTagView);

        /// <summary>
        /// 标签组编辑标签
        /// </summary>
        /// <param name="tagGroupTagView"></param>
        /// <returns></returns>
        ReturnValueModel GroupUpdateTags(TagGroupTagViewModel tagGroupTagView);

        /// <summary>
        ///  删除标签组以及标签
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        ReturnValueModel DeleteTagGroup( TagGroup tagGroup);

        ReturnValueModel GetTagSearchList(RowNumModel<TagGroupViewModel> tagGroup);

        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        ReturnValueModel GetManualTagList(RowNumModel<TagGroupViewModel> tagGroup);


        /// <summary>
        /// 编辑时获取标签组和标签明细
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        ReturnValueModel GetGroupUpdateTagsList(TagGroup tagGroup);
    }
}
