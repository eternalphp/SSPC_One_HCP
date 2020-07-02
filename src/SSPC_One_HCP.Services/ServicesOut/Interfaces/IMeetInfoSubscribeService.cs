using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.ServicesOut.Dto;

namespace SSPC_One_HCP.ServicesOut.Interfaces
{
    public interface IMeetInfoSubscribeService
    {
        /// <summary>
        /// 获取BU
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetBu(MeetInfoSubscribeDto dto);


        /// <summary>
        /// 新增会议信息
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        ReturnValueModel AddMeetInfo(MeetInfoSubscribeDto dto, string body);

        /// <summary>
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetListOfQA(MeetInfoSubscribeDto dto, string body);
        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetQuestionList(MeetInfoSubscribeDto dto, string body);
        /// <summary>
        /// 从其它会议导入题目或者从题库中选择题目
        /// </summary>
        /// <param name="meetQAs"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMeetQA(MeetInfoSubscribeDto dto, string body);
    }
}
