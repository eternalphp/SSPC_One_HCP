using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.RongCloud.Dto.YsWeChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud.YsWeChat
{
    public interface IWxRongCloudService
    {
        /// <summary>
        /// 融云-获取Token
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        ReturnValueModel GetToken(WorkUser workUser);

        /// <summary>
        /// 用户是否在聊天室
        /// </summary>
        /// <param name="ChatRoomId"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel UserChatroom(string chatRoomId, WorkUser workUser);
    }
}
