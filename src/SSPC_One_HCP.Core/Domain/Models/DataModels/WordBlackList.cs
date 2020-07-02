using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 用于过滤用户输入的词语黑名单
    /// </summary>
    [DataContract]
    public class WordBlackList : BaseEntity
    {
        /// <summary>
        /// 禁止使用的词组（用,分隔）
        /// </summary>
        [DataMember]
        public string Words { get; set; }

        /// <summary>
        /// 用途类型
        /// doctor_name: 医生姓名
        /// </summary>
        [DataMember]
        public string Type { get; set; }
    }
}
