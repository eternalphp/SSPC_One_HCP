using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 推广二维码
    /// </summary>
    [DataContract]
    public class SpreadQRCode : BaseEntity
    {
        /// <summary>
        /// APPID
        /// </summary>
        [DataMember]
        public string SpreadAppId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string SpreadName { get; set; }
        /// <summary>
        /// 推广类型
        /// 1:多福医生 2:其他
        /// </summary>
        [DataMember]
        public int SpreadQRType { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        [DataMember]
        public string SpreadQRCodeUrl { get; set; }
        /// <summary>
        /// 注册数
        /// </summary>
        [DataMember]
        public int RegisteredCount { get; set; }
        /// <summary>
        /// 访问数
        /// </summary>
        [DataMember]
        public int VisitorsCount { get; set; }
    }
}
