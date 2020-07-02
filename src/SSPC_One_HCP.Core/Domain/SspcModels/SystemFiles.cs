using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.SspcModels
{
    /// <summary>
    /// 上传文件后返回的数据
    /// </summary>
    public class SystemFiles
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件全名称
        /// </summary>
        public string FileLongName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int? FileSize { get; set; }
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 记录是否删除
        /// </summary>
        public int? IsDelete { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 文件FileId
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 文件压缩状态: 0-未压缩 1-压缩中 2-压缩完成
        /// </summary>
        public int? FileCompressStatus { get; set; }
        /// <summary>
        /// 文件归档目录
        /// </summary>
        public string ArchiveFileName { get; set; }
        /// <summary>
        /// 文件文档时间
        /// </summary>
        public DateTime? ArchiveDateTime { get; set; }
        /// <summary>
        /// 文件解压时间
        /// </summary>
        public DateTime? DecompressDateTime { get; set; }
        /// <summary>
        /// 转成成Pdf的Id
        /// </summary>
        public string PdfFileId { get; set; }
        /// <summary>
        /// 略缩图路径
        /// </summary>
        public string CoverSavePath { get; set; }
        /// <summary>
        /// 略缩图Id
        /// </summary>
        public string CoverFileId { get; set; }
    }
}
