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
    /// 会议问卷候选答案视图（微信小程序）
    /// </summary>
    [NotMapped]
    [DataContract]
    public class WxAnswerViewModel
    {
        /// <summary>
        /// 答案编号
        /// </summary>
        [DataMember]
        public string AnswerId { get; set; }

        /// <summary>
        /// 答案内容
        /// </summary>
        [DataMember]
        public string AnswerContent { get; set; }

        /// <summary>
        /// 答案的位置
        /// </summary>
        [DataMember]
        public string Sort { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember]
        public bool? Selected { get; set; }
    }
}
