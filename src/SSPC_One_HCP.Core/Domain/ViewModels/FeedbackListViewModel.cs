using System;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 意见反馈列表模型
    /// </summary>
    public class FeedbackListViewModel
    {
        /// <summary>
        /// 意见反馈编号
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 意见内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DataMember]
        public string CreateUser { get; set; }
    }
}
