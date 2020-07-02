using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot.Dto
{
    public class ContentCreateDto
    {
        /// <summary>
        /// 业务单位
        /// </summary>    
        public string BusinessUnitId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>    
        public string Title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>    
        public int Sort { get; set; }
        /// <summary>
        /// 简介
        /// </summary>    
        public string BriefIntroduction { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>    
        public int ContentType { get; set; }
        /// <summary>
        /// 文档类型
        /// </summary>    
        public int DocumentType { get; set; }
        /// <summary>
        /// 文件来源
        /// </summary>    
        public int FileSourc { get; set; }
        /// <summary>
        /// 本地内容
        /// </summary>    
        public string LocalContent { get; set; }
    }
    public class ContentUploadInputDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] InputStream { get; set; }
    }
}
