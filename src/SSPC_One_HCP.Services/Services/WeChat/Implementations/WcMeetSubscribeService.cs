using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    /// <summary>
    /// 小程序消息订阅
    /// </summary>
    public class WcMeetSubscribeService : IWcMeetSubscribeService
    {
        private readonly IEfRepository _rep;
        public WcMeetSubscribeService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 小程序会议订阅-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel AddMeetSubscribe(MeetSubscribeInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (dto == null && workUser == null)
            {
                rvm.Msg = "错误";
                rvm.Success = false;
                rvm.Result = "";
                return rvm;
            }

            var model = new MeetSubscribe
            {
                Id = Guid.NewGuid().ToString(),
                UserId = workUser?.WxSaleUser?.Id,
                UnionId = dto.UnionId,
                MeetId = dto.MeetId,
                OpenId = dto.OpenId,
                HasReminded = 0,
                RemindOffsetMinutes = -30,
                CreateTime = DateTime.UtcNow.AddHours(8),
                TemplateId = string.Join(",", dto.TemplateId),
            };

            _rep.Insert(model);
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = "";
            return rvm;
        }
    }
}
