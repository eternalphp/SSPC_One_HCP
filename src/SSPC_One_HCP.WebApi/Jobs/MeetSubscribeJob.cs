using Newtonsoft.Json;
using Quartz;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.WebApi.Jobs
{
    public class MeetSubscribeJob : IJob
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        private static double expires_in = 7200D;
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => RemindMeetSubscribe());
            }
            catch (Exception e)
            {
                JobExecutionException e2 = new JobExecutionException(e);
                // this job will refire immediately
                e2.RefireImmediately = true;
                throw e2;
            }
        }
        private void GetAccessToken()
        {
            if (Expires.AddSeconds(expires_in) < DateTime.UtcNow.AddHours(8) || string.IsNullOrEmpty(AccessToken))
            {
                var _config = ContainerManager.Resolve<IConfig>();
                var appId = _config.GetAppIdHcp();
                var appSecret = _config.GetAppSecretHcp();
                //https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
                var url = $"{"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="}{appId}&secret={appSecret}";
                string data = HttpService.Get(url);
                var result = JsonConvert.DeserializeObject<WxToken>(data);

                if (!string.IsNullOrEmpty(result?.access_token))
                {
                    AccessToken = result.access_token;
                    Expires = DateTime.Now;
                }
            }
        }
        private void RemindMeetSubscribe()
        {
            try
            {
                GetAccessToken();
                var url = $"{"https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token="}{AccessToken}";
                var dt = DateTime.UtcNow.AddHours(8);
                var rep = ContainerManager.Resolve<IEfRepository>();
                Send obj = new Send
                {
                    miniprogram_state = "trial",
                    lang = "zh_CN"
                };

                var meetSubscribes = rep.Where<MeetSubscribe>(o => o.IsDeleted != 1 && o.HasReminded != 1).ToList();
                foreach (var item in meetSubscribes)
                {
                    var meetInfo = rep.FirstOrDefault<MeetInfo>(o => o.Id == item.MeetId);
                    var mt = meetInfo?.MeetStartTime.Value.AddMinutes(item.RemindOffsetMinutes);
                    var sd = mt <= dt;
                    if (sd)
                    {
                        var templateIds = item.TemplateId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < templateIds.Length; i++)
                        {
                            obj.template_id = templateIds[i];// "aWDAFcu-zLFhUdA8m2TeYsUWhVz9v0zZLLlvlByb0qM";
                            obj.page = $"page/meet/pages/liveMeetDetail/liveMeetDetail?id={item.MeetId}";//点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
                            obj.touser = item.OpenId;
                            obj.data = new SendData()
                            {
                                //phrase1 = new SendValue() { value = meetInfo.Id },
                                //thing2 = new SendValue() { value = meetInfo.MeetTitle },
                                //thing3 = new SendValue() { value = $"马上就要开始了" },
                                //date5 = new SendValue() { value = DateTime.UtcNow.AddHours(8).ToString() }

                                thing1 = new SendValue() { value = meetInfo.MeetTitle },
                                time2 = new SendValue() { value = meetInfo?.MeetStartTime.ToString() }
                            };

                            var sendData = JsonConvert.SerializeObject(obj);
                            string result = HttpService.Post(sendData, url, false, 10);


                            var sendResult = JsonConvert.DeserializeObject<SendResult>(result);
                            item.ReminderResults = result;
                            if (sendResult.errcode == "0")
                            {
                                item.HasReminded = 1; //已发送提醒短信
                            }
                            item.UpdateTime = DateTime.UtcNow.AddHours(8);
                            rep.Update(item);
                            rep.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception e)
            {

                LoggerHelper.Error("------------------");
                LoggerHelper.Error($"RemindMeetSubscribe:");
                LoggerHelper.Error($"Msg={e.Message}");
                LoggerHelper.Error("------------------");
            }
        }
    }

    public class Send
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 所需下发的订阅模板id
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>
        public string miniprogram_state { get; set; }
        /// <summary>
        /// zh_CN
        /// </summary>
        public string lang { get; set; }
        /// <summary>
        /// zh_CN
        /// </summary>
        public string page { get; set; }

        public SendData data { get; set; }
    }

    public class SendData
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public SendValue thing1 { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public SendValue time2 { get; set; }
        ///// <summary>
        ///// 内容
        ///// </summary>
        //public SendValue thing3 { get; set; }
        ///// <summary>
        ///// 时间
        ///// </summary>
        //public SendValue date5 { get; set; }
    }

    public class SendValue
    {
        public string value { get; set; }
    }
    public class WxToken
    {

        public string access_token { get; set; }
        public int expires_in { get; set; }

    }

    public class SendResult
    {

        public string errcode { get; set; }
        public string errmsg { get; set; }

    }
}