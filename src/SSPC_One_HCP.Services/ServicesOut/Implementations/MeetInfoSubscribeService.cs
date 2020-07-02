using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.RongCloud.util;
using SSPC_One_HCP.Services.Utils;
using SSPC_One_HCP.ServicesOut.Dto;
using SSPC_One_HCP.ServicesOut.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SSPC_One_HCP.ServicesOut.Implementations
{
    public class MeetInfoSubscribeService : IMeetInfoSubscribeService
    {
        private readonly IEfRepository _rep;
        public MeetInfoSubscribeService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取BU
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetBu(MeetInfoSubscribeDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string loginSecretkey = ConfigurationManager.AppSettings["ThirdPartyKey"];
            string signature = RongHttpClient.GetHash(loginSecretkey + dto.Nonce + dto.SignTimestamp);
            if (signature.ToUpper() != dto.Signature.ToUpper())
            {
                var massage = string.Format("Error:input={0}==>service={1},{2}", dto.Signature, signature, RongJsonUtil.ObjToJsonString(dto));
                LoggerHelper.Error(massage);
                rvm.Success = false;
                rvm.Msg = massage;
                return rvm;
            }

            var pros = _rep.Table<ProductInfo>();
            var depts = _rep.Table<DepartmentInfo>();
            var list = from a in _rep.Table<BuProDeptRel>()
                       group a by a.BuName
                into g1
                       select new
                       {
                           BuName = g1.Key,
                           DeptPro = from b in g1
                                     join c in pros on b.ProId equals c.Id
                                     where b.BuName == g1.Key
                                     group b by c into g2
                                     select new
                                     {
                                         ProId = g2.Key.Id,
                                         ProName = g2.Key.ProductName,
                                         ProUrl = g2.Key.ProductUrl,
                                         Depts = from d in g2
                                                 join e in depts on d.DeptId equals e.Id
                                                 select new
                                                 {
                                                     e.DepartmentName,
                                                     e.DepartmentType,
                                                     e.Id
                                                 }
                                     }

                       };
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list
            };
            return rvm;
        }
   
        /// <summary>
        /// 新增会议信息
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        public ReturnValueModel AddMeetInfo(MeetInfoSubscribeDto dto, string body)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var meetInfoView = RongJsonUtil.JsonStringToObj<MeetInfoViewDtoModel>(body);

            string loginSecretkey = ConfigurationManager.AppSettings["ThirdPartyKey"];
            string signature = RongHttpClient.GetHash(loginSecretkey + dto.Nonce + dto.SignTimestamp);
            if (signature.ToUpper() != dto.Signature.ToUpper())
            {
                var massage = string.Format("Error:input={0}==>service={1},{2}", dto.Signature, signature, RongJsonUtil.ObjToJsonString(dto));
                LoggerHelper.Error(massage);
                rvm.Success = false;
                rvm.Msg = massage;
                return rvm;
            }
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

            // var approvalEnabled = false; //是否启用审核

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    if (meet == null)
                    {
                        meetInfoView.Meet.Id = Guid.NewGuid().ToString();
                        meetInfoView.Meet.CreateTime = DateTime.Now;
                        meetInfoView.Meet.CreateUser = "00000000-0000-0000-0000-000000000000";
                        meetInfoView.Meet.IsCompleted = EnumComplete.Approved;
                        meetInfoView.Meet.Remark = "第三方调用";
                    }

                    meet = meetInfoView.Meet;

                    _rep.Insert(meet);
                    _rep.SaveChanges();
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
                                CreateUser = "00000000-0000-0000-0000-000000000000",
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
                            CreateUser = "00000000-0000-0000-0000-000000000000",
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
                                CreateUser = "00000000-0000-0000-0000-000000000000",
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
                            item.CreateUser = "00000000-0000-0000-0000-000000000000";
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
                            item.CreateUser = "00000000-0000-0000-0000-000000000000";
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
                            item.CreateUser = "00000000-0000-0000-0000-000000000000";
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
                                    val.Speaker.CreateUser = "00000000-0000-0000-0000-000000000000";
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

                                Schedule.CreateTime = DateTime.Now;
                                Schedule.CreateUser = "00000000-0000-0000-0000-000000000000";
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
                                    meetAndProAndDep.CreateUser = "00000000-0000-0000-0000-000000000000";
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
                            meetTag.CreateUser = "00000000-0000-0000-0000-000000000000";
                            meetTag.Remark = "第三方调用";
                            _rep.Insert(meetTag);
                        }
                        _rep.SaveChanges();
                    }
                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = new
                    {
                        meet = meet ?? meetInfoView.Meet
                    };
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
        string GetQRImgUrl(string Id, int type = 1)
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
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetListOfQA(MeetInfoSubscribeDto dto, string body)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var rowNum = RongJsonUtil.JsonStringToObj<RowNumModel<MeetSearchViewModel>>(body);

            string loginSecretkey = ConfigurationManager.AppSettings["ThirdPartyKey"];
            string signature = RongHttpClient.GetHash(loginSecretkey + dto.Nonce + dto.SignTimestamp);
            if (signature.ToUpper() != dto.Signature.ToUpper())
            {
                var massage = string.Format("Error:input={0}==>service={1},{2}", dto.Signature, signature, RongJsonUtil.ObjToJsonString(dto));
                LoggerHelper.Error(massage);
                rvm.Success = false;
                rvm.Msg = massage;
                return rvm;
            }
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
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetQuestionList(MeetInfoSubscribeDto dto, string body)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var rowNum = RongJsonUtil.JsonStringToObj<RowNumModel<QuestionModel>>(body);

            string loginSecretkey = ConfigurationManager.AppSettings["ThirdPartyKey"];
            string signature = RongHttpClient.GetHash(loginSecretkey + dto.Nonce + dto.SignTimestamp);
            if (signature.ToUpper() != dto.Signature.ToUpper())
            {
                var massage = string.Format("Error:input={0}==>service={1},{2}", dto.Signature, signature, RongJsonUtil.ObjToJsonString(dto));
                LoggerHelper.Error(massage);
                rvm.Success = false;
                rvm.Msg = massage;
                return rvm;
            }

            var list = _rep.Where<QuestionModel>(s => s.IsDeleted != 1 && string.IsNullOrEmpty(s.MeetId)).Where(rowNum.SearchParams);

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }
        /// <summary>
        /// 从其它会议导入题目或者从题库中选择题目
        /// </summary>
        /// <param name="meetQAs"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateMeetQA(MeetInfoSubscribeDto dto, string body)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var meetQAs = RongJsonUtil.JsonStringToObj<MeetQARelationViewModel>(body);

            string loginSecretkey = ConfigurationManager.AppSettings["ThirdPartyKey"];
            string signature = RongHttpClient.GetHash(loginSecretkey + dto.Nonce + dto.SignTimestamp);
            if (signature.ToUpper() != dto.Signature.ToUpper())
            {
                var massage = string.Format("Error:input={0}==>service={1},{2}", dto.Signature, signature, RongJsonUtil.ObjToJsonString(dto));
                LoggerHelper.Error(massage);
                rvm.Success = false;
                rvm.Msg = massage;
                return rvm;
            }
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

                            var meetQA = new MeetQAModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                MeetId = meetQAs.MeetId,
                                QAType = meetQAs.QAType,
                                QuestionId = question.Id,
                                CreateTime = DateTime.Now,
                                CreateUser = "00000000-0000-0000-0000-000000000000"
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


    }
}
