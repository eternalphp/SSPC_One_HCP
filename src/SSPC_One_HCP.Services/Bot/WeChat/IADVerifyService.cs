using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Bot.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot
{
    public interface IADVerifyService
    {
        /// <summary>
        /// 管理后台AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ReturnValueModel> AdminVerifyAsync(VerifyAdminInputDto dto);

        /// <summary>
        /// 验证小程序AD是否授权过
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ReturnValueModel> WxVerify(AuthorizedOrNotInputDto dto);
        /// <summary>
        /// 小程序AD验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ReturnValueModel> GetSaleUserInfo(VerifyInputDto dto);

    }
}
