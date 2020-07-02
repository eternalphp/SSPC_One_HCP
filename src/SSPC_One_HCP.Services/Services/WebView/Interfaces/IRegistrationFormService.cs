using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Services.WebView.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WebView.Interfaces
{
    public interface IRegistrationFormService
    {
        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <returns></returns>
         ReturnValueModel GetUserInfo(string code);
        /// <summary>
        /// 新增修改 参会报名表
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
         ReturnValueModel AddOrUpdate(RegistrationFormInputDto inputDto);
    }
}
