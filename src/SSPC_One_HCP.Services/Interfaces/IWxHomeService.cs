using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using System;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxHomeService
    {
        ReturnValueModel GetAttendMeetings(WorkUser workUser, DateTime? dateTime = null);
        ReturnValueModel GetLeaningTimes(WorkUser workUser, DateTime? dateTime = null);
        ReturnValueModel GetVisitCitys(WorkUser workUser, DateTime? dateTime = null);
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        ReturnValueModel AddFeedback(FeedbackViewModel viewModel, WorkUser workUser);

        /// <summary>
        /// 删除个人信息
        /// </summary>
        /// <returns></returns>
        ReturnValueModel DeleteMyAccount(WorkUser workUser);

        /// <summary>
        /// 费森竞争产品
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetCompetingProduct();
    }
}
