using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSPC_One_HCP.Core.Domain.CommonModels;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class SmsUtil
    {
       
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="sendSms"></param>
        /// <returns></returns>
        public static SmsResult SendMessage(SendSmsModel sendSms)
        {
            var url = ConfigurationManager.AppSettings["smsUrl"];
            var postData = JsonConvert.SerializeObject(sendSms);
            return HttpUtils.PostResponse<SmsResult>(url, postData);
        }
    }
}
