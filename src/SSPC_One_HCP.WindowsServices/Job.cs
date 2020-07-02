using Newtonsoft.Json;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;


namespace WindowsServices
{
    public class Job
    {
        /// <summary>
        /// 临床接口地址
        /// </summary>
        private static string GuidVisitUrl = ConfigurationManager.AppSettings["GuidUrl"];
        /// <summary>
        /// 临床接口 ProjectId
        /// </summary>
        private static string GuidProjectId = ConfigurationManager.AppSettings["GuidProjectId"];
        /// <summary>
        /// 临床接口 KeyCode
        /// </summary>
        private static string AESGuidKeyCode = ConfigurationManager.AppSettings["AESGuidKeyCode"];
        /// <summary>
        /// 费卡文库地址
        /// </summary>
        private readonly string _host = ConfigurationManager.AppSettings["FkLibHostUrl"];
        /// <summary>
        /// 费卡文库
        /// </summary>
        private readonly string _appId = ConfigurationManager.AppSettings["FkLibAppId"];

        /// <summary>
        /// 科室会
        /// </summary>
        private readonly string MeetingUrl = ConfigurationManager.AppSettings["MeetingUrl"];
        private readonly string MeetingPerUrl = ConfigurationManager.AppSettings["MeetingPerUrl"];


        private static int pageSize = 50;

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="dateTime">Unix时间戳</param>  
        /// <returns>long</returns>  
        private static long TimeStamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>  
        /// 将c#Unix时间戳格式 转换为 DateTime时间格式
        /// </summary>  
        /// <param name="timeStamp">时间</param>  
        /// <returns>long</returns>  
        private static DateTime TimeStamp(string timeStamp)
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


        #region 临床指南同步

        /// <summary>
        /// 临床指南 浏览记录同步
        /// </summary>
        public void GuidVisit()
        {
            var result = GetGuidVisit();

            if (result.success && result.data.count > 0)
            {
                var pageCount = (result.data.count % pageSize == 0) ? (result.data.count / pageSize) : (result.data.count / pageSize + 1);
                for (int i = 1; i <= pageCount; i++)
                {
                    result = GetGuidVisit(i);
                    UpdateGuidVisit(result.data.items);
                }
            }
        }

        /// <summary>
        /// 获取临床指南浏览记录数据
        /// </summary>
        private GuidVisitResult GetGuidVisit(int pageStart = 1)
        {
            try
            {
                var date = DateTime.Parse(DateTime.Now.AddDays(-1.0).ToShortDateString());
                long startTime = TimeStamp(date.AddMilliseconds(1.0));
                long endTime = TimeStamp(date.AddDays(1.0).AddMilliseconds(-1.0));
                string guidurl = $"{GuidVisitUrl}projectId={GuidProjectId}&startTime={startTime}&endTime={endTime}&start={pageStart}&num={pageSize}";
                //获取临床指南返回的数据 
                string res = HttpService.Get(guidurl);
                res = HttpUtility.UrlDecode(res);
                string key = AESGuidKeyCode;
                string iv = res.Substring(0, 16);
                //解密数据
                string data = EncryptHelper.DecodeAES(res, key, iv);
                data = data.Substring(data.IndexOf('{'), data.LastIndexOf('}') - data.IndexOf('{') + 1);
                LoggerHelper.WriteLogInfo("[success!!!][GetGuidVisit]:" + data);
                //序列化数据
                return JsonConvert.DeserializeObject<GuidVisitResult>(data);
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[Error!!!][GetGuidVisit]:" + e.Message);
                LoggerHelper.WriteLogInfo("------------------");
                return new GuidVisitResult() { success = false, data=new GuidVisitData() { } };
            }
           
        }

        /// <summary>
        /// 同步临床指南浏览记录数据
        /// </summary>
        /// <param name="guiditems"></param>
        private void UpdateGuidVisit(GuidVisitItem[] guiditems)
        {
            var sqls = new List<Dictionary<string, object>>() { };
            foreach (GuidVisitItem item in guiditems)
            {
                sqls.Add(new Dictionary<string, object>() { { "insert into GuidVisit(Id, userid, ActionType, GuideId, GuideName, GuideType, CreateTime, UpdateTime, VisitStart, VisitEnd, StaySeconds, IsEnabled, IsDeleted)values(@Id, @userid, @ActionType, @GuideId, @GuideName, @GuideType, @CreateTime, @UpdateTime, @VisitStart, @VisitEnd, @StaySeconds, 0, 0)", new
                {
                    Id = Guid.NewGuid().ToString(),
                    userid = item.uid,
                    ActionType = item.actionType,
                    GuideId = item.guideId,
                    GuideName = item.guideName,
                    GuideType = item.guideType,
                    CreateTime = TimeStamp(item.createTime),
                    UpdateTime = TimeStamp(item.createTime),
                    VisitStart = TimeStamp(item.createTime),
                    VisitEnd = TimeStamp(item.createTime).AddMilliseconds(item.stayTime),
                    StaySeconds = item.stayTime / 1000
                } } });
            }
            try
            {
                var result = DapperHelper<GuidVisitItem>.ExecuteTransaction(sqls);
                LoggerHelper.WriteLogInfo("[成功!!!][UpdateGuidVisit]:" + result);
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[失败!!!][UpdateGuidVisit]:" + e.Message);
                LoggerHelper.WriteLogInfo("------------------");
            }
        }

        #endregion

        #region 每天凌晨2:00同步费卡文库的科室会

        /// <summary>
        /// 每天凌晨2:00同步费卡文库的科室会
        /// </summary>
        public void FkLibMeetings()
        {
            //两种方式：
            //1）费卡文库创建科室会，同步在小程序生成；缺点：一旦系统维护，不能及时同步
            //2）定期同步；缺点：非实时；每天凌晨科室会同步
            //
            //注：将采用定期同步：每天凌晨同步已完成的科室会到OneHCP数据库
            //同步内容：
            //科室会编号、科室会标题，活动状态，创建时间，召开时间，医院，科室，内容，参与人数
            try
            {
                var returnModel = HttpUtils.PostResponse<MeetingResult>(MeetingUrl, "", "application/x-www-form-urlencoded");
                if (returnModel.status == "1")
                {
                    var result = JsonConvert.DeserializeObject<List<MeetingItem>>(returnModel.result);
                    UpdateMeeting(result);
                }
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[error!!!][FkLibMeetings]:" + e.Message);
                LoggerHelper.WriteLogInfo("------------------");               
            }       
        }
        /// <summary>
        /// 同步科室会
        /// </summary>
        /// <param name="guiditems"></param>
        private void UpdateMeeting(List<MeetingItem> guiditems)
        {
            var sqls = new List<Dictionary<string, object>>() { };
            foreach (MeetingItem item in guiditems)
            {
                sqls.Add(new Dictionary<string, object>() { { "insert into MeetInfo(Id,SourceId,MeetTitle,CreateTime,MeetDate,SourceHospital,SourceDepartment,MeetIntroduction,MeetingNumber,IsCompleted,Source)values(@Id,@SourceId,@MeetTitle,@CreateTime,@MeetDate,@SourceHospital,@SourceDepartment,@MeetIntroduction,@MeetingNumber,@IsCompleted,@Source)", new
                {
                    Id = Guid.NewGuid().ToString(),
                    SourceId = item.ActivityID, //科室会ID
                    MeetTitle = item.ActivityName, //科室会标题
                    CreateTime = Convert.ToDateTime(item.CreatTime), //创建时间
                    MeetDate = Convert.ToDateTime(item.HoldTime), //召开时间
                    SourceHospital = item.Hospital, //医院
                    SourceDepartment = item.KeShi, //科室
                    MeetIntroduction = item.Context, //内容
                    MeetingNumber = item.PartInNum, //参与人数
                    IsCompleted =1,//已审核
                    Source = _appId//来源费卡文库
            } } });
            }
            try
            {
                var c = DapperHelper<GuidVisitItem>.ExecuteTransaction(sqls);
                LoggerHelper.WriteLogInfo("[sucess!!!][UpdateMeeting]:" + c);
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[error!!!][UpdateMeeting]:" + e.Message);
                LoggerHelper.WriteLogInfo(JsonConvert.SerializeObject(guiditems));
                LoggerHelper.WriteLogInfo("------------------");
            }
        }
        #endregion

        #region 每月1日凌晨1:00同步更新签到人员认证信息到费卡文库

        /// <summary>
        /// 每月1日凌晨1:00同步更新签到人员认证信息到费卡文库
        /// </summary>
        public void PersonInfo()
        {
            //将采用定期同步：每月凌晨同步更新，根据OneHCP唯一ID更新签到人员认证信息
            //同步字段：OneHCP医生验证状态、理由、云势ID
            List<PerInfoItem> persons = GetPerson();
            if ((persons?.Count ?? 0) > 0)
            {
                var postStr = $"perInfos={JsonConvert.SerializeObject(persons)}";
                try
                {                    
                    var returnModel = HttpUtils.PostResponse<PerInfoReturn>(MeetingPerUrl, postStr, "application/x-www-form-urlencoded");
                    if (returnModel.status == "1")
                    {
                        LoggerHelper.WriteLogInfo("------------------");
                        LoggerHelper.WriteLogInfo("[success!!!][PersonInfo]:发送成功");
                        LoggerHelper.WriteLogInfo("------------------");
                    }
                    else
                    {
                        LoggerHelper.WriteLogInfo("------------------");
                        LoggerHelper.WriteLogInfo("[error!!!][PersonInfo]:失败");
                        LoggerHelper.WriteLogInfo("[error!!!][PersonInfo]:" + JsonConvert.SerializeObject(returnModel));
                        LoggerHelper.WriteLogInfo("------------------");
                    }
                }
                catch (Exception e)
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[error!!!][PersonInfo]:" + e.Message);
                    LoggerHelper.WriteLogInfo("------------------");
                   
                }
            }
            else
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[info!!!][PersonInfo]:无需更新");
                LoggerHelper.WriteLogInfo("------------------");
            }
        }

        private List<PerInfoItem> GetPerson()
        {
            var sql = "select Id as OneHCPID, yunshi_doctor_id as YSID, reason as OneHCPReason,status as OneHCPState  from DoctorModel where IsDeleted!=1";
            try
            {
                LoggerHelper.WriteLogInfo("[success!!!][GetPerson]:" );
                var result = DapperHelper<PerInfoItem>.Query(sql, null);
                return result;
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("[error!!!][UpdateMeeting]:" + e.Message);
                LoggerHelper.WriteLogInfo(sql);
                LoggerHelper.WriteLogInfo("------------------");
                return null;
            }
        }
        #endregion

    }
}