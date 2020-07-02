using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议调研结果
    /// </summary>
    [DataContract]
    public class MeetQAResult : BaseEntity
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 问卷Id
        /// </summary>
        [DataMember]
        public string MeetQAId { get; set; }

        /// <summary>
        /// 参会人员（报名人员）Id
        /// </summary>
        [DataMember]
        public string SignUpUserId { get; set; }

        /// <summary>
        /// 参会人员选择的答案Id（用于单选题和多选题）
        /// </summary>
        [DataMember]
        public string UserAnswerId { get; set; }

        /// <summary>
        /// 参会人员填写的答案（用于填空题）
        /// </summary>
        [DataMember]
        public string UserAnswer { get; set; }

    }
}
