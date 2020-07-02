using SSPC_One_HCP.Data.Mappings.DataMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsServices;

namespace SSPC_One_HCP.Test
{
    class Program
    {
        static void Main(string[] args)
        {


            Msg();


        }

        private static void Msg()
        {
            Console.WriteLine("╔═══════════════╗");
            Console.WriteLine("║请输入需要执行的指令编号      ║");
            Console.WriteLine("║1、删除所有过期的FormID       ║");
            Console.WriteLine("║2、[未注册]授权后第一天       ║");
            Console.WriteLine("║3、[未注册]授权后第三天       ║");
            Console.WriteLine("║4、[未注册]授权后第七天       ║");
            Console.WriteLine("║5、[未注册]内容上线后一天     ║");
            Console.WriteLine("║6、[已注册]认证通过消息       ║");
            Console.WriteLine("║7、[已注册]认证失败消息       ║");
            Console.WriteLine("║8、[已注册]新内容上新七天未点 ║");
            Console.WriteLine("║9、[已注册]节假日消息         ║");
            Console.WriteLine("║10、[已注册]统计信息周五才有效║");
            Console.WriteLine("║11、发送消息                  ║");
            Console.WriteLine("║****、代码                  ║");
            Console.WriteLine("║       ════════       ║");
            Console.WriteLine("║12、会议提醒设置提醒才有效    ║");
            Console.WriteLine("║13、[已注册]统计信息周五才有效║");
            Console.WriteLine("╚═══════════════╝");

            //分配任务
            TemplateJob templateJob = new TemplateJob();
            Console.WriteLine("APPID:" + templateJob.MinAppID);
            Console.WriteLine("AppSecret:" + templateJob.MinAppSecret);
            Console.WriteLine("AppSecret:" + templateJob.SHCG);
            Console.WriteLine(DateTime.Now.Hour);

            Console.Write("请输入你要执行的操作：");
            Console.ReadKey();
            var key = Console.ReadLine();
            while (!key.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                switch (key)
                {
                    case "1":
                        templateJob.InvalidMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "2":
                        templateJob.SendNoRegMsg(1);
                        Console.WriteLine("执行完毕！");
                        break;
                    case "3":
                        templateJob.SendNoRegMsg(3);
                        Console.WriteLine("执行完毕！");
                        break;
                    case "4":
                        templateJob.SendNoRegMsg(7);
                        Console.WriteLine("执行完毕！");
                        break;
                    case "5":
                        templateJob.SendNoRegDataMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "6":
                        templateJob.SendRegMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "7":
                        templateJob.SendRejectRegMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "8":
                        templateJob.SendRegDataMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "9":
                        templateJob.SendHolidayMsg();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "10":
                        templateJob.SendStatMsg(true);
                        Console.WriteLine("执行完毕！");
                        break;
                    case "11":
                        templateJob.SendMsgJob();
                        Console.WriteLine("执行完毕！");
                        break;
                    case "12":
                        templateJob.SendRemindMsg();
                        templateJob.SendMsgJob(false);
                        Console.WriteLine("执行完毕！");
                        break;
                    case "13":
                        templateJob.SendQAMsg();
                        templateJob.SendMsgJob(false);
                        Console.WriteLine("执行完毕！");
                        break;

                    case "20191118":
                        templateJob.Send20191118();                      
                        Console.WriteLine("Success！");
                        break;
                    default:
                        Console.WriteLine("请输入正确指令");
                        key = Console.ReadLine();
                        break;
                }
                Console.Write("请输入你要执行的操作：");
                key = Console.ReadLine();
            }
            Console.WriteLine("感谢使用 任意键退出");
            Console.ReadKey();
        }
    
    }
    
}
