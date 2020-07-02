using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcHcpDataOperationInfoService
    {
        /// <summary>
        ///  资料库操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel AddHcpDataOperationInfo(HcpDataOperationInfo dto, WorkUser workUser);
    }
}
