using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxHomeService : IWxHomeService
    {
        private readonly IEfRepository _rep;

        public WxHomeService(IEfRepository rep)
        {
            _rep = rep;
        }
        public class MeetCount
        {
            public int count { get; set; }

            public int day { get; set; }
        }
        /// <summary>
        /// 参加会议次数
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ReturnValueModel GetAttendMeetings(WorkUser workUser, DateTime? dateTime = null)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            //LoggerHelper.WriteLogInfo("[GetAttendMeetings]:**********参加会议次数开始************");
            //LoggerHelper.WriteLogInfo("[GetAttendMeetings]: UnionID:" + workUser?.WxUser?.UnionId ?? "空的！！！");
            DateTime dtNow = DateTime.Now;
            int year = dateTime?.Year ?? 0;
            int month = dateTime?.Month ?? 0;
            if (dateTime == null)
            {
                year = dtNow.Year;
                month = dtNow.Month;
            }
            ReturnValueModel rvm = new ReturnValueModel();
            //修改时间：2019-03-21 修改人：ywk
            //修改内容：参会次数=线上会议（参加报名）+线下会议（打卡签到）
            var wxuserid = workUser?.WxUser?.Id ?? "";
            #region  线下会议（打卡签到）

            var signList = from a in _rep.Table<MeetSignUp>().Where(s => s.IsDeleted != 1 && s.IsSignIn != null)
                           join c in _rep.Where<WxUserModel>(s => s.IsDeleted != 1)
                               on a.SignUpUserId equals c.Id
                           where (a.CreateTime.Value.Year == year && a.CreateTime.Value.Month == month && c.Id == wxuserid)
                           group a by new
                           {

                               time = new { a.CreateTime.Value.Year, a.CreateTime.Value.Month, a.CreateTime.Value.Day }
                           }
                into g
                           orderby g.Key.time.Day
                           select new
                           {
                               count = g.Select(s => s.MeetId).Distinct().Count(),
                               day = g.Key.time.Day
                           };
            #endregion

            #region 线上会议（参加报名）
            var meetorderlist = from a in _rep.Table<MyMeetOrder>().Where(s=>s.IsDeleted!=1)
                                
                                join c in _rep.Where<WxUserModel>(s => s.IsDeleted != 1) on a.CreateUser equals c.Id
                                where (a.CreateTime.Value.Year == year && a.CreateTime.Value.Month == month && c.Id == wxuserid)
                                group a by new
                                {

                                    time = new { a.CreateTime.Value.Year, a.CreateTime.Value.Month, a.CreateTime.Value.Day }
                                }
                into g
                                orderby g.Key.time.Day
                                select new
                                {
                                    count = g.Select(s => s.MeetId).Distinct().Count(),
                                    day = g.Key.time.Day
                                };

            #endregion

 
            var list = new List<MeetCount>();

            int days = DateTime.DaysInMonth(year, month);//获取出入月份的天数

            for (int i = 1; i <= days; i++)
            {
                var signtemp = signList.Where(s => s.day == i);
                var meetordertemp = meetorderlist.Where(s => s.day == i);
                var visitcount = (signtemp.FirstOrDefault()?.count ?? 0) + (meetordertemp.FirstOrDefault()?.count ?? 0);
                list.Add(new MeetCount
                {
                    count = visitcount,
                    day = i
                });
            }
            rvm.Result = new
            {
                list = list.OrderBy(s => s.day)
            };
            rvm.Success = true;
            rvm.Msg = "";
            //LoggerHelper.WriteLogInfo("[GetAttendMeetings]: UnionID:" + workUser?.WxUser?.UnionId ?? "空的！！！");
            //LoggerHelper.WriteLogInfo("[GetAttendMeetings]:**********参加会议次数结束************");
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }
        /// <summary>
        /// 学习时间
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ReturnValueModel GetLeaningTimes(WorkUser workUser, DateTime? dateTime = null)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间

            DateTime dtNow = DateTime.Now;
            int year = dateTime?.Year ?? 0;
            int month = dateTime?.Month ?? 0;
            if (dateTime == null)
            {
                year = dtNow.Year;
                month = dtNow.Month;
            }
            ReturnValueModel rvm = new ReturnValueModel();
            var recordList = from t in _rep.Table<MyLRecord>()
                             where (t.UnionId == workUser.WxUser.UnionId &&
                                    t.LDate.Year == year && t.LDate.Month == month

                                    )
                             group t by new
                             {
                                 time = new { t.LDate.Year, t.LDate.Month, t.LDate.Day }

                             } into g
                             orderby g.Key.time.Day
                             select new
                             {
                                 //hours = (int)Math.Ceiling((double)g.Sum(s=>s.LObjectDate)/3600),
                                 hours = Math.Round(((double)g.Sum(s => s.LObjectDate) / 3600), 2),
                                 day = g.Key.time.Day
                             };
            var list = recordList.ToList();
            LoggerHelper.WriteLogInfo("******");
            LoggerHelper.WriteLogInfo("[GetLeaningTimes]开始：");
            LoggerHelper.WriteLogInfo(recordList.ToString());
            int days = DateTime.DaysInMonth(year, month);//获取出入月份的天数
            for (int i = 1; i <= days; i++)
            {
                var temp = recordList.Where(s => s.day == i);
                if (temp.FirstOrDefault() == null)
                {
                    list.Add(new { hours = 0.00, day = i });
                }
            }
            LoggerHelper.WriteLogInfo("[GetLeaningTimes]结束：");
            rvm.Result = new
            {
                list = list.OrderBy(s => s.day)
            };
            rvm.Success = true;
            rvm.Msg = "";
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        /// <summary>
        /// 访问城市
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ReturnValueModel GetVisitCitys(WorkUser workUser, DateTime? dateTime = null)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间

            DateTime dtNow = DateTime.Now;
            int year = dateTime?.Year ?? 0;
            int month = dateTime?.Month ?? 0;
            if (dateTime == null)
            {
                year = dtNow.Year;
                month = dtNow.Month;
            }
            ReturnValueModel rvm = new ReturnValueModel();


            var recordList = from t in _rep.Table<MeetSignUp>().Where(a => a.SignUpUserId == workUser.WxUser.Id && a.IsSignIn == 1)
                             join m in _rep.Table<MeetInfo>() on t.MeetId equals m.Id
                             where (

                                 t.CreateTime.Value.Year == year && t.CreateTime.Value.Month == month && m.MeetType == 3 && m.MeetCity != null)

                             group t by new
                             {
                                 city = m.MeetCity

                             } into g
                             orderby g.Key.city
                             select new
                             {
                                 name = "",
                                 value = g.Key.city,
                                 yAxis = g.Select(s => s.MeetId).Distinct().Count(),
                             };

            rvm.Result = new
            {
                list = recordList.ToList().OrderBy(p => p.value).Select((p, idx) => new
                {
                    xAxis = idx,
                    name = p.name,
                    value = (p.value.IndexOf(',')>-1? p.value.Split(',')[1] : p.value),
                    yAxis = p.yAxis
                })
            };
            rvm.Success = true;
            rvm.Msg = "";
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        /// <summary>
        /// 增加意见反馈
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ReturnValueModel AddFeedback(FeedbackViewModel viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var feedback = new Feedback();
            feedback.Id = Guid.NewGuid().ToString();
            feedback.Content = viewModel.Content;
            feedback.CreateUser = workUser.WxUser.Id;
            feedback.CreateTime = DateTime.Now;

            _rep.Insert(feedback);
            _rep.SaveChanges();

            rvm.Success = true;
            return rvm;
        }

        /// <summary>
        /// 删除个人信息
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel DeleteMyAccount(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == workUser.WxUser.Id);
           
            if (wxUser == null)
            {
                rvm.Success = true;
                rvm.Msg = "User not found.";
            }

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    wxUser.IsDeleted = 1;
                    _rep.Update(wxUser);

                    var MyMeetOrderList = _rep.Where<MyMeetOrder>(s => s.UnionId == wxUser.UnionId);
                    foreach(var item in MyMeetOrderList)
                    {
                        item.UnionId = null;  //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                        item.WxUserId = wxUser.Id; //记录用户ID，以后统计时能找到和用户的关系
                    }
                    _rep.UpdateList(MyMeetOrderList);

                    var MyLRecordList = _rep.Where<MyLRecord>(s => s.UnionId == wxUser.UnionId);
                    foreach (var item in MyLRecordList)
                    {
                        item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                        item.WxUserId = wxUser.Id; //记录用户ID，以后统计时能找到和用户的关系
                    }
                    _rep.UpdateList(MyLRecordList);

                    var MyCollectionInfoList = _rep.Where<MyCollectionInfo>(s => s.UnionId == wxUser.UnionId);
                    foreach (var item in MyCollectionInfoList)
                    {
                        item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                        item.WxUserId = wxUser.Id; //记录用户ID，以后统计时能找到和用户的关系
                    }
                    _rep.UpdateList(MyCollectionInfoList);

                    var MyReadRecordList = _rep.Where<MyReadRecord>(s => s.UnionId == wxUser.UnionId);
                    foreach (var item in MyReadRecordList)
                    {
                        item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                        item.WxUserId = wxUser.Id; //记录用户ID，以后统计时能找到和用户的关系
                    }
                    _rep.UpdateList(MyReadRecordList);

                    var RegisterModelList = _rep.Where<RegisterModel>(s => s.UnionId == wxUser.UnionId);
                    foreach (var item in RegisterModelList)
                    {
                        item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                        item.WxUserId = wxUser.Id; //记录用户ID，以后统计时能找到和用户的关系
                    }
                    _rep.UpdateList(RegisterModelList);

                    _rep.SaveChanges();

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
        /// 费森竞争产品
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetCompetingProduct()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            var result = new ReturnValueModel() { Success = true, Msg = "" };
            var list = _rep.Where<CompetingProduct>(x => true);
            result.Result = new { list };
            stopwatch.Stop();//结束
            result.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return result;
        }
    }
}
