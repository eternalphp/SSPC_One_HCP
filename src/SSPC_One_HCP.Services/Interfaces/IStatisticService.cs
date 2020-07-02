using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 报表
    /// </summary>
    public interface IStatisticService
    {
        /// <summary>
        /// 概览
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetOverViewList(RowNumModel<StatisticsTimeViewModel> model );

        /// <summary>
        /// 增长趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetGrowthList(RowNumModel<StatisticsTimeViewModel> model);

        /// <summary>
        /// 活跃趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetActiveList(RowNumModel<StatisticsTimeViewModel> model);


        /// <summary>
        /// 医生学习趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetDocStudyList(RowNumModel<StatisticsTimeViewModel> model);

        ReturnValueModel GetOpenTimesList(RowNumModel<StatisticsTimeViewModel> model);

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetDoctor(RowNumModel<List<int>> model);

        /// <summary>
        /// 新增留存率
        /// 指定时间新增（即首次访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetAddRetain(StatisticsTimeViewModel model);
        /// <summary>
        /// 活跃留存
        /// 指定时间活跃（即访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel GetActiveRetain(StatisticsTimeViewModel model);

    }
}
