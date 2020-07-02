using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    /// <summary>
    /// 系列课程
    /// </summary>
    public class WcSeriesCoursesService : IWcSeriesCoursesService
    {
        private readonly IEfRepository _rep;
        public WcSeriesCoursesService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 系列课程-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetSeriesCoursesList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var query = _rep.Where<SeriesCourses>(o => o.IsDeleted != 1);
            if (rowNum?.SearchParams != null)
            {
                if (rowNum?.SearchParams?.CourseTitle != null)
                {
                    query = query.Where(o => o.CourseTitle.Contains(rowNum.SearchParams.CourseTitle));
                }
            }
            var total = query.Count();
            var rows = query.OrderByDescending(o => o.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            var nowData = DateTime.UtcNow.AddHours(8);
            List<SeriesCoursesOutDto> outDtos = new List<SeriesCoursesOutDto>();
            foreach (var item in rows)
            {
                SeriesCoursesOutDto dto = new SeriesCoursesOutDto
                {
                    Id = item.Id,
                    CourseTitle = item.CourseTitle,
                    Speaker = item.Speaker,
                    Hospital = item.Hospital,
                    IsHot = item.IsHot,
                    CourseCoverSmall = item.CourseCoverSmall,
                    CourseCoverBig = item.CourseCoverBig
                };

                var meetRel = (from a in _rep.Where<SeriesCoursesMeetRel>(o => o.IsDeleted != 1)
                               join b in _rep.Where<MeetInfo>(o => o.IsDeleted != 1) on a.MeetInfoId equals b.Id
                               where
                               a.SeriesCoursesId == item.Id
                               && b.MeetEndTime >= nowData
                               && b.IsCompleted == EnumComplete.Approved
                               && string.IsNullOrEmpty(b.Source)
                               select new
                               {
                                   b.MeetStartTime,
                                   b.MeetEndTime
                               }).OrderBy(o => o.MeetStartTime).FirstOrDefault();

                if (meetRel == null)
                {
                    //dto.StateName = "已结束";
                }
                else
                {
                    dto.MeetStartTime = meetRel.MeetStartTime;
                    dto.MeetEndTime = meetRel.MeetEndTime;
                }

                outDtos.Add(dto);
            }
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = outDtos,
            };

            return rvm;
        }

        /// <summary>
        /// 系列课程- 获取明细列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetSeriesCoursesMeetRelList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (rowNum?.SearchParams?.Id == null)
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            var query = (from a in _rep.Where<SeriesCoursesMeetRel>(o => o.IsDeleted != 1)
                         join b in _rep.Where<MeetInfo>(o => o.IsDeleted != 1) on a.MeetInfoId equals b.Id
                         where
                         a.SeriesCoursesId == rowNum.SearchParams.Id
                         && b.IsCompleted == EnumComplete.Approved
                         && string.IsNullOrEmpty(b.Source)
                         select new SeriesCoursesMeetRelOutDto
                         {
                             MeetInfoId = a.MeetInfoId,
                             MeetTitle = b.MeetTitle,
                             MeetCoverSmallId = b.MeetCoverSmall,
                             MeetCoverBigId = b.MeetCoverBig,
                             MeetStartTime = b.MeetStartTime,
                             MeetEndTime = b.MeetEndTime,
                             ApprovalNote = b.ApprovalNote,
                             IsChoiceness = b.IsChoiceness,
                             IsHot = b.IsHot,
                             MeetCodeUrl = b.MeetCodeUrl,
                             MeetData = b.MeetData,
                             MeetAddress = b.MeetAddress,
                             ReplayAddress = b.ReplayAddress,
                             MeetType = b.MeetType,
                             Sort = a.Sort,
                         });

            var rows = query.OrderBy(o => o.Sort).ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            var total = query.Count();
            rows.ForEach(o =>
            {
                o.MeetCoverSmall = _rep.FirstOrDefault<MeetPic>(s => s.IsDeleted != 1 && s.MeetId == o.MeetInfoId && s.Id == o.MeetCoverSmallId)?.MeetPicUrl;
                o.MeetCoverBig = _rep.FirstOrDefault<MeetPic>(s => s.IsDeleted != 1 && s.MeetId == o.MeetInfoId && s.Id == o.MeetCoverBigId)?.MeetPicUrl;
                var meetSchedule = _rep.Where<MeetSchedule>(s => s.IsDeleted != 1 && s.MeetId == o.MeetInfoId).OrderBy(s => s.Sort).Skip(0).Take(1).FirstOrDefault();
                o.Chairman = meetSchedule?.Speaker ?? "";
                o.Hospital = meetSchedule?.Hospital ?? "";
            });

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                total = total,
                rows = rows,
            };
            return rvm;
        }
    }
}
