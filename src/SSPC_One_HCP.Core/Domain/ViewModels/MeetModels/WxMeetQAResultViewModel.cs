using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 提交会议问卷结果视图（微信小程序）
    /// </summary>
    [NotMapped]
    [DataContract]
    public class WxMeetQAResultViewModel
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 会议问卷类型
        /// 1.会前问卷
        /// 2.会后问卷
        /// </summary>
        [DataMember]
        public int? QAType { get; set; }
        
        /// <summary>
        /// 参会人员（报名人员）Id
        /// </summary>
        [DataMember]
        public string SignUpUserId { get; set; }

        /// <summary>
        /// 用户选择的答案
        /// </summary>
        [DataMember]
        public IEnumerable<WxMeetQAViewModel> Questions { get; set; }
    }
}
