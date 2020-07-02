using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcLiveBroadcastPVService
    {
        /// <summary>
        /// 记录明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel AddLiveBroadcastPV(LiveBroadcastPV dto);
        /// <summary>
        /// 查询累计点击数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ReturnValueModel GetLiveBroadcastPV(string id);


    }
}
