using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels
{
    /// <summary>
    /// 医生标签视图模型
    /// </summary>
    public class DoctorTagView
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        [DataMember]
        public string DocId { get; set; }

        /// <summary>
        /// 标签名称数组
        /// </summary>
        [DataMember]
        public string[] Tags { get; set; }
    }
}
