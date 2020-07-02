using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    [DataContract]
    public class Feedback : BaseEntity
    {
        /// <summary>
        /// 意见内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
