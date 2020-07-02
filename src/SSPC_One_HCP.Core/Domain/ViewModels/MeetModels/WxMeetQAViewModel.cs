using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议问卷视图（微信小程序）
    /// </summary>
    [NotMapped]
    [DataContract]
    public class WxMeetQAViewModel
    {
        /// <summary>
        /// 关系表Id
        /// </summary>
        [DataMember]
        public string MeetQAId { get; set; }

        /// <summary>
        /// 问题Id
        /// </summary>
        [DataMember]
        public string QuestionId { get; set; }

        /// <summary>
        /// 题目类型
        /// 1.单选题
        /// 2.多选题
        /// 3.填空题
        /// </summary>
        [DataMember]
        public int? QuestionType { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        [DataMember]
        public string QuestionContent { get; set; }

        /// <summary>
        /// 候选答案
        /// </summary>
        [DataMember]
        public IEnumerable<WxAnswerViewModel> Answers { get; set; }
    }
}
