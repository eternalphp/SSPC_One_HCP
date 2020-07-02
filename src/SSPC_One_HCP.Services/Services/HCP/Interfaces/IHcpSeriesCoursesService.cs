using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.HCP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Interfaces
{
    public interface IHcpSeriesCoursesService
    {
        /// <summary>
        /// 系列课程-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser);
        /// <summary>
        /// 系列课程- 获取明细
        /// </summary>
        /// <returns></returns>
        ReturnValueModel Get(string id, WorkUser workUser);
        /// <summary>
        /// 系列课程- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdate(SeriesCoursesInputDto dto, WorkUser workUser);
        /// <summary>
        /// 系列课程- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel Delete(SeriesCourses dto, WorkUser workUser);
    }
}
