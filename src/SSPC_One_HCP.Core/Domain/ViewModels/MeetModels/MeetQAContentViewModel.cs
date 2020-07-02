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
    /// 会议问卷视图
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MeetQAContentViewModel
    {
        /// <summary>
        /// 问题
        /// </summary>
        [DataMember]
        public QuestionModel Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [DataMember]
        public IEnumerable<AnswerModel> Answers { get; set; }
       
    }
}
