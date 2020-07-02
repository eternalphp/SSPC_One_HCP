using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SSPC_One_HCP.WebApi.Jobs
{
    /// <summary>
    /// 后台计划任务
    /// </summary>
    public class JobScheduler
    {
        //调度器工厂
        private static readonly ISchedulerFactory factory = null;
        //调度器
        private static readonly IScheduler scheduler = null;

        static JobScheduler()
        {
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler().Result;
            scheduler.Start();
        }
        
        /// <summary>
        /// 启动后台计划任务
        /// </summary>
        public static void Start()
        {
            scheduler.GetJobGroupNames();

            /*-------------计划任务代码实现------------------*/
            IJobDetail job;
            ITrigger trigger;

            //创建任务
            job = Quartz.JobBuilder.Create<RemindJob>().Build();
            //创建触发器
            trigger = TriggerBuilder.Create()
                .WithIdentity("TimeTrigger", "TimeGroup")
                .WithSimpleSchedule(t => t.WithIntervalInMinutes(5).RepeatForever())
                .Build();
            //添加任务及触发器至调度器中
            scheduler.ScheduleJob(job, trigger);


            //创建任务
            //job = Quartz.JobBuilder.Create<MeetSubscribeJob>().Build();
            ////创建触发器
            //trigger = TriggerBuilder.Create()
            //    .WithIdentity("MeetSubscribeJob", "TimeGroup")
            //    .WithSimpleSchedule(t => t.WithIntervalInMinutes(5).RepeatForever())
            //    .Build();
            ////添加任务及触发器至调度器中
            //scheduler.ScheduleJob(job, trigger);

            //每天凌晨1:00获取临床接口的访问数据
            //创建任务
            //job = Quartz.JobBuilder.Create<SyncGuidJob>().Build();
            ////创建触发器
            //string cronExpression2 ="" ;
            //if (string.IsNullOrEmpty(cronExpression2))
            //      cronExpression2 = "0 0 1 * * ? *";
            //    //cronExpression2 = "0 10,15,40 * * * ? ";
            //trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression2).Build();
            ////添加任务及触发器至调度器中
            //scheduler.ScheduleJob(job, trigger);


            //每天凌晨2:00同步费卡文库的科室会
            //job = Quartz.JobBuilder.Create<SyncFkLibMeetingsJob>().Build();
            //string cronExpression = ConfigurationManager.AppSettings["SyncFkLibMeetingsJobCronExpression"];
            //if (string.IsNullOrEmpty(cronExpression)) cronExpression = "0 0 2 ? * *";
            //trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();
            //scheduler.ScheduleJob(job, trigger);

            //每月1日凌晨1:00同步更新签到人员认证信息到费卡文库
            //job = Quartz.JobBuilder.Create<SyncFkLibUsersJob>().Build();
            ////1.Seconds 秒
            ////2.Minutes 分钟
            ////3.Hours 小时
            ////4.Day - of - Month 月中的天
            ////5.Month 月
            ////6.Day - of - Week 周中的天
            ////7.Year(optional field) 年（可选的域）
            //cronExpression = ConfigurationManager.AppSettings["SyncFkLibUsersJobCronExpression"];
            //if (string.IsNullOrEmpty(cronExpression)) cronExpression = "0 0 1 1 * ?";
            //trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();
            //scheduler.ScheduleJob(job, trigger);

            /*-------------计划任务代码实现------------------*/

            //启动
            scheduler.Start();
        }
    }
}