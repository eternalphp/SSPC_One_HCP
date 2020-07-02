using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class StatisticService : IStatisticService
    {
        private readonly IEfRepository _rep;
        private readonly IConfig _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep"></param>
        public StatisticService(IEfRepository rep, IConfig config)
        {
            _rep = rep;
            _config = config;
        }
        /// <summary>
        /// 概览
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetOverViewList(RowNumModel<StatisticsTimeViewModel> model)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var doctorList = _rep.Where<WxUserModel>(x => (x.IsSalesPerson ?? 0) != 1);
            //总 授权人数 去重
            var hasAuthNumber = doctorList.Where(x => !string.IsNullOrEmpty(x.UnionId)).Select(x => x.UnionId).Distinct().Count();
            //总访问人数 
            var allVisitNumber = hasAuthNumber;
            //注册人数 去重
            var regNumber = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1).Select(x => x.UnionId).Count();
            
            //认证通过人数 
            var verifyNumberA = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && x.IsVerify == 1).Select(x => x.UnionId).Count();
            //认证未通过人数（失败 和申诉拒绝）
            var verifyNumberB = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && (x.IsVerify == 3 || x.IsVerify == 6)).Select(x => x.UnionId).Count();
            //认证 未定人数 （2 不确定）
            var verifyNumberC = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && x.IsVerify == 2).Select(x => x.UnionId).Count();
            //总待验证通过人数( 4申诉中 或 5认证中 )
            var verifyNumberD = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && (x.IsVerify == 5 || x.IsVerify == 4)).Select(x => x.UnionId).Count();
            rvm.Success = true;
            rvm.Msg = "success！";
            rvm.Result = new
            {
                rows = new
                {
                    allvisitcount = allVisitNumber,
                    hasauthListcount = hasAuthNumber,
                    hasregistercount = regNumber,
                    hascheckcount = verifyNumberA,
                    notpasscount = verifyNumberB,
                    notsurecount = verifyNumberC,
                    waitcheckcount = verifyNumberD
                }
            };

            return rvm;
        }

        /// <summary>
        /// 医生学习趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetDocStudyList(RowNumModel<StatisticsTimeViewModel> model)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            #region 搜索时间初始化
            DateTime nowdate = DateTime.Now;
            DateTime begin_date = new DateTime(nowdate.Year, nowdate.Month, 1);
            DateTime end_date = begin_date.AddMonths(1).AddDays(-1);
            if (model.SearchParams.begin_date != begin_date)
            {
                begin_date = Convert.ToDateTime(model.SearchParams.begin_date.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if (model.SearchParams.end_date != end_date)
            {
                end_date = Convert.ToDateTime(model.SearchParams.end_date.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            #endregion

            try
            {
                #region 最受欢迎学习资料前15 

                var meet = from a in _rep.Table<MyLRecord>().Where(s =>
                        s.IsDeleted != 1 && s.LDate >= begin_date && s.LDate <= end_date)
                           select a;
                int StudyType = Convert.ToInt32(model?.SearchParams?.StudyType ?? "0");

                //会议或者播客
                if (StudyType == 3 || StudyType == 5)
                {

                    meet = meet.Where(s => s.LObjectType == StudyType);
                }
                else
                {
                    meet = meet.Where(s => s.LObjectType != 3 && s.LObjectType != 5);
                }

                if (StudyType == 5)
                {
                }

                var meetlist = from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1)
                               join b in meet
                                   on a.Id equals b.LObjectId
                               select new
                               {
                                   MeetTile = a.MeetTitle,
                                   MeetId = a.Id
                               };


                var meetfavlist = (from a in meetlist
                                   group a by a.MeetId into g
                                   select new
                                   {
                                       Title = meetlist.Where(s => s.MeetId == g.Key).FirstOrDefault().MeetTile,
                                       Count = meetlist.Where(s => s.MeetId == g.Key).Count()
                                   }).OrderByDescending(s => s.Count).Take(15).ToList();

                var datalist = from a in _rep.Table<DataInfo>().Where(s => s.IsDeleted != 1)
                               join b in meet
                                   on a.Id equals b.LObjectId
                               select new
                               {
                                   MeetTile = a.Title,
                                   MeetId = a.Id
                               };
                var datafavlist = (from a in datalist
                                   group a by a.MeetId into g
                                   select new
                                   {
                                       Title = datalist.Where(s => s.MeetId == g.Key).FirstOrDefault().MeetTile,
                                       Count = datalist.Where(s => s.MeetId == g.Key).Count()
                                   }).OrderByDescending(s => s.Count).Take(50).ToList();

                #endregion

                #region  最活跃城市  根据会议连接

                var citylist = from a in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1 && s.MeetCity != null)
                               join b in _rep.Table<MeetSignUp>().Where(s => s.IsDeleted != 1 && s.SignInTime >= begin_date && s.SignInTime <= end_date) on a.Id equals b.MeetId
                               select new
                               {
                                   City = a.MeetCity
                               };
                var cityacvlist = (from a in citylist
                                   group a by a.City
                                   into g
                                   select new
                                   {
                                       City = g.Key,
                                       Count = citylist.Where(s => s.City == g.Key).Count()
                                   }).OrderByDescending(s => s.Count).Take(50).ToList();
                #endregion

                #region 最活跃职称
                var poslist = from a in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && !string.IsNullOrEmpty(s.job_title))
                              join b in _rep.Table<MyLRecord>().Where(s =>
                                      s.IsDeleted != 1 && s.LDateStart >= begin_date && s.LDateEnd <= end_date)
                                  on a.Id equals b.WxUserId
                              select new
                              {
                                  position = a.job_title
                              };

                var posfavlist = (from a in poslist
                                  group a by a.position into g
                                  select new
                                  {
                                      Position = g.Key,
                                      Count = poslist.Where(s => s.position == g.Key).Count()
                                  }).OrderByDescending(s => s.Count).Take(50).ToList();


                #endregion

                #region 最活跃医院
                var hosplist = from a in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && s.HospitalName != null)
                               join b in _rep.Table<MyLRecord>().Where(s =>
                                       s.IsDeleted != 1 && s.LDateStart >= begin_date && s.LDateEnd <= end_date)
                                   on a.Id equals b.WxUserId
                               select new
                               {
                                   hospitalName = a.HospitalName
                               };

                var hospfavlist = (from a in hosplist
                                   group a by a.hospitalName into g
                                   select new
                                   {
                                       hospitalName = g.Key,
                                       Count = hosplist.Where(s => s.hospitalName == g.Key).Count()
                                   }).OrderByDescending(s => s.Count).Take(50).ToList();
                #endregion

                #region 最活跃标签

                var taglist = from a in _rep.Table<MyLRecord>().Where(s => s.IsDeleted != 1 && s.LDateStart >= begin_date && s.LDateEnd <= end_date)
                              join b in _rep.Table<DocTag>().Where(s => s.IsDeleted != 1)
                                  on a.WxUserId equals b.DocId
                              join c in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1)
                                  on b.TagId equals c.Id
                              select new
                              {
                                  TagName = c.TagName
                              };
                var tagfavlist = (from a in taglist
                                  group a by a.TagName into g
                                  select new
                                  {
                                      TagName = g.Key,
                                      Count = taglist.Where(s => s.TagName == g.Key).Count()
                                  }).OrderByDescending(s => s.Count).Take(50).ToList();

                #endregion
                rvm.Success = true;
                rvm.Result = new
                {
                    rows = new
                    {
                        meetfavlist = (StudyType == 5) ? meetfavlist : datafavlist,
                        cityacvlist = cityacvlist,
                        posfavlist = posfavlist,
                        hospfavlist = hospfavlist,
                        tagfavlist = tagfavlist
                    }

                };
            }
            catch (Exception e)
            {
                rvm.Success = false;
                rvm.Msg = e.Message;
                LoggerHelper.WriteLogInfo("[医生学习趋势 GetDocStudyList Error]" + e.Message);
            }


            return rvm;
        }

        /// <summary>
        /// 增长趋势
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetGrowthList(RowNumModel<StatisticsTimeViewModel> model)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var startTime = (model?.SearchParams?.begin_date ?? DateTime.Now).AddSeconds(1.0);
            var endTime = (model?.SearchParams?.end_date ?? DateTime.Now).AddDays(1.0).AddSeconds(-1.0);

            var doctorList = _rep.Where<WxUserModel>(x => x.IsDeleted != 1 && (x.IsSalesPerson ?? 0) != 1 && (x.CreateTime ?? x.UpdateTime) >= startTime && (x.CreateTime ?? x.UpdateTime) <= endTime);

            //新增访问人数
            var visitNumber = doctorList.Count();
            //新增授权人数 
            var authNumber = doctorList.Select(x => x.UnionId).Distinct().Count();
            //新增注册人数
            var regNumber = doctorList.Where(x => x.IsCompleteRegister == 1).Select(x => x.UnionId).Distinct().Count();
            //新增通过验证人数
            var verifyNumberA = doctorList.Where(x => x.IsCompleteRegister == 1 && x.IsVerify == 1).Select(x => x.UnionId).Distinct().Count();
            //新增待验证人数
            var verifyNumberB = doctorList.Where(x => x.IsCompleteRegister == 1 && (x.IsVerify == 5 || x.IsVerify == 4)).Select(x => x.UnionId).Distinct().Count();

            var days = (endTime - startTime).Days;
            var dayList = new List<DayList>();
            for (int i = 0; i <= days; i++)
            {
                dayList.Add(new DayList() { Day = startTime.AddDays(i).ToShortDateString(), Num = 0 });

            }

            //平台生成的二维码
            var fromA = doctorList.Where(x => x.SourceType == "1" || x.SourceType == "2" || x.SourceType == "4").ToList()
                .Select(x => (x.CreateTime ?? x.UpdateTime).Value.ToShortDateString()).GroupBy(x => x)
                .Select(x => new DayList { Day = x.Key, Num = x.Count() }).ToList();
            var dayListA = dayList.Union(fromA).GroupBy(x => x.Day)
                .Select(o => new DayList { Day = o.Key, Num = o.Sum(x => x.Num), DayTime = DateTime.Parse(o.Key) }).OrderBy(x => x.DayTime);

            //费卡文库
            string fkLibAppId = _config.GetFkLibAppId();//费卡文库AppId
            var fromB = doctorList.Where(x => x.SourceType == "3" && x.SourceAppId.Equals(fkLibAppId)).ToList()
                .Select(x => (x.CreateTime ?? x.UpdateTime).Value.ToShortDateString()).GroupBy(x => x)
                .Select(x => new DayList { Day = x.Key, Num = x.Count() }).ToList();
            var dayListB = dayList.Union(fromB).GroupBy(x => x.Day)
                .Select(o => new DayList { Day = o.Key, Num = o.Sum(x => x.Num), DayTime = DateTime.Parse(o.Key) }).OrderBy(x => x.DayTime);


            //绑定小程序的公众号
            var wechatList = _rep.Where<WechatPublicAccount>(x => x.IsDeleted != 1 && !x.AppId.Equals(fkLibAppId));
            var fromC = doctorList.Where(x => x.SourceType == "3" && wechatList.Count(s => s.AppId == x.SourceAppId) > 0).ToList()
                .Select(x => (x.CreateTime ?? x.UpdateTime).Value.ToShortDateString()).GroupBy(x => x)
                .Select(x => new DayList { Day = x.Key, Num = x.Count() }).ToList();
            var dayListC = dayList.Union(fromC).GroupBy(x => x.Day)
                .Select(o => new DayList { Day = o.Key, Num = o.Sum(x => x.Num), DayTime = DateTime.Parse(o.Key) }).OrderBy(x => x.DayTime);

            //其它小程序场景
            var fromDd = doctorList.Where(x => x.SourceType == "0").ToList();
            var fromD = fromDd
                .Select(x => (x.CreateTime ?? x.UpdateTime).Value.ToShortDateString()).GroupBy(x => x)
                .Select(x => new DayList { Day = x.Key, Num = x.Count() }).ToList();
            var dayListD = dayList.Union(fromD).GroupBy(x => x.Day)
                .Select(o => new DayList { Day = o.Key, Num = o.Sum(x => x.Num), DayTime = DateTime.Parse(o.Key) }).OrderBy(x => x.DayTime);
        
            rvm.Result = new
            {
                visitcount = visitNumber,
                authcountnew = authNumber,
                registercountnew = regNumber,
                passcheckcountnew = verifyNumberA,
                waitpasscountnew = verifyNumberB,
                codelistnew = dayListA.Select(s => s.Num),
                scandoclist = dayListB.Select(s => s.Num),
                publiclistnew = dayListC.Select(s => s.Num),
                otherlistnew = dayListD.Select(s => s.Num)
            };
            rvm.Success = true;
            rvm.Msg = "";
            return rvm;
        }

        /// <summary>
        /// 活跃趋势-活跃趋势人数、Top 访问页、Top 访问模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetActiveList(RowNumModel<StatisticsTimeViewModel> model)
        {
            #region 搜索初始化
            ReturnValueModel rvm = new ReturnValueModel();
            DateTime nowdate = DateTime.Now;
            DateTime begin_date = new DateTime(nowdate.Year, nowdate.Month, 1);
            DateTime end_date = begin_date.AddMonths(1).AddDays(-1);

            if (model.SearchParams.begin_date != begin_date)
            {
                begin_date = Convert.ToDateTime(model.SearchParams.begin_date.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if (model.SearchParams.end_date != end_date)
            {
                end_date = Convert.ToDateTime(model.SearchParams.end_date.ToString("yyyy-MM-dd") + " 23:59:59");
            }

            #endregion

            var startTime = (model?.SearchParams?.begin_date ?? DateTime.Now).AddSeconds(1.0);
            var endTime = (model?.SearchParams?.end_date ?? DateTime.Now).AddDays(1.0).AddSeconds(-1.0);


            try
            {
                #region Top 访问页

                var pagelist = from a in _rep.Table<VisitModules>().Where(s => s.IsDeleted != 1 && s.VisitStart >= begin_date && s.VisitStart <= end_date&&s.UnionId!=null)
                               join b in _rep.Table<VisitModulesName>() on a.ModulePageNo equals b.ModulesNo
                               join c in _rep.Table<WxUserModel>().Where(s=>s.IsDeleted!=1) on a.WxUserid equals c.Id
                               select new
                               {
                                   ModulesName = b.ModulesName,
                                   IsVisitor = a.Isvisitor,
                                   ModuleNo = b.ModulesNo,
                                   Wxuserid = a.WxUserid,
                                   IsCompleteRegister = c.IsCompleteRegister
                               };
                var pagefavlist = (from a in pagelist
                                   group a by a.ModuleNo
                    into g
                                   select new
                                   {
                                       ModulesName = pagelist.Where(s => s.ModuleNo == g.Key).FirstOrDefault().ModulesName,
                                       AllCount = pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count(),
                                       //VisitorPerCent = (pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0) ? Math.Ceiling((double)pagelist.Where(s => s.ModuleNo == g.Key && s.IsVisitor == 1).Select(s => s.Wxuserid).Distinct().Count() / pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100) : 0,

                                       VisitorPerCent = (pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0) ? Math.Ceiling((double)pagelist.Where(s => s.ModuleNo == g.Key && s.IsCompleteRegister == 0).Select(s => s.Wxuserid).Distinct().Count() / pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100) : 0,

                                       UserPerCent = 100 - ((pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0) ? Math.Ceiling((double)pagelist.Where(s => s.ModuleNo == g.Key && s.IsCompleteRegister == 0).Select(s => s.Wxuserid).Distinct().Count() / pagelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100) : 0)


                                   }).OrderByDescending(s => s.AllCount).ToList();
                #endregion

                #region Top访问模块
                var modulelist = from a in _rep.Table<VisitModules>().Where(s => s.IsDeleted != 1 && s.VisitStart >= begin_date && s.VisitStart <= end_date&&s.UnionId != null)
                                 join b in _rep.Table<WxUserModel>().Where(s=>s.IsDeleted!=1) on a.WxUserid equals  b.Id
                                 select new
                                 {
                                     ModulesName = a.ModuleNo == "1" ? "发现" : (a.ModuleNo == "2" ? "会议" : (a.ModuleNo == "3" ? "知识库" : "我的")),
                                     IsVisitor = a.Isvisitor,
                                     ModuleNo = a.ModuleNo,
                                     Wxuserid = a.WxUserid,
                                     IsCompleteRegister = b.IsCompleteRegister
                                 };

                var modulefavlist = (from a in modulelist
                                     group a by a.ModuleNo
                    into g
                                     select new
                                     {
                                         ModulesName = modulelist.Where(s => s.ModuleNo == g.Key).FirstOrDefault().ModulesName,
                                         AllCount = modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count(),
                                         //VisitorPerCent = (modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0)
                                         //    ? Math.Ceiling((double)modulelist.Where(s => s.ModuleNo == g.Key && s.IsVisitor == 1).Select(s => s.Wxuserid).Distinct().Count() /
                                         //                   modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100)
                                         //    : 0,

                                         VisitorPerCent = (modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0)
                                             ? Math.Ceiling((double)modulelist.Where(s => s.ModuleNo == g.Key && s.IsCompleteRegister == 0).Select(s => s.Wxuserid).Distinct().Count() /
                                                            modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100)
                                             : 0,


                                         UserPerCent = 100 - ((modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() > 0)
                                                         ? Math.Ceiling((double)modulelist.Where(s => s.ModuleNo == g.Key && s.IsCompleteRegister == 0).Select(s => s.Wxuserid).Distinct().Count() /
                                                                        modulelist.Where(s => s.ModuleNo == g.Key).Select(s => s.Wxuserid).Distinct().Count() * 100)
                                                         : 0)
                                     }).OrderByDescending(s => s.AllCount).ToList();
                #endregion
                 
                #region 活跃趋势人数

                var visitmodulelist = from a in _rep.Table<VisitModules>().Where(s => s.IsDeleted != 1&&s.UnionId!=null) select a;
                //月活跃注册用户人数=时间段内访问过小程序 注册用户
                var activecount = visitmodulelist.Where(s => s.CreateTime >= begin_date && s.CreateTime <= end_date).Select(s => s.WxUserid).Distinct().Count();
                var activeusercount = visitmodulelist.Where(s => s.CreateTime >= begin_date && s.CreateTime <= end_date && s.Isvisitor == 0).Select(s => s.WxUserid).Distinct().Count();

                //月活跃注册用户比例= (时间段内访问过小程序的注册用户/时间段内访问过的总人数)
                var activepercent = (activecount > 0) ? Math.Ceiling((decimal)activeusercount / activecount * 100) : 0;

                //新增注册用户留存率   查询结束前的注册用户中在 时间段内访问过小程序的人数/  开始时间前的注册用户

                var beforereglist = (from a in _rep.Table<WxUserModel>().Where(s => s.CodeTime <= begin_date)
                                     select new
                                     {
                                         wxuserid = a.Id
                                     }).Distinct();

                var newvistcount = (from a in _rep.Table<VisitModules>().Where(s =>
                        s.IsDeleted != 1 && s.CreateTime >= begin_date && s.CreateTime <= end_date)
                                    join b in beforereglist on a.WxUserid equals b.wxuserid
                                    select a).Select(s => s.WxUserid).Distinct().Count();

                var newstaypercent = (beforereglist.Count() > 0) ? Math.Ceiling((decimal)newvistcount / beforereglist.Count() * 100) : 0;


                //活跃注册用户留存率   查询结束前的活跃用户中在 时间段内访问过小程序的人数/  开始时间前的活跃用户
                var actbeforereglist = (from a in _rep.Table<VisitModules>().Where(s =>
                        s.IsDeleted != 1 && s.CreateTime >= begin_date && s.CreateTime <= end_date)
                                       join b in _rep.Table<WxUserModel>().Where(s=>s.IsDeleted!=1&&s.IsCompleteRegister==1)
                                           on a.WxUserid equals b.Id
                                        select new
                                        {
                                            wxuserid = a.WxUserid
                                        }).Distinct();

                var actvistcount = (from a in _rep.Table<VisitModules>().Where(s =>
                      s.IsDeleted != 1 && s.CreateTime >= begin_date && s.CreateTime <= end_date)
                                    join b in actbeforereglist on a.WxUserid equals b.wxuserid
                                    select a).Select(s => s.WxUserid).Distinct().Count();

                var actstaypercent = (actbeforereglist.Count() > 0) ? Math.Ceiling((decimal)actvistcount / actbeforereglist.Count() * 100) : 0;

                //累计访问人数=时间段内访问过的总人数
                var allvistcount = activecount;

                #endregion
                rvm.Success = true;
                rvm.Msg = "success！";
                rvm.Result = new
                {
                    rows = new
                    {
                        pagefavlist = pagefavlist, 
                        modulefavlist = modulefavlist,
                        activeusercount= activeusercount, //月活跃注册用户人数
                        activepercent = activepercent,    //月活跃注册用户比例
                        newstaypercent = newstaypercent,  //新增注册用户留存率
                        actstaypercent = actstaypercent,  //活跃注册用户留存率
                        allvistcount = allvistcount      //累计总访问人数
                    }
                };



            }
            catch (Exception e)
            {
                rvm.Msg = e.Message;
                rvm.Success = false;
                LoggerHelper.WriteLogInfo("[GetActiveList Error]" + e.Message);

            }

            return rvm;

        }

        /// <summary>
        /// 活跃趋势-打开次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetOpenTimesList(RowNumModel<StatisticsTimeViewModel> model)
        {

            ReturnValueModel rvm = new ReturnValueModel();

            var startTime = model.SearchParams.begin_date.AddSeconds(1.0);
            var endTime = model.SearchParams.end_date.AddDays(1.0).AddSeconds(-1.0);
            int IsVisotor = Convert.ToInt32(model.SearchParams.IsVistor ?? "0");

            var allVisitList = _rep.Where<VisitTimes>(x => x.IsDeleted != 1 && x.VisitStart >= startTime && x.VisitStart <= endTime);
            var visitList = allVisitList.Where(x => x.Isvisitor == IsVisotor).ToList();//游客/非游客

            var allvisitlistNumber = allVisitList?.Count() ?? 0;
            var visitlistNumber = visitList?.Count() ?? 0;
            var advTime = visitlistNumber == 0 ? 0 : visitList.Sum(s => s.StaySeconds) / visitlistNumber;

            rvm.Success = true;
            rvm.Msg = "success！";
            rvm.Result = new
            {
                IsVisotor = IsVisotor == 0 ? "用户" : "游客",
                Count = visitlistNumber,
                PerCent = allvisitlistNumber == 0 ? 0 : (int)(((double)visitlistNumber / (double)allvisitlistNumber)*100),
                AdvTime = advTime + "秒"
            };
     
            return rvm;

        }

        /// <summary>
        /// 获取医生列表信息
        /// 1 认证通过 ; 
        ///3,6 认证未通过人数（失败 和申诉拒绝）
        ///2 认证 未定人数 
        ///5,4 总待验证通过人数
        /// </summary>
        /// <param name="model">1 认证通过 ;3,6 认证未通过人数（失败 和申诉拒绝） 2 认证 未定人数 5,4 总待验证通过人数  </param>
        /// <returns></returns>
        public ReturnValueModel GetDoctor(RowNumModel<List<int>> model)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var doctorList = _rep.Where<WxUserModel>(x => x.IsDeleted != 1 && (x.IsSalesPerson ?? 0) != 1);
            var isVerify = model.SearchParams;
            if (isVerify == null)
            {
                //未注册(已授权未注册的清单)
                doctorList = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister != 1);
            }
            else
            {
                doctorList = doctorList.Where(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && isVerify.Contains(x.IsVerify));
            }
            doctorList.Select(x => new DoctorViewModel()
            {
                Id = x.Id,
                WxPicture = x.WxPicture,
                gender = x.WxGender,
                WxName = x.WxName,
                UserName = x.UserName,
                HospitalName = x.HospitalName,
                DepartmentName = x.DepartmentName,
                Mobile = x.Mobile,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime
            });
            rvm.Success = true;
            rvm.Msg = "";
            var total = doctorList.Count();
            var rows =  doctorList.ToList().ToPaginationList(model.PageIndex, model.PageSize);

            rvm.Result = new
            {
                total,
                rows
            };
            return rvm;
        }

        /// <summary>
        /// 新增留存率
        /// 指定时间新增（即首次访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetAddRetain(StatisticsTimeViewModel model)
        {
            var startTime = model.begin_date;
            var endTime = model.end_date.AddDays(1).AddSeconds(-1);
            //天数差
            var daySum = (int)(model.end_date - model.begin_date).TotalDays;
            //访问人数 注册&医生
            var doctorList = _rep.Where<WxUserModel>(x => x.IsCompleteRegister == 1 && (x.IsSalesPerson ?? 0) == 0 && startTime <= x.CreateTime && x.CreateTime <= endTime).Select(x=>new { x.Id, x.CreateTime }).ToList();
            var doctorCount = doctorList.Count();

            List<float> pres = new List<float>();
            List<int> dayList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 14, 30 };

            foreach (var item in dayList)
            {
                var tempCount = 0;
                var per = 0F;
                //天数内
                foreach (var item2 in doctorList)
                {
                    //注册时间
                    var creatTime = DateTime.Parse(item2.CreateTime.Value.ToShortDateString());
                    //天数内都
                    var visitStartTime = creatTime.AddDays(item);
                    var visitEndTime = creatTime.AddDays(item+1).AddSeconds(-1);
                    //是否访问
                    var isVisit = _rep.FirstOrDefault<VisitModules>(x => visitStartTime <= x.CreateTime && x.CreateTime <= visitEndTime && x.WxUserid == item2.Id) != null;
                    if (isVisit)
                    {
                        tempCount++;
                    }
                }
                if (doctorCount != 0)
                {
                    per = (tempCount * 1F) / (doctorCount * 1F);
                }
                pres.Add(per);
            }
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = pres;
            return rvm;
        }
        /// <summary>
        /// 活跃留存
        /// 指定时间活跃（即访问小程序）的用户，在之后的第N天，再次访问小程序的用户数占比
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValueModel GetActiveRetain(StatisticsTimeViewModel model)
        {
            List<float> pres = new List<float>();
            List<int> dayList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 14, 30 };
            var startTime = model.begin_date;
            var endTime = model.end_date.AddDays(1).AddSeconds(-1);

            //当前时间段活跃用户
            var doctorList = _rep.Where<VisitModules>(x => startTime <= x.CreateTime && x.CreateTime <= endTime).Join(_rep.Where<WxUserModel>(x => x.IsCompleteRegister == 1 && (x.IsSalesPerson ?? 0) == 0), x => x.WxUserid, y => y.Id, (x, y) => new { x.Id, x.WxUserid, x.CreateTime }).GroupBy(x => x.WxUserid).Select(x => new { WxUserid = x.Key, CreateTime = x.Min(item => item.CreateTime) });
            //当前用户数量 去重
            var doctorCount = doctorList.Select(x=>x.WxUserid).Count();

            foreach (var item in dayList)
            {
                var tempCount = 0;
                var per = 0F;
                //天数内
                foreach (var item2 in doctorList)
                {
                    //访问时间
                    var creatTime = DateTime.Parse(item2.CreateTime.Value.ToShortDateString());
                    //天数内都
                    var visitStartTime = creatTime.AddDays(item);
                    var visitEndTime = creatTime.AddDays(item + 1).AddSeconds(-1);
                    //是否访问
                    var isVisit = _rep.FirstOrDefault<VisitModules>(x => visitStartTime <= x.CreateTime && x.CreateTime <= visitEndTime && x.WxUserid == item2.WxUserid) != null;
                    if (isVisit)
                    {
                        tempCount++;
                    }
                }
                if (doctorCount != 0)
                {
                    per = (tempCount * 1F) / (doctorCount * 1F);
                }
               
                pres.Add(per);
            }

            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = pres;
            return rvm;
        }
    }
}
