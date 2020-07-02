using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 资料下载
    /// </summary>
    public class WxDownLoadController : WxBaseApiController
    {
        private readonly IwxDownLoadService _iwxDownLoadService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iwxDownLoadService"></param>
        public WxDownLoadController(IwxDownLoadService iwxDownLoadService)
        {
            _iwxDownLoadService = iwxDownLoadService;
        }

        /// <summary>
        /// 显示加密后的数据
        /// </summary>
        /// <param name="UrlContent"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult DownLoadDecode(string UrlContent)
        {
            var result = _iwxDownLoadService.GetDownLoadEncryptUrl(UrlContent);
            return Ok(result);
        }

        /// <summary>
        /// 显示解密后的数据
        /// </summary>
        /// <param name="UrlContent"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetDownLoadDecryptUrl(string UrlContent)
        {
            var result = _iwxDownLoadService.GetDownLoadDecryptUrl(UrlContent);
            return Ok(result);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">文件路径</param>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DownloadFile(string url)
        {
            var browser = String.Empty;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                browser = HttpContext.Current.Request.UserAgent.ToUpper();
            }

            //路径解密
            url = EncryptHelper.AES_Decrypt(url).Replace(" ", "+");
            string filePath = AppDomain.CurrentDomain.BaseDirectory + url;
            //string filePath = url;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName =
                    browser.Contains("FIREFOX")
                        ? Path.GetFileName(filePath)
                        : HttpUtility.UrlEncode(Path.GetFileName(filePath))
                //FileName = HttpUtility.UrlEncode(Path.GetFileName(filePath))
            };

            return httpResponseMessage;
        }
        /// <summary>
        /// 临床指南解码测试
        /// </summary>
        /// <param name="UrlContent"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetDecode(string UrlContent)
        {

            var result = _iwxDownLoadService.GetDecode(UrlContent);
            return Ok(result);
        }

        /// <summary>
        /// 临床指南测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GuidTest()
        {

            var result = _iwxDownLoadService.GuidTest();
            return Ok(result);
        }
        
    }
}
