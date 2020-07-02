using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 问卷题目模型
    /// </summary>
    [DataContract]
    public class QuestionModel:BaseEntity
    {
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
        /// 问题的正确答案
        /// </summary>
        [DataMember]
        public string QuestionOfA { get; set; }

        /// <summary>
        /// 会议Id （为空表示题库题）
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
    }
}
