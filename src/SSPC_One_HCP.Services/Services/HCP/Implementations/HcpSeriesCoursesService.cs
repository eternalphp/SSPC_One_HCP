using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Services.HCP.Dto;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Implementations
{
    /// <summary>
    /// 系列课程
    /// </summary>
    public class HcpSeriesCoursesService : IHcpSeriesCoursesService
    {

        private readonly IEfRepository _rep;
        public HcpSeriesCoursesService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 系列课程-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetList(RowNumModel<SeriesCourses> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var query = _rep.Where<SeriesCourses>(o => o.IsDeleted != 1);
            if (rowNum?.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum?.SearchParams?.CourseTitle))
                {
                    query = query.Where(o => o.CourseTitle.Contains(rowNum.SearchParams.CourseTitle));
                }
            }
            var total = query.Count();
            var rows = query.OrderByDescending(o => o.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows,
            };

            return rvm;
        }

        /// <summary>
        /// 系列课程- 获取明细
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel Get(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var seriesCourses = _rep.FirstOrDefault<SeriesCourses>(o => o.Id == id && o.IsDeleted == 0);
            //var seriesCoursesMeetRel = _rep.Where<SeriesCoursesMeetRel>(o => o.SeriesCoursesId == id && o.IsDeleted == 0).ToList();

            var seriesCoursesMeetRel = (from a in _rep.Where<SeriesCoursesMeetRel>(o => o.IsDeleted != 1)
                                        join b in _rep.Where<MeetInfo>(o => o.IsDeleted != 1) on a.MeetInfoId equals b.Id
                                        where
                                        a.SeriesCoursesId == id
                                        select new
                                        {
                                            Sort = a.Sort,
                                            MeetInfoId = a.MeetInfoId,
                                            MeetTitle = b.MeetTitle,
                                            MeetCoverSmall = b.MeetCoverSmall,
                                            MeetCoverBig = b.MeetCoverBig,
                                            MeetStartTime = b.MeetStartTime,
                                            MeetEndTime = b.MeetEndTime,
                                            ApprovalNote = b.ApprovalNote,
                                            IsChoiceness = b.IsChoiceness,
                                            IsHot = b.IsHot,
                                            MeetCodeUrl = b.MeetCodeUrl,
                                            MeetData = b.MeetData,
                                            MeetAddress = b.MeetAddress,
                                            ReplayAddress = b.ReplayAddress,
                                        }).OrderBy(o => o.Sort).ToList();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                seriesCourses,
                seriesCoursesMeetRel
            };
            return rvm;
        }
        /// <summary>
        /// 系列课程- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdate(SeriesCoursesInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            SeriesCourses seriesCourses = null;

            seriesCourses = _rep.FirstOrDefault<SeriesCourses>(o => o.Id == dto.Series.Id && o.IsDeleted == 0);
            if (seriesCourses == null)
            {
                seriesCourses = new SeriesCourses()
                {
                    Id = Guid.NewGuid().ToString(),
                    CourseTitle = dto.Series.CourseTitle,
                    Speaker = dto.Series.Speaker,
                    Hospital = dto.Series.Hospital,
                    IsHot = dto.Series.IsHot,
                    CourseCoverSmall = dto.Series.CourseCoverSmall,
                    CourseCoverBig = dto.Series.CourseCoverBig,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.User.Id,
                    Remark = dto.Series.Remark,
                };
                _rep.Insert(seriesCourses);
            }
            else
            {
                seriesCourses.CourseTitle = dto.Series.CourseTitle;
                seriesCourses.Speaker = dto.Series.Speaker;
                seriesCourses.Hospital = dto.Series.Hospital;
                seriesCourses.IsHot = dto.Series.IsHot;
                seriesCourses.CourseCoverSmall = dto.Series.CourseCoverSmall;
                seriesCourses.CourseCoverBig = dto.Series.CourseCoverBig;
                seriesCourses.Remark = dto.Series.Remark;
                seriesCourses.UpdateTime = DateTime.UtcNow.AddHours(8);
                seriesCourses.UpdateUser = workUser.User.Id;
                _rep.Update(seriesCourses);
            }
            var seriesCoursesMeetRel = _rep.Where<SeriesCoursesMeetRel>(o => o.SeriesCoursesId == dto.Series.Id).ToList();
            if (seriesCoursesMeetRel != null && seriesCoursesMeetRel.Count > 0)
            {
                _rep.DeleteList(seriesCoursesMeetRel);
            }
            foreach (var item in dto.SeriesCoursesMeetRels)
            {
                var model = new SeriesCoursesMeetRel()
                {
                    Id = Guid.NewGuid().ToString(),
                    SeriesCoursesId = seriesCourses.Id,
                    MeetInfoId = item.MeetInfoId,
                    Sort = item.Sort,
                    CreateTime = DateTime.UtcNow.AddHours(8),
                    CreateUser = workUser.User.Id,
                };
                _rep.Insert(model);
            }
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = seriesCourses;
            return rvm;
        }
        /// <summary>
        /// 系列课程- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel Delete(SeriesCourses dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var seriesCourses = _rep.FirstOrDefault<SeriesCourses>(o => o.Id == dto.Id && o.IsDeleted == 0);
            seriesCourses.IsDeleted = 1;
            seriesCourses.UpdateTime = DateTime.UtcNow.AddHours(8);
            seriesCourses.UpdateUser = workUser.User.Id;
            _rep.Update(seriesCourses);
            var seriesCoursesMeetRel = _rep.Where<SeriesCoursesMeetRel>(o => o.SeriesCoursesId == dto.Id && o.IsDeleted == 0).ToList();
            foreach (var item in seriesCoursesMeetRel)
            {
                item.IsDeleted = 1;
                item.UpdateTime = DateTime.UtcNow.AddHours(8);
                item.UpdateUser = workUser.User.Id;
                _rep.Update(item);
            }
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }
    }
}
