using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Dml;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.GuidModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxDownLoadService : IwxDownLoadService
    {
        private readonly IEfRepository _rep;

        public static string SysUrl = ConfigurationManager.AppSettings["HostUrl"];

        public static string GuidUrl = ConfigurationManager.AppSettings["GuidUrl"];

        public static string GuidProjectId = ConfigurationManager.AppSettings["GuidProjectId"];

        public WxDownLoadService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 解密并下载
        /// </summary>
        /// <param name="urlContent"></param>
        /// <returns></returns>
        public ReturnValueModel GetDownLoadDecryptUrl(string urlContent)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Result = new
            {
                rows = new
                {
                    urlContent = EncryptHelper.AES_Decrypt(urlContent )
                }
            };
            return rvm;
        }

        /// <summary>
        /// 回传加密数据
        /// </summary>
        /// <param name="urlContent"></param>
        /// <returns></returns>
        public ReturnValueModel GetDownLoadEncryptUrl(string urlContent)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string RetUrlContent = string.Empty;
            if (!string.IsNullOrEmpty(urlContent)&& urlContent.IndexOf('/')>-1)
            {
                string[] ContentArr = urlContent.Split('/');
              
                for (int i = 3; i < ContentArr.Length; i++)
                {
                    RetUrlContent += ContentArr[i] + "/";
                } 
                RetUrlContent = urlContent.TrimEnd('/');
               
            }

            rvm.Result = new
            {
                rows = new
                {
                    urlContent = SysUrl+"/"+ EncryptHelper.AES_Encrypt(RetUrlContent )
                }
            };
            return rvm;
        }


        public ReturnValueModel GetDecode(string urlContent)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string aa= EncryptHelper.GuidDecode(urlContent);
            rvm.Result = aa;
            return rvm;
        }
        /// <summary>
        /// 临床指南接口测试
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GuidTest()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string BegoreDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            long startTime = ConvertDateTimeToInt(Convert.ToDateTime(BegoreDate + " 00:00:00"));
            long endTime= ConvertDateTimeToInt(Convert.ToDateTime(BegoreDate + " 23:59:59"));
            string guidurl = GuidUrl+ "projectId="+ GuidProjectId+ "&startTime=1555653009128&endTime=1555903191541&start=1&num=50";
            //获取临床指南返回的数据 
            string guidback=  HttpService.Get(guidurl); 
            guidback = HttpUtility.UrlDecode(guidback);

            string KeyWord = "JstoBn29LpomgYk9";
            string iv = guidback.Substring(0, 16);

            string guiddata = EncryptHelper.DecodeAES(guidback, KeyWord, iv);
            guiddata = guiddata.Substring(guiddata.IndexOf('{'), guiddata.LastIndexOf('}')- guiddata.IndexOf('{')+1);
            GuidResult model = JsonConvert.DeserializeObject<GuidResult>(guiddata);
            rvm.Success = false;
            if (model.success)
            {
                rvm.Success = true;
                foreach (GuidItem guidItem in model.data.items)
                {
                    
                    string abc = guidItem.guideName;
                    _rep.Insert(new GuidVisit()
                    {
                        Id=Guid.NewGuid().ToString(),
                        userid = guidItem.uid,
                        ActionType=guidItem.actionType,
                        GuideId = guidItem.guideId,
                        GuideName = guidItem.guideName,
                        GuideType = guidItem.guideType,
                        CreateTime = DateTime.Now.AddDays(-1),
                        UpdateTime = DateTime.Now,
                        IsEnabled = 0,
                        IsDeleted = 0
                    });
                    _rep.SaveChanges();
                }

            }
            return rvm;
        }

         
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        
       
    }
}
