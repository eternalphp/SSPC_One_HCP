using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxDiscoveryService
    {
        /// <summary>
        /// 获取微信发现页面数据
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel WxDisMainPage(WorkUser workUser);

    }
}
