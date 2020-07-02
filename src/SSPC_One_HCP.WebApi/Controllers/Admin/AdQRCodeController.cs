using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 推广二维码 废弃
    /// </summary>
    public class AdQRCodeController : BaseApiController
    {
        private readonly IQRCodeService _QRCodeService;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="qRCodeService"></param>
        public AdQRCodeController(IQRCodeService qRCodeService)
        {
            _QRCodeService = qRCodeService;
        }

        /// <summary>
        /// 新增或修改推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateAdQRCode(AdQRCode viewModel)
        {
            var ret = _QRCodeService.AddOrUpdateAdQRCode(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteAdQRCode(AdQRCode viewModel)
        {
            var ret = _QRCodeService.DeleteAdQRCode(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 解析推广二维码, 获取推广的公众号或小程序的相关信息, 同时二维码访问次数+1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult AnalyzeAdQRCode([FromUri] string id)
        {
            var ret = _QRCodeService.AnalyzeAdQRCode(id);
            return Ok(ret);
        }

        /// <summary>
        /// 获取推广二维码的列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAdQRCodeList(RowNumModel<AdQRCode> rowNum)
        {
            var ret = _QRCodeService.GetAdQRCodeList(rowNum);
            return Ok(ret);
        }



    }
}