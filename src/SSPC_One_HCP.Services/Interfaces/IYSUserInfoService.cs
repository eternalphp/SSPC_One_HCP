using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IYSUserInfoService
    {

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="unionid"></param>
        /// <returns></returns>
        ReturnValueModel GetUserInfo(string unionid);

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="unionid"></param>
        /// <returns></returns>
        ReturnValueModel DelUserInfo(string unionid);

    }
}
