using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Bot.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot
{
    public interface IWxManageService
    {
        /// <summary>
        /// 获取小程序unionod
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel GetWxUserInfo(WxManageInputDto dto);

        ReturnValueModel GetSSOId(string  userId);
        /// <summary>
        /// 获取用户已获取的勋章 和 未获取的勋章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReturnValueModel GetMedal(string appId, WorkUser workUser);
    }
}
