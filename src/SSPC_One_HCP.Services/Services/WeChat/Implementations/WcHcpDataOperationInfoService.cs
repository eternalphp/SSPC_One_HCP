using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    public class WcHcpDataOperationInfoService : IWcHcpDataOperationInfoService
    {

        private readonly IEfRepository _rep;
        public WcHcpDataOperationInfoService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 新增 资料库操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel AddHcpDataOperationInfo(HcpDataOperationInfo dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var model = new HcpDataOperationInfo
            {
                Id = Guid.NewGuid().ToString(),
                UserId = workUser.WxSaleUser.Id,
                ClickContent = dto.ClickContent,
                ActionContent = dto.ActionContent,
                CreateTime = DateTime.UtcNow.AddHours(8),
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
