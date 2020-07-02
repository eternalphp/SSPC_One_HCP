using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 名片夹
    /// </summary>
    public class WxBusinessCardController : WxBaseApiController
    {
        private readonly IWxBusinessCardService _wxBusinessCardService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public WxBusinessCardController(IWxBusinessCardService wxBusinessCardService)
        {
            _wxBusinessCardService = wxBusinessCardService;
        }

        /// <summary>
        /// 添加名片夹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddBusinessCard(AddBusinessCardViewModel businessCard)
        {
            var ret = _wxBusinessCardService.AddBusinessCard(businessCard, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 我的名片夹
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetBusinessCardList(RowNumModel<BusinessCard> rowData)
        {
            var ret = _wxBusinessCardService.GetBusinessCardList(rowData, WorkUser);
            return Ok(ret);
        }
    }
}
