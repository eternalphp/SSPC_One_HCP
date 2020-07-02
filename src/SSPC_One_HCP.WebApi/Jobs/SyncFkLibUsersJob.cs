using Newtonsoft.Json;
using Quartz;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Threading.Tasks;

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 同步费卡文库的人员信息
    /// </summary>
    public class SyncFkLibUsersJob : IJob
    {
        /// <summary>
        /// 异步执行工作线程
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => SyncUsers());
            }
            catch (Exception e)
            {
                LoggerHelper.Error($"--------------------------------------------------------------------------------");
                LoggerHelper.Error($"[MSG]:{e.Message};\n");
                LoggerHelper.Error($"[Source]:{e.Source}\n");
                LoggerHelper.Error($"[StackTrace]:{e.StackTrace}\n");
                LoggerHelper.Error($"[StackTrace]:{e.TargetSite.Name}\n");
                LoggerHelper.Error($"[MethodName]：[[SSPC_One_HCP.WebApi.Jobs.SyncFkLibUsers.SyncUsers]]\n");
                LoggerHelper.Error($"--------------------------------------------------------------------------------");

                //JobExecutionException e2 = new JobExecutionException(e);
                // this job will refire immediately
                //e2.RefireImmediately = true;
                //throw e2;
            }
        }

        /// <summary>
        /// 同步费卡文库的人员信息
        /// </summary>
        /// <returns></returns>
        public void SyncUsers()
        {
            LoggerHelper.WriteLogInfo($"------- SSPC_One_HCP.WebApi.Jobs.SyncFkLibMeetings.SyncMettings Begin ------------------------------");
            LoggerHelper.WriteLogInfo($"[SyncUsers]Time={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            var fkLib = ContainerManager.Resolve<IFKLibService>();

            var ret = fkLib.SyncPersonInfo();

            LoggerHelper.WriteLogInfo($"[SyncMettings]result.Success=" + ret.Success);
            LoggerHelper.WriteLogInfo($"[SyncMettings]result.Msg=" + ret.Msg);
            LoggerHelper.WriteLogInfo($"[SyncMettings]result.Result=" + JsonConvert.SerializeObject(ret.Result));
            LoggerHelper.WriteLogInfo($"------- SSPC_One_HCP.WebApi.Jobs.SyncFkLibMeetings.SyncMettings End  -------------------------------");
        }
    }
}