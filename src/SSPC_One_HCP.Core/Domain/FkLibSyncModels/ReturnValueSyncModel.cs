using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.FkLibSyncModels
{
    /// <summary>
    /// 同步返回消息Model
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ReturnValueSyncModel
    {
        /// <summary>
        /// 状态（1、成功 -1、失败）
        /// </summary>
        [DataMember]
        public string status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string message { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [DataMember]
        public string result { get; set; }
    }
}
