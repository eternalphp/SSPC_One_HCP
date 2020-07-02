using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 微信API接口
    /// </summary>
    public interface IWxOpenService
    {
        /// <summary>
        /// 获取unionid
        /// </summary>
        /// <param name="WxUserInfoRequestModel"></param>
        /// <returns></returns>
        ReturnValueModel GetUnionId(WxUserInfoRequestModel wxUserInfoRequestModel);

        /// <summary>
        /// 文库登录
        /// </summary>
        /// <param name="wxUserInfoRequestModel"></param>
        /// <returns></returns>
        ReturnValueModel WKLogin(WKUser user);

        /// <summary>
        /// 保存FormID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ReturnValueModel FormID(TemplateForm user);

        /// <summary>
        /// 手机号
        /// </summary>
        /// <param name="wxUserInfoRequestModel"></param>
        /// <returns></returns>
        ReturnValueModel GetWXPhone(DecodePhoneModel phoneModel);
    }
}
