using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 资料操作信息
    /// </summary>
    public class HcpDataOperationInfo : BaseEntity
    {
        /// <summary>
        /// 点击用户ID
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 点击内容
        /// </summary>
        [DataMember]
        public string ClickContent { get; set; }
        /// <summary>
        /// 动作内容
        /// 点击
        /// 预览
        /// 下载
        /// 转发
        /// </summary>
        [DataMember]
        public string ActionContent { get; set; }
        
    }
}
