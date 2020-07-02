using System.Collections.Generic;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.WxMedicine;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信-用药接口
    /// </summary>
    public class WxMedicineController   : WxBaseApiController
    {
        private readonly IWxMedicineService _iWxMedicineService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wxHomeService"></param>
        public WxMedicineController(IWxMedicineService iWxMedicineService)
        {
            _iWxMedicineService = iWxMedicineService;
        }

        /// <summary>
        /// 增加热搜记录和个人搜索记录
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult AddHotSearch(RowNumModel<MedicineHotSearchViewModel> rowNum)
        {
            var ret = _iWxMedicineService.AddHotSearch(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 批量删除个人搜索记录
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult DeleteSearchHistory(RowNumModel<WxMedicineDelViewModel> model)
        {
            var ret = _iWxMedicineService.DeleteSearchHistory(model);
            return Ok(ret);
        }

        /// <summary>
        /// 查询热搜列表和个人搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetHotSearchList(RowNumModel<MedicineHotSearch> model)
        {
            var ret = _iWxMedicineService.GetHotSearchList(model, WorkUser);
            return Ok(ret);
        }
    }
}
