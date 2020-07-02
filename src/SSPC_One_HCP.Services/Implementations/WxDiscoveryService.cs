using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxDiscoveryService : IWxDiscoveryService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        public WxDiscoveryService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取微信发现页面数据
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel WxDisMainPage(WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();

            //LoggerHelper.WarnInTimeTest("***********************");
            //LoggerHelper.WarnInTimeTest("Inner-[WxDisMainPage] Start：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            //LoggerHelper.WriteLogInfo("****************[WxDisMainPage]获取微信发现页面数据 开始**********************");

            WxUserModel wxUser = null;
            //LoggerHelper.WriteLogInfo("[WxDisMainPage] workUser.WxUser.UnionId:" + (workUser?.WxUser?.UnionId ?? "空的！！！"));
            if (!string.IsNullOrEmpty(workUser?.WxUser?.UnionId))
            {
                wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == workUser.WxUser.UnionId);
            }
            //LoggerHelper.WriteLogInfo("[WxDisMainPage] unionID:" + (wxUser?.UnionId ?? "空的！！！"));
            //TODO激励语
            var motivational = "路漫漫其修远兮，吾将上下而求索";

            if (workUser != null)
            {
                motivational = ReturnInspire(workUser);
            }

            LoggerHelper.WriteLogInfo("[WxDisMainPage]：准备获取页面数据");
            if (wxUser != null && wxUser.IsCompleteRegister == 1)
            {

                ////推荐会议主题
                //var meets = _rep.Where<MeetInfo>(s => s.MeetDep == wxUser.DepartmentName)
                //    .OrderByDescending(o => o.MeetStartTime).Take(2);

                LoggerHelper.WriteLogInfo("[WxDisMainPage]：获取页面数据");
                DateTime dt = DateTime.Now;  //当前时间
                DateTime startWeek = Convert.ToDateTime(dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).ToString("yyyy-MM-dd") + " 00:00:00");  //本周周一
                DateTime endWeek = Convert.ToDateTime(startWeek.AddDays(6).ToString("yyyy-MM-dd") + " 23:59:59");  //本周周日
                DateTime startMonth = Convert.ToDateTime(dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd") + " 00:00:00");  //本月月初
                DateTime endMonth = Convert.ToDateTime(startMonth.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");  //本月月末//
                var lastStartWeek = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).ToString("yyyy-MM- dd") + " 00:00:00");        //上周一
                var lastEndWeek = Convert.ToDateTime(DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).AddDays(6).ToString("yyyy-MM-dd") + " 23:59:59");//上周日


                //当前年月
                var currentYM = DateTime.Now.Year + "年" + DateTime.Now.Month + "月";

                //var recordList = _rep.Table<MyLRecord>().Where(s => s.UnionId == workUser.WxUser.UnionId).ToList();
                var recordList = _rep.Table<MyLRecord>().Where(s => s.WxUserId == workUser.WxUser.Id && s.IsDeleted != 1).ToList();

                //本周学习时间
                var thisLearnHours = getLearningHours(recordList, startWeek, endWeek);
                var lastLearnHours = getLearningHours(recordList, lastStartWeek, lastEndWeek);
                string learnDiffString = "增加";
                if (thisLearnHours - lastLearnHours < 0)
                {
                    learnDiffString = "减少";
                }

                //上周学习时间
                var learnCompare = "比上周" + learnDiffString + System.Math.Abs(thisLearnHours - lastLearnHours) + "小时";

                //本周参会次数 线上学习  线下签到
                var meetsignlist = (from a in _rep.Table<MyMeetOrder>()
                        .Where(s => s.IsDeleted == 0 && s.CreateUser == workUser.WxUser.Id)
                                    select new MeetList
                                    {
                                        LDate = a.CreateTime
                                    }).Union(from a in _rep.Table<MeetSignUp>()
                                        .Where(s => s.IsDeleted == 0 && s.SignUpUserId == workUser.WxUser.Id)
                                             select new MeetList
                                             {
                                                 LDate = a.SignInTime
                                             }).ToList();


                var thisMeetingTimes = getMeetingTimes(meetsignlist, startWeek, endWeek);
                var lastMeetingTimes = getMeetingTimes(meetsignlist, lastStartWeek, lastEndWeek);
                string meetingDiffString = "增加";
                if (thisMeetingTimes - lastMeetingTimes < 0)
                {
                    meetingDiffString = "减少";
                }

                //上周参会次数
                var meetCompare = "比上周" + meetingDiffString + System.Math.Abs(thisMeetingTimes - lastMeetingTimes) + "次";


                //TODO百分比 右连接
                //var recordAllList = from t in _rep.Table<MyLRecord>().Where(t => t.LDate.Year == DateTime.Now.Year && t.LDate.Month == DateTime.Now.Month)
                //                    join f in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && s.HospitalName != null)
                //                        on t.WxUserId equals f.Id

                //                   into joinRecord
                //                    from t3 in joinRecord.DefaultIfEmpty()
                //                    group t by new
                //                    {
                //                        //unionid = t.UnionId
                //                        wxuserid = t.WxUserId

                //                    } into g

                //                    select new
                //                    {
                //                        hours = g.Sum(s => s.LObjectDate),
                //                        //unionid = g.Key.unionid
                //                        wxuserid = g.Key.wxuserid
                //                    };

                //var recordlist = from a in _rep.Table<MyLRecord>().Where(t =>
                //    t.LDate.Year == DateTime.Now.Year && t.LDate.Month == DateTime.Now.Month) select  a;
                //var recordAllList = from t in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1)

                //    join f in _rep.Table<MyLRecord>()
                //        on t.Id equals f.WxUserId into joinRecord
                //    from t3 in joinRecord.DefaultIfEmpty()
                //    select new
                //    {
                //        wxuserid = t3.WxUserId,
                //        LObjectDate = t3.LObjectDate==null?0 : t3.LObjectDate
                //    };
                //var    recordAllList2 = from a in recordAllList
                //    group a by a.wxuserid
                //    into temp
                //    select new
                //    {
                //        hours = temp.Sum(s => s.LObjectDate),
                //        wxuserid = temp.Key 
                //    };


                #region  个人排名百分比

              
                var list = (from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1)
                            join b in _rep.Where<MyLRecord>(s => s.IsDeleted != 1) on a.Id equals b.WxUserId into JoinedModel
                            from b in JoinedModel.DefaultIfEmpty()
                            group new { b } by a
               into all
                            select new  
                            {
                                Id = all.Key.Id, 
                                //产品资料
                                DocLearnTime = all.Where(s => s.b.LObjectType == 1 || s.b.LObjectType == 2 || s.b.LObjectType == 4).Sum(s => s.b.LObjectDate).HasValue
                                    ? all.Where(s => s.b.LObjectType == 1 || s.b.LObjectType == 2 || s.b.LObjectType == 4).Sum(s => s.b.LObjectDate) : 0, 
                            }) ;
                list = list.OrderByDescending(s => s.DocLearnTime);

                #endregion
                var thisRecord = list.FirstOrDefault(s => s.Id == workUser.WxUser.Id);

                int recordAllCount = list.Select(s => s.Id).Distinct().Count();
                int myIndex = recordAllCount;
              

                if (thisRecord != null)
                {
                    myIndex = list.Where(s => s.Id != null).OrderByDescending(s => s.DocLearnTime).ToList().IndexOf(thisRecord) + 1;
                    LoggerHelper.WarnInTimeTest("[recordAllList] 我的排名:" + myIndex);
                }




                LoggerHelper.WarnInTimeTest("[recordAllList] 总人数:" + recordAllCount);
                decimal learnPercent = 0;
                if (recordAllCount > 0)
                {
                    //learnPercent = (int)Math.Ceiling((double)recordAllList.Count() - myIndex / recordAllList.Count()) * 100;
                    //学习记录百分比 
                    learnPercent = Math.Round(((decimal)((recordAllCount - myIndex)) / recordAllCount), 3) * 100;
                   
                    LoggerHelper.WriteLogInfo("[WxDisMainPage] 学习记录 recordAllCount：" + recordAllCount + ",myIndex:" + myIndex);
                }



                //判断注册时间
                var isNewRegister = 1; //1 注册未满1周 
                if (!IsNewRegister(workUser.WxUser.creation_time))
                {
                    isNewRegister = 2; //2 注册已满1周 
                }

                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    //meets = meets,
                    isNewRegister = isNewRegister,
                    currentYM = currentYM,
                    motivational = motivational,
                    thisLearnHours = thisLearnHours,
                    learnCompare = learnCompare,
                    meetCompare = meetCompare,
                    thisMeetingTimes = thisMeetingTimes,
                    learnPercent = learnPercent
                };
            }
            else
            {
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    isNewRegister = 0,  //未注册
                    motivational = motivational
                };
            }
            //LoggerHelper.WriteLogInfo("[WxDisMainPage]：接口是否成功：" + ((rvm.Success == true) ? "success" : "false"));
            //LoggerHelper.WriteLogInfo("****************获取微信发现页面数据 结束**********************");
            //LoggerHelper.WarnInTimeTest("Inner-[WxDisMainPage] End：" +  DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }
        private int getMeetingTimes(List<MeetList> recordList, DateTime dtStart, DateTime dtEnd)
        {
            int meetingTimes = recordList.Where(t =>
                             t.LDate >= dtStart && t.LDate <= dtEnd
                    //  && (t.LDateEnd > t.LDate && t.LDateStart <= t.LDate) 
                    //&& t.LObjectType == 5).Select(s => s.LObjectId).Distinct().Count();
                    ).Distinct().Count();
            return meetingTimes;
        }
        private int getLearningHours(List<MyLRecord> recordList, DateTime dtStart, DateTime dtEnd)
        {
            var weekLearn = (double)recordList.Where(t =>
                            t.LDate >= dtStart && t.LDate <= dtEnd
                            ).Sum(s => s.LObjectDate);
            var learnHours = (int)Math.Ceiling(weekLearn / 3600);
            return learnHours;
        }
        private bool IsNewRegister(DateTime? dt)
        {
            if (dt == null) return false;
            DateTime dt1 = Convert.ToDateTime(dt);
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);

            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);

            TimeSpan ts = ts1.Subtract(ts2).Duration();

            if (ts.Days < 7)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回学习或为学习的天数
        /// (1)是否学过
        /// (2)当天学了 找到最近的学习时间段
        /// (3)当天没学 找到最近的学习时间  算出时间段
        /// (4)没学天数超过一个月  考虑会议的情况
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        private int StudyTimes(WorkUser workUser)
        {
            var recordList = (from a in _rep.Table<MyLRecord>().Where(s => s.WxUserId == workUser.WxUser.Id)
                              select new
                              {
                                  StudyDate = a.CreateTime.Value.Year + "-" + a.CreateTime.Value.Month + "-" + a.CreateTime.Value.Day

                              }).Distinct().OrderByDescending(s => s.StudyDate);
            LoggerHelper.WriteLogInfo("[StudyTimes]:生成sql:" + recordList.ToString());

            var NowDate = DateTime.Now;
            var YesterDate = NowDate.AddDays(-1);
            var NowDateStr = GetDate(NowDate.ToString("yyyy-MM-dd"));

            //今天是否学习过
            var temp = recordList.Where(s => s.StudyDate == NowDateStr).AsQueryable().Count();

            var DaysCount = (temp > 0) ? 1 : -1;

            //最后的学习时间
            string laststudytime = string.Empty;

            //最后的会议时间
            DateTime lastmeetdate = Convert.ToDateTime("2000-01-01");


            bool IsStudy = true;
            //递归 昨天 叠加学习或者未学习天数
            while (IsStudy)
            {

                var tempDateStr = GetDate(YesterDate.ToString("yyyy-MM-dd"));
                //前一天学习天数
                var tempmodel = recordList.Where(s => s.StudyDate == tempDateStr).AsQueryable().Count();

                //当天已学习  前天也学习
                if (temp > 0 && tempmodel > 0)
                {
                    DaysCount++;
                    if (DaysCount >= 365) break;


                    YesterDate = YesterDate.AddDays(-1);
                }
                //当天已学习  前天未学习
                else if (temp > 0 && tempmodel == 0)
                {
                    IsStudy = false;
                }
                //当天未学习  前天也没学
                else if (temp == 0 && tempmodel == 0)
                {
                    DaysCount--;
                    if (DaysCount <= -365) break;
                    YesterDate = YesterDate.AddDays(-1);
                }
                //当天未学习  前天学习了
                else if (temp == 0 && tempmodel > 0)
                {
                    laststudytime = tempDateStr;
                    IsStudy = false;
                }
                else
                {
                    IsStudy = false;
                }
            }

            #region 会议天数功能

            if (temp < 0)
            {
                //近期是否参加过会议  （1）线上会议 报名即可 （2）线下会议 扫码签到

                var onlinemeetlist =
                    (from a in _rep.Table<MyMeetOrder>().Where(s => s.WxUserId == workUser.WxUser.Id)
                     join b in _rep.Table<MeetInfo>() on a.MeetId equals b.Id
                     select new
                     {
                         joinDate = a.CreateTime
                     }
                    ).ToList();
                DateTime onlinedate = Convert.ToDateTime("2000-01-01");
                if (onlinemeetlist.OrderByDescending(s => s.joinDate).ToList().Count > 0)
                {
                    onlinedate = Convert.ToDateTime(onlinemeetlist.OrderByDescending(s => s.joinDate).FirstOrDefault()?.joinDate);
                }


                var offlinemeetlist = (from a in _rep.Table<MeetSignUp>()
                                       join b in _rep.Table<MeetInfo>()
                                           on a.MeetId equals b.Id
                                       join c in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && s.Id == workUser.WxUser.Id)
                                           on a.SignUpUserId equals c.Id
                                       select new
                                       {
                                           joinDate = a.CreateTime
                                       }
                    ).ToList();

                DateTime offlinedate = Convert.ToDateTime("2000-01-01");
                if (offlinemeetlist.OrderByDescending(s => s.joinDate).ToList().Count > 0)
                {
                    offlinedate = Convert.ToDateTime(offlinemeetlist.OrderByDescending(s => s.joinDate).FirstOrDefault()?.joinDate);
                }

                lastmeetdate = (DateTime.Compare(onlinedate, offlinedate) > 0) ? onlinedate : offlinedate;



                //参加过  如果最后参加会议天数和最后学习天数在一个年月

                DateTime laststudytimeDT = Convert.ToDateTime(laststudytime);
                if (lastmeetdate.Year == laststudytimeDT.Year && lastmeetdate.Month == laststudytimeDT.Month)
                {
                    DaysCount = DaysCount - (DateTime.DaysInMonth(lastmeetdate.Year, lastmeetdate.Month) -
                                             laststudytimeDT.Day);
                }
                //最后一天学习 如果和当前年月相同 则返回0 ;如果不同 重新计算
                else if (laststudytimeDT.Year == NowDate.Year && laststudytimeDT.Month == NowDate.Month)
                {
                    DaysCount = 0;
                }
                else if (laststudytimeDT.Year != NowDate.Year && laststudytimeDT.Month != NowDate.Month)
                {
                    DaysCount = DaysCount - (DateTime.DaysInMonth(laststudytimeDT.Year, laststudytimeDT.Month) -
                                             NowDate.Day);
                }

            }




            #endregion



            return DaysCount;

        }

        /// <summary>
        /// 是否学习过  以前从未学习 且是今天
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        private bool IsStudy(WorkUser workUser)
        {
            //学习时间
            var recordList = (from a in _rep.Table<MyLRecord>().Where(s => s.WxUserId == workUser.WxUser.Id) select a).Distinct().Count();

            //会议 线上和线下
            var onlinemeetlist =
                (from a in _rep.Table<MyMeetOrder>().Where(s => s.WxUserId == workUser.WxUser.Id)
                 join b in _rep.Table<MeetInfo>() on a.MeetId equals b.Id
                 select a
                ).Distinct().Count();

            var offlinemeetlist = (from a in _rep.Table<MeetSignUp>()
                                   join b in _rep.Table<MeetInfo>()
                                       on a.MeetId equals b.Id
                                   join c in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && s.Id == workUser.WxUser.Id)
                                       on a.SignUpUserId equals c.Id
                                   select a
                ).Distinct().Count();

            return (recordList == 0 && onlinemeetlist == 0 && offlinemeetlist == 0) ? false : true;
        }

        /// <summary>
        /// 是否初次注册
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        private bool IsFirstRegister(WorkUser workUser)
        {
            var model = (from a in _rep.Table<WxUserModel>().Where(s => s.Id == workUser.WxUser.Id) select a).FirstOrDefault();
            bool IsFirst = false;
            if (model != null)
            {
                string upDateTimeStr = Convert.ToDateTime(model.CreateTime).ToString("yyyyMMdd");
                //如果更新时间在当天
                if (upDateTimeStr == DateTime.Now.ToString("yyyyMMdd"))
                {
                    IsFirst = true;
                }
            }

            return IsFirst;

        }

        /// <summary>
        /// 日期处理
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        private string GetDate(string dateTimeStr)
        {
            string[] value = dateTimeStr.Split('-');
            return value[0] + "-" +
                   (Convert.ToInt32(value[1].ToString()) + "-" + (Convert.ToInt32(value[2]).ToString()));
        }

        /// <summary>
        /// 返回激励语
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        private string ReturnInspire(WorkUser workUser)
        {
            string InspireStr = string.Empty;
            //从未注册
            if (!IsStudy(workUser) && IsFirstRegister(workUser))
            {
                InspireStr = "路漫漫其修远兮，吾将上下而求索";
            }
            else
            {
                int studyDays = StudyTimes(workUser);
                bool isStudy = (studyDays < 0 ? false : true);
                studyDays = Math.Abs(studyDays);
                if (isStudy)
                {

                    if (studyDays <= 7)
                    {
                        InspireStr = "敏而好学，不耻下问";
                    }
                    else if (studyDays <= 30)
                    {
                        InspireStr = "三人行，必有我师也";
                    }

                    else if (studyDays <= 90)
                    {
                        InspireStr = "爱学出勤奋，勤奋出天才";
                    }
                    else if (studyDays <= 180)
                    {
                        InspireStr = "读书破万卷，下笔如有神";
                    }
                    else if (studyDays >= 181)
                    {
                        InspireStr = "会当凌绝顶，一览众山小";
                    }
                }
                else
                {
                    if (studyDays <= 7)
                    {
                        InspireStr = "业精于勤，荒于嬉 ";
                    }
                    else if (studyDays <= 30)
                    {
                        InspireStr = "锲而不舍，金石可镂";
                    }

                    else if (studyDays <= 90)
                    {
                        InspireStr = "黑发不知勤学早，白发方悔读书迟";
                    }
                    else if (studyDays <= 180)
                    {
                        InspireStr = "人生在勤，不索何获";
                    }
                    else if (studyDays >= 181)
                    {
                        InspireStr = "欲穷千里目，更上一层楼";
                    }

                }
            }

            return InspireStr;
        }

        public class MeetList
        {
            public DateTime? LDate { get; set; }
        }
    }
}
