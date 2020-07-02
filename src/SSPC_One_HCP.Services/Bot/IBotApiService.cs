using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Bot.Dto;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.Bot
{
    public interface IBotApiService
    {
        /// <summary>
        /// 获取KBS bot信息
        /// </summary>
        /// <returns></returns>
        Task<ReturnValueModel> BotInfo(WorkUser workUser);
        /// <summary>
        /// 根据KBS BOT ID查询知识包
        /// </summary>
        /// <returns></returns>
        Task<ReturnValueModel> GetByBotIdPacks(string id, WorkUser workUser);
        /// <summary>
        /// 内容自动标签-添加
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
         Task<ReturnValueModel> CreateAutomaticContentTag(AutomaticContentTagInputDto dto, WorkUser workUser);
        /// <summary>
        /// 内容自动标签-根据内容ID 获取自动标签
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> GetAutomaticContentTag(string id, WorkUser workUser);
        /// <summary>
        /// 内容自动标签-删除
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> DeleteAutomaticContentTag(string id, WorkUser workUser);

        /// <summary>
        /// 内容业务标签-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> CreateBusinessContentTag(BusinessContentTagInputDto dto, WorkUser workUser);
        /// <summary>
        /// 内容业务标签-根据内容ID 获取业务标签
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
         Task<ReturnValueModel> GetBusinessContentTagDto(string id, WorkUser workUser);
        /// <summary>
        /// 内容业务标签-删除
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
         Task<ReturnValueModel> DeleteBusinessContentTagDto(string id, WorkUser workUser);

        Task<ReturnValueModel> Upload(HttpFileCollection httpFile, WorkUser workUser);
    }
}
