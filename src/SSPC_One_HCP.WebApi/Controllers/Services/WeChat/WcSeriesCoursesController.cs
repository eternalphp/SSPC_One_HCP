using System.Web.Http;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using SSPC_One_HCP.WebApi.CustomerAuth;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.WebApi.Controllers.Services.WeChat
{
    /// <summary>
    /// 小程序 系列课程
    /// </summary>
    public class WcSeriesCoursesController : WxBaseApiController
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IWcSeriesCoursesService _wcSeriesCoursesService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wcSeriesCoursesService">小程序通用服务</param>
        public WcSeriesCoursesController(IWcSeriesCoursesService wcSeriesCoursesService)
        {
            _wcSeriesCoursesService = wcSeriesCoursesService;
        }

        /// <summary>
        /// 系列课程- 获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetSeriesCoursesList(RowNumModel<SeriesCourses> rowNum)
        {
            var result = _wcSeriesCoursesService.GetSeriesCoursesList(rowNum, WorkUser);
            return Ok(result);
        }
        /// <summary>
        /// 系列课程-  获取明细列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetSeriesCoursesMeetRelList(RowNumModel<SeriesCourses> rowNum)
        {
            var result = _wcSeriesCoursesService.GetSeriesCoursesMeetRelList(rowNum, WorkUser);
            return Ok(result);
        }
    }
}