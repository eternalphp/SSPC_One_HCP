using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.SspcModels
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUploadParams
    {
        /// <summary>
        /// 文件数据byte
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectNo { get; set; }
        /// <summary>
        /// 保存类型
        /// </summary>
        public string SaveType { get; set; }
    }
}
