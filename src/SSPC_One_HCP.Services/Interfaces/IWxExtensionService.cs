using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
  public interface IWxExtensionService
    {
        /// <summary>
        /// 根据用户科室获取公众号推广
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel WxGetPublicAccount(RowNumModel<PublicAccount> publicaccount, WorkUser workUser);
        ///// <summary>
        ///// 获取访问记录
        ///// </summary>
        ///// <param name="qRcodeRecord"></param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //ReturnValueModel WxGetRecordCount(QRcodeRecord qRcodeRecord,WorkUser workUser);
    }
}
