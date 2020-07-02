using System;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using System.Net;
using System.Web.Http.Results;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Tool;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.Bot.WeChat
{
    /// <summary>
    /// 智多星-文献PDF
    /// </summary>
    public class WcDocumentPDFController : WxSaleBaseApiController
    {
        private readonly IDocumentManagerService _documentManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentManager"></param>
        public WcDocumentPDFController(IDocumentManagerService documentManager)
        {
            _documentManager = documentManager;
        }
        /// <summary>
        /// 获取PDF文件流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PreviewPdf(string id)
        {
            var ret = _documentManager.PreviewPdf(id);
            if (!ret.Exists)
            {
                return new StatusCodeResult(HttpStatusCode.NotFound, this);
            }
            return new FileStreamResult(ret.OpenRead(), "application/octet-stream", $"{Guid.NewGuid().ToString()}{".pdf"}");

        }

    }
}
