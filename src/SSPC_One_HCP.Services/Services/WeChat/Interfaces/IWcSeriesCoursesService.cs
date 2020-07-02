using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcSeriesCoursesService
    {

        /// <summary>
        /// 系列课程-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
         ReturnValueModel GetSeriesCoursesList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser);

        /// <summary>
        /// 系列课程- 获取明细列表
        /// </summary>
        /// <returns></returns>
         ReturnValueModel GetSeriesCoursesMeetRelList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser);
    }
}
