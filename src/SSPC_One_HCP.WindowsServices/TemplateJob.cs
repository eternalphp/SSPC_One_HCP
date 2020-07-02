using Newtonsoft.Json;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplateJob
    {
        public string MinAppID { get; set; }
        //private static string minAppID = ConfigurationManager.AppSettings["MinAppID"];
        public string MinAppSecret { get; set; }
        //private static string minAppSecret = ConfigurationManager.AppSettings["MinAppSecret"];
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        private static double expires_in = 7200D;

        /// <summary>
        /// 审核通过提醒
        /// 审核结果{{keyword1.DATA}}
        /// 时间{{keyword2.DATA}}
        /// </summary>
        public string SHCG { get; set; }
        /// <summary>
        /// 审核未通过提醒
        /// 审核结果{{keyword1.DATA}}
        /// 时间{{keyword2.DATA}}
        /// </summary>
        public string ZCSB { get; set; }

        /// <summary>
        /// 任务接收通知
        /// 任务内容{{keyword1.DATA}}
        /// 发布时间{{keyword2.DATA}}
        /// </summary>      
        public string RWTX { get; set; }

        public DateTime SendTime { get; set; }

        public TemplateJob()
        {
            SendTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 17:30:00");
            MinAppID = ConfigurationManager.AppSettings["MinAppID"];
            MinAppSecret = ConfigurationManager.AppSettings["MinAppSecret"];
            SHCG  = ConfigurationManager.AppSettings["SHCG"];
            ZCSB = ConfigurationManager.AppSettings["ZCSB"];
            RWTX= ConfigurationManager.AppSettings["RWTX"];
            AccessToken = "";
            Expires = DateTime.Now.AddDays(-1); 
            //minAppID = "wxeebd12df09bcecc4";
            //minAppSecret = "956ebc34ebf6943b3da76ebf55b05365";
        }

        /// <summary>
        /// 获取微信token
        /// https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        /// </summary>
        private void GetAccessToken()
        {
            //accessToken = "25_pF9QK4Cacf09M6v5G8mxQu1bKkwg0Fnd2Q1B-J6kgAWVVQPXVTXuuRPEAmQ16Q3hu8yD1ETKKEo0UhN87c2kc2rFoQ3FDENevZ9ZrW8N6gIIBsd1s4DlOuYfeanm7-gFR9v9vPq7SQjWM8oUUEReAIAJTG";
            //expires = DateTime.Now;
            //AccessToken 过期 
            if (Expires.AddSeconds(expires_in) < DateTime.Now || string.IsNullOrEmpty(AccessToken))
            {
              
                var url = "https://" + $"api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={MinAppID}&secret={MinAppSecret}";
                Console.WriteLine("url:" + url);
                string data = HttpService.Get(url);
                var result = JsonConvert.DeserializeObject<WxPackage.Token>(data);
                if (!string.IsNullOrEmpty(result?.access_token))
                {
                    AccessToken = result.access_token;
                    Expires = DateTime.Now;
                }
                Console.WriteLine("Token:" + AccessToken);
            }
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        private void SendTemplate(TemplateForm form, string val,string template)
        {
            
            GetAccessToken();

            var url = "https://" + $"api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={AccessToken}";
            LoggerHelper.WriteLogInfo("------------------");
            LoggerHelper.WriteLogInfo($"sendTemplate({form.Id}, {val}, {template})");
            LoggerHelper.WriteLogInfo($"Url={url}");
            LoggerHelper.WriteLogInfo("------------------");
            var data = new WxPackage.Send();
            data.access_token = AccessToken;
            data.form_id = form.FormID;
            data.template_id = template;
            data.touser = form.OpenID;
            data.page = form.Page;
            data.data = new WxPackage.SendData() { keyword1 = new WxPackage.SendValue() { value = val }, keyword2 = new WxPackage.SendValue() { value = DateTime.Now.ToString() } };
            var sendData = JsonConvert.SerializeObject(data);
            string result = HttpService.Post(sendData, url, false, 10);
            Console.WriteLine("result:" + result);
            var sql = "update TemplateForm set IsDeleted=1 ,UpdateTime=GETDATE(),Remark=Remark+@Result where id=@ID";
            DapperHelper<int>.Execute(sql, new { ID = form.Id, Result = result });
            if (result.Contains("ok"))
            {
               
            }
        }

        /// <summary>
        /// 释放无效分配
        /// 未注册消息 ：注册了的用户取消分配
        /// </summary>
        /// <param name="userid"></param>
        private void CancelSendNoRegMsg(string userid)
        {
            var sqlup = "update TemplateForm set MsgID=0, IsEnabled=0 , SendTime=null, Remark=Remark+'-释放' where IsDeleted=0 and IsEnabled=1  and  CreateUser=@CreateUser and MsgID in (1,3,7) ";
            DapperHelper<int>.Execute(sqlup, new { CreateUser = userid });
        }

        /// <summary>
        /// 删除所有过期的 FormID
        /// </summary>
        public void InvalidMsg()
        {
            //删除所有过期的了
            var sqldel = "update TemplateForm set  IsDeleted=1 , Remark=Remark+'---已过期---'   where  DATEDIFF(DD,CreateTime ,@SendTime)>=7";
            DapperHelper<int>.Execute(sqldel, new { SendTime = SendTime });
        }

        /// <summary>
        /// 未注册消息提醒
        /// 查找所有 注册时间 ？？ 的用户 发送消息
        /// </summary>
        /// <param name="createDay">时间差 1 -3 -7</param>
        public void SendNoRegMsg(int createDay)
        {
            //获取N天前的 未注册 未删除 未分配任务人员
            var sql = "select Id from DoctorModel where IsDeleted!=1 and IsCompleteRegister=0 and  DATEDIFF(DD,CreateTime ,GETDATE())=@CreateDay and Id not in (select distinct CreateUser from TemplateForm where MsgID=@CreateDay )";
            var list = DapperHelper<string>.Query(sql, new { CreateDay = createDay });
            //formid 分配
            foreach (var item in list)
            {
                var sqlup = "update top (1) TemplateForm set MsgID=@CreateDay, IsEnabled=1 , SendTime=@SendTime , Remark='提醒注册-授权后第'+convert(varchar,@CreateDay)+'天' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and  dateadd(DD,7,CreateTime)>@SendTime";
                DapperHelper<DoctorModel>.Execute(sqlup, new { CreateDay = createDay, SendTime = SendTime, CreateUser = item });
            }
        }

        /// <summary>
        /// 注册成功消息
        /// </summary>
        public void SendRegMsg()
        {
            //查找注册成功 云势认证 未发送过模板的用户
            var sql = "select Id from DoctorModel where IsDeleted!=1 and IsCompleteRegister=1 and IsVerify=1 and Id not in (select distinct CreateUser from TemplateForm where MsgID=4 )";
            var list = DapperHelper<string>.Query(sql, null);
            foreach (var item in list)
            {
                CancelSendNoRegMsg(item);
                //第二天发送
                var sqlup = "update top (1) TemplateForm set MsgID=4, IsEnabled=1 , SendTime=@SendTime , Remark='审核成功提醒' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@SendTime";
                DapperHelper<DoctorModel>.Execute(sqlup, new { SendTime = SendTime, CreateUser = item });
            }
        }

        /// <summary>
        /// 认证失败模板消息
        /// </summary>
        public void SendRejectRegMsg()
        {
               //查找注册成功 云势认证 未发送过模板的用户
               var sql = "select Id from DoctorModel where IsDeleted!=1 and IsCompleteRegister=1 and IsVerify=3 and Id not in (select distinct CreateUser from TemplateForm where MsgID=5 )";
            var list = DapperHelper<string>.Query(sql, null);
            foreach (var item in list)
            {
                CancelSendNoRegMsg(item);
                var sqlup = "update top (1) TemplateForm set MsgID=5, IsEnabled=1 , SendTime=@SendTime, Remark='审核失败提醒' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@SendTime";
                DapperHelper<DoctorModel>.Execute(sqlup, new { SendTime = SendTime, CreateUser = item });
            }
        }

        /// <summary>
        /// 未注册用户 内容上线后第一天
        /// </summary>
        public void SendNoRegDataMsg()
        {
            var sql2 = " select * from DataInfo where IsDeleted!=1 and IsCompleted=1 and DATEDIFF(DD,CreateTime ,GETDATE())=1";
            var list2 = DapperHelper<DataInfo>.Query(sql2, null);

            var sqlDoctor = "select * from DoctorModel where IsDeleted!=1 and IsCompleteRegister=0";
            var listDoctor = DapperHelper<DoctorModel>.Query(sqlDoctor, null);

            var sqlup = "update top (1) TemplateForm set MsgID=2, IsEnabled=1 , SendTime=@SendTime , Remark=@Remark+'上线啦！快进入小程序解锁课程。' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@SendTime";
            foreach (var item in listDoctor)
            {
                foreach (var item2 in list2)
                {
                    DapperHelper<DoctorModel>.Execute(sqlup, new { CreateUser = item.Id, SendTime = SendTime, Remark = item2.Title });
                }
            }
        }

        /// <summary>
        /// 内容上新的第7天，未点击的用户
        /// </summary>
        public void SendRegDataMsg()
        {            
            var dataInfoSQL = " select * from DataInfo where IsDeleted!=1 and IsCompleted=1 and DATEDIFF(DD,CreateTime ,GETDATE())=7";
            var dataInfoList = DapperHelper<DataInfo>.Query(dataInfoSQL, null);

            var doctorSQL = "select id from DoctorModel where IsDeleted!=1 and IsCompleteRegister=1";
            var doctotList = DapperHelper<string>.Query(doctorSQL, null);

            foreach (var item in dataInfoList)
            {
                //已浏览用户ID
                var sql = @"  select DoctorModel.Id from DoctorModel 
  join MyLRecord on DoctorModel.Id = MyLRecord.WxUserId
  join DataInfo on MyLRecord.LObjectId = DataInfo.Id
  where MyLRecord.LObjectType in (2, 3, 4) and DataInfo.Id = @DataID and DoctorModel.IsCompleteRegister = 1 and DoctorModel.IsDeleted != 1";
                var recordList = DapperHelper<string>.Query(sql, new { DataID = item.Id });
                //没有浏览用户
                doctotList = doctotList.Where(x => !recordList.Contains(x)).ToList();

                foreach (var item2 in doctotList)
                {
                    var sqlup = "update top (1) TemplateForm set MsgID=6,IsEnabled=1,SendTime=@Sendtime,Remark=@Remark+'上线好几天了，你还没有查看哦，快进入小程序解锁课程吧。' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@Sendtime";
                    DapperHelper<DoctorModel>.Execute(sqlup, new { CreateUser = item2, Sendtime = SendTime, Remark = item.Title });
                }
            }

        }

        /// <summary>
        /// 发送假日消息
        /// 执行一次 只有时间匹配才会继续
        /// </summary>
        public void SendHolidayMsg()
        {          
            var timeConfig = ConfigurationManager.AppSettings["Holiday"];
            var remark = ConfigurationManager.AppSettings["HolidayMsg"];
            //var remark = "恭喜发财";
            var time1 = DateTime.Now.AddDays(1);
            var time2 = DateTime.Now;
            DateTime.TryParse(timeConfig, out time1);

            if ((time1 - time2).Days == 0)
            {
                var doctorSQL = "select id from DoctorModel where IsDeleted!=1 and IsCompleteRegister=1";
                var doctotList = DapperHelper<string>.Query(doctorSQL, null);
                foreach (var item in doctotList)
                {
                    var sqlup = "update top (1) TemplateForm set MsgID=8, IsEnabled=1 , SendTime=@Sendtime, Remark=@Remark where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@Sendtime";
                    DapperHelper<DoctorModel>.Execute(sqlup, new { CreateUser = item, Sendtime = SendTime, Remark = remark });
                }
            }

        }

        /// <summary>
        /// 统计消息
        /// 每天执行一次 只有周五才会继续
        /// </summary>
        public void SendStatMsg(bool sendnow=false)
        {
            
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday || sendnow)
            {         
                
                var sql = @"select 
   ROW_NUMBER() over(order by SUM(LObjectDate) desc) as LV,WxUserId 
 from MyLRecord where datediff(week, CreateTime, GETDATE()) = 1 and LObjectType in (2, 3, 4)
  group by WxUserId";
                var list = DapperHelper<RecordLV>.Query(sql, null);
               
                var reamke = "『业精于勤，荒于嬉』您上周学习时间{0}小时,参加会议{1}次，比{2}%用户还要努力。";
                foreach (var item in list)
                {
                    var sql2 = @"  select COUNT(*) from MyMeetOrder  where datediff(week, CreateTime, getdate()) = 1 and CreateUser=@CreateUser ";
                    var orderCount = DapperHelper<int>.QueryFirst(sql2, new { CreateUser = item.WxUserId });
                    var sqlup = "update top (1) TemplateForm set MsgID=9, IsEnabled=1 , SendTime=@Sendtime , Remark=@Remark where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@Sendtime";
                    DapperHelper<DoctorModel>.Execute(sqlup, new { CreateUser = item.WxUserId, Sendtime = SendTime, Remark = string.Format(reamke, Math.Round(item.LV / 60.0, 0), orderCount, Math.Round((100.0 * item.LV / list.Count), 0)) });
                }
            }
        }

        /// <summary>
        /// 会议提醒
        /// </summary>
        public void SendRemindMsg()
        {
            //设置提醒的报名人员
            var sql = @"select * from MyMeetOrder  where IsRemind=1 and  RemindTime is not null  and DATEDIFF(mi,RemindTime,GETDATE())=0";
            var list = DapperHelper<MyMeetOrder>.Query(sql, null);

            foreach (var item in list)
            {
                var sqlup = "update top (1) TemplateForm set MsgID=10, IsEnabled=1 , SendTime=@RemindTime , Remark=@MeetId+'会议提醒' where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@RemindTime  and (select COUNT(*) from TemplateForm where CreateUser=@CreateUser and  MsgID=10 and   Remark=@MeetId+'会议提醒')=0";
                DapperHelper<int>.Execute(sqlup, new { CreateUser = item.CreateUser, RemindTime = item.RemindTime , MeetId = item.MeetId});
            }
        }

        /// <summary>
        /// 会议问卷
        /// </summary>
        public void SendQAMsg()
        {
            //问卷会议ID
            var meetQASQL = "select distinct MeetId from MeetQAModel where IsDeleted!=1";
            var list = DapperHelper<string>.Query(meetQASQL, null);
            var meetOrderSQL = "select * from MyMeetOrder where MeetId in @MeetIds and IsDeleted!=1 and DATEDIFF(mi,RemindTime,GETDATE())=0";
            var meetOrders = DapperHelper<MyMeetOrder>.Query(meetOrderSQL, new { MeetIds = list });
            foreach (var item2 in meetOrders)
            {
                var sqlup = "update top (1) TemplateForm set MsgID=11, IsEnabled=1 , SendTime=@RemindTime , Remark=@MeetID where IsDeleted=0 and IsEnabled=0  and  CreateUser=@CreateUser and dateadd(DD,7,CreateTime)>@RemindTime and (select COUNT(*) from TemplateForm where CreateUser=@CreateUser and  MsgID=11 and   Remark=@MeetId)=0";
                DapperHelper<int>.Execute(sqlup, new { CreateUser = item2.CreateUser, RemindTime = item2.RemindTime, MeetID = item2.MeetId });
            }

        }


        /// <summary>
        /// 发送任务 
        /// 今天需要发送的
        /// </summary>
        /// <param name="isNow">是否当天发送</param>
        public void SendMsgJob(bool isNow = true)
        {
            var sql = string.Empty;
            if (isNow)
            {
                //获取今天 需要在 17:00发送的数据
                sql = "select * from TemplateForm where  IsDeleted=0 and IsEnabled=1 and   DATEDIFF(DD,SendTime ,GETDATE())=0 and  isnull(OpenID,'')!='' and MsgID not in (10,11)";
            }
            else
            {
                //获取一分钟后需要发送的数据
                sql = "select * from TemplateForm where  IsDeleted=0 and IsEnabled=1 and   DATEDIFF(mi,SendTime ,@NowDate)=0 and  isnull(OpenID,'')<>'' and MsgID in (10,11)";
            }
            var list = DapperHelper<TemplateForm>.Query(sql, new { NowDate=DateTime.Now });
            LoggerHelper.WriteLogInfo("------------------");
            LoggerHelper.WriteLogInfo(JsonConvert.SerializeObject(list));
            foreach (var item in list)
            {
                var msg = string.Empty;
                var template = string.Empty;
                item.Page = "/page/tabBar/discover/index";
                switch (item.MsgID)
                {
                    case 1:
                        template = RWTX;
                        msg = "想知道您关注领域的最新临床指南和学术会议吗？您想随时查约2万份药品说明书、适应症、相互作用吗？您想要国内外含CSI收录的共4700种期刊影响力查询吗？多福医生在这通通帮您搞定~只需一分钟注册，就可以开启您专属的多福医生之旅啦~";
                        break;
                    case 3:
                        template = RWTX;
                        msg = "多福医生是谁？多福医生是费森尤斯卡比全新打造的一个贴心懂您，为您省时省力的一站式医生服务平台。请快快来体验我吧。";
                        break;
                    case 7:
                        template = RWTX;
                        msg = "众里寻你千百度，蓦然回首，你却依然不在灯火阑珊处；在离别之际，可不可以最后再进来我看看你？";
                        break;
                    case 4:
                        template = SHCG;
                        msg = @"这是一份新手指南，请亲启；
用药助手：随时查询约2万份用药说明书、适应症、相互作用。
会议资讯：设置会议提醒让您不落下任何重要学术会议。
临床指南：指南文献的检索和20 + 领域每周更新的最新资料；
期刊影响力查询：国内外含CSI收录的共4700种期刊助，您临床学术两不误。
多福还会有更多的福利惊喜哟~
在等什么呢？您的多福新旅程已经开始啦！";
                        break;
                    case 5:
                        template = ZCSB;
                        msg = "遗憾地通知您，您注册资料提交不成功。马上进入小程序“我的”，提交申诉资料，我们将尽快为您处理。";
                        break;

                    case 10:
                        template = RWTX;
                        msg = "您关注的会议马上开始了，不要错过咯";
                        break;
                    case 11:
                        template = RWTX;
                        item.Page = "/page/tabBar/discover/index?share=1&type=meet&id=" + item.Remark;
                        msg = "会议调研";
                        break;

                    default:
                        template = RWTX;
                        msg = item.Remark;
                        break;
                }
                LoggerHelper.WriteLogInfo("------------------");
                LoggerHelper.WriteLogInfo(item.Id);
                LoggerHelper.WriteLogInfo("------------------");
                SendTemplate(item, msg, template);
            }
        }

        /// <summary>
        /// 立刻发送消息
        /// </summary>
        public void Send20191118()
        {
            var sql = $@"select  
                        b.Id,
                        b.FormID,
                        b.MsgID,
                        b.OpenID,
                        b.CreateTime from EdaCheckInRecord as A
                        join TemplateForm  as B on a.OpenId=b.OpenID
                        join DoctorModel as C on a.OpenId=c.OpenId
                        where a.ActivityID='22b3a353-93f1-46d4-abd8-5e4b1ea14e30'
                         and b.IsDeleted=0 and b.IsEnabled=0  and DateDiff(dd,b.CreateTime,getdate())<=7
                        ";
           //and c.IsCompleteRegister = 1 and c.IsDeleted = 0 and c.UserName like '%顾翠%'"
            var list = DapperHelper<TemplateForm>.Query(sql, null);
            var grouplist = list.GroupBy(x => x.OpenID);

            foreach (var item in grouplist)
            {
                try
                {
                    var form = item.FirstOrDefault();
                    if (form != null)
                    {
                        form.Page = "page/meet/pages/meetQuestion/meetQuestion?id=22b3a353-93f1-46d4-abd8-5e4b1ea14e30";
                        form.MsgID = 20191118;
                        form.Remark = "您参与的【2019年CKD营养与代谢峰会高峰论坛】已经完美谢幕，感谢您的关注！快来与我们谈谈您对CKD营养与代谢的见解吧…";
                        var upSql = "update  TemplateForm set MsgID=@MsgID, IsEnabled=1 , SendTime=@RemindTime , Remark=@Remark, Page=@Page where IsDeleted=0 and IsEnabled=0  and Id=@Id";
                        DapperHelper<int>.Execute(upSql, new { Id = form.Id, MsgID = form.MsgID, RemindTime = DateTime.Parse("2019-11-18 17:30:00"), Remark = form.Remark, Page = form.Page });
                        SendTemplate(form, form.Remark, RWTX);
                    }
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine(ex.ToString());
                    LoggerHelper.WriteLogInfo("--20191118--");
                    LoggerHelper.WriteLogInfo(JsonConvert.SerializeObject(item));
                }
                
            }
        }

    }
    public class RecordLV
    {
        public int LV { get; set; }
        public string WxUserId { get; set; }
    }

}
