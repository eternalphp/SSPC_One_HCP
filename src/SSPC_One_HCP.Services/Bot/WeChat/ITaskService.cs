using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.KBS.InputDto;
using SSPC_One_HCP.Services.Bot.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot
{
    public interface ITaskService
    {
        /// <summary>
        /// 获取欢迎语
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        Task<ReturnValueModel> BOTWelcoming(string appId, int sex);
        /// <summary>
        /// 用户会话主入口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> Entrance(TaskInputDto dto, WorkUser workUser);

        /// <summary>
        /// 满意度
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> SatisfactionDegree(SatisfactionDegreeInputDto dto, WorkUser workUser);

        /// <summary>
        /// 获取推荐
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
         ReturnValueModel BOTRecommend(string appId, WorkUser workUser);
    }
}
