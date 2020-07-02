using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcBannerInfoService
    {
        /// <summary>
        /// 横幅管理- 归属获取明细
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetByBusinessTag(string input, WorkUser workUser);
    }
}
