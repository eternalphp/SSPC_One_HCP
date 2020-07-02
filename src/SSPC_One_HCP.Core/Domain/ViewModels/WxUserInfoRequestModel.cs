using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class WxUserInfoRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string encryptedData { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string iv { set; get; }

        /// <summary>
        /// 医生来源AppId
        /// </summary>
        public string SourceAppId { get; set; }

        /// <summary>
        /// 医生来源类型
        /// 0. 其它场景 
        /// 1. 会议二维码 
        /// 2. 名片二维码 
        /// 3. 第三方公众号二维码 
        /// 4. 推广二维码
        /// 5. 全国大会
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 微信场景ID
        /// </summary>
        public string WxSceneId { get; set; }

        /// <summary>
        /// 会议ID
        /// </summary>
        public string ActivityID { get; set; }
    }
}
