using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.HCP
{
    /// <summary>
    /// 横幅管理
    /// </summary>
    public class HcpBannerInfoController : BaseApiController
    {
        private readonly IHcpBannerInfoService _bannerInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bannerInfoService"></param>
        public HcpBannerInfoController(IHcpBannerInfoService bannerInfoService)
        {
            _bannerInfoService = bannerInfoService;
        }
        /// <summary>
        /// 横幅管理-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetList(RowNumModel<BannerInfo> rowNum)
        {
            var ret = _bannerInfoService.GetList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 横幅管理- ID获取明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetById(string id)
        {
            var ret = _bannerInfoService.GetById(id, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 横幅管理- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(BannerInfo dto)
        {
            var ret = _bannerInfoService.AddOrUpdate(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 横幅管理- 明细新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateBannerInfoItem(BannerInfoItem dto)
        {
            var ret = _bannerInfoService.AddOrUpdateBannerInfoItem(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 横幅管理- 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Delete(BannerInfo dto)
        {
            var ret = _bannerInfoService.Delete(dto, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 横幅管理- 明细删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteBannerInfoItem(BannerInfoItem dto)
        {
            var ret = _bannerInfoService.DeleteBannerInfoItem(dto, WorkUser);
            return Ok(ret);
        }
    }
}
