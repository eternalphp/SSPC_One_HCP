using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 资料下载信息
    /// </summary>
    public class HcpDownloadInfo : BaseEntity
    {
        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public string Sender { get; set; }
        /// <summary>
        /// 下载人
        /// </summary>
        [DataMember]
        public string DownloadPeople { get; set; }
        /// <summary>
        /// 资料库ID
        /// </summary>
        [DataMember]
        public string HcpDataInfoId { get; set; }
        /// <summary>
        /// 下载文件
        /// </summary>
        [DataMember]
        public string DownloadFileName { get; set; }
    }
}
