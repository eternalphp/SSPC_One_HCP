using System;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using System.Net;
using System.Web.Http.Results;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Tool;

namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// 小程序-资料库
    /// </summary>
    public class WeChatHcpDataInfoController : WxSaleBaseApiController
    {
        private readonly IWcHcpDataInfoService _weChatHcpDataInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
        public WeChatHcpDataInfoController(IWcHcpDataInfoService hcpDataInfoService)
        {
            _weChatHcpDataInfoService = hcpDataInfoService;
        }
        /// <summary>
        /// 目录列表
        /// </summary>
        /// <param name="buName"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFileList(string buName)
        {
            var ret = _weChatHcpDataInfoService.GetFileList(buName, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 分页查询资料列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDataPageList(RowNumModel<WeChatHcpDataInfoInputDto> rowNum)
        {
            var ret = _weChatHcpDataInfoService.GetDataPageList(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetData(string id)
        {
            var ret = _weChatHcpDataInfoService.GetData(id, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 预览PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
       //[Route("{id}")]
        [AllowAnonymous]
        public IHttpActionResult PreviewPdf(string id)
        {
            var ret = _weChatHcpDataInfoService.PreviewPdf(id);
            if (!ret.Exists)
            {
                return new StatusCodeResult(HttpStatusCode.NotFound, this);
            }
            return new FileStreamResult(ret.OpenRead(), "application/octet-stream", $"{Guid.NewGuid().ToString()}{".pdf"}");

        }


        /// <summary>
        /// 资料是否显示红点
        /// </summary>
        /// <param name="buName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetRedDot(string buName)
        {
            var ret = _weChatHcpDataInfoService.GetRedDot(buName, null);
            return Ok(ret);
        }
    }


}
