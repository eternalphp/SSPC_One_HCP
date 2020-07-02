using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.HCP.Dto;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.Admin;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Services.HCP
{
    /// <summary>
    /// 系列课程
    /// </summary>
    public class HcpSeriesCoursesController : BaseApiController
    {
        private readonly IHcpSeriesCoursesService _seriesCoursesService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seriesCoursesService"></param>
        public HcpSeriesCoursesController(IHcpSeriesCoursesService seriesCoursesService)
        {
            _seriesCoursesService = seriesCoursesService;
        }
        /// <summary>
        /// 系列课程-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetList(RowNumModel<SeriesCourses> rowNum)
        {
            var ret = _seriesCoursesService.GetList(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 系列课程-获取明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var ret = _seriesCoursesService.Get(id, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 系列课程-新增或修改资料信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(SeriesCoursesInputDto dto)
        {
            var ret = _seriesCoursesService.AddOrUpdate(dto, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 系列课程-删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Delete(SeriesCourses dto)
        {
            var ret = _seriesCoursesService.Delete(dto, WorkUser);
            return Ok(ret);
        }
    }
}
