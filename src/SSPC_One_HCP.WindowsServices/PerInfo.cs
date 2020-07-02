using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices
{
    public class PerInfoItem
    {
        /// <summary>
        /// 主键ID
        /// </summary>
         public string OneHCPID { get; set; }

        /// <summary>
        /// 云势ID
        /// </summary>
        public string YSID { get; set; }

        /// <summary>
        /// OneHCP理由
        /// </summary>
        public string OneHCPReason { get; set; }

        /// <summary>
        /// OneHCP验证结果
        /// </summary>
        public string OneHCPState { get; set; }
    }
    public class PerInfoReturn
    {
        /// <summary>
        /// 状态（1、成功 -1、失败）
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; }
    }
}
