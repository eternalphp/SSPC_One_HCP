using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    public class PdfController : ApiController
    {
        private readonly IWcHcpDataInfoService _weChatHcpDataInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
        public PdfController(IWcHcpDataInfoService hcpDataInfoService)
        {
            _weChatHcpDataInfoService = hcpDataInfoService;
        }
        /// <summary>
        /// 获取文件Title
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetTitle(string id)
        {
            var ret = _weChatHcpDataInfoService.GetTitle(id);
            return Ok(ret);
        }
        /// <summary>
        /// 预览PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetLiterature(string id)
        {
            var ret = _weChatHcpDataInfoService.PreviewPdf(id);
            if (!ret.Exists)
            {
                return new StatusCodeResult(HttpStatusCode.NotFound, this);
            }
            return new FileStreamResult(ret.OpenRead(), "application/octet-stream", $"{Guid.NewGuid().ToString()}{".pdf"}");

        }
    }


    public class FileStreamResult : IHttpActionResult
    {
        readonly Stream _stream;
        readonly string _mediaType;
        readonly string _fileName;

        public FileStreamResult(Stream stream, string mediaType) : this(stream, mediaType, null) { }

        public FileStreamResult(Stream stream, string mediaType, string fileName)
        {
            _stream = stream;
            _mediaType = mediaType;
            _fileName = fileName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpResponseMessage>(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                httpResponseMessage.Content = new StreamContent(_stream);
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
                if (!string.IsNullOrEmpty(_fileName))
                {
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = HttpUtility.UrlEncode(_fileName, Encoding.UTF8),
                    };
                }
                return httpResponseMessage;
            }
            catch
            {
                httpResponseMessage.Dispose();
                throw;
            }
        }
    }


    public class FileByteResult : FileStreamResult
    {
        public FileByteResult(byte[] buffer, string mediaType) : base(new MemoryStream(buffer), mediaType) { }

        public FileByteResult(byte[] buffer, string mediaType, string fileName) : base(new MemoryStream(buffer), mediaType, fileName) { }
    }
}
