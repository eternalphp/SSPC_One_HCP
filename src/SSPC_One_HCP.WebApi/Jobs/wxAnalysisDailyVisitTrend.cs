using Newtonsoft.Json;
using Quartz;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SSPC_One_HCP.Core.Domain.Enums;
using System.Configuration;
using System.Text;

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 微信接口-获取用户访问小程序数据日趋势
    /// </summary>
    public class wxAnalysisDailyVisitTrend : IJob
    {

        private   string analysisDailyVisitTrendUrl = ConfigurationManager.AppSettings["AnalysisDailyVisitTrendUrl"];
        private   string getApiTokenUrl = ConfigurationManager.AppSettings["GetApiTokenUrl"];
        private readonly string xappid = ConfigurationManager.AppSettings["xappid"];
        private readonly string xappsecret = ConfigurationManager.AppSettings["xappsecret"];
        
        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => GetAnalysisDailyVisitTrend());
            }
            catch (Exception e)
            {
                JobExecutionException e2 = new JobExecutionException(e);
              
                e2.RefireImmediately = true;
                throw e2;
            }
        }
        /// <summary>
        /// 获取用户访问小程序数据日趋势
        /// </summary>
        public void GetAnalysisDailyVisitTrend()
        {
            try
            {
                var rep = ContainerManager.Resolve<IEfRepository>();
                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:******进入方法！！******");

               string getApiTokenUrlStr = getApiTokenUrl+"&appid=" +xappid+"&secret="+ xappsecret;
                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:getApiTokenUrl:" + getApiTokenUrlStr);
                //获取access_Token
                string tokenresultStr =
                    HttpWebResponseUtil.CreateGetHttpResponse(getApiTokenUrl, null, null, null);

                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:获取access_Token:" + tokenresultStr);
                tokenResult model = JsonConvert.DeserializeObject<tokenResult>(tokenresultStr);

                string beforedate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                string postJosn = "{\"begin_date\":\""+ beforedate + "\",\"end_date\":\""+ beforedate + "\"}";
                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:analysisDailyVisitTrendUrl 参数传递:" + postJosn);

                //调用小程序接口
                string  analysisDailyVisitTrendUrlStr = analysisDailyVisitTrendUrl+ model.access_token;
                string dataresult = HttpWebResponseUtil.CreatePostHttpResponse(analysisDailyVisitTrendUrlStr, beforedate,
                    null, null, Encoding.UTF8, null);

                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:analysisDailyVisitTrendUrl 返回值:" + dataresult);

                Rootobject rootmodel = JsonConvert.DeserializeObject<Rootobject>(dataresult);
                LoggerHelper.WriteLogInfo($"[GetAnalysisDailyVisitTrend]:访问人数：" + rootmodel.list[0].visit_uv);
                if (rootmodel != null)
                {
                    var dailyVisit = rep.FirstOrDefault<AnalysisDailyVisitTrend>(s => s.ref_date == beforedate);
                    if (dailyVisit == null )
                    {
                        var newVisit = new AnalysisDailyVisitTrend();
                        newVisit.ref_date = beforedate;
                        newVisit.session_cnt = rootmodel.list[0].session_cnt ;
                        newVisit.visit_pv = rootmodel.list[0].visit_pv ;
                        newVisit.visit_uv = rootmodel.list[0].visit_uv ;
                        newVisit.visit_uv_new = rootmodel.list[0].visit_uv_new ;
                        newVisit.stay_time_uv = rootmodel.list[0].stay_time_uv ;
                        newVisit.stay_time_session = rootmodel.list[0].stay_time_session;
                        newVisit.visit_depth = rootmodel.list[0].visit_depth ;
                        newVisit.Id = Guid.NewGuid().ToString();
                        newVisit.IsDeleted = 0;
                        newVisit.IsEnabled = 0;
                        newVisit.CreateTime=(Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")));
                        rep.Insert(newVisit);
                        rep.SaveChanges();
                        LoggerHelper.WriteLogInfo($"[RemindMeet]:set HasReminded to 1");
                    }
                }

                //调小程序接口 保存至表单中  
                //调用时日期都是前一天的

            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                LoggerHelper.Error($"[MSG]:{ex.Message};\n");
                LoggerHelper.Error($"[Source]:{ex.Source}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.StackTrace}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.wxAnalysisDailyVisitTrend.GetAnalysisDailyVisitTrend]]\n");
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                throw ex;
            }
        }

    

        private class tokenResult
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
        }

        private class Rootobject
        {
            public List[] list { get; set; }
        }

        private class List
        {
            public string ref_date { get; set; }
            public int session_cnt { get; set; }
            public int visit_pv { get; set; }
            public int visit_uv { get; set; }
            public int visit_uv_new { get; set; }
            public float stay_time_session { get; set; }

            public float stay_time_uv { get; set; }
            public float visit_depth { get; set; }
        }
    }
}