using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    /// <summary>
    /// 小程序-横幅管理- 归属获取明细
    /// </summary>
    public class WcBannerInfoService : IWcBannerInfoService
    {
        private readonly IEfRepository _rep;
        public WcBannerInfoService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 横幅管理- 业务标签获取明细
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetByBusinessTag(string input, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(input))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Scene' is required.";
                return rvm;
            }
            var bannerInfo = _rep.FirstOrDefault<BannerInfo>(o => o.Scene == input && o.IsDeleted == 0);
            List<BannerInfoItem> bannerInfoItem = new List<BannerInfoItem>();
            if (bannerInfo != null)
            {
                bannerInfoItem = _rep.Where<BannerInfoItem>(o => o.BannerInfoId == bannerInfo.Id && o.IsDeleted == 0).OrderBy(o=>o.Sort).ToList();

            }
            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                bannerInfo,
                bannerInfoItem,
            };
            return rvm;
        }
    }
}
