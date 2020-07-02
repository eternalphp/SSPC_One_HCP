using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcPneumoniaBotService
    {
        /// <summary>
        ///  肺炎Bot转发记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel AddPneumoniaBotForward(PneumoniaBotForward dto);
        /// <summary>
        /// 新增 肺炎Bot操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel AddPneumoniaBotOperationRecord(PneumoniaBotOperationRecordInputDto dto);
        /// <summary>
        /// 分页查询AI主播知识播报
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetAiBroadcastPageList(RowNumModel<AiBroadcastInputDto> dto);
        /// <summary>
        /// 获取音频媒体详情
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        ReturnValueModel GetAiBroadcast(DataInfo dataInfo);

    }
}
