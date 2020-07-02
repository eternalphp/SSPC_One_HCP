using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface ISmsService
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="wxUserModel">传入手机号码</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> SendSms(WxUserModel wxUserModel);

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="wxUserModel">传入Code、Mobile</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> VerifySmsCode(WxUserModel wxUserModel);
    }
}
