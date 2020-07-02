using Newtonsoft.Json;
using Quartz;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Domain.GuidModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using SSPC_One_HCP.Services.Implementations;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 临床接口定时任务 触发器
    /// </summary>
    public class SyncGuidJob:IJob
    {

        /// <summary>
        /// 临床接口地址
        /// </summary>
        public static string GuidUrl { get; } = ConfigurationManager.AppSettings["GuidUrl"];

        /// <summary>
        /// 临床接口 ProjectId
        /// </summary>
        public static string GuidProjectId { get; } = ConfigurationManager.AppSettings["GuidProjectId"];

        /// <summary>
        /// 临床接口 KeyCode
        /// </summary>
        public static string AESGuidKeyCode { get; } = ConfigurationManager.AppSettings["AESGuidKeyCode"];

        /// <summary>
        /// 异步执行工作线程
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => SyncGuid());
            }
            catch (Exception e)
            {
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                LoggerHelper.Error($"[MSG]:{e.Message};\n");
                LoggerHelper.Error($"[Source]:{e.Source}\n");
                LoggerHelper.Error($"[StackTrace]:{e.StackTrace}\n");
                LoggerHelper.Error($"[StackTrace]:{e.TargetSite.Name}\n");
                LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.SyncGuidJob.SyncGuid]]\n");
                LoggerHelper.Error($"--------------------------------------------------------------------------------"); 
            }
        }

        /// <summary>
        /// 同步临床接口数据 
        /// </summary>
        /// <returns></returns>
        public void SyncGuid()
        {
            int pagesize = 50;
            LoggerHelper.WarnInTimeTest("[SyncGuid]:开始执行！" );
            string BeforeDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            long startTime = ConvertDateTimeToInt(Convert.ToDateTime(BeforeDate + " 00:00:00"));
            long endTime = ConvertDateTimeToInt(Convert.ToDateTime(BeforeDate + " 23:59:59"));
           
            string guidurl = GuidUrl + "projectId=" + GuidProjectId + "&startTime="+ startTime + "&endTime="+ endTime + "&start=1&num=50";

            LoggerHelper.WarnInTimeTest("[SyncGuid]====guidurl:" + guidurl);
            //获取临床指南返回的数据 
            string guidback = HttpService.Get(guidurl);
            guidback = HttpUtility.UrlDecode(guidback);

            string KeyWord = AESGuidKeyCode;
            string iv = guidback.Substring(0, 16);
            //解密数据
            string guiddata = EncryptHelper.DecodeAES(guidback, KeyWord, iv);
            guiddata = guiddata.Substring(guiddata.IndexOf('{'), guiddata.LastIndexOf('}') - guiddata.IndexOf('{') + 1);
            //序列化数据
            GuidResult model = JsonConvert.DeserializeObject<GuidResult>(guiddata);
            
            if (model.success&&model.data.count>0)
            {
               
                int allpagesize = (model.data.count % pagesize == 0)
                    ? (model.data.count / pagesize)
                    : (model.data.count / pagesize + 1);
                LoggerHelper.WarnInTimeTest("[SyncGuid]====allpagesize:"+allpagesize);
                //只有一页
                if (allpagesize == 1)
                {
                    AddGuid(model.data.items);
                }
                //不止一页
                else
                {
                    //轮询数据 并写入
                    for (int i = 1; i <= allpagesize; i++)
                    {
                        try
                        {
                            guidurl = GuidUrl + "projectId=" + GuidProjectId + "&startTime=" + startTime + "&endTime=" + endTime + "&start=" + i + "&num=" + pagesize + "";
                            guidback = HttpService.Get(guidurl);
                            guidback = HttpUtility.UrlDecode(guidback);
                            iv = guidback.Substring(0, 16);
                            guiddata = EncryptHelper.DecodeAES(guidback, KeyWord, iv);
                            guiddata = guiddata.Substring(guiddata.IndexOf("data") - 2, guiddata.LastIndexOf('}') - guiddata.IndexOf('{') + 1);

                            LoggerHelper.WarnInTimeTest("[SyncGuid]====current page:" + i);

                            GuidResult newmodel = JsonConvert.DeserializeObject<GuidResult>(guiddata);
                            AddGuid(newmodel.data.items);
                        }
                        catch (Exception e)
                        {

                            LoggerHelper.WarnInTimeTest("[写入GuidVisit Error]:" + e.Message);
                        }
                      
                    }
                }



            }
        }

        /// <summary>
        /// 临床指南数据批量新增
        /// </summary>
        /// <param name="guiditems"></param>
        public void AddGuid(GuidItem[] guiditems)
        {
            try
            {
                var rep = ContainerManager.Resolve<IEfRepository>();
                foreach (GuidItem guidItem in guiditems)
                {

                    string abc = guidItem.guideName;
                    var staySeconds = guidItem.stayTime / 1000;
                    rep.Insert(new GuidVisit()
                    {
                        Id = Guid.NewGuid().ToString(),
                        userid = guidItem.uid,
                        ActionType = guidItem.actionType,
                        GuideId = guidItem.guideId,
                        GuideName = guidItem.guideName,
                        GuideType = guidItem.guideType,
                        //CreateTime = DateTime.Now.AddDays(-1),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        VisitStart = ConvertStringToDateTime(guidItem.createTime),
                        VisitEnd = ConvertStringToDateTime(guidItem.createTime).AddSeconds(staySeconds),
                        StaySeconds = staySeconds,
                        IsEnabled = 0,
                        IsDeleted = 0
                    });
                    rep.SaveChanges();
                }
            }
            catch (Exception e)
            {
                LoggerHelper.WarnInTimeTest("[SyncGuid]:" + e.Message);
            }
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 时间戳转为C#格式时间  
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            if (timeStamp != null && !string.IsNullOrEmpty(timeStamp))
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            return DateTime.Now.AddDays(-1);
        }
    }
}