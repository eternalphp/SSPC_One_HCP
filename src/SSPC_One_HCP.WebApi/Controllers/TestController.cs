using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Aspose.Cells;
using Bot.Tool;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestController : ApiController
    {

        private readonly IADDoctorService _iADDoctorService;
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
        public TestController(IADDoctorService aDDoctorService, ICacheManager cacheManager)
        {
            _iADDoctorService = aDDoctorService;
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public IHttpActionResult GetSecret([FromUri]string secret)
        {
            /*
             * clientId:yunforcode
             * clientSecret:bCEqDYJH064j4lrVjRdUDYraxQK6faL4Hi1Y3WV/E/Q=
             */
            var ret = secret.GetHash();
            return Ok(ret);
        }

        [HttpPost]
        [HttpGet]
        public HttpResponseMessage Test()
        {


            var d = new RowNumModel<DoctorLearnViewModel>();
            d.SearchParams = new DoctorLearnViewModel();
            var ret = _iADDoctorService.GetDoctorLearn(d, true);
            var _filePath = HostingEnvironment.MapPath("/ExcelTemplate/" + "CatCityList.xlsx");

            Workbook wb = new Workbook(_filePath);
            WorkbookDesigner book = new WorkbookDesigner(wb);
            book.SetDataSource("H", ret.Result);
            book.Process();
            book.Workbook.Worksheets[0].Name = "222";
            MemoryStream stream = new MemoryStream();
            book.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] bytes = stream.ToArray();


            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-excel");
            resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmsss")}{".xlsx"}"
            };
            return resp;
        }
        [HttpGet]
        [HttpPost]
        public void Test1()
        {
            var d = new RowNumModel<DoctorLearnViewModel>();
            d.SearchParams = new DoctorLearnViewModel();
            var ret = _iADDoctorService.GetDoctorLearn(d, true);
            var _filePath = HostingEnvironment.MapPath("/ExcelTemplate/" + "学习时间模板.xlsx");
            var bytes = AsposeExcelTool.ExportTemplate(_filePath, "H", "医生学习时间", ret.Result);

            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.Buffer = true;
            curContext.Response.Charset = "UTF-8";
            curContext.Response.AddHeader("content-disposition", "attachment; filename=XiuJiaMonthlyHandel.xlsx");
            curContext.Response.ContentEncoding = Encoding.UTF8;  //必须写，否则会有乱码
            curContext.Response.ContentType = "application/octet-stream";
            curContext.Response.AddHeader("Content-Length", bytes.Length.ToString());
            curContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
            curContext.Response.Flush();
            curContext.Response.Close();
        }

    }
}
