using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 短信发送
    /// </summary>
    public class SendSmsModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumbers { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 模板编号
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 所属系统
        /// </summary>
        public string SystemId { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// json字符串的base64编码
        /// </summary>
        /// <remarks>
        /// { "code":"123456" }
        /// </remarks>
        public string ParamName { get; set; }
    }
}
