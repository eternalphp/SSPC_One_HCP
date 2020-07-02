using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 报表管理
    /// </summary>
    public class StatisticController : BaseApiController
    {
        private readonly IStatisticService _statisticService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="statisticService"></param>
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        /// <summary>
        /// 获取概览统计
        /// </summary>
        /// <returns>_statisticService</returns>
        [HttpPost]
        public IHttpActionResult GetOverViewList(RowNumModel<StatisticsTimeViewModel> rowNum)
        {
            var ret = _statisticService.GetOverViewList(rowNum);
            return Ok(ret);
        }


        /// <summary>
        /// 获取增长趋势
        /// </summary>
        /// <returns>_statisticService</returns>
        [HttpPost]
        public IHttpActionResult GetGrowthList(RowNumModel<StatisticsTimeViewModel> rowNum)
        {
            var ret = _statisticService.GetGrowthList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 医生学习趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDocStudyList(RowNumModel<StatisticsTimeViewModel> rowNum)
        {
            var ret = _statisticService.GetDocStudyList(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 活跃趋势 活跃趋势人数、Top 访问页、Top 访问模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetActiveList(RowNumModel<StatisticsTimeViewModel> rowNum)
        {
            var ret = _statisticService.GetActiveList(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 活跃趋势-打开次数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetOpenTimesList(RowNumModel<StatisticsTimeViewModel> rowNum)
        {
            var ret = _statisticService.GetOpenTimesList(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 获取医生列表信息
        /// 1 认证通过 ; 
        ///3,6 认证未通过人数（失败 和申诉拒绝）
        ///2 认证 未定人数 
        ///5,4 总待验证通过人数
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctor(RowNumModel<List<int>> rowNum)
        {
            var ret = _statisticService.GetDoctor(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 新增留存率
        /// 指定时间新增（即首次访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAddRetain(StatisticsTimeViewModel model)
        {
            var ret = _statisticService.GetAddRetain(model);
            return Ok(ret);
        }
        /// <summary>
        /// 活跃留存
        /// 指定时间活跃（即访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetActiveRetain(StatisticsTimeViewModel model)
        {
            var ret = _statisticService.GetActiveRetain(model);
            return Ok(ret);
        }

    }
}
