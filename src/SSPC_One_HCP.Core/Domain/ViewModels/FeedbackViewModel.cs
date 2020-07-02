using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 意见反馈模型
    /// </summary>
    public class FeedbackViewModel
    {
        /// <summary>
        /// 意见内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
