using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SSPC_One_HCP.Services.Utils
{
    public class MailUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// {0} 审批人中文名，{1} 单号 ，{2} 申请人中文名
        /// {3} 审批人英文名, {4} 申请人英文名
        /// </remarks>
        public static string SysUrl { get; } = ConfigurationManager.AppSettings["HostUrl"];

        public static string Notice { get; } = "<!DOCTYPE html>" +
                                      "<html lang=\"en\">" +
                                      "<head>" +
                                      "<meta charset=\"UTF-8\">" +
                                      "<title></title>" +
                                      "<style>" +
                                      "* {{margin: 0;padding: 0;}} " +
                                      "body{{font-family:\"微软雅黑\";font-size:14px;padding:0px 20px;line-height:24px;}} " +
                                      ".title{{text-align:center;padding:20px 0px 10px;color:#f00;font-size:12px;}} " +
                                      ".content{{margin-bottom:20px;}} " +
                                      ".border{{border-bottom:3px double #000;margin:20px 0px;}}" +
                                      "</style> " +
                                      "</head> " +
                                      "<body> " +
                                      " <div class=\"footer\"> " +
                                      "<p>管理员您好,</p> " +
                                      "<div class=\"content\"> " +
                                      //"<p>会议《{0}》待审核，请及时登录系统操作！</p> " +
                                      "<p>由{0}创建的{1}《{2}》待审核，请点击以下链接<a>{3}</a>,登录系统操作</p> " +
                                      "</div> " +
                                      "<p>此邮件为系统自动发送，请勿回复</p ></div> " +

                                      "</body> " +
                                      "</html>";

        /// <summary>
        /// 通过服务发送邮件
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="reciverName"></param>
        /// <param name="attachments"></param>
        public static bool SendMail(string body, string subject, string reciverName, List<Attachment> attachments = null)
        {
            LoggerHelper.WarnInTimeTest("[SendMail Start]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            bool isSendSuccess = true;
            string host = ConfigurationManager.AppSettings["MailServer"];
            string address = ConfigurationManager.AppSettings["MailAddress"];
            string userName = ConfigurationManager.AppSettings["MailFromUserName"];
            string password = ConfigurationManager.AppSettings["MailFromPassword"];
            string testMail = ConfigurationManager.AppSettings["MailTo"];
            ILog logger = LogManager.GetLogger("ErrorFileLogger");
            try
            {
                MailAddress from = new MailAddress(address, userName);
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = from;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;
                mailMessage.Body = body;
                var sendMails = reciverName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!string.IsNullOrEmpty(testMail))
                {
                    sendMails = testMail.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                for (int i = 0; i < sendMails.Count; i++)
                {
                    mailMessage.To.Add(sendMails[i]);
                }
                mailMessage.Subject = subject;
                if (attachments != null && attachments.Any())
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(item);
                    }
                }
                smtpClient.Host = host;
                smtpClient.UseDefaultCredentials = true;

                smtpClient.Credentials = new NetworkCredential(userName, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //object mail = mailMessage;
                //smtpClient.SendAsync(mailMessage, mail); //改为异步发送
                smtpClient.Send(mailMessage);
              
                LoggerHelper.WarnInTimeTest("[SendMail End]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            catch (Exception e)
            {
                isSendSuccess = false;
             
                LoggerHelper.WarnInTimeTest("[SendMail]:邮件发送失败  原因:" + e.Message);
            }
            return isSendSuccess;
        }

        /// <summary>
        /// 会议,产品资料，播客的   新增 修改 删除时 待审核邮件发送
        /// 改成异步 解决网络延迟的问题
        /// </summary>
        /// <param name="meetTitle"></param>
        /// <returns></returns>
        public static async Task<bool> SendMeetMail(string sendname, string sendtype, string meetTitle)
        {
            return await Task.Run(() =>
            {
                var subject = ConfigurationManager.AppSettings["MailSubject"];
                var body = string.Format(Notice, sendname, sendtype, meetTitle, SysUrl);

                SendMail(body, subject, "");
                return true;
            });
        }
    }
}
