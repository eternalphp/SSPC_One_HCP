using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.RongCloud.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.RongCloud
{
    public interface IRongCloudService
    {
        /// <summary>
        /// 融云-模板路由回调
        /// </summary>
        dynamic TemplateRouteCallback(TemplateRouteInputDto inputDto);
        /// <summary>
        /// 融云-聊天室 状态同步
        /// </summary>
        dynamic ChatroomStatusSync(ChatroomStatusSyncDto inputDto, string body);
        /// <summary>
        /// 融云-消息撤回
        /// </summary>
        ReturnValueModel MessageRecall(RecallMessageInputDto inputDto, WorkUser workUser);

        /// <summary>
        /// 以发送聊天室消息方法实现：业务处理聊天室禁言与解禁
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomSend(ChatroomSendInputDto dto, WorkUser workUser);
        /// <summary>
        /// 消息历史记录下载地址获取
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        ReturnValueModel MessageHistory(string date);
        #region 聊天室
        /// <summary>
        /// 融云-创建聊天室
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomCreate(ChatroomInputDto inputDto, WorkUser workUser);
        /// <summary>
        /// 融云-查询聊天室
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomQuery(string chatroomId, WorkUser workUser);
        /// <summary>
        /// 融云-销毁聊天室
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomDestroy(ChatroomInputDto inputDto, WorkUser workUser);
        /// <summary>
        /// 融云-添加-聊天室保活服务
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomKeepaliveAdd(ChatroomInputDto inputDto, WorkUser workUser);
        /// <summary>
        /// 融云-删除-聊天室保活服务
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomKeepaliveRemove(ChatroomInputDto inputDto, WorkUser workUser);
        /// <summary>
        /// 融云-删除-获取聊天室保活
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ChatroomKeepaliveGetList();
        #endregion
    }
}
