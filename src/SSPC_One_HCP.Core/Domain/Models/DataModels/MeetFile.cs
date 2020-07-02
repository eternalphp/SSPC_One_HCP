using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议资料
    /// </summary>
    [DataContract]
    public class MeetFile : BaseEntity
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public int FileSize { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [DataMember]
        public string FileType { get; set; }
        /// <summary>
        /// 是否有版权，关系到是否可以下载
        /// 2.没有版权
        /// 1.有版权
        /// </summary>
        [DataMember]
        public int? IsCopyRight { get; set; }
    }
}
