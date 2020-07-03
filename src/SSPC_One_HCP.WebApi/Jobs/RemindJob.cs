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

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 短信提示
    /// </summary>
    public class RemindJob : IJob
    {
        private const string System_Name = "Dr.FK 多福医生";
        private const string Time_Format = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 异步执行会议提醒工作线程
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => RemindMeet());
            }
            catch (Exception e)
            {
                JobExecutionException e2 = new JobExecutionException(e);
                // this job will refire immediately
                e2.RefireImmediately = true;
                throw e2;
            }
        }

        /// <summary>
        /// 会议提醒
        /// </summary>
        /// <returns></returns>
        public void RemindMeet()
        {
            try
            {

                var rep = ContainerManager.Resolve<IEfRepository>();
                var dt = DateTime.UtcNow.AddHours(8);
                var list = from a in rep.Where<MeetInfo>(s => s.IsDeleted != 1 && s.MeetStartTime > dt)
                           join b in rep.Where<MyMeetOrder>(s => s.IsDeleted != 1 && s.HasReminded != 1 && s.IsRemind == 1) on a.Id equals b.MeetId
                           join c in rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.Mobile != null) on b.UnionId equals c.UnionId
                           select new
                           {
                               MeetOrderId = b.Id,
                               a.MeetTitle,
                               a.MeetStartTime,
                               c.Mobile,
                               b.RemindOffsetMinutes
                           };

                if (list == null || list.Count() == 0) return;

                var list2 = from a in list.ToList()
                            where a.MeetStartTime.Value.AddMinutes(a.RemindOffsetMinutes) <= DateTime.UtcNow.AddHours(8)
                            select a;

                foreach (var item in list2)
                {
                    if (string.IsNullOrEmpty(item.Mobile))
                        continue;
                    try
                    {
                        string title = item.MeetTitle;
                        string time = item.MeetStartTime?.ToString(Time_Format) ?? "";

                        SendSmsModel sm = new SendSmsModel
                        {
                            CompanyCode = "4033",
                            ParamName = JsonConvert.SerializeObject(new
                            {
                                systemName = System_Name,
                                meetingTime = time,
                                meetingType = title
                            }).Base64Encoding(),
                            PhoneNumbers = item.Mobile,
                            SystemId = "3",
                            SignName = "费卡中国",
                            TemplateId = "FKSMS0046"
                        };

                        LoggerHelper.WriteLogInfo($"[RemindMeet]:******发送会议提醒短信开始******");
                        LoggerHelper.WriteLogInfo($"[RemindMeet]:PhoneNumbers={sm.PhoneNumbers}");
                        LoggerHelper.WriteLogInfo($"[RemindMeet]:meetingTime={time}");
                        LoggerHelper.WriteLogInfo($"[RemindMeet]:meetingType={title}");
                        var smsResult = SmsUtil.SendMessage(sm);
                        if (smsResult?.ResultFlag ?? false)
                        {
                            LoggerHelper.WriteLogInfo($"[RemindMeet]:smsResult.ResultFlag=true");
                            var meetOrder = rep.FirstOrDefault<MyMeetOrder>(s => s.IsDeleted != 1 && s.Id == item.MeetOrderId);
                            if (meetOrder != null)
                            {
                                meetOrder.HasReminded = 1; //已发送提醒短信
                                rep.Update(meetOrder);
                                rep.SaveChanges();
                                LoggerHelper.WriteLogInfo($"[RemindMeet]:set HasReminded to 1");
                            }
                        }
                        LoggerHelper.WriteLogInfo($"[RemindMeet]:******发送会议提醒短信结束******");
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Error($"--------------------------------------------------------------------------------");
                        LoggerHelper.Error($"[MSG]:{ex.Message};\n");
                        LoggerHelper.Error($"[Source]:{ex.Source}\n");
                        LoggerHelper.Error($"[StackTrace]:{ex.StackTrace}\n");
                        LoggerHelper.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                        LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.RemindJob.RemindMeet]]\n");
                        LoggerHelper.Error($"--------------------------------------------------------------------------------");
                    }
                }



            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                LoggerHelper.Error($"[MSG]:{ex.Message};\n");
                LoggerHelper.Error($"[Source]:{ex.Source}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.StackTrace}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.RemindJob.RemindMeet]]\n");
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                //throw ex;
            }
        }
    }
}