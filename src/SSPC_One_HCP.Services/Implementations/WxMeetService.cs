using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Implementations.Dto;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{

    /// <summary>
    /// 会议信息
    /// </summary>
    public class MeetInfoModel
    {
        public string Id { get; set; }
        public string MeetTitle { get; set; }

        public string MeetSubject { get; set; }


        public int? MeetType { get; set; }


        public string MeetDep { get; set; }


        public string MeetIntroduction { get; set; }


        public DateTime? MeetStartTime { get; set; }


        public DateTime? MeetEndTime { get; set; }

        public DateTime? MeetDate { get; set; }


        public string Speaker { get; set; }


        public string SpeakerDetail { get; set; }


        public string MeetAddress { get; set; }


        public string ReplayAddress { get; set; }


        public string MeetData { get; set; }


        public string MeetCodeUrl { get; set; }

        public string MeetCity { get; set; }

        public int MeetingNumber { get; set; }

        public string MeetSite { get; set; }

        public string MeetCoverSmall { get; set; }

        public string MeetCoverBig { get; set; }

        public int? IsCompleted { get; set; }



        public int? IsChoiceness { get; set; }

        public int? IsHot { get; set; }

        public string OldId { get; set; }


        public string ApprovalNote { get; set; }


        public string Source { get; set; }
        public string SourceId { get; set; }
        public string SourceHospital { get; set; }
        public string SourceDepartment { get; set; }
        public int HasReminded { get; set; }

    }
    public class WxMeetService : IWxMeetService
    {
        private readonly IEfRepository _rep;
        private readonly IConfig _config;

        public WxMeetService(IEfRepository rep, IConfig config)
        {
            _rep = rep;
            _config = config;
        }
        /// <summary>
        /// 获取会议
        /// </summary>
        /// <param name="meetInfo">会议信息，之传入Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetDetail(MeetInfo meetInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //会议信息
            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfo.Id);
            if (meet == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meeting id";
                return rvm;
            }

            //会议资料
            var meetFile = _rep.Where<MeetFile>(s => s.MeetId == meetInfo.Id).ToList();
            List<MeetFile> meetFilelist = _rep.Where<MeetFile>(s => s.MeetId == meetInfo.Id).ToList();
            foreach (var model in meetFilelist)
            {
                //下载路径加密
                model.Remark = "api/WxDownLoad/DownloadFile?url=" + EncryptHelper.AES_Encrypt(model.FilePath);
            }

            //会议日程 及 讲者
            var meetSchedule = (from a in _rep.Table<MeetSchedule>()
                                join b in _rep.Table<MeetSpeaker>() on a.MeetSpeakerId equals b.Id
                                orderby a.ScheduleStart descending
                                where a.MeetId == meetInfo.Id && b.MeetId == meetInfo.Id
                                                              && a.IsDeleted != 1 && b.IsDeleted != 1
                                group new { a, b } by new { a.Remark } into g1
                                select new MeetScheduleViewModel
                                {
                                    ScheduleDate = g1.Key.Remark,
                                    ScheduleViews = from v1 in g1
                                                    orderby v1.a.ScheduleStart
                                                    select new ScheduleView
                                                    {
                                                        Schedule = new MeetScheduleView
                                                        {
                                                            Id = v1.a.Id,
                                                            MeetId = v1.a.MeetId,
                                                            ScheduleContent = v1.a.ScheduleContent,
                                                            MeetSpeakerId = v1.a.MeetSpeakerId,
                                                            ScheduleStart = v1.a.ScheduleStart.HasValue ? (SqlFunctions.DatePart("HOUR", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleStart) : SqlFunctions.DateName("HOUR", v1.a.ScheduleStart)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart)) : "00:00:00",
                                                            ScheduleEnd = v1.a.ScheduleEnd.HasValue ? (SqlFunctions.DatePart("HOUR", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd) : SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd)) : "00:00:00"
                                                        },
                                                        Speaker = v1.b
                                                    }
                                }).ToList();

            var meetSpeaker = _rep.Where<MeetSpeaker>(s => s.IsDeleted != 1 && s.MeetId == meetInfo.Id).ToList();

            var meetOrder =
                _rep.FirstOrDefault<MyMeetOrder>(s => s.IsDeleted != 1 &&
                    s.MeetId == meetInfo.Id && s.UnionId == workUser.WxUser.UnionId);

            var meetSignUp =
                _rep.FirstOrDefault<MeetSignUp>(s => s.IsDeleted != 1 &&
                    s.MeetId == meetInfo.Id && s.SignUpUserId == workUser.WxUser.Id);

            var isCollection = _rep.Where<MyCollectionInfo>(s => s.IsDeleted != 1 &&
                    s.UnionId == workUser.WxUser.UnionId && s.CollectionDataId == meetInfo.Id && s.CollectionType == 5)
                .Any()
                ? 1
                : 2;

            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1 && s.MeetId == meet.Id);
            meet.MeetCoverSmall = (from b in pic
                                   where b.Id == meet.MeetCoverSmall
                                   select b.MeetPicUrl).FirstOrDefault();
            meet.MeetCoverBig = (from b in pic
                                 where b.Id == meet.MeetCoverBig
                                 select b.MeetPicUrl).FirstOrDefault();

            //是否有会前问卷
            bool hasQA1 = _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 1).Any();

            //是否有会后问卷
            bool hasQA2 = _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 2).Any();

            //是否做过会前问卷
            bool didQA1 = hasQA1 &&
                (from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.SignUpUserId == workUser.WxUser.Id)
                 join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 1) on a.MeetQAId equals b.Id
                 select a).Any();

            //是否做过会后问卷
            bool didQA2 = hasQA2 &&
                (from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.SignUpUserId == workUser.WxUser.Id)
                 join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 2) on a.MeetQAId equals b.Id
                 select a).Any();

            //是否需要弹出问卷
            int showQAType = 0;
            if (meetOrder != null || (meetSignUp != null && meetSignUp.IsSignIn == 1))  //已报名或者已签到的才弹出问卷
            {
                //根据当前时间判断是否需要弹出问卷
                if (hasQA1 && !didQA1 && meet.MeetStartTime.HasValue && meet.MeetStartTime > DateTime.Now && meet.MeetStartTime.Value.AddMinutes(-30) <= DateTime.Now)
                {
                    showQAType = 1;
                }
                else if (hasQA2 && !didQA2 && meet.MeetEndTime.HasValue && meet.MeetEndTime < DateTime.Now && meet.MeetEndTime.Value.AddMinutes(30) >= DateTime.Now)
                {
                    showQAType = 2;
                }
            }

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                meet,
                meetFile = meetFilelist,
                meetSchedule,
                meetSpeaker,
                isSignUp = meetOrder != null ? 1 : 2,
                meetOrder = meetOrder,
                isCollection = isCollection,
                tel = workUser.WxUser.Mobile.Substring(7),
                shouldShowQA = showQAType, //是否需要弹出问卷
                hasQA1,  //是否有会前问卷
                hasQA2,  //是否有会后问卷
                didQA1,  //是否做过会前问卷
                didQA2   //是否做过会后问卷
            };

            return rvm;
        }

        /// <summary>
        /// 获取历史参会列表
        /// </summary>
        /// <param name="rowMeet">分页</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public ReturnValueModel GetHistoryMeeting(RowNumModel<MeetInfo> rowMeet, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);
            #region  线下签到会议列表
            var list = (from a in _rep.Table<MeetInfo>()
                        join b in _rep.Table<MeetSignUp>() on a.Id equals b.MeetId
                        join c in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on b.SignUpUserId equals c.Id
                        orderby a.MeetDate descending
                        where a.IsDeleted != 1 && b.IsDeleted != 1 && c.IsDeleted != 1 &&
                              b.IsSignIn == 1 && c.Id == workUser.WxUser.Id &&
                              (a.IsCompleted == EnumComplete.Approved)
                        select new
                        {
                            Id = a.Id,
                            MeetTitle = a.MeetTitle,
                            MeetSubject = a.MeetSubject,
                            MeetType = a.MeetType,
                            MeetDep = a.MeetDep,
                            MeetIntroduction = a.MeetIntroduction,
                            MeetStartTime = a.MeetStartTime,
                            MeetEndTime = a.MeetEndTime,
                            CreateTime = a.CreateTime,
                            MeetDate = a.MeetDate,
                            Speaker = a.Speaker,
                            SpeakerDetail = a.SpeakerDetail,
                            MeetAddress = a.MeetAddress,
                            ReplayAddress = a.ReplayAddress,
                            MeetData = a.MeetData,
                            MeetCodeUrl = a.MeetCodeUrl,
                            MeetCity = a.MeetCity,
                            MeetingNumber = a.MeetingNumber,
                            MeetSite = a.MeetSite,
                            MeetCoverSmall = (from b in pic
                                              where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                              select b.MeetPicUrl).FirstOrDefault(),
                            MeetCoverBig = (from b in pic
                                            where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                            select b.MeetPicUrl).FirstOrDefault(),
                            IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                            ApprovalNote = a.ApprovalNote,
                            IsChoiceness = a.IsChoiceness,
                            IsHot = a.IsHot,
                        }).ToList();

            #endregion

            #region 线上报名会议
            var offlinelist = (from a in _rep.Table<MeetInfo>()
                               join b in _rep.Table<MyMeetOrder>() on a.Id equals b.MeetId
                               join c in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on b.CreateUser equals c.Id
                               orderby a.MeetDate descending
                               where a.IsDeleted != 1 && b.IsDeleted != 1 && c.IsDeleted != 1 &&
                                     c.Id == workUser.WxUser.Id &&
                                     (a.IsCompleted == EnumComplete.Approved)
                               select new
                               {
                                   Id = a.Id,
                                   MeetTitle = a.MeetTitle,
                                   MeetSubject = a.MeetSubject,
                                   MeetType = a.MeetType,
                                   MeetDep = a.MeetDep,
                                   MeetIntroduction = a.MeetIntroduction,
                                   MeetStartTime = a.MeetStartTime,
                                   MeetEndTime = a.MeetEndTime,
                                   CreateTime = a.CreateTime,
                                   MeetDate = a.MeetDate,
                                   Speaker = a.Speaker,
                                   SpeakerDetail = a.SpeakerDetail,
                                   MeetAddress = a.MeetAddress,
                                   ReplayAddress = a.ReplayAddress,
                                   MeetData = a.MeetData,
                                   MeetCodeUrl = a.MeetCodeUrl,
                                   MeetCity = a.MeetCity,
                                   MeetingNumber = a.MeetingNumber,
                                   MeetSite = a.MeetSite,
                                   MeetCoverSmall = (from b in pic
                                                     where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                                     select b.MeetPicUrl).FirstOrDefault(),
                                   MeetCoverBig = (from b in pic
                                                   where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                                   select b.MeetPicUrl).FirstOrDefault(),
                                   IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                                   ApprovalNote = a.ApprovalNote,
                                   IsChoiceness = a.IsChoiceness,
                                   IsHot = a.IsHot,
                               }).ToList();
            #endregion

            if (offlinelist.Count > 0)
            {
                list.AddRange(offlinelist);
            }

            //历史会议 只显示已经结束的
            list = list.Where(s => s.MeetEndTime <= DateTime.Now).ToList();
            var total = list.Count;
            var rows = list.OrderByDescending(o => o.MeetStartTime).ToPaginationList(rowMeet.PageIndex, rowMeet.PageSize);

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }
        /// <summary>
        /// 获取HOT会议列表 发现页面
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetList(RowNumModel<MeetInfo> rowNum, WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间

            ReturnValueModel rvm = new ReturnValueModel() { };
            var wheresql = string.Empty;
            if (!string.IsNullOrEmpty(workUser?.WxUser?.DepartmentName))
            {
                wheresql = $" AND c.DepartmentName = '{workUser.WxUser.DepartmentName}' ";
            }
            //当前用户允许访问的会议 
            var allDoctorID = "00000000-0000-0000-0000-000000000000";
            var doctorMeetingList = _rep.Where<DoctorMeeting>(x => (x.DoctorID.Equals(workUser.WxUser.Id) || x.DoctorID.Equals(allDoctorID)) && !string.IsNullOrEmpty(x.DoctorID)).Select(x => x.MeetingID).ToList();

            if (doctorMeetingList.Count() > 0)
            {
                wheresql += $" AND a.Id in('{string.Join("','", doctorMeetingList)}') ";
            }
            //Redmine #4054
            //1.后台创建的会议必须是设为推荐的才显示，没有勾选推荐不显示;
            //2.只显示两条，排序按照会议开始时间显示;
            //3.如果可推荐的只有一条，从没有结束的会议中挑一条，即使没有设为推荐也显示过来;
            var sql = $@"
SELECT TOP 2 *
FROM (
		SELECT DISTINCT TOP 2 a.Id, a.MeetTitle, a.MeetSubject, a.MeetType, a.MeetDep
			, a.MeetIntroduction, a.MeetStartTime, a.MeetEndTime, a.CreateTime, a.MeetDate
			, a.Speaker, a.SpeakerDetail, a.MeetAddress, a.ReplayAddress, a.MeetData
			, a.MeetCodeUrl, a.MeetCity, a.MeetingNumber, a.MeetSite
			, convert(int, ISNULL(a.IsCompleted, 2)) AS IsCompleted
			, a.ApprovalNote, a.IsChoiceness, a.IsHot, d.MeetPicUrl AS MeetCoverSmall, e.MeetPicUrl AS MeetCoverBig
		FROM MeetInfo a
			JOIN MeetAndProAndDepRelation b ON a.Id = b.MeetId
			JOIN DepartmentInfo c ON b.DepartmentId = c.Id
			JOIN MeetPic d ON d.id = a.MeetCoverSmall
			JOIN MeetPic e ON e.id = a.MeetCoverSmall
		WHERE a.IsCompleted = 1
			AND a.IsDeleted != 1
			AND a.MeetStartTime >= GETDATE()
			{wheresql}
		ORDER BY a.IsHot DESC, a.MeetStartTime
	UNION
		SELECT DISTINCT TOP 2 a.Id, a.MeetTitle, a.MeetSubject, a.MeetType, a.MeetDep
			, a.MeetIntroduction, a.MeetStartTime, a.MeetEndTime, a.CreateTime, a.MeetDate
			, a.Speaker, a.SpeakerDetail, a.MeetAddress, a.ReplayAddress, a.MeetData
			, a.MeetCodeUrl, a.MeetCity, a.MeetingNumber, a.MeetSite
			, convert(int, ISNULL(a.IsCompleted, 2)) AS IsCompleted
			, a.ApprovalNote, a.IsChoiceness, a.IsHot, d.MeetPicUrl AS MeetCoverSmall, e.MeetPicUrl AS MeetCoverBig
		FROM MeetInfo a
			JOIN MeetAndProAndDepRelation b ON a.Id = b.MeetId
			JOIN DepartmentInfo c ON b.DepartmentId = c.Id
			JOIN MeetPic d ON d.id = a.MeetCoverSmall
			JOIN MeetPic e ON e.id = a.MeetCoverSmall
		WHERE a.IsCompleted = 1
			AND a.MeetStartTime <= getdate()
			AND a.IsDeleted != 1
			{wheresql}
		ORDER BY a.IsHot DESC, a.MeetStartTime DESC
) n
ORDER BY MeetStartTime DESC
";

            try
            {
                var list = _rep.SqlQuery<MeetInfoModel>(sql).OrderBy(x => x.MeetStartTime).ToList();
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    total = list.Count(),
                    rows = list
                };

            }
            catch (Exception e)
            {
                LoggerHelper.WarnInTimeTest("WxMeetService/GetMeetList" + e.Message);
                rvm.Success = false;
                rvm.Msg = e.Message;
            }
            finally
            {
                stopwatch.Stop();//结束
                rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            }
            return rvm;
        }


        /// <summary>
        /// 获取所有会议列表 推荐会议和历史会议
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetLists(RowNumModel<MeetInfo> rowNum, WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间

            //LoggerHelper.WarnInTimeTest("Inner-[GetMeetLists] Start:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            ReturnValueModel rvm = new ReturnValueModel();

            //会议图片
            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);

            //所有部门
            var deptlist = _rep.Where<DepartmentInfo>(x => true);

            if (workUser.WxUser != null)
            {
                //用户部门过滤
                if (!string.IsNullOrEmpty(workUser.WxUser.DepartmentName))
                {
                    deptlist = deptlist.Where(a => a.DepartmentName == workUser.WxUser.DepartmentName);
                }
            }
            //当前会议没有有效医生,则为对所有人开放
            var allDoctorID = "00000000-0000-0000-0000-000000000000";
            var doctorMeetingList = _rep.Where<DoctorMeeting>(x => (x.DoctorID.Equals(workUser.WxUser.Id) || x.DoctorID.Equals(allDoctorID)) && !string.IsNullOrEmpty(x.DoctorID)).Select(x => x.MeetingID).ToList();

            var NowData = DateTime.UtcNow.AddHours(8);

            #region  推荐会议 只显示本部门对应的未开始的会议

            var recommandlist = (from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1).Where(s => s.MeetEndTime >= NowData)
                                 join e in _rep.Table<MeetAndProAndDepRelation>() on a.Id equals e.MeetId
                                 join c in deptlist on e.DepartmentId equals c.Id
                                 where a.IsCompleted == EnumComplete.Approved && string.IsNullOrEmpty(a.Source)
                                 select new MeetSearchOutDto
                                 {
                                     Id = a.Id,
                                     MeetTitle = a.MeetTitle,
                                     MeetSubject = a.MeetSubject,
                                     MeetType = a.MeetType,
                                     MeetDep = a.MeetDep,
                                     MeetIntroduction = a.MeetIntroduction,
                                     MeetStartTime = a.MeetStartTime,
                                     MeetEndTime = a.MeetEndTime,
                                     CreateTime = a.CreateTime,
                                     MeetDate = a.MeetDate,
                                     Speaker = a.Speaker,
                                     SpeakerDetail = a.SpeakerDetail,
                                     MeetAddress = a.MeetAddress,
                                     ReplayAddress = a.ReplayAddress,
                                     MeetData = a.MeetData,
                                     MeetCodeUrl = a.MeetCodeUrl,
                                     MeetCity = a.MeetCity,
                                     MeetingNumber = a.MeetingNumber,
                                     MeetSite = a.MeetSite,
                                     MeetCoverSmall = (from b in pic
                                                       where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                                       select b.MeetPicUrl).FirstOrDefault(),
                                     MeetCoverBig = (from b in pic
                                                     where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                                     select b.MeetPicUrl).FirstOrDefault(),
                                     IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                                     ApprovalNote = a.ApprovalNote,
                                     IsChoiceness = a.IsChoiceness,
                                     IsHot = a.IsHot
                                 }).Distinct().OrderBy(s => s.MeetStartTime).ToList();
            //筛选当前医生
            if (doctorMeetingList.Count() > 0)
            {
                recommandlist = recommandlist.Where(x => doctorMeetingList.Contains(x.Id)).ToList();
            }
            #endregion

            #region 已结束的会议
            var endlist = (from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1).Where(rowNum.SearchParams)
                           .Where(s => s.MeetEndTime < NowData && (s.MeetType == 1 || s.MeetType == 4))
                           join e in _rep.Table<MeetAndProAndDepRelation>() on a.Id equals e.MeetId
                           join c in deptlist on e.DepartmentId equals c.Id
                           where a.IsCompleted == EnumComplete.Approved && string.IsNullOrEmpty(a.Source)
                           select new MeetSearchOutDto
                           {
                               Id = a.Id,
                               MeetTitle = a.MeetTitle,
                               MeetSubject = a.MeetSubject,
                               MeetType = a.MeetType,
                               MeetDep = a.MeetDep,
                               MeetIntroduction = a.MeetIntroduction,
                               MeetStartTime = a.MeetStartTime,
                               MeetEndTime = a.MeetEndTime,
                               CreateTime = a.CreateTime,
                               MeetDate = a.MeetDate,
                               Speaker = a.Speaker,
                               SpeakerDetail = a.SpeakerDetail,
                               MeetAddress = a.MeetAddress,
                               ReplayAddress = a.ReplayAddress,
                               MeetData = a.MeetData,
                               MeetCodeUrl = a.MeetCodeUrl,
                               MeetCity = a.MeetCity,
                               MeetingNumber = a.MeetingNumber,
                               MeetSite = a.MeetSite,
                               MeetCoverSmall = (from b in pic
                                                 where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                                 select b.MeetPicUrl).FirstOrDefault(),
                               MeetCoverBig = (from b in pic
                                               where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                               select b.MeetPicUrl).FirstOrDefault(),
                               IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                               ApprovalNote = a.ApprovalNote,
                               IsChoiceness = a.IsChoiceness,
                               IsHot = a.IsHot
                           }).Distinct().OrderByDescending(s => s.MeetEndTime).ToList();

            //筛选当前医生
            if (doctorMeetingList.Count() > 0)
            {
                endlist = endlist.Where(x => doctorMeetingList.Contains(x.Id)).ToList();
            }
            #endregion

            rvm.Success = true;
            rvm.Msg = "";
            var total = endlist.Count();
            endlist = endlist.ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            recommandlist.ForEach(o =>
            {
                var meetSchedule = _rep.Where<MeetSchedule>(s => s.IsDeleted != 1 && s.MeetId == o.Id).OrderBy(s => s.Sort).Skip(0).Take(1).FirstOrDefault();
                o.Chairman = meetSchedule?.Speaker ?? "";
                o.Hospital = meetSchedule?.Hospital ?? "";
            });
            endlist.ForEach(o =>
            {
                var meetSchedule = _rep.Where<MeetSchedule>(s => s.IsDeleted != 1 && s.MeetId == o.Id).OrderBy(s => s.Sort).Skip(0).Take(1).FirstOrDefault();
                o.Chairman = meetSchedule?.Speaker ?? "";
                o.Hospital = meetSchedule?.Hospital ?? "";

            });

            rvm.Result = new
            {
                total,
                rows = new
                {
                    recommandlist = recommandlist,
                    endlist = endlist
                }
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }
        /// <summary>
        /// 搜索 条件
        /// </summary>
        /// <param name="type">1：推荐 2：历史</param>
        /// <param name="workUser"></param>
        /// <returns></returns>

        public ReturnValueModel GetDepartmentInfo(int type, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var deptlist = _rep.Where<DepartmentInfo>(o => o.IsDeleted != 1).OrderByDescending(o => o.Remark).Select(o => new
            {
                o.DepartmentName,
                o.Id,
            }).ToList();
            var popularList = _rep.Where<DepartmentInfo>(o => o.IsDeleted != 1).OrderByDescending(o => o.CompanyCode).Skip(0).Take(5).Select(o => new
            {
                o.DepartmentName,
                o.Id,
            }).ToList();

            List<int> years = new List<int>();
            var nowData = DateTime.UtcNow.AddHours(8);
            if (type == 1)
            {
                years.Add(nowData.Year);
                years.Add(nowData.Year + 1);
            }
            else
            {
                years.Add(nowData.Year);
                years.Add(nowData.Year - 1);
            }

            rvm.Success = true;
            rvm.Msg = "";

            rvm.Result = new
            {
                popularList,
                deptlist,
                years
            };

            return rvm;
        }
        /// <summary>
        /// 推荐会议搜索
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetRecommendSearch(RowNumModel<MeetSearchInputDto> row, WorkUser workUser)
        {

            ReturnValueModel rvm = new ReturnValueModel();

            //会议图片
            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);

       
            var nowData = DateTime.UtcNow.AddHours(8);


            var recommandlist = (from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1)/*.Where(s => s.MeetEndTime >= nowData)*/
                                                                                              //join e in meetAndProAndDepRelation on a.Id equals e.MeetId
                                                                                              //join c in deptlist on e.DepartmentId equals c.Id
                                 where a.IsCompleted == EnumComplete.Approved && string.IsNullOrEmpty(a.Source)
                                 select new MeetSearchOutDto
                                 {
                                     Id = a.Id,
                                     MeetTitle = a.MeetTitle,
                                     MeetSubject = a.MeetSubject,
                                     MeetType = a.MeetType,
                                     MeetDep = a.MeetDep,
                                     MeetIntroduction = a.MeetIntroduction,
                                     MeetStartTime = a.MeetStartTime,
                                     MeetEndTime = a.MeetEndTime,
                                     CreateTime = a.CreateTime,
                                     MeetDate = a.MeetDate,
                                     Speaker = a.Speaker,
                                     SpeakerDetail = a.SpeakerDetail,
                                     MeetAddress = a.MeetAddress,
                                     ReplayAddress = a.ReplayAddress,
                                     MeetData = a.MeetData,
                                     MeetCodeUrl = a.MeetCodeUrl,
                                     MeetCity = a.MeetCity,
                                     MeetingNumber = a.MeetingNumber,
                                     MeetSite = a.MeetSite,
                                     MeetCoverSmall = (from b in pic
                                                       where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                                       select b.MeetPicUrl).FirstOrDefault(),
                                     MeetCoverBig = (from b in pic
                                                     where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                                     select b.MeetPicUrl).FirstOrDefault(),
                                     IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                                     ApprovalNote = a.ApprovalNote,
                                     IsChoiceness = a.IsChoiceness,
                                     IsHot = a.IsHot,
                                     MeetState = nowData > a.MeetEndTime ? 2 : nowData < a.MeetStartTime ? 0 : 1,
                                 }).Distinct();


            if (row.SearchParams.Years != null && row.SearchParams.Years.Count() > 0)
            {
                recommandlist = recommandlist.Where(x => row.SearchParams.Years.Contains(x.MeetStartTime.Value.Year));
            }
            if (row.SearchParams.Months != null && row.SearchParams.Months.Count() > 0)
            {
                recommandlist = recommandlist.Where(x => row.SearchParams.Months.Contains(x.MeetStartTime.Value.Month));
            }
            if (row.SearchParams.MeetStates != null && row.SearchParams.MeetStates.Count() > 0)
            {
                recommandlist = recommandlist.Where(x => row.SearchParams.MeetStates.Contains(x.MeetState));
            }
            if (row.SearchParams.DepartmentIds != null && row.SearchParams.DepartmentIds.Count() > 0)
            {
                var ids = _rep.Where<MeetAndProAndDepRelation>(o => row.SearchParams.DepartmentIds.Contains(o.DepartmentId) && o.DepartmentType == 1).GroupBy(o => o.MeetId).Select(o => o.Key).ToList();

                recommandlist = recommandlist.Where(x => ids.Contains(x.Id));
            }


            var res = recommandlist.OrderByDescending(s => s.MeetStartTime).ToPaginationList(row.PageIndex, row.PageSize).ToList();


            res.ForEach(o =>
            {
                var meetSchedule = _rep.Where<MeetSchedule>(s => s.IsDeleted != 1 && s.MeetId == o.Id).OrderBy(s => s.Sort).Skip(0).Take(1).FirstOrDefault();
                o.Chairman = meetSchedule?.Speaker ?? "";
                o.Hospital = meetSchedule?.Hospital ?? "";
                if (o.MeetState == 2)
                {
                    o.MeetStateName = "已结束";
                }
                else
                {
                    TimeSpan startTime = new TimeSpan(nowData.Ticks);
                    TimeSpan endTime = new TimeSpan(o.MeetStartTime.Value.Ticks);
                    TimeSpan ts = startTime.Subtract(endTime).Duration();
                    if (ts.Days > 0)
                        o.MeetStateName = $"{ts.Days.ToString()}天{ts.Hours.ToString()}小时{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                    else if (ts.Hours > 0)
                        o.MeetStateName = $"{ts.Hours.ToString()}小时{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                    else if (ts.Minutes > 0)
                        o.MeetStateName = $"{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                    else if (ts.Seconds > 0)
                        o.MeetStateName = $"{ts.Seconds.ToString()}秒";
                    else
                        o.MeetStateName = "正在直播";
                }
            });


            rvm.Success = true;
            rvm.Msg = "";
            var total = recommandlist.Count();

            rvm.Result = new
            {
                total,
                list = res,
            };

            return rvm;
        }
        /// <summary>
        /// 历史会议搜索
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetHistorySearch(RowNumModel<MeetSearchInputDto> row, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //会议图片
            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);

            //所有部门
            //var deptlist = _rep.Where<DepartmentInfo>(x => true);
            //if (row.SearchParams.DepartmentIds != null && row.SearchParams.DepartmentIds.Count() > 0)
            //{
            //    deptlist = deptlist.Where(o => row.SearchParams.DepartmentIds.Contains(o.Id));
            //}

            //if (workUser.WxUser != null)
            //{
            //    //用户部门过滤
            //    if (!string.IsNullOrEmpty(workUser.WxUser.DepartmentName))
            //    {
            //        deptlist = deptlist.Where(a => a.DepartmentName == workUser.WxUser.DepartmentName);
            //    }
            //}
            //当前会议没有有效医生,则为对所有人开放
            //var allDoctorID = "00000000-0000-0000-0000-000000000000";
            //var doctorMeetingList = _rep.Where<DoctorMeeting>(x => (x.DoctorID.Equals(workUser.WxUser.Id) || x.DoctorID.Equals(allDoctorID)) && !string.IsNullOrEmpty(x.DoctorID)).Select(x => x.MeetingID).ToList();

            var NowData = DateTime.UtcNow.AddHours(8);
            var meetAndProAndDepRelation = _rep.Where<MeetAndProAndDepRelation>(x => true);
            if (row.SearchParams.DepartmentIds != null && row.SearchParams.DepartmentIds.Count() > 0)
            {
                meetAndProAndDepRelation = meetAndProAndDepRelation.Where(o => row.SearchParams.DepartmentIds.Contains(o.DepartmentId));
            }


            #region 已结束的会议
            var endlist = (from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1).Where(s => s.MeetEndTime < NowData && (s.MeetType == 1 || s.MeetType == 4))
                           join e in meetAndProAndDepRelation on a.Id equals e.MeetId
                           //join c in deptlist on e.DepartmentId equals c.Id
                           where a.IsCompleted == EnumComplete.Approved && string.IsNullOrEmpty(a.Source)
                           select new MeetSearchOutDto
                           {
                               Id = a.Id,
                               MeetTitle = a.MeetTitle,
                               MeetSubject = a.MeetSubject,
                               MeetType = a.MeetType,
                               MeetDep = a.MeetDep,
                               MeetIntroduction = a.MeetIntroduction,
                               MeetStartTime = a.MeetStartTime,
                               MeetEndTime = a.MeetEndTime,
                               CreateTime = a.CreateTime,
                               MeetDate = a.MeetDate,
                               Speaker = a.Speaker,
                               SpeakerDetail = a.SpeakerDetail,
                               MeetAddress = a.MeetAddress,
                               ReplayAddress = a.ReplayAddress,
                               MeetData = a.MeetData,
                               MeetCodeUrl = a.MeetCodeUrl,
                               MeetCity = a.MeetCity,
                               MeetingNumber = a.MeetingNumber,
                               MeetSite = a.MeetSite,
                               MeetCoverSmall = (from b in pic
                                                 where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                                 select b.MeetPicUrl).FirstOrDefault(),
                               MeetCoverBig = (from b in pic
                                               where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                               select b.MeetPicUrl).FirstOrDefault(),
                               IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                               ApprovalNote = a.ApprovalNote,
                               IsChoiceness = a.IsChoiceness,
                               IsHot = a.IsHot
                           }).Distinct();

            //筛选当前医生
            //if (doctorMeetingList.Count() > 0)
            //{
            //    endlist = endlist.Where(x => doctorMeetingList.Contains(x.Id));
            //}
            #endregion

            if (row.SearchParams.Years != null && row.SearchParams.Years.Count() > 0)
            {
                endlist = endlist.Where(x => row.SearchParams.Years.Contains(x.MeetStartTime.Value.Year));
            }
            if (row.SearchParams.Months != null && row.SearchParams.Months.Count() > 0)
            {
                endlist = endlist.Where(x => row.SearchParams.Months.Contains(x.MeetStartTime.Value.Month));
            }

            var res = endlist.OrderByDescending(s => s.MeetEndTime).ToPaginationList(row.PageIndex, row.PageSize).ToList();
            res.ForEach(o =>
            {
                var meetSchedule = _rep.Where<MeetSchedule>(s => s.IsDeleted != 1 && s.MeetId == o.Id).OrderBy(s => s.Sort).Skip(0).Take(1).FirstOrDefault();
                o.Chairman = meetSchedule?.Speaker ?? "";
                o.Hospital = meetSchedule?.Hospital ?? "";
            });

            rvm.Success = true;
            rvm.Msg = "";
            var total = endlist.Count();

            rvm.Result = new
            {
                total,
                list = res,
            };

            return rvm;
        }

        /// <summary>
        /// 获取会议的日历
        /// </summary>
        /// <param name="workUser">当前登录人</param>
        /// <param name="dateTime">年月</param>
        /// <returns></returns>
        public ReturnValueModel GetMeetCalendar(WorkUser workUser, DateTime? dateTime = null)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();

            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }

            int year = dateTime.Value.Year;
            int month = dateTime.Value.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1); //不含

            string wxUserId = workUser?.WxUser?.Id;
            string unionId = workUser?.WxUser?.UnionId;

            //当月的所有会议日程安排
            var schedules = from b in _rep.Where<MeetInfo>(s => s.IsDeleted != 1)
                                //join c in _rep.Where<MeetSchedule>(s => s.IsDeleted != 1) on b.Id equals c.MeetId
                            where b.MeetStartTime != null && b.MeetEndTime != null && b.MeetEndTime >= startDate && b.MeetStartTime < endDate
                                && b.IsCompleted == EnumComplete.Approved
                            select new
                            {
                                MeetId = b.Id,
                                ScheduleStart = b.MeetStartTime,
                                ScheduleEnd = b.MeetEndTime
                            };

            //有会议签到的日期
            var signUpDays = (from a in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1 && s.IsSignIn == 1 && !string.IsNullOrEmpty(wxUserId) && s.SignUpUserId == wxUserId)
                              join b in schedules on a.MeetId equals b.MeetId
                              select new
                              {
                                  ScheduleStart = b.ScheduleStart.Value,
                                  ScheduleEnd = b.ScheduleEnd.Value
                              }).ToList();


            //有预约会议的日期
            var orderDays = (from a in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && !string.IsNullOrEmpty(wxUserId) && s.CreateUser == wxUserId)
                             join b in schedules on a.MeetId equals b.MeetId
                             select new
                             {
                                 ScheduleStart = b.ScheduleStart.Value,
                                 ScheduleEnd = b.ScheduleEnd.Value
                             }).ToList();

            //有已收藏会议的日期
            var collectionDays = (from a in _rep.Where<MyCollectionInfo>(s => s.CollectionType == 5 && !string.IsNullOrEmpty(unionId) && s.UnionId == unionId)
                                  join b in schedules on a.CollectionDataId equals b.MeetId
                                  select new
                                  {
                                      ScheduleStart = b.ScheduleStart.Value,
                                      ScheduleEnd = b.ScheduleEnd.Value
                                  }).ToList();


            int days = DateTime.DaysInMonth(year, month);//获取当月的天数
            DateTime day1 = new DateTime(year, month, 1);
            int week1 = (int)day1.DayOfWeek;//获取当年当月1号的星期
            Type dataType = typeof(object);

            DataTable dt = new DataTable();
            dt.Columns.Add("Sun", dataType);
            dt.Columns.Add("Mon", dataType);
            dt.Columns.Add("Tue", dataType);
            dt.Columns.Add("Wed", dataType);
            dt.Columns.Add("Thu", dataType);
            dt.Columns.Add("Fri", dataType);
            dt.Columns.Add("Sat", dataType);

            //循环这个月的每一天
            for (int i = 1; i <= days; i++)
            {
                int w = (int)Math.Ceiling((i + week1) / 7m) - 1; //这个月的第几个星期(从0开始)
                int d = (i + week1 - 1) % 7; //这天的星期(从0开始)
                if (dt.Rows.Count < w + 1)
                {
                    dt.Rows.Add(dt.NewRow());
                }

                DateTime date = new DateTime(year, month, i);
                int state = 0;  //0 无安排
                if (signUpDays.Any(s => s.ScheduleStart.Date <= date && s.ScheduleEnd.Date >= date))
                {
                    state = 3;  //3 已签到
                }
                else if (orderDays.Any(s => s.ScheduleStart.Date <= date && s.ScheduleEnd.Date >= date))
                {
                    state = 1;  //1 已预约
                }
                else if (collectionDays.Any(s => s.ScheduleStart.Date <= date && s.ScheduleEnd.Date >= date))
                {
                    state = 2;  //2 已收藏 
                }

                dt.Rows[w][d] = new
                {
                    day = i,
                    State = state
                };
            }

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                time = dateTime?.ToString("yyyy-MM-dd"),
                dt
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }
        /// <summary>
        /// 会议报名
        /// </summary>
        /// <param name="mmo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel MeetingOrder(MyMeetOrderViewModel mmo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meetInfo = _rep.FirstOrDefault<MeetInfo>(s => s.Id == mmo.MeetId
                                && s.IsCompleted == EnumComplete.Approved);

            if (meetInfo == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid MeetId.";
                return rvm;
            }
            var startTime = _rep.Where<MeetSchedule>(x => x.IsDeleted != 1 && x.MeetId == meetInfo.Id).OrderBy(x => x.ScheduleStart).FirstOrDefault();
            var myMeetOrder = _rep.FirstOrDefault<MyMeetOrder>(s => s.MeetId == mmo.MeetId && s.CreateUser == workUser.WxUser.Id);

            var remindMinutes = mmo.IsRemind == 1 ? (-(mmo.WarnMinutes ?? 30)) : 0;
            var remindTime = mmo.IsRemind == 1 ? startTime.ScheduleStart?.AddMinutes(remindMinutes) : null;

            if (myMeetOrder == null)
            {

                myMeetOrder = new MyMeetOrder
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetId = mmo.MeetId,
                    UnionId = workUser.WxUser.UnionId,
                    IsRemind = mmo.IsRemind,
                    RemindOffsetMinutes = remindMinutes,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.WxUser.Id,
                    RemindTime = remindTime,

                };
                _rep.Insert(myMeetOrder);
            }
            else
            {
                myMeetOrder.RemindTime = remindTime;
                myMeetOrder.MeetId = mmo.MeetId;
                myMeetOrder.UnionId = workUser.WxUser.UnionId;
                myMeetOrder.IsRemind = mmo.IsRemind;
                myMeetOrder.RemindOffsetMinutes = remindMinutes;
                myMeetOrder.UpdateTime = DateTime.Now;
                myMeetOrder.UpdateUser = workUser.WxUser.Id;
                _rep.Update(myMeetOrder);
            }
            _rep.SaveChanges();



            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                myMeetOrder
            };

            return rvm;
        }

        /// <summary>
        /// 会议签到
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel MeetingSignUp(MeetSignUpViewModel viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.MeetId))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'MeetId' is required.";
                return rvm;
            }

            if (!string.IsNullOrEmpty(viewModel.AppId) && viewModel.AppId != "0")
            {
                //判断是不是费卡文库的公众号ID
                if (viewModel.AppId == _config.GetFkLibAppId())
                {
                    //调用费卡文库的签到接口同步签到信息
                    IFKLibService fKLibSync = ContainerManager.Resolve<IFKLibService>();
                    return fKLibSync.SyncCheckIn(workUser?.WxUser, viewModel.MeetId);
                }
                else
                {
                    rvm.Success = false;
                    rvm.Msg = "Invalid AppId";
                    return rvm;
                }
            }

            MeetInfo meetInfo = _rep.FirstOrDefault<MeetInfo>(s => s.IsDeleted != 1 && s.Id == viewModel.MeetId);
            if (meetInfo == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meeting id";
                return rvm;
            }

            if (meetInfo.IsCompleted != EnumComplete.Approved)
            {
                rvm.Success = false;
                rvm.Msg = "not_approved";
                return rvm;
            }

            //会议签到时间不能早于会议前30分钟
            if (!meetInfo.MeetStartTime.HasValue || DateTime.Now < meetInfo.MeetStartTime.Value.AddMinutes(-30))
            {
                rvm.Success = false;
                rvm.Msg = "sign_up_not_started";
                return rvm;
            }

            if (meetInfo.MeetEndTime.HasValue && DateTime.Now > meetInfo.MeetEndTime)
            {
                rvm.Success = false;
                rvm.Msg = "meeting_ended";
                return rvm;
            }

            var meetSignUp = _rep.FirstOrDefault<MeetSignUp>(s => s.IsDeleted != 1 && s.MeetId == viewModel.MeetId && s.SignUpUserId == workUser.WxUser.Id);

            if (meetSignUp != null && meetSignUp.IsSignIn == 1)
            {
                rvm.Success = false;
                rvm.Msg = "already_signed_in";
                return rvm;
            }

            if (meetSignUp == null)
            {
                meetSignUp = new MeetSignUp
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetId = viewModel.MeetId,
                    SignUpUserId = workUser.WxUser.Id,
                    IsSignIn = 1,
                    SignInTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.WxUser.Id,
                };
                _rep.Insert(meetSignUp);
            }
            else
            {
                meetSignUp.IsSignIn = 1;
                meetSignUp.SignInTime = DateTime.Now;
                meetSignUp.UpdateTime = DateTime.Now;
                meetSignUp.UpdateUser = workUser.WxUser.Id;
                _rep.Update(meetSignUp);
            }
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                meetSignUp
            };

            return rvm;
        }

        /// <summary>
        /// 更新状态：已查看会议详情 (导出会议报告时需要)
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel KnownMeetingDetail(MeetSignUp model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meetInfo = _rep.FirstOrDefault<MeetInfo>(s => s.IsDeleted != 1 && s.Id == model.MeetId && s.IsCompleted == EnumComplete.Approved);
            if (meetInfo == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meeting id";
                return rvm;
            }

            var meetSignUp = _rep.FirstOrDefault<MeetSignUp>(s => s.IsDeleted != 1 && s.MeetId == model.MeetId && s.SignUpUserId == workUser.WxUser.Id);

            if (meetSignUp == null)
            {
                meetSignUp = new MeetSignUp
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetId = model.MeetId,
                    SignUpUserId = workUser.WxUser.Id,
                    IsKnewDetail = 1,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.WxUser.Id,
                };
                _rep.Insert(meetSignUp);
            }
            else
            {
                meetSignUp.IsKnewDetail = 1;
                meetSignUp.UpdateTime = DateTime.Now;
                meetSignUp.UpdateUser = workUser.WxUser.Id;
                _rep.Update(meetSignUp);
            }
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                meetSignUp
            };

            return rvm;
        }

        /// <summary>
        /// 未开始的会议
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel NotStartMeet(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);

            var list = (from a in _rep.Where<MeetInfo>(s => s.IsDeleted != 1 && s.IsCompleted == EnumComplete.Approved)
                        join b in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1) on a.Id equals b.MeetId
                        join c in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on b.UnionId equals c.UnionId
                        where c.UnionId == workUser.WxUser.UnionId && DateTime.Now < a.MeetStartTime
                        orderby a.MeetStartTime descending
                        select new
                        {
                            Id = a.Id,
                            MeetTitle = a.MeetTitle,
                            MeetSubject = a.MeetSubject,
                            MeetType = a.MeetType,
                            MeetDep = a.MeetDep,
                            MeetIntroduction = a.MeetIntroduction,
                            MeetStartTime = a.MeetStartTime,
                            MeetEndTime = a.MeetEndTime,
                            CreateTime = a.CreateTime,
                            MeetDate = a.MeetDate,
                            Speaker = a.Speaker,
                            SpeakerDetail = a.SpeakerDetail,
                            MeetAddress = a.MeetAddress,
                            ReplayAddress = a.ReplayAddress,
                            MeetData = a.MeetData,
                            MeetCodeUrl = a.MeetCodeUrl,
                            MeetCity = a.MeetCity,
                            MeetingNumber = a.MeetingNumber,
                            MeetSite = a.MeetSite,
                            MeetCoverSmall = (from b in pic
                                              where b.MeetId == a.Id && b.Id == a.MeetCoverSmall
                                              select b.MeetPicUrl).FirstOrDefault(),
                            MeetCoverBig = (from b in pic
                                            where b.MeetId == a.Id && b.Id == a.MeetCoverBig
                                            select b.MeetPicUrl).FirstOrDefault(),
                            IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                            ApprovalNote = a.ApprovalNote,
                            IsChoiceness = a.IsChoiceness,
                            IsHot = a.IsHot,
                        }).Distinct().ToList();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                list
            };
            return rvm;
        }

        /// <summary>
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMeetQAInfo(MeetQARelationViewModel meetQA, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(workUser?.WxUser?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "无法获取参会者Id。";
                return rvm;
            }

            var meeting = _rep.FirstOrDefault<MeetInfo>(a => a.IsDeleted != 1 && a.Id == meetQA.MeetId && a.IsCompleted == EnumComplete.Approved);
            if (meeting == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meeting ID";
                return rvm;
            }

            //var startTime = (from a in _rep.Table<MeetSchedule>()
            //                 where a.IsDeleted != 1 && a.MeetId == meetQA.MeetId && a.ScheduleStart.HasValue
            //                 orderby a.ScheduleStart ascending
            //                 select a.ScheduleStart).FirstOrDefault();

            //if (!startTime.HasValue)
            //{
            //    //无日程安排无法判断是会前还是会后，所以暂时不返回问卷 2019-03-05
            //    rvm.Success = false;
            //    rvm.Msg = "此会议无日程安排，暂无法返回问卷。";
            //    return rvm;
            //}

            var startTime = meeting.MeetStartTime;
            if (!startTime.HasValue)
            {
                //无会议开始时间无法判断是会前还是会后，所以暂时不返回问卷 2019-03-05
                rvm.Success = false;
                rvm.Msg = "此会议无开始时间，暂无法返回问卷。";
                return rvm;
            }

            var QAType = DateTime.UtcNow.AddHours(8) < startTime ? 1 : 2; //1.会前问卷 2.会后问卷 

            var QAResultList = _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meetQA.MeetId && s.SignUpUserId == workUser.WxUser.Id);
            var answers = _rep.Where<AnswerModel>(s => s.IsDeleted != 1);

            var list = from a in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meetQA.MeetId)
                       join b in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on a.QuestionId equals b.Id
                       select new WxMeetQAViewModel
                       {
                           MeetQAId = a.Id,
                           QuestionId = b.Id,
                           QuestionType = b.QuestionType,
                           QuestionContent = b.QuestionContent,
                           Answers = (from c in answers.Where(s => s.QuestionId == b.Id)
                                      join d in QAResultList.Where(s => s.MeetQAId == a.Id) on c.Id equals d.UserAnswerId into cd
                                      from d1 in cd.DefaultIfEmpty()
                                      where (b.QuestionType == 1 || b.QuestionType == 2)
                                      orderby c.Sort
                                      select new WxAnswerViewModel
                                      {
                                          AnswerId = c.Id,
                                          AnswerContent = c.AnswerContent,
                                          Selected = (d1 != null)
                                      })
                                      .Concat((from e in QAResultList.Where(s => s.MeetQAId == a.Id)
                                               where (b.QuestionType == 3)
                                               select new WxAnswerViewModel
                                               {
                                                   AnswerId = null,
                                                   AnswerContent = e.UserAnswer,
                                                   Selected = true
                                               }))
                       };

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                meetQA.MeetId,
                QAType,
                list
            };

            return rvm;
        }

        /// <summary>
        /// 提交会议问卷结果
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel CommitQAResult(WxMeetQAResultViewModel meetQA, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (meetQA == null)
            {
                rvm.Msg = "Invalid parameters.";
                rvm.Success = false;
                return rvm;
            }

            if (string.IsNullOrEmpty(meetQA.MeetId))
            {
                rvm.Msg = $"The parameter '{nameof(meetQA.MeetId)}' is required.";
                rvm.Success = false;
                return rvm;
            }

            if (meetQA.QAType == null)
            {
                rvm.Msg = $"The parameter '{nameof(meetQA.QAType)}' is required.";
                rvm.Success = false;
                return rvm;
            }

            if (meetQA.Questions == null)
            {
                rvm.Msg = $"The parameter '{nameof(meetQA.Questions)}' is required.";
                rvm.Success = false;
                return rvm;
            }

            var oldResults = from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.SignUpUserId == workUser.WxUser.Id)
                             join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1) on a.MeetQAId equals b.Id
                             where b.QAType == meetQA.QAType && b.MeetId == meetQA.MeetId
                             select a;

            var list = (from a in meetQA.Questions
                        from b in a.Answers.Select(s => new { s.AnswerId, s.AnswerContent }).Distinct()
                        join c in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1) on a.MeetQAId equals c.Id
                        join d in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on c.QuestionId equals d.Id
                        where c.QAType == meetQA.QAType && c.MeetId == meetQA.MeetId
                        select new MeetQAResult
                        {
                            Id = Guid.NewGuid().ToString(),
                            MeetId = c.MeetId,
                            MeetQAId = c.Id,
                            SignUpUserId = workUser.WxUser.Id,
                            UserAnswerId = b.AnswerId,
                            UserAnswer = b.AnswerContent,
                            CreateUser = workUser.WxUser.Id,
                            CreateTime = DateTime.Now
                        }).ToList();

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    foreach (var a in oldResults)
                    {
                        a.IsDeleted = 1;
                        a.UpdateUser = workUser.WxUser.Id;
                        a.UpdateTime = DateTime.Now;
                    }
                    _rep.UpdateList(oldResults);

                    _rep.InsertList(list);

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }

            return rvm;
        }

        /// <summary>
        /// 根据日期获取当天的会议
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetByDate(RowNumModel<MeetInfo> meet, WorkUser wxuser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //判断是否获取到时间
            if (meet.SearchParams.MeetStartTime == null)
            {
                rvm.Success = false;
                rvm.Msg = "Time is Not Found";
            }
            else
            {
                //判断是否获取到用户
                if (wxuser.WxUser.UnionId == null && wxuser.WxUser.UnionId == "" && wxuser.WxUser.UnionId == null && wxuser.WxUser.UnionId == "")
                {
                    rvm.Success = false;
                    rvm.Msg = "Users cannot be empty";
                }
                else
                {
                    var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1);
                    DateTime? StartTime;
                    DateTime? EndTime;
                    DateTime? StartEndTime1;
                    DateTime? StartEndTime2;
                    string sendtime = meet.SearchParams.MeetStartTime.ToString();
                    string time1 = " 00:00:00";
                    string time2 = " 23:59:59";
                    string SendTimeStart = sendtime.Split(' ')[0] + time1;
                    string SendTimeEnd = sendtime.Split(' ')[0] + time2;
                    StartTime = DateTime.Parse(SendTimeStart);
                    EndTime = DateTime.Parse(SendTimeEnd);
                    //判断用户当前点击的日期是否是当天
                    if (sendtime.Split(' ')[0] == (DateTime.Now.ToString()).Split(' ')[0])
                    {
                        StartEndTime1 = DateTime.Parse(DateTime.Now.ToString().Split(' ')[0] + " 00:00:00.0001");
                        StartEndTime2 = DateTime.Parse(DateTime.Now.ToString());
                    }
                    else
                    {
                        StartEndTime1 = DateTime.Parse(sendtime.Split(' ')[0] + time1);
                        StartEndTime2 = DateTime.Parse(sendtime.Split(' ')[0] + time2);
                    }

                    #region 获取已收藏会议
                    //获取开始是在选择的时间当天的已收藏会议
                    var SendCollections1 = from a in _rep.SqlQuery<MyCollectionInfo>("select * from MyCollectionInfo where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "'and CollectionType= 5 and IsDeleted =0")
                                           join b in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetStartTime<='" + EndTime + "' and MeetStartTime>='" + StartTime + "'") on a.CollectionDataId equals b.Id
                                           select new
                                           {
                                               Id = b.Id,
                                               MeetTitle = b.MeetTitle,
                                               MeetSubject = b.MeetSubject,
                                               MeetType = b.MeetType,
                                               MeetDep = b.MeetDep,
                                               MeetIntroduction = b.MeetIntroduction,
                                               MeetStartTime = b.MeetStartTime,
                                               MeetEndTime = b.MeetEndTime,
                                               CreateTime = b.CreateTime,
                                               MeetDate = b.MeetDate,
                                               Speaker = b.Speaker,
                                               SpeakerDetail = b.SpeakerDetail,
                                               MeetAddress = b.MeetAddress,
                                               ReplayAddress = b.ReplayAddress,
                                               MeetData = b.MeetData,
                                               MeetCodeUrl = b.MeetCodeUrl,
                                               MeetCity = b.MeetCity,
                                               MeetingNumber = b.MeetingNumber,
                                               MeetSite = b.MeetSite,
                                               MeetCoverSmall = (from c in pic
                                                                 where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                 select c.MeetPicUrl).FirstOrDefault(),

                                               MeetCoverBig = (from c in pic
                                                               where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                               select c.MeetPicUrl).FirstOrDefault(),
                                               IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                               ApprovalNote = b.ApprovalNote,
                                               IsChoiceness = b.IsChoiceness,
                                               IsHot = b.IsHot

                                           };
                    //获取结束时间是在选择的时间当天的已收藏会议
                    var SendCollections2 = from a in _rep.SqlQuery<MyCollectionInfo>("select * from MyCollectionInfo where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "'and CollectionType= 5 and IsDeleted =0")
                                           join b in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetEndTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'") on a.CollectionDataId equals b.Id
                                           select new
                                           {
                                               Id = b.Id,
                                               MeetTitle = b.MeetTitle,
                                               MeetSubject = b.MeetSubject,
                                               MeetType = b.MeetType,
                                               MeetDep = b.MeetDep,
                                               MeetIntroduction = b.MeetIntroduction,
                                               MeetStartTime = b.MeetStartTime,
                                               MeetEndTime = b.MeetEndTime,
                                               CreateTime = b.CreateTime,
                                               MeetDate = b.MeetDate,
                                               Speaker = b.Speaker,
                                               SpeakerDetail = b.SpeakerDetail,
                                               MeetAddress = b.MeetAddress,
                                               ReplayAddress = b.ReplayAddress,
                                               MeetData = b.MeetData,
                                               MeetCodeUrl = b.MeetCodeUrl,
                                               MeetCity = b.MeetCity,
                                               MeetingNumber = b.MeetingNumber,
                                               MeetSite = b.MeetSite,
                                               MeetCoverSmall = (from c in pic
                                                                 where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                 select c.MeetPicUrl).FirstOrDefault(),

                                               MeetCoverBig = (from c in pic
                                                               where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                               select c.MeetPicUrl).FirstOrDefault(),
                                               IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                               ApprovalNote = b.ApprovalNote,
                                               IsChoiceness = b.IsChoiceness,
                                               IsHot = b.IsHot

                                           };
                    //获取在当天进行的已收藏的会议
                    var SendCollections3 = from a in _rep.SqlQuery<MyCollectionInfo>("select * from MyCollectionInfo where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "'and CollectionType= 5 and IsDeleted =0")
                                           join b in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetStartTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'") on a.CollectionDataId equals b.Id
                                           select new
                                           {
                                               Id = b.Id,
                                               MeetTitle = b.MeetTitle,
                                               MeetSubject = b.MeetSubject,
                                               MeetType = b.MeetType,
                                               MeetDep = b.MeetDep,
                                               MeetIntroduction = b.MeetIntroduction,
                                               MeetStartTime = b.MeetStartTime,
                                               MeetEndTime = b.MeetEndTime,
                                               CreateTime = b.CreateTime,
                                               MeetDate = b.MeetDate,
                                               Speaker = b.Speaker,
                                               SpeakerDetail = b.SpeakerDetail,
                                               MeetAddress = b.MeetAddress,
                                               ReplayAddress = b.ReplayAddress,
                                               MeetData = b.MeetData,
                                               MeetCodeUrl = b.MeetCodeUrl,
                                               MeetCity = b.MeetCity,
                                               MeetingNumber = b.MeetingNumber,
                                               MeetSite = b.MeetSite,
                                               MeetCoverSmall = (from c in pic
                                                                 where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                 select c.MeetPicUrl).FirstOrDefault(),

                                               MeetCoverBig = (from c in pic
                                                               where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                               select c.MeetPicUrl).FirstOrDefault(),
                                               IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                               ApprovalNote = b.ApprovalNote,
                                               IsChoiceness = b.IsChoiceness,
                                               IsHot = b.IsHot

                                           };
                    //如果会议时间大于两天，选择最中间一天去获取已收藏会议
                    var SendCollections4 = from a in _rep.SqlQuery<MyCollectionInfo>("select * from MyCollectionInfo where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and  CollectionType= 5 and IsDeleted =0")
                                           join b in _rep.SqlQuery<MeetInfo>("select * from MeetInfo where IsDeleted=0 and MeetStartTime<='" + StartTime + "' and MeetEndTime>='" + EndTime + "'") on a.CollectionDataId equals b.Id
                                           select new
                                           {
                                               Id = b.Id,
                                               MeetTitle = b.MeetTitle,
                                               MeetSubject = b.MeetSubject,
                                               MeetType = b.MeetType,
                                               MeetDep = b.MeetDep,
                                               MeetIntroduction = b.MeetIntroduction,
                                               MeetStartTime = b.MeetStartTime,
                                               MeetEndTime = b.MeetEndTime,
                                               CreateTime = b.CreateTime,
                                               MeetDate = b.MeetDate,
                                               Speaker = b.Speaker,
                                               SpeakerDetail = b.SpeakerDetail,
                                               MeetAddress = b.MeetAddress,
                                               ReplayAddress = b.ReplayAddress,
                                               MeetData = b.MeetData,
                                               MeetCodeUrl = b.MeetCodeUrl,
                                               MeetCity = b.MeetCity,
                                               MeetingNumber = b.MeetingNumber,
                                               MeetSite = b.MeetSite,
                                               MeetCoverSmall = (from c in pic
                                                                 where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                 select c.MeetPicUrl).FirstOrDefault(),

                                               MeetCoverBig = (from c in pic
                                                               where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                               select c.MeetPicUrl).FirstOrDefault(),
                                               IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                               ApprovalNote = b.ApprovalNote,
                                               IsChoiceness = b.IsChoiceness,
                                               IsHot = b.IsHot

                                           };
                    #endregion
                    #region 获取已预约的会议
                    //获取开始是在选择的时间当天的已预约会议
                    var SendMakeanappointmentMeet1 = from a in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0")
                                                     join b in _rep.SqlQuery<MeetInfo>("select *from meetInfo where IsDeleted = 0 and MeetStartTime<= '" + EndTime + "' and MeetStartTime>= '" + StartTime + "'") on a.MeetId equals b.Id
                                                     select new
                                                     {
                                                         Id = b.Id,
                                                         MeetTitle = b.MeetTitle,
                                                         MeetSubject = b.MeetSubject,
                                                         MeetType = b.MeetType,
                                                         MeetDep = b.MeetDep,
                                                         MeetIntroduction = b.MeetIntroduction,
                                                         MeetStartTime = b.MeetStartTime,
                                                         MeetEndTime = b.MeetEndTime,
                                                         CreateTime = b.CreateTime,
                                                         MeetDate = b.MeetDate,
                                                         Speaker = b.Speaker,
                                                         SpeakerDetail = b.SpeakerDetail,
                                                         MeetAddress = b.MeetAddress,
                                                         ReplayAddress = b.ReplayAddress,
                                                         MeetData = b.MeetData,
                                                         MeetCodeUrl = b.MeetCodeUrl,
                                                         MeetCity = b.MeetCity,
                                                         MeetingNumber = b.MeetingNumber,
                                                         MeetSite = b.MeetSite,
                                                         MeetCoverSmall = (from c in pic
                                                                           where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                           select c.MeetPicUrl).FirstOrDefault(),

                                                         MeetCoverBig = (from c in pic
                                                                         where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                                         select c.MeetPicUrl).FirstOrDefault(),
                                                         IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                         ApprovalNote = b.ApprovalNote,
                                                         IsChoiceness = b.IsChoiceness,
                                                         IsHot = b.IsHot
                                                     };
                    //获取结束时间是在选择的时间当天的已预约会议
                    var SendMakeanappointmentMeet2 = from a in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0")
                                                     join b in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetEndTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'") on a.MeetId equals b.Id
                                                     select new
                                                     {
                                                         Id = b.Id,
                                                         MeetTitle = b.MeetTitle,
                                                         MeetSubject = b.MeetSubject,
                                                         MeetType = b.MeetType,
                                                         MeetDep = b.MeetDep,
                                                         MeetIntroduction = b.MeetIntroduction,
                                                         MeetStartTime = b.MeetStartTime,
                                                         MeetEndTime = b.MeetEndTime,
                                                         CreateTime = b.CreateTime,
                                                         MeetDate = b.MeetDate,
                                                         Speaker = b.Speaker,
                                                         SpeakerDetail = b.SpeakerDetail,
                                                         MeetAddress = b.MeetAddress,
                                                         ReplayAddress = b.ReplayAddress,
                                                         MeetData = b.MeetData,
                                                         MeetCodeUrl = b.MeetCodeUrl,
                                                         MeetCity = b.MeetCity,
                                                         MeetingNumber = b.MeetingNumber,
                                                         MeetSite = b.MeetSite,
                                                         MeetCoverSmall = (from c in pic
                                                                           where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                           select c.MeetPicUrl).FirstOrDefault(),

                                                         MeetCoverBig = (from c in pic
                                                                         where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                                         select c.MeetPicUrl).FirstOrDefault(),
                                                         IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                         ApprovalNote = b.ApprovalNote,
                                                         IsChoiceness = b.IsChoiceness,
                                                         IsHot = b.IsHot
                                                     };
                    //获取在当天进行的已收藏的已预约会议
                    var SendMakeanappointmentMeet3 = from a in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0")
                                                     join b in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetStartTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'") on a.MeetId equals b.Id
                                                     select new
                                                     {
                                                         Id = b.Id,
                                                         MeetTitle = b.MeetTitle,
                                                         MeetSubject = b.MeetSubject,
                                                         MeetType = b.MeetType,
                                                         MeetDep = b.MeetDep,
                                                         MeetIntroduction = b.MeetIntroduction,
                                                         MeetStartTime = b.MeetStartTime,
                                                         MeetEndTime = b.MeetEndTime,
                                                         CreateTime = b.CreateTime,
                                                         MeetDate = b.MeetDate,
                                                         Speaker = b.Speaker,
                                                         SpeakerDetail = b.SpeakerDetail,
                                                         MeetAddress = b.MeetAddress,
                                                         ReplayAddress = b.ReplayAddress,
                                                         MeetData = b.MeetData,
                                                         MeetCodeUrl = b.MeetCodeUrl,
                                                         MeetCity = b.MeetCity,
                                                         MeetingNumber = b.MeetingNumber,
                                                         MeetSite = b.MeetSite,
                                                         MeetCoverSmall = (from c in pic
                                                                           where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                           select c.MeetPicUrl).FirstOrDefault(),

                                                         MeetCoverBig = (from c in pic
                                                                         where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                                         select c.MeetPicUrl).FirstOrDefault(),
                                                         IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                         ApprovalNote = b.ApprovalNote,
                                                         IsChoiceness = b.IsChoiceness,
                                                         IsHot = b.IsHot
                                                     };
                    //如果会议时间大于两天，选择最中间一天去获取已预约会议
                    var SendMakeanappointmentMeet4 = from a in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0")
                                                     join b in _rep.SqlQuery<MeetInfo>("select * from MeetInfo where IsDeleted=0 and MeetStartTime<='" + StartTime + "' and MeetEndTime>='" + EndTime + "'") on a.MeetId equals b.Id
                                                     select new
                                                     {
                                                         Id = b.Id,
                                                         MeetTitle = b.MeetTitle,
                                                         MeetSubject = b.MeetSubject,
                                                         MeetType = b.MeetType,
                                                         MeetDep = b.MeetDep,
                                                         MeetIntroduction = b.MeetIntroduction,
                                                         MeetStartTime = b.MeetStartTime,
                                                         MeetEndTime = b.MeetEndTime,
                                                         CreateTime = b.CreateTime,
                                                         MeetDate = b.MeetDate,
                                                         Speaker = b.Speaker,
                                                         SpeakerDetail = b.SpeakerDetail,
                                                         MeetAddress = b.MeetAddress,
                                                         ReplayAddress = b.ReplayAddress,
                                                         MeetData = b.MeetData,
                                                         MeetCodeUrl = b.MeetCodeUrl,
                                                         MeetCity = b.MeetCity,
                                                         MeetingNumber = b.MeetingNumber,
                                                         MeetSite = b.MeetSite,
                                                         MeetCoverSmall = (from c in pic
                                                                           where c.MeetId == b.Id && c.Id == b.MeetCoverSmall
                                                                           select c.MeetPicUrl).FirstOrDefault(),

                                                         MeetCoverBig = (from c in pic
                                                                         where c.MeetId == b.Id && c.Id == b.MeetCoverBig
                                                                         select c.MeetPicUrl).FirstOrDefault(),
                                                         IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                         ApprovalNote = b.ApprovalNote,
                                                         IsChoiceness = b.IsChoiceness,
                                                         IsHot = b.IsHot
                                                     };

                    #endregion
                    #region 获取已参加会议
                    //获取开始是在选择的时间当天的已参加的线下会议
                    //and MeetEndTime>= '" + StartEndTime1+ "' and MeetEndTime<= '" + StartEndTime2 + "'
                    var SendParticipateinMeet1 = from a in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted = 0  and MeetType= 2 or MeetType= 3 and MeetStartTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'")
                                                 join b in _rep.SqlQuery<MeetSignUp>("select * from MeetSignUp where IsDeleted =0 and SignUpUserId='" + wxuser.WxUser.Id + "' ") on a.Id equals b.MeetId
                                                 select new
                                                 {
                                                     Id = a.Id,
                                                     MeetTitle = a.MeetTitle,
                                                     MeetSubject = a.MeetSubject,
                                                     MeetType = a.MeetType,
                                                     MeetDep = a.MeetDep,
                                                     MeetIntroduction = a.MeetIntroduction,
                                                     MeetStartTime = a.MeetStartTime,
                                                     MeetEndTime = a.MeetEndTime,
                                                     CreateTime = a.CreateTime,
                                                     MeetDate = a.MeetDate,
                                                     Speaker = a.Speaker,
                                                     SpeakerDetail = a.SpeakerDetail,
                                                     MeetAddress = a.MeetAddress,
                                                     ReplayAddress = a.ReplayAddress,
                                                     MeetData = a.MeetData,
                                                     MeetCodeUrl = a.MeetCodeUrl,
                                                     MeetCity = a.MeetCity,
                                                     MeetingNumber = a.MeetingNumber,
                                                     MeetSite = a.MeetSite,
                                                     MeetCoverSmall = (from e in pic
                                                                       where e.MeetId == a.Id && e.Id == a.MeetCoverSmall
                                                                       select e.MeetPicUrl).FirstOrDefault(),

                                                     MeetCoverBig = (from e in pic
                                                                     where e.MeetId == a.Id && e.Id == a.MeetCoverBig
                                                                     select e.MeetPicUrl).FirstOrDefault(),
                                                     IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                     ApprovalNote = a.ApprovalNote,
                                                     IsChoiceness = a.IsChoiceness,
                                                     IsHot = a.IsHot
                                                 };
                    //获取已经参加的线上会议
                    var SendParticipateinMeet2 = from a in _rep.SqlQuery<MeetInfo>("select *from meetInfo where IsDeleted = 0 and MeetType=1 and MeetStartTime>= '" + StartEndTime1 + "' and MeetStartTime<= '" + StartEndTime2 + "' and MeetStartTime<=GETDATE() ")
                                                 join c in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0") on a.Id equals c.MeetId
                                                 select new
                                                 {
                                                     Id = a.Id,
                                                     MeetTitle = a.MeetTitle,
                                                     MeetSubject = a.MeetSubject,
                                                     MeetType = a.MeetType,
                                                     MeetDep = a.MeetDep,
                                                     MeetIntroduction = a.MeetIntroduction,
                                                     MeetStartTime = a.MeetStartTime,
                                                     MeetEndTime = a.MeetEndTime,
                                                     CreateTime = a.CreateTime,
                                                     MeetDate = a.MeetDate,
                                                     Speaker = a.Speaker,
                                                     SpeakerDetail = a.SpeakerDetail,
                                                     MeetAddress = a.MeetAddress,
                                                     ReplayAddress = a.ReplayAddress,
                                                     MeetData = a.MeetData,
                                                     MeetCodeUrl = a.MeetCodeUrl,
                                                     MeetCity = a.MeetCity,
                                                     MeetingNumber = a.MeetingNumber,
                                                     MeetSite = a.MeetSite,
                                                     MeetCoverSmall = (from e in pic
                                                                       where e.MeetId == a.Id && e.Id == a.MeetCoverSmall
                                                                       select e.MeetPicUrl).FirstOrDefault(),

                                                     MeetCoverBig = (from e in pic
                                                                     where e.MeetId == a.Id && e.Id == a.MeetCoverBig
                                                                     select e.MeetPicUrl).FirstOrDefault(),
                                                     IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                                                     ApprovalNote = a.ApprovalNote,
                                                     IsChoiceness = a.IsChoiceness,
                                                     IsHot = a.IsHot
                                                 };
                    ////获取结束时间是在选择的时间当天的已参加会议
                    //var SendParticipateinMeet2 = from a in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetEndTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'")
                    //                            join b in _rep.SqlQuery<MeetSignUp>("select * from MeetSignUp where IsDeleted =0 and SignUpUserId='" + wxuser.WxUser.Id + "'") on a.Id equals b.MeetId
                    //                            join c in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0") on b.MeetId equals c.MeetId
                    //                            select new
                    //                            {
                    //                                Id = a.Id,
                    //                                MeetTitle = a.MeetTitle,
                    //                                MeetSubject = a.MeetSubject,
                    //                                MeetType = a.MeetType,
                    //                                MeetDep = a.MeetDep,
                    //                                MeetIntroduction = a.MeetIntroduction,
                    //                                MeetStartTime = a.MeetStartTime,
                    //                                MeetEndTime = a.MeetEndTime,
                    //                                CreateTime = a.CreateTime,
                    //                                MeetDate = a.MeetDate,
                    //                                Speaker = a.Speaker,
                    //                                SpeakerDetail = a.SpeakerDetail,
                    //                                MeetAddress = a.MeetAddress,
                    //                                ReplayAddress = a.ReplayAddress,
                    //                                MeetData = a.MeetData,
                    //                                MeetCodeUrl = a.MeetCodeUrl,
                    //                                MeetCity = a.MeetCity,
                    //                                MeetingNumber = a.MeetingNumber,
                    //                                MeetSite = a.MeetSite,
                    //                                MeetCoverSmall = (from d in pic
                    //                                                  where d.MeetId == a.Id && b.Id == a.MeetCoverSmall
                    //                                                  select d.MeetPicUrl).FirstOrDefault(),
                    //                                MeetCoverBig = (from d in pic
                    //                                                where d.MeetId == a.Id && b.Id == a.MeetCoverBig
                    //                                                select d.MeetPicUrl).FirstOrDefault(),
                    //                                IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                    //                                ApprovalNote = a.ApprovalNote,
                    //                                IsChoiceness = a.IsChoiceness,
                    //                                IsHot = a.IsHot
                    //                            };
                    ////获取在当天进行的已参加的会议
                    //var SendParticipateinMeet3 = from a in _rep.SqlQuery<MeetInfo>("select * from meetInfo where IsDeleted=0 and MeetStartTime>='" + StartTime + "' and MeetEndTime<='" + EndTime + "'")
                    //                            join b in _rep.SqlQuery<MeetSignUp>("select * from MeetSignUp where IsDeleted =0 and SignUpUserId='" + wxuser.WxUser.Id + "'") on a.Id equals b.MeetId
                    //                            join c in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0") on b.MeetId equals c.MeetId
                    //                            select new
                    //                            {
                    //                                Id = a.Id,
                    //                                MeetTitle = a.MeetTitle,
                    //                                MeetSubject = a.MeetSubject,
                    //                                MeetType = a.MeetType,
                    //                                MeetDep = a.MeetDep,
                    //                                MeetIntroduction = a.MeetIntroduction,
                    //                                MeetStartTime = a.MeetStartTime,
                    //                                MeetEndTime = a.MeetEndTime,
                    //                                CreateTime = a.CreateTime,
                    //                                MeetDate = a.MeetDate,
                    //                                Speaker = a.Speaker,
                    //                                SpeakerDetail = a.SpeakerDetail,
                    //                                MeetAddress = a.MeetAddress,
                    //                                ReplayAddress = a.ReplayAddress,
                    //                                MeetData = a.MeetData,
                    //                                MeetCodeUrl = a.MeetCodeUrl,
                    //                                MeetCity = a.MeetCity,
                    //                                MeetingNumber = a.MeetingNumber,
                    //                                MeetSite = a.MeetSite,
                    //                                MeetCoverSmall = (from d in pic
                    //                                                  where d.MeetId == a.Id && b.Id == a.MeetCoverSmall
                    //                                                  select d.MeetPicUrl).FirstOrDefault(),
                    //                                MeetCoverBig = (from d in pic
                    //                                                where d.MeetId == a.Id && b.Id == a.MeetCoverBig
                    //                                                select d.MeetPicUrl).FirstOrDefault(),
                    //                                IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                    //                                ApprovalNote = a.ApprovalNote,
                    //                                IsChoiceness = a.IsChoiceness,
                    //                                IsHot = a.IsHot
                    //                            };
                    ////如果会议时间大于两天，选择最中间一天去获取已参加会议
                    //var SendParticipateinMeet4 = from a in _rep.SqlQuery<MeetInfo>("select * from MeetInfo where IsDeleted=0 and MeetStartTime<='" + StartTime + "' and MeetEndTime>='" + EndTime + "'")
                    //                             join b in _rep.SqlQuery<MeetSignUp>("select * from MeetSignUp where IsDeleted =0 and SignUpUserId='" + wxuser.WxUser.Id + "'") on a.Id equals b.MeetId
                    //                             join c in _rep.SqlQuery<MyMeetOrder>("select * from MyMeetOrder where UnionId='" + wxuser.WxUser.UnionId + "' or WxUserId='" + wxuser.WxUser.Id + "' and IsDeleted =0") on b.MeetId equals c.MeetId
                    //                             select new
                    //                             {
                    //                                 Id = a.Id,
                    //                                 MeetTitle = a.MeetTitle,
                    //                                 MeetSubject = a.MeetSubject,
                    //                                 MeetType = a.MeetType,
                    //                                 MeetDep = a.MeetDep,
                    //                                 MeetIntroduction = a.MeetIntroduction,
                    //                                 MeetStartTime = a.MeetStartTime,
                    //                                 MeetEndTime = a.MeetEndTime,
                    //                                 CreateTime = a.CreateTime,
                    //                                 MeetDate = a.MeetDate,
                    //                                 Speaker = a.Speaker,
                    //                                 SpeakerDetail = a.SpeakerDetail,
                    //                                 MeetAddress = a.MeetAddress,
                    //                                 ReplayAddress = a.ReplayAddress,
                    //                                 MeetData = a.MeetData,
                    //                                 MeetCodeUrl = a.MeetCodeUrl,
                    //                                 MeetCity = a.MeetCity,
                    //                                 MeetingNumber = a.MeetingNumber,
                    //                                 MeetSite = a.MeetSite,
                    //                                 MeetCoverSmall = (from d in pic
                    //                                                   where d.MeetId == a.Id && b.Id == a.MeetCoverSmall
                    //                                                   select d.MeetPicUrl).FirstOrDefault(),
                    //                                 MeetCoverBig = (from d in pic
                    //                                                 where d.MeetId == a.Id && b.Id == a.MeetCoverBig
                    //                                                 select d.MeetPicUrl).FirstOrDefault(),
                    //                                 IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                    //                                 ApprovalNote = a.ApprovalNote,
                    //                                 IsChoiceness = a.IsChoiceness,
                    //                                 IsHot = a.IsHot
                    //                             };
                    #endregion
                    //将四个集合进行合并去重
                    var CollectionMeet = (((SendCollections1.Union(SendCollections2)).Union(SendCollections3)).Union(SendCollections4));
                    var MakeanappointmentMeet = (((SendMakeanappointmentMeet1.Union(SendMakeanappointmentMeet2)).Union(SendMakeanappointmentMeet3)).Union(SendMakeanappointmentMeet4));
                    var ParticipateinMeet = SendParticipateinMeet1.Union(SendParticipateinMeet2);
                    rvm.Success = true;
                    rvm.Msg = "";
                    var total = CollectionMeet.Count() + MakeanappointmentMeet.Count() + ParticipateinMeet.Count();
                    CollectionMeet = CollectionMeet.ToPaginationList(meet.PageIndex, meet.PageSize).ToList();
                    MakeanappointmentMeet = MakeanappointmentMeet.ToPaginationList(meet.PageIndex, meet.PageSize).ToList();
                    ParticipateinMeet = ParticipateinMeet.ToPaginationList(meet.PageIndex, meet.PageSize).ToList();
                    rvm.Result = new
                    {
                        total,
                        rows = new
                        {
                            CollectionMeet = CollectionMeet,
                            MakeanappointmentMeet = MakeanappointmentMeet,
                            ParticipateinMeet = ParticipateinMeet
                        }
                    };
                }
            }



            //from a in 
            //join b in 
            //join c in _rep.SqlQuery<MeetInfo>("select * from MeetInfo where MeetStartTime<='" + StartTime + "' and MeetEndTime>='" + StartTime + "'")
            //if (meet.SearchParams.MeetStartTime == null || date == "") {
            //    rvm.Success = false;
            //    rvm.Msg = "Date Is Null!";
            //    rvm.Result = null;
            //}
            //else
            //{
            //    DateTime startDate = Convert.ToDateTime(date + " 00:00:00");
            //    DateTime endDate = Convert.ToDateTime(date + " 23:59:59");

            //    var sendMeeting = _rep.Where<MeetInfo>(p => p.IsDeleted != 1 && p.IsCompleted == EnumComplete.Approved && p.MeetStartTime >= startDate && p.MeetEndTime <= endDate);
            //    if (sendMeeting != null)
            //    {
            //        rvm.Success = true;
            //        rvm.Msg = "success";
            //        rvm.Result = sendMeeting;
            //    }
            //    else {
            //        rvm.Success = false;
            //        rvm.Msg = "No data were available for the day!";
            //        rvm.Result = sendMeeting;
            //    }
            //}

            return rvm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetInfo(MeetInfo meetInfo, WorkUser workUser)
        {

            ReturnValueModel rvm = new ReturnValueModel();
            //会议信息
            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfo.Id);
            if (meet == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meeting id";
                return rvm;
            }
            if (meetInfo == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid meetInfo";
                return rvm;
            }
            if (workUser == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid workUser";
                return rvm;
            }
            meet.IsForbiddenWords = meet.IsForbiddenWords ?? 0;
            //会议资料
            var meetFile = _rep.Where<MeetFile>(s => s.MeetId == meetInfo.Id).ToList();
            List<MeetFile> meetFilelist = _rep.Where<MeetFile>(s => s.MeetId == meetInfo.Id).ToList();
            foreach (var model in meetFilelist)
            {
                //下载路径加密
                model.Remark = "api/WxDownLoad/DownloadFile?url=" + EncryptHelper.AES_Encrypt(model.FilePath);
            }

            //会议日程 及 讲者
            var meetSchedule = _rep.Where<MeetSchedule>(x => x.IsDeleted != 1 && x.MeetId == meetInfo.Id).OrderBy(x => x.ScheduleStart).ToList().GroupBy(x => x.Remark)
                        .Select(x => new
                        {
                            DateStr = x.Key,
                            ScheduleView = x.OrderBy(y => y.Sort).GroupBy(y => y.Sort)
                                .Select(y => new
                                {
                                    AMPM = y.Key,
                                    ScheduleView = y.OrderBy(_ => _.Sort).GroupBy(_ => _.Topic)
                                        .Select(_ => new
                                        {
                                            Topic = _.Key,
                                            Speaker = _.FirstOrDefault().Speaker,
                                            Sort = _.FirstOrDefault().Sort,
                                            Schedule = _.Where(z => true).Join(_rep.Where<MeetSpeaker>(z => true), z1 => z1.MeetSpeakerId, z2 => z2.Id, (z1, z2) => new { MeetSpeaker = z2.SpeakerName, ScheduleContent = z1.ScheduleContent, ScheduleStart = z1.ScheduleStart }).OrderBy(z3 => z3.ScheduleStart).ToList()
                                        })
                                })
                        });

            var meetScheduleOld = (from a in _rep.Table<MeetSchedule>()
                                   join b in _rep.Table<MeetSpeaker>() on a.MeetSpeakerId equals b.Id
                                   orderby a.ScheduleStart descending
                                   where a.MeetId == meetInfo.Id && b.MeetId == meetInfo.Id
                                                                 && a.IsDeleted != 1 && b.IsDeleted != 1
                                   group new { a, b } by new { a.Remark } into g1
                                   select new MeetScheduleViewModel
                                   {
                                       ScheduleDate = g1.Key.Remark,
                                       ScheduleViews = from v1 in g1
                                                       orderby v1.a.ScheduleStart
                                                       select new ScheduleView
                                                       {
                                                           Schedule = new MeetScheduleView
                                                           {
                                                               Id = v1.a.Id,
                                                               MeetId = v1.a.MeetId,
                                                               ScheduleContent = v1.a.ScheduleContent,
                                                               MeetSpeakerId = v1.a.MeetSpeakerId,
                                                               ScheduleStart = v1.a.ScheduleStart.HasValue ? (SqlFunctions.DatePart("HOUR", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleStart) : SqlFunctions.DateName("HOUR", v1.a.ScheduleStart)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart)) : "00:00:00",
                                                               ScheduleEnd = v1.a.ScheduleEnd.HasValue ? (SqlFunctions.DatePart("HOUR", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd) : SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd)) : "00:00:00"
                                                           },
                                                           Speaker = v1.b
                                                       }
                                   }).ToList();

            var meetSpeaker = _rep.Where<MeetSpeaker>(s => s.IsDeleted != 1 && s.MeetId == meetInfo.Id).ToList();

            var meetOrder =
                _rep.FirstOrDefault<MyMeetOrder>(s => s.IsDeleted != 1 &&
                    s.MeetId == meetInfo.Id && s.UnionId == workUser.WxUser.UnionId);

            var meetSignUp =
                _rep.FirstOrDefault<MeetSignUp>(s => s.IsDeleted != 1 &&
                    s.MeetId == meetInfo.Id && s.SignUpUserId == workUser.WxUser.Id);

            var isCollection = _rep.Where<MyCollectionInfo>(s => s.IsDeleted != 1 &&
                    s.UnionId == workUser.WxUser.UnionId && s.CollectionDataId == meetInfo.Id && s.CollectionType == 5)
                .Any()
                ? 1
                : 2;

            var pic = _rep.Where<MeetPic>(s => s.IsDeleted != 1 && s.MeetId == meet.Id);
            meet.MeetCoverSmall = (from b in pic
                                   where b.Id == meet.MeetCoverSmall
                                   select b.MeetPicUrl).FirstOrDefault();
            meet.MeetCoverBig = (from b in pic
                                 where b.Id == meet.MeetCoverBig
                                 select b.MeetPicUrl).FirstOrDefault();

            //是否有会前问卷
            bool hasQA1 = _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 1).Any();

            //是否有会后问卷
            bool hasQA2 = _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 2).Any();

            //是否做过会前问卷
            bool didQA1 = hasQA1 &&
                (from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.SignUpUserId == workUser.WxUser.Id)
                 join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 1) on a.MeetQAId equals b.Id
                 select a).Any();

            //是否做过会后问卷
            bool didQA2 = hasQA2 &&
                (from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.SignUpUserId == workUser.WxUser.Id)
                 join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meet.Id && s.QAType == 2) on a.MeetQAId equals b.Id
                 select a).Any();

            //是否需要弹出问卷
            int showQAType = 0;
            if (meetOrder != null || (meetSignUp != null && meetSignUp.IsSignIn == 1))  //已报名或者已签到的才弹出问卷
            {
                //根据当前时间判断是否需要弹出问卷
                if (hasQA1 && !didQA1 && meet.MeetStartTime.HasValue && meet.MeetStartTime > DateTime.Now && meet.MeetStartTime.Value.AddMinutes(-30) <= DateTime.Now)
                {
                    showQAType = 1;
                }
                else if (hasQA2 && !didQA2 && meet.MeetEndTime.HasValue && meet.MeetEndTime < DateTime.Now && meet.MeetEndTime.Value.AddMinutes(30) >= DateTime.Now)
                {
                    showQAType = 2;
                }
            }

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {

                wxUser = workUser?.WxUser,
                meet,
                meetFile = meetFilelist,
                meetSchedule,
                meetScheduleOld,
                meetSpeaker,
                isSignUp = meetOrder != null ? 1 : 2,
                meetOrder = meetOrder,
                isCollection = isCollection,
                tel = workUser.WxUser?.Mobile?.Substring(7),
                shouldShowQA = showQAType, //是否需要弹出问卷
                hasQA1,  //是否有会前问卷
                hasQA2,  //是否有会后问卷
                didQA1,  //是否做过会前问卷
                didQA2   //是否做过会后问卷
            };

            return rvm;
        }

    }
}
