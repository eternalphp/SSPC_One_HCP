using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using EntityFramework.Extensions;
using SSPC_One_HCP.Services.RongCloud;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 会议管理
    /// </summary>
    public class MeetService : IMeetService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ISystemService _systemService;
        private readonly string IsSendMail = ConfigurationManager.AppSettings["IsSendMail"];
        private readonly string _FkLibAppId = ConfigurationManager.AppSettings["FkLibAppId"];

        public MeetService(IEfRepository rep, ICommonService commonService, ISystemService systemService)
        {
            _rep = rep;
            _commonService = commonService;
            _systemService = systemService;
        }

        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetList(RowNumModel<MeetSearchViewModel> rowNum, WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();

            var isAdmin = _commonService.IsAdmin(workUser);

            //var listTag = _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "M2");
            //var listProductInfo = _rep.Where<ProductInfo>(s => s.IsDeleted != 1);
            //var listDepartmentInfo = _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1);
            var listBuInfo = _rep.Where<BuInfo>(s => s.IsDeleted != 1);

            var speakers = _rep.Where<MeetSpeaker>(s => s.IsDeleted != 1);
            var c = _rep.Where<MeetAndProAndDepRelation>(s => s.IsDeleted != 1);

            var dt = DateTime.UtcNow.AddHours(8);
            //var g = _rep.Where<MeetTag>(s => s.IsDeleted != 1);
            var list = from A in _rep.Table<MeetInfo>()
                       where (A.IsCompleted != EnumComplete.Locked
                            && A.IsCompleted != EnumComplete.Obsolete
                            && A.IsCompleted != EnumComplete.CanceledUpdate)
                       select new MeetInfoViewModel
                       {
                           Meet = A,
                           Speakers = speakers.Where(s => s.MeetId == A.Id),
                           IsMeetEnd = dt > A.MeetEndTime ? 2 : dt < A.MeetStartTime ? 0 : 1,
                           ProductAndDeps = new ProductBuDeptSelectionViewModel
                           {
                               BuNameList = from v1 in c
                                            join D in listBuInfo on v1.BuName equals D.BuName
                                            where v1.MeetId == A.Id
                                            group v1 by D.BuName into g2
                                            select g2.Key,
                               //Products = from v1 in c
                               //           join E in listProductInfo on v1.ProductId equals E.Id
                               //           where v1.MeetId == A.Id
                               //           group v1 by E into g2
                               //           select new ProductSelectionViewModel
                               //           {
                               //               ProId = g2.Key.Id,
                               //               ProName = g2.Key.ProductName
                               //           },
                               //Departments = from v1 in c
                               //              join F in listDepartmentInfo on v1.DepartmentId equals F.Id
                               //              where v1.MeetId == A.Id
                               //              group v1 by F into g2
                               //              select new DepartmentSelectionViewModel
                               //              {
                               //                  DeptId = g2.Key.Id,
                               //                  DeptName = g2.Key.DepartmentName
                               //              }
                           },
                           //Tags = from v2 in g
                           //       join H in listTag on v2.TagId equals H.Id
                           //       where v2.MeetId == A.Id
                           //       select new TagView
                           //       {
                           //           Id = H.Id,
                           //           TagName = H.TagName,
                           //           TextKey = H.TextKey
                           //       }
                       };

            if (!isAdmin)
            {
                list = list.Where(s => s.Meet.CreateUser == workUser.User.Id);
            }

            if (rowNum?.SearchParams != null)
            {
                if (rowNum.SearchParams.Meet != null)
                {
                    //会议标题
                    if (!string.IsNullOrEmpty(rowNum.SearchParams.Meet.MeetTitle))
                    {
                        list = list.Where(s => s.Meet.MeetTitle.Contains(rowNum.SearchParams.Meet.MeetTitle));
                    }
                    //会议类型
                    if (rowNum.SearchParams.Meet.MeetType != null && rowNum.SearchParams.Meet.MeetType != 0)
                    {
                        list = list.Where(s => s.Meet.MeetType == rowNum.SearchParams.Meet.MeetType);
                    }
                    ////审批状态
                    //if (rowNum.SearchParams.Meet.IsCompleted != null)
                    //{
                    //    list = list.Where(s => s.Meet.IsCompleted == rowNum.SearchParams.Meet.IsCompleted);
                    //}
                }

                //会议开始时间结束时间
                if (rowNum.SearchParams.Meet_Start.HasValue && rowNum.SearchParams.Meet_End.HasValue)
                {
                    DateTime startTime = rowNum.SearchParams.Meet_Start.Value;
                    DateTime endTime = rowNum.SearchParams.Meet_End.Value.AddDays(1);

                    list = list.Where(s => s.Meet.MeetStartTime < endTime && s.Meet.MeetEndTime >= startTime);
                }
                //部门(BU)
                if (!string.IsNullOrEmpty(rowNum.SearchParams.BuName))
                {
                    list = list.Where(s => s.ProductAndDeps.BuNameList.Contains(rowNum.SearchParams.BuName));
                }
                //产品名称
                //if (!string.IsNullOrEmpty(rowNum.SearchParams.ProductName))
                //{
                //    list = list.Where(s => s.ProductAndDeps.Any(p => p.ProductInfo.ProductName.Contains(rowNum.SearchParams.ProductName)));
                //}
                //科室名称
                //if (!string.IsNullOrEmpty(rowNum.SearchParams.DepartmentName))
                //{
                //    list = list.Where(s => s.ProductAndDeps.Any(p => p.Departments.Any(d => d.DepartmentName.Contains(rowNum.SearchParams.DepartmentName))));
                //}

                //是否精选
                if (rowNum.SearchParams.IsChoiceness != null)
                {
                    list = list.Where(s => s.Meet != null && s.Meet.IsChoiceness == rowNum.SearchParams.IsChoiceness);
                }

                //是否精选
                if (rowNum.SearchParams.IsHot != null)
                {
                    list = list.Where(s => s.Meet != null && s.Meet.IsHot == rowNum.SearchParams.IsHot);
                }

                //是否是获取待审核列表
                if (rowNum.SearchParams.IsGettingApprovalList)
                {
                    list = list.Where(s => s.Meet.IsDeleted != 1 && (s.Meet.IsCompleted == EnumComplete.AddedUnapproved || s.Meet.IsCompleted == EnumComplete.UpdatedUnapproved || s.Meet.IsCompleted == EnumComplete.WillDelete));
                }
            }
            //去除 费卡文库 创建人
            list = list.Where(x => !x.Meet.Source.Equals(_FkLibAppId));

            IEnumerable<MeetInfoViewModel> list1 = list.ToList();

            foreach (var item in list1)
            {
                if (item?.Meet != null)
                {
                    //如果会议已经开始，管理员没审核，状态变成审核拒绝，提示文字"管理员没审核"
                    if (item.Meet.MeetStartTime.HasValue
                        && item.Meet.MeetStartTime <= DateTime.Now)
                    {
                        if (item.Meet.IsCompleted == EnumComplete.AddedUnapproved || item.Meet.IsCompleted == EnumComplete.UpdatedUnapproved)
                        {
                            item.Meet.IsCompleted = EnumComplete.Reject;
                            item.Meet.ApprovalNote = "管理员没审核";
                        }
                        if (item.Meet.IsCompleted == EnumComplete.Reject)
                        {
                            item.IsMeetEnd = 4; //已失效
                        }
                    }
                    if (item.Meet.IsCompleted == EnumComplete.WillDelete)
                    {
                        item.IsMeetEnd = 3; //删除中
                    }
                    if (item.Meet.IsDeleted == 1)
                    {
                        item.IsMeetEnd = 5; //禁用
                    }
                }
            }

            if (rowNum?.SearchParams != null)
            {
                if (rowNum.SearchParams.Meet != null)
                {
                    //审批状态
                    if (rowNum.SearchParams.Meet.IsCompleted != null)
                    {
                        list1 = list1.Where(s => s.Meet.IsCompleted == rowNum.SearchParams.Meet.IsCompleted);
                    }
                }

                if (rowNum.SearchParams.IsCompletedList != null && rowNum.SearchParams.IsCompletedList.Count > 0)
                {
                    //审批状态
                    list1 = list1.Where(s => rowNum.SearchParams.IsCompletedList.Contains(s.Meet.IsCompleted));
                }

                //会议状态
                if (rowNum.SearchParams.IsMeetEnd != null)
                {
                    list1 = list1.Where(s => s.IsMeetEnd == rowNum.SearchParams.IsMeetEnd);
                }
                //是否是获取待审核列表
                if (rowNum.SearchParams.IsGettingApprovalList)
                {
                    list1 = list1.Where(s => s.Meet.IsCompleted == EnumComplete.AddedUnapproved || s.Meet.IsCompleted == EnumComplete.UpdatedUnapproved || s.Meet.IsCompleted == EnumComplete.WillDelete);
                }
                //时间区域 
                if (rowNum.SearchParams.Meet_End != null && rowNum.SearchParams.Meet_Start != null)
                {
                    var meetStart = rowNum.SearchParams.Meet_Start.Value.AddSeconds(1);
                    var meetEnd = rowNum.SearchParams.Meet_Start.Value.AddDays(1).AddSeconds(-1);
                    list1 = list1.Where(x => x.Meet.MeetStartTime >= meetStart && x.Meet.MeetEndTime <= meetEnd);
                }
            }
            var total = list1.Count();
            var rows = list1.OrderByDescending(s => s.Meet.CreateTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = (from a in rows
                        join b in _rep.Table<UserModel>() on a.Meet.CreateUser equals b.Id
                                into ab
                        from bb in ab.DefaultIfEmpty()
                        select new
                        {
                            Meet = new
                            {
                                Id = a.Meet.Id,
                                MeetTitle = a.Meet.MeetTitle,
                                MeetSubject = a.Meet.MeetSubject,
                                MeetType = a.Meet.MeetType,
                                MeetDep = a.Meet.MeetDep,
                                MeetIntroduction = a.Meet.MeetIntroduction,
                                MeetStartTime = a.Meet.MeetStartTime.HasValue ? a.Meet.MeetStartTime.Value.ToString("yyyy-MM-dd") : "",
                                MeetEndTime = a.Meet.MeetEndTime.HasValue ? a.Meet.MeetEndTime.Value.ToString("yyyy-MM-dd") : "",
                                MeetDate = a.Meet.MeetDate.HasValue ? a.Meet.MeetDate.Value.ToString("yyyy-MM-dd") : "",
                                Speaker = a.Meet.Speaker,
                                SpeakerDetail = a.Meet.SpeakerDetail,
                                MeetAddress = a.Meet.MeetAddress,
                                ReplayAddress = a.Meet.ReplayAddress,
                                MeetData = a.Meet.MeetData,
                                MeetCodeUrl = a.Meet.MeetCodeUrl,
                                MeetCity = a.Meet.MeetCity,
                                MeetingNumber = a.Meet.MeetingNumber,
                                MeetSite = a.Meet.MeetSite,
                                MeetCoverSmall = a.Meet.MeetCoverSmall,
                                MeetCoverBig = a.Meet.MeetCoverBig,
                                IsCompleted = a.Meet.IsCompleted ?? EnumComplete.AddedUnapproved,
                                ApprovalNote = a.Meet.ApprovalNote,
                                IsChoiceness = a.Meet.IsChoiceness,
                                IsHot = a.Meet.IsHot,
                                CreateTime = a.Meet.CreateTime,
                                CreateUser = (a.Meet.Source == _FkLibAppId) ? "费卡文库" : bb?.ChineseName,
                                CreateUserADAccount = bb?.ADAccount,
                                a.Meet.PVCount,
                                a.Meet.IsChat
                            },
                            Speakers = a.Speakers,
                            IsMeetEnd = a.IsMeetEnd,
                            Tags = a.Tags,
                            ProductAndDeps = a.ProductAndDeps
                        }).OrderByDescending(o => o.Meet.CreateTime).ToList()
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        /// <summary>
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetListOfQA(RowNumModel<MeetSearchViewModel> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var isAdmin = _commonService.IsAdmin(workUser);

            var listQA = from A in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1)
                         group A by new { A.MeetId, A.QAType } into g1
                         select g1.Key;

            var list = from A in _rep.Where<MeetInfo>(s => s.IsCompleted == EnumComplete.Approved)
                       join B in listQA on A.Id equals B.MeetId
                       group B by A into g1
                       select new
                       {
                           Meet = g1.Key,
                           IsMeetEnd = DateTime.Now > g1.Key.MeetEndTime ? 2 : DateTime.Now < g1.Key.MeetStartTime ? 0 : 1,
                           HasQABefore = g1.Any(s => s.QAType == 1), //是否有会前问卷
                           HasQAAfter = g1.Any(s => s.QAType == 2)   //是否有会后问卷
                       };

            if (!isAdmin)
            {
                list = list.Where(s => s.Meet.CreateUser == workUser.User.Id);
            }

            if (rowNum?.SearchParams != null)
            {
                if (rowNum.SearchParams.Meet != null)
                {
                    //会议标题
                    if (!string.IsNullOrEmpty(rowNum.SearchParams.Meet.MeetTitle))
                    {
                        list = list.Where(s => s.Meet.MeetTitle.Contains(rowNum.SearchParams.Meet.MeetTitle));
                    }
                    //会议类型
                    if (rowNum.SearchParams.Meet.MeetType != null && rowNum.SearchParams.Meet.MeetType != 0)
                    {
                        list = list.Where(s => s.Meet.MeetType == rowNum.SearchParams.Meet.MeetType);
                    }
                }

                //会议开始时间结束时间
                if (rowNum.SearchParams.Meet_Start.HasValue && rowNum.SearchParams.Meet_End.HasValue)
                {
                    DateTime startTime = rowNum.SearchParams.Meet_Start.Value;
                    DateTime endTime = rowNum.SearchParams.Meet_End.Value.AddDays(1);

                    list = list.Where(s => s.Meet.MeetStartTime < endTime && s.Meet.MeetEndTime >= startTime);
                }
            }

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.Meet.CreateTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };
            return rvm;
        }

        /// <summary>
        /// 新增或更新会议信息（内部调用）
        /// </summary>
        /// <param name="meet">原始的会议信息或null</param>
        /// <param name="meetInfoView">修改后的会议信息</param>
        /// <param name="workUser">当前操作者</param>
        private void AddOrUpdateMeetInfo(MeetInfo meet, MeetInfoViewModel meetInfoView, WorkUser workUser)
        {
            if (meet != null)
            {
                //更新主表数据
                meet.MeetAddress = meetInfoView.Meet.MeetAddress;
                //meet.MeetCodeUrl = meetInfoView.Meet.MeetCodeUrl;
                meet.MeetData = meetInfoView.Meet.MeetData;
                meet.MeetDate = meetInfoView.Meet.MeetDate;
                meet.MeetDep = meetInfoView.Meet.MeetDep;
                meet.MeetEndTime = meetInfoView.Meet.MeetEndTime;
                meet.MeetIntroduction = meetInfoView.Meet.MeetIntroduction;
                meet.MeetStartTime = meetInfoView.Meet.MeetStartTime;
                meet.MeetSubject = meetInfoView.Meet.MeetSubject;
                meet.MeetTitle = meetInfoView.Meet.MeetTitle;
                meet.MeetType = meetInfoView.Meet.MeetType;
                meet.Remark = meetInfoView.Meet.Remark;
                meet.ReplayAddress = meetInfoView.Meet.ReplayAddress;
                //meet.Speaker = meetInfoView.Meet.Speaker;
                meet.SpeakerDetail = meetInfoView.Meet.SpeakerDetail;  //讲者简历放到一起 modified on 2019/4/2
                meet.MeetCity = meetInfoView.Meet.MeetCity;
                meet.MeetingNumber = meetInfoView.Meet.MeetingNumber;
                meet.MeetAddress = meetInfoView.Meet.MeetAddress;
                meet.MeetSite = meetInfoView.Meet.MeetSite;
                meet.IsChoiceness = meetInfoView.Meet.IsChoiceness;
                meet.IsHot = meetInfoView.Meet.IsHot;
                meet.InvitationDetail = meetInfoView.Meet.InvitationDetail;
                meet.WithinExternalType = meetInfoView.Meet.WithinExternalType;
                meet.WatchType = meetInfoView.Meet.WatchType;
                meet.IsChat = meetInfoView.Meet.IsChat;
                meet.LivePicture = meetInfoView.Meet.LivePicture;
                _rep.Update(meet);
                _rep.SaveChanges();


                var meetdata = _rep.Where<MeetFile>(s => s.MeetId == meet.Id);
                var meetspeaker = _rep.Where<MeetSpeaker>(s => s.MeetId == meet.Id);
                var meetschedule = _rep.Where<MeetSchedule>(s => s.MeetId == meet.Id);
                var meetRelation = _rep.Where<MeetAndProAndDepRelation>(s => s.MeetId == meet.Id);
                var tags = _rep.Where<MeetTag>(s => s.MeetId == meet.Id);
                var coverPictures = _rep.Where<MeetPic>(s => s.MeetId == meet.Id);

                var doctorMeeting = _rep.Where<DoctorMeeting>(x => x.MeetingID == meet.Id);

                //演讲者时间变更重新短信提醒
                var newMeet = _rep.FirstOrDefault<MeetInfo>(s => s.OldId == meet.Id);
                if (newMeet != null)
                {
                    var newSchedule = _rep.Where<MeetSchedule>(s => s.MeetId == newMeet.Id).Select(x => new MeetScheduleView() { ScheduleStart = x.ScheduleStart.ToString(), ScheduleEnd = x.ScheduleEnd.ToString() }).OrderByDescending(o => o.ScheduleStart).ToList();
                    var oldSchedule = _rep.Where<MeetSchedule>(s => s.MeetId == newMeet.OldId).Select(x => new MeetScheduleView() { ScheduleStart = x.ScheduleStart.ToString(), ScheduleEnd = x.ScheduleEnd.ToString() }).OrderByDescending(o => o.ScheduleStart).ToList();

                    if (!newSchedule.Equals(oldSchedule))
                    {
                        var meetOrder = _rep.Where<MyMeetOrder>(x => x.MeetId == meet.Id && x.IsRemind == 1);
                        foreach (var item in meetOrder)
                        {
                            item.HasReminded = 0;

                        }
                        _rep.UpdateList<MyMeetOrder>(meetOrder);
                    }
                }


                //清除子表数据
                _rep.DeleteList(meetdata);
                _rep.DeleteList(meetspeaker);
                _rep.DeleteList(meetschedule);
                _rep.DeleteList(meetRelation);
                _rep.DeleteList(tags);
                _rep.DeleteList(coverPictures);
                _rep.DeleteList(doctorMeeting);

                _rep.SaveChanges();
            }
            else
            {
                meet = meetInfoView.Meet;

                _rep.Insert(meet);
                _rep.SaveChanges();
            }

            //线下会议需要生成签到二维码
            if ((meet.MeetType == 2 || meet.MeetType == 3) && string.IsNullOrEmpty(meet.MeetCodeUrl))
            {
                meet.MeetCodeUrl = GetQRImgUrl(meet.Id);
            }
            //全国会议
            if (meet.MeetType == 4 && string.IsNullOrEmpty(meet.MeetCodeUrl))
            {
                meet.MeetCodeUrl = GetQRImgUrl(meet.Id, 5);
            }

            //直播会议
            if (meet.MeetType == 5 && string.IsNullOrEmpty(meet.MeetCodeUrl))
            {
                meet.MeetCodeUrl = GetQRImgUrl(meet.Id, 6);
            }

            //添加参会医生名单
            if (meetInfoView.DoctorList != null)
            {
                foreach (var item in meetInfoView.DoctorList)
                {
                    DoctorMeeting doctorMeeting = new DoctorMeeting
                    {
                        Id = Guid.NewGuid().ToString(),
                        DoctorID = item,
                        MeetingID = meet.Id,
                        CreateUser = workUser.User.Id,
                        CreateTime = DateTime.Now
                    };
                    _rep.Insert(doctorMeeting);
                }
                _rep.SaveChanges();
            }
            else
            {
                //全部人员都可访问
                DoctorMeeting doctorMeeting = new DoctorMeeting
                {
                    Id = Guid.NewGuid().ToString(),
                    DoctorID = "00000000-0000-0000-0000-000000000000",
                    MeetingID = meet.Id,
                    CreateUser = workUser.User.Id,
                    CreateTime = DateTime.Now
                };
                _rep.Insert(doctorMeeting);
                _rep.SaveChanges();
            }

            //添加标签分组信息
            if (meetInfoView.TagGroupList != null)
            {
                foreach (var item in meetInfoView.TagGroupList)
                {
                    DoctorMeeting doctorMeeting = new DoctorMeeting
                    {
                        Id = Guid.NewGuid().ToString(),
                        TagGroupID = item,
                        MeetingID = meet.Id,
                        CreateUser = workUser.User.Id,
                        CreateTime = DateTime.Now
                    };
                    _rep.Insert(doctorMeeting);
                }
                _rep.SaveChanges();
            }

            //添加封面图片
            if (meetInfoView.CoverPictures != null)
            {
                foreach (var item in meetInfoView.CoverPictures)
                {
                    if (string.IsNullOrEmpty(item?.MeetPicType) || string.IsNullOrEmpty(item?.MeetPicUrl))
                    {
                        continue; //过滤空数据
                    }

                    item.Id = Guid.NewGuid().ToString();
                    item.MeetId = meet.Id;
                    switch (item.MeetPicType)
                    {
                        case "L":
                            meet.MeetCoverBig = item.Id;
                            break;
                        case "S":
                            meet.MeetCoverSmall = item.Id;
                            break;
                    }
                    item.CreateTime = DateTime.Now;
                    item.CreateUser = workUser.User.Id;
                    _rep.Insert(item);
                    _rep.SaveChanges();
                }
            }

            _rep.Update(meet);
            _rep.SaveChanges();

            //添加会议资料
            if (meetInfoView.Files != null)
            {
                foreach (var item in meetInfoView.Files)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.MeetId = meet.Id;
                    item.CreateTime = DateTime.Now;
                    item.CreateUser = workUser.User.Id;
                    _rep.Insert(item);
                    _rep.SaveChanges();
                }
            }

            //添加讲者
            if (meetInfoView.Speakers != null)
            {
                foreach (var item in meetInfoView.Speakers)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.MeetId = meet.Id;
                    item.CreateTime = DateTime.Now;
                    item.CreateUser = workUser.User.Id;
                    _rep.Insert(item);
                    _rep.SaveChanges();
                }
            }

            //添加会议日程
            if (meetInfoView.Schedules != null)
            {
                DateTime? startTime = null;
                DateTime? endTime = null;
                foreach (var item in meetInfoView.Schedules)
                {
                    var scheduledate = item.ScheduleDate;
                    scheduledate = Convert.ToDateTime(scheduledate).ToString("yyyy-MM-dd");

                    MeetSchedule Schedule;
                    foreach (var val in item.ScheduleViews)
                    {
                        if (meetInfoView.Speakers == null || meetInfoView.Speakers.Count() == 0)
                        {
                            val.Speaker.Id = Guid.NewGuid().ToString();
                            val.Speaker.MeetId = meet.Id;
                            val.Speaker.CreateTime = DateTime.Now;
                            val.Speaker.CreateUser = workUser.User.Id;
                            _rep.Insert(val.Speaker);
                            _rep.SaveChanges();
                        }
                        else
                        {
                            val.Speaker.Id = meetInfoView.Speakers.First(s => s.SpeakerName == val.Speaker.SpeakerName)?.Id;
                        }

                        Schedule = new MeetSchedule();
                        Schedule.Id = Guid.NewGuid().ToString();
                        Schedule.MeetId = meet.Id;
                        Schedule.MeetSpeakerId = val.Speaker.Id;
                        Schedule.ScheduleStart = DateTime.Parse(scheduledate + " " + val.Schedule.ScheduleStart);
                        Schedule.ScheduleEnd = DateTime.Parse(scheduledate + " " + val.Schedule.ScheduleEnd);
                        Schedule.ScheduleContent = val.Schedule.ScheduleContent;


                        Schedule.Sort = val.Schedule.Sort;
                        Schedule.AMPM = val.Schedule.AMPM;
                        Schedule.Topic = val.Schedule.Topic;
                        Schedule.Speaker = val.Schedule.Speaker;
                        Schedule.Hospital = val.Schedule.Hospital;

                        Schedule.CreateTime = DateTime.Now;
                        Schedule.CreateUser = workUser.User.Id;
                        Schedule.Remark = scheduledate;

                        if (!startTime.HasValue || Schedule.ScheduleStart < startTime)
                        {
                            startTime = Schedule.ScheduleStart;
                        }
                        if (!endTime.HasValue || Schedule.ScheduleEnd > endTime)
                        {
                            endTime = Schedule.ScheduleEnd;
                        }

                        _rep.Insert(Schedule);
                    }
                }
                if (startTime.HasValue) meet.MeetStartTime = startTime;
                if (endTime.HasValue) meet.MeetEndTime = endTime;
                _rep.Update(meet);
                _rep.SaveChanges();
            }

            //添加会议和产品和科室关系表
            if (meetInfoView.ProductAndDeps != null
                && meetInfoView.ProductAndDeps.BuNameList != null
                && meetInfoView.ProductAndDeps.Products != null
                && meetInfoView.ProductAndDeps.Departments != null)
            {
                MeetAndProAndDepRelation meetAndProAndDep;
                foreach (var buName in meetInfoView.ProductAndDeps.BuNameList)
                {
                    foreach (var pro in meetInfoView.ProductAndDeps.Products)
                    {
                        foreach (var dep in meetInfoView.ProductAndDeps.Departments)
                        {
                            meetAndProAndDep = new MeetAndProAndDepRelation();
                            meetAndProAndDep.Id = Guid.NewGuid().ToString();
                            meetAndProAndDep.MeetId = meet.Id;
                            meetAndProAndDep.ProductId = pro.ProId;
                            meetAndProAndDep.DepartmentId = dep.DeptId;
                            meetAndProAndDep.BuName = buName;
                            meetAndProAndDep.CreateTime = DateTime.Now;
                            meetAndProAndDep.CreateUser = workUser.User.Id;
                            meetAndProAndDep.DepartmentType = dep.DepartmentType;
                            _rep.Insert(meetAndProAndDep);
                        }
                    }
                }
                _rep.SaveChanges();
            }

            //会议标识
            if (meetInfoView.Tags != null)
            {
                MeetTag meetTag;
                foreach (var item in meetInfoView.Tags)
                {
                    meetTag = new MeetTag();
                    meetTag.Id = Guid.NewGuid().ToString();
                    meetTag.MeetId = meet.Id;
                    meetTag.TagId = item.Id;
                    meetTag.CreateTime = DateTime.Now;
                    meetTag.CreateUser = workUser.User.Id;
                    _rep.Insert(meetTag);
                }
                _rep.SaveChanges();
            }
        }

        /// <summary>
        /// 新增或更新会议信息
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateMeetInfo(MeetInfoViewModel meetInfoView, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string MeetType = string.Empty;
            if (meetInfoView?.Meet == null)
            {
                rvm.Msg = "The parameter 'Meet' is required.";
                rvm.Success = false;
                return rvm;
            }

            if (meetInfoView?.Schedules == null || meetInfoView.Schedules.Count() == 0)
            {
                rvm.Msg = "The parameter 'Schedules' is required, and cannot be empty.";
                rvm.Success = false;
                return rvm;
            }

            MeetInfo meet = null;

            if (!string.IsNullOrEmpty(meetInfoView.Meet.Id))
            {
                meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfoView.Meet.Id && s.IsDeleted != 1);
            }

            //if (meet != null && meet.MeetStartTime.HasValue && meet.MeetStartTime <= DateTime.Now)
            //{
            //    rvm.Msg = "This meeting is started, cannot change it now.";
            //    rvm.Success = false;
            //    return rvm;
            //}

            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    if (meet == null)
                    {
                        meetInfoView.Meet.Id = Guid.NewGuid().ToString();
                        meetInfoView.Meet.CreateTime = DateTime.Now;
                        meetInfoView.Meet.CreateUser = workUser.User.Id;
                        meetInfoView.Meet.IsCompleted = approvalEnabled ? EnumComplete.AddedUnapproved : EnumComplete.Approved;
                    }
                    else
                    {
                        if (meet.IsDeleted == 1)
                        {
                            rvm.Msg = "This meetting has been deleted.";
                            rvm.Success = false;
                            return rvm;
                        }
                        switch (meet.IsCompleted ?? EnumComplete.AddedUnapproved)
                        {
                            case EnumComplete.Approved:
                                if (approvalEnabled)
                                {
                                    meet.IsCompleted = EnumComplete.Locked;
                                    _rep.Update(meet);
                                    _rep.SaveChanges();

                                    //复制一条新数据
                                    meetInfoView.Meet.Id = Guid.NewGuid().ToString();
                                    meetInfoView.Meet.CreateTime = meet.CreateTime;
                                    meetInfoView.Meet.CreateUser = meet.CreateUser;
                                    meetInfoView.Meet.UpdateTime = DateTime.Now;
                                    meetInfoView.Meet.UpdateUser = workUser.User.Id;
                                    meetInfoView.Meet.IsCompleted = EnumComplete.UpdatedUnapproved;
                                    //meetInfoView.Meet.MeetCodeUrl = null; //修改会议，二维码不需要重新生成
                                    meetInfoView.Meet.OldId = meet.Id;
                                    meet = null;
                                }
                                else
                                {
                                    meet.UpdateTime = DateTime.Now;
                                    meet.UpdateUser = workUser.User.Id;
                                }
                                break;
                            case EnumComplete.Reject:
                            case EnumComplete.AddedUnapproved:
                            case EnumComplete.UpdatedUnapproved:
                                meet.IsCompleted = string.IsNullOrEmpty(meet.OldId) ? EnumComplete.AddedUnapproved : EnumComplete.UpdatedUnapproved;
                                meet.UpdateTime = DateTime.Now;
                                meet.UpdateUser = workUser.User.Id;
                                break;
                            default:
                                rvm.Msg = "This meeting info can not be changed at this time.";
                                rvm.Success = false;
                                return rvm;
                        }
                    }

                    AddOrUpdateMeetInfo(meet, meetInfoView, workUser);

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = new
                    {
                        meet = meet ?? meetInfoView.Meet
                    };

                    //新增邮件发送
                    if (meetInfoView.Meet != null && IsSendMail == "1")
                    {
                        MeetType = meetInfoView.Meet?.MeetType.ToString();
                        switch (MeetType)
                        {
                            case "1":
                                MeetType = "线上会议";
                                break;
                            case "2":
                                MeetType = "线下会议（科室会）";
                                break;
                            case "3":
                                MeetType = "线下会议（城市会）";
                                break;
                            default:
                                MeetType = "线上会议";
                                break;
                        }
                        MailUtil.SendMeetMail(workUser?.User?.ChineseName ?? "", MeetType, meetInfoView.Meet?.MeetTitle ?? "")
                                       .ContinueWith((previousTask) =>
                                       {
                                           bool rCount = previousTask.Result;
                                       });
                        //MailUtil.SendMeetMail(workUser?.User?.ChineseName??"", MeetType, meetInfoView.Meet?.MeetTitle ?? "");
                    }


                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = ex.Message;
                    rvm.Success = false;
                }
            }

            return rvm;
        }

        /// <summary>
        /// 处理删除会议的请求
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteMeetInfo(MeetInfo meetInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfo.Id);


            if (meet == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id";
                return rvm;
            }

            if (meet.IsDeleted == 1)
            {
                rvm.Msg = "This meeting info has been deleted.";
                rvm.Success = true;
                return rvm;
            }

            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核
            if (approvalEnabled)
            {
                switch (meet.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    case EnumComplete.Approved:
                        meet.IsCompleted = EnumComplete.WillDelete;  //将要删除（等待审核）
                        _rep.Update(meet);
                        _rep.SaveChanges();
                        //邮件发送
                        if (IsSendMail == "1")
                        {
                            string MeetType = meet?.MeetType.ToString();
                            switch (MeetType)
                            {
                                case "1":
                                    MeetType = "线上会议";
                                    break;
                                case "2":
                                    MeetType = "线下会议（科室会）";
                                    break;
                                case "3":
                                    MeetType = "线下会议（城市会）";
                                    break;
                                default:
                                    MeetType = "线上会议";
                                    break;
                            }
                            LoggerHelper.WarnInTimeTest("[DeleteMeetInfo] 会议删除开始：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            MailUtil.SendMeetMail(workUser?.User?.ChineseName, MeetType, meet?.MeetTitle ?? "")
                                       .ContinueWith((previousTask) =>
                                       {
                                           bool rCount = previousTask.Result;
                                       });
                            //MailUtil.SendMeetMail(workUser?.User?.ChineseName, MeetType, meet?.MeetTitle ?? "");
                            LoggerHelper.WarnInTimeTest("[DeleteMeetInfo] 会议删除结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }

                        break;
                    case EnumComplete.Reject:
                    case EnumComplete.AddedUnapproved:
                    case EnumComplete.UpdatedUnapproved:
                        DoMeetInfoDeletion(meet, workUser); //删除数据
                        break;
                    default:
                        rvm.Msg = "This meeting info can not be deleted at this time.";
                        rvm.Success = false;
                        return rvm;
                }
            }
            else
            {
                DoMeetInfoDeletion(meet, workUser); //删除数据
            }

            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 从数据库删除会议数据 （逻辑删除）
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        private void DoMeetInfoDeletion(MeetInfo meet, WorkUser workUser)
        {
            meet.IsDeleted = 1;
            meet.UpdateTime = DateTime.Now;
            meet.UpdateUser = workUser.User.Id;

            if (!string.IsNullOrEmpty(meet.OldId))
            {
                meet.IsCompleted = EnumComplete.CanceledUpdate; //修改后的副本变为取消修改状态

                //是否存在需要审核的其它记录
                bool hasApprovingCopy = _rep.Table<MeetInfo>().Any(s => s.IsDeleted != 1 && s.Id != meet.Id && s.OldId == meet.OldId);
                if (!hasApprovingCopy)
                {
                    //解锁原始数据
                    var meetOriginal = _rep.FirstOrDefault<MeetInfo>(s => s.IsDeleted != 1 && s.Id == meet.OldId && s.IsCompleted == EnumComplete.Locked);
                    if (meetOriginal != null)
                    {
                        meetOriginal.IsCompleted = EnumComplete.Approved;
                        _rep.Update(meetOriginal);

                    }
                }
            }
            _rep.Update(meet);
            _rep.SaveChanges();
        }

        /// <summary>
        /// 获取会议详情
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetInfo(MeetInfo meetInfo, WorkUser workUser)
        {

            ReturnValueModel rvm = new ReturnValueModel();
            var meetView = new MeetInfoViewModel();

            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfo.Id);

            if (meet == null)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                return rvm;
            }

            meet.IsCompleted = meet.IsCompleted ?? EnumComplete.AddedUnapproved;


            var deptRelations = _rep.Where<MeetAndProAndDepRelation>(s => s.MeetId == meet.Id && s.IsDeleted != 1);
            meetView.Meet = meet;
            //meetView.Files = _rep.Where<MeetFile>(s => s.MeetId == meet.Id);
            meetView.Speakers = _rep.Where<MeetSpeaker>(s => s.MeetId == meet.Id);
            var schedules = from a in _rep.Table<MeetSchedule>()
                            join b in _rep.Table<MeetSpeaker>() on a.MeetSpeakerId equals b.Id
                            where a.IsDeleted != 1 && a.MeetId == meet.Id && b.IsDeleted != 1
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
                                                        Speaker = v1.a.Speaker,
                                                        Hospital = v1.a.Hospital,
                                                        Sort = v1.a.Sort,
                                                        Topic = v1.a.Topic,
                                                        AMPM = v1.a.AMPM,
                                                        ScheduleStart = (SqlFunctions.DatePart("HOUR", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleStart) : SqlFunctions.DateName("HOUR", v1.a.ScheduleStart)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleStart) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleStart)),
                                                        ScheduleEnd = (SqlFunctions.DatePart("HOUR", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd) : SqlFunctions.DateName("HOUR", v1.a.ScheduleEnd)) + ":" + (SqlFunctions.DatePart("MINUTE", v1.a.ScheduleEnd) < 10 ? "0" + SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd) : SqlFunctions.DateName("MINUTE", v1.a.ScheduleEnd))
                                                    },
                                                    //Topic = v1.Topic,
                                                    Speakers = v1.a.Speaker,
                                                    Sort = v1.a.Sort,
                                                    Speaker = v1.b
                                                }
                            };

            meetView.Schedules = schedules.OrderBy(x => x.ScheduleDate);


            meetView.IsMeetEnd = DateTime.Now > meet.MeetEndTime ? 2 : DateTime.Now < meet.MeetStartTime ? 0 : 1;
            meetView.ProductAndDeps = new ProductBuDeptSelectionViewModel
            {
                BuNameList = from A in deptRelations
                             join D in _rep.Where<BuInfo>(s => s.IsDeleted != 1) on A.BuName equals D.BuName
                             group A by D.BuName into g1
                             select g1.Key,
                Products = from A in deptRelations
                           join E in _rep.Where<ProductInfo>(s => s.IsDeleted != 1) on A.ProductId equals E.Id
                           group A by E into g1
                           select new ProductSelectionViewModel
                           {
                               ProId = g1.Key.Id,
                               ProName = g1.Key.ProductName
                           },
                Departments = from A in deptRelations
                              join F in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1) on A.DepartmentId equals F.Id
                              group A by F into g1
                              select new DepartmentSelectionViewModel
                              {
                                  DeptId = g1.Key.Id,
                                  DeptName = g1.Key.DepartmentName,
                                  DeptType = g1.Key.DepartmentType
                              }
            };

            var MeetFiles = _rep.Where<MeetFile>(s => s.MeetId == meet.Id).ToList().Select(s => new
            {
                Title = s.Title,
                FileName = s.FileName,
                FilePath = s.FilePath,
                IsCopyRight = s.IsCopyRight,
                files = new List<object>
                    {
                        new {name = s.FileName,
                        url = s.FilePath}
                    }
            });
            //meetView.Files = list;

            //会议标识
            meetView.Tags = from a in _rep.Where<MeetTag>(s => s.IsDeleted != 1)
                            join b in _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "M2") on a.TagId equals b.Id
                            where a.MeetId == meet.Id
                            select new TagView
                            {
                                Id = b.Id,
                                TagName = b.TagName,
                                TextKey = b.TextKey
                            };

            //会议封面图片
            meetView.CoverPictures = from a in _rep.Table<MeetPic>()
                                     where a.IsDeleted != 1 && a.MeetId == meet.Id
                                     select a;

            //选择人员列表
            meetView.DoctorList = _rep.Where<DoctorMeeting>(x => x.MeetingID == meet.Id && !string.IsNullOrEmpty(x.DoctorID)).Select(x => x.DoctorID).ToList();
            //选择分组
            meetView.TagGroupList = _rep.Where<DoctorMeeting>(x => x.MeetingID == meet.Id && !string.IsNullOrEmpty(x.TagGroupID)).Select(x => x.TagGroupID).ToList();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                meet = meetView,
                MeetFiles = MeetFiles
            };

            return rvm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetInfoTest(MeetInfo meetInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var result = new
            {
                MeetInfo = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetInfo.Id),
                MeetSchedule = _rep.Where<MeetSchedule>(x => x.IsDeleted != 1 && x.MeetId == meetInfo.Id).GroupBy(x => x.Remark)
                    .Select(x => new
                    {
                        DateStr = x.Key,
                        ScheduleView = x.GroupBy(y => y.AMPM)
                            .Select(y => new
                            {
                                AMPM = y.Key,
                                ScheduleView = y.GroupBy(_ => _.Topic)
                                    .Select(_ => new
                                    {
                                        Topic = _.Key,
                                        Speaker = _.FirstOrDefault().Speaker,
                                        Schedule = _.Where(z => true)
                                    })
                            })
                    }),
                MeetFiles = _rep.Where<MeetFile>(s => s.MeetId == meetInfo.Id).ToList().Select(s => new
                {
                    Title = s.Title,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    IsCopyRight = s.IsCopyRight,
                    //files = new List<object>
                    //{
                    //    new { name = s.FileName,
                    //        url = s.FilePath }
                    //}
                }),
                CoverPictures = _rep.Where<MeetPic>(x => x.IsDeleted != 1 && x.MeetId == meetInfo.Id)
            };

            rvm.Msg = "fail";
            rvm.Success = true;
            rvm.Result = result;
            return rvm;
        }

        /// <summary>
        /// 获取会议签到一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel GetMeetSignUpList(RowNumModel<MeetInfo> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string meetId = rowNum?.SearchParams?.Id;
            if (string.IsNullOrEmpty(meetId))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var list = from a in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1)
                       join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on a.SignUpUserId equals b.Id
                       join c in _rep.Table<MeetInfo>() on a.MeetId equals c.Id
                       join d in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals d.Id
                       where a.MeetId == meetId && a.IsSignIn == 1
                       select new
                       {
                           DoctorId = b.UnionId,
                           DoctorName = b.UserName,
                           HospitalName = b.HospitalName,
                           DepartmentName = b.DepartmentName,
                           CreaterName = d.ChineseName,
                           SignInTime = a.CreateTime
                       };

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.SignInTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }

        /// <summary>
        /// 获取会议报名一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel GetMeetOrderList(RowNumModel<MeetInfo> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string meetId = rowNum?.SearchParams?.Id;
            if (string.IsNullOrEmpty(meetId))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }
            var list = from a in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetId && s.UnionId != null)
                       join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId != null) on a.UnionId equals b.UnionId
                       join c in _rep.Where<MeetInfo>(s => s.IsDeleted != 1) on a.MeetId equals c.Id
                       join d in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals d.Id
                       select new MeetSituationViewModel
                       {
                           DoctorId = b.UnionId,
                           DoctorName = b.UserName,
                           HospitalName = b.HospitalName,
                           DepartmentName = b.DepartmentName,
                           CreaterName = d.ChineseName,
                           OrderTime = a.CreateTime
                       };

            //  var list2 = from a in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetId)
            //              join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on a.UnionId equals b.UnionId
            //              join c in _rep.Table<MeetInfo>() on a.MeetId equals  c.Id
            //              join d in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals d.Id
            //              join e in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1) on a.MeetId equals e.MeetId
            //              select new
            //              {
            //                  DoctorId = b.UnionId,
            //                  DoctorName = b.UserName,
            //                  HospitalName = b.HospitalName,
            //                  DepartmentName = b.DepartmentName,
            //                  CreaterName = d.ChineseName,
            //                  OrderTime = a.CreateTime,
            //                  SignInTime = e.SignInTime

            //                                       };
            //var send=  list2.Union(list2);

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.OrderTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }

        /// <summary>
        /// 导出会议签到列表
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel ExportMeetSignUpList(MeetInfo meetInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(meetInfo?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var list = from a in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1)
                       join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on a.SignUpUserId equals b.Id
                       join c in _rep.Where<MeetInfo>(s => s.IsDeleted != 1) on a.MeetId equals c.Id
                       join d in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals d.Id
                       where a.MeetId == meetInfo.Id && a.MeetId == c.Id && c.Id == meetInfo.Id
                       select new MeetSituationViewModel
                       {
                           DoctorId = b.UnionId,
                           DoctorName = b.UserName,
                           HospitalName = b.HospitalName,
                           DepartmentName = b.DepartmentName,
                           CreaterName = d.ChineseName,
                           SignInTime = a.CreateTime
                       };

            var _root = AppDomain.CurrentDomain.BaseDirectory;
            var _dir = "\\download\\";
            var _path = _root + _dir;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            var filename = "Meeting_SignUp_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>() { "DoctorId", "DoctorName", "HospitalName", "DepartmentName", "CreaterName", "SignInTime" };
            var isSuccess = helper.Export(list.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = _dir + filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 导出会议报名列表
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel ExportMeetOrderList(MeetInfo meetInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(meetInfo?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var list = from a in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetInfo.Id)
                       join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on a.UnionId equals b.UnionId
                       join c in _rep.Where<MeetInfo>(s => s.IsDeleted != 1) on a.MeetId equals c.Id
                       join d in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals d.Id
                       select new MeetSituationViewModel
                       {
                           DoctorId = b.UnionId,
                           DoctorName = b.UserName,
                           HospitalName = b.HospitalName,
                           DepartmentName = b.DepartmentName,
                           CreaterName = d.ChineseName,
                           OrderTime = a.CreateTime
                       };

            var _root = AppDomain.CurrentDomain.BaseDirectory;
            var _dir = "\\download\\";
            var _path = _root + _dir;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            var filename = "Meeting_Order_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>() { "DoctorId", "DoctorName", "HospitalName", "DepartmentName", "CreaterName", "OrderTime" };
            var isSuccess = helper.Export(list.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = _dir + filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 获取参会医生报告一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel GetMeetSituation(RowNumModel<MeetInfo> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string meetId = rowNum?.SearchParams?.Id;
            if (string.IsNullOrEmpty(meetId))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var listSignUp = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1)
                             join b in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1 && s.MeetId == meetId && s.IsSignIn == 1) on a.Id equals b.SignUpUserId into ab
                             from b1 in ab.DefaultIfEmpty()
                             join c in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetId) on a.UnionId equals c.UnionId into ac
                             from c1 in ac.DefaultIfEmpty()
                             group new { b1, c1 } by a into g1
                             where g1.Any(s => s.b1 != null || s.c1 != null)
                             select new
                             {
                                 MeetId = meetId,
                                 WxUser = g1.Key,
                                 IsKnewDetail = true,
                                 SignInTime = (from v1 in g1 where v1.b1 != null && v1.b1.IsSignIn.HasValue && v1.b1.IsSignIn == 1 select v1.b1.SignInTime).FirstOrDefault(),
                                 OrderTime = (from v1 in g1 where v1.c1 != null select v1.c1.CreateTime).FirstOrDefault()
                             };

            var list = from a in _rep.Table<MeetInfo>()
                       join b in _rep.Where<UserModel>(s => s.IsDeleted != 1) on a.CreateUser equals b.Id
                       join c in listSignUp on a.Id equals c.MeetId
                       where a.Id == meetId
                       select new MeetSituationViewModel
                       {
                           MeetId = a.Id,
                           MeetTitle = a.MeetTitle,
                           MeetAddress = a.MeetAddress,
                           MeetStartTime = a.MeetStartTime,
                           MeetEndTime = a.MeetEndTime,
                           DoctorId = c.WxUser.UnionId,
                           DoctorName = c.WxUser.UserName,
                           DoctorTitle = c.WxUser.Title,
                           HospitalName = c.WxUser.HospitalName,
                           DepartmentName = c.WxUser.DepartmentName,
                           Mobile = c.WxUser.Mobile,
                           UNIONID = c.WxUser.UnionId,
                           CreaterName = b.ChineseName,
                           IsKnewDetail = c.IsKnewDetail ? "Y" : "N",
                           SignInTime = c.SignInTime,
                           OrderTime = c.OrderTime
                       };

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.SignInTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }

        /// <summary>
        /// 导出参会医生报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel ExportMeetSituation(MeetInfo meetInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string meetId = meetInfo?.Id;
            if (string.IsNullOrEmpty(meetId))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }



            //var listSignUp = from a in _rep.All<WxUserModel>()
            //                 join b in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1 && s.MeetId == meetId) on a.Id equals b.SignUpUserId into ab
            //                 from b1 in ab.DefaultIfEmpty()
            //                 join c in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetId) on a.UnionId equals c.UnionId into ac
            //                 from c1 in ac.DefaultIfEmpty()
            //                 group new { b1, c1 } by a into g1
            //                 where g1.Any(s => s.b1 != null || s.c1 != null)
            //                 select new
            //                 {
            //                     MeetId = meetId,
            //                     WxUser = g1.Key,
            //                     IsKnewDetail = g1.Any(s => s.b1 != null && s.b1.IsKnewDetail == 1),
            //                     SignInTime = (from v1 in g1 where v1.b1 != null && v1.b1.IsSignIn.HasValue && v1.b1.IsSignIn == 1 select v1.b1.SignInTime).FirstOrDefault(),
            //                     OrderTime = (from v1 in g1 where v1.c1 != null select v1.c1.CreateTime).FirstOrDefault()
            //                 };

            //var list = from a in _rep.Where<MeetInfo>(s => s.IsDeleted != 1)
            //           join c in listSignUp on a.Id equals c.MeetId
            //           join b in _rep.All<UserModel>() on c.WxUser.Id equals b.Id
            //           where a.Id == meetId
            //           select new MeetSituationViewModel
            //           {
            //               MeetId = a.Id,
            //               MeetTitle = a.MeetTitle,
            //               MeetAddress = a.MeetAddress,
            //               MeetStartTime = a.MeetStartTime,
            //               MeetEndTime = a.MeetEndTime,
            //               DoctorId = c.WxUser.UnionId,
            //               DoctorName = c.WxUser.UserName,
            //               DoctorTitle = c.WxUser.Title,
            //               HospitalName = c.WxUser.HospitalName,
            //               DepartmentName = c.WxUser.DepartmentName,
            //               Mobile = c.WxUser.Mobile,
            //               UNIONID = c.WxUser.UnionId,
            //               CreaterName = b.ChineseName,
            //               IsKnewDetail = c.IsKnewDetail ? "Y" : "N",
            //               SignInTime = c.SignInTime,
            //               OrderTime = c.OrderTime
            //           };

            var listSignUp = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1)
                             join b in _rep.Where<MeetSignUp>(s => s.IsDeleted != 1 && s.MeetId == meetId && s.IsSignIn == 1) on a.Id equals b.SignUpUserId into ab
                             from b1 in ab.DefaultIfEmpty()
                             join c in _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.MeetId == meetId) on a.UnionId equals c.UnionId into ac
                             from c1 in ac.DefaultIfEmpty()
                             group new { b1, c1 } by a into g1
                             where g1.Any(s => s.b1 != null || s.c1 != null)
                             select new
                             {
                                 MeetId = meetId,
                                 WxUser = g1.Key,
                                 IsKnewDetail = true,
                                 SignInTime = (from v1 in g1 where v1.b1 != null && v1.b1.IsSignIn.HasValue && v1.b1.IsSignIn == 1 select v1.b1.SignInTime).FirstOrDefault(),
                                 OrderTime = (from v1 in g1 where v1.c1 != null select v1.c1.CreateTime).FirstOrDefault()
                             };

            var list = from a in _rep.Table<MeetInfo>()
                       join b in _rep.Where<UserModel>(s => s.IsDeleted != 1) on a.CreateUser equals b.Id
                       join c in listSignUp on a.Id equals c.MeetId
                       where a.Id == meetId
                       select new MeetSituationViewModel
                       {
                           MeetId = a.Id,
                           MeetTitle = a.MeetTitle,
                           MeetAddress = a.MeetAddress,
                           MeetStartTime = a.MeetStartTime,
                           MeetEndTime = a.MeetEndTime,
                           DoctorId = c.WxUser.UnionId,
                           DoctorName = c.WxUser.UserName,
                           DoctorTitle = c.WxUser.Title,
                           HospitalName = c.WxUser.HospitalName,
                           DepartmentName = c.WxUser.DepartmentName,
                           Mobile = c.WxUser.Mobile,
                           UNIONID = c.WxUser.UnionId,
                           CreaterName = b.ChineseName,
                           IsKnewDetail = c.IsKnewDetail ? "Y" : "N",
                           SignInTime = c.SignInTime,
                           OrderTime = c.OrderTime
                       };

            var _root = AppDomain.CurrentDomain.BaseDirectory;
            var _dir = "\\download\\";
            var _path = _root + _dir;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            var filename = "MeetInfo_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>() { "MeetId", "MeetTitle", "MeetAddress", "MeetStartTime", "MeetEndTime", "DoctorId", "DoctorName", "DoctorTitle", "HospitalName", "DepartmentName", "Mobile", "UNIONID", "CreaterName", "IsKnewDetail", "SignInTime" };
            var isSuccess = helper.Export(list.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = _dir + filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 获取会议调研报告一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel GetMeetQAResultList(RowNumModel<MeetInfo> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(rowNum?.SearchParams?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var answers = _rep.Where<AnswerModel>(s => s.IsDeleted != 1);

            var list = from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == rowNum.SearchParams.Id)
                       orderby a.CreateTime
                       group new { a.UserAnswerId, a.UserAnswer, a.CreateTime } by new { a.MeetId, a.MeetQAId, a.SignUpUserId } into g1
                       join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1) on g1.Key.MeetQAId equals b.Id
                       join c in _rep.Table<MeetInfo>() on g1.Key.MeetId equals c.Id
                       join d in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on g1.Key.SignUpUserId equals d.Id
                       join e in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals e.Id
                       join f in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on b.QuestionId equals f.Id
                       select new
                       {
                           MeetId = g1.Key.MeetId,
                           MeetTitle = c.MeetTitle,
                           MeetAddress = c.MeetAddress,
                           MeetStartTime = c.MeetStartTime,
                           MeetEndTime = c.MeetEndTime,
                           DoctorId = d.yunshi_doctor_id,
                           DoctorName = d.UserName,
                           DoctorTitle = d.Title,
                           HospitalName = d.HospitalName,
                           DepartmentName = d.DepartmentName,
                           Mobile = d.Mobile,
                           UNIONID = d.UnionId,
                           QAType = b.QAType,
                           QuestionId = f.Id,
                           Question = f.QuestionContent,
                           Answers = (from g in answers
                                      where g.QuestionId == b.QuestionId && g.IsRight.HasValue && g.IsRight.Value
                                      select g.AnswerContent).ToList(),
                           QuestionType = f.QuestionType,
                           UserAnswers = (from v1 in g1
                                          join h in answers on v1.UserAnswerId equals h.Id into ah
                                          from h1 in ah.DefaultIfEmpty()
                                          select h1 == null ? v1.UserAnswer : h1.AnswerContent
                                          //select (f.QuestionType == 1 /*单选题*/ || f.QuestionType == 2 /*多选题*/) ?
                                          //  (h1 == null ? "" : h1.AnswerContent) /*单选题和多选题*/ :
                                          //  v1.UserAnswer /*填空题*/
                                         ).Distinct(),
                           AnswerTime = (from v1 in g1 select v1.CreateTime).FirstOrDefault()
                       };

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.AnswerTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            var list2 = from a in rows
                        select new MeetQAResultViewModel
                        {
                            MeetId = a.MeetId,
                            MeetTitle = a.MeetTitle,
                            MeetAddress = a.MeetAddress,
                            MeetStartTime = a.MeetStartTime,
                            MeetEndTime = a.MeetEndTime,
                            DoctorId = a.DoctorId,
                            DoctorName = a.DoctorName,
                            DoctorTitle = a.DoctorTitle,
                            HospitalName = a.HospitalName,
                            DepartmentName = a.DepartmentName,
                            Mobile = a.Mobile,
                            UNIONID = a.UNIONID,
                            QAType = a.QAType == 1 ? "会前" : "会后",
                            QuestionId = a.QuestionId,
                            Question = a.Question,
                            Answers = a.Answers.Any() ? a.Answers.Aggregate((s1, s2) => (s1 ?? "") + "," + (s2 ?? "")) : "",
                            QuestionType = a.QuestionType == 1 ? "单选" : a.QuestionType == 2 ? "多选" : a.QuestionType == 3 ? "填空" : "",
                            UserAnswers = a.UserAnswers.Any() ? a.UserAnswers.Aggregate((s1, s2) => (s1 ?? "") + "," + (s2 ?? "")) : "",
                            AnswerTime = a.AnswerTime.HasValue ? a.AnswerTime.Value.ToString("yyyy-MM-dd HH:mm") : ""
                        };

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows = list2
            };

            return rvm;
        }

        /// <summary>
        /// 导出会议调研报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        public ReturnValueModel ExportMeetQAResultReport(MeetInfo meetInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(meetInfo?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "Require a meeting Id";
                return rvm;
            }

            var answers = _rep.Where<AnswerModel>(s => s.IsDeleted != 1);

            var list = from a in _rep.Where<MeetQAResult>(s => s.IsDeleted != 1 && s.MeetId == meetInfo.Id)
                       orderby a.CreateTime
                       group new { a.UserAnswerId, a.UserAnswer, a.CreateTime } by new { a.MeetId, a.MeetQAId, a.SignUpUserId } into g1
                       join b in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1) on g1.Key.MeetQAId equals b.Id
                       join c in _rep.Where<MeetInfo>(s => s.IsDeleted != 1) on g1.Key.MeetId equals c.Id
                       join d in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on g1.Key.SignUpUserId equals d.Id
                       join e in _rep.Where<UserModel>(s => s.IsDeleted != 1) on c.CreateUser equals e.Id
                       join f in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on b.QuestionId equals f.Id
                       select new
                       {
                           MeetId = g1.Key.MeetId,
                           MeetTitle = c.MeetTitle,
                           MeetAddress = c.MeetAddress,
                           MeetStartTime = c.MeetStartTime,
                           MeetEndTime = c.MeetEndTime,
                           // DoctorId = d.yunshi_doctor_id,
                           DoctorName = d.UserName,
                           DoctorTitle = d.Title,
                           HospitalName = d.HospitalName,
                           DepartmentName = d.DepartmentName,
                           Mobile = d.Mobile,
                           UNIONID = d.UnionId,
                           QAType = b.QAType,
                           QuestionId = f.Id,
                           Question = f.QuestionContent,
                           Answers = (from g in answers
                                      where g.QuestionId == b.QuestionId && g.IsRight.HasValue && g.IsRight.Value
                                      select g.AnswerContent).ToList(),
                           QuestionType = f.QuestionType,
                           UserAnswers = (from v1 in g1
                                          join h in answers on v1.UserAnswerId equals h.Id into ah
                                          from h1 in ah.DefaultIfEmpty()
                                          select h1 == null ? v1.UserAnswer : h1.AnswerContent
                                          //select (f.QuestionType == 1 /*单选题*/ || f.QuestionType == 2 /*多选题*/) ?
                                          //  (h1 == null ? "" : h1.AnswerContent) /*单选题和多选题*/ :
                                          //  v1.UserAnswer /*填空题*/
                                            ).Distinct(),
                           AnswerTime = (from v1 in g1 select v1.CreateTime).FirstOrDefault()
                       };

            var list2 = from a in list.ToList()
                        select new MeetQAResultViewModel
                        {
                            MeetId = a.MeetId,
                            MeetTitle = a.MeetTitle,
                            MeetAddress = a.MeetAddress,
                            MeetStartTime = a.MeetStartTime,
                            MeetEndTime = a.MeetEndTime,
                            // DoctorId = a.DoctorId,
                            DoctorName = a.DoctorName,
                            DoctorTitle = a.DoctorTitle,
                            HospitalName = a.HospitalName,
                            DepartmentName = a.DepartmentName,
                            Mobile = a.Mobile,
                            UNIONID = a.UNIONID,
                            QAType = a.QAType == 1 ? "会前" : "会后",
                            QuestionId = a.QuestionId,
                            Question = a.Question,
                            Answers = a.Answers.Any() ? a.Answers.Aggregate((s1, s2) => (s1 ?? "") + "," + (s2 ?? "")) : "",
                            QuestionType = a.QuestionType == 1 ? "单选" : a.QuestionType == 2 ? "多选" : a.QuestionType == 3 ? "填空" : "",
                            UserAnswers = a.UserAnswers.Any() ? a.UserAnswers.Aggregate((s1, s2) => (s1 ?? "") + "," + (s2 ?? "")) : "",
                            AnswerTime = a.AnswerTime?.ToString("yyyy-MM-dd HH:mm") ?? ""
                        };

            var _root = AppDomain.CurrentDomain.BaseDirectory;
            var _dir = "\\download\\";
            var _path = _root + _dir;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            var filename = "MeetInfo_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>() { "MeetId", "MeetTitle", "MeetAddress", "MeetStartTime", "MeetEndTime", "DoctorName", "DoctorTitle", "HospitalName", "DepartmentName", "Mobile", "UNIONID", "QAType", "QuestionId", "Question", "Answers", "QuestionType", "UserAnswers", "AnswerTime" };
            var isSuccess = helper.Export(list2.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = _dir + filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 从其它会议导入题目或者从题库中选择题目
        /// </summary>
        /// <param name="meetQAs"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateMeetQA(MeetQARelationViewModel meetQAs, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            IEnumerable<string> questionIdList;
            IEnumerable<MeetQAContentViewModel> listNew;

            if (string.IsNullOrEmpty(meetQAs?.MeetId))
            {
                rvm.Msg = "The parameter 'MeetId' is required.";
                rvm.Success = false;
                return rvm;
            }

            //如果来源会议Id不为空，说明是过往会议的问卷添加
            if (!string.IsNullOrEmpty(meetQAs?.FromMeetId))
            {
                if (!meetQAs.FromQAType.HasValue)
                {
                    rvm.Msg = "The parameter 'FromQAType' is required if exsits 'FromMeetId'.";
                    rvm.Success = false;
                    return rvm;
                }
                questionIdList = _rep.Where<MeetQAModel>(s => s.MeetId == meetQAs.FromMeetId && s.QAType == meetQAs.FromQAType).Select(s => s.QuestionId).Distinct();
            }
            else if (meetQAs?.meetQAs != null)
            {
                questionIdList = meetQAs.meetQAs.Select(s => s?.QuestionId).Distinct();
            }
            else
            {
                rvm.Msg = "One of parameter 'meetQAs' or 'FromMeetId' is required.";
                rvm.Success = false;
                return rvm;
            }

            questionIdList = questionIdList.ToList();

            listNew = from b in _rep.Where<QuestionModel>(s => s.IsDeleted != 1 && questionIdList.Contains(s.Id))
                      join c in _rep.Where<AnswerModel>(s => s.IsDeleted != 1) on b.Id equals c.QuestionId into bc
                      from c1 in bc.DefaultIfEmpty()
                      group c1 by b into g1
                      select new MeetQAContentViewModel
                      {
                          Question = g1.Key,
                          Answers = from v1 in g1
                                    where v1 != null
                                    select v1
                      };

            listNew = listNew.ToList();

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    if (listNew.Count() > 0)
                    {
                        //_rep.DeleteList(list);

                        foreach (var item in listNew)
                        {
                            var question = item.Question;
                            if (question.MeetId != meetQAs.MeetId)
                            {
                                //题库或者其他会议的题目，需要复制
                                question = new QuestionModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetId = meetQAs.MeetId,
                                    QuestionType = item.Question.QuestionType,
                                    QuestionContent = item.Question.QuestionContent,
                                    QuestionOfA = item.Question.QuestionOfA
                                };
                                _rep.Insert(question);

                                foreach (var a in item.Answers)
                                {
                                    var answer = new AnswerModel
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        QuestionId = question.Id,
                                        AnswerContent = a.AnswerContent,
                                        Sort = a.Sort,
                                        IsRight = a.IsRight
                                    };
                                    _rep.Insert(answer);
                                }
                            }
                            if (question.MeetId == meetQAs.MeetId && meetQAs.FromQAType != meetQAs.QAType)
                            {
                                //题库或者其他会议的题目，需要复制
                                question = new QuestionModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MeetId = meetQAs.MeetId,
                                    QuestionType = item.Question.QuestionType,
                                    QuestionContent = item.Question.QuestionContent,
                                    QuestionOfA = item.Question.QuestionOfA
                                };
                                _rep.Insert(question);

                                foreach (var a in item.Answers)
                                {
                                    var answer = new AnswerModel
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        QuestionId = question.Id,
                                        AnswerContent = a.AnswerContent,
                                        Sort = a.Sort,
                                        IsRight = a.IsRight
                                    };
                                    _rep.Insert(answer);
                                }
                            }
                            var s = meetQAs.meetQAs.FirstOrDefault(o => o.QuestionId == question.Id);
                            var meetQA = new MeetQAModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetId = meetQAs.MeetId,
                                QAType = meetQAs.QAType,
                                QuestionId = question.Id,
                                Sort = s?.Sort,
                                CreateTime = DateTime.Now,
                                CreateUser = workUser.User.Id
                            };
                            _rep.Insert(meetQA);
                        }
                        _rep.SaveChanges();
                    }

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
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMeetQAInfo(MeetQARelationViewModel meetQA)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == meetQA.MeetId);
            var mpdr = _rep.Where<MeetAndProAndDepRelation>(s => s.MeetId == meetQA.MeetId).ToList();
            var prodIds = mpdr.Select(s => s.ProductId).ToList();
            var deptIds = mpdr.Select(s => s.DepartmentId).ToList();
            var prods = _rep.Where<ProductInfo>(s => prodIds.Contains(s.Id)).Distinct().Select(s => s.ProductName).ToList();
            var depts = _rep.Where<DepartmentInfo>(s => prodIds.Contains(s.Id)).Distinct().Select(s => s.DepartmentName).ToList();

            var meetQAModels = _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meetQA.MeetId).OrderBy(o => o.Sort).ToList();

            var list = new List<MeetQAContentViewModel>();

            foreach (var item in meetQAModels)
            {
                var question = _rep.FirstOrDefault<QuestionModel>(s => s.IsDeleted != 1 && s.Id == item.QuestionId);
                if (question != null)
                {
                    var answers = _rep.Where<AnswerModel>(s => s.IsDeleted != 1 && s.QuestionId == item.QuestionId).OrderBy(o => o.Sort).ToList();
                    list.Add(new MeetQAContentViewModel
                    {

                        Question = question,
                        Answers = answers,
                    });
                }
            }

            //var list = (from a in _rep.Where<MeetQAModel>(s => s.IsDeleted != 1 && s.MeetId == meetQA.MeetId)
            //            join b in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on a.QuestionId equals b.Id into ab
            //            from bb in ab.DefaultIfEmpty()
            //            join c in _rep.Where<AnswerModel>(s => s.IsDeleted != 1) on bb.Id equals c.QuestionId into bc
            //            from c1 in bc.DefaultIfEmpty()
            //            where a.QAType == meetQA.QAType
            //            group c1 by bb into g1
            //            select new MeetQAContentViewModel
            //            {
            //                Question = g1.Key,
            //                Answers = g1.Select(s => s).OrderBy(a => a.Sort)
            //            });
            var ContentValue = list.Count() > 0 ? 1 : 0;

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                meet = new
                {
                    Title = meet.MeetTitle,
                    Time = meet.MeetStartTime?.ToString("yyyy-MM-dd") + " ~ " + meet.MeetEndTime?.ToString("yyyy-MM-dd"),
                    Products = prods.Any() ? prods.Aggregate((s, a) => s + "," + a) : "",
                    Depts = depts.Any() ? depts.Aggregate((s, a) => s + "," + a) : ""
                },
                meetQA.QAType,
                ContentValue,
                list
            };

            return rvm;
        }

        /// <summary>
        /// 删除会议问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteMeetQAInfo(MeetQARelationViewModel meetQA)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            List<string> questionIds = null;
            if (meetQA?.meetQAs != null)
            {
                questionIds = (from a in meetQA.meetQAs
                               select a.QuestionId).ToList();
            }
            var meetQAInfo = from a in _rep.Where<MeetQAModel>(s => s.MeetId == meetQA.MeetId && s.QAType == meetQA.QAType)
                             join b in _rep.Where<QuestionModel>(s => s.IsDeleted != 1) on a.QuestionId equals b.Id into ab
                             from b1 in ab.DefaultIfEmpty()
                             join c in _rep.Where<AnswerModel>(s => s.IsDeleted != 1) on b1.Id equals c.QuestionId into bc
                             from c1 in bc.DefaultIfEmpty()
                             group c1 by new { a, b1 } into g1
                             select new
                             {
                                 MeetQA = g1.Key.a,
                                 Question = g1.Key.b1,
                                 Answers = g1.Select(s => s)
                             };


            if (questionIds != null && questionIds.Count > 0)
            {
                meetQAInfo = meetQAInfo.Where(s => questionIds.Contains(s.MeetQA.QuestionId));
            }
            if (meetQAInfo.Count() > 0)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var s in meetQAInfo)
                        {
                            s.MeetQA.IsDeleted = 1;
                            _rep.Update(s.MeetQA);
                            if (s.Question != null && s.MeetQA.MeetId == s.Question.MeetId)
                            {
                                s.Question.IsDeleted = 1;
                                _rep.Update(s.Question);
                                foreach (var a in s.Answers)
                                {
                                    if (a != null)
                                    {
                                        a.IsDeleted = 1;
                                        _rep.Update(a);
                                    }
                                }
                            }
                        }
                        _rep.SaveChanges();

                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();

                        rvm.Msg = "fail";
                        rvm.Success = false;
                    }
                }
            }
            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 获取签到二维码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private string GetQRImgUrl(string Id, int type = 1)
        {
            var _path = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "QRCode\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

            string HostUrl = ConfigurationManager.AppSettings["QRCodeAddress"];
            string query = "?APPID=0&Type=" + type + "&ActivityID=" + Id;

            var fs = QRCodeUtils.DownQrCodeFile(HostUrl + query, _path + fileName, "");
            if (fs)
            {
                return fileName.Replace('\\', '/');
            }
            return "";
        }

        /// <summary>
        /// 提交会议的审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel Approval(ApprovalResultViewModel approvalResult, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //var isAdmin = workUser.Roles.FirstOrDefault().RoleName.Contains("Admin");
            var isAdmin = _commonService.IsAdmin(workUser);
            if (!isAdmin)
            {
                rvm.Msg = "You have no administrator permission.";
                rvm.Success = false;
                return rvm;
            }

            if (approvalResult == null || !approvalResult.Approved.HasValue || string.IsNullOrEmpty(approvalResult.Id))
            {
                rvm.Msg = "Invalid parameter.";
                rvm.Success = false;
                return rvm;
            }

            MeetInfo modifiedMeet = null, originalMeet = null;
            modifiedMeet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == approvalResult.Id);
            var doctorList = _rep.Where<DoctorMeeting>(x => x.MeetingID == approvalResult.Id && !string.IsNullOrEmpty(x.DoctorID)).Select(x => x.DoctorID).ToList();
            var doctorTagList = _rep.Where<DoctorMeeting>(x => x.MeetingID == approvalResult.Id && !string.IsNullOrEmpty(x.TagGroupID)).Select(x => x.TagGroupID).ToList();
            if (modifiedMeet == null)
            {
                rvm.Msg = $"Invalid meet Id: {approvalResult.Id}";
                rvm.Success = false;
                return rvm;
            }

            switch (modifiedMeet.IsCompleted ?? EnumComplete.AddedUnapproved)
            {
                case EnumComplete.AddedUnapproved:
                case EnumComplete.UpdatedUnapproved:
                case EnumComplete.Reject:
                case EnumComplete.WillDelete:
                    break;
                default:
                    rvm.Msg = $"This meeting is not unapproved.";
                    rvm.Success = false;
                    return rvm;
            }

            if (!string.IsNullOrEmpty(modifiedMeet.OldId))
            {
                originalMeet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == modifiedMeet.OldId);
                if (originalMeet == null)
                {
                    rvm.Msg = "Data is broken. Invalid original meet Id.";
                    rvm.Success = false;
                    return rvm;
                }
            }

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    switch (modifiedMeet.IsCompleted ?? EnumComplete.AddedUnapproved)
                    {
                        case EnumComplete.AddedUnapproved:
                        case EnumComplete.UpdatedUnapproved:
                        case EnumComplete.Reject:
                            if (approvalResult.Approved ?? false)
                            {
                                if (originalMeet == null)
                                {
                                    modifiedMeet.IsCompleted = EnumComplete.Approved; //审核通过
                                    _rep.Update(modifiedMeet);
                                }
                                else
                                {
                                    AddOrUpdateMeetInfo(originalMeet, new MeetInfoViewModel
                                    {
                                        Meet = modifiedMeet,
                                        DoctorList = doctorList,
                                        TagGroupList = doctorTagList
                                    }, workUser);

                                    originalMeet.MeetCoverBig = modifiedMeet.MeetCoverBig;
                                    originalMeet.MeetCoverSmall = modifiedMeet.MeetCoverSmall;

                                    //转移子表关联
                                    var meetdata = _rep.Where<MeetFile>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    var meetspeaker = _rep.Where<MeetSpeaker>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    var meetschedule = _rep.Where<MeetSchedule>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    var meetRelation = _rep.Where<MeetAndProAndDepRelation>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    var tags = _rep.Where<MeetTag>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    var coverPictures = _rep.Where<MeetPic>(s => s.MeetId == modifiedMeet.Id && s.IsDeleted != 1);
                                    //var doctorMeeting = _rep.Where<DoctorMeeting>(x => x.MeetingID == modifiedMeet.Id && x.IsDeleted != 1);

                                    foreach (var item in meetdata)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    foreach (var item in meetspeaker)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    foreach (var item in meetschedule)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    foreach (var item in meetRelation)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    foreach (var item in tags)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    foreach (var item in coverPictures)
                                    {
                                        item.MeetId = originalMeet.Id;
                                    }
                                    //foreach (var item in doctorMeeting)
                                    //{
                                    //    item.MeetingID= originalMeet.Id;
                                    //}

                                    _rep.UpdateList(meetdata);
                                    _rep.UpdateList(meetspeaker);
                                    _rep.UpdateList(meetschedule);
                                    _rep.UpdateList(meetRelation);
                                    _rep.UpdateList(tags);
                                    _rep.UpdateList(coverPictures);
                                    //_rep.UpdateList(doctorMeeting);

                                    modifiedMeet.IsCompleted = EnumComplete.Obsolete;  //已作废 
                                    modifiedMeet.IsDeleted = 1;
                                    _rep.Update(modifiedMeet);

                                    originalMeet.IsCompleted = EnumComplete.Approved; //原记录审核通过
                                    originalMeet.UpdateUser = modifiedMeet.UpdateUser;
                                    originalMeet.UpdateTime = modifiedMeet.UpdateTime;
                                    _rep.Update(originalMeet);
                                }
                            }
                            else
                            {
                                modifiedMeet.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedMeet.ApprovalNote = approvalResult.Note;
                                _rep.Update(modifiedMeet);
                            }
                            break;
                        case EnumComplete.WillDelete:
                            if (originalMeet == null)
                            {
                                if (approvalResult.Approved ?? false)
                                {
                                    //同意删除
                                    modifiedMeet.IsDeleted = 1;
                                    modifiedMeet.IsCompleted = EnumComplete.Deleted;
                                }
                                else
                                {
                                    //拒绝删除
                                    modifiedMeet.IsCompleted = EnumComplete.Approved;
                                }
                                _rep.Update(modifiedMeet);
                            }
                            break;
                    }

                    _rep.SaveChanges();

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }
            return rvm;
        }

        /// <summary>
        /// 会议人员限制
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetDoctorMeeting(RowNumModel<MeetInfo> meetInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (meetInfo.SearchParams.Id != null)
            {
                var list = _rep.Where<DoctorMeeting>(x => x.MeetingID == meetInfo.SearchParams.Id).Select(x => x.DoctorID).Distinct().ToList();
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    total = list.Count(),
                    rows = list.ToPaginationList(meetInfo.PageIndex, meetInfo.PageSize).ToList()
                };
            }
            else
            {
                rvm.Msg = "Invalid parameter.";
                rvm.Success = false;
                return rvm;
            }
            return rvm;
        }


    }
}
