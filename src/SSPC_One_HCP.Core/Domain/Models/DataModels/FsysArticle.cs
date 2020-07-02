using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 临床指南精选文章
    /// </summary>
    public class FsysArticle : BaseEntity
    {
        /// <summary>
        /// 科室编号
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [DataMember]
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章链接
        /// </summary>
        [DataMember]
        public string ArticleUrl { get; set; }

        /// <summary>
        /// 文章排序
        /// </summary>
        [DataMember]
        public int ArticleSort { get; set; }
        /// <summary>
        /// 是否精选(0不精选，1精选)
        /// </summary>
        [DataMember]
        public int ArticleIsHot { get; set; }
        /// <summary>
        /// 科室id集合
        /// </summary>
        [DataMember]
        public string[] DeptList { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        [DataMember]
        public string ArticleSource { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DataMember]
        public DateTime? PublishedDate { get; set; }
    }
}
