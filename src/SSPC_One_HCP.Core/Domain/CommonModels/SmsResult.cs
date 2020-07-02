using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 发送验证码返回
    /// </summary>
    public class SmsResult
    {
        public bool ResultFlag { get; set; }
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string BizId { get; set; }
    }
}
