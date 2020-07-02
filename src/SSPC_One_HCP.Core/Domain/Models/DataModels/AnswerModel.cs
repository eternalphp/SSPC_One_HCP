using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 问卷答案
    /// </summary>
    [DataContract]
    public class AnswerModel:BaseEntity
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        [DataMember]
        public string QuestionId { get; set; }
        
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
        /// 是否是正确答案
        /// </summary>
        [DataMember]
        public bool? IsRight { get; set; }
        
    }
}
