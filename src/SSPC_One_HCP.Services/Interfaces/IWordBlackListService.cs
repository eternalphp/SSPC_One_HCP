using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWordBlackListService
    {
        /// <summary>
        /// 新增或更新关键词
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateWords(WordBlackList viewModel, WorkUser workUser);

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteWords(WordBlackList viewModel, WorkUser workUser);

        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetWordBlackLists(RowNumModel<WordBlackList> rowNum, WorkUser workUser);

        /// <summary>
        /// 根据类型获取关键词
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string[] GetWordBlackListByType(string type, char separator = ',');
    }
}
