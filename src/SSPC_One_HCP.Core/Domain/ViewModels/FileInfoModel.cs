using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 普通文件模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class FileInfoModel
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string url { get; set; }
    }

    /// <summary>
    /// 媒体文件模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class MediaInfoModel
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string url { get; set; }

        /// <summary>
        /// 文件时长
        /// </summary>
        [DataMember]
        public string filetime { get; set; }
    }
}
