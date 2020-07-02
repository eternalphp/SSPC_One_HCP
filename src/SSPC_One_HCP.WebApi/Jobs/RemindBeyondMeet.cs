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

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 超期未审核的会议邮件提醒
    /// </summary>
    public class RemindBeyondMeet : IJob
    {
        /// <summary>
        /// 异步执行会议超期审核提醒
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => RemindMeetBeyond());
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
        /// 会议超期审核提醒
        /// </summary>
        public void RemindMeetBeyond()
        {
            try
            {
                LoggerHelper.WriteLogInfo($"[RemindMeetBeyond]:******进入方法！！******");
                var rep = ContainerManager.Resolve<IEfRepository>();

                //获取创建后已经过了会议开始时间，管理员还没有审核的会议
                //未删除 未审核 开始日期小于等于当前日期
                var list = from a in rep.Where<MeetInfo>(s => s.IsDeleted != 1 && s.MeetStartTime <= DateTime.Now && (s.IsCompleted == EnumComplete.AddedUnapproved || s.IsCompleted == EnumComplete.UpdatedUnapproved) &&s.HasReminded==0) 
                           select new
                           {
                               a.Id,
                               a.MeetTitle,
                               a.MeetStartTime, 
                           };
                list = list.Take(1);
                if (list == null || list.Count() == 0) return; 

                //遍历需要提醒的会议 发送邮件提醒
                //foreach (var item in list)
                //{
                //    try
                //    {
                //        LoggerHelper.WriteLogInfo($"[RemindMeetBeyond]:******发送会议邮件开始******");
                //        string title = item.MeetTitle;
                //        var body = "多福医生-会议即将过期，请及时审核！";
                //        var subject = "多福医生-会议超期提醒";
                //        if (MailUtil.SendMail(body, subject, ""))
                //        {
                //            //发送成功 更新是否提醒字段HasReminded
                //            var meetInfo = rep.FirstOrDefault<MeetInfo>(s => s.IsDeleted != 1 && s.Id == item.Id);
                //            if (meetInfo != null)
                //            {
                //                LoggerHelper.WriteLogInfo($"[RemindMeetBeyond]:item.id:" + item.Id);

                //                meetInfo.HasReminded = 1; //已发送提醒短信
                //                rep.Update(meetInfo);
                //                rep.SaveChanges();
                                
                //                LoggerHelper.WriteLogInfo($"[RemindMeetBeyond]:set HasReminded to 1");
                //            }
                //        }


                //        LoggerHelper.WriteLogInfo($"[RemindMeetBeyond]:******发送会议邮件结束******");
                //    }
                //    catch (Exception ex)
                //    {
                //        LoggerHelper.Error($"--------------------------------------------------------------------------------");
                //        LoggerHelper.Error($"[MSG]:{ex.Message};\n");
                //        LoggerHelper.Error($"[Source]:{ex.Source}\n");
                //        LoggerHelper.Error($"[StackTrace]:{ex.StackTrace}\n");
                //        LoggerHelper.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                //        LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.RemindJob.RemindMeet]]\n");
                //        LoggerHelper.Error($"--------------------------------------------------------------------------------");
                //    }
                //}
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                LoggerHelper.Error($"[MSG]:{ex.Message};\n");
                LoggerHelper.Error($"[Source]:{ex.Source}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.StackTrace}\n");
                LoggerHelper.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.RemindBeyondMeet.RemindMeetBeyond]]\n");
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                throw ex;
            }
        }

    }
}