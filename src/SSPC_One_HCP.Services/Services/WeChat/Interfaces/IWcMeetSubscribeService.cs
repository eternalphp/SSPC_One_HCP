using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcMeetSubscribeService
    {
        /// <summary>
        /// 小程序会议订阅-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel AddMeetSubscribe(MeetSubscribeInputDto dto, WorkUser workUser);
    }
}
