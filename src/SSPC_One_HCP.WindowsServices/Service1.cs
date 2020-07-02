using SSPC_One_HCP.Services.Utils;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServices
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private Timer guidVisitTimer;

        private Timer templateNoticeMsgTimer;
        private Timer templateRemindMsgTimer;

        private static TemplateJob templateJob;

        protected override void OnStart(string[] args)
        {
            LoggerHelper.WriteLogInfo("------------------");
            LoggerHelper.WriteLogInfo("serverStart V2");
            LoggerHelper.WriteLogInfo("------------------");
            templateJob = new TemplateJob();

            guidVisitTimer = new System.Timers.Timer();
            guidVisitTimer.Interval = 3610000;  //一小时
            //guidVisitTimert.Interval = 60000;  //一分钟
            guidVisitTimer.Elapsed += new System.Timers.ElapsedEventHandler(DoJob);
            guidVisitTimer.Enabled = true;

            templateNoticeMsgTimer = new Timer();
            templateNoticeMsgTimer.Interval = 3610000;
            templateNoticeMsgTimer.Elapsed += new System.Timers.ElapsedEventHandler(SendNotice);
            templateNoticeMsgTimer.Enabled = true;

            templateRemindMsgTimer = new Timer();
            templateRemindMsgTimer.Interval = 60000;
            templateRemindMsgTimer.Elapsed += new System.Timers.ElapsedEventHandler(SendRemind);
            templateRemindMsgTimer.Enabled = true;

        }

        protected override void OnStop()
        {
            LoggerHelper.WriteLogInfo("------------------");
            LoggerHelper.WriteLogInfo("SeverClosed");
            LoggerHelper.WriteLogInfo("------------------");
            guidVisitTimer.Enabled = false;
            guidVisitTimer.Close();
            guidVisitTimer.Dispose();

            templateNoticeMsgTimer.Enabled = false;
            templateNoticeMsgTimer.Close();
            templateNoticeMsgTimer.Dispose();

            templateRemindMsgTimer.Enabled = false;
            templateRemindMsgTimer.Close();
            templateRemindMsgTimer.Dispose();

        }

        /// <summary>
        ///  访问数据
        /// </summary>
        private void DoJob(object sender, System.Timers.ElapsedEventArgs e)
        {
            var nowDate = DateTime.Now;
            Job job = new Job();
            //job.GuidVisit();
            //job.FkLibMeetings();
            //job.PersonInfo();
            if (nowDate.Hour == 1)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("同步认为临床指南浏览记录任务已开始！！！");
                LoggerHelper.WriteLogInfo("------------------");
                job.GuidVisit();
            }
            if (nowDate.Hour==2)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("同步认为费卡文库的科室会任务已开始！！！");
                LoggerHelper.WriteLogInfo("------------------");
                job.FkLibMeetings();
            }
            if (nowDate.Day == 1 && nowDate.Hour == 1)
            {
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo("同步人员认证信息到费卡文库任务已开始！！！");
                LoggerHelper.WriteLogInfo("------------------");
                job.PersonInfo();
            }

        }

        /// <summary>
        /// 准备模板数据 每一小时判断一次
        /// 每天3:00
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void SendNotice(object sender, System.Timers.ElapsedEventArgs e)
        {
            var nowDate = DateTime.Now;
          
            if (nowDate.Hour == 3)
            {
                try
                {
                    templateJob.SendTime=  DateTime.Parse(DateTime.Now.ToShortDateString() + " 17:30:00");
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Start]SendNotice");
                    LoggerHelper.WriteLogInfo("------------------");
                    templateJob.InvalidMsg();
                    templateJob.SendNoRegMsg(1);
                    templateJob.SendNoRegMsg(3);
                    templateJob.SendNoRegMsg(7);
                    templateJob.SendNoRegDataMsg();
                    templateJob.SendRegMsg();
                    templateJob.SendRejectRegMsg();
                    templateJob.SendRegDataMsg();
                    templateJob.SendHolidayMsg();
                    templateJob.SendStatMsg();
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Error]SendNotice");
                    LoggerHelper.WriteLogInfo(ex.ToString());
                    LoggerHelper.WriteLogInfo("------------------");

                }
            }
            if (nowDate.Hour==17)
            {
                try
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Start]SendMsgJob");
                    LoggerHelper.WriteLogInfo("------------------");
                    templateJob.SendMsgJob();
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Error]SendMsgJob");
                    LoggerHelper.WriteLogInfo(ex.ToString());
                    LoggerHelper.WriteLogInfo("------------------");
                }
            }            
        }
        
        /// <summary>
        /// 发送提醒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendRemind(object sender, System.Timers.ElapsedEventArgs e)
        {
            var nowDate = DateTime.Now;
            if (nowDate.Hour != 17 || nowDate.Hour != 3)
            {
                try
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Start]SendRemind");
                    LoggerHelper.WriteLogInfo("------------------");
                    templateJob.SendRemindMsg();
                    templateJob.SendQAMsg();
                    templateJob.SendMsgJob(false);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteLogInfo("------------------");
                    LoggerHelper.WriteLogInfo("[Error]SendRemind");
                    LoggerHelper.WriteLogInfo(ex.ToString());
                    LoggerHelper.WriteLogInfo("------------------");
                }
            }
        }
    }
}
